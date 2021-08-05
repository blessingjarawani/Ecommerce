using AutoMapper;
using BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.BLL.ViewModels;
using BoookStoreDatabase2.DAL.Context;
using BoookStoreDatabase2.DAL.Entities;
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
        public SignInManager<ApplicationUser> _signInManager { get; }
        public ApplicationUsersRepository(StoreContext storesDbContext, SignInManager<ApplicationUser> signInManager, IMapper mapper = null) : base(storesDbContext, mapper)
        {
            _signInManager = signInManager;
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
            var result = await _dbContext.Users.Include(t=>t.User)?.FirstOrDefaultAsync(x => x.Email == userName || x.UserName == userName);
            return  new UserDTO
            {
                FirstName = result.Customer != null ? result.Customer.FirstName : result.User.FirstName,
                LastName = result.Customer != null ? result.Customer.LastName : result.User.LastName,
                UserRole = result.Customer != null ? Roles.Customer : Roles.Administrator,
                UserName = result.UserName,
                Email = result.Email
            };
        }
        public async Task<SignInResult> Login(LoginViewModel loginViewModel)
        {

            return await _signInManager.PasswordSignInAsync(loginViewModel.UserName,
                loginViewModel.Password, loginViewModel.RememberMe.Value, false);
        }
    }
}
