using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
using BoookStoreDatabase2.DAL.Entities;
using BoookStoreDatabase2.WEB.Models.ViewModels;
using Ecommerce.BLL.ViewModels;
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _client.PostAsync("Account/Register", new StringContent(JsonConvert.SerializeObject(registerViewModel), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ObjectResponse<bool>>(content);
                    if (!result.Success)
                    {
                        ModelState.AddModelError(string.Empty, result.Message);
                        return View(registerViewModel);
                    }
                    return RedirectToAction("login", "account");
                }
            }
            return View(registerViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _client.PostAsync("Account/Login", new StringContent(JsonConvert.SerializeObject(loginViewModel), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<ObjectResponse<AuthenticateResponse>>(content);
                        if (!result.Success)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid Login");
                            return View(loginViewModel);
                        }
                        HttpContext.Session.SetString("token", result.Data.Token);
                        var claims = new List<Claim>() {
                        new Claim(ClaimTypes.NameIdentifier, Convert.ToString(result.Data.UserId)),
                        new Claim(ClaimTypes.Name, result.Data.UserName),
                        new Claim(ClaimTypes.Role, result.Data.UserRole.ToString()),
                        new Claim(ClaimTypes.Email, result.Data.Email)
                        };
                        //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                        var principal = new ClaimsPrincipal(identity);
                        //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                        {
                            IsPersistent = loginViewModel.RememberMe
                        });
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.GetBaseException().Message);
                return View(loginViewModel);
            }

        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            if (ModelState.IsValid)
            {
                var currentUserEmail = HttpContext.User?.FindFirst(ClaimTypes.Name).Value;
                changePasswordViewModel.CurrentUserName = currentUserEmail;
                var response = await _client.PostAsync("Account/ChangePassword", new StringContent(JsonConvert.SerializeObject(changePasswordViewModel), Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ObjectResponse<bool>>(content);
                    if (!result.Success)
                    {
                        ModelState.AddModelError(string.Empty, result.Message);
                        return View(changePasswordViewModel);
                    }
                    return RedirectToAction("index", "home");
                }
            }
            return View(changePasswordViewModel);
        }
    }
}

