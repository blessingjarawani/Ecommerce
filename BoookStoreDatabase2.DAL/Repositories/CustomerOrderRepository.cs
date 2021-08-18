using BoookStoreDatabase2.DAL.Context;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repositories
{
    public class CustomerOrderRepository : ICustomerOrderRepository
    {
        private readonly StoreContext _db;

        public CustomerOrderRepository(StoreContext db)
        {
            _db = db;
        }

        public async Task<bool> AddOrder(int productId, int customerId, int qnty, decimal amount, string orderNumber)
        {
            var order = new CustomerOrder
            {
                ProductId = productId,
                CustomerId = customerId,
                Quantity = qnty,
                Amount = amount,
                OrderNumber = orderNumber
            };
            _db.CustomerOrders.Add(order);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
