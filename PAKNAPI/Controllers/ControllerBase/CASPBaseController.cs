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

namespace PAKNAPI.ControllerBase
{
	[Route("api/CASPBase")]
	[ApiController]
	public class CASPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public CASPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("CAAdministrativeUnitsGetDropDownBase")]
		public async Task<ActionResult<object>> CAAdministrativeUnitsGetDropDownBase(int? Id)
		{
			try
			{
				List<CAAdministrativeUnitsGetDropDown> rsCAAdministrativeUnitsGetDropDown = await new CAAdministrativeUnitsGetDropDown(_appSetting).CAAdministrativeUnitsGetDropDownDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAAdministrativeUnitsGetDropDown", rsCAAdministrativeUnitsGetDropDown},
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
		[Authorize("ThePolicy")]
		[Route("CADistrictGetAllBase")]
		public async Task<ActionResult<object>> CADistrictGetAllBase(byte? ProvinceId)
		{
			try
			{
				List<CADistrictGetAll> rsCADistrictGetAll = await new CADistrictGetAll(_appSetting).CADistrictGetAllDAO(ProvinceId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADistrictGetAll", rsCADistrictGetAll},
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
		[Authorize("ThePolicy")]
		[Route("CAFieldDAMGetDropdownBase")]
		public async Task<ActionResult<object>> CAFieldDAMGetDropdownBase()
		{
			try
			{
				List<CAFieldDAMGetDropdown> rsCAFieldDAMGetDropdown = await new CAFieldDAMGetDropdown(_appSetting).CAFieldDAMGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldDAMGetDropdown", rsCAFieldDAMGetDropdown},
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
		[Authorize("ThePolicy")]
		[Route("CAFieldDAMInsertBase")]
		public async Task<ActionResult<object>> CAFieldDAMInsertBase(CAFieldDAMInsertIN _cAFieldDAMInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldDAMInsert(_appSetting).CAFieldDAMInsertDAO(_cAFieldDAMInsertIN) };
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
		[Route("CAFieldGetDropdownBase")]
		public async Task<ActionResult<object>> CAFieldGetDropdownBase()
		{
			try
			{
				List<CAFieldGetDropdown> rsCAFieldGetDropdown = await new CAFieldGetDropdown(_appSetting).CAFieldGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetDropdown", rsCAFieldGetDropdown},
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
		[Route("CAHashtagGetAllBase")]
		public async Task<ActionResult<object>> CAHashtagGetAllBase()
		{
			try
			{
				List<CAHashtagGetAll> rsCAHashtagGetAll = await new CAHashtagGetAll(_appSetting).CAHashtagGetAllDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetAll", rsCAHashtagGetAll},
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
		[Route("CAHashtagGetDropdownBase")]
		public async Task<ActionResult<object>> CAHashtagGetDropdownBase()
		{
			try
			{
				List<CAHashtagGetDropdown> rsCAHashtagGetDropdown = await new CAHashtagGetDropdown(_appSetting).CAHashtagGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetDropdown", rsCAHashtagGetDropdown},
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
		[Authorize("ThePolicy")]
		[Route("CAProvinceGetAllBase")]
		public async Task<ActionResult<object>> CAProvinceGetAllBase()
		{
			try
			{
				List<CAProvinceGetAll> rsCAProvinceGetAll = await new CAProvinceGetAll(_appSetting).CAProvinceGetAllDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAProvinceGetAll", rsCAProvinceGetAll},
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
		[Authorize("ThePolicy")]
		[Route("CAVillageGetAllBase")]
		public async Task<ActionResult<object>> CAVillageGetAllBase(byte? ProvinceId, byte? DistrictId)
		{
			try
			{
				List<CAVillageGetAll> rsCAVillageGetAll = await new CAVillageGetAll(_appSetting).CAVillageGetAllDAO(ProvinceId, DistrictId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAVillageGetAll", rsCAVillageGetAll},
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
	}
}
