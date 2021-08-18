using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.WEB.Models.ViewModels;
using Ecommerce.BLL.Models.DTO;
using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.WEB.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : BaseController
    {
        public CartController(IECommerceHttpClient httpClient) : base(httpClient)
        {

        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            if (ModelState.IsValid && id > 0)
            {
                var userId = await GetUserId();
                var command = new AddToCartCommand
                {
                    CustomerId = int.Parse(userId),
                    ProductId = id,
                    Quantity = quantity

                };
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
                var response = await _client.PostAsync($"Cart/AddToCart", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<ObjectResponse<bool>>(content);
                if (!product.Success)
                {
                    ModelState.AddModelError(string.Empty, product.Message);
                    return View();
                }
                return RedirectToAction("Details", "Cart");

            }

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details()
        {
            var userId = await GetUserId();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await _client.PostAsync($"Cart/Details", new StringContent(JsonConvert.SerializeObject(new ProductSearchDTO { Id = int.Parse(userId) }), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ObjectResponse<List<OrderLineDTO>>>(content);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return View(result.Data);
        }
    }

}
