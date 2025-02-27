//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Hosting;
//using System.IO;

//namespace Business.Utilities.File;

//public class FileService : IFileService
//{
//    private readonly IWebHostEnvironment _webHostEnvironment;

//    public FileService(IWebHostEnvironment webHostEnvironment)
//    {
//        _webHostEnvironment = webHostEnvironment;
//    }

//    public string Upload(IFormFile file, string folder)
//    {
//        if (file == null || file.Length == 0) return string.Empty;

//        var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
//        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

//        Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, folder));

//        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
//        {
//            file.CopyTo(fileStream);
//        }

//        return fileName;
//    }

//    public void Delete(string folder, string fileName)
//    {
//        if (string.IsNullOrEmpty(fileName)) return;

//        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, folder, fileName);

//        if (System.IO.File.Exists(filePath))
//        {
//            System.IO.File.Delete(filePath);
//        }
//    }

//    public bool IsImage(string contentType)
//    {
//        return contentType.StartsWith("image/");
//    }

//    public bool IsTrueSize(long length, long maxSize = 500)
//    {
//        return length <= maxSize * 1024;
//    }
//}