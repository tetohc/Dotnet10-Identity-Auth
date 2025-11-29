using IdentityNET10.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace IdentityNET10.Services
{
    /// <summary>
    /// Implementación del servicio de envío de correos electrónicos.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _settings;

        public EmailSender(IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        /// <summary>
        /// Envía un correo electrónico de forma asíncrona.
        /// <summary>
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient())
            {
                client.Host = _settings.Host;
                client.Port = _settings.Port;
                client.EnableSsl = _settings.EnableSSL;
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

                var message = new MailMessage
                {
                    From = new MailAddress(_settings.Username, "Soporte - IdentityNET10"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(new MailAddress(toEmail));
                await client.SendMailAsync(message);
            }
        }
    }
}