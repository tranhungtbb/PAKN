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
	[Route("api/BITableBase")]
	[ApiController]
	public class BITableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public BITableBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
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
		[Route("BIBusinessUpdate")]
		public async Task<ActionResult<object>> BIBusinessUpdate(BIBusiness _bIBusiness)
		{
			try
			{
				int count = await new BIBusiness(_appSetting).BIBusinessUpdate(_bIBusiness);
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
		[Route("BIBusinessDelete")]
		public async Task<ActionResult<object>> BIBusinessDelete(BIBusiness _bIBusiness)
		{
			try
			{
				int count = await new BIBusiness(_appSetting).BIBusinessDelete(_bIBusiness);
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
		[Route("BIBusinessDeleteAll")]
		public async Task<ActionResult<object>> BIBusinessDeleteAll()
		{
			try
			{
				int count = await new BIBusiness(_appSetting).BIBusinessDeleteAll();
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
		[Route("BIBusinessCount")]
		public async Task<ActionResult<object>> BIBusinessCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusiness(_appSetting).BIBusinessCount() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion BIBusiness

		#region BIIndividual

		[HttpGet]
		[Authorize]
		[Route("BIIndividualGetByID")]
		public async Task<ActionResult<object>> BIIndividualGetByID(long? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("BIIndividualGetAll")]
		public async Task<ActionResult<object>> BIIndividualGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
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
		[Route("BIIndividualUpdate")]
		public async Task<ActionResult<object>> BIIndividualUpdate(BIIndividual _bIIndividual)
		{
			try
			{
				int count = await new BIIndividual(_appSetting).BIIndividualUpdate(_bIIndividual);
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
		[Route("BIIndividualDelete")]
		public async Task<ActionResult<object>> BIIndividualDelete(BIIndividual _bIIndividual)
		{
			try
			{
				int count = await new BIIndividual(_appSetting).BIIndividualDelete(_bIIndividual);
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
		[Route("BIIndividualDeleteAll")]
		public async Task<ActionResult<object>> BIIndividualDeleteAll()
		{
			try
			{
				int count = await new BIIndividual(_appSetting).BIIndividualDeleteAll();
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
		[Route("BIIndividualCount")]
		public async Task<ActionResult<object>> BIIndividualCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividual(_appSetting).BIIndividualCount() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion BIIndividual
	}
}
