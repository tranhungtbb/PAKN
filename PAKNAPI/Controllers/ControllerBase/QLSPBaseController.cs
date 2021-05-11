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

namespace PAKNAPI.ControllerBase
{
	[Route("api/QLSPBase")]
	[ApiController]
	public class QLSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public QLSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("QLDoanhNghiepInsertBase")]
		public async Task<ActionResult<object>> QLDoanhNghiepInsertBase(QLDoanhNghiepInsertIN _qLDoanhNghiepInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new QLDoanhNghiepInsert(_appSetting).QLDoanhNghiepInsertDAO(_qLDoanhNghiepInsertIN) };
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
		[Route("QLDoanhNghiepInsertListBase")]
		public async Task<ActionResult<object>> QLDoanhNghiepInsertListBase(List<QLDoanhNghiepInsertIN> _qLDoanhNghiepInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _qLDoanhNghiepInsertIN in _qLDoanhNghiepInsertINs)
				{
					var result = await new QLDoanhNghiepInsert(_appSetting).QLDoanhNghiepInsertDAO(_qLDoanhNghiepInsertIN);
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
		[Route("QLNguoiDanInsertBase")]
		public async Task<ActionResult<object>> QLNguoiDanInsertBase(QLNguoiDanInsertIN _qLNguoiDanInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new QLNguoiDanInsert(_appSetting).QLNguoiDanInsertDAO(_qLNguoiDanInsertIN) };
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
		[Route("QLNguoiDanInsertListBase")]
		public async Task<ActionResult<object>> QLNguoiDanInsertListBase(List<QLNguoiDanInsertIN> _qLNguoiDanInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _qLNguoiDanInsertIN in _qLNguoiDanInsertINs)
				{
					var result = await new QLNguoiDanInsert(_appSetting).QLNguoiDanInsertDAO(_qLNguoiDanInsertIN);
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
