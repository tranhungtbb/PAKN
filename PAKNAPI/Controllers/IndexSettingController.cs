using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.Models;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using System;
using Dapper;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Bugsnag;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using NSwag.Annotations;

namespace PAKNAPI.Controllers
{
    [Route("api/index-setting")]
    [ApiController]
    [ValidateModel]
    [OpenApiTag("Cài đặt trang chủ", Description = "Cài đặt trang chủ")]

    public class IndexSettingController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IndexSettingController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

        /// <summary>
        /// thông tin cấu hình trang chủ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("get-info")]
        public async Task<object> SYIndexSettingGetInfo() {
            try {
                IndexSettingModel result = new IndexSettingModel();

                List<SYIndexSetting> lstIntroduce = await new SYIndexSetting(_appSetting).SYIndexSettingGetInfoDAO();

                if (lstIntroduce.Count > 0) {
                    result.model = lstIntroduce.FirstOrDefault();
                    result.lstSYIndexWebsite = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteGetListDAO();
                    result.lstIndexSettingBanner = await new SYIndexSettingBanner(_appSetting).SYIndexSettingBannerGetByIndexSettingId(result.model.Id);

                    return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Success" };

                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Introduce not exit" };
                }
            }
            catch(Exception ex)
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// cập nhập cấu hình trang chủ - Authorize
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("update")]
        public async Task<object> IndexSettingUpdate() {
            try
            {
                var model = new IndexSettingModel();
                model.model = JsonConvert.DeserializeObject<SYIndexSetting>(Request.Form["model"].ToString());
                model.Files = Request.Form.Files;

                var ErrorMessage = ValidationForFormData.validObject(model.model);
                if (ErrorMessage != null)
                {
                    return StatusCode(200, new ResultApi
                    {
                        Success = ResultCode.ORROR,
                        Result = 0,
                        Message = ErrorMessage
                    });
                }


                // remove banner bị xóa

                if (model.Files != null && model.Files.Count > 0)
                {
                    string folder = "Upload\\BannerIndex\\BannerMain";
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // check bannner Main

                    var bannerMain = model.Files.Where(x => x.Name == "bannerMain").FirstOrDefault();
                    if (bannerMain != null) {
                        // xóa hết trong banner Main
                        string[] files = Directory.GetFiles(folder);
                        foreach (string file in files)
                        {
                            System.IO.File.Delete(file);
                        }

                        var nameImg = Path.GetFileName(bannerMain.FileName).Replace("+", "");
                        model.model.BannerUrl = Path.Combine(folder, nameImg);
                        string filePath = Path.Combine(folderPath, nameImg);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            bannerMain.CopyTo(stream);
                        }
                    }

                    // insert banner thêm mới
                    if (model.Files.Where(x => x.Name == "lstInsertBanner").ToList().Count > 0) {

                        folder = "Upload\\BannerIndex\\Banner";
                        folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        foreach (var item in model.Files.Where(x => x.Name == "lstInsertBanner"))
                        {
                            SYIndexSettingBanner file = new SYIndexSettingBanner();
                            file.IndexSystemId = model.model.Id;
                            file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                            string filePath = Path.Combine(folderPath, file.Name);
                            file.FileAttach = Path.Combine(folder, file.Name);
                            file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            await new SYIndexSettingBanner(_appSetting).SYIndexBannerInsertDAO(file);
                        }
                    }
                    

                }
                await new SYIndexSetting(_appSetting).SYIndexSettingUpdateDAO(model.model);
                // delete all website
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK, Result = null };
            }
            catch (Exception ex) {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// introduce unit
        /// 

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("get-list-index-website")]
        public async Task<object> SYIndexWebsiteGetAll()
        {
            try
            {
                var syIndexWebsiteGetAll = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteGetListDAO();

                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                foreach (var item in syIndexWebsiteGetAll)
                {
                    item.FilePathBase = decrypt.EncryptData(item.FilePath);
                }

                return new ResultApi { Success = ResultCode.OK, Result = syIndexWebsiteGetAll };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("index-website-delete")]
        public async Task<object> SYIndexWebsiteDelete(int Id)
        {
            try
            {
                var syIndexWebsiteGetAll = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteDeleteAllDAO(Id);

                return new ResultApi { Success = ResultCode.OK, Result = syIndexWebsiteGetAll };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        


        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("index-website-insert")]
        public async Task<object> SYIndexWebsiteInsert()
        {
            try
            {
                var model = JsonConvert.DeserializeObject<SYIndexWebsite>(Request.Form["model"].ToString());
                var files = Request.Form.Files;
                if (files == null || files.Count <= 0) {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "File is required" };
                }


                string folder = "Upload\\BannerIndex\\Banner";
                string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                model.FileName = Path.GetFileName(files[0].FileName).Replace("+", "");
                string filePath = Path.Combine(folderPath, files[0].FileName);
                model.FilePath = Path.Combine(folder, model.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    files[0].CopyTo(stream);
                }


                var result = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteInsertDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else
                {
                    return new ResultApi { Success = ResultCode.ORROR, Result = result };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("index-website-update")]
        public async Task<object> SYIndexWebsiteUpdate()
        {
            try
            {
                var model = JsonConvert.DeserializeObject<SYIndexWebsite>(Request.Form["model"].ToString());
                var files = Request.Form.Files;
                if (files.Count > 0)
                {
                    string folder = "Upload\\BannerIndex\\Banner";
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    model.FileName = Path.GetFileName(files[0].FileName).Replace("+", "");
                    string filePath = Path.Combine(folderPath, files[0].FileName);
                    model.FilePath = Path.Combine(folder, model.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        files[0].CopyTo(stream);
                    }
                }


                var result = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteUpdateDAO(model);

                return new ResultApi { Success = ResultCode.OK, Result = result };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        public bool deletefile(string fname)
        {
            try
            {
                string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, fname);
                //string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, "Invitation\\", fname);
                if ((System.IO.File.Exists(_imageToBeDeleted)))
                {
                    System.IO.File.Delete(_imageToBeDeleted);
                }
                return true;
            }
            catch (Exception ex) { return false; }
        }

    }
}
