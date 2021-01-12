using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService
{
    public class EmailSender : IEmailSender
    {

        private EmailConfiguration _config;

        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _config = emailConfiguration;
        }


        public async Task SendEmail(Message message)
        {
            var email = CreateEmail(message);
            await Send(email);
        }

        private async Task Send(MimeMessage email)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_config.SmtpServer, _config.Port, true);
                    await client.AuthenticateAsync(_config.UserName, Environment.GetEnvironmentVariable("Password", EnvironmentVariableTarget.User));
                    client.AuthenticationMechanisms.Remove("X0AUTH2");
                    await client.SendAsync(email);
                }
                catch(Exception ex) { 
                    throw new Exception(ex.ToString());
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }

            }
        }

        public MimeMessage CreateEmail(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_config.From));
            emailMessage.To.AddRange(message.To.Select(user=>MailboxAddress.Parse(user)));
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format(message.Content) };
            return emailMessage;
        }
    }
}
