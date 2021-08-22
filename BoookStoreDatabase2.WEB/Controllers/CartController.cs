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
using ECommerce.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.WEB.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : BaseController
    {
        private readonly IConfiguration _configuration;
        public CartController(IECommerceHttpClient httpClient, IConfiguration configuration) : base(httpClient)
        {
            _configuration = configuration;
        }


        public async Task<IActionResult> AddToCart([FromBody] AddToCart cart)
        {
            if (ModelState.IsValid)
            {
                var userId = await GetUserId();
                var command = new AddToCartCommand
                {
                    CustomerId = int.Parse(userId),
                    ProductId =(cart.Id),
                    Quantity =(cart.Quantity)

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

        public async Task<IActionResult> CheckOut()
        {
            var userId = await GetUserId();
            var userEmail = await GetUserEmail();
            var command = new UpdateCustomerOrderLineCommand
            {
                CustomerId = int.Parse(userId),
                CurrentStatus = CartStatus.InProgress,
                NewStatus = CartStatus.InOrderingProcess,
                UserEmail = userEmail
            };
            _client.BaseAddress = new Uri(_configuration["Api:OrderUrl"]);
            var response = await _client.PostAsync($"Order/CheckOutCustomerOrderLine", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<BaseResponse>(content);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return RedirectToAction("Details");
        }

        [HttpGet]
        public async Task<IActionResult> CartHistory()
        {
            var userId = await GetUserId();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await _client.PostAsync($"Cart/GetPurchaseHistory", new StringContent(JsonConvert.SerializeObject(new GetCustomerOrderCommand { CustomerId = int.Parse(userId) }), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ObjectResponse<IEnumerable<CustomerOrderSummaryDTO>>> (content);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            return View(result.Data);
        }

    }

}
