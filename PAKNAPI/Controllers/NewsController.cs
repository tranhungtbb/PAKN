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
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.Controller
{
	[Route("api/news")]
	[ApiController]
	public class NewsController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IFileService _fileService;

		public NewsController(IAppSetting appSetting, IClient bugsnag, IFileService fileService)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_fileService = fileService;
		}

		[HttpGet]
		//[Authorize]
		[Route("get-list-group-word-on-page")]
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
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("get-list-group-word-on-home-page")]
		public async Task<ActionResult<object>> NENewsGetListHome()
		{
			try
			{
				List<NENewsGetListHomePage> rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage(true);
				if (rsPUNewsGetListHomePage.Count < 4)
				{
					rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage(false);
				}
				return new ResultApi { Success = ResultCode.OK, Result = rsPUNewsGetListHomePage };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("get-by-id")]
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

		[HttpPost]
		[Authorize]
		[Route("delete")]
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

		[HttpPost]
		[Authorize]
		[Route("insert"), DisableRequestSizeLimit]
		public async Task<ActionResult<object>> NENewsInsertBase()
		{
			try
			{
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
				int res = Int32.Parse((await new NENewsInsert(_appSetting).NENewsInsertDAO(_nENewsInsertIN)).ToString());
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				if (res > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = res, Message = "Thêm mới thành công" };
				}
				else if (res == -1)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Tiêu đề bị trùng" };
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Thêm mới thất bại" };
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
		[Route("update"), DisableRequestSizeLimit]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);


				int res = Int32.Parse((await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN)).ToString());
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				if (res > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = res, Message = "Cập nhập thành công" };
				}
				else if (res == -1)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Tiêu đề bị trùng" };
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Cập nhập thất bại" };
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
		[Route("get-list-relates")]
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


		[HttpGet]
		[Route("get-detail")]
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
		[Route("get-detail-public")]
		public async Task<ActionResult<object>> NENewsViewDetailPublicBase(long? Id)
		{
			try
			{
				NENewsViewDetail rsNENewsViewDetail = (await new NENewsViewDetail(_appSetting).NENewsViewDetailDAO(Id)).FirstOrDefault();
				if (rsNENewsViewDetail == null)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = -1, Message = "Không tồn tại bài viết" };
				}
				if (rsNENewsViewDetail.Status != 1)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = 0, Message = "Bài viết chưa được công bố" };
				}
				else
				{
					return new ResultApi { Success = ResultCode.OK, Result = rsNENewsViewDetail };
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
		[Route("get-list-relates-by-id")]
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


		[HttpPost]
		[Authorize]
		[Route("insert-his")]
		public async Task<ActionResult<object>> HISNewsInsert(HISNews _hISNews)
		{
			try
			{
				_hISNews.CreatedBy = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				_hISNews.CreatedDate = DateTime.Now;
				string userName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);

				switch (_hISNews.Status)
				{
					case STATUS_HISNEWS.CREATE:
						_hISNews.Content = userName + " đã khởi tạo bài viết";
						break;
					case STATUS_HISNEWS.UPDATE:
						_hISNews.Content = userName + " đã cập nhập bài viết";
						break;
					case STATUS_HISNEWS.COMPILE:
						_hISNews.Content = userName + " đang soạn thảo bài viết";
						break;
					case STATUS_HISNEWS.PUBLIC:
						_hISNews.Content = userName + " đã công bố bài viết";
						break;
					case STATUS_HISNEWS.CANCEL:
						_hISNews.Content = userName + " đã hủy công bố bài viết";
						break;
				}


				return new ResultApi { Success = ResultCode.OK, Result = await new HISNews(_appSetting).HISNewsInsert(_hISNews) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize]
		[Route("get-list-his-on-page")]
		public async Task<ActionResult<object>> HISNewsGetByNewsId(int NewsId)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISNewsModel(_appSetting).HISNewsGetByNewsId(NewsId) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



	}
}
