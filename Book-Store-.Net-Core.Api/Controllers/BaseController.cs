using BoookStoreDatabase2.DAL.Entities;
using EnumsNET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace Ecommerce.Api.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public BaseController(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        protected async Task<int> GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userDetails = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            return userDetails.CustomerId.HasValue ? userDetails.CustomerId.Value : userDetails.EmployeeId.Value;
        }
        protected async Task<List<Roles>> GetUserRoles()
        {
            var roles = await Task.Run(() => _httpContextAccessor.HttpContext.User?.Claims.Where(x => x.Type == ClaimTypes.Role));
            return roles?.Select(x => Enums.Parse<Roles>(x.Value)).ToList();
        }
    }
}

