using ECommerceAPI.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;

namespace ECommerceAPI.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<SmtpSettings> smtpSettings, ILogger<EmailSender> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
        {
            try
            {
                // SMTP logic here
                var mail = new MimeMessage();
                mail.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
                mail.To.Add(MailboxAddress.Parse(to));
                mail.Subject = subject;
                mail.Body = new TextPart("html") { Text = body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                await smtp.ConnectAsync(
                _smtpSettings.Host,
                _smtpSettings.Port,
                SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(
                    _smtpSettings.Username,
                    _smtpSettings.Password);

                await smtp.SendAsync(mail);

                await smtp.DisconnectAsync(true);

                Console.WriteLine($"Email sent to {to}");

                await Task.CompletedTask;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", to);
            }
        }
    }
}
