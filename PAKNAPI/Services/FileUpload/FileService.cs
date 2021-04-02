using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services.FileUpload
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<byte[]> GetBinary(string filePath)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string fullPath = Path.Combine(contentRootPath, filePath);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        byte[] byteImage = memoryStream.ToArray();
                        return byteImage;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public Task<object> Save(IFormFileCollection files, string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
