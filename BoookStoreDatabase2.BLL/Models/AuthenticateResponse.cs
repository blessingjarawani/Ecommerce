using BoookStoreDatabase2.BLL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Models
{
   public class AuthenticateResponse
    {
        public int UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        public string Token { get; private set; }
        public Roles UserRole { get; private set; }
        public AuthenticateResponse(UserDTO user, string token)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Token = token;
            UserRole = user.UserRole;
        }

    }
}
