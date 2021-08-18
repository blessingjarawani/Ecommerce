using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Models.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Services
{
    public class CartService : ICartService
    {
        private ICartRepository _cartRepository { get; }

        private readonly ILogger<CartService> _logger;

        public CartService(ICartRepository cartRepository, ILogger<CartService> logger)
        {
            _cartRepository = cartRepository;
            _logger = logger;
        }
        public async Task<ObjectResponse<bool>> AddToCart(AddToCartCommand addToCartCommand)
        {
            try
            {
                if (!addToCartCommand.IsValid)
                {
                    return new ObjectResponse<bool> { Success = false, Message = "Invalid Object" };
                }
                var result = await _cartRepository.AddToCart(addToCartCommand);
                return result ? new ObjectResponse<bool> { Success = true, Data = result } : new ObjectResponse<bool> { Success = false, Message = "Invalid Object" };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<bool> { Success = false, Message = ex.GetBaseException().Message };
            }

        }

        public async Task<ObjectResponse<List<OrderLineDTO>>> GetCustomerCart(int customerId, CartStatus cartStatus)
        {
            try
            {
                var result = await _cartRepository.GetCustomerOrder(customerId, cartStatus);
                return new ObjectResponse<List<OrderLineDTO>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<List<OrderLineDTO>> { Success = false, Message = ex.GetBaseException().Message };
            }
        }

        public async Task<BaseResponse> UpdateCustomerOrderLine(UpdateCustomerOrderLineCommand updateCustomerOrderLineCommand)
        {
            try
            {
                var result = await _cartRepository.UpdateCustomerOrderLine(updateCustomerOrderLineCommand.CustomerId, updateCustomerOrderLineCommand.CurrentStatus, updateCustomerOrderLineCommand.NewStatus);
                return result ? new ObjectResponse<bool> { Success = true, Data = result } : new ObjectResponse<bool> { Success = false, Message = "DB save Error" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<bool> { Success = false, Message = ex.GetBaseException().Message };
            }
        }
    }
}
