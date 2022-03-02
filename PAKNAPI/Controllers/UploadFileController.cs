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
using PAKNAPI.Services.FileUpload;
using PAKNAPI.Models.Results;
using NSwag.Annotations;
using Microsoft.AspNetCore.SignalR;
using PAKNAPI.Chat;
using SignalR.Hubs;
using PAKNAPI.Chat.ResponseModel;
using System.Text.Json;
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.Controllers
{

    [Route("api/upload-files")]
    [ApiController]
    [OpenApiTag("Upload file", Description = "upload file")]
    public class UploadFileController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppSetting _appSetting;
        private readonly IFileService _fileService;
        private readonly IHubContext<ChatHub> _hubContext;



        public UploadFileController(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting, IFileService fileService, IHubContext<ChatHub> hub)
        {
            _webHostEnvironment = webHostEnvironment;
            _appSetting = appSetting;
            _fileService = fileService;
            _hubContext = hub;
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

                string fileNamePath = Path.Combine(folderPath, fileName);

                using (var memoryStream = System.IO.File.Create(fileNamePath))
                {
                    await file.CopyToAsync(memoryStream);
                }

                var fileInfo = new
                {
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
        public async Task<List<object>> GetNewsAvatars([FromBody] string[] newsIds)
        {
            string contentRootPath = _webHostEnvironment.ContentRootPath;

            var listAvatarByte = new List<object>();
            try
            {

                List<NENewsGetAllOnPage> rsNENewsGetAllOnPage = await new NENewsGetAllOnPage(_appSetting).NENewsGetAllOnPageDAO(string.Join(",", newsIds), int.MaxValue, 1, null, null, null);

                if (rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Any())
                {
                    foreach (var item in rsNENewsGetAllOnPage)
                    {

                        if (string.IsNullOrEmpty(item.ImagePath.Trim())) continue;

                        string filePath = Path.Combine(contentRootPath, item.ImagePath);
                        if (!System.IO.File.Exists(filePath)) continue;
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
            catch (Exception e)
            {
                return new List<object>();
            }
        }

        [HttpGet]
        [Route("downloadbin")]
        public async Task<byte[]> DownloadBin(string path)
        {
            Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
            var filePath = decrypt.DecryptData(path);

            byte[] bin = await _fileService.GetBinary(filePath);

            return bin;

        }
        [HttpPost]
        [Route("downloadfile")]
        public async Task<IActionResult> DownloadFile([FromForm] string path)
        {
            Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
            var filePath = decrypt.DecryptData(path);

            byte[] bin = await _fileService.GetBinary(filePath);
            return new FileContentResult(bin, "application/octet-stream");
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
        [HttpPost]
        [Route("upload-image-news")]
        public async Task<ActionResult<object>> UploadImageNews()
        {
            try
            {
                string NameFile = "", filePath = "", FullPath = "";
                var FilesUpload = Request.Form.Files;
                string folder = Path.Combine("Upload\\NewsFile\\", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString());
                string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                foreach (var item in FilesUpload)
                {
                    NameFile = Path.GetFileName(item.FileName).Replace("+", "");
                    filePath = Path.Combine(folderPath, NameFile);
                    FullPath = Path.Combine(folder, NameFile);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                }
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"fullPaths", FullPath},
                };
                return new ResultApi { Success = ResultCode.OK, Result = new { data = json } };
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("upload-image-document")]
        public ActionResult<object> UploadImageDocument()
        {
            try
            {
                string NameFile = "", filePath = "", FullPath = "";
                var FilesUpload = Request.Form.Files;
                string folder = Path.Combine("Upload\\Document\\", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Minute.ToString());
                string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                foreach (var item in FilesUpload)
                {
                    NameFile = Path.GetFileName(item.FileName).Replace("+", "");
                    filePath = Path.Combine(folderPath, NameFile);
                    FullPath = Path.Combine(folder, NameFile);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                }
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"fullPaths", FullPath},
                };
                return new ResultApi { Success = ResultCode.OK, Result = new { data = json } };
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpPost]
        [Route("upload-image-introduce")]
        public ActionResult<object> UploadImageIntroduce()
        {
            try
            {
                string NameFile = "", filePath = "", FullPath = "";
                var FilesUpload = Request.Form.Files;
                string folder = Path.Combine("Upload\\Introduce\\", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Minute.ToString());
                string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                foreach (var item in FilesUpload)
                {
                    NameFile = Path.GetFileName(item.FileName).Replace("+", "");
                    filePath = Path.Combine(folderPath, NameFile);
                    FullPath = Path.Combine(folder, NameFile);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        item.CopyTo(stream);
                    }
                }
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"fullPaths", FullPath},
                };
                return new ResultApi { Success = ResultCode.OK, Result = new { data = json } };
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }





        [HttpPost]
        [Route("upload-file-chatbot")]
        public async Task<ActionResult<object>> UploadImageChatBot([FromForm] RequestSendFile request)
        {
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                List<ChatBotFile> files = new List<ChatBotFile>();
                string NameFile = "", filePath = "";

                var FilesUpload = Request.Form.Files;
                string folder = Path.Combine("Upload\\ChatBot\\" + request.RoomName);
                string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                foreach (var item in FilesUpload)
                {
                    NameFile = Path.GetFileName(item.FileName).Replace("+", "");
                    filePath = Path.Combine(folderPath, NameFile);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }

                    files.Add(new ChatBotFile()
                    {
                        Name = NameFile,
                        FileType = GetFileTypes.GetFileTypeInt(item.ContentType),
                        FilePath = decrypt.EncryptData(Path.Combine(folder, NameFile)),
                        FilePathUrl = Path.Combine(folder, NameFile)
                    });
                }

                // insert db
                int userId = 0;
                int unit = 0;
                try {
                    userId = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                    unit = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                }
                catch (Exception) { }
                

                // signIR
                string fullName = userId == 0 ? "Người dân" : new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                var room = await new BOTRoom(_appSetting).BOTRoomGetByName(request.RoomName);
                if (room == null) {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Room is not correct" };
                }
                DateTime dateSent = DateTime.Now;
                Message messageModel = new Message();
                messageModel.Content = JsonSerializer.Serialize(files);
                messageModel.FromId = userId == 0 ? "0" : userId.ToString();
                messageModel.From = fullName;
                messageModel.FromFullName = fullName;
                messageModel.To = request.RoomName;
                messageModel.Timestamp = ((DateTimeOffset)dateSent).ToUnixTimeSeconds().ToString();
                messageModel.Type = MessageTypes.File;
                messageModel.DateSend = DateTime.Now;

                // insert db

                await new BOTMessage(_appSetting).BOTMessageInsertDAO(messageModel.Content, userId, room.Id, fullName, "", DateTime.Now);

                await _hubContext.Clients.Group(messageModel.To).SendAsync("ReceiveMessageToGroup", messageModel);
                await _hubContext.Clients.All.SendAsync("OnNewMessage", room);
                // get all ơ đây



                return new ResultApi { Success = ResultCode.OK, Result = files };
            }
            catch (Exception ex)
            {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


    }
}
