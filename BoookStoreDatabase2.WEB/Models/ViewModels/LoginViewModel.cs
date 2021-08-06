using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BoookStoreDatabase2.WEB.Models.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [JsonProperty("userName")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        [JsonProperty("rememberMe")]
        public bool RememberMe { get; set; }
    }
}
