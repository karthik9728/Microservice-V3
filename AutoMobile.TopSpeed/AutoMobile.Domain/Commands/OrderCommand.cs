using MicroRabbit.Domain.Core.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Commands
{
    public class OrderCommand : Command
    {
        public int ProductId { get; protected set; }

        public int Quantity { get; protected set; }

        public DateTime OrderPlacedOn { get; protected set; }
    }
}
