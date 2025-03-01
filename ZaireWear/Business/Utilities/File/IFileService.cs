
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utilities.File
{
    public interface IFileService
    {
        string Upload(IFormFile file, string folder);
        void Delete(string folder, string fileName);
        bool IsImage(string contentType);
        bool IsTrueSize(long length, long maxSize = 500);

    }
}
