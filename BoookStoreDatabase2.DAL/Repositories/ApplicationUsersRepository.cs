using AutoMapper;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.BLL.ViewModels;
using BoookStoreDatabase2.DAL.Context;
using BoookStoreDatabase2.DAL.Entities;
using Ecommerce.BLL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.DAL.Repositories
{
    public class ApplicationUsersRepository : BaseRepository, IApplicationUsersRepository
    {
        private SignInManager<ApplicationUser> _signInManager { get; }
        private RoleManager<IdentityRole> _roleManager { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        public ApplicationUsersRepository(StoreContext storesDbContext, SignInManager<ApplicationUser> signInManager,
                   RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper = null) : base(storesDbContext, mapper)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<List<ApplicationUsersDTO>> GetAll()
        {
            var result = await _dbContext.Users?.Select(x => new ApplicationUsersDTO
            {
                FirstName = x.Customer != null ? x.Customer.FirstName : x.User.FirstName,
                LastName = x.Customer != null ? x.Customer.LastName : x.User.LastName,
                UserType = x.Customer != null ? "Customer" : "Administrator",
                UserName = x.UserName,
                Email = x.Email
            })?.ToListAsync();
            return result;
        }


        public async Task<UserDTO> GetUserByUserName(string userName)
        {
            var result = await _dbContext.Users.Include(t => t.User).Include(y => y.Customer)?.FirstOrDefaultAsync(x => x.Email == userName || x.UserName == userName);
            return new UserDTO
            {
                FirstName = result.Customer != null ? result.Customer.FirstName : result.User.FirstName,
                LastName = result.Customer != null ? result.Customer.LastName : result.User.LastName,
                UserRole = result.Customer != null ? Roles.Customer : Roles.Administrator,
                UserName = result.UserName,
                Email = result.Email,
                UserId = result.Customer != null ? result.Customer.Id : result.User.Id,
            };
        }
        public async Task<SignInResult> Login(LoginViewModel loginViewModel)
        {

            return await _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                loginViewModel.Password, loginViewModel.RememberMe.Value, false);
        }

        public async Task<IdentityResult> Register(RegisterUserViewModel user)
        {
            var appUser = new ApplicationUser
            {
                Email = user.EmailAddress,
                UserName = user.UserName,

                Customer = new Customer { FirstName = user.FirstName, LastName = user.LastName, IsActive = true, DOB = DateTime.Parse("1994-01-01") }
            };
            if (_userManager.FindByNameAsync(user.EmailAddress).Result == null)
            {
                var result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    var role = "Customer";
                    _userManager.AddToRoleAsync(appUser, role).Wait();
                }
                return result;
            }
            return null;
        }
    }
}
