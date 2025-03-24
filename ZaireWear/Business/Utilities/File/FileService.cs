using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Business.Utilities.File;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Upload(IFormFile file, string folder)
    {
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        var filePath = Path.Combine(folderPath, fileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
        {
            file.CopyTo(fileStream);
        }

        return Path.Combine(folder, fileName).Replace("\\", "/");
    }


    public void Delete(string folder, string fileName)
    {
        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    public bool IsImage(string contentType)
    {
        return contentType.Contains("image/");
    }

    public bool IsTrueSize(long length, long maxSize = 500)
    {
        return length / 1024 < maxSize;
    }
}
