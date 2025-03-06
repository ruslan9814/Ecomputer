using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Presentation.Users.Requests;

public sealed record LoginUserRequest(
    [NotNull] string Email, 
    [NotNull] string Password);

