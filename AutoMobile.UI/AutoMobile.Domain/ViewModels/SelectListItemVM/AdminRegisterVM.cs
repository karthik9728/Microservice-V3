using AutoMobile.Domain.DTO.User;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.ViewModels.SelectListItemVM
{
    public class AdminRegisterVM
    {
        public AdminRegisterationRequestDto RegisterationRequestDto { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}
