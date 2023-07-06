using AutoMobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Domain.Interface
{
    public interface IOrderDetailsRepository
    {
        IEnumerable<OrderDetails> GetOrderDetails();

        void Add(OrderDetails orderDetails);
    }
}
