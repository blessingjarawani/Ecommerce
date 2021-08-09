using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.WEB.Models.ViewModels;
using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.WEB.Controllers
{

    public class BaseController : Controller
    {

        protected readonly HttpClient _client;
        protected readonly IECommerceHttpClient _httpClient;
        public BaseController(IECommerceHttpClient httpClient)
        {
            _httpClient = httpClient;
            _client = httpClient.InitClient();
        }

        protected string GetToken() =>  HttpContext.Session.GetString("token");


        public async Task<string> GetUserId()
        {
            var userId = HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;
        }
    }
}
