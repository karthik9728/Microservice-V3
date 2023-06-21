using AutoMobile.Application.Contracts.Persistence;
using AutoMobile.Domain.Models;
using AutoMobile.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Repository
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task Update(Vehicle vehicle)
        {
            var objFromDb = await _dbContext.Vehicle.FirstOrDefaultAsync(x => x.Id == vehicle.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = vehicle.Name;
                objFromDb.Brand = vehicle.Brand;
                objFromDb.VehicleType = vehicle.VehicleType;
                objFromDb.EngineAndFuelType = vehicle.EngineAndFuelType;
                objFromDb.Transmission = vehicle.Transmission;
                objFromDb.Engine = vehicle.Engine;
                objFromDb.TopSpeed = vehicle.TopSpeed;
                objFromDb.Mileage = vehicle.Mileage;
                objFromDb.Range = vehicle.Range;
                objFromDb.SeatingCapacity = vehicle.SeatingCapacity;
                objFromDb.PriceFrom = vehicle.PriceFrom;
                objFromDb.PriceTo = vehicle.PriceTo;
                objFromDb.Ratings = vehicle.Ratings;
            }

            _dbContext.Update(objFromDb);
        }
    }
}
