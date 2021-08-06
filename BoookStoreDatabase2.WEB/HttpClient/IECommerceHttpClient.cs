using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommerce.WEB.EcommerceHttpClient
{
    public interface IECommerceHttpClient
    {
        HttpClient InitClient();
    }
}
