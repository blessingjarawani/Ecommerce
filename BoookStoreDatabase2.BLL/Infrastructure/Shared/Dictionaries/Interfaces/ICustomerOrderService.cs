using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using Ecommerce.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface ICustomerOrderService
    {
        Task<BaseResponse> CreateOrder(CustomerOrderDTO customerOrder);
    }
}
