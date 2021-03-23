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
		[Route("CAPositionDeleteBase")]
		public async Task<ActionResult<object>> CAPositionDeleteBase(CAPositionDeleteIN _cAPositionDeleteIN)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = new CAPositionDelete(_appSetting).CAPositionDeleteDAO(_cAPositionDeleteIN) };
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
				return Ok(json);
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
		public async Task<ActionResult<object>> CAPositionGetAllOnPageBase(int? PageSize, int? PageIndex)
		{
			try
			{
				List<CAPositionGetAllOnPage> rsCAPositionGetAllOnPage = await new CAPositionGetAllOnPage(_appSetting).CAPositionGetAllOnPageDAO(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAPositionGetAllOnPage", rsCAPositionGetAllOnPage},
					};
				return Ok(json);
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
		[Route("CAPositionInsertBase")]
		public async Task<ActionResult<object>> CAPositionInsertBase(CAPositionInsertIN _cAPositionInsertIN)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = new CAPositionInsert(_appSetting).CAPositionInsertDAO(_cAPositionInsertIN) };
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
		[Route("CAPositionUpdateBase")]
		public async Task<ActionResult<object>> CAPositionUpdateBase(CAPositionUpdateIN _cAPositionUpdateIN)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = new CAPositionUpdate(_appSetting).CAPositionUpdateDAO(_cAPositionUpdateIN) };
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
				return Ok(json);
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
