using CompanyManagementSystem.DAL.Models;
using System.Net.Mail;
using System.Net;

namespace CompanyManagementSystem.PL.Utilities.EmailHelper
{
    public static class EmailSender
    {
        public static void SendEmail(Email email, string smtpServer, int smtpPort, string senderEmail, string GeneratedPassword)
        {
            var client = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, GeneratedPassword)
            };

            var message = new MailMessage(senderEmail, email.To, email.Subject, email.Body);
            client.Send(message);
        }

        //public static void SendEmail(Email email)
        //{
        //    var Client = new SmtpClient("smtp.gmail.com", 587)
        //    {
        //        EnableSsl = true,
        //        Credentials = new NetworkCredential("chopyasser722@gmail.com", "rnvanuuyfewpexub")

        //    };
        //    Client.Send("chopyasser722@gmail.com", email.To, email.Subject, email.Body);
        //}
    }
}
