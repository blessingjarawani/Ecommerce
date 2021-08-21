using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using Ecommerce.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using Ecommerce.BLL.Models.DTO;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace OrderServicesProducer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IBusControl _bus;
        private readonly IConfiguration _config;
        private readonly ICustomerOrderService _orderService;

        public OrderController(ICartService cartService, IBusControl bus, IConfiguration config, ICustomerOrderService orderService)
        {
            _cartService = cartService;
            _bus = bus;
            _config = config;
            _orderService = orderService;
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse> CheckOutCustomerOrderLine([FromBody] UpdateCustomerOrderLineCommand command)
        {
            command.NewStatus = CartStatus.InOrderingProcess;
            var updateResult = await _cartService.UpdateCustomerOrderLine(command);
            if (!updateResult.Success)
                return updateResult;
            var orderLinesResult = await _cartService.GetCustomerCart(command.CustomerId,command.NewStatus);
            var stringurl = _config["RabbitMQ:OrderQueue"];
            var url = new Uri(stringurl);
            var endPoint = await _bus.GetSendEndpoint(url);
            await endPoint.Send(orderLinesResult);
            return new BaseResponse { Success = orderLinesResult.Success }; ;
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse> CheckOutCustomerOrderLine([FromBody] GetCustomerOrderCommand command)
            => await _orderService.GetCustomerOrderHistory(command.CustomerId);
    }
}
