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

namespace PAKNAPI.Controllers
{
    [Route("api/RMRemind")]
    [ApiController]
   
    public class RemindController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public RemindController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

        #region RM_RemindInsert

        [HttpPost]
        [Authorize]
        public async Task<object> RMRemindInsert(RMRemindInsertRequest _rMRemindInsert) {
            try {
                // insert vào Remind
                int id = int.Parse((await new RMRemindModelDAO(_appSetting).RMRemindInsert(_rMRemindInsert.Model)).ToString());
                if (id > 0) {
                    // insert vào RM_FileAttach

                    if (_rMRemindInsert.Files != null && _rMRemindInsert.Files.Count > 0)
                    {
                        string folder = "Upload\\Remind\\";
                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        foreach (var item in _rMRemindInsert.Files)
                        {
                            RMFileAttach file = new RMFileAttach();
                            file.RemindId = id;
                            file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                            string filePath = Path.Combine(folderPath, file.Name);
                            file.FileAttach = Path.Combine(folder, file.Name);
                            file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            await new RMFileAttachDAO(_appSetting).RMFileAttachInsert(file);
                        }
                    }


                    //_rMRemindInsert.ltsFiles.ForEach(async item =>
                    //{
                    //    item.RemindId = id;
                    //    int? insertFileAttach = await new RMFileAttachDAO(_appSetting).RMFileAttachInsert(item);
                    //    if (insertFileAttach < 0) { throw new ArgumentException("Error while insert RMFileAttach"); }
                    //});

                }
                // insert vào RMForward
                _rMRemindInsert.Forward.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                _rMRemindInsert.Forward.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                _rMRemindInsert.Forward.DateSend = DateTime.Now;
                _rMRemindInsert.Forward.IsView = 1; // chưa biết là gì, auto để 1

                int? insertForward = await new RMForwardDAO(_appSetting).RMFileAttachInsert(_rMRemindInsert.Forward);
                if (insertForward < 0) {
                    throw new ArgumentException("error while insert RMForward");
                }

                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK };
            }
            catch(Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        #endregion RM_RemindInsert
    }
}
