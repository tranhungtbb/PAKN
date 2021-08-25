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
    [Route("api/department-group")]
    [ApiController]
	[ValidateModel]
	public class DepartmentGroupController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public DepartmentGroupController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		[HttpPost]
		[Authorize]
		[Route("delete")]
		public async Task<ActionResult<object>> CADepartmentGroupDeleteBase(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupDelete(_appSetting).CADepartmentGroupDeleteDAO(_cADepartmentGroupDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		//[HttpPost]
		//[Authorize]
		//[Route("CAWordDeleteBase")]
		//public async Task<ActionResult<object>> CAWordDeleteBase(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		//{
		//	try
		//	{
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

		//		return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupDelete(_appSetting).CAWordDeleteDAO(_cADepartmentGroupDeleteIN) };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}
		[HttpGet]
		[Authorize]
		[Route("get-list-department-group-on-page")]
		public async Task<ActionResult<object>> CADepartmentGroupGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CADepartmentGroupGetAllOnPage> rsCADepartmentGroupGetAllOnPage = await new CADepartmentGroupGetAllOnPage(_appSetting).CADepartmentGroupGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGroupGetAllOnPage", rsCADepartmentGroupGetAllOnPage},
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
		[Route("get-by-id")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("insert")]
		public async Task<ActionResult<object>> CADepartmentGroupInsertBase(CADepartmentGroupInsertIN _cADepartmentGroupInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupInsert(_appSetting).CADepartmentGroupInsertDAO(_cADepartmentGroupInsertIN) };
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
		[Route("update")]
		public async Task<ActionResult<object>> CADepartmentGroupUpdateBase(CADepartmentGroupUpdateIN _cADepartmentGroupUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupUpdate(_appSetting).CADepartmentGroupUpdateDAO(_cADepartmentGroupUpdateIN) };
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
