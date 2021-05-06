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
	[Route("api/SMSSPBase")]
	[ApiController]
	public class SMSSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SMSSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("SMSDoanhNghiepDeleteBySMSIdBase")]
		public async Task<ActionResult<object>> SMSDoanhNghiepDeleteBySMSIdBase(SMSDoanhNghiepDeleteBySMSIdIN _sMSDoanhNghiepDeleteBySMSIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSDoanhNghiepDeleteBySMSId(_appSetting).SMSDoanhNghiepDeleteBySMSIdDAO(_sMSDoanhNghiepDeleteBySMSIdIN) };
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
		[Route("SMSDoanhNghiepDeleteBySMSIdListBase")]
		public async Task<ActionResult<object>> SMSDoanhNghiepDeleteBySMSIdListBase(List<SMSDoanhNghiepDeleteBySMSIdIN> _sMSDoanhNghiepDeleteBySMSIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSDoanhNghiepDeleteBySMSIdIN in _sMSDoanhNghiepDeleteBySMSIdINs)
				{
					var result = await new SMSDoanhNghiepDeleteBySMSId(_appSetting).SMSDoanhNghiepDeleteBySMSIdDAO(_sMSDoanhNghiepDeleteBySMSIdIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
		[Route("SMSDoanhNghiepInsertBase")]
		public async Task<ActionResult<object>> SMSDoanhNghiepInsertBase(SMSDoanhNghiepInsertIN _sMSDoanhNghiepInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSDoanhNghiepInsert(_appSetting).SMSDoanhNghiepInsertDAO(_sMSDoanhNghiepInsertIN) };
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
		[Route("SMSDoanhNghiepInsertListBase")]
		public async Task<ActionResult<object>> SMSDoanhNghiepInsertListBase(List<SMSDoanhNghiepInsertIN> _sMSDoanhNghiepInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSDoanhNghiepInsertIN in _sMSDoanhNghiepInsertINs)
				{
					var result = await new SMSDoanhNghiepInsert(_appSetting).SMSDoanhNghiepInsertDAO(_sMSDoanhNghiepInsertIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
		[Route("SMSGetListIndividualBusinessBySMSIdBase")]
		public async Task<ActionResult<object>> SMSGetListIndividualBusinessBySMSIdBase(int? SMSId)
		{
			try
			{
				List<SMSGetListIndividualBusinessBySMSId> rsSMSGetListIndividualBusinessBySMSId = await new SMSGetListIndividualBusinessBySMSId(_appSetting).SMSGetListIndividualBusinessBySMSIdDAO(SMSId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SMSGetListIndividualBusinessBySMSId", rsSMSGetListIndividualBusinessBySMSId},
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
		[Route("SMSNguoiNhanDeleteBySMSIdBase")]
		public async Task<ActionResult<object>> SMSNguoiNhanDeleteBySMSIdBase(SMSNguoiNhanDeleteBySMSIdIN _sMSNguoiNhanDeleteBySMSIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSNguoiNhanDeleteBySMSId(_appSetting).SMSNguoiNhanDeleteBySMSIdDAO(_sMSNguoiNhanDeleteBySMSIdIN) };
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
		[Route("SMSNguoiNhanDeleteBySMSIdListBase")]
		public async Task<ActionResult<object>> SMSNguoiNhanDeleteBySMSIdListBase(List<SMSNguoiNhanDeleteBySMSIdIN> _sMSNguoiNhanDeleteBySMSIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSNguoiNhanDeleteBySMSIdIN in _sMSNguoiNhanDeleteBySMSIdINs)
				{
					var result = await new SMSNguoiNhanDeleteBySMSId(_appSetting).SMSNguoiNhanDeleteBySMSIdDAO(_sMSNguoiNhanDeleteBySMSIdIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
		[Route("SMSNguoiNhanInsertBase")]
		public async Task<ActionResult<object>> SMSNguoiNhanInsertBase(SMSNguoiNhanInsertIN _sMSNguoiNhanInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSNguoiNhanInsert(_appSetting).SMSNguoiNhanInsertDAO(_sMSNguoiNhanInsertIN) };
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
		[Route("SMSNguoiNhanInsertListBase")]
		public async Task<ActionResult<object>> SMSNguoiNhanInsertListBase(List<SMSNguoiNhanInsertIN> _sMSNguoiNhanInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSNguoiNhanInsertIN in _sMSNguoiNhanInsertINs)
				{
					var result = await new SMSNguoiNhanInsert(_appSetting).SMSNguoiNhanInsertDAO(_sMSNguoiNhanInsertIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
		[Route("SMSQuanLyTinNhanDeleteBase")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanDeleteBase(SMSQuanLyTinNhanDeleteIN _sMSQuanLyTinNhanDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSQuanLyTinNhanDelete(_appSetting).SMSQuanLyTinNhanDeleteDAO(_sMSQuanLyTinNhanDeleteIN) };
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
		[Route("SMSQuanLyTinNhanDeleteListBase")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanDeleteListBase(List<SMSQuanLyTinNhanDeleteIN> _sMSQuanLyTinNhanDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSQuanLyTinNhanDeleteIN in _sMSQuanLyTinNhanDeleteINs)
				{
					var result = await new SMSQuanLyTinNhanDelete(_appSetting).SMSQuanLyTinNhanDeleteDAO(_sMSQuanLyTinNhanDeleteIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
		[Route("SMSQuanLyTinNhanGetAllOnPageBase")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanGetAllOnPageBase(int? PageSize, int? PageIndex, string Title, string UnitName, string Type, byte? Status)
		{
			try
			{
				List<SMSQuanLyTinNhanGetAllOnPage> rsSMSQuanLyTinNhanGetAllOnPage = await new SMSQuanLyTinNhanGetAllOnPage(_appSetting).SMSQuanLyTinNhanGetAllOnPageDAO(PageSize, PageIndex, Title, UnitName, Type, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SMSQuanLyTinNhanGetAllOnPage", rsSMSQuanLyTinNhanGetAllOnPage},
						{"TotalCount", rsSMSQuanLyTinNhanGetAllOnPage != null && rsSMSQuanLyTinNhanGetAllOnPage.Count > 0 ? rsSMSQuanLyTinNhanGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSMSQuanLyTinNhanGetAllOnPage != null && rsSMSQuanLyTinNhanGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSMSQuanLyTinNhanGetAllOnPage != null && rsSMSQuanLyTinNhanGetAllOnPage.Count > 0 ? PageSize : 0},
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
		[Route("SMSQuanLyTinNhanInsertBase")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanInsertBase(SMSQuanLyTinNhanInsertIN _sMSQuanLyTinNhanInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSQuanLyTinNhanInsert(_appSetting).SMSQuanLyTinNhanInsertDAO(_sMSQuanLyTinNhanInsertIN) };
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
		[Route("SMSQuanLyTinNhanUpdateBase")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanUpdateBase(SMSQuanLyTinNhanUpdateIN _sMSQuanLyTinNhanUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSQuanLyTinNhanUpdate(_appSetting).SMSQuanLyTinNhanUpdateDAO(_sMSQuanLyTinNhanUpdateIN) };
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
		[Route("SMSTinNhanAdministrativeUnitMapDeleteBySMSIdBase")]
		public async Task<ActionResult<object>> SMSTinNhanAdministrativeUnitMapDeleteBySMSIdBase(SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN _sMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSTinNhanAdministrativeUnitMapDeleteBySMSId(_appSetting).SMSTinNhanAdministrativeUnitMapDeleteBySMSIdDAO(_sMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN) };
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
		[Route("SMSTinNhanAdministrativeUnitMapDeleteBySMSIdListBase")]
		public async Task<ActionResult<object>> SMSTinNhanAdministrativeUnitMapDeleteBySMSIdListBase(List<SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN> _sMSTinNhanAdministrativeUnitMapDeleteBySMSIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN in _sMSTinNhanAdministrativeUnitMapDeleteBySMSIdINs)
				{
					var result = await new SMSTinNhanAdministrativeUnitMapDeleteBySMSId(_appSetting).SMSTinNhanAdministrativeUnitMapDeleteBySMSIdDAO(_sMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
		[Route("SMSTinNhanAdministrativeUnitMapInsertBase")]
		public async Task<ActionResult<object>> SMSTinNhanAdministrativeUnitMapInsertBase(SMSTinNhanAdministrativeUnitMapInsertIN _sMSTinNhanAdministrativeUnitMapInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SMSTinNhanAdministrativeUnitMapInsert(_appSetting).SMSTinNhanAdministrativeUnitMapInsertDAO(_sMSTinNhanAdministrativeUnitMapInsertIN) };
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
		[Route("SMSTinNhanAdministrativeUnitMapInsertListBase")]
		public async Task<ActionResult<object>> SMSTinNhanAdministrativeUnitMapInsertListBase(List<SMSTinNhanAdministrativeUnitMapInsertIN> _sMSTinNhanAdministrativeUnitMapInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sMSTinNhanAdministrativeUnitMapInsertIN in _sMSTinNhanAdministrativeUnitMapInsertINs)
				{
					var result = await new SMSTinNhanAdministrativeUnitMapInsert(_appSetting).SMSTinNhanAdministrativeUnitMapInsertDAO(_sMSTinNhanAdministrativeUnitMapInsertIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
