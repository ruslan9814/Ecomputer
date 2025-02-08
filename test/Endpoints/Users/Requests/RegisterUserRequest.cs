namespace test.Endpoints.Users.Requests;

public sealed record RegisterUserRequest(
    string Username,
    string Email,
    string Password,
    string Address);

