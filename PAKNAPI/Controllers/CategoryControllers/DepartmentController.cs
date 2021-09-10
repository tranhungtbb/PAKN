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
    [Route("api/department")]
    [ApiController]
	[ValidateModel]
	public class DepartmentController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public DepartmentController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		/// <summary>
		/// xóa sở ngành
		/// </summary>
		/// <param name="_cADepartmentDeleteIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> CADepartmentDeleteBase(CADepartmentDeleteIN _cADepartmentDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentDelete(_appSetting).CADepartmentDeleteDAO(_cADepartmentDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách sở ngành
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		/// <param name="IsActived"></param>
		/// <param name="DepartmentGroupId"></param>
		/// <param name="Phone"></param>
		/// <param name="Email"></param>
		/// <param name="Address"></param>
		/// <param name="Fax"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-department-on-page")]
		public async Task<ActionResult<object>> CADepartmentGetAllOnPageBase()
		{
			try
			{
				List<CADepartmentGetAllOnPage> rsCADepartmentGetAllOnPage = await new CADepartmentGetAllOnPage(_appSetting).CADepartmentGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGetAllOnPage", rsCADepartmentGetAllOnPage},
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
		///  chi tiết sở ngành
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CADepartmentGetByIDBase(int? Id)
		{
			try
			{
				List<CADepartmentGetByID> rsCADepartmentGetByID = await new CADepartmentGetByID(_appSetting).CADepartmentGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGetByID", rsCADepartmentGetByID},
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
		/// thêm mới sở ngành
		/// </summary>
		/// <param name="_cADepartmentInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<ActionResult<object>> CADepartmentInsertBase(CADepartmentInsertIN _cADepartmentInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentInsert(_appSetting).CADepartmentInsertDAO(_cADepartmentInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập sở ngành
		/// </summary>
		/// <param name="_cADepartmentUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> CADepartmentUpdateBase(CADepartmentUpdateIN _cADepartmentUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentUpdate(_appSetting).CADepartmentUpdateDAO(_cADepartmentUpdateIN) };
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
