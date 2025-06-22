using Infrastrcture.Email;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrasctructure.Email;

public class EmailSenderService(IOptions<EmailSettingsService> emailSettings)
    : IEmailSenderService
{
    private readonly List<EmailAccount> _accounts = emailSettings.Value.Accounts;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(subject);
        ArgumentNullException.ThrowIfNull(htmlMessage);

        var account = _accounts[0]; 

        var message = new MailMessage
        {
            From = new MailAddress(account.Email),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        message.To.Add(email);

        using var smtp = new SmtpClient(account.Server, account.Port)
        {
            EnableSsl = account.EnableSsl,
            Credentials = new NetworkCredential(account.Email, account.Password)
        };

        await smtp.SendMailAsync(message);
    }
}
