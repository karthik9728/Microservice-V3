using AutoMapper;
using AutoMobile.Domain.InputModel.Users;
using AutoMobile.Domain.Models;
using AutoMobile.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Common
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, RegisterInputModel>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserVM>().ReverseMap();
        }
    }
}
