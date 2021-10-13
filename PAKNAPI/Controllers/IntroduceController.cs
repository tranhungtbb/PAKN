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

namespace PAKNAPI.Controllers
{
    [Route("api/introduce")]
    [ApiController]
    [ValidateModel]
    public class IntroduceController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IntroduceController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

        /// <summary>
        /// thông tin cấu hình trang giới thiệu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize("ThePolicy")]
        [Route("get-by-id")]
        public async Task<object> SYIntroduceGetInfo() {
            try {
                IntroduceModel result = new IntroduceModel();

                List<SYIntroduce> lstIntroduce = await new SYIntroduce(_appSetting).SYIntroduceGetInfoDAO();

                if (lstIntroduce.Count > 0) {
                    result.model = lstIntroduce.FirstOrDefault();
                    result.lstIntroduceFunction = await new SYIntroduceFunction(_appSetting).SYIntroduceFunctionGetByIntroductId(result.model.Id);
                    result.lstIntroduceUnit = await new SYIntroduceUnit(_appSetting).SYIntroduceUnitGetByIntroduceId(result.model.Id);

                    //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
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
        /// cập nhập cấu hình trang giới thiệu
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("update")]
        public async Task<object> SYIntroduceUpdate() {
            try
            {
                var model = new IntroduceModel();
                model.model = JsonConvert.DeserializeObject<SYIntroduce>(Request.Form["model"].ToString());

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

                model.lstIntroduceFunction = JsonConvert.DeserializeObject<List<SYIntroduceFunction>>(Request.Form["lstIntroduceFunction"].ToString());
                model.Files = Request.Form.Files;

                if (model.Files != null && model.Files.Count > 0)
                {
                    // banner
                    string folder = "Upload\\Introduce\\BannerIntroduce\\";
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    var banner = model.Files.Where(x => x.Name == "BannerImg").FirstOrDefault();
                    if (banner != null) {
                        // xóa hết
                        string[] files = Directory.GetFiles(folder);
                        foreach (string file in files)
                        {
                            System.IO.File.Delete(file);
                        }

                        var nameImg = Path.GetFileName(banner.FileName).Replace("+", "");
                        model.model.BannerUrl = Path.Combine(folder, nameImg);
                        string filePath = Path.Combine(folderPath, nameImg);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                           banner.CopyTo(stream);
                        }
                    }

                    var ltsIcon = model.Files.Where(x => x.Name == "ltsIcon").ToList();
                    if (ltsIcon.Count > 0) {
                        folder = "Upload\\Introduce\\IconIntroduce\\";
                        folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        foreach (var icon in ltsIcon) {
                            foreach (var item in model.lstIntroduceFunction) {
                                if (item.IconNew != null && icon.FileName.Contains(item.IconNew)) {
                                    var nameImg = Path.GetFileName(icon.FileName).Replace("+", "");
                                    item.Icon = Path.Combine(folder, nameImg);
                                    string filePath = Path.Combine(folderPath, nameImg);
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        icon.CopyTo(stream);
                                    }
                                }
                            }
                        }
                    }
                }
                model.model.UpdateDate = DateTime.Now;
                await new SYIntroduce(_appSetting).SYIntroduceUpdateDAO(model.model);
                foreach (var item in model.lstIntroduceFunction) {
                    // insert file nữa này
                    await new SYIntroduceFunction(_appSetting).SYIntroduceFunctionUpdateDAO(item);
                }

                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK, Result = null };
            }
            catch (Exception ex) {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách đơn vị trang giới thiệu
        /// </summary>
        /// <param name="IntroduceId"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-introduce-unit-on-page")]
        public async Task<object> SYIntroduceUnitGetOnPage(int? IntroduceId, int? PageSize, int? PageIndex)
        {
            try
            {
                var syIntroduceUnitGetOnPage = await new SYIntroduceUnit(_appSetting).SYIntroduceUnitGetOnPageByIntroduceId(IntroduceId,PageSize,PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"SYIntroduceUnitGetOnPage", syIntroduceUnitGetOnPage},
                        {"TotalCount", syIntroduceUnitGetOnPage != null && syIntroduceUnitGetOnPage.Count > 0 ? syIntroduceUnitGetOnPage[0].RowNumber : 0},
                        {"PageIndex", syIntroduceUnitGetOnPage != null && syIntroduceUnitGetOnPage.Count > 0 ? PageIndex : 0},
                        {"PageSize", syIntroduceUnitGetOnPage != null && syIntroduceUnitGetOnPage.Count > 0 ? PageSize : 0},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// chi tiết đơn vị trang giới thiệu
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("introduce-unit-get-by-id")]
        public async Task<object> SYIntroduceUnitGetById(int? Id)
        {
            try
            {
                var syIntroduceUnit = await new SYIntroduceUnit(_appSetting).SYIntroduceUnitGetById(Id);
                
                return new ResultApi { Success = ResultCode.OK, Result = syIntroduceUnit };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// thêm mới đơn vị trang giới thiệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("introduce-unit-insert")]
        public async Task<object> SYIntroduceUnitInsert(SYIntroduceUnit model)
        {
            try
            {
                if (model.Index == null) { model.Index = 0; };
                var result  = (int)await new SYIntroduceUnit(_appSetting).SYIntroduceUnitInsertDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR,  Result = result};
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// cập nhập đơn vị trang giới thiệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("introduce-unit-update")]
        public async Task<object> SYIntroduceUnitUpdate(SYIntroduceUnit model)
        {
            try
            {
                if (model.Index == null) { model.Index = 0; };
                var result = (int)await new SYIntroduceUnit(_appSetting).SYIntroduceUnitUpdateDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else
                {
                    return new ResultApi { Success = ResultCode.ORROR, Result = result };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// xóa đơn vị trang giới thiệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("introduce-unit-delete")]
        public async Task<object> SYIntroduceUnitDelete(SYIntroduceUnitDelete model)
        {
            try
            {
                var result = (int)await new SYIntroduceUnit(_appSetting).SYIntroduceUnitDeleteDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else
                {
                    return new ResultApi { Success = ResultCode.ORROR, Result = result };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

    }
}
