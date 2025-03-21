using Microsoft.AspNetCore.Http;

namespace Business.Utilities.File;

public interface IFileService
{
    string Upload(IFormFile file, string folder, string oldFileName = null);
    void Delete(string folder, string fileName);
    bool IsImage(string contentType);
    bool IsTrueSize(long length, long maxSize = 500);
}
