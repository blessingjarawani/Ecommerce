using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Models.DTO;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceConsumer
{
    public class OrderConsumer : IConsumer<ObjectResponse<List<OrderLineDTO>>>
    {
        private readonly ICustomerOrderService _customerOrderService;

        public OrderConsumer(ICustomerOrderService customerOrderService)
        {
            _customerOrderService = customerOrderService;
        }

        public async Task Consume(ConsumeContext<ObjectResponse<List<OrderLineDTO>>> context)
        {
            var orders = context.Message;
            if (orders.Success)
            {
                if ((orders.Data?.Any()) ?? false)
                {
                    var orderNumber = Guid.NewGuid().ToString(); 
                    foreach (var order in orders.Data)
                    {
                        var product = order.Products[0];
                        var customerOrder = new CustomerOrderDTO
                        {
                            Amount = product.Amount,
                            CustomerId = order.CustomerId,
                            Quantity = product.Quantity,
                            ProductId = product.Id,
                            OrderNumber = orderNumber
                        };
                        await _customerOrderService.CreateOrder(customerOrder);
                    }
                }
            }
        }
    }
}
