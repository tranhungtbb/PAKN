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
    [Route("api/INVInvitation")]
    [ApiController]
   
    public class InvitationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public InvitationController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

		[HttpPost]
		[Authorize]
		[Route("INVInvitationDelete")]
		public async Task<ActionResult<object>> INVInvitationDeleteBase(INVInvitationDeleteIN _iNVInvitationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				// delete file
				List <INVFileAttachGetAllByInvitationId> rsINVFileAttachGetAllByInvitationId = await new INVFileAttachGetAllByInvitationId(_appSetting).INVFileAttachGetAllByInvitationIdDAO(_iNVInvitationDeleteIN.Id);

				foreach (INVFileAttachGetAllByInvitationId item in rsINVFileAttachGetAllByInvitationId) {
					// xóa file trong folder
					deletefile(item.FileAttach);
				}
				// xóa trong db
				INVFileAttachDeleteByInvitationIdIN model = new INVFileAttachDeleteByInvitationIdIN();
				model.InvitationId = _iNVInvitationDeleteIN.Id;
				await new INVFileAttachDeleteByInvitationId(_appSetting).INVFileAttachDeleteByInvitationIdDAO(model);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationDelete(_appSetting).INVInvitationDeleteDAO(_iNVInvitationDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
