using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using BoookStoreDatabase2.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Interfaces
{
    public interface IApplicationUsersService
    {
        Task<Response<List<ApplicationUsersDTO>>> GetAllUsers();
    }
}
