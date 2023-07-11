using AutoMobile.Domain.Common;
using AutoMobile.Domain.InputModel;
using AutoMobile.Domain.Models;
using AutoMobile.Infrastructure.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static AutoMobile.Application.ApplicationConstants.ApplicationConstant;

namespace AutoMobile.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuReposiotry _menuReposiotry;
        protected ApiResponse _response;

        public MenuController(IMenuReposiotry menuReposiotry)
        {
            _menuReposiotry = menuReposiotry;
            this._response = new ApiResponse();
        }

        [HttpGet]
        public async Task<ApiResponse> GetMenuList()
        {
            try
            {
                var menus = await _menuReposiotry.GetMenuList();

                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;
                _response.Result = menus;

            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }

        [HttpPut]
        public async Task<ApiResponse> Update([FromBody] MenuUpdateInputModel menu)
        {
            try
            {
                await _menuReposiotry.UpdateMenu(menu);
                _response.StatusCode = HttpStatusCode.OK;
                _response.IsSuccess = true;

            }
            catch (Exception)
            {
                _response.AddError(CommonMessage.SystemError);
            }

            return _response;
        }
    }
}
