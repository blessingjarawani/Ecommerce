using BoookStoreDatabase2.BLL.Infrastructure.Shared.Responses;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL.Infrastructure.Shared.Services.Email
{
    public class MailService : IMailService
    {
        private readonly EmailSenderConfig _config;
        private readonly ILogger<MailService> _logger;

        public MailService(EmailSenderConfig config, ILogger<MailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public BaseResponse SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            return Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        private BaseResponse Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_config.SmtpServer, _config.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_config.UserName, _config.Password);
                    client.Send(mailMessage);
                    return new BaseResponse{Success = true};
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.GetBaseException().Message);
                    return new BaseResponse { Success = false, Message = ex.GetBaseException().Message };
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
