using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
using BoookStoreDatabase2.BLL.ViewModels;
using BoookStoreDatabase2.DAL.Entities;
using Ecommerce.BLL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
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
        public async Task<Response<AuthenticateResponse>> Login([FromBody] LoginViewModel loginViewModel)
        => await _applicationUsersService.Authenticate(loginViewModel);

        [HttpPost("[action]")]
        public async Task<Response<bool>> Register([FromBody] RegisterUserViewModel request)
        => await _applicationUsersService.Register(request);

        [HttpPost("[action]")]
        public async Task<Response<bool>> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
            => await _applicationUsersService.ChangePassword(changePasswordViewModel);
    }
}

