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
	[Route("api/CATableBase")]
	[ApiController]
	public class CATableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public CATableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region CADepartment

		[HttpGet]
		[Authorize]
		[Route("CADepartmentGetByID")]
		public async Task<ActionResult<object>> CADepartmentGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartment(_appSetting).CADepartmentGetByID(Id) };
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
		[Route("CADepartmentGetAll")]
		public async Task<ActionResult<object>> CADepartmentGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartment(_appSetting).CADepartmentGetAll() };
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
		[Route("CADepartmentGetAllOnPage")]
		public async Task<ActionResult<object>> CADepartmentGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CADepartmentOnPage> rsCADepartmentOnPage = await new CADepartment(_appSetting).CADepartmentGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartment", rsCADepartmentOnPage},
						{"TotalCount", rsCADepartmentOnPage != null && rsCADepartmentOnPage.Count > 0 ? rsCADepartmentOnPage[0].RowNumber : 0},
						{"PageIndex", rsCADepartmentOnPage != null && rsCADepartmentOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCADepartmentOnPage != null && rsCADepartmentOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CADepartmentInsert")]
		public async Task<ActionResult<object>> CADepartmentInsert(CADepartment _cADepartment)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartment(_appSetting).CADepartmentInsert(_cADepartment) };
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
		[Route("CADepartmentListInsert")]
		public async Task<ActionResult<object>> CADepartmentListInsert(List<CADepartment> _cADepartments)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CADepartment _cADepartment in _cADepartments)
				{
					int? result = await new CADepartment(_appSetting).CADepartmentInsert(_cADepartment);
					if (result != null)
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
		[Route("CADepartmentUpdate")]
		public async Task<ActionResult<object>> CADepartmentUpdate(CADepartment _cADepartment)
		{
			try
			{
				int count = await new CADepartment(_appSetting).CADepartmentUpdate(_cADepartment);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADepartmentDelete")]
		public async Task<ActionResult<object>> CADepartmentDelete(CADepartment _cADepartment)
		{
			try
			{
				int count = await new CADepartment(_appSetting).CADepartmentDelete(_cADepartment);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADepartmentListDelete")]
		public async Task<ActionResult<object>> CADepartmentListDelete(List<CADepartment> _cADepartments)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CADepartment _cADepartment in _cADepartments)
				{
					var result = await new CADepartment(_appSetting).CADepartmentDelete(_cADepartment);
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
		[Route("CADepartmentDeleteAll")]
		public async Task<ActionResult<object>> CADepartmentDeleteAll()
		{
			try
			{
				int count = await new CADepartment(_appSetting).CADepartmentDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADepartmentCount")]
		public async Task<ActionResult<object>> CADepartmentCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartment(_appSetting).CADepartmentCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CADepartment

		#region CADepartmentGroup

		[HttpGet]
		[Authorize]
		[Route("CADepartmentGroupGetByID")]
		public async Task<ActionResult<object>> CADepartmentGroupGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroup(_appSetting).CADepartmentGroupGetByID(Id) };
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
		[Route("CADepartmentGroupGetAll")]
		public async Task<ActionResult<object>> CADepartmentGroupGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroup(_appSetting).CADepartmentGroupGetAll() };
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
		[Route("CADepartmentGroupGetAllOnPage")]
		public async Task<ActionResult<object>> CADepartmentGroupGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CADepartmentGroupOnPage> rsCADepartmentGroupOnPage = await new CADepartmentGroup(_appSetting).CADepartmentGroupGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGroup", rsCADepartmentGroupOnPage},
						{"TotalCount", rsCADepartmentGroupOnPage != null && rsCADepartmentGroupOnPage.Count > 0 ? rsCADepartmentGroupOnPage[0].RowNumber : 0},
						{"PageIndex", rsCADepartmentGroupOnPage != null && rsCADepartmentGroupOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCADepartmentGroupOnPage != null && rsCADepartmentGroupOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CADepartmentGroupInsert")]
		public async Task<ActionResult<object>> CADepartmentGroupInsert(CADepartmentGroup _cADepartmentGroup)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroup(_appSetting).CADepartmentGroupInsert(_cADepartmentGroup) };
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
		[Route("CADepartmentGroupListInsert")]
		public async Task<ActionResult<object>> CADepartmentGroupListInsert(List<CADepartmentGroup> _cADepartmentGroups)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CADepartmentGroup _cADepartmentGroup in _cADepartmentGroups)
				{
					int? result = await new CADepartmentGroup(_appSetting).CADepartmentGroupInsert(_cADepartmentGroup);
					if (result != null)
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
		[Route("CADepartmentGroupUpdate")]
		public async Task<ActionResult<object>> CADepartmentGroupUpdate(CADepartmentGroup _cADepartmentGroup)
		{
			try
			{
				int count = await new CADepartmentGroup(_appSetting).CADepartmentGroupUpdate(_cADepartmentGroup);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADepartmentGroupDelete")]
		public async Task<ActionResult<object>> CADepartmentGroupDelete(CADepartmentGroup _cADepartmentGroup)
		{
			try
			{
				int count = await new CADepartmentGroup(_appSetting).CADepartmentGroupDelete(_cADepartmentGroup);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADepartmentGroupListDelete")]
		public async Task<ActionResult<object>> CADepartmentGroupListDelete(List<CADepartmentGroup> _cADepartmentGroups)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CADepartmentGroup _cADepartmentGroup in _cADepartmentGroups)
				{
					var result = await new CADepartmentGroup(_appSetting).CADepartmentGroupDelete(_cADepartmentGroup);
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
		[Route("CADepartmentGroupDeleteAll")]
		public async Task<ActionResult<object>> CADepartmentGroupDeleteAll()
		{
			try
			{
				int count = await new CADepartmentGroup(_appSetting).CADepartmentGroupDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADepartmentGroupCount")]
		public async Task<ActionResult<object>> CADepartmentGroupCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentGroup(_appSetting).CADepartmentGroupCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CADepartmentGroup

		#region CADistrict

		[HttpGet]
		[Authorize]
		[Route("CADistrictGetByID")]
		public async Task<ActionResult<object>> CADistrictGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADistrict(_appSetting).CADistrictGetByID(Id) };
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
		[Route("CADistrictGetAll")]
		public async Task<ActionResult<object>> CADistrictGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADistrict(_appSetting).CADistrictGetAll() };
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
		[Route("CADistrictGetAllOnPage")]
		public async Task<ActionResult<object>> CADistrictGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CADistrictOnPage> rsCADistrictOnPage = await new CADistrict(_appSetting).CADistrictGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADistrict", rsCADistrictOnPage},
						{"TotalCount", rsCADistrictOnPage != null && rsCADistrictOnPage.Count > 0 ? rsCADistrictOnPage[0].RowNumber : 0},
						{"PageIndex", rsCADistrictOnPage != null && rsCADistrictOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCADistrictOnPage != null && rsCADistrictOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CADistrictInsert")]
		public async Task<ActionResult<object>> CADistrictInsert(CADistrict _cADistrict)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADistrict(_appSetting).CADistrictInsert(_cADistrict) };
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
		[Route("CADistrictListInsert")]
		public async Task<ActionResult<object>> CADistrictListInsert(List<CADistrict> _cADistricts)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CADistrict _cADistrict in _cADistricts)
				{
					int? result = await new CADistrict(_appSetting).CADistrictInsert(_cADistrict);
					if (result != null)
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
		[Route("CADistrictUpdate")]
		public async Task<ActionResult<object>> CADistrictUpdate(CADistrict _cADistrict)
		{
			try
			{
				int count = await new CADistrict(_appSetting).CADistrictUpdate(_cADistrict);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADistrictDelete")]
		public async Task<ActionResult<object>> CADistrictDelete(CADistrict _cADistrict)
		{
			try
			{
				int count = await new CADistrict(_appSetting).CADistrictDelete(_cADistrict);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADistrictListDelete")]
		public async Task<ActionResult<object>> CADistrictListDelete(List<CADistrict> _cADistricts)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CADistrict _cADistrict in _cADistricts)
				{
					var result = await new CADistrict(_appSetting).CADistrictDelete(_cADistrict);
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
		[Route("CADistrictDeleteAll")]
		public async Task<ActionResult<object>> CADistrictDeleteAll()
		{
			try
			{
				int count = await new CADistrict(_appSetting).CADistrictDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CADistrictCount")]
		public async Task<ActionResult<object>> CADistrictCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CADistrict(_appSetting).CADistrictCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CADistrict

		#region CAField

		[HttpGet]
		[Authorize]
		[Route("CAFieldGetByID")]
		public async Task<ActionResult<object>> CAFieldGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAField(_appSetting).CAFieldGetByID(Id) };
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
		[Route("CAFieldGetAll")]
		public async Task<ActionResult<object>> CAFieldGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAField(_appSetting).CAFieldGetAll() };
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
		[Route("CAFieldGetAllOnPage")]
		public async Task<ActionResult<object>> CAFieldGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CAFieldOnPage> rsCAFieldOnPage = await new CAField(_appSetting).CAFieldGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAField", rsCAFieldOnPage},
						{"TotalCount", rsCAFieldOnPage != null && rsCAFieldOnPage.Count > 0 ? rsCAFieldOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAFieldOnPage != null && rsCAFieldOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAFieldOnPage != null && rsCAFieldOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAFieldInsert")]
		public async Task<ActionResult<object>> CAFieldInsert(CAField _cAField)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAField(_appSetting).CAFieldInsert(_cAField) };
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
		[Route("CAFieldListInsert")]
		public async Task<ActionResult<object>> CAFieldListInsert(List<CAField> _cAFields)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAField _cAField in _cAFields)
				{
					int? result = await new CAField(_appSetting).CAFieldInsert(_cAField);
					if (result != null)
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
		[Route("CAFieldUpdate")]
		public async Task<ActionResult<object>> CAFieldUpdate(CAField _cAField)
		{
			try
			{
				int count = await new CAField(_appSetting).CAFieldUpdate(_cAField);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAFieldDelete")]
		public async Task<ActionResult<object>> CAFieldDelete(CAField _cAField)
		{
			try
			{
				int count = await new CAField(_appSetting).CAFieldDelete(_cAField);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAFieldListDelete")]
		public async Task<ActionResult<object>> CAFieldListDelete(List<CAField> _cAFields)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAField _cAField in _cAFields)
				{
					var result = await new CAField(_appSetting).CAFieldDelete(_cAField);
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
		[Route("CAFieldDeleteAll")]
		public async Task<ActionResult<object>> CAFieldDeleteAll()
		{
			try
			{
				int count = await new CAField(_appSetting).CAFieldDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAFieldCount")]
		public async Task<ActionResult<object>> CAFieldCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAField(_appSetting).CAFieldCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAField

		#region CAHashtag

		[HttpGet]
		[Authorize]
		[Route("CAHashtagGetByID")]
		public async Task<ActionResult<object>> CAHashtagGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagGetByID(Id) };
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
		[Route("CAHashtagGetAll")]
		public async Task<ActionResult<object>> CAHashtagGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagGetAll() };
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
		[Route("CAHashtagGetAllOnPage")]
		public async Task<ActionResult<object>> CAHashtagGetAllOnPage(int PageSize, int PageIndex, string? Name, bool? IsActived)
		{
			try
			{
				List<CAHashtagOnPage> rsCAHashtagOnPage = await new CAHashtag(_appSetting).CAHashtagGetAllOnPage(PageSize, PageIndex,Name, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtag", rsCAHashtagOnPage},
						{"TotalCount", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? rsCAHashtagOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAHashtagOnPage != null && rsCAHashtagOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAHashtagInsert")]
		public async Task<ActionResult<object>> CAHashtagInsert(CAHashtag _cAHashtag)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagInsert(_cAHashtag) };
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
		[Route("CAHashtagListInsert")]
		public async Task<ActionResult<object>> CAHashtagListInsert(List<CAHashtag> _cAHashtags)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAHashtag _cAHashtag in _cAHashtags)
				{
					int? result = await new CAHashtag(_appSetting).CAHashtagInsert(_cAHashtag);
					if (result != null)
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
		[Route("CAHashtagUpdate")]
		public async Task<ActionResult<object>> CAHashtagUpdate(CAHashtag _cAHashtag)
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagUpdate(_cAHashtag);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAHashtagDelete")]
		public async Task<ActionResult<object>> CAHashtagDelete(CAHashtag _cAHashtag)
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagDelete(_cAHashtag);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAHashtagListDelete")]
		public async Task<ActionResult<object>> CAHashtagListDelete(List<CAHashtag> _cAHashtags)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAHashtag _cAHashtag in _cAHashtags)
				{
					var result = await new CAHashtag(_appSetting).CAHashtagDelete(_cAHashtag);
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
		[Route("CAHashtagDeleteAll")]
		public async Task<ActionResult<object>> CAHashtagDeleteAll()
		{
			try
			{
				int count = await new CAHashtag(_appSetting).CAHashtagDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAHashtagCount")]
		public async Task<ActionResult<object>> CAHashtagCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtag(_appSetting).CAHashtagCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAHashtag

		#region CANewsType

		[HttpGet]
		[Authorize]
		[Route("CANewsTypeGetByID")]
		public async Task<ActionResult<object>> CANewsTypeGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsType(_appSetting).CANewsTypeGetByID(Id) };
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
		[Route("CANewsTypeGetAll")]
		public async Task<ActionResult<object>> CANewsTypeGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsType(_appSetting).CANewsTypeGetAll() };
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
		[Route("CANewsTypeGetAllOnPage")]
		public async Task<ActionResult<object>> CANewsTypeGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CANewsTypeOnPage> rsCANewsTypeOnPage = await new CANewsType(_appSetting).CANewsTypeGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CANewsType", rsCANewsTypeOnPage},
						{"TotalCount", rsCANewsTypeOnPage != null && rsCANewsTypeOnPage.Count > 0 ? rsCANewsTypeOnPage[0].RowNumber : 0},
						{"PageIndex", rsCANewsTypeOnPage != null && rsCANewsTypeOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCANewsTypeOnPage != null && rsCANewsTypeOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CANewsTypeInsert")]
		public async Task<ActionResult<object>> CANewsTypeInsert(CANewsType _cANewsType)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsType(_appSetting).CANewsTypeInsert(_cANewsType) };
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
		[Route("CANewsTypeListInsert")]
		public async Task<ActionResult<object>> CANewsTypeListInsert(List<CANewsType> _cANewsTypes)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CANewsType _cANewsType in _cANewsTypes)
				{
					int? result = await new CANewsType(_appSetting).CANewsTypeInsert(_cANewsType);
					if (result != null)
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
		[Route("CANewsTypeUpdate")]
		public async Task<ActionResult<object>> CANewsTypeUpdate(CANewsType _cANewsType)
		{
			try
			{
				int count = await new CANewsType(_appSetting).CANewsTypeUpdate(_cANewsType);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CANewsTypeDelete")]
		public async Task<ActionResult<object>> CANewsTypeDelete(CANewsType _cANewsType)
		{
			try
			{
				int count = await new CANewsType(_appSetting).CANewsTypeDelete(_cANewsType);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CANewsTypeListDelete")]
		public async Task<ActionResult<object>> CANewsTypeListDelete(List<CANewsType> _cANewsTypes)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CANewsType _cANewsType in _cANewsTypes)
				{
					var result = await new CANewsType(_appSetting).CANewsTypeDelete(_cANewsType);
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
		[Route("CANewsTypeDeleteAll")]
		public async Task<ActionResult<object>> CANewsTypeDeleteAll()
		{
			try
			{
				int count = await new CANewsType(_appSetting).CANewsTypeDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CANewsTypeCount")]
		public async Task<ActionResult<object>> CANewsTypeCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CANewsType(_appSetting).CANewsTypeCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CANewsType

		#region CAPosition

		[HttpGet]
		[Authorize]
		[Route("CAPositionGetByID")]
		public async Task<ActionResult<object>> CAPositionGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAPosition(_appSetting).CAPositionGetByID(Id) };
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
		[Route("CAPositionGetAll")]
		public async Task<ActionResult<object>> CAPositionGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAPosition(_appSetting).CAPositionGetAll() };
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
		[Route("CAPositionGetAllOnPage")]
		public async Task<ActionResult<object>> CAPositionGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CAPositionOnPage> rsCAPositionOnPage = await new CAPosition(_appSetting).CAPositionGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAPosition", rsCAPositionOnPage},
						{"TotalCount", rsCAPositionOnPage != null && rsCAPositionOnPage.Count > 0 ? rsCAPositionOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAPositionOnPage != null && rsCAPositionOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAPositionOnPage != null && rsCAPositionOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAPositionInsert")]
		public async Task<ActionResult<object>> CAPositionInsert(CAPosition _cAPosition)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAPosition(_appSetting).CAPositionInsert(_cAPosition) };
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
		[Route("CAPositionListInsert")]
		public async Task<ActionResult<object>> CAPositionListInsert(List<CAPosition> _cAPositions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAPosition _cAPosition in _cAPositions)
				{
					int? result = await new CAPosition(_appSetting).CAPositionInsert(_cAPosition);
					if (result != null)
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
		[Route("CAPositionUpdate")]
		public async Task<ActionResult<object>> CAPositionUpdate(CAPosition _cAPosition)
		{
			try
			{
				int count = await new CAPosition(_appSetting).CAPositionUpdate(_cAPosition);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAPositionDelete")]
		public async Task<ActionResult<object>> CAPositionDelete(CAPosition _cAPosition)
		{
			try
			{
				int count = await new CAPosition(_appSetting).CAPositionDelete(_cAPosition);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAPositionListDelete")]
		public async Task<ActionResult<object>> CAPositionListDelete(List<CAPosition> _cAPositions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAPosition _cAPosition in _cAPositions)
				{
					var result = await new CAPosition(_appSetting).CAPositionDelete(_cAPosition);
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
		[Route("CAPositionDeleteAll")]
		public async Task<ActionResult<object>> CAPositionDeleteAll()
		{
			try
			{
				int count = await new CAPosition(_appSetting).CAPositionDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAPositionCount")]
		public async Task<ActionResult<object>> CAPositionCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAPosition(_appSetting).CAPositionCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAPosition

		#region CAProvince

		[HttpGet]
		[Authorize]
		[Route("CAProvinceGetByID")]
		public async Task<ActionResult<object>> CAProvinceGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAProvince(_appSetting).CAProvinceGetByID(Id) };
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
		[Route("CAProvinceGetAll")]
		public async Task<ActionResult<object>> CAProvinceGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAProvince(_appSetting).CAProvinceGetAll() };
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
		[Route("CAProvinceGetAllOnPage")]
		public async Task<ActionResult<object>> CAProvinceGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CAProvinceOnPage> rsCAProvinceOnPage = await new CAProvince(_appSetting).CAProvinceGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAProvince", rsCAProvinceOnPage},
						{"TotalCount", rsCAProvinceOnPage != null && rsCAProvinceOnPage.Count > 0 ? rsCAProvinceOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAProvinceOnPage != null && rsCAProvinceOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAProvinceOnPage != null && rsCAProvinceOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAProvinceInsert")]
		public async Task<ActionResult<object>> CAProvinceInsert(CAProvince _cAProvince)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAProvince(_appSetting).CAProvinceInsert(_cAProvince) };
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
		[Route("CAProvinceListInsert")]
		public async Task<ActionResult<object>> CAProvinceListInsert(List<CAProvince> _cAProvinces)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAProvince _cAProvince in _cAProvinces)
				{
					int? result = await new CAProvince(_appSetting).CAProvinceInsert(_cAProvince);
					if (result != null)
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
		[Route("CAProvinceUpdate")]
		public async Task<ActionResult<object>> CAProvinceUpdate(CAProvince _cAProvince)
		{
			try
			{
				int count = await new CAProvince(_appSetting).CAProvinceUpdate(_cAProvince);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAProvinceDelete")]
		public async Task<ActionResult<object>> CAProvinceDelete(CAProvince _cAProvince)
		{
			try
			{
				int count = await new CAProvince(_appSetting).CAProvinceDelete(_cAProvince);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAProvinceListDelete")]
		public async Task<ActionResult<object>> CAProvinceListDelete(List<CAProvince> _cAProvinces)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAProvince _cAProvince in _cAProvinces)
				{
					var result = await new CAProvince(_appSetting).CAProvinceDelete(_cAProvince);
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
		[Route("CAProvinceDeleteAll")]
		public async Task<ActionResult<object>> CAProvinceDeleteAll()
		{
			try
			{
				int count = await new CAProvince(_appSetting).CAProvinceDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAProvinceCount")]
		public async Task<ActionResult<object>> CAProvinceCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAProvince(_appSetting).CAProvinceCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAProvince

		#region CAWards

		[HttpGet]
		[Authorize]
		[Route("CAWardsGetByID")]
		public async Task<ActionResult<object>> CAWardsGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAWards(_appSetting).CAWardsGetByID(Id) };
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
		[Route("CAWardsGetAll")]
		public async Task<ActionResult<object>> CAWardsGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAWards(_appSetting).CAWardsGetAll() };
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
		[Route("CAWardsGetAllOnPage")]
		public async Task<ActionResult<object>> CAWardsGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CAWardsOnPage> rsCAWardsOnPage = await new CAWards(_appSetting).CAWardsGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWards", rsCAWardsOnPage},
						{"TotalCount", rsCAWardsOnPage != null && rsCAWardsOnPage.Count > 0 ? rsCAWardsOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAWardsOnPage != null && rsCAWardsOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAWardsOnPage != null && rsCAWardsOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAWardsInsert")]
		public async Task<ActionResult<object>> CAWardsInsert(CAWards _cAWards)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWards(_appSetting).CAWardsInsert(_cAWards) };
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
		[Route("CAWardsListInsert")]
		public async Task<ActionResult<object>> CAWardsListInsert(List<CAWards> _cAWardss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAWards _cAWards in _cAWardss)
				{
					int? result = await new CAWards(_appSetting).CAWardsInsert(_cAWards);
					if (result != null)
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
		[Route("CAWardsUpdate")]
		public async Task<ActionResult<object>> CAWardsUpdate(CAWards _cAWards)
		{
			try
			{
				int count = await new CAWards(_appSetting).CAWardsUpdate(_cAWards);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAWardsDelete")]
		public async Task<ActionResult<object>> CAWardsDelete(CAWards _cAWards)
		{
			try
			{
				int count = await new CAWards(_appSetting).CAWardsDelete(_cAWards);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAWardsListDelete")]
		public async Task<ActionResult<object>> CAWardsListDelete(List<CAWards> _cAWardss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAWards _cAWards in _cAWardss)
				{
					var result = await new CAWards(_appSetting).CAWardsDelete(_cAWards);
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
		[Route("CAWardsDeleteAll")]
		public async Task<ActionResult<object>> CAWardsDeleteAll()
		{
			try
			{
				int count = await new CAWards(_appSetting).CAWardsDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAWardsCount")]
		public async Task<ActionResult<object>> CAWardsCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAWards(_appSetting).CAWardsCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAWards

		#region CAWord

		[HttpGet]
		[Authorize]
		[Route("CAWordGetByID")]
		public async Task<ActionResult<object>> CAWordGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAWord(_appSetting).CAWordGetByID(Id) };
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
		[Route("CAWordGetAll")]
		public async Task<ActionResult<object>> CAWordGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAWord(_appSetting).CAWordGetAll() };
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
		[Route("CAWordGetAllOnPage")]
		public async Task<ActionResult<object>> CAWordGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CAWordOnPage> rsCAWordOnPage = await new CAWord(_appSetting).CAWordGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAWord", rsCAWordOnPage},
						{"TotalCount", rsCAWordOnPage != null && rsCAWordOnPage.Count > 0 ? rsCAWordOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAWordOnPage != null && rsCAWordOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAWordOnPage != null && rsCAWordOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAWordInsert")]
		public async Task<ActionResult<object>> CAWordInsert(CAWord _cAWord)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAWord(_appSetting).CAWordInsert(_cAWord) };
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
		[Route("CAWordListInsert")]
		public async Task<ActionResult<object>> CAWordListInsert(List<CAWord> _cAWords)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAWord _cAWord in _cAWords)
				{
					int? result = await new CAWord(_appSetting).CAWordInsert(_cAWord);
					if (result != null)
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
		[Route("CAWordUpdate")]
		public async Task<ActionResult<object>> CAWordUpdate(CAWord _cAWord)
		{
			try
			{
				int count = await new CAWord(_appSetting).CAWordUpdate(_cAWord);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAWordDelete")]
		public async Task<ActionResult<object>> CAWordDelete(CAWord _cAWord)
		{
			try
			{
				int count = await new CAWord(_appSetting).CAWordDelete(_cAWord);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAWordListDelete")]
		public async Task<ActionResult<object>> CAWordListDelete(List<CAWord> _cAWords)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAWord _cAWord in _cAWords)
				{
					var result = await new CAWord(_appSetting).CAWordDelete(_cAWord);
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
		[Route("CAWordDeleteAll")]
		public async Task<ActionResult<object>> CAWordDeleteAll()
		{
			try
			{
				int count = await new CAWord(_appSetting).CAWordDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("CAWordCount")]
		public async Task<ActionResult<object>> CAWordCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAWord(_appSetting).CAWordCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAWord
	}
}
