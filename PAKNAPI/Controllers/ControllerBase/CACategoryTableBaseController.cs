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
	[Route("api/CACategoryTableBase")]
	[ApiController]
	public class CACategoryTableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public CACategoryTableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region CAGroupWord

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("CAGroupWordGetByID")]
		public async Task<ActionResult<object>> CAGroupWordGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWord(_appSetting).CAGroupWordGetByID(Id) };
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
		[Route("CAGroupWordGetAll")]
		public async Task<ActionResult<object>> CAGroupWordGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWord(_appSetting).CAGroupWordGetAll() };
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
		[Route("CAGroupWordGetAllOnPage")]
		public async Task<ActionResult<object>> CAGroupWordGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<CAGroupWordOnPage> rsCAGroupWordOnPage = await new CAGroupWord(_appSetting).CAGroupWordGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWord", rsCAGroupWordOnPage},
						{"TotalCount", rsCAGroupWordOnPage != null && rsCAGroupWordOnPage.Count > 0 ? rsCAGroupWordOnPage[0].RowNumber : 0},
						{"PageIndex", rsCAGroupWordOnPage != null && rsCAGroupWordOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsCAGroupWordOnPage != null && rsCAGroupWordOnPage.Count > 0 ? PageSize : 0},
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
		[Route("CAGroupWordInsert")]
		public async Task<ActionResult<object>> CAGroupWordInsert(CAGroupWord _cAGroupWord)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWord(_appSetting).CAGroupWordInsert(_cAGroupWord) };
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
		[Route("CAGroupWordListInsert")]
		public async Task<ActionResult<object>> CAGroupWordListInsert(List<CAGroupWord> _cAGroupWords)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAGroupWord _cAGroupWord in _cAGroupWords)
				{
					int? result = await new CAGroupWord(_appSetting).CAGroupWordInsert(_cAGroupWord);
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
		[Authorize("ThePolicy")]
		[Route("CAGroupWordUpdate")]
		public async Task<ActionResult<object>> CAGroupWordUpdate(CAGroupWord _cAGroupWord)
		{
			try
			{
				int count = await new CAGroupWord(_appSetting).CAGroupWordUpdate(_cAGroupWord);
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
		[Authorize("ThePolicy")]
		[Route("CAGroupWordDelete")]
		public async Task<ActionResult<object>> CAGroupWordDelete(CAGroupWord _cAGroupWord)
		{
			try
			{
				int count = await new CAGroupWord(_appSetting).CAGroupWordDelete(_cAGroupWord);
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
		[Authorize("ThePolicy")]
		[Route("CAGroupWordListDelete")]
		public async Task<ActionResult<object>> CAGroupWordListDelete(List<CAGroupWord> _cAGroupWords)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (CAGroupWord _cAGroupWord in _cAGroupWords)
				{
					var result = await new CAGroupWord(_appSetting).CAGroupWordDelete(_cAGroupWord);
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
		[Authorize("ThePolicy")]
		[Route("CAGroupWordDeleteAll")]
		public async Task<ActionResult<object>> CAGroupWordDeleteAll()
		{
			try
			{
				int count = await new CAGroupWord(_appSetting).CAGroupWordDeleteAll();
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

		#endregion CAGroupWord
	}
}
