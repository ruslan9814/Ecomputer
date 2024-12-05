namespace test.Endpoints.Users.Models;

public class UserResponse
{
    public int Id { get; set; }        
    public string? Username { get; set; } 
    public string? Email { get; set; }  
    public DateTime CreatedAt { get; set; } 
}
