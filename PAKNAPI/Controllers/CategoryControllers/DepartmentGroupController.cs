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
    [Route("api/department-group")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Nhóm sở ngành", Description = "Danh mục nhóm sở ngành - Authorize")]
	[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
	public class DepartmentGroupController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public DepartmentGroupController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }
		/// <summary>
		/// xóa nhóm sở ngành - Authorize
		/// </summary>
		/// <param name="_cADepartmentGroupDeleteIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("delete")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentGroupDeleteBase(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupDelete(_appSetting).CADepartmentGroupDeleteDAO(_cADepartmentGroupDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		
		/// <summary>
		/// danh sách nhóm sở ngành - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route("get-list-department-group-on-page")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentGroupGetAllOnPageBase()
		{
			try
			{
				List<CADepartmentGroupGetAllOnPage> rsCADepartmentGroupGetAllOnPage = await new CADepartmentGroupGetAllOnPage(_appSetting).CADepartmentGroupGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGroupGetAllOnPage", rsCADepartmentGroupGetAllOnPage},
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
		/// chi tiết nhóm sở ngành - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("get-by-id")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentGroupGetByIDBase(int? Id)
		{
			try
			{
				List<CADepartmentGroupGetByID> rsCADepartmentGroupGetByID = await new CADepartmentGroupGetByID(_appSetting).CADepartmentGroupGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGroupGetByID", rsCADepartmentGroupGetByID},
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
		/// thêm mới nhóm sở ngành - Authorize
		/// </summary>
		/// <param name="_cADepartmentGroupInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("insert")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentGroupInsertBase(CADepartmentGroupInsertIN _cADepartmentGroupInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				int result = Convert.ToInt32(await new CADepartmentGroupInsert(_appSetting).CADepartmentGroupInsertDAO(_cADepartmentGroupInsertIN));
				if (result > 0) {
					return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Thêm mới thành công" };
				}
				else {
					return new ResultApi { Success = ResultCode.ORROR, Result =  result, Message = "Đã tồn tại nhóm sở ngành"};
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
		/// cập nhập nhóm sở ngành - Authorize
		/// </summary>
		/// <param name="_cADepartmentGroupUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("update")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentGroupUpdateBase(CADepartmentGroupUpdateIN _cADepartmentGroupUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupUpdate(_appSetting).CADepartmentGroupUpdateDAO(_cADepartmentGroupUpdateIN) };
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
