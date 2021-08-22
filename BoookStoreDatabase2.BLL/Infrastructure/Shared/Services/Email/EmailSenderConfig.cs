using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Infrastructure.Shared.Services.Email
{
    public class EmailSenderConfig
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
