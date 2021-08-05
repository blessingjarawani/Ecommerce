using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_.Net_Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : BaseController
    {
        public ICartService _cartService { get; }
        public IProductsService _productsService { get; }

        public CartController(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ICartService cartService, IProductsService productsService) : base(httpContextAccessor, userManager)
        {
            _cartService = cartService;
            _productsService = productsService;
        }

        [HttpPost("[action]")]
        public async Task<Response<bool>> AddToCart(int id)
        {

            var userId = await GetUserId();
            var product = await _productsService.GetProduct(id);
            if (!product.Success)
            {
            }
            var command = new AddToCartCommand
            {
                CustomerId = userId,
                Product = product.Data
            };
            return await _cartService.AddToCart(command);

        }
        [HttpPost("[action]")]
        public async Task<Response<List<OrderLineDTO>>> Details()
        {
            var userId = await GetUserId();
            return await _cartService.GetCustomerCart(userId);
        }
    }
}
