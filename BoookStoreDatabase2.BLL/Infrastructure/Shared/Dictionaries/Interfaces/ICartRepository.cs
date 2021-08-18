using BoookStoreDatabase2.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface ICartRepository
    {
        Task<List<OrderLineDTO>> GetCustomerOrder(int? customerId, CartStatus cartStatus);
        Task<bool> AddToCart(AddToCartCommand cartCommand);
        Task<bool> UpdateCustomerOrderLine(int customerId, CartStatus currentStatus, CartStatus newStatus);

    }
}
