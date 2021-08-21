using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Models.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace Ecommerce.BLL.Infrastructure.Shared.Services
{
    public class CustomerOrderService : ICustomerOrderService
    {
        private readonly ICustomerOrderRepository _repo;
        private readonly ILogger<CustomerOrderService> _logger;
        private readonly ICartRepository _cartRepository;

        public CustomerOrderService(ICustomerOrderRepository repo, ILogger<CustomerOrderService> logger, ICartRepository cartRepository)
        {
            _repo = repo;
            _logger = logger;
            _cartRepository = cartRepository;
        }

        public async Task<BaseResponse> CreateOrder(CustomerOrderDTO customerOrder)
        {
            try
            {
                var result = await _repo.AddOrder(customerOrder.ProductId, customerOrder.CustomerId,
                                            customerOrder.Quantity, customerOrder.Amount, customerOrder.OrderNumber);
                return result ? new BaseResponse { Success = true }
                      : new BaseResponse { Success = false, Message = "DB Save Error" };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new BaseResponse { Success = false, Message = ex.GetBaseException().Message };
            }
        }
        public async Task<ObjectResponse<List<OrderLineDTO>>> GetOrdersToBeProcessed()
        {
            try
            {
                var result = await _cartRepository.GetCustomerOrder(customerId: null, CartStatus.InProgress);
                return new ObjectResponse<List<OrderLineDTO>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<List<OrderLineDTO>> { Success = false, Message = ex.GetBaseException().Message };
            }
        }

        public async Task<ObjectResponse<IEnumerable<CustomerOrderSummaryDTO>>> GetCustomerOrderHistory(int customerId)
        {
            try
            {
                var result = await _repo.GetOrderHistory(customerId);
                return new ObjectResponse<IEnumerable<CustomerOrderSummaryDTO>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<IEnumerable<CustomerOrderSummaryDTO>> { Success = false, Message = ex.GetBaseException().Message };
            }
        }
    }
}
