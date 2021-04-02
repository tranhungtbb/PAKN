using Microsoft.AspNetCore.Http;
using PAKNAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services.FileUpload
{
    public interface IFileService
    {
        Task<IList<FileInfoModel>> Save(IFormFileCollection files, string folderPath);
        Task<byte[]> GetBinary(string filePath);
        Task<bool> Remove(string filePath);
    }
}
