using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net;
using System.Net.Mail;

namespace Blog.Service
{
    public class EmailSender : IEmailSender
    {
        public SmtpSettings Options { get; }

        private SmtpClient client;

        public EmailSender(IOptions<SmtpSettings> options)
        {
            Options = options.Value;

            client = new SmtpClient(Options.Host, Options.Port)
            {
                Credentials = new NetworkCredential(Options.Username, Options.Password)
            };
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailMessage = new MailMessage(Options.From, email, subject, htmlMessage);

            return client.SendMailAsync(mailMessage);
        }
    }
}
