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

namespace PAKNAPI.ControllerBase
{
	[Route("api/CACategorySPBase")]
	[ApiController]
	public class CACategorySPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public CACategorySPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CADepartmentDeleteListBase")]
		public async Task<ActionResult<object>> CADepartmentDeleteListBase(List<CADepartmentDeleteIN> _cADepartmentDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cADepartmentDeleteIN in _cADepartmentDeleteINs)
				{
					var result = await new CADepartmentDelete(_appSetting).CADepartmentDeleteDAO(_cADepartmentDeleteIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("CADepartmentGetAllOnPageBase")]
		public async Task<ActionResult<object>> CADepartmentGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CADepartmentGetAllOnPage> rsCADepartmentGetAllOnPage = await new CADepartmentGetAllOnPage(_appSetting).CADepartmentGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGetAllOnPage", rsCADepartmentGetAllOnPage},
						{"TotalCount", rsCADepartmentGetAllOnPage != null && rsCADepartmentGetAllOnPage.Count > 0 ? rsCADepartmentGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCADepartmentGetAllOnPage != null && rsCADepartmentGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCADepartmentGetAllOnPage != null && rsCADepartmentGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CADepartmentGroupDeleteListBase")]
		public async Task<ActionResult<object>> CADepartmentGroupDeleteListBase(List<CADepartmentGroupDeleteIN> _cADepartmentGroupDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cADepartmentGroupDeleteIN in _cADepartmentGroupDeleteINs)
				{
					var result = await new CADepartmentGroupDelete(_appSetting).CADepartmentGroupDeleteDAO(_cADepartmentGroupDeleteIN);
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
						{"TotalCount", rsCADepartmentGroupGetAllOnPage != null && rsCADepartmentGroupGetAllOnPage.Count > 0 ? rsCADepartmentGroupGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCADepartmentGroupGetAllOnPage != null && rsCADepartmentGroupGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCADepartmentGroupGetAllOnPage != null && rsCADepartmentGroupGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CADepartmentGroupUpdateListBase")]
		public async Task<ActionResult<object>> CADepartmentGroupUpdateListBase(List<CADepartmentGroupUpdateIN> _cADepartmentGroupUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cADepartmentGroupUpdateIN in _cADepartmentGroupUpdateINs)
				{
					var result = await new CADepartmentGroupUpdate(_appSetting).CADepartmentGroupUpdateDAO(_cADepartmentGroupUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CADepartmentUpdateListBase")]
		public async Task<ActionResult<object>> CADepartmentUpdateListBase(List<CADepartmentUpdateIN> _cADepartmentUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cADepartmentUpdateIN in _cADepartmentUpdateINs)
				{
					var result = await new CADepartmentUpdate(_appSetting).CADepartmentUpdateDAO(_cADepartmentUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAFieldDeleteListBase")]
		public async Task<ActionResult<object>> CAFieldDeleteListBase(List<CAFieldDeleteIN> _cAFieldDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAFieldDeleteIN in _cAFieldDeleteINs)
				{
					var result = await new CAFieldDelete(_appSetting).CAFieldDeleteDAO(_cAFieldDeleteIN);
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
						{"TotalCount", rsCAFieldGetAllOnPage != null && rsCAFieldGetAllOnPage.Count > 0 ? rsCAFieldGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAFieldGetAllOnPage != null && rsCAFieldGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAFieldGetAllOnPage != null && rsCAFieldGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAHashtagDeleteListBase")]
		public async Task<ActionResult<object>> CAHashtagDeleteListBase(List<CAHashtagDeleteIN> _cAHashtagDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAHashtagDeleteIN in _cAHashtagDeleteINs)
				{
					var result = await new CAHashtagDelete(_appSetting).CAHashtagDeleteDAO(_cAHashtagDeleteIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("CAHashtagGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAHashtagGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, bool? IsActived)
		{
			try
			{
				List<CAHashtagGetAllOnPage> rsCAHashtagGetAllOnPage = await new CAHashtagGetAllOnPage(_appSetting).CAHashtagGetAllOnPageDAO(PageSize, PageIndex, Name, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagGetAllOnPage", rsCAHashtagGetAllOnPage},
						{"TotalCount", rsCAHashtagGetAllOnPage != null && rsCAHashtagGetAllOnPage.Count > 0 ? rsCAHashtagGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAHashtagGetAllOnPage != null && rsCAHashtagGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAHashtagGetAllOnPage != null && rsCAHashtagGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAHashtagUpdateListBase")]
		public async Task<ActionResult<object>> CAHashtagUpdateListBase(List<CAHashtagUpdateIN> _cAHashtagUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAHashtagUpdateIN in _cAHashtagUpdateINs)
				{
					var result = await new CAHashtagUpdate(_appSetting).CAHashtagUpdateDAO(_cAHashtagUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CANewsTypeDeleteListBase")]
		public async Task<ActionResult<object>> CANewsTypeDeleteListBase(List<CANewsTypeDeleteIN> _cANewsTypeDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cANewsTypeDeleteIN in _cANewsTypeDeleteINs)
				{
					var result = await new CANewsTypeDelete(_appSetting).CANewsTypeDeleteDAO(_cANewsTypeDeleteIN);
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
						{"TotalCount", rsCANewsTypeGetAllOnPage != null && rsCANewsTypeGetAllOnPage.Count > 0 ? rsCANewsTypeGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCANewsTypeGetAllOnPage != null && rsCANewsTypeGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCANewsTypeGetAllOnPage != null && rsCANewsTypeGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CANewsTypeInsertListBase")]
		public async Task<ActionResult<object>> CANewsTypeInsertListBase(List<CANewsTypeInsertIN> _cANewsTypeInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cANewsTypeInsertIN in _cANewsTypeInsertINs)
				{
					var result = await new CANewsTypeInsert(_appSetting).CANewsTypeInsertDAO(_cANewsTypeInsertIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CANewsTypeUpdateListBase")]
		public async Task<ActionResult<object>> CANewsTypeUpdateListBase(List<CANewsTypeUpdateIN> _cANewsTypeUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cANewsTypeUpdateIN in _cANewsTypeUpdateINs)
				{
					var result = await new CANewsTypeUpdate(_appSetting).CANewsTypeUpdateDAO(_cANewsTypeUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAPositionDeleteListBase")]
		public async Task<ActionResult<object>> CAPositionDeleteListBase(List<CAPositionDeleteIN> _cAPositionDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAPositionDeleteIN in _cAPositionDeleteINs)
				{
					var result = await new CAPositionDelete(_appSetting).CAPositionDeleteDAO(_cAPositionDeleteIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAPositionInsertListBase")]
		public async Task<ActionResult<object>> CAPositionInsertListBase(List<CAPositionInsertIN> _cAPositionInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAPositionInsertIN in _cAPositionInsertINs)
				{
					var result = await new CAPositionInsert(_appSetting).CAPositionInsertDAO(_cAPositionInsertIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAPositionUpdateListBase")]
		public async Task<ActionResult<object>> CAPositionUpdateListBase(List<CAPositionUpdateIN> _cAPositionUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAPositionUpdateIN in _cAPositionUpdateINs)
				{
					var result = await new CAPositionUpdate(_appSetting).CAPositionUpdateDAO(_cAPositionUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAUnitDeleteListBase")]
		public async Task<ActionResult<object>> CAUnitDeleteListBase(List<CAUnitDeleteIN> _cAUnitDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAUnitDeleteIN in _cAUnitDeleteINs)
				{
					var result = await new CAUnitDelete(_appSetting).CAUnitDeleteDAO(_cAUnitDeleteIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("CAUnitGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAUnitGetAllOnPageBase(int? PageSize, int? PageIndex, int? ParentId, byte? UnitLevel, string Name, string Phone, string Email, string Address, bool? IsActive, bool? IsMain)
		{
			try
			{
				List<CAUnitGetAllOnPage> rsCAUnitGetAllOnPage = await new CAUnitGetAllOnPage(_appSetting).CAUnitGetAllOnPageDAO(PageSize, PageIndex, ParentId, UnitLevel, Name, Phone, Email, Address, IsActive, IsMain);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitGetAllOnPage", rsCAUnitGetAllOnPage},
						{"TotalCount", rsCAUnitGetAllOnPage != null && rsCAUnitGetAllOnPage.Count > 0 ? rsCAUnitGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAUnitGetAllOnPage != null && rsCAUnitGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAUnitGetAllOnPage != null && rsCAUnitGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAUnitInsertListBase")]
		public async Task<ActionResult<object>> CAUnitInsertListBase(List<CAUnitInsertIN> _cAUnitInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAUnitInsertIN in _cAUnitInsertINs)
				{
					var result = await new CAUnitInsert(_appSetting).CAUnitInsertDAO(_cAUnitInsertIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAUnitUpdateListBase")]
		public async Task<ActionResult<object>> CAUnitUpdateListBase(List<CAUnitUpdateIN> _cAUnitUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAUnitUpdateIN in _cAUnitUpdateINs)
				{
					var result = await new CAUnitUpdate(_appSetting).CAUnitUpdateDAO(_cAUnitUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAWordDeleteBase")]
		public async Task<ActionResult<object>> CAWordDeleteBase(CAWordDeleteIN _cAWordDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWordDelete(_appSetting).CAWordDeleteDAO(_cAWordDeleteIN) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAWordDeleteListBase")]
		public async Task<ActionResult<object>> CAWordDeleteListBase(List<CAWordDeleteIN> _cAWordDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAWordDeleteIN in _cAWordDeleteINs)
				{
					var result = await new CAWordDelete(_appSetting).CAWordDeleteDAO(_cAWordDeleteIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("CAWordGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAWordGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CAWordGetAllOnPage> rsCAWordGetAllOnPage = await new CAWordGetAllOnPage(_appSetting).CAWordGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWordGetAllOnPage", rsCAWordGetAllOnPage},
						{"TotalCount", rsCAWordGetAllOnPage != null && rsCAWordGetAllOnPage.Count > 0 ? rsCAWordGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAWordGetAllOnPage != null && rsCAWordGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAWordGetAllOnPage != null && rsCAWordGetAllOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAWordInsertListBase")]
		public async Task<ActionResult<object>> CAWordInsertListBase(List<CAWordInsertIN> _cAWordInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAWordInsertIN in _cAWordInsertINs)
				{
					var result = await new CAWordInsert(_appSetting).CAWordInsertDAO(_cAWordInsertIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("CAWordUpdateListBase")]
		public async Task<ActionResult<object>> CAWordUpdateListBase(List<CAWordUpdateIN> _cAWordUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _cAWordUpdateIN in _cAWordUpdateINs)
				{
					var result = await new CAWordUpdate(_appSetting).CAWordUpdateDAO(_cAWordUpdateIN);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
