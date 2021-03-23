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
	[Route("api/SYCaptChaSPBase")]
	[ApiController]
	public class SYCaptChaSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public SYCaptChaSPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		[HttpPost]
		[Authorize]
		[Route("SYCaptChaDeleteBase")]
		public async Task<ActionResult<object>> SYCaptChaDeleteBase(SYCaptChaDeleteIN _sYCaptChaDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptChaDelete(_appSetting).SYCaptChaDeleteDAO(_sYCaptChaDeleteIN) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYCaptChaDeleteListBase")]
		public async Task<ActionResult<object>> SYCaptChaDeleteListBase(List<SYCaptChaDeleteIN> _sYCaptChaDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYCaptChaDeleteIN in _sYCaptChaDeleteINs)
				{
					var result = await new SYCaptChaDelete(_appSetting).SYCaptChaDeleteDAO(_sYCaptChaDeleteIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return Ok(json);
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYCaptChaInsertDataBase")]
		public async Task<ActionResult<object>> SYCaptChaInsertDataBase(SYCaptChaInsertDataIN _sYCaptChaInsertDataIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptChaInsertData(_appSetting).SYCaptChaInsertDataDAO(_sYCaptChaInsertDataIN) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYCaptChaInsertDataListBase")]
		public async Task<ActionResult<object>> SYCaptChaInsertDataListBase(List<SYCaptChaInsertDataIN> _sYCaptChaInsertDataINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYCaptChaInsertDataIN in _sYCaptChaInsertDataINs)
				{
					var result = await new SYCaptChaInsertData(_appSetting).SYCaptChaInsertDataDAO(_sYCaptChaInsertDataIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
