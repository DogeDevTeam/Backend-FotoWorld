using FotoWorldBackend.Models;
using FotoWorldBackend.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace FotoWorldBackend.Services.Email
{
    /// <summary>
    /// Provides email sending for all controllers
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        
        public EmailService(IConfiguration config)
        {
            _config= config;
        }

        public void SendEmail(EmailModel request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(request.Recipant));
            email.Subject = request.Subject;
            email.Body= new TextPart(TextFormat.Html) { Text=request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailHost"], Convert.ToInt32(_config["EmailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailUsername"], _config["EmailPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        public void SendActivationEmailUser(User user)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Account Activation";

            var url = _config.GetValue<string>("Urls:BackendUrl") + "/activate-user/"+ 
                SymmetricEncryption.Encrypt(_config["SECRET_KEY"], Convert.ToString(user.Id));

            //tutaj trzeba skleic ladnego urla
            email.Body = new TextPart(TextFormat.Html) { Text = url };

            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailHost"], Convert.ToInt32(_config["EmailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailUsername"], _config["EmailPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);
        }


        public void SendRestartPasswordEmail(User user) {

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Restart Password";

            var url = _config.GetValue<string>("Urls:BackendUrl") + "/restart-password/" +
                SymmetricEncryption.Encrypt(_config["SECRET_KEY"], Convert.ToString(user.Id));

            //tutaj trzeba skleic ladnego urla
            email.Body = new TextPart(TextFormat.Html) { Text = url };

            using var smtp = new SmtpClient();
            smtp.Connect(_config["EmailHost"], Convert.ToInt32(_config["EmailPort"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_config["EmailUsername"], _config["EmailPassword"]);
            smtp.Send(email);
            smtp.Disconnect(true);


        }



    }
}
