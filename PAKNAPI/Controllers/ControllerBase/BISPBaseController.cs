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
	[Route("api/BISPBase")]
	[ApiController]
	public class BISPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public BISPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("BIBusinessCheckExistsBase")]
		public async Task<ActionResult<object>> BIBusinessCheckExistsBase(string Field, string Value)
		{
			try
			{
				List<BIBusinessCheckExists> rsBIBusinessCheckExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO(Field, Value);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessCheckExists", rsBIBusinessCheckExists},
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
		[Route("BIBusinessGetDropdownBase")]
		public async Task<ActionResult<object>> BIBusinessGetDropdownBase()
		{
			try
			{
				List<BIBusinessGetDropdown> rsBIBusinessGetDropdown = await new BIBusinessGetDropdown(_appSetting).BIBusinessGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetDropdown", rsBIBusinessGetDropdown},
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
		[Route("BIBusinessGetRepresentativeByIdBase")]
		public async Task<ActionResult<object>> BIBusinessGetRepresentativeByIdBase(long? Id)
		{
			try
			{
				List<BIBusinessGetRepresentativeById> rsBIBusinessGetRepresentativeById = await new BIBusinessGetRepresentativeById(_appSetting).BIBusinessGetRepresentativeByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetRepresentativeById", rsBIBusinessGetRepresentativeById},
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
		[Route("BIBusinessGetRepresentativeEmailBase")]
		public async Task<ActionResult<object>> BIBusinessGetRepresentativeEmailBase(string Email)
		{
			try
			{
				List<BIBusinessGetRepresentativeEmail> rsBIBusinessGetRepresentativeEmail = await new BIBusinessGetRepresentativeEmail(_appSetting).BIBusinessGetRepresentativeEmailDAO(Email);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetRepresentativeEmail", rsBIBusinessGetRepresentativeEmail},
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
		[Route("BIBusinessInsertBase")]
		public async Task<ActionResult<object>> BIBusinessInsertBase(BIBusinessInsertIN _bIBusinessInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusinessInsert(_appSetting).BIBusinessInsertDAO(_bIBusinessInsertIN) };
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
		[Route("BIBusinessInsertListBase")]
		public async Task<ActionResult<object>> BIBusinessInsertListBase(List<BIBusinessInsertIN> _bIBusinessInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _bIBusinessInsertIN in _bIBusinessInsertINs)
				{
					var result = await new BIBusinessInsert(_appSetting).BIBusinessInsertDAO(_bIBusinessInsertIN);
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
		[Route("BIBusinessUpdateInfoBase")]
		public async Task<ActionResult<object>> BIBusinessUpdateInfoBase(BIBusinessUpdateInfoIN _bIBusinessUpdateInfoIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusinessUpdateInfo(_appSetting).BIBusinessUpdateInfoDAO(_bIBusinessUpdateInfoIN) };
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
		[Route("BIBusinessUpdateInfoListBase")]
		public async Task<ActionResult<object>> BIBusinessUpdateInfoListBase(List<BIBusinessUpdateInfoIN> _bIBusinessUpdateInfoINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _bIBusinessUpdateInfoIN in _bIBusinessUpdateInfoINs)
				{
					var result = await new BIBusinessUpdateInfo(_appSetting).BIBusinessUpdateInfoDAO(_bIBusinessUpdateInfoIN);
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
