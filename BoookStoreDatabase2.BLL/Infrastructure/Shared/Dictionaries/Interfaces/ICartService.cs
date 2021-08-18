using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface ICartService
    {
        Task<ObjectResponse<bool>> AddToCart(AddToCartCommand addToCartCommand);
        Task<ObjectResponse<List<OrderLineDTO>>> GetCustomerCart(int customerId, CartStatus cartStatus);
        Task<BaseResponse> UpdateCustomerOrderLine(UpdateCustomerOrderLineCommand updateCustomerOrderLineCommand);


    }
}
