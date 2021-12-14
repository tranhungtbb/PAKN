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
		/// <summary>
		/// drop đơn vị theo lĩnh vực
		/// </summary>
		/// <param name="FieldId"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("get-drop-unit-by-field")]
		public async Task<ActionResult<object>> UnitGetDropByFieldId(int? FieldId)
		{
			try
			{
				if (FieldId == null) {
					return new ResultApi { Success = ResultCode.ORROR, Result = null, Message = "Không có dữ liệu" };
				}

				var listUnit = await new CAUnitGetAll(_appSetting).UnitGetDropByFieldIdDAO(FieldId);
				if (listUnit.Count == 0) {
					return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitGetAll(_appSetting).UnitGetDropByFieldIdDAO(null) };
				};
				if (listUnit.Count == 1)
				{
					return new ResultApi { Success = ResultCode.OK, Result = listUnit };
				};

				SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
				if (listUnit.Count > 1 && !listUnit.Any(es => es.Value == dataMain.Id)) {
					var unitMain = new DropdownTree();
					unitMain.Value = dataMain.Id;
					unitMain.Text = dataMain.Name;
					unitMain.IsMain = true;
					unitMain.UnitLevel = 1;
					listUnit.Add(unitMain);
				}

				return new ResultApi { Success = ResultCode.OK, Result = listUnit };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// danh sách đơn vị - all
		/// </summary>
		/// <param name="ParentId"></param>
		/// <param name="UnitLevel"></param>
		/// <returns></returns>

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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách đơn vị
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="ParentId"></param>
		/// <param name="UnitLevel"></param>
		/// <param name="Name"></param>
		/// <param name="Phone"></param>
		/// <param name="Email"></param>
		/// <param name="Address"></param>
		/// <param name="IsActived"></param>
		/// <param name="IsMain"></param>
		/// <param name="SortDir"></param>
		/// <param name="SortField"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// :D
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-data-for-create")]
		public async Task<ActionResult<object>> UnitGetDataForCreateBase()
		{
			try
			{
				List<CAFieldGetDropdown> rsCAFieldGetDropdown = await new CAFieldGetDropdown(_appSetting).CAFieldGetDropdownForCreateUnitDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"lstField", rsCAFieldGetDropdown},
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
		/// chi tiết đơn vị
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// thêm mới đơn vị
		/// </summary>
		/// <param name="_cAUnitInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<ActionResult<object>> CAUnitInsertBase(CAUnitInsertIN _cAUnitInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitInsert(_appSetting).CAUnitInsertDAO(_cAUnitInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập đơn vị
		/// </summary>
		/// <param name="_cAUnitUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> CAUnitUpdateBase(CAUnitUpdateIN _cAUnitUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var s = await new CAUnitUpdate(_appSetting).CAUnitUpdateDAO(_cAUnitUpdateIN);
				return new ResultApi { Success = ResultCode.OK, Result = s };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// xóa đơn vị
		/// </summary>
		/// <param name="_cAUnitDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> CAUnitDeleteBase(CAUnitDeleteIN _cAUnitDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitDelete(_appSetting).CAUnitDeleteDAO(_cAUnitDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thay trạng thái đơn vị
		/// </summary>
		/// <param name="_sYUnitChageStatusIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("change-status")]
		public async Task<ActionResult<object>> SYUnitChageStatusBase(SYUnitChageStatusIN _sYUnitChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUnitChageStatus(_appSetting).SYUnitChageStatusDAO(_sYUnitChageStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// check tồn tại
		/// </summary>
		/// <param name="Field"></param>
		/// <param name="Value"></param>
		/// <param name="Id"></param>
		/// <returns></returns>
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// :D
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("get-children-dropdown-by-parent")]
		public async Task<ActionResult<object>> SY_UnitGetChildrenDropdown(int ParentId)
		{
			try
			{
				List<SYUnitDropdown> data = await new SYUnit(_appSetting).SYUnitGetDropdownByParent(ParentId);

				return new ResultApi { Success = ResultCode.OK, Result = data };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("unit-get-by-group")]
		public async Task<ActionResult<object>> SY_UnitGetDropdown(int GroupId)
		{
			try
			{
				List<SYUnitDropdown> data = await new SYUnit(_appSetting).SYUnitDropdown(GroupId);

				return new ResultApi { Success = ResultCode.OK, Result = data };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-unit-permission-on-page")]
		public async Task<ActionResult<object>> CAUnitPermissionGetAllOnPageBase()
		{
			try
			{
				List<CAUnitPermissionSMS> cAUnitPermissionSMs = await new CAUnitPermissionSMS(_appSetting).CAUnitPermissionSMSGetAllDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitPermissionSMs", cAUnitPermissionSMs},
						{"TotalCount", cAUnitPermissionSMs != null && cAUnitPermissionSMs.Count > 0 ? cAUnitPermissionSMs[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("unit-permission-insert")]
		public async Task<ActionResult<object>> CAUnitPermissionInsertBase(CAUnitPermissionInsert model)
		{
			try
			{
				List<Task> tasks = new List<Task>();
				foreach (var item in model.ListUnit) {
					tasks.Add(new CAUnitPermissionSMS(_appSetting).CAUnitPermissionSMSInsertDAO(item));
				}
				await Task.WhenAll(tasks);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
				return new ResultApi { Success = ResultCode.OK};
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("unit-permission-delete")]
		public async Task<ActionResult<object>> CAUnitPermissionDeleteBase(CAUnitPermissionDelete _cAUnitDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitPermissionSMS(_appSetting).CAUnitPermissionSMSDeleteDAO(_cAUnitDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-unit-dropdown-not-permission")]
		public async Task<ActionResult<object>> CAUnitNotPermissionGetDropdownBase()
		{
			try
			{
				var list = await new CAUnitPermissionSMS(_appSetting).CAUnitNotPermissionSMSGetDropdownDAO();
				return new ResultApi { Success = ResultCode.OK, Result = list };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("unit-has-permission")]
		public async Task<ActionResult<object>> CAUnitCheckPermission()
		{
			try
			{
				var unit = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				var list = await new CAUnitPermissionSMS(_appSetting).CAUnitCheckPermissionSMSDAO(unit);
				return new ResultApi { Success = ResultCode.OK, Result = list > 0 ? true : false };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


	}
}
