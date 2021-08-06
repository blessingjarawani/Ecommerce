using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.WEB.Models.ViewModels;
using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.WEB.Controllers
{
    public class AccountController : Controller
    {
       
        private readonly HttpClient _client;
        private readonly IECommerceHttpClient _httpClient;
        public AccountController(IECommerceHttpClient httpClient)
        {
            _httpClient = httpClient;
            _client = httpClient.InitClient();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
         
            return RedirectToAction("index", "home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsync("http://localhost:45447/api/Account/Login", new StringContent(JsonConvert.SerializeObject(loginViewModel), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Login");

            }

            return View(loginViewModel);
        }
    }
}

