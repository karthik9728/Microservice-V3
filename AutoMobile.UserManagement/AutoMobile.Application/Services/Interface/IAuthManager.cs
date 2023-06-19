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
        Task<IEnumerable<IdentityError>> SignUp(RegisterInputModel registerInputModel);

        Task<object> SignIn(LoginInputModel loginInputModel);

        Task<AuthResponseVM> VerfiyRefreshToken(AuthResponseInputModel authResponseInputModel);

        Task<bool> IsUserExists(string emailId);
    }
}
