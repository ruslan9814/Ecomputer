namespace Test.Endpoints.Users.Requests;

public sealed record UpdateUserRequest(int Id, string Username, string Email, string NewPassword);

