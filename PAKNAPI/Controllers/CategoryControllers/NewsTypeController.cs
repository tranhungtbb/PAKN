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
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/news-type")]
    [ApiController]
	[ValidateModel]
	public class NewsTypeController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public NewsTypeController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }
		/// <summary>
		/// xóa kiểu bài viết
		/// </summary>
		/// <param name="_cANewsTypeDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> CANewsTypeDeleteBase(CANewsTypeDeleteIN _cANewsTypeDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeDelete(_appSetting).CANewsTypeDeleteDAO(_cANewsTypeDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách kiểu bài viết
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		/// <param name="IsActived"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-news-type-on-page")]
		public async Task<ActionResult<object>> CANewsTypeGetAllOnPageBase()
		{
			try
			{
				List<CANewsTypeGetAllOnPage> rsCANewsTypeGetAllOnPage = await new CANewsTypeGetAllOnPage(_appSetting).CANewsTypeGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsTypeGetAllOnPage", rsCANewsTypeGetAllOnPage},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// chi tiết kiểu bài viết
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CANewsTypeGetByIDBase(int? Id)
		{
			try
			{
				List<CANewsTypeGetByID> rsCANewsTypeGetByID = await new CANewsTypeGetByID(_appSetting).CANewsTypeGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsTypeGetByID", rsCANewsTypeGetByID},
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
		/// thêm mới
		/// </summary>
		/// <param name="_cANewsTypeInsertIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<ActionResult<object>> CANewsTypeInsertBase(CANewsTypeInsertIN _cANewsTypeInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeInsert(_appSetting).CANewsTypeInsertDAO(_cANewsTypeInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập
		/// </summary>
		/// <param name="_cANewsTypeUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> CANewsTypeUpdateBase(CANewsTypeUpdateIN _cANewsTypeUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeUpdate(_appSetting).CANewsTypeUpdateDAO(_cANewsTypeUpdateIN) };
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
