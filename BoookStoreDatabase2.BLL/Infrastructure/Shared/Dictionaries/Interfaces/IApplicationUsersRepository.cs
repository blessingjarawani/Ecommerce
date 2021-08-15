using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.BLL.ViewModels;
using Ecommerce.BLL.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface IApplicationUsersRepository
    {
        Task<List<ApplicationUsersDTO>> GetAll();
        Task<SignInResult> Login(LoginViewModel loginViewModel);
        Task<UserDTO> GetUserByUserName(string userName);
        Task<IdentityResult> Register(RegisterUserViewModel user);
        Task LockOutUser(string userId);
        Task<bool> ChangePassword(ChangePasswordViewModel changePasswordViewModel);

    }
}
