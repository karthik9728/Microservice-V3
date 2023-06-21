using AutoMobile.Domain.DTO.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services.Interface
{
    public interface IVehicleService
    {
        Task<T> GetAllAsync<T>(string token);

        Task<T> GetAsync<T>(int id, string token);

        Task<T> CreateAsync<T>(VehicleCreateDto dto, string token);

        Task<T> UpdateAsync<T>(VehicleUpdateDto dto, string token);

        Task<T> DeleteAsync<T>(int id, string token);
    }
}
