using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.WEB.Models.ViewModels;
using Ecommerce.BLL.Models.DTO;
using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.WEB.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        private IProductsService _productsService { get; }

        public ProductController(IWebHostEnvironment hostingEnvironment, IECommerceHttpClient httpClient) : base(httpClient)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Index(bool? price = null, bool? name = null)
        {
            try
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
                var response = await _client.PostAsync("http://localhost:45447/api/Product/GetProducts", new StringContent(JsonConvert.SerializeObject(new { price, name }), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductsDTO>>(content);
                return View(result);

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.GetBaseException().Message);
                return View();
            }


        }

        [HttpGet]
        public async Task<IActionResult> Books(bool? price = null, bool? name = null)
        {
            try
            {
                var productSearch = new ProductSearchDTO
                {
                    price = price,
                    name = name
                };
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
                var response = await _client.PostAsync($"Product/GetProducts", new StringContent(JsonConvert.SerializeObject(productSearch), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<ProductsDTO>>(content);

                return View(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.GetBaseException().Message);
                return View();
            }



        }
       
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
   
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
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


                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
                var response = await _client.PostAsync("Product/CreateOrUpdate", new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ObjectResponse<int>>(content);
                if (result.Success)
                {
                    return RedirectToAction("details", new { id = result.Data });
                }
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await _client.PostAsync($"Product/Details", new StringContent(JsonConvert.SerializeObject(new { id }), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ObjectResponse<ProductsDTO>>(content);
            if (result.Success)
            {
                return View(result.Data);
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await _client.PostAsync($"Product/GetProduct", new StringContent(JsonConvert.SerializeObject(new { id }), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ObjectResponse<ProductsDTO>>(content);
            if (result.Success)
            {
                var editProduct = new EditProductViewModel
                {
                    ExistingPhotoPath = result.Data.ImagePath,
                    Id = result.Data.Id,
                    Name = result.Data.Name,
                    ProductType = result.Data.ProductType,
                    Quantity = result.Data.Quantity,
                    Price = result.Data.Price
                };
                return View(editProduct);
            }
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
    }
}
