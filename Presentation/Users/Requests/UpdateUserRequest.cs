using Microsoft.AspNetCore.Http;

namespace Presentation.Users.Requests;

public sealed record UpdateUserRequest(int Id, string? Name = null, 
    string? Address = null, string? Password = null, IFormFile? ImageFile = null);
