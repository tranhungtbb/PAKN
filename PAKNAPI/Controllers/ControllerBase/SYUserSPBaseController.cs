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
	[Route("api/SYUserSPBase")]
	[ApiController]
	public class SYUserSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public SYUserSPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		[HttpGet]
		[Authorize]
		[Route("SYUSRGetPermissionByUserIdBase")]
		public async Task<ActionResult<object>> SYUSRGetPermissionByUserIdBase(long? UserId)
		{
			try
			{
				List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId = await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUSRGetPermissionByUserId", rsSYUSRGetPermissionByUserId},
					};
				return Ok(json);
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
