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
		[Route("INVFileAttachDeleteByIdBase")]
		public async Task<ActionResult<object>> INVFileAttachDeleteByIdBase(INVFileAttachDeleteByIdIN _iNVFileAttachDeleteByIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVFileAttachDeleteById(_appSetting).INVFileAttachDeleteByIdDAO(_iNVFileAttachDeleteByIdIN) };
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
		[Route("INVInvitationUserMapDeleteByInvitationIdBase")]
		public async Task<ActionResult<object>> INVInvitationUserMapDeleteByInvitationIdBase(INVInvitationUserMapDeleteByInvitationIdIN _iNVInvitationUserMapDeleteByInvitationIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationUserMapDeleteByInvitationId(_appSetting).INVInvitationUserMapDeleteByInvitationIdDAO(_iNVInvitationUserMapDeleteByInvitationIdIN) };
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
		[Route("INVInvitationUserMapGetByInvitationIdBase")]
		public async Task<ActionResult<object>> INVInvitationUserMapGetByInvitationIdBase(int? InvitationId)
		{
			try
			{
				List<INVInvitationUserMapGetByInvitationId> rsINVInvitationUserMapGetByInvitationId = await new INVInvitationUserMapGetByInvitationId(_appSetting).INVInvitationUserMapGetByInvitationIdDAO(InvitationId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"INVInvitationUserMapGetByInvitationId", rsINVInvitationUserMapGetByInvitationId},
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
		[Route("INVInvitationUserMapInsertBase")]
		public async Task<ActionResult<object>> INVInvitationUserMapInsertBase(INVInvitationUserMapInsertIN _iNVInvitationUserMapInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationUserMapInsert(_appSetting).INVInvitationUserMapInsertDAO(_iNVInvitationUserMapInsertIN) };
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

		[HttpGet]
		[Authorize]
		[Route("INVInvitationGetAllOnPageBase")]
		public async Task<ActionResult<object>> INVInvitationGetAllOnPageBase(int? PageSize, int? PageIndex, string Title, DateTime? StartDate, DateTime? EndDate, string Place, byte? Status)
		{
			try
			{
				List<INVInvitationGetAllOnPage> rsINVInvitationGetAllOnPage = await new INVInvitationGetAllOnPage(_appSetting).INVInvitationGetAllOnPageDAO(PageSize, PageIndex, Title, StartDate, EndDate, Place, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"INVInvitationGetAllOnPage", rsINVInvitationGetAllOnPage},
						{"TotalCount", rsINVInvitationGetAllOnPage != null && rsINVInvitationGetAllOnPage.Count > 0 ? rsINVInvitationGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsINVInvitationGetAllOnPage != null && rsINVInvitationGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsINVInvitationGetAllOnPage != null && rsINVInvitationGetAllOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize]
		[Route("INVInvitationGetByIdBase")]
		public async Task<ActionResult<object>> INVInvitationGetByIdBase(int? id)
		{
			try
			{
				List<INVInvitationGetById> rsINVInvitationGetById = await new INVInvitationGetById(_appSetting).INVInvitationGetByIdDAO(id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"INVInvitationGetById", rsINVInvitationGetById},
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
		[Route("INVInvitationInsertBase")]
		public async Task<ActionResult<object>> INVInvitationInsertBase(INVInvitationInsertIN _iNVInvitationInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationInsert(_appSetting).INVInvitationInsertDAO(_iNVInvitationInsertIN) };
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
		[Route("INVInvitationUpdateBase")]
		public async Task<ActionResult<object>> INVInvitationUpdateBase(INVInvitationUpdateIN _iNVInvitationUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationUpdate(_appSetting).INVInvitationUpdateDAO(_iNVInvitationUpdateIN) };
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
