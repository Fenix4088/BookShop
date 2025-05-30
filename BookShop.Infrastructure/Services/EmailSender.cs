using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;

namespace BookShop.Infrastructure.Services;

public sealed class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("noreplay@bookshop.com"));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, SecureSocketOptions.StartTls);
        await smtpClient.AuthenticateAsync("d78f6b4d233758", "6e1daebf589e93");
        await smtpClient.SendAsync(email);
        await smtpClient.DisconnectAsync(true);
    }
}
