﻿using AutoMapper;
using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.ApplicationEnums;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.DTO.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private string APIGatewayUrl;

        private readonly IConfiguration _configuration;

        public AuthService(IHttpClientFactory httpClientFactory,IHttpContextAccessor httpContextAccessor,IConfiguration configuration) : base(httpClientFactory,httpContextAccessor)
        {
            _configuration = configuration;
            APIGatewayUrl = _configuration["ServiceUrls:APIGateway"];
        }



        public Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApplicationEnum.ApiType.POST,
                Url = APIGatewayUrl + "/api/usermanagement/User/SignIn",
                Data = loginRequestDto
            });
        }

        public Task<T> RegisterAsync<T>(RegisterationRequestDto registerationRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApplicationEnum.ApiType.POST,
                Url = APIGatewayUrl + "/api/usermanagement/User/SignUp",
                Data = registerationRequestDto
            });
        }

        public Task<T> AdminRegisterAsync<T>(AdminRegisterationRequestDto registerationRequestDto)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApplicationEnum.ApiType.POST,
                Url = APIGatewayUrl + "/api/usermanagement/User/AdminSignUp",
                Data = registerationRequestDto
            });
        }

        public Task<T> GetRolesAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApplicationEnum.ApiType.GET,
                Url = APIGatewayUrl + "/api/usermanagement/User/Roles",
            });
        }

        public Task<T> EmailConfirmationAsync<T>(string queryString)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = ApplicationEnum.ApiType.POST,
                Url = APIGatewayUrl + "/api/usermanagement/User/EmailConfirmation" + queryString,
            });
        }
    }
}
