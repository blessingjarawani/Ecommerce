using BoookStoreDatabase2.DAL.Context;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Models.DTO;
using Ecommerce.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<CustomerOrderSummaryDTO>> GetOrderHistory(int customerId)
        {
            var result = await Task.Run(() => _db.CustomerOrders.Where(t => t.CustomerId == customerId).Include(x => x.Product)
                         .AsEnumerable().GroupBy(t => new { t.OrderNumber, t.DateCreated.Date })
                         .Select(t => new CustomerOrderSummaryDTO
                         {
                             OrderNumber = t.Key.OrderNumber,
                             OrderDate = t.Key.Date,
                             TotalAmount = t.Sum(x => x.Amount),
                             Products = t.Select(x => new OrderProducts
                             {
                                 Amount = x.Amount,
                                 Name = x.Product.Name,
                                 Quantity = x.Quantity
                             }).ToList()
                         }));
            return result;
        }
    }
}
