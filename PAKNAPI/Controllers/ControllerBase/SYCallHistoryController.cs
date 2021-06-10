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
    public class SYCallHistoryController : BaseApiController
    {
		private readonly IAppSetting _appSetting;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IClient _bugsnag;
		public SYCallHistoryController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpGet]
		[Route("SYCallHistoryGetPagedList")]
		[Authorize]
		public async Task<ActionResult<object>> SYCallHistoryGetPagedList(int? type, string phone, int pageIndex = 1, int pageSize =20)
		{
			try
			{
				List<SYCallHistoryPagedList> rsData = await new SYCallHistoryPagedList(_appSetting).GetData(type,phone,pageIndex,pageSize);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"ListData", rsData},
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
	}
}
