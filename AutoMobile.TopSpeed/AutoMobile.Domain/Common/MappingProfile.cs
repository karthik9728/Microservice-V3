using AutoMapper;
using AutoMobile.Domain.DTO.Vehicle;
using AutoMobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Common
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {

            CreateMap<Vehicle, VehicleDto>().ReverseMap();
            CreateMap<Vehicle, VehicleCreateDto>().ReverseMap();
            CreateMap<Vehicle, VehicleUpdateDto>().ReverseMap();
            CreateMap<Vehicle, VehicleDetailsDto>().ReverseMap();
        }
    }
}
