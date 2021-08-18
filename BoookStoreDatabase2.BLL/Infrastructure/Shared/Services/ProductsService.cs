using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Services
{
    public class ProductsService : IProductsService
    {
        private IProductsRepository _productsRepository { get; }

        private readonly ILogger<ProductsService> _logger;

        public ProductsService(IProductsRepository productsRepository, ILogger<ProductsService> logger)
        {
            _productsRepository = productsRepository;
            _logger = logger;
        }

        public async Task<ObjectResponse<List<ProductsDTO>>> GetProducts(string productType = null)
        {
            try
            {
                var result = await _productsRepository.GetProducts(productType);
                return new ObjectResponse<List<ProductsDTO>> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<List<ProductsDTO>> { Success = false, Message = ex.GetBaseException().Message };
            }

        }

        public async Task<ObjectResponse<ProductsDTO>> GetProduct(int id)
        {
            try
            {
                var result = await _productsRepository.GetProduct(id);
                return new ObjectResponse<ProductsDTO> { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<ProductsDTO> { Success = false, Message = ex.GetBaseException().Message };
            }

        }

        public async Task<ObjectResponse<int>> CreateOrUpdateProduct(ProductsDTO productsDTO)
        {
            try
            {
                if (productsDTO != null && productsDTO.IsValid)
                {
                    var result = await _productsRepository.AddOrUpdate(productsDTO);
                    return result > 0 ? new ObjectResponse<int> { Success = true, Data = result } : new ObjectResponse<int> { Success = false, Message = "Failed to Add Product" };
                }
                return new ObjectResponse<int> { Success = false, Message = "Invalid Product DTO" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
                return new ObjectResponse<int> { Success = false, Message = ex.GetBaseException().Message };
            }
        }

    }
}
