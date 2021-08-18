using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
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

        public OrderController(ICartService cartService, IBusControl bus, IConfiguration config)
        {
            _cartService = cartService;
            _bus = bus;
            _config = config;
        }

        [HttpPost("[action]")]
        public async Task<BaseResponse> CheckOutCustomerOrderLine(UpdateCustomerOrderLineCommand command)
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
    }
}
