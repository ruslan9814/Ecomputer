namespace Presentation.Users.Responses;

public sealed record UserResponse(
    int Id,
    string Username,
    string Email,
    DateTime CreatedAt);

