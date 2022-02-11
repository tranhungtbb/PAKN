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
    [Route("api/word")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Từ ngữ cấm", Description = "Danh mục từ ngữ cấm - Authorize")]
	public class WordController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public WordController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }
		/// <summary>
		/// danh sách từ ngữ - Authorize
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-word-on-page")]
		public async Task<ActionResult<object>> CAWordGetAllOnPageBase()
		{
			try
			{
				List<CAWordGetAllOnPage> rsCAWordGetAllOnPage = await new CAWordGetAllOnPage(_appSetting).CAWordGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetAllOnPage", rsCAWordGetAllOnPage},
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
		/// danh sách từ ngữ theo nhóm từ -Authorize
		/// </summary>
		/// <param name="GroupId"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-word-on-page-by-group-id")]
		public async Task<ActionResult<object>> CAWordGetAllOnPageByGroupIdBase(int GroupId)
		{
			try
			{
				List<CAWordGetAllOnPage> rsCAWordGetAllOnPage = await new CAWordGetAllOnPage(_appSetting).CAWordGetAllOnPageByGroupIdDAO(GroupId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetAllOnPageByGroupId", rsCAWordGetAllOnPage},
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
		/// chi tiết từ ngữ - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAWordGetByIDBase(int? Id)
		{
			try
			{
				List<CAWordGetByID> rsCAWordGetByID = await new CAWordGetByID(_appSetting).CAWordGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetByID", rsCAWordGetByID},
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
		/// thêm mới từ ngữ - Authorize
		/// </summary>
		/// <param name="_cAWordInsertIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("insert")]
		public async Task<ActionResult<object>> CAWordInsertBase(CAWordInsertIN _cAWordInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				var result = await new CAWordInsert(_appSetting).CAWordInsertDAO(_cAWordInsertIN);

				if (result > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Thêm mới thành công" };
				}
				else if (result == -1)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Tiêu đề đã tồn tại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = result, Message = "Tiêu đề đã tồn tại" };
				}
				else {
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Thêm mới thất bại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = result, Message = "Thêm mới thất bại" };
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
		/// cập nhập từ ngữ
		/// </summary>
		/// <param name="_cAWordUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("update")]
		public async Task<ActionResult<object>> CAWordUpdateBase(CAWordUpdateIN _cAWordUpdateIN)
		{
			try
			{

				var result = await new CAWordUpdate(_appSetting).CAWordUpdateDAO(_cAWordUpdateIN);

				if (result > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Cập nhập thành công" };
				}
				else if (result == -1)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Tiêu đề đã bị trùng", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = result, Message = "Tiêu đề đã bị trùng" };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Cập nhập thất bại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = result, Message = "Cập nhập thất bại" };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		[HttpGet]
		[Route("get-list-suggest")]
		public async Task<ActionResult<object>> CA_WordGetListSuggestBase()
		{
			try
			{
				List<CA_WordGetListSuggest> rsCA_WordGetListSuggest = await new CA_WordGetListSuggest(_appSetting).CA_WordGetListSuggestDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetListSuggest", rsCA_WordGetListSuggest},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa từ ngữ
		/// </summary>
		/// <param name="_cADepartmentGroupDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("delete")]
		public async Task<ActionResult<object>> CAWordDeleteBase(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupDelete(_appSetting).CAWordDeleteDAO(_cADepartmentGroupDeleteIN) };
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
