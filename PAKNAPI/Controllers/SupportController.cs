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
using Microsoft.AspNetCore.Http;

namespace PAKNAPI.Controllers
{
    [Route("api/SYSupport")]
    [ApiController]
   
    public class SupportController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SupportController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

        [HttpGet]
        [Authorize]
        [Route("SYSupportGetByCategory")]
        public async Task<object> SYSupportGetAll(int? Category) {
            try {

                var result = await new SYSupportMenu(_appSetting).SYSupportMenuGetByCategoryDAO(Category);
                return new ResultApi { Success = ResultCode.OK, Result = result };
            }
            catch(Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("SYSupportInsert")]
        public async Task<object> SYSupportInsert()
        {
            try
            {
                SYSupportMenu model = JsonConvert.DeserializeObject<SYSupportMenu>(Request.Form["model"].ToString());
                IFormFileCollection File = Request.Form.Files;
                int id = Int16.Parse((await new SYSupportMenu(_appSetting).SYSupportMenuInsertDAO(model)).ToString());
                if (id > 0)
                {
                    if (File != null && File.Count > 0)
                    {
                        model.Id = id;
                        string folder = "Upload\\Suppport\\" + id;
                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        model.FileName = Path.GetFileName(File[0].FileName).Replace("+", "");
                        model.FilePath = Path.Combine(folder, model.FileName);
                        model.FileType = GetFileTypes.GetFileTypeInt(File[0].ContentType);
                        
                        string filePath = Path.Combine(folderPath, model.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            File[0].CopyTo(stream);
                        }
                        await new SYSupportMenu(_appSetting).SYSupportMenuUpdateDAO(model);
                    }
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK, Result = id };
                }
                else {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception());
                    return new ResultApi { Success = ResultCode.ORROR, Result = id };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("SYSupportUpdate")]
        public async Task<object> SYIntroduceUpdate() {
            try
            {
                SYSupportMenu model = JsonConvert.DeserializeObject<SYSupportMenu>(Request.Form["model"].ToString());
                var modelFirst = model;
                IFormFileCollection File = Request.Form.Files;
                string folder = "Upload\\Suppport\\" + model.Id;
                string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                if (File != null && File.Count > 0)
                {
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    model.FileName = Path.GetFileName(File[0].FileName).Replace("+", "");
                    model.FilePath = Path.Combine(folder, model.FileName);
                    model.FileType = GetFileTypes.GetFileTypeInt(File[0].ContentType);

                    string filePath = Path.Combine(folderPath, model.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        File[0].CopyTo(stream);
                    }
                }
                var s = await new SYSupportMenu(_appSetting).SYSupportMenuUpdateDAO(model);
                if (s > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK };
                }
                else {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception());
                    return new ResultApi { Success = ResultCode.ORROR, Result = s };
                }
                
                
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("SYSupportDetete")]
        public async Task<object> SYSupportDelete(SYSupportMenuDelete model)
        {
            try
            {
                var result = (int)await new SYSupportMenu(_appSetting).SYSupportMenuDeleteDAO(model);

                string folder = "Upload\\Suppport\\" + model.Id;


                // xóa hết
                string[] files = Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    System.IO.File.Delete(file);
                }
                Directory.Delete(folder);

                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = result };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

    }
}
