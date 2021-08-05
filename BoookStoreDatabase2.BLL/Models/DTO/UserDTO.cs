using System;
using System.Collections.Generic;
using System.Text;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Models.DTO
{
   public class UserDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Roles UserRole { get; set; }
    }
}
