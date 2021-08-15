using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ApplicationUsersController : ControllerBase
    {
        private IApplicationUsersService _applicationUsersService { get; }

        public ApplicationUsersController(IApplicationUsersService applicationUsersService)
        {
            _applicationUsersService = applicationUsersService;
        }

        [HttpGet("[action]")]
        public async Task<Response<List<ApplicationUsersDTO>>> GetUsers()
        {
            return await _applicationUsersService.GetAllUsers();
        }

        [HttpPost("[action]")]
        public async Task LockUser([FromBody] ApplicationUserCommandDTO command)
        {
           await _applicationUsersService.LockOutUser(command.UserId);
        }
    }
}
