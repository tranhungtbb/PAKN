using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers.ControllerBase
{
	[Route("api/call-history")]
	[ApiController]
	public class CallHistoryController : BaseApiController
    {
		private readonly IAppSetting _appSetting;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IClient _bugsnag;
		public CallHistoryController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_hostingEnvironment = hostingEnvironment;
			_bugsnag = bugsnag;
		}
		/// <summary>
		/// danh sách lịch sử hê thống
		/// </summary>
		/// <param name="type"></param>
		/// <param name="phone"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("get-list-call-history-on-page")]
		[Authorize("ThePolicy")]
		public async Task<ActionResult<object>> SYCallHistoryGetPagedList(int? type, string phone, int pageIndex = 1, int pageSize =20)
		{
			try
			{
				List<SYCallHistoryPagedList> rsData = await new SYCallHistoryPagedList(_appSetting).GetData(type,phone,pageIndex,pageSize);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"ListData", rsData},
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// xóa lịch sử hê thống
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("delete")]
		[Authorize("ThePolicy")]
		public async Task<ActionResult<object>> Delete(long id)
		{
			try
			{
				var a =  await new SYCallHistoryPagedList(_appSetting).Delete(id);
				
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = null };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
