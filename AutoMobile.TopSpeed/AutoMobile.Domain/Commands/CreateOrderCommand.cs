using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Commands
{
    public class CreateOrderCommand : OrderCommand
    {
        public CreateOrderCommand(int productId,int quantity)
        {
            ProductId = productId;
            Quantity = quantity;
            OrderPlacedOn = DateTime.Now;
        }
    }
}
