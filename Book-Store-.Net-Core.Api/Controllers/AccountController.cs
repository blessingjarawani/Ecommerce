using BoookStoreDatabase2.BLL.ViewModels;
using BoookStoreDatabase2.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store_.Net_Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager { get; }
        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
           
                var loginResult = await _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                    loginViewModel.Password, loginViewModel.RememberMe, false);

        }
    }
}

