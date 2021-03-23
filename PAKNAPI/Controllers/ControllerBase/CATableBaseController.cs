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
	[Route("api/CATableBase")]
	[ApiController]
	public class CATableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public CATableBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
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
				return Ok(json);
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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
					return count;
				}
				else
				{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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
				return Ok(json);
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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
					return count;
				}
				else
				{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CADepartmentGroup

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
				return Ok(json);
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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
					return count;
				}
				else
				{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAField

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
				return Ok(json);
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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
					return count;
				}
				else
				{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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
				return Ok(json);
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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
					return count;
				}
				else
				{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAPosition

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
				return Ok(json);
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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
					return count;
				}
				else
				{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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

				return Ok(json);
			}
			catch (Exception ex)
			{
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

					return count;
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAWord
	}
}
