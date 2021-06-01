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
using PAKNAPI.Services.FileUpload;
using Microsoft.AspNetCore.Http;

namespace PAKNAPI.ControllerBase
{
	[Route("api/NESPBase")]
	[ApiController]
	public class NESPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IFileService _fileService;

		public NESPBaseController(IAppSetting appSetting, IClient bugsnag, IFileService fileService)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_fileService = fileService;
		}

		[HttpGet]
		//[Authorize]
		[Route("NENewsGetAllOnPageBase")]
		public async Task<ActionResult<object>> NENewsGetAllOnPageBase(string NewsIds, int? PageSize, int? PageIndex, string Title, int? NewsType, int? Status)
		{
			try
			{
				List<NENewsGetAllOnPage> rsNENewsGetAllOnPage = await new NENewsGetAllOnPage(_appSetting).NENewsGetAllOnPageDAO(NewsIds, PageSize, PageIndex, Title, NewsType, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetAllOnPage", rsNENewsGetAllOnPage},
						{"TotalCount", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? rsNENewsGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsNENewsGetAllOnPage != null && rsNENewsGetAllOnPage.Count > 0 ? PageSize : 0},
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
		[Route("NENewsDeleteBase")]
		public async Task<ActionResult<object>> NENewsDeleteBase(NENewsDeleteIN _nENewsDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new NENewsDelete(_appSetting).NENewsDeleteDAO(_nENewsDeleteIN) };
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
		[Route("NENewsGetAllRelatesBase")]
		public async Task<ActionResult<object>> NENewsGetAllRelatesBase(long? Id)
		{
			try
			{
				List<NENewsGetAllRelates> rsNENewsGetAllRelates = await new NENewsGetAllRelates(_appSetting).NENewsGetAllRelatesDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetAllRelates", rsNENewsGetAllRelates},
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

		[HttpGet]
		[Authorize]
		[Route("NENewsGetByIDBase")]
		public async Task<ActionResult<object>> NENewsGetByIDBase(int? Id)
		{
			try
			{
				List<NENewsGetByID> rsNENewsGetByID = await new NENewsGetByID(_appSetting).NENewsGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetByID", rsNENewsGetByID},
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

		[HttpGet]
		[Authorize]
		[Route("NENewsGetByIDOnJoinBase")]
		public async Task<ActionResult<object>> NENewsGetByIDOnJoinBase(int? Id)
		{
			try
			{
				List<NENewsGetByIDOnJoin> rsNENewsGetByIDOnJoin = await new NENewsGetByIDOnJoin(_appSetting).NENewsGetByIDOnJoinDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetByIDOnJoin", rsNENewsGetByIDOnJoin},
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
		[Route("NENewsInsertBase"),DisableRequestSizeLimit]
		public async Task<ActionResult<object>> NENewsInsertBase(/*NENewsInsertIN _nENewsInsertIN*/)
		{
			try
			{
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				var files = Request.Form.Files;
				NENewsInsertIN _nENewsInsertIN = JsonConvert.DeserializeObject<NENewsInsertIN>(Request.Form["data"].ToString(), jss);

				string avatarFilePath = null;
				if (files != null && files.Any())
				{
					var listFile = await _fileService.Save(files, $"News");
					avatarFilePath = listFile[0]?.Path;

				}

				if (!string.IsNullOrEmpty(avatarFilePath))
				{
					_nENewsInsertIN.ImagePath = avatarFilePath;
				}

				return new ResultApi { Success = ResultCode.OK, Result = await new NENewsInsert(_appSetting).NENewsInsertDAO(_nENewsInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		//[HttpPost]
		//[Authorize]
		//[Route("NENewsUpdateBase")]
		//public async Task<ActionResult<object>> NENewsUpdateBase(NENewsUpdateIN _nENewsUpdateIN)
		//{
		//	try
		//	{
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

		//		return new ResultApi { Success = ResultCode.OK, Result = await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN) };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}
		[HttpPost]
		[Authorize]
		[Route("NENewsUpdateBase"),DisableRequestSizeLimit]
		public async Task<ActionResult<object>> NENewsUpdateBase(
			//[FromForm] NENewsUpdateIN _nENewsUpdateIN
		)
		{
			try
			{
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				var files = Request.Form.Files;
				NENewsUpdateIN _nENewsUpdateIN = JsonConvert.DeserializeObject<NENewsUpdateIN>(Request.Form["data"].ToString(), jss);

				string avatarFilePath = null;
				if (files != null && files.Any())
				{
					var listFile = await _fileService.Save(files, $"News/{_nENewsUpdateIN.Id}");
					avatarFilePath = listFile[0]?.Path;

				}

				if (!string.IsNullOrEmpty(avatarFilePath))
                {
					var rs = await _fileService.Remove(_nENewsUpdateIN.ImagePath);
					_nENewsUpdateIN.ImagePath = avatarFilePath;
				}

				return new ResultApi { Success = ResultCode.OK, Result = await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("NENewsViewDetailBase")]
		public async Task<ActionResult<object>> NENewsViewDetailBase(long? Id)
		{
			try
			{
				List<NENewsViewDetail> rsNENewsViewDetail = await new NENewsViewDetail(_appSetting).NENewsViewDetailDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsViewDetail", rsNENewsViewDetail},
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

		[HttpGet]
		[Authorize]
		[Route("NERelateGetAllBase")]
		public async Task<ActionResult<object>> NERelateGetAllBase(int? NewsId)
		{
			try
			{
				List<NERelateGetAll> rsNERelateGetAll = await new NERelateGetAll(_appSetting).NERelateGetAllDAO(NewsId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NERelateGetAll", rsNERelateGetAll},
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
	}
}
