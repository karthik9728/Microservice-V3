using AutoMobile.Domain.Events;
using AutoMobile.Domain.Interface;
using AutoMobile.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.EventHandlers
{
    public class OrderEventHandler : IEventHandler<OrderCreatedEvent>
    {
        private readonly IOrderDetailsRepository _orderDetailsRepository;

        public OrderEventHandler(IOrderDetailsRepository orderDetailsRepository)
        {
            _orderDetailsRepository = orderDetailsRepository;
        }

        public Task Handler(OrderCreatedEvent @event)
        {
            _orderDetailsRepository.Add(new OrderDetails()
            {
                ProductId = @event.ProductId,
                Quantity = @event.Quantity,
                OrderPlacedOn = @event.OrderPlacedOn,
            });

            return Task.CompletedTask;
        }
    }
}
