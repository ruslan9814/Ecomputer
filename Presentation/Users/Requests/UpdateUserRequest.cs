namespace Presentation.Users.Requests;

public sealed record UpdateUserRequest(int Id, string Name, string Address);
