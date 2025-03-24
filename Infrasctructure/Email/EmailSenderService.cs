using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Infrastrcture.Email;

public class EmailSenderService(IOptions<EmailSettingsService> emailSettings) 
    : IEmailSenderService
{
    private readonly EmailSettingsService _emailSettings = emailSettings.Value;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        ArgumentNullException.ThrowIfNull(email, nameof(email));
        ArgumentNullException.ThrowIfNull(subject, nameof(email));
        ArgumentNullException.ThrowIfNull(htmlMessage, nameof(email));

        await Execute(email, subject, htmlMessage);
    }

    private async Task Execute(string email, string subject, string htmlMessage)
    {
        var message = new MailMessage()
        {
            From = new MailAddress(_emailSettings.Email)
        };
        message.To.Add(email);
        message.Subject = subject;   
        message.Body = htmlMessage;
        message.IsBodyHtml = true;

        using var smtp = new SmtpClient(_emailSettings.Server, _emailSettings.Port);
        smtp.EnableSsl = _emailSettings.EnableSsl;
        smtp.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);

        await smtp.SendMailAsync(message);
    }


}
