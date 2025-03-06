using Domain.Users;

namespace Presentation.Users.Requests;

public sealed record RegisterUserRequest(
    string Username,
    string Email,
    string Password,
    string Address,
    Role Role,
    string returnUrl);//////role podumat

