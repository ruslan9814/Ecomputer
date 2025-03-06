namespace Infrastrcture.Email;


public class EmailSettingsService
{
    public string Server { get; set; } = null!;
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
