namespace test.Infrastrcture.Email;

public interface IEmailSenderService
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage);
}