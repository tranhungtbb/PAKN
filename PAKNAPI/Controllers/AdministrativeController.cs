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

namespace PAKNAPI
{
	[Route("api/administrative")]
	[ApiController]
	public class AdministrativeController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public AdministrativeController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}



		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-province")]
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
		[Route("get-list-district")]
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
		[Route("get-list-village")]
		public async Task<ActionResult<object>> CAVillageGetAllBase(short? ProvinceId, short? DistrictId)
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
		[HttpGet]
		[Route("get-list-province-by-province-id")]
		public async Task<ActionResult<object>> GetAllByProvinceId(short? ProvinceId)
		{
			try
			{
				List<CAGetAllByProvinceId> rsList = await new CAGetAllByProvinceId(_appSetting).GetAll(ProvinceId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"ListData", rsList},
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
		[Route("get-drop-down")]
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



	}
}
