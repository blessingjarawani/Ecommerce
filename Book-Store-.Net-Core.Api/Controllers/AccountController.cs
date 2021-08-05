﻿using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
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
        private readonly IApplicationUsersService _applicationUsersService;

        public AccountController(IApplicationUsersService applicationUsersService)
        {
            _applicationUsersService = applicationUsersService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        [HttpPost("[action]")]
        public async Task<Response<AuthenticateResponse>> Login([FromBody] LoginViewModel loginViewModel)
        => await _applicationUsersService.Authenticate(loginViewModel);
    }
}

