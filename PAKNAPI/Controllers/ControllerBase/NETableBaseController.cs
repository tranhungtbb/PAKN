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
	[Route("api/NETableBase")]
	[ApiController]
	public class NETableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public NETableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region NEFileAttach

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("NEFileAttachGetByID")]
		public async Task<ActionResult<object>> NEFileAttachGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NEFileAttach(_appSetting).NEFileAttachGetByID(Id) };
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
		[Route("NEFileAttachGetAll")]
		public async Task<ActionResult<object>> NEFileAttachGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NEFileAttach(_appSetting).NEFileAttachGetAll() };
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
		[Route("NEFileAttachGetAllOnPage")]
		public async Task<ActionResult<object>> NEFileAttachGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<NEFileAttachOnPage> rsNEFileAttachOnPage = await new NEFileAttach(_appSetting).NEFileAttachGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NEFileAttach", rsNEFileAttachOnPage},
						{"TotalCount", rsNEFileAttachOnPage != null && rsNEFileAttachOnPage.Count > 0 ? rsNEFileAttachOnPage[0].RowNumber : 0},
						{"PageIndex", rsNEFileAttachOnPage != null && rsNEFileAttachOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsNEFileAttachOnPage != null && rsNEFileAttachOnPage.Count > 0 ? PageSize : 0},
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
		[Route("NEFileAttachInsert")]
		public async Task<ActionResult<object>> NEFileAttachInsert(NEFileAttach _nEFileAttach)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new NEFileAttach(_appSetting).NEFileAttachInsert(_nEFileAttach) };
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
		[Route("NEFileAttachListInsert")]
		public async Task<ActionResult<object>> NEFileAttachListInsert(List<NEFileAttach> _nEFileAttachs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (NEFileAttach _nEFileAttach in _nEFileAttachs)
				{
					int? result = await new NEFileAttach(_appSetting).NEFileAttachInsert(_nEFileAttach);
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
		[Route("NEFileAttachUpdate")]
		public async Task<ActionResult<object>> NEFileAttachUpdate(NEFileAttach _nEFileAttach)
		{
			try
			{
				int count = await new NEFileAttach(_appSetting).NEFileAttachUpdate(_nEFileAttach);
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
		[Route("NEFileAttachDelete")]
		public async Task<ActionResult<object>> NEFileAttachDelete(NEFileAttach _nEFileAttach)
		{
			try
			{
				int count = await new NEFileAttach(_appSetting).NEFileAttachDelete(_nEFileAttach);
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
		[Route("NEFileAttachListDelete")]
		public async Task<ActionResult<object>> NEFileAttachListDelete(List<NEFileAttach> _nEFileAttachs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (NEFileAttach _nEFileAttach in _nEFileAttachs)
				{
					var result = await new NEFileAttach(_appSetting).NEFileAttachDelete(_nEFileAttach);
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
		[Route("NEFileAttachDeleteAll")]
		public async Task<ActionResult<object>> NEFileAttachDeleteAll()
		{
			try
			{
				int count = await new NEFileAttach(_appSetting).NEFileAttachDeleteAll();
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
		[Authorize("ThePolicy")]
		[Route("NEFileAttachCount")]
		public async Task<ActionResult<object>> NEFileAttachCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NEFileAttach(_appSetting).NEFileAttachCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion NEFileAttach

		#region NENews

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("NENewsGetByID")]
		public async Task<ActionResult<object>> NENewsGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NENews(_appSetting).NENewsGetByID(Id) };
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
		[Route("NENewsGetAll")]
		public async Task<ActionResult<object>> NENewsGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NENews(_appSetting).NENewsGetAll() };
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
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
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("NENewsCount")]
		public async Task<ActionResult<object>> NENewsCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NENews(_appSetting).NENewsCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion NENews

		#region NERelate

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("NERelateGetByID")]
		public async Task<ActionResult<object>> NERelateGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NERelate(_appSetting).NERelateGetByID(Id) };
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
		[Route("NERelateGetAll")]
		public async Task<ActionResult<object>> NERelateGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NERelate(_appSetting).NERelateGetAll() };
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
		[Route("NERelateGetAllOnPage")]
		public async Task<ActionResult<object>> NERelateGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<NERelateOnPage> rsNERelateOnPage = await new NERelate(_appSetting).NERelateGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NERelate", rsNERelateOnPage},
						{"TotalCount", rsNERelateOnPage != null && rsNERelateOnPage.Count > 0 ? rsNERelateOnPage[0].RowNumber : 0},
						{"PageIndex", rsNERelateOnPage != null && rsNERelateOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsNERelateOnPage != null && rsNERelateOnPage.Count > 0 ? PageSize : 0},
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
		[Route("NERelateInsert")]
		public async Task<ActionResult<object>> NERelateInsert(NERelate _nERelate)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new NERelate(_appSetting).NERelateInsert(_nERelate) };
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
		[Route("NERelateListInsert")]
		public async Task<ActionResult<object>> NERelateListInsert(List<NERelate> _nERelates)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (NERelate _nERelate in _nERelates)
				{
					int? result = await new NERelate(_appSetting).NERelateInsert(_nERelate);
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
		[Route("NERelateUpdate")]
		public async Task<ActionResult<object>> NERelateUpdate(NERelate _nERelate)
		{
			try
			{
				int count = await new NERelate(_appSetting).NERelateUpdate(_nERelate);
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
		[Route("NERelateDelete")]
		public async Task<ActionResult<object>> NERelateDelete(NERelate _nERelate)
		{
			try
			{
				int count = await new NERelate(_appSetting).NERelateDelete(_nERelate);
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
		[Route("NERelateListDelete")]
		public async Task<ActionResult<object>> NERelateListDelete(List<NERelate> _nERelates)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (NERelate _nERelate in _nERelates)
				{
					var result = await new NERelate(_appSetting).NERelateDelete(_nERelate);
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
		[Route("NERelateDeleteAll")]
		public async Task<ActionResult<object>> NERelateDeleteAll()
		{
			try
			{
				int count = await new NERelate(_appSetting).NERelateDeleteAll();
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
		[Authorize("ThePolicy")]
		[Route("NERelateCount")]
		public async Task<ActionResult<object>> NERelateCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new NERelate(_appSetting).NERelateCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion NERelate
	}
}
