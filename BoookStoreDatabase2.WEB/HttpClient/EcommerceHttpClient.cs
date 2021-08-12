using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public ECommerceHttpClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public HttpClient InitClient()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = new TimeSpan(0, 0, 0, 20);
            var baseUrl = _configuration["Api:BaseUrl"];
            httpClient.BaseAddress = new Uri(baseUrl);
            return httpClient;
        }



          
    }
}
