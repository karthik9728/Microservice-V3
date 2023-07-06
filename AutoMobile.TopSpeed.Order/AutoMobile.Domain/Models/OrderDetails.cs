using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }

        public int ProductId { get;  set; }

        public int Quantity { get;  set; }

        public DateTime OrderPlacedOn { get;  set; }
    }
}
