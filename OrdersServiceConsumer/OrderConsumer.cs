using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Infrastructure.Shared.Services.Email;
using Ecommerce.BLL.Models.DTO;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace OrdersServiceConsumer
{
    public class OrderConsumer : IConsumer<ObjectResponse<List<OrderLineDTO>>>
    {
        private readonly ICustomerOrderService _customerOrderService;
        private readonly IMailService _mailService;
        private readonly ICartService _cartService;

        public OrderConsumer(ICustomerOrderService customerOrderService, IMailService mailService, ICartService cartService)
        {
            _customerOrderService = customerOrderService;
            _mailService = mailService;
            _cartService = cartService;
        }

        public async Task Consume(ConsumeContext<ObjectResponse<List<OrderLineDTO>>> context)
        {
            var orders = context.Message;
            if (orders.Success)
            {
                if ((orders.Data?.Any()) ?? false)
                {
                    var orderNumber = Guid.NewGuid().ToString();
                    var email = orders.Data[0].UserEmail;
                    var customerId = orders.Data[0].CustomerId;
                    foreach (var order in orders.Data)
                    {
                        var product = order.Products[0];
                        var customerOrder = new CustomerOrderDTO
                        {
                            Amount = (decimal)product.Total,
                            CustomerId = order.CustomerId,
                            Quantity = product.Quantity,
                            ProductId = product.Id,
                            OrderNumber = orderNumber
                        };
                        await _customerOrderService.CreateOrder(customerOrder);
                    }
                    var updateResult = await _cartService.UpdateCustomerOrderLine(new UpdateCustomerOrderLineCommand {CustomerId = customerId ,NewStatus = CartStatus.Processed, CurrentStatus =CartStatus.InOrderingProcess});
                    _mailService.SendEmail(new Message(new List<string> { email }, $"Order Number {orderNumber.Substring(0, 6)}", $"Order Number {orderNumber.Substring(0, 6)} Check Cart History in app"));
                }
            }
        }
    }
}
