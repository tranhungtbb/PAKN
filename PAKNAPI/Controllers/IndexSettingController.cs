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

        //'SYIntroduce/IntroduceGetInfo'
        /// <summary>
        /// thông tin cấu hình trang chủ
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("ThePolicy")]
        [Route("get-info")]
        public async Task<object> SYIndexSettingGetInfo() {
            try {
                IndexSettingModel result = new IndexSettingModel();

                List<SYIndexSetting> lstIntroduce = await new SYIndexSetting(_appSetting).SYIndexSettingGetInfoDAO();

                if (lstIntroduce.Count > 0) {
                    result.model = lstIntroduce.FirstOrDefault();
                    result.lstSYIndexWebsite = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteGetByIndexSettingId(result.model.Id);
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
        /// cập nhập cấu hình trang chủ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("update")]
        public async Task<object> IndexSettingUpdate() {
            try
            {
                var model = new IndexSettingModel();
                model.model = JsonConvert.DeserializeObject<SYIndexSetting>(Request.Form["model"].ToString());
                model.lstSYIndexWebsite = JsonConvert.DeserializeObject<List<SYIndexWebsite>>(Request.Form["ltsIndexWebsite"].ToString());
                model.lstRemoveBanner = JsonConvert.DeserializeObject<List<SYIndexSettingBanner>>(Request.Form["lstRemoveBanner"].ToString());
                model.Files = Request.Form.Files;

                var ErrorMessage = ValidationForFormData.validObject(model.model);
                if (ErrorMessage != null)
                {
                    return StatusCode(400, new ResultApi
                    {
                        Success = ResultCode.ORROR,
                        Result = 0,
                        Message = ErrorMessage
                    });
                }


                // remove banner bị xóa
                if (model.lstRemoveBanner.Count > 0)
                {
                    foreach (var item in model.lstRemoveBanner)
                    {
                        deletefile(item.FileAttach);
                        SYIntroduceUnitDelete fileDel = new SYIntroduceUnitDelete();
                        fileDel.Id = item.Id;
                        await new SYIndexSettingBanner(_appSetting).SYIndexBannerDeleteDAO(fileDel);
                    }

                }

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
                await new SYIndexWebsite(_appSetting).SY_IndexWebsiteDeleteAllDAO();

                foreach (var item in model.lstSYIndexWebsite) {
                    // insert file nữa này
                    await new SYIndexWebsite(_appSetting).SY_IndexWebsiteInsertDAO(item);
                }

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

        //[HttpGet]
        //[Authorize("ThePolicy")]
        //[Route("get-list-index-website")]
        //public async Task<object> SYIndexWebsiteGetAll()
        //{
        //    try
        //    {
        //        var syIndexWebsiteGetAll = await new SYIndexWebsite(_appSetting).SY_IndexWebsiteDeleteAllDAO();
               
        //        return new ResultApi { Success = ResultCode.OK, Result = syIndexWebsiteGetAll };
        //    }
        //    catch (Exception ex)
        //    {
        //        new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
        //        return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
        //    }
        //}


        //[HttpPost]
        //[Authorize("ThePolicy")]
        //[Route("index-website-insert")]
        //public async Task<object> SYIndexWebsiteInsert(SYIndexWebsite model)
        //{
        //    try
        //    {
        //        var result  = (int)await new SYIndexWebsite(_appSetting).SY_IndexWebsiteInsertDAO(model);

        //        if (result > 0)
        //        {
        //            new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
        //            return new ResultApi { Success = ResultCode.OK, Result = result };
        //        }
        //        else {
        //            return new ResultApi { Success = ResultCode.ORROR,  Result = result};
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
        //        return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
        //    }
        //}

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
