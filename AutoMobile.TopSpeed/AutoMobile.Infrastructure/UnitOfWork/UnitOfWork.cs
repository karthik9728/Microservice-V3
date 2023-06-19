using AutoMobile.Application.Contracts.Persistence;
using AutoMobile.Infrastructure.Common;
using AutoMobile.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _dbContext;
        private IPrincipal _principal;
        private IDecodeAccessToken _decodeAccessToken;

        public UnitOfWork(ApplicationDbContext dbContext, IPrincipal principal, IDecodeAccessToken decodeAccessToken)
        {
            _dbContext = dbContext;
            _principal = principal;
            _decodeAccessToken = decodeAccessToken;
            Brand = new BrandRepository(_dbContext);
        }

        //public IVehicleTypeRepository VehicleType { get; private set; }

        public IBrandRepository Brand { get; private set; }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task SaveAsync()
        {
            _dbContext.SaveCommonFields(_principal,_decodeAccessToken);
            await _dbContext.SaveChangesAsync();
        }
    }
}
