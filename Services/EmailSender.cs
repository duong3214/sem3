using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace backendMuseum.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        // Constructor để nhận thông tin từ IConfiguration
        public EmailSender(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");

            // Kiểm tra giá trị null và gán giá trị mặc định nếu null
            _smtpServer = emailSettings["SmtpServer"] ?? throw new ArgumentNullException("SmtpServer is required.");
            _smtpPort = int.TryParse(emailSettings["SmtpPort"], out var port) ? port : throw new ArgumentNullException("SmtpPort is required.");
            _smtpUser = emailSettings["SmtpUser"] ?? throw new ArgumentNullException("SmtpUser is required.");
            _smtpPass = emailSettings["SmtpPass"] ?? throw new ArgumentNullException("SmtpPass is required.");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your App Name", _smtpUser));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_smtpServer, _smtpPort, false);
                    await client.AuthenticateAsync(_smtpUser, _smtpPass);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    throw new Exception("Không thể gửi email: " + ex.Message);
                }
            }
        }
    }
}
