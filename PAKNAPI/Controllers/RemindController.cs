﻿using PAKNAPI.Common;
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
        [Route("RemindInsert")]
        public async Task<object> RMRemindInsert() {
            try {
                // insert vào Remind
                RMRemindInsertRequest _rMRemindInsert = new RMRemindInsertRequest();
                _rMRemindInsert.Model = JsonConvert.DeserializeObject<RMRemindModel>(Request.Form["Model"].ToString());
                _rMRemindInsert.Files = Request.Form.Files;
                int id =Int32.Parse((await new RMRemind(_appSetting).RMRemindInsert(_rMRemindInsert.Model)).ToString());
                if (id > 0)
                {
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
                            RMFileAttachModel file = new RMFileAttachModel();
                            file.RemindId = id;
                            file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                            string filePath = Path.Combine(folderPath, file.Name);
                            file.FileAttach = Path.Combine(folder, file.Name);
                            file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            await new RMFileAttach(_appSetting).RMFileAttachInsert(file);
                        }
                    }
                    // insert vào RMForward
                    RMForwardModel _rMForwardInsert = new RMForwardModel();
                    _rMForwardInsert.RemindId = id;
                    _rMForwardInsert.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                    _rMForwardInsert.SenderName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                    _rMForwardInsert.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                    _rMForwardInsert.ReceiveOrgId =Int32.Parse(Request.Form["SendOrgId"].ToString());
                    _rMForwardInsert.DateSend = DateTime.Now;
                    _rMForwardInsert.IsView = 1; // chưa biết là gì, auto để 1

                    int? insertForward = await new RMForward(_appSetting).RMFileAttachInsert(_rMForwardInsert);
                    if (insertForward < 0)
                    {
                        throw new ArgumentException("error while insert RMForward");
                    }

                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK, Message = "Success" };
                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Error" };
                }
            }
            catch(Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        #endregion RM_RemindInsert

        #region danh sách nhắc việc trong detail PAKN mà cơ quan mình nhận được hoặc mình là người gửi

        [HttpPost]
        [Authorize]
        [Route("RemindGetList")]
        public async Task<object> RMRemindGetAll(int? RecommnendationId , int? Org) {
            try
            {
                List<RMRemindObject> result = new List<RMRemindObject>();

                result = await new RMRemind(_appSetting).RMRemindGetList(RecommnendationId, Org, true);
                if (result.Count > 0) {
                    result.ForEach(async item =>
                    {
                        List<RMFileAttachModel> files = await new RMFileAttach(_appSetting).RMFileAttachGetByRemindID(item.Id);
                        item.Files = files;
                    });
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }

                return new ResultApi { Success = ResultCode.OK, Result = null };
            }
            catch (Exception ex) {
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }



        #endregion
    }
}
