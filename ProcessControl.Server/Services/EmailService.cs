using Microsoft.Extensions.Options;
using ProcessControl.Server.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProcessControl.Server.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mail = new MailMessage();
            mail.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using (var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                
                smtpClient.EnableSsl = true;

                await smtpClient.SendMailAsync(mail);
            }
        }
    }
}

