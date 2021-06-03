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
	[Route("api/CACategorySPBase")]
	[ApiController]
	public class CACategorySPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public CACategorySPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("CADepartmentDeleteBase")]
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
		[Route("CADepartmentGetAllOnPageBase")]
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
		[Route("CADepartmentGetByIDBase")]
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
		[Route("CADepartmentGroupDeleteBase")]
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
		[HttpPost]
		[Authorize]
		[Route("CAWordDeleteBase")]
		public async Task<ActionResult<object>> CAWordDeleteBase(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroupDelete(_appSetting).CAWordDeleteDAO(_cADepartmentGroupDeleteIN) };
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
		[Route("CADepartmentGroupGetAllOnPageBase")]
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
		[Route("CADepartmentGroupGetByIDBase")]
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
		[Route("CADepartmentGroupInsertBase")]
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
		[Route("CADepartmentGroupUpdateBase")]
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

		[HttpPost]
		[Authorize]
		[Route("CADepartmentInsertBase")]
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
		[Route("CADepartmentUpdateBase")]
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

		[HttpPost]
		[Authorize]
		[Route("CAFieldDeleteBase")]
		public async Task<ActionResult<object>> CAFieldDeleteBase(CAFieldDeleteIN _cAFieldDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldDelete(_appSetting).CAFieldDeleteDAO(_cAFieldDeleteIN) };
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
		[Route("CAFieldGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAFieldGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CAFieldGetAllOnPage> rsCAFieldGetAllOnPage = await new CAFieldGetAllOnPage(_appSetting).CAFieldGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetAllOnPage", rsCAFieldGetAllOnPage},
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
		[Route("CAFieldGetByIDBase")]
		public async Task<ActionResult<object>> CAFieldGetByIDBase(int? Id)
		{
			try
			{
				List<CAFieldGetByID> rsCAFieldGetByID = await new CAFieldGetByID(_appSetting).CAFieldGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetByID", rsCAFieldGetByID},
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
		[Route("CAFieldInsertBase")]
		public async Task<ActionResult<object>> CAFieldInsertBase(CAFieldInsertIN _cAFieldInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldInsert(_appSetting).CAFieldInsertDAO(_cAFieldInsertIN) };
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
		[Route("CAFieldKNCTGetDropdownBase")]
		public async Task<ActionResult<object>> CAFieldKNCTGetDropdownBase()
		{
			try
			{
				List<CAFieldKNCTGetDropdown> rsCAFieldKNCTGetDropdown = await new CAFieldKNCTGetDropdown(_appSetting).CAFieldKNCTGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldKNCTGetDropdown", rsCAFieldKNCTGetDropdown},
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
		[Route("CAFieldKNCTInsertBase")]
		public async Task<ActionResult<object>> CAFieldKNCTInsertBase(CAFieldKNCTInsertIN _cAFieldKNCTInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldKNCTInsert(_appSetting).CAFieldKNCTInsertDAO(_cAFieldKNCTInsertIN) };
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
		[Route("CAFieldUpdateBase")]
		public async Task<ActionResult<object>> CAFieldUpdateBase(CAFieldUpdateIN _cAFieldUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldUpdate(_appSetting).CAFieldUpdateDAO(_cAFieldUpdateIN) };
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
		[Route("CAGroupWordDeleteBase")]
		public async Task<ActionResult<object>> CAGroupWordDeleteBase(CAGroupWordDeleteIN _cAGroupWordDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWordDelete(_appSetting).CAGroupWordDeleteDAO(_cAGroupWordDeleteIN) };
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
		[Route("CAGroupWordGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAGroupWordGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CAGroupWordGetAllOnPage> rsCAGroupWordGetAllOnPage = await new CAGroupWordGetAllOnPage(_appSetting).CAGroupWordGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWordGetAllOnPage", rsCAGroupWordGetAllOnPage},
						{"TotalCount", rsCAGroupWordGetAllOnPage != null && rsCAGroupWordGetAllOnPage.Count > 0 ? rsCAGroupWordGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAGroupWordGetAllOnPage != null && rsCAGroupWordGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAGroupWordGetAllOnPage != null && rsCAGroupWordGetAllOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAGroupWordGetByIDBase")]
		public async Task<ActionResult<object>> CAGroupWordGetByIDBase(int? Id)
		{
			try
			{
				List<CAGroupWordGetByID> rsCAGroupWordGetByID = await new CAGroupWordGetByID(_appSetting).CAGroupWordGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWordGetByID", rsCAGroupWordGetByID},
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
		[Route("CAGroupWordGetListSuggestBase")]
		public async Task<ActionResult<object>> CAGroupWordGetListSuggestBase()
		{
			try
			{
				List<CAGroupWordGetListSuggest> rsCAGroupWordGetListSuggest = await new CAGroupWordGetListSuggest(_appSetting).CAGroupWordGetListSuggestDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWordGetListSuggest", rsCAGroupWordGetListSuggest},
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
		[Route("CAGroupWordInsertBase")]
		public async Task<ActionResult<object>> CAGroupWordInsertBase(CAGroupWordInsertIN _cAGroupWordInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWordInsert(_appSetting).CAGroupWordInsertDAO(_cAGroupWordInsertIN) };
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
		[Route("CAGroupWordUpdateBase")]
		public async Task<ActionResult<object>> CAGroupWordUpdateBase(CAGroupWordUpdateIN _cAGroupWordUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWordUpdate(_appSetting).CAGroupWordUpdateDAO(_cAGroupWordUpdateIN) };
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
		[Route("CAHashtagDeleteBase")]
		public async Task<ActionResult<object>> CAHashtagDeleteBase(CAHashtagDeleteIN _cAHashtagDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtagDelete(_appSetting).CAHashtagDeleteDAO(_cAHashtagDeleteIN) };
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
		[Route("CAHashtagGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAHashtagGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, int? QuantityUser, bool? IsActived)
		{
			try
			{
				List<CAHashtagGetAllOnPage> rsCAHashtagGetAllOnPage = await new CAHashtagGetAllOnPage(_appSetting).CAHashtagGetAllOnPageDAO(PageSize, PageIndex, Name, QuantityUser, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetAllOnPage", rsCAHashtagGetAllOnPage},
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
		[Route("CAHashtagGetByIDBase")]
		public async Task<ActionResult<object>> CAHashtagGetByIDBase(int? Id)
		{
			try
			{
				List<CAHashtagGetByID> rsCAHashtagGetByID = await new CAHashtagGetByID(_appSetting).CAHashtagGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetByID", rsCAHashtagGetByID},
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
		[Route("CAHashtagInsertBase")]
		public async Task<ActionResult<object>> CAHashtagInsertBase(CAHashtagInsertIN _cAHashtagInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtagInsert(_appSetting).CAHashtagInsertDAO(_cAHashtagInsertIN) };
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
		[Route("CAHashtagUpdateBase")]
		public async Task<ActionResult<object>> CAHashtagUpdateBase(CAHashtagUpdateIN _cAHashtagUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtagUpdate(_appSetting).CAHashtagUpdateDAO(_cAHashtagUpdateIN) };
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
		[Route("CANewsTypeDeleteBase")]
		public async Task<ActionResult<object>> CANewsTypeDeleteBase(CANewsTypeDeleteIN _cANewsTypeDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeDelete(_appSetting).CANewsTypeDeleteDAO(_cANewsTypeDeleteIN) };
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
		[Route("CANewsTypeGetAllOnPageBase")]
		public async Task<ActionResult<object>> CANewsTypeGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CANewsTypeGetAllOnPage> rsCANewsTypeGetAllOnPage = await new CANewsTypeGetAllOnPage(_appSetting).CANewsTypeGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsTypeGetAllOnPage", rsCANewsTypeGetAllOnPage},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("CANewsTypeGetByIDBase")]
		public async Task<ActionResult<object>> CANewsTypeGetByIDBase(int? Id)
		{
			try
			{
				List<CANewsTypeGetByID> rsCANewsTypeGetByID = await new CANewsTypeGetByID(_appSetting).CANewsTypeGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsTypeGetByID", rsCANewsTypeGetByID},
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
		[Route("CANewsTypeInsertBase")]
		public async Task<ActionResult<object>> CANewsTypeInsertBase(CANewsTypeInsertIN _cANewsTypeInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeInsert(_appSetting).CANewsTypeInsertDAO(_cANewsTypeInsertIN) };
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
		[Route("CANewsTypeUpdateBase")]
		public async Task<ActionResult<object>> CANewsTypeUpdateBase(CANewsTypeUpdateIN _cANewsTypeUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsTypeUpdate(_appSetting).CANewsTypeUpdateDAO(_cANewsTypeUpdateIN) };
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
		[Route("CAPositionDeleteBase")]
		public async Task<ActionResult<object>> CAPositionDeleteBase(CAPositionDeleteIN _cAPositionDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAPositionDelete(_appSetting).CAPositionDeleteDAO(_cAPositionDeleteIN) };
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
		[Route("CAPositionGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAPositionGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Code, string Description, bool? IsActived)
		{
			try
			{
				List<CAPositionGetAllOnPage> rsCAPositionGetAllOnPage = await new CAPositionGetAllOnPage(_appSetting).CAPositionGetAllOnPageDAO(PageSize, PageIndex, Name, Code, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAPositionGetAllOnPage", rsCAPositionGetAllOnPage},
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
		[Route("CAPositionGetByIDBase")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("CAPositionGetDropdownBase")]
		public async Task<ActionResult<object>> CAPositionGetDropdownBase()
		{
			try
			{
				List<CAPositionGetDropdown> rsCAPositionGetDropdown = await new CAPositionGetDropdown(_appSetting).CAPositionGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAPositionGetDropdown", rsCAPositionGetDropdown},
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
		[Route("CAPositionInsertBase")]
		public async Task<ActionResult<object>> CAPositionInsertBase(CAPositionInsertIN _cAPositionInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAPositionInsert(_appSetting).CAPositionInsertDAO(_cAPositionInsertIN) };
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
		[Route("CAPositionUpdateBase")]
		public async Task<ActionResult<object>> CAPositionUpdateBase(CAPositionUpdateIN _cAPositionUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAPositionUpdate(_appSetting).CAPositionUpdateDAO(_cAPositionUpdateIN) };
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
		[Route("CAUnitDeleteBase")]
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

		[HttpGet]
		[Authorize]
		[Route("CAUnitGetAllBase")]
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
		[Route("CAUnitGetAllOnPageBase")]
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
		[Route("CAUnitGetByIDBase")]
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

		[HttpGet]
		[Authorize]
		[Route("CAUnitGetTreeBase")]
		public async Task<ActionResult<object>> CAUnitGetTreeBase()
		{
			try
			{
				List<CAUnitGetTree> rsCAUnitGetTree = await new CAUnitGetTree(_appSetting).CAUnitGetTreeDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitGetTree", rsCAUnitGetTree},
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
		[Route("CAUnitInsertBase")]
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
		[Route("CAUnitUpdateBase")]
		public async Task<ActionResult<object>> CAUnitUpdateBase(CAUnitUpdateIN _cAUnitUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAUnitUpdate(_appSetting).CAUnitUpdateDAO(_cAUnitUpdateIN) };
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
		[Route("CAUserGetByUnitIdBase")]
		public async Task<ActionResult<object>> CAUserGetByUnitIdBase(int? PageIndex, int? PageSize, string UserName, string FullName, string Phone, bool? IsActive, int? UnitId)
		{
			try
			{
				List<CAUserGetByUnitId> rsCAUserGetByUnitId = await new CAUserGetByUnitId(_appSetting).CAUserGetByUnitIdDAO(PageIndex, PageSize, UserName, FullName, Phone, IsActive, UnitId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUserGetByUnitId", rsCAUserGetByUnitId},
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
		[Route("CAWordGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAWordGetAllOnPageBase(int? PageSize, int? PageIndex, int? GroupId, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CAWordGetAllOnPage> rsCAWordGetAllOnPage = await new CAWordGetAllOnPage(_appSetting).CAWordGetAllOnPageDAO(PageSize, PageIndex, GroupId, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetAllOnPage", rsCAWordGetAllOnPage},
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
		[Route("CAWordGetByIDBase")]
		public async Task<ActionResult<object>> CAWordGetByIDBase(int? Id)
		{
			try
			{
				List<CAWordGetByID> rsCAWordGetByID = await new CAWordGetByID(_appSetting).CAWordGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetByID", rsCAWordGetByID},
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
		[Route("CAWordInsertBase")]
		public async Task<ActionResult<object>> CAWordInsertBase(CAWordInsertIN _cAWordInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWordInsert(_appSetting).CAWordInsertDAO(_cAWordInsertIN) };
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
		[Route("CAWordUpdateBase")]
		public async Task<ActionResult<object>> CAWordUpdateBase(CAWordUpdateIN _cAWordUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWordUpdate(_appSetting).CAWordUpdateDAO(_cAWordUpdateIN) };
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
		[Route("CAWordGetListSuggestBase")]
		public async Task<ActionResult<object>> CA_WordGetListSuggestBase()
		{
			try
			{
				List<CA_WordGetListSuggest> rsCA_WordGetListSuggest = await new CA_WordGetListSuggest(_appSetting).CA_WordGetListSuggestDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetListSuggest", rsCA_WordGetListSuggest},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
