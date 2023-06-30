using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.InputModel.UserManager;
using AutoMobile.Domain.InputModel.Users;
using AutoMobile.Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> AdminSignUp(AdminRegisterInputModel registerInputModel);

        Task<IEnumerable<IdentityError>> SignUp(RegisterInputModel registerInputModel);

        Task<object> SignIn(LoginInputModel loginInputModel);

        Task<bool> EmailConfirmation(string userId, string token);

        Task<IEnumerable<IdentityError>> ChangePassword(ChangePasswordInputModel changePasswordInputModel);

        Task<bool> ForgetPassword(string emailId);

        Task<bool> ResetPassword(string userId, string token, string newPassword);


        Task<bool> IsUserExists(string emailId);

        Task<bool> IsUserExistsByUserId(string userId);

        Task<List<string>> GetRoles();

        Task<List<ApplicationUserVM>> GetUsers();

        Task<ApplicationUserVM> GetUserById(string id);

        Task<bool> ChangeUserRole(string id, string role);

        Task<bool> AddOrRemoveClaim(AddOrRemoveClaimInputModel addOrRemoveClaim);
    }
}
