using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BookShop.Infrastructure.Services;

public sealed class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        using var client = new SmtpClient("smtp.example.com")
        {
            Credentials = new NetworkCredential("username", "password"),
            EnableSsl = true
        };

        var message = new MailMessage("noreply@bookshop.com", email, subject, htmlMessage);
        await client.SendMailAsync(message);
    }
}