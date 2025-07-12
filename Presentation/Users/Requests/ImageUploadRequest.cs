using Microsoft.AspNetCore.Http;

namespace Presentation.Users.Requests;

public class ImageUploadRequest
{
    public IFormFile ImageFile { get; set; }
}
