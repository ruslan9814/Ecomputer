namespace test.Endpoints.Users.Requests;

public sealed record RegistUserRequest(
    string Username,
    string Email,
    string Password);

