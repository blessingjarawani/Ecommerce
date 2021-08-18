using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
   public interface IProductsService
    {
        Task<ObjectResponse<int>> CreateOrUpdateProduct(ProductsDTO productsDTO);
        Task<ObjectResponse<List<ProductsDTO>>> GetProducts(string productType = null);
        Task<ObjectResponse<ProductsDTO>> GetProduct(int id);
    }
}
