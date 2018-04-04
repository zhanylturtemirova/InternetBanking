using System.Net.Mail;
using MimeKit;
using System.Threading.Tasks;
using InternetBanking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace InternetBanking.Services
{
    public class EmailService:IEmailService
    {


        public async Task SendEmailAsync(string email, string subject, string message)

        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "esdp_group1@mail.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.mail.ru", 25, false);
                await client.AuthenticateAsync("esdp_group1@mail.ru", "User1!");
                client.Send(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        
    }
}
