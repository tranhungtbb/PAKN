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
    public class DepartmentController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public DepartmentController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }


		[HttpPost]
		[Authorize]
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

		[HttpGet]
		[Authorize]
		[Route("get-list-department-on-page")]
		public async Task<ActionResult<object>> CADepartmentGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived, int? DepartmentGroupId, string Phone, string Email, string Address, string Fax)
		{
			try
			{
				List<CADepartmentGetAllOnPage> rsCADepartmentGetAllOnPage = await new CADepartmentGetAllOnPage(_appSetting).CADepartmentGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived, DepartmentGroupId, Phone, Email, Address, Fax);
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

		[HttpGet]
		[Authorize]
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



		[HttpPost]
		[Authorize]
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

		[HttpPost]
		[Authorize]
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
