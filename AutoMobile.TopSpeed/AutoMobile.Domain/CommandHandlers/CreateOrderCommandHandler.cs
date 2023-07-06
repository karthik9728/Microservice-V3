using AutoMobile.Domain.Commands;
using AutoMobile.Domain.Events;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.CommandHandlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IEventBus _eventBus;

        public CreateOrderCommandHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _eventBus.Publish(new OrderCreatedEvent(request.ProductId,request.Quantity));

            return Task.FromResult(true);
        }
    }
}
