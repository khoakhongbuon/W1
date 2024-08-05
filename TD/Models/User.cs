using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using TD.Areas.Identity.Data;

namespace TD.Models
{
    public class User
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public async Task<IdentityResult> RegisterAsync(UserManager<TDUser> manager, string password)
        {
            var user = new TDUser { UserName = this.UserName, Email = this.UserName };
            var result = await manager.CreateAsync(user, password);
            return result;
        }

        public async Task<SignInResult> LoginAsync(SignInManager<TDUser> signInManager)
        {
            var result = await signInManager.PasswordSignInAsync(this.UserName, this.Password, false, lockoutOnFailure: false);
            return result;
        }
    }
    public class EmailSender : IEmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailSender(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpUser),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            return client.SendMailAsync(mailMessage);
        }
    }
}
