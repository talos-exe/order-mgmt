using MimeKit;
using OrderMgmtRevision.Models;
using MailKit.Net.Smtp;

namespace OrderMgmtRevision.Services
{

    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Get email settings with logging
            var sender = _config["EmailSettings:Sender"];
            var password = _config["EmailSettings:Password"];
            var smtpServer = _config["EmailSettings:SmtpServer"];
            var portStr = _config["EmailSettings:Port"];

            Console.WriteLine($"Using sender: {sender}");
            Console.WriteLine($"SMTP Server: {smtpServer}");
            Console.WriteLine($"Port: {portStr}");
            Console.WriteLine($"To Email: {toEmail}");

            // Validate settings
            if (string.IsNullOrWhiteSpace(sender))
                throw new InvalidOperationException("Email sender address is not configured");

            if (string.IsNullOrWhiteSpace(smtpServer))
                throw new InvalidOperationException("SMTP server is not configured");

            if (string.IsNullOrWhiteSpace(portStr) || !int.TryParse(portStr, out int port))
                throw new InvalidOperationException("Email port is not configured or invalid");

            if (string.IsNullOrWhiteSpace(toEmail))
                throw new ArgumentException("Recipient email address is required");

            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(sender);
            email.From.Add(MailboxAddress.Parse(sender)); // Also set From address
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = message };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpServer, port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(sender, password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
