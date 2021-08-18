using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface ICustomerOrderRepository
    {
        Task<bool> AddOrder(int productId, int customerId, int qnty, decimal amount, string orderNumber);
    }
}
