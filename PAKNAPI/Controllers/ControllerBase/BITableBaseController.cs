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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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
		[Authorize("ThePolicy")]
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

		#region BIIndividual

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("BIIndividualGetByID")]
		public async Task<ActionResult<object>> BIIndividualGetByID(long? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualGetByID(Id) };
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
		[Route("BIIndividualGetAll")]
		public async Task<ActionResult<object>> BIIndividualGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualGetAll() };
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
		[Route("BIIndividualGetAllOnPage")]
		public async Task<ActionResult<object>> BIIndividualGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<BIIndividualOnPage> rsBIIndividualOnPage = await new BIIndividual(_appSetting).BIIndividualGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividual", rsBIIndividualOnPage},
						{"TotalCount", rsBIIndividualOnPage != null && rsBIIndividualOnPage.Count > 0 ? rsBIIndividualOnPage[0].RowNumber : 0},
						{"PageIndex", rsBIIndividualOnPage != null && rsBIIndividualOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsBIIndividualOnPage != null && rsBIIndividualOnPage.Count > 0 ? PageSize : 0},
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
		[Route("BIIndividualInsert")]
		public async Task<ActionResult<object>> BIIndividualInsert(BIIndividual _bIIndividual)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualInsert(_bIIndividual) };
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
		[Route("BIIndividualListInsert")]
		public async Task<ActionResult<object>> BIIndividualListInsert(List<BIIndividual> _bIIndividuals)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (BIIndividual _bIIndividual in _bIIndividuals)
				{
					int? result = await new BIIndividual(_appSetting).BIIndividualInsert(_bIIndividual);
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
		[Route("BIIndividualUpdate")]
		public async Task<ActionResult<object>> BIIndividualUpdate(BIIndividual _bIIndividual)
		{
			try
			{
				int count = await new BIIndividual(_appSetting).BIIndividualUpdate(_bIIndividual);
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
		[Route("BIIndividualDelete")]
		public async Task<ActionResult<object>> BIIndividualDelete(BIIndividual _bIIndividual)
		{
			try
			{
				int count = await new BIIndividual(_appSetting).BIIndividualDelete(_bIIndividual);
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
		[Route("BIIndividualListDelete")]
		public async Task<ActionResult<object>> BIIndividualListDelete(List<BIIndividual> _bIIndividuals)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (BIIndividual _bIIndividual in _bIIndividuals)
				{
					var result = await new BIIndividual(_appSetting).BIIndividualDelete(_bIIndividual);
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
		[Route("BIIndividualDeleteAll")]
		public async Task<ActionResult<object>> BIIndividualDeleteAll()
		{
			try
			{
				int count = await new BIIndividual(_appSetting).BIIndividualDeleteAll();
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
		[Route("BIIndividualCount")]
		public async Task<ActionResult<object>> BIIndividualCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion BIIndividual
	}
}
