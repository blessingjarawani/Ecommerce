using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Models.DTO
{
    public class CustomerOrderSummaryDTO
    {
        public DateTime OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderProducts> Products { get; set; }
    }

    public class OrderProducts
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
}
