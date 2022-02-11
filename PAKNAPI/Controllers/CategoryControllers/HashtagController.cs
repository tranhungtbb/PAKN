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
using NSwag.Annotations;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/hashtag")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Nhãn dữ liệu", Description = "Danh mục nhãn dữ liệu - Authorize")]
	public class HashtagController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public HashtagController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		/// <summary>
		/// danh sách hashtag - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-hashtag-on-page")]
		public async Task<ActionResult<object>> CAHashtagGetAllOnPage()
		{
			try
			{
				List<CAHashtagListPage> rsCAHashtagOnPage = await new CAHashtagListPage(_appSetting).CAHashtagGetAllOnPage();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtag", rsCAHashtagOnPage},
						{"TotalCount", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? rsCAHashtagOnPage[0].RowNumber : 0}
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
		/// danh sách hashtag
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-all")]
		public async Task<ActionResult<object>> CAHashtagGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagGetAll() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết hashtag - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAHashtagGetByIDBase(int? Id)
		{
			try
			{
				List<CAHashtagGetByID> rsCAHashtagGetByID = await new CAHashtagGetByID(_appSetting).CAHashtagGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetByID", rsCAHashtagGetByID},
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
		/// thêm mới hashtag - Authorize
		/// </summary>
		/// <param name="_cAHashtag"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("insert")]
		public async Task<ActionResult<object>> CAHashtagInsert(CAHashtag _cAHashtag)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagInsert(_cAHashtag) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// update hashtag - Authorize
		/// </summary>
		/// <param name="_cAHashtag"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("update")]
		public async Task<ActionResult<object>> CAHashtagUpdate(CAHashtag _cAHashtag)
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagUpdate(_cAHashtag);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,"Tiêu đề bị trùng" , new Exception());

					return new ResultApi { Success = ResultCode.ORROR, Result = count, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa hashtag - Authorize
		/// </summary>
		/// <param name="_cAHashtag"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("delete")]
		public async Task<ActionResult<object>> CAHashtagDelete(HashtagDelete _cAHashtag)
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagDelete(_cAHashtag);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
