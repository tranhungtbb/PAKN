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
	[Route("api/INVSPBase")]
	[ApiController]
	public class INVSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public INVSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("INVFileAttachDeleteByInvitationIdBase")]
		public async Task<ActionResult<object>> INVFileAttachDeleteByInvitationIdBase(INVFileAttachDeleteByInvitationIdIN _iNVFileAttachDeleteByInvitationIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVFileAttachDeleteByInvitationId(_appSetting).INVFileAttachDeleteByInvitationIdDAO(_iNVFileAttachDeleteByInvitationIdIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("INVFileAttachDeleteByInvitationIdListBase")]
		public async Task<ActionResult<object>> INVFileAttachDeleteByInvitationIdListBase(List<INVFileAttachDeleteByInvitationIdIN> _iNVFileAttachDeleteByInvitationIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _iNVFileAttachDeleteByInvitationIdIN in _iNVFileAttachDeleteByInvitationIdINs)
				{
					var result = await new INVFileAttachDeleteByInvitationId(_appSetting).INVFileAttachDeleteByInvitationIdDAO(_iNVFileAttachDeleteByInvitationIdIN);
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

		[HttpGet]
		[Authorize]
		[Route("INVFileAttachGetAllByInvitationIdBase")]
		public async Task<ActionResult<object>> INVFileAttachGetAllByInvitationIdBase(int? InvitationId)
		{
			try
			{
				List<INVFileAttachGetAllByInvitationId> rsINVFileAttachGetAllByInvitationId = await new INVFileAttachGetAllByInvitationId(_appSetting).INVFileAttachGetAllByInvitationIdDAO(InvitationId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"INVFileAttachGetAllByInvitationId", rsINVFileAttachGetAllByInvitationId},
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
		[Authorize]
		[Route("INVFileAttachInsertBase")]
		public async Task<ActionResult<object>> INVFileAttachInsertBase(INVFileAttachInsertIN _iNVFileAttachInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVFileAttachInsert(_appSetting).INVFileAttachInsertDAO(_iNVFileAttachInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("INVFileAttachInsertListBase")]
		public async Task<ActionResult<object>> INVFileAttachInsertListBase(List<INVFileAttachInsertIN> _iNVFileAttachInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _iNVFileAttachInsertIN in _iNVFileAttachInsertINs)
				{
					var result = await new INVFileAttachInsert(_appSetting).INVFileAttachInsertDAO(_iNVFileAttachInsertIN);
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
		[Authorize]
		[Route("INVInvitationDeleteBase")]
		public async Task<ActionResult<object>> INVInvitationDeleteBase(INVInvitationDeleteIN _iNVInvitationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationDelete(_appSetting).INVInvitationDeleteDAO(_iNVInvitationDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("INVInvitationDeleteListBase")]
		public async Task<ActionResult<object>> INVInvitationDeleteListBase(List<INVInvitationDeleteIN> _iNVInvitationDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _iNVInvitationDeleteIN in _iNVInvitationDeleteINs)
				{
					var result = await new INVInvitationDelete(_appSetting).INVInvitationDeleteDAO(_iNVInvitationDeleteIN);
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
