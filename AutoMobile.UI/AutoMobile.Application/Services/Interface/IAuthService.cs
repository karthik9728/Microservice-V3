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

        Task<T> AdminRegisterAsync<T>(AdminRegisterationRequestDto registerationRequestDto);

        Task<T> GetRolesAsync<T>();

        Task<T> EmailConfirmationAsync<T>(string queryString);

        Task<T> ForgetPasswordAsync<T>(string emailId);

        Task<T> ResetPasswordAsync<T>(string queryString);
    }
}
