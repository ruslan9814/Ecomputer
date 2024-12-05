using Azure.Identity;

namespace test.Endpoints.Users.Models;

public class UserRequest
{
    public string? Username { get; set; } 
    public string? Email { get; set; }
    public string? Password { get; set; }
}
