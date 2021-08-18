using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.DAL.Entities;
using Ecommerce.BLL.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace Ecommerce.Api.Controllers
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
        public async Task<ObjectResponse<bool>> AddToCart([FromBody] AddToCartCommand addToCartCommand)
        {

            var product = await _productsService.GetProduct(addToCartCommand.ProductId);
            if (!product.Success)
            {
            }
            var command = new AddToCartCommand
            {
                CustomerId = addToCartCommand.CustomerId,
                Product = product.Data,
                Quantity =addToCartCommand.Quantity
            };
            return await _cartService.AddToCart(command);

        }
        [HttpPost("[action]")]
        public async Task<ObjectResponse<List<OrderLineDTO>>> Details([FromBody] ProductSearchDTO productSearch)
        {
            return await _cartService.GetCustomerCart(productSearch.Id.Value,CartStatus.InProgress);
        }
    }
}
