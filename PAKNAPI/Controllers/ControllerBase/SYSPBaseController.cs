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

namespace PAKNAPI.ControllerBase
{
	[Route("api/SYSPBase")]
	[ApiController]
	public class SYSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public SYSPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		[HttpGet]
		[Authorize]
		[Route("SYRoleGetAllBase")]
		public async Task<ActionResult<object>> SYRoleGetAllBase()
		{
			try
			{
				List<SYRoleGetAll> rsSYRoleGetAll = await new SYRoleGetAll(_appSetting).SYRoleGetAllDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYRoleGetAll", rsSYRoleGetAll},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
