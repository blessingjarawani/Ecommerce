using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using Ecommerce.BLL.Models.DTO;
using ECommerce.WEB.EcommerceHttpClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Newtonsoft.Json;

namespace BoookStoreDatabase2.WEB.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ApplicationUsersController : BaseController
    {
        private IApplicationUsersService _applicationUsersService { get; }

        public ApplicationUsersController(IECommerceHttpClient httpClient) : base(httpClient)
        {

        }

        public async Task<IActionResult> Index()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await _client.GetAsync($"ApplicationUsers/GetUsers");
            var content = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<ObjectResponse<List<ApplicationUsersDTO>>>(content);
            if (!users.Success)
            {
                ModelState.AddModelError(string.Empty, users.Message);
                return View();
            }
            return View(users.Data);
        }
        public async Task<IActionResult> LockUser(string userId)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GetToken());
            var response = await _client.PostAsync($"ApplicationUsers/LockUser", new StringContent(JsonConvert.SerializeObject(new ApplicationUserCommandDTO { UserId = userId }), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error On Deactivating user");

            }
            return RedirectToAction("Index");
        }
    }
}