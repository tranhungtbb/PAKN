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

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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
					return new ResultApi { Success = ResultSuccess.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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
					return new ResultApi { Success = ResultSuccess.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
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
					return new ResultApi { Success = ResultSuccess.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultSuccess.ORROR, Message = ex.Message };
			}
		}

		#endregion CAPosition
	}
}
