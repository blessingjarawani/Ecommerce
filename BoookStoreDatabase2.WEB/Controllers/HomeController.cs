using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BoookStoreDatabase2.WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BoookStoreDatabase2.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using ECommerce.WEB.EcommerceHttpClient;

namespace BoookStoreDatabase2.WEB.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController(IECommerceHttpClient httpClient) : base(httpClient)
        {

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
