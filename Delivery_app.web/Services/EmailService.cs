using Delivery_app.web.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Delivery_app.web.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toName, List<string> toEmail, string subject, string htmlBody);
    }

    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string toName, List<string> toEmail, string subject, string htmlBody)
        {
            try
            {
                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
                message.From.Add(from);

                foreach(var s in toEmail)
                {
                    MailboxAddress to = new MailboxAddress(toName, s);
                    message.To.Add(to);
                }

                message.Subject = subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = htmlBody;

                message.Body = bodyBuilder.ToMessageBody();

                SmtpClient client = new SmtpClient();
                await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port);
                await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
