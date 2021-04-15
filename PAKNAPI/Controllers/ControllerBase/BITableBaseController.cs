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
	[Route("api/BITableBase")]
	[ApiController]
	public class BITableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public BITableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region BIBusiness

		[HttpGet]
		[Authorize]
		[Route("BIBusinessGetByID")]
		public async Task<ActionResult<object>> BIBusinessGetByID(long? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusiness(_appSetting).BIBusinessGetByID(Id) };
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
		[Route("BIBusinessGetAll")]
		public async Task<ActionResult<object>> BIBusinessGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusiness(_appSetting).BIBusinessGetAll() };
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
		[Route("BIBusinessGetAllOnPage")]
		public async Task<ActionResult<object>> BIBusinessGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<BIBusinessOnPage> rsBIBusinessOnPage = await new BIBusiness(_appSetting).BIBusinessGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusiness", rsBIBusinessOnPage},
						{"TotalCount", rsBIBusinessOnPage != null && rsBIBusinessOnPage.Count > 0 ? rsBIBusinessOnPage[0].RowNumber : 0},
						{"PageIndex", rsBIBusinessOnPage != null && rsBIBusinessOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsBIBusinessOnPage != null && rsBIBusinessOnPage.Count > 0 ? PageSize : 0},
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
		[Route("BIBusinessInsert")]
		public async Task<ActionResult<object>> BIBusinessInsert(BIBusiness _bIBusiness)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusiness(_appSetting).BIBusinessInsert(_bIBusiness) };
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
		[Route("BIBusinessListInsert")]
		public async Task<ActionResult<object>> BIBusinessListInsert(List<BIBusiness> _bIBusinesss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (BIBusiness _bIBusiness in _bIBusinesss)
				{
					int? result = await new BIBusiness(_appSetting).BIBusinessInsert(_bIBusiness);
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
		[Route("BIBusinessUpdate")]
		public async Task<ActionResult<object>> BIBusinessUpdate(BIBusiness _bIBusiness)
		{
			try
			{
				int count = await new BIBusiness(_appSetting).BIBusinessUpdate(_bIBusiness);
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
		[Route("BIBusinessDelete")]
		public async Task<ActionResult<object>> BIBusinessDelete(BIBusiness _bIBusiness)
		{
			try
			{
				int count = await new BIBusiness(_appSetting).BIBusinessDelete(_bIBusiness);
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
		[Route("BIBusinessListDelete")]
		public async Task<ActionResult<object>> BIBusinessListDelete(List<BIBusiness> _bIBusinesss)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (BIBusiness _bIBusiness in _bIBusinesss)
				{
					var result = await new BIBusiness(_appSetting).BIBusinessDelete(_bIBusiness);
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
		[Route("BIBusinessDeleteAll")]
		public async Task<ActionResult<object>> BIBusinessDeleteAll()
		{
			try
			{
				int count = await new BIBusiness(_appSetting).BIBusinessDeleteAll();
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
		[Route("BIBusinessCount")]
		public async Task<ActionResult<object>> BIBusinessCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusiness(_appSetting).BIBusinessCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion BIBusiness
