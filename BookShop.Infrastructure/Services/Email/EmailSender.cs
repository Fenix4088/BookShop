using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace BookShop.Infrastructure.Services.Email;

public sealed class EmailSender : IEmailSender
{

    private readonly EmailSettings emailSettings;
    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        this.emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(emailSettings.EmailFrom));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync(emailSettings.User, emailSettings.Password);
        await smtpClient.SendAsync(email);
        await smtpClient.DisconnectAsync(true);
    }
}
