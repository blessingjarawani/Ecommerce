using AutoMapper;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.DAL.Context;
using BoookStoreDatabase2.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.DAL.Repositories
{
    public class CartRepository : BaseRepository, ICartRepository
    {
        public CartRepository(StoreContext storeDbContext, IMapper mapper) : base(storeDbContext, mapper)
        {

        }

        public async Task<List<OrderLineDTO>> GetCustomerOrder(int? customerId, CartStatus cartStatus)
        {
            var query = _dbContext.OrderLines.Include(x => x.Product)
             .Where(x => x.IsActive && x.CartStatus == cartStatus);
            if (customerId.HasValue)
            {
                query = query.Where(x => x.CustomerId == customerId.Value);
            }
            var result = (await query.ToListAsync())
             .GroupBy(x => new { x.CustomerId, x.ProductId })
                        .Select(fl => new OrderLineDTO
                        {
                            CustomerId = fl.Key.CustomerId,
                            Products = fl.Select(y =>
                            new ProductsDTO
                            {
                                Id = fl.Key.ProductId,
                                Quantity = fl.Sum(x => x.Quantity),
                                Name = y.Product.Name,
                                Price = fl.Sum(t => t.Price * t.Quantity),
                                ProductType = y.Product.ProductType
                            }).ToList()
                        });

            return result?.ToList();
        }



        public async Task<bool> AddToCart(AddToCartCommand cartCommand)
        {
            var orderLine = new OrderLines
            {
                IsActive = true,
                CustomerId = cartCommand.CustomerId,
                Quantity = cartCommand.Quantity,
                Price = cartCommand.Product.Price,
                CartStatus = CartStatus.InProgress,
                ProductId = cartCommand.Product.Id
            };
            await _dbContext.OrderLines.AddAsync(orderLine);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateCustomerOrderLine(int customerId, CartStatus currentStatus, CartStatus newStatus)
        {
            var result = await _dbContext.OrderLines.Where(t => t.CustomerId == customerId && t.CartStatus == currentStatus && t.IsActive).ToListAsync();
            if (result.Any())
            {
                result.ForEach(t =>
                {
                    t.CartStatus = newStatus;
                });

                return await SaveChangesAsync();
            }
            return false;

        }
        private int GenerateId() =>
          _dbContext.OrderLines.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;
    }
}
