using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.ApplicationConstants;
using AutoMobile.Domain.ApplicationEnums;
using AutoMobile.Domain.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services
{
    public class BaseService : IBaseService
    {
        public ApiResponse responseModel { get; set ; }

        public IHttpClientFactory _httpClient { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public BaseService(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            responseModel = new ApiResponse();
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("APIGateway");

                HttpRequestMessage message = new HttpRequestMessage();

                //Header for API Call
                message.Headers.Add("Accept", "application/json");


                //API request URL
                message.RequestUri = new Uri(apiRequest.Url);

                //Check if any payload on request
                if(apiRequest.Data != null)
                {
                    message.Content =  new StringContent(JsonConvert
                        .SerializeObject(apiRequest.Data),Encoding.UTF8,"application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case ApplicationEnum.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case ApplicationEnum.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case ApplicationEnum.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                //send the token in header on request
                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",apiRequest.Token);
                }

                //making API call
                apiResponse = await client.SendAsync(message);

                //extract api content after receiving API response
                var apiContent = await apiResponse.Content.ReadAsStringAsync();

                try
                {
                    ApiResponse APIResponse = JsonConvert.DeserializeObject<ApiResponse>(apiContent);

                    if(apiResponse.StatusCode == HttpStatusCode.BadRequest || apiResponse.StatusCode == HttpStatusCode.NotFound) 
                    {
                        APIResponse.StatusCode = HttpStatusCode.BadRequest;
                        APIResponse.IsSuccess = false;

                        var res = JsonConvert.SerializeObject(APIResponse);

                        var returnObj = JsonConvert.DeserializeObject<T>(res);

                        return returnObj;
                    }
                    else if (apiResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        var context = _httpContextAccessor.HttpContext;

                        await _httpContextAccessor.HttpContext.SignOutAsync();

                        _httpContextAccessor.HttpContext.Session.SetString(ApplicationConstant.SessionToken, "");

                        context.Response.Redirect("/Auth/Login");
                    }
                    else if(apiResponse.StatusCode == HttpStatusCode.Forbidden)
                    {
                        var context = _httpContextAccessor.HttpContext;

                        context.Response.Redirect("/Auth/AccessDenied");
                    }
                    else if( apiResponse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        var context = _httpContextAccessor.HttpContext;

                        context.Response.Redirect("/Auth/InternalServerError");
                    }
                }
                catch (Exception)
                {

                    var execptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return execptionResponse;
                }

                var ApiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return ApiResponse;

            }
            catch (Exception ex)
            {
                List<ApiError> errors = new List<ApiError>()
                {
                    new ApiError
                    {
                        Description = Convert.ToString(ex.Message)
                    }
                };

                var dto = new ApiResponse
                {
                    Errors = errors,
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);

                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
           
        }
    }
}
