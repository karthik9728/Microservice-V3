using AutoMobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services
{
    public interface ITopSpeedService
    {
        void PlaceOrder(PlaceOrder placeOrder);
    }
}
