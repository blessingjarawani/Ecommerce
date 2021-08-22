using BoookStoreDatabase2.BLL.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static BoookStoreDatabase2.BLL.Infrastructure.Shared.Dictionaries.Dictionary.Dictionary;

namespace BoookStoreDatabase2.BLL.Models
{
    public class AuthenticateResponse
    {
        [JsonProperty("userId")]
        public int UserId { get; private set; }
        [JsonProperty("firstName")]
        public string FirstName { get; private set; }
        [JsonProperty("lastName")]
        public string LastName { get; private set; }
        [JsonProperty("userName")]
        public string UserName { get; private set; }
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("userRole")]
        public Roles UserRole { get; private set; }
        [JsonProperty("email")]
        public string Email { get; private set; }
        public AuthenticateResponse()
        {
        }
        public AuthenticateResponse(UserDTO user, string token)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            UserName = user.UserName;
            Token = token;
            UserRole = user.UserRole;
            Email = user.Email;
        }

    }
}
