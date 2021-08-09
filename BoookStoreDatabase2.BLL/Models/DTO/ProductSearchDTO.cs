using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Models.DTO
{
    public class ProductSearchDTO
    {
        public bool? price { get; set; }
        public bool? name { get; set; }

        public int? Id { get; set; }
      
    }
}
