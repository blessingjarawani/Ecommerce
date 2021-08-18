using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Models.DTO;
using Ecommerce.BLL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private IProductsService _productsService { get; }

        public ProductController(IWebHostEnvironment hostingEnvironment, IProductsService productsService)
        {
            _hostingEnvironment = hostingEnvironment;
            _productsService = productsService;
        }


        [HttpPost("[action]")]
        public async Task<List<ProductsDTO>> GetProducts([FromBody] ProductSearchDTO productSearch = null)
        {

            var result = await _productsService.GetProducts();
            if (productSearch != null)
            {
                if (productSearch.price.HasValue && productSearch.price.Value == true)
                {
                    result.Data = result.Data?.OrderBy(x => x.Price).ToList();
                }
                if (productSearch.name.HasValue && productSearch.name.Value == true)
                {
                    result.Data = result.Data?.OrderBy(x => x.Name).ToList();
                }
            }
            return result.Data;
        }
        [HttpPost("[action]")]
        public async Task<ObjectResponse<int>> CreateOrUpdate([FromBody] ProductsDTO model)
        {

            return await _productsService.CreateOrUpdateProduct(model);

        }


        [HttpPost("[action]")]
        public async Task<ObjectResponse<ProductsDTO>> Details([FromBody]ProductSearchDTO dto) =>
        await _productsService.GetProduct(dto.Id.Value);

        [HttpPost("[action]")]

        public async Task<ObjectResponse<ProductsDTO>> GetProduct([FromBody]ProductSearchDTO dto) =>
        await _productsService.GetProduct(dto.Id.Value);

    }
}

