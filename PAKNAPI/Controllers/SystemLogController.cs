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
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PAKNAPI.ControllerBase
{
	[Route("api/system-log")]
	[ApiController]
	public class SystemLogController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SystemLogController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-system-log-on-page")]
		public async Task<ActionResult<object>> SYSystemLogGetAllOnPageBase(int? UserId, int? PageSize, int? PageIndex, DateTime? FromDate, DateTime? ToDate, string Content, int? Status)
		{
			try
			{
				List<SYSystemLogGetAllOnPage> rsSYSystemLogGetAllOnPage = await new SYSystemLogGetAllOnPage(_appSetting).SYSystemLogGetAllOnPageDAO(UserId, PageSize, PageIndex, FromDate, ToDate, Content, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYSystemLogGetAllOnPage", rsSYSystemLogGetAllOnPage},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-system-log-admin-on-page")]
		public async Task<ActionResult<object>> SYSystemLogGetAllOnPageAdminBase(int? UserId, int? PageSize, int? PageIndex, DateTime? CreateDate, byte? Status, string Description)
		{
			try
			{
				List<SYSystemLogGetAllOnPageAdmin> rsSYSystemLogGetAllOnPageAdmin = await new SYSystemLogGetAllOnPageAdmin(_appSetting).SYSystemLogGetAllOnPageAdminDAO(UserId, PageSize, PageIndex, CreateDate, Status, Description);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYSystemLogGetAllOnPageAdmin", rsSYSystemLogGetAllOnPageAdmin},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> SYSystemLogDeleteBase(int? Id)
		{
			try
			{
				SYSystemLogDeleteIN _sYSystemLogDeleteIN = new SYSystemLogDeleteIN();
				_sYSystemLogDeleteIN.Id = Id;
				return new ResultApi { Success = ResultCode.OK, Result = await new SYSystemLogDelete(_appSetting).SYSystemLogDeleteDAO(_sYSystemLogDeleteIN) };
			}
			catch (Exception ex)
			{
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


	}
}
