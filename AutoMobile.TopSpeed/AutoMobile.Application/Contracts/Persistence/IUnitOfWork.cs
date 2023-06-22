using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    { 

        public IVehicleRepository Vehicle { get; }

        Task SaveAsync();
    }
}
