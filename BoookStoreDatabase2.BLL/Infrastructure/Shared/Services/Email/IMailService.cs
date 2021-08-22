using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Infrastructure.Shared.Services.Email
{
    public interface IMailService
    {
        BaseResponse SendEmail(Message message);
    }
}
