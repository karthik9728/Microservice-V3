using AutoMobile.Domain.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDto loginRequestDto);

        Task<T> RegisterAsync<T>(RegisterationRequestDto registerationRequestDto);
    }
}
