using System;
using System.Collections.Generic;
using System.Text;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace Ecommerce.BLL.Models.DTO
{
    public class UpdateCustomerOrderLineCommand
    {
        public int CustomerId { get; set; }
        public CartStatus CurrentStatus { get; set; }
        public CartStatus NewStatus { get; set; }
    }
}
