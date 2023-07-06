using MicroRabbit.Domain.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Events
{
    public class OrderCreatedEvent : Event
    {
        public int ProductId { get; private set; }

        public int Quantity { get; private set; }

        public DateTime OrderPlacedOn { get; private set; }

        public OrderCreatedEvent(int productId, int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
            OrderPlacedOn = DateTime.Now;
        }
    }
}
