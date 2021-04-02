using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services.FileUpload
{
    public interface IFileService
    {
        Task<object> Save(IFormFileCollection files, string folderPath);
        Task<byte[]> GetBinary(string filePath);
    }
}
