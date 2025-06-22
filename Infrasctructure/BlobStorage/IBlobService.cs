using Microsoft.AspNetCore.Http;
 

namespace Infrasctructure.BlobStorage;

public interface IBlobService
{
    Task<string> UploadFileAsync(IFormFile file);
}
