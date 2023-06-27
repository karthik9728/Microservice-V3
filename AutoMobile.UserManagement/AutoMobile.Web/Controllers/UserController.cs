using AutoMobile.Application.Services.Interface;
using AutoMobile.Domain.Common;
using AutoMobile.Domain.InputModel.UserManager;
using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.InputModel.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMobile.Domain.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace AutoMobile.Web.Controllers
{
    [Route("api/usermanagement/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IAuthManager _authManager;
        protected ApiResponse _response;

        public UserController(IAuthManager authManager)
        {
            _authManager = authManager;
            _response =  new ApiResponse();
        }


        [HttpPost]
        [Route("AdminSignUp")]
        [AllowAnonymous]
        public async Task<ApiResponse> AdminSignUp([FromBody] AdminRegisterInputModel user)
        {
            try
            {
                var errors = await _authManager.AdminSignUp(user);
                List<string> errorList = new List<string>();
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);

                        errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                        foreach (var item in errorList)
                        {
                            _response.AddError(item);
                        }
                    }
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = user;
                }
            }
            catch (Exception ex)
            {
                _response.AddError(ex.ToString());
            }

            return _response;
        }

        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public async Task<ApiResponse> SignUp([FromBody] RegisterInputModel user)
        {
            try
            {
                var errors = await _authManager.SignUp(user);
                List<string> errorList = new List<string>();
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);

                        errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                        foreach (var item in errorList)
                        {
                            _response.AddError(item);
                        }
                    }
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return _response;
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.Result = user;
                }
            }
            catch (Exception ex)
            {           
                _response.AddError(ex.ToString());
            }

            return _response;
        }


        [HttpPost]
        [Route("SignIn")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> SignIn([FromBody] LoginInputModel login)
        {
            try
            {
                if (!ModelState.IsValid) { return BadRequest(); }

                var isUserExists = await _authManager.IsUserExists(login.Email);

                if (isUserExists)
                {
                    var result = await _authManager.SignIn(login);

                    if (result is string)
                    {
                        _response.IsSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.AddError(result.ToString());
                    }
                    else if(result is AuthResponseVM)
                    {
                        _response.IsSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Result = result;
                    }

                   
                }
                else
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.AddError("Invalid Email Id");
                    return _response;
                }
            }
            catch (Exception ex)
            {           
                _response.AddError(ex.ToString());
            }

            return _response;
        }

        [HttpGet]
        [Route("Roles")]
        public async Task<ApiResponse> GetRoles()
        {
            try
            {
                var roles = await _authManager.GetRoles();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = roles;
            }
            catch (Exception ex)
            {
                _response.AddError(ex.ToString());
            }

            return _response;
        }

        [HttpPost]
        [Route("EmailConfirmation")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> EmailConfirmation([Required] string UserId, [Required] string Token)
        {
            try
            {
                var authResponse = await _authManager.EmailConfirmation(UserId, Token);
                if (!authResponse)
                {
                    _response.AddError("Email is Not Verified");
                }
                else
                {
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.OK;
                    _response.DisplayMessage = "Email is Successfully Verified";
                }
            }
            catch (Exception ex)
            {
                _response.AddError(ex.ToString());
            }

            return _response;
        }
    }
}
