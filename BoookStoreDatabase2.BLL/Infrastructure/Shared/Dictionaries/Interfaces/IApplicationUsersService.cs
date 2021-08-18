using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models;
using BoookStoreDatabase2.BLL.Models.DTO;
using BoookStoreDatabase2.BLL.ViewModels;
using Ecommerce.BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface IApplicationUsersService
    {
        Task<ObjectResponse<List<ApplicationUsersDTO>>> GetAllUsers();
        Task<ObjectResponse<AuthenticateResponse>> Authenticate(LoginViewModel request);
        Task<ObjectResponse<bool>> Register(RegisterUserViewModel request);
        Task LockOutUser(string userId);
        Task<ObjectResponse<bool>> ChangePassword(ChangePasswordViewModel changePasswordViewModel);
    }
}
