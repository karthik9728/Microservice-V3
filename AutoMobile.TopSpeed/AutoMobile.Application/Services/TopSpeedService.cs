using AutoMobile.Domain.Commands;
using AutoMobile.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Application.Services
{
    public class TopSpeedService : ITopSpeedService
    {
        private readonly IEventBus _eventBus;

        public TopSpeedService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void PlaceOrder(PlaceOrder placeOrder)
        {
            var createOrderCommand = new CreateOrderCommand(placeOrder.ProductId, placeOrder.Quantity);

            _eventBus.SendCommand(createOrderCommand);
        }
    }
}
