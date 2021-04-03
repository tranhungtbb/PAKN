using Microsoft.AspNetCore.Hosting;
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
using PAKNAPI.ModelBase;

namespace PAKNAPI.Controllers
{

    [Route("api/files")]
    [ApiController]
    public class UploadFileController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppSetting _appSetting;

        public UploadFileController(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting)
        {
            _webHostEnvironment = webHostEnvironment;
            _appSetting = appSetting;
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

                var fileInfo = new {
                    Name = file.FileName,
                    Path = $"{folderName}/{fileName}",
                    Type = file.ContentType
                };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = fileInfo };
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
            //string contentRootPath = _webHostEnvironment.ContentRootPath;
            //string filePath = Path.Combine(contentRootPath , "Upload/News", name);
            //try
            //{
            //    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            //    {
            //        using (var memoryStream = new MemoryStream())
            //        {
            //            await fileStream.CopyToAsync(memoryStream);
            //            Bitmap image = new Bitmap(1, 1);
            //            image.Save(memoryStream, ImageFormat.Jpeg);

            //            byte[] byteImage = memoryStream.ToArray();
            //            return byteImage;
            //        }
            //    }
            //}
            //catch
            //{
            //    return null;
            //}
            string filePath = Path.Combine("Upload/News", name);
            var bin = await this.getFileBin(filePath);
            return bin;

        }
        [HttpPost]
        [Route("get-news-avatar")]
        public async Task<List<object>> GetNewsAvatars([FromBody]string[] newsIds)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var listAvatarByte = new List<object>();
            try
            {

                List<NENewsGetAllOnPage> rsNENewsGetAllOnPage = await new NENewsGetAllOnPage(_appSetting).NENewsGetAllOnPageDAO(string.Join(",", newsIds),int.MaxValue,1,null,null,null);

                if(rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Any())
                {
                    foreach (var item in rsNENewsGetAllOnPage)
                    {

                        if (string.IsNullOrEmpty(item.ImagePath.Trim())) continue;

                        string filePath = Path.Combine(contentRootPath, item.ImagePath);

                        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                await fileStream.CopyToAsync(memoryStream);
                                Bitmap image = new Bitmap(1, 1);
                                image.Save(memoryStream, ImageFormat.Jpeg);

                                byte[] byteImage = memoryStream.ToArray();
                                listAvatarByte.Add(new { item.Id, byteImage });
                            }
                        }
                    }
                }
                return listAvatarByte;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        private async Task<byte[]> getFileBin(string filePath)
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
