using AutoMobile.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IBaseService
    {
        ApiResponse responseModel { get; set; }

        Task<T> SendAsync<T>(ApiRequest apiRequest);
    }
}
