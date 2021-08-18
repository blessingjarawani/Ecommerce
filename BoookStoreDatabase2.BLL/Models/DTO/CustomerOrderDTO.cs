using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecommerce.BLL.Models.DTO
{
    public class CustomerOrderDTO
    {
        [Required]
        public int CustomerId { get; set; }
        public string OrderNumber { get; set;}

        [Required]
        public int ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Amount { get; set; }

       

        
    }
}
