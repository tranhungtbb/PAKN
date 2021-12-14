using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models;
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
                if (!System.IO.File.Exists(fullPath)) return null;
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

        public async Task<bool> Remove(string filePath)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string fullPath = Path.Combine(contentRootPath,filePath);
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Remove(string[] filePaths)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string fullPath = "";

            try
            {
                foreach(var path in filePaths)
                {
                    fullPath = Path.Combine(contentRootPath, path);
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IList<FileInfoModel>> Save(IFormFileCollection files, string folder)
        {
            try
            {

                var list = new List<FileInfoModel>();

                string contentRootPath = _webHostEnvironment.ContentRootPath;
                string rootFolder = "Upload";

                string folderPath = Path.Combine(contentRootPath, rootFolder, folder);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach(var file in files)
                {
                    string fileName = $"{DateTime.Now.ToString("ddMMyyyyhhmmss")}-{file.FileName}";
                    string filePath = Path.Combine(folderPath, fileName);
                    using (var memoryStream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(memoryStream);
                    }
                    list.Add(new FileInfoModel {
                        Name = file.FileName, 
                        Path = Path.Combine(rootFolder, folder, fileName), 
                        Type = file.ContentType
                    });
                }

                return list;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

    }
}
