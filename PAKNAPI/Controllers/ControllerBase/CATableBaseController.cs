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

		#region CAField

		[HttpGet]
		[Authorize]
		[Route("CAFieldGetByID")]
		public async Task<ActionResult<object>> CAFieldGetByID(int? Id)
		{
			try
			{
				return await new CAField(_appSetting).CAFieldGetByID(Id);
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
				return await new CAField(_appSetting).CAFieldGetAll();
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
				return await new CAField(_appSetting).CAFieldInsert(_cAField);
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
					return count;
				}
				else
				{
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
					return count;
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAField

		#region CAPosition

		[HttpGet]
		[Authorize]
		[Route("CAPositionGetByID")]
		public async Task<ActionResult<object>> CAPositionGetByID(int? Id)
		{
			try
			{
				return await new CAPosition(_appSetting).CAPositionGetByID(Id);
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
				return await new CAPosition(_appSetting).CAPositionGetAll();
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
				return await new CAPosition(_appSetting).CAPositionInsert(_cAPosition);
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
					return count;
				}
				else
				{
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
					return count;
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion CAPosition
	}
}
