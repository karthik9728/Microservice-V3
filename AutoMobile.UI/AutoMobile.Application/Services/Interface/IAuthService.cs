using AutoMobile.Domain.DTO.User;
using AutoMobile.Domain.DTO.UserManager;
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

        Task<T> GetUsersAsync<T>();

        Task<T> GetUserByIdAsync<T>(string id);

        Task<T> ChangeUserRoleAsync<T>(string id,string role);

        Task<T> GetClaimsAsync<T>();

        Task<T> GetUserClaimsAsync<T>(string id);

        Task<T> AddOrRemoveUserClaimAsync<T>(AddOrRemoveClaimDto dto);

        Task<T> ValidateUserAsync<T>(string userId);
    }
}
