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
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }

        public async Task Update(Brand brand)
        {
            var objFromDb = await _dbContext.Brand.FirstOrDefaultAsync(x => x.Id == brand.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = brand.Name;
                objFromDb.EstablishedYear = brand.EstablishedYear;
            }

            _dbContext.Update(objFromDb);
        }
    }
}
