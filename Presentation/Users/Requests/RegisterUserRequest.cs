using Domain.Users;
using Microsoft.AspNetCore.Http;

namespace Presentation.Users.Requests;

public sealed record RegisterUserRequest(
    string Name,
    string Email,
    string Password,
    string Address,
    Role Role,
    string returnUrl
    );//////role podumat

