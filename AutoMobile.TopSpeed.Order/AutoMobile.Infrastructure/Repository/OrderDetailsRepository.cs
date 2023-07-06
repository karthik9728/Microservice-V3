using AutoMobile.Domain.Interface;
using AutoMobile.Domain.Models;
using AutoMobile.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMobile.Infrastructure.Repository
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public OrderDetailsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<OrderDetails> GetOrderDetails()
        {
            return _dbContext.OrderDetails;
        }

        public void Add(OrderDetails orderDetails)
        {
            _dbContext.OrderDetails.Add(orderDetails);
            _dbContext.SaveChanges();
        }
    }
}
