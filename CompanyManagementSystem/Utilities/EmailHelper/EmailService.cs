using CompanyManagementSystem.DAL.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace CompanyManagementSystem.PL.Utilities.EmailHelper
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public void SendEmail(Email email)
        {
            EmailSender.SendEmail(email, _emailSettings.SmtpServer, _emailSettings.SmtpPort, _emailSettings.SenderEmail, _emailSettings.GeneratedPassword);
        }

    }
}
