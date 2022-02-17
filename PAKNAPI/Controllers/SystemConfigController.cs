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
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using NSwag.Annotations;

namespace PAKNAPI.Controllers
{
    [Route("api/system-config")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Cấu hình hệ thống", Description = "Cấu hình hệ thống")]

	public class SystemConfigController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SystemConfigController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }
		/// <summary>
		/// danh sách cấu hình hệ thống - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-system-config-on-page")]
		public async Task<ActionResult<object>> SYConfigGetAllOnPageBase()
		{
			try
			{
				List<SYConfig> rsSYConfigGetAllOnPage = await new SYConfig(_appSetting).SYConfigGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYConfigGetAllOnPageBase", rsSYConfigGetAllOnPage},
						{"TotalCount", rsSYConfigGetAllOnPage != null && rsSYConfigGetAllOnPage.Count > 0 ? rsSYConfigGetAllOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// chi tiết cấu hình hệ thống - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> SYConfigGetByIDBase(int? Id)
		{
			try
			{
				List<SYConfig> rsSYConfigGetByID = await new SYConfig(_appSetting).SYConfigGetByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYConfigGetByID", rsSYConfigGetByID},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// chi tiết cấu hình hệ thống by type
		/// </summary>
		/// <param name="Type"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("get-by-type")]
		public async Task<ActionResult<object>> SYConfigGetByTypeBase(int? Type)
		{
			try
			{
				SYConfig rsSYConfigGetByType = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(Type)).FirstOrDefault();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYConfigGetByType", rsSYConfigGetByType},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



		/// <summary>
		/// cập nhập cấu hình hệ thống - Authorize
		/// </summary>
		/// <param name="_sYConfigUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("update")]
		public async Task<ActionResult<object>> SYConfigUpdateBase(SYConfig _sYConfigUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYConfig(_appSetting).SYConfigUpdateDAO(_sYConfigUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// xóa cấu hình thời gian - Authorize
		/// </summary>
		/// <param name="_sYTimeDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("sys-time-delete")]
		public async Task<ActionResult<object>> SYTimeDeleteBase(SYTimeDeleteIN _sYTimeDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTimeDelete(_appSetting).SYTimeDeleteDAO(_sYTimeDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách cấu hình thời gian - Authorize
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-sys-time-on-page")]
		public async Task<ActionResult<object>> SYTimeGetAllOnPageBase()
		{
			try
			{
				List<SYTimeGetAllOnPage> rsSYTimeGetAllOnPage = await new SYTimeGetAllOnPage(_appSetting).SYTimeGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYTimeGetAllOnPage", rsSYTimeGetAllOnPage},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết cấu hình thời gian - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("sys-time-get-by-id")]
		public async Task<ActionResult<object>> SYTimeGetByIDBase(int? Id)
		{
			try
			{
				List<SYTimeGetByID> rsSYTimeGetByID = await new SYTimeGetByID(_appSetting).SYTimeGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYTimeGetByID", rsSYTimeGetByID},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách thời gian cấu hình
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("sys-time-data-active")]
		public async Task<ActionResult<object>> SYTimeGetDateActiveBase()
		{
			try
			{
				List<SYTimeGetDateActive> rsSYTimeGetDateActive = await new SYTimeGetDateActive(_appSetting).SYTimeGetDateActiveDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYTimeGetDateActive", rsSYTimeGetDateActive},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới cấu hình thời gian - Authorize
		/// </summary>
		/// <param name="_sYTimeInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("sys-time-insert")]
		public async Task<ActionResult<object>> SYTimeInsertBase(SYTimeInsertIN _sYTimeInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTimeInsert(_appSetting).SYTimeInsertDAO(_sYTimeInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập cấu hình thời gian - Authorize
		/// </summary>
		/// <param name="_sYTimeUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("sys-time-update")]
		public async Task<ActionResult<object>> SYTimeUpdateBase(SYTimeUpdateIN _sYTimeUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTimeUpdate(_appSetting).SYTimeUpdateDAO(_sYTimeUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// danh sách template SMS - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-template-sms")]
		public async Task<ActionResult<object>> SYTemplateSMSGetAllOnPageBase(int? UnitId)
		{
			try
			{
				List<SYTemplateSMS> rsSYConfigGetAllOnPage = await new SYTemplateSMS(_appSetting).SYTemplateSMSGetAllOnPageDAO(UnitId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYTemplateSMS", rsSYConfigGetAllOnPage},
						{"TotalCount", rsSYConfigGetAllOnPage != null && rsSYConfigGetAllOnPage.Count > 0 ? rsSYConfigGetAllOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// thêm mới template SMS - Authorize
		/// </summary>
		/// <param name="update"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("template-sms-insert")]
		public async Task<ActionResult<object>> SYTemplateSMSInsertBase(SYTemplateSMS update)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTemplateSMS(_appSetting).SYTemplateSMSInsertDAO(update) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// cập nhập template SMS - Authorize
		/// </summary>
		/// <param name="update"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("template-sms-update")]
		public async Task<ActionResult<object>> SYTemplateSMSUpdateBase(SYTemplateSMS update)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTemplateSMS(_appSetting).SYTemplateSMSUpdateDAO(update) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// xóa template SMS - Authorize
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("template-sms-delete")]
		public async Task<ActionResult<object>> SYTemplateSMSDeleteBase(SYTemplateSMSDelete model)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTemplateSMS(_appSetting).SYTemplateSMSDeleteDAO(model) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


	}
}
