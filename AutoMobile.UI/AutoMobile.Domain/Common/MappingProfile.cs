using AutoMapper;
using AutoMobile.Domain.DTO.Vehicle;
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
            CreateMap<VehicleDetailsDto,VehicleUpdateDto>().ReverseMap();
        }
    }
}
