﻿using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Drawing.Imaging;

namespace PAKNAPI.Controllers
{

    [Route("api/files")]
    [ApiController]
    public class UploadFileController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UploadFileController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("upload")]
        [Authorize]
        public async Task<ActionResult<object>> Upload(string folder = null)
        {
            try
            {

                var file = Request.Form.Files[0];

                if (file.Length <= 0)
                {
                    return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "File not found!" };
                }

                string contentRootPath = _webHostEnvironment.ContentRootPath;

                string folderName = string.IsNullOrEmpty(folder) ? "Upload/Orther" : "Upload/" + folder;

                string fileName = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}-{file.FileName}";
                string folderPath = Path.Combine(contentRootPath, folderName);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileNamePath = Path.Combine(folderPath,fileName);

                using (var memoryStream = System.IO.File.Create(fileNamePath))
                {
                    await file.CopyToAsync(memoryStream);
                }

                var model = new UpdateFileInfoModel {
                    Name = file.FileName,
                    Path = $"{folderName}/{fileName}",
                    Type = file.ContentType
                };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = model };
            }
            catch (Exception e)
            {
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = e.Message };
            }
        }
        [HttpGet]
        [Route("get-news-avatar/{name}")]
        public async Task<byte[]> GetNewsAvatar(string name)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            string filePath = Path.Combine(contentRootPath , "Upload/News", name);

            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);
                        Bitmap image = new Bitmap(1, 1);
                        image.Save(memoryStream, ImageFormat.Jpeg);

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
    }
}
