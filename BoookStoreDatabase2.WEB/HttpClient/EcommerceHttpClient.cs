using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ECommerce.WEB.EcommerceHttpClient
{
    public class ECommerceHttpClient : IECommerceHttpClient
    {
        private HttpClient httpClient;
        public HttpClient InitClient()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 0, 5);
           
            return httpClient;
        }



          
    }
}
