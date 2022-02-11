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
    [Route("api/position")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Chức vụ", Description = "Danh mục chức vụ - Authorize")]
	public class PositionController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public PositionController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }
		/// <summary>
		/// xóa chức vụ - Authorize
		/// </summary>
		/// <param name="_cAPositionDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("delete")]
		public async Task<ActionResult<object>> CAPositionDeleteBase(CAPositionDeleteIN _cAPositionDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAPositionDelete(_appSetting).CAPositionDeleteDAO(_cAPositionDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// danh sách chức vụ - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-position-on-page")]
		public async Task<ActionResult<object>> CAPositionGetAllOnPageBase()
		{
			try
			{
				List<CAPositionGetAllOnPage> rsCAPositionGetAllOnPage = await new CAPositionGetAllOnPage(_appSetting).CAPositionGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAPositionGetAllOnPage", rsCAPositionGetAllOnPage},
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
		/// chi tiết - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAPositionGetByIDBase(int? Id)
		{
			try
			{
				List<CAPositionGetByID> rsCAPositionGetByID = await new CAPositionGetByID(_appSetting).CAPositionGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAPositionGetByID", rsCAPositionGetByID},
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

		//[HttpGet]
		//[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		//[Route("CAPositionGetDropdownBase")]
		//public async Task<ActionResult<object>> CAPositionGetDropdownBase()
		//{
		//	try
		//	{
		//		List<CAPositionGetDropdown> rsCAPositionGetDropdown = await new CAPositionGetDropdown(_appSetting).CAPositionGetDropdownDAO();
		//		IDictionary<string, object> json = new Dictionary<string, object>
		//			{
		//				{"CAPositionGetDropdown", rsCAPositionGetDropdown},
		//			};
		//		return new ResultApi { Success = ResultCode.OK, Result = json };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}

		/// <summary>
		/// thêm mới chức vụ - Authorize
		/// </summary>
		/// <param name="_cAPositionInsertIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("insert")]
		public async Task<ActionResult<object>> CAPositionInsertBase(CAPositionInsertIN _cAPositionInsertIN)
		{
			try
			{
				var result = Convert.ToInt16(await new CAPositionInsert(_appSetting).CAPositionInsertDAO(_cAPositionInsertIN));
				if (result == -1) {
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Tiêu đề đã tồn tại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tiêu đề đã tồn tại" };
                }
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result =  result};
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		/// <summary>
		/// cập nhập chức vụ - Authorize
		/// </summary>
		/// <param name="_cAPositionUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("update")]
		public async Task<ActionResult<object>> CAPositionUpdateBase(CAPositionUpdateIN _cAPositionUpdateIN)
		{
			try
			{
				var result = Convert.ToInt16(await new CAPositionUpdate(_appSetting).CAPositionUpdateDAO(_cAPositionUpdateIN));
				if (result == -1)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Tiêu đề đã tồn tại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Message = "Tiêu đề đã tồn tại" };
				}
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = result };
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
