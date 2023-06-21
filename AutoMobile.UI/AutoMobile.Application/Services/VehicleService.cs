using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.ApplicationEnums;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.Vehicle;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoMobile.Domain.ApplicationEnums.ApplicationEnum;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoMobile.Application.Services
{
    public class VehicleService : BaseService, IVehicleService
    {
        private string APIGatewayUrl;

        private readonly IConfiguration _configuration;

        public VehicleService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _configuration = configuration;
            APIGatewayUrl = _configuration["ServiceUrls:APIGateway"];
        }


        public Task<T> CreateAsync<T>(VehicleCreateDto dto, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.POST,
                Url = APIGatewayUrl + "/api/topspeed/Vehicle",
                Data = dto,
                Token = token
            });
        }

        public Task<T> DeleteAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.DELETE,
                Url = APIGatewayUrl + $"/api/topspeed/Vehicle?id={id}",
                Token = token
            });
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = APIGatewayUrl + "/api/topspeed/Vehicle",
                Token = token
            });
        }

        public Task<T> GetAsync<T>(int id, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.GET,
                Url = APIGatewayUrl + $"/api/topspeed/Vehicle/GetById?id={id}",
                Token = token
            });
        }

        public Task<T> UpdateAsync<T>(VehicleUpdateDto dto, string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = ApiType.PUT,
                Url = APIGatewayUrl + "/api/topspeed/Vehicle",
                Data = dto,
                Token = token
            });
        }
    }
}
