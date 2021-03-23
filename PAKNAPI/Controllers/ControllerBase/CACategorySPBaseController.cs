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

		[HttpPost]
		[Authorize]
		[Route("CAUnitDeleteBase")]
		public async Task<ActionResult<object>> CAUnitDeleteBase(CAUnitDeleteIN _cAUnitDeleteIN)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = new CAUnitDelete(_appSetting).CAUnitDeleteDAO(_cAUnitDeleteIN) };
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
		[Route("CAUnitGetAllOnPageBase")]
		public async Task<ActionResult<object>> CAUnitGetAllOnPageBase(int? PageSize, int? PageIndex, string Sort, int? ParentId, byte? Level, string SearchName, string SearchPhone, string SearchEmail, string SearchAddress, bool? SearchActive)
		{
			try
			{
				List<CAUnitGetAllOnPage> rsCAUnitGetAllOnPage = await new CAUnitGetAllOnPage(_appSetting).CAUnitGetAllOnPageDAO(PageSize, PageIndex, Sort, ParentId, Level, SearchName, SearchPhone, SearchEmail, SearchAddress, SearchActive);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAUnitGetAllOnPage", rsCAUnitGetAllOnPage},
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
		[Route("CAUnitInsertBase")]
		public async Task<ActionResult<object>> CAUnitInsertBase(CAUnitInsertIN _cAUnitInsertIN)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = new CAUnitInsert(_appSetting).CAUnitInsertDAO(_cAUnitInsertIN) };
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
		[Route("CAUnitUpdateBase")]
		public async Task<ActionResult<object>> CAUnitUpdateBase(CAUnitUpdateIN _cAUnitUpdateIN)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = new CAUnitUpdate(_appSetting).CAUnitUpdateDAO(_cAUnitUpdateIN) };
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
