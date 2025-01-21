using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Test.Endpoints.Users.Requests;

public sealed record RegistUserRequest(
    string Username,
    string Email,
    string Password);

