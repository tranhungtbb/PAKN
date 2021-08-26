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

namespace PAKNAPI.Controllers
{
    [Route("api/system-config")]
    [ApiController]
	[ValidateModel]

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
		/// danh sách cấu hình hệ thống
		/// </summary>
		/// <param name="Title"></param>
		/// <param name="Description"></param>
		/// <param name="Type"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-system-config-on-page")]
		public async Task<ActionResult<object>> SYConfigGetAllOnPageBase(string Title, string Description, int? Type, int? PageSize, int? PageIndex)
		{
			try
			{
				List<SYConfig> rsSYConfigGetAllOnPage = await new SYConfig(_appSetting).SYConfigGetAllOnPageDAO(Title, Description, Type, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYConfigGetAllOnPageBase", rsSYConfigGetAllOnPage},
						{"TotalCount", rsSYConfigGetAllOnPage != null && rsSYConfigGetAllOnPage.Count > 0 ? rsSYConfigGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYConfigGetAllOnPage != null && rsSYConfigGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYConfigGetAllOnPage != null && rsSYConfigGetAllOnPage.Count > 0 ? PageSize : 0},
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

		/// <summary>
		/// chi tiết cấu hình hệ thống
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập cấu hình hệ thống
		/// </summary>
		/// <param name="_sYConfigUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> SYConfigUpdateBase(SYConfig _sYConfigUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYConfig(_appSetting).SYConfigUpdateDAO(_sYConfigUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// xóa cấu hình thời gian
		/// </summary>
		/// <param name="_sYTimeDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("sys-time-delete")]
		public async Task<ActionResult<object>> SYTimeDeleteBase(SYTimeDeleteIN _sYTimeDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTimeDelete(_appSetting).SYTimeDeleteDAO(_sYTimeDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách cấu hình thời gian
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Name"></param>
		/// <param name="Code"></param>
		/// <param name="Time"></param>
		/// <param name="Description"></param>
		/// <param name="IsActived"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-sys-time-on-page")]
		public async Task<ActionResult<object>> SYTimeGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Code, DateTime? Time, string Description, bool? IsActived)
		{
			try
			{
				List<SYTimeGetAllOnPage> rsSYTimeGetAllOnPage = await new SYTimeGetAllOnPage(_appSetting).SYTimeGetAllOnPageDAO(PageSize, PageIndex, Name, Code, Time, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYTimeGetAllOnPage", rsSYTimeGetAllOnPage},
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
		/// <summary>
		/// chi tiết cấu hình thời gian
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách thời gian cấu hình
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới cấu hình thời gian
		/// </summary>
		/// <param name="_sYTimeInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("sys-time-insert")]
		public async Task<ActionResult<object>> SYTimeInsertBase(SYTimeInsertIN _sYTimeInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTimeInsert(_appSetting).SYTimeInsertDAO(_sYTimeInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập cấu hình thời gian
		/// </summary>
		/// <param name="_sYTimeUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("sys-time-update")]
		public async Task<ActionResult<object>> SYTimeUpdateBase(SYTimeUpdateIN _sYTimeUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYTimeUpdate(_appSetting).SYTimeUpdateDAO(_sYTimeUpdateIN) };
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
