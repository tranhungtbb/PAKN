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
using PAKNAPI.Models.Statistic;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/unit")]
    [ApiController]
	[ValidateModel]
	public class UnitController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public UnitController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		[HttpGet]
		[Route("get-all")]
		public async Task<ActionResult<object>> CAUnitGetAllBase(int? ParentId, byte? UnitLevel)
		{
			try
			{
				List<CAUnitGetAll> rsCAUnitGetAll = await new CAUnitGetAll(_appSetting).CAUnitGetAllDAO(ParentId, UnitLevel);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitGetAll", rsCAUnitGetAll},
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
		[Route("get-list-unit-on-page")]
		public async Task<ActionResult<object>> CAUnitGetAllOnPageBase(int? PageSize, int? PageIndex, int? ParentId, byte? UnitLevel, string Name, string Phone, string Email, string Address, bool? IsActived, bool? IsMain, string SortDir, string SortField)
		{
			try
			{
				List<CAUnitGetAllOnPage> rsCAUnitGetAllOnPage = await new CAUnitGetAllOnPage(_appSetting).CAUnitGetAllOnPageDAO(PageSize, PageIndex, ParentId, UnitLevel, Name, Phone, Email, Address, IsActived, IsMain, SortDir, SortField);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitGetAllOnPage", rsCAUnitGetAllOnPage},
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
		[Route("get-data-for-create")]
		public async Task<ActionResult<object>> UnitGetDataForCreateBase()
		{
			try
			{
				List<CAFieldGetDropdown> rsCAFieldGetDropdown = await new CAFieldGetDropdown(_appSetting).CAFieldGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"lstField", rsCAFieldGetDropdown},
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
		public async Task<ActionResult<object>> CAUnitGetByIDBase(int? Id)
		{
			try
			{
				List<CAUnitGetByID> rsCAUnitGetByID = await new CAUnitGetByID(_appSetting).CAUnitGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitGetByID", rsCAUnitGetByID},
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

		//[HttpGet]
		//[Authorize]
		//[Route("CAUnitGetTreeBase")]
		//public async Task<ActionResult<object>> CAUnitGetTreeBase()
		//{
		//	try
		//	{
		//		List<CAUnitGetTree> rsCAUnitGetTree = await new CAUnitGetTree(_appSetting).CAUnitGetTreeDAO();
		//		IDictionary<string, object> json = new Dictionary<string, object>
		//			{
		//				{"CAUnitGetTree", rsCAUnitGetTree},
		//			};
		//		return new ResultApi { Success = ResultCode.OK, Result = json };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}

		[HttpPost]
		[Authorize]
		[Route("insert")]
		public async Task<ActionResult<object>> CAUnitInsertBase(CAUnitInsertIN _cAUnitInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitInsert(_appSetting).CAUnitInsertDAO(_cAUnitInsertIN) };
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
		public async Task<ActionResult<object>> CAUnitUpdateBase(CAUnitUpdateIN _cAUnitUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				var s = await new CAUnitUpdate(_appSetting).CAUnitUpdateDAO(_cAUnitUpdateIN);
				return new ResultApi { Success = ResultCode.OK, Result = s };
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
		[Route("delete")]
		public async Task<ActionResult<object>> CAUnitDeleteBase(CAUnitDeleteIN _cAUnitDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitDelete(_appSetting).CAUnitDeleteDAO(_cAUnitDeleteIN) };
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
		[Route("change-status")]
		public async Task<ActionResult<object>> SYUnitChageStatusBase(SYUnitChageStatusIN _sYUnitChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUnitChageStatus(_appSetting).SYUnitChageStatusDAO(_sYUnitChageStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("check-exists")]
		public async Task<ActionResult<object>> SYUnitCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<SYUnitCheckExists> rsSYUnitCheckExists = await new SYUnitCheckExists(_appSetting).SYUnitCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnitCheckExists", rsSYUnitCheckExists},
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
		[Route("get-children-dropdown")]
		public async Task<ActionResult<object>> SY_UnitGetChildrenDropdown()
		{
			try
			{
				var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				List<UnitGetChildrenDropdown> data = await new UnitGetChildrenDropdown(_appSetting).UnitGetChildrenDropdownDAO(unitId);

				return new ResultApi { Success = ResultCode.OK, Result = data };
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
