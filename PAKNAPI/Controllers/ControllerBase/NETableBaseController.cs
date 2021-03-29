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
	[Route("api/NETableBase")]
	[ApiController]
	public class NETableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public NETableBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		#region NENews

		[HttpGet]
		[Authorize]
		[Route("NENewsGetByID")]
		public async Task<ActionResult<object>> NENewsGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NENews(_appSetting).NENewsGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("NENewsGetAll")]
		public async Task<ActionResult<object>> NENewsGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NENews(_appSetting).NENewsGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("NENewsGetAllOnPage")]
		public async Task<ActionResult<object>> NENewsGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<NENewsOnPage> rsNENewsOnPage = await new NENews(_appSetting).NENewsGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENews", rsNENewsOnPage},
						{"TotalCount", rsNENewsOnPage != null && rsNENewsOnPage.Count > 0 ? rsNENewsOnPage[0].RowNumber : 0},
						{"PageIndex", rsNENewsOnPage != null && rsNENewsOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsNENewsOnPage != null && rsNENewsOnPage.Count > 0 ? PageSize : 0},
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
		[Route("NENewsInsert")]
		public async Task<ActionResult<object>> NENewsInsert(NENews _nENews)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new NENews(_appSetting).NENewsInsert(_nENews) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("NENewsListInsert")]
		public async Task<ActionResult<object>> NENewsListInsert(List<NENews> _nENewss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (NENews _nENews in _nENewss)
				{
					int? result = await new NENews(_appSetting).NENewsInsert(_nENews);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("NENewsUpdate")]
		public async Task<ActionResult<object>> NENewsUpdate(NENews _nENews)
		{
			try
			{
				int count = await new NENews(_appSetting).NENewsUpdate(_nENews);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("NENewsDelete")]
		public async Task<ActionResult<object>> NENewsDelete(NENews _nENews)
		{
			try
			{
				int count = await new NENews(_appSetting).NENewsDelete(_nENews);
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("NENewsListDelete")]
		public async Task<ActionResult<object>> NENewsListDelete(List<NENews> _nENewss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (NENews _nENews in _nENewss)
				{
					var result = await new NENews(_appSetting).NENewsDelete(_nENews);
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
		[Route("NENewsDeleteAll")]
		public async Task<ActionResult<object>> NENewsDeleteAll()
		{
			try
			{
				int count = await new NENews(_appSetting).NENewsDeleteAll();
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion NENews
	}
}
