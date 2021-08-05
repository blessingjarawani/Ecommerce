using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_.Net_Core.Api.Controllers
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
        public async Task<List<ProductsDTO>> Index(bool? price = null, bool? name = null)
        {

            var result = await _productsService.GetProducts();
            if (price.HasValue && price.Value == true)
            {
                result.Data = result.Data?.OrderBy(x => x.Price).ToList();
            }
            if (name.HasValue && name.Value == true)
            {
                result.Data = result.Data?.OrderBy(x => x.Name).ToList();
            }
            return result.Data;
        }
        [HttpPost("[action]")]
        public async Task<Response<int>> Create([FromBody] CreateProductViewModel model)
        {

            string uniqueFileName = null;
            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                model.Image.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            var product = new ProductsDTO
            {
                Id = model.Id,
                Name = model.Name,
                ImagePath = uniqueFileName,
                ProductType = model.ProductType,
                Price = model.Price,
                Quantity = model.Quantity
            };

            return await _productsService.AddProduct(product);

        }


        [HttpPost("[action]")]
        public async Task<Response<ProductsDTO>> Details(int id) =>
        await _productsService.GetProduct(id);

        [HttpPost("[action]")]

        public async Task<Response<ProductsDTO>> Edit(int id) =>
        await _productsService.GetProduct(id);

    }
}

