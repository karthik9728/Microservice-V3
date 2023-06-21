using AutoMobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Contracts.Persistence
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task Update(Vehicle vehicle);
    }
}
