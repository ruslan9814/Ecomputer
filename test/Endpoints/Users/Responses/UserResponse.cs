namespace Test.Endpoints.Users.Responses;

public sealed record UserResponse(
    int Id,
    string Username,
    string Email,
    DateTime CreatedAt);

