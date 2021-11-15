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
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.Controller
{
	[Route("api/news")]
	[ApiController]
	[ValidateModel]
	public class NewsController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IFileService _fileService;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

		public NewsController(IAppSetting appSetting, IClient bugsnag, IFileService fileService, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_fileService = fileService;
			_configuration = configuration;
		}
		/// <summary>
		/// danh sách tin tức
		/// </summary>
		/// <param name="NewsIds"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Title"></param>
		/// <param name="NewsType"></param>
		/// <param name="Status"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("get-list-news-on-page")]
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
		[Route("get-list-news-relates-forcreate-by-id")]
		public async Task<ActionResult<object>> NENewsRelatesGetAllOnPageBase(int? NewId, string LstNewsId, string Title, int? NewsType, int? PageSize, int? PageIndex)
		{
			try
			{
				List<NENewsGetAllOnPage> rsNENewsGetAllOnPage = await new NENewsGetAllOnPage(_appSetting).NENewsRelatesGetAllOnPageDAO(NewId,LstNewsId, Title, NewsType, PageSize, PageIndex);
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

		/// <summary>
		/// danh sách tin tức trang chủ (slide)
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Route("get-list-news-on-home-page")]
		public async Task<ActionResult<object>> NENewsGetListHome()
		{
			try
			{
				List<NENewsGetListHomePage> rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage();
				
				return new ResultApi { Success = ResultCode.OK, Result = rsPUNewsGetListHomePage };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết  tin tức 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa  tin tức 
		/// </summary>
		/// <param name="_nENewsDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> NENewsDeleteBase(NENewsDeleteIN _nENewsDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, null);
				return new ResultApi { Success = ResultCode.OK, Result = await new NENewsDelete(_appSetting).NENewsDeleteDAO(_nENewsDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới  tin tức 
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert"), DisableRequestSizeLimit]
		public async Task<ActionResult<object>> NENewsInsertBase() // [FromForm] NENewsInsertIN _nENewsInsertIN
		{
			try
			{
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				NENewsInsertIN _nENewsInsertIN = JsonConvert.DeserializeObject<NENewsInsertIN>(Request.Form["data"].ToString(), jss);

				var ErrorMessage = ValidationForFormData.validObject(_nENewsInsertIN);
				if (ErrorMessage != null) {
                    return StatusCode(400, new ResultApi
                    {
                        Success = ResultCode.ORROR,
                        Result = 0,
                        Message = ErrorMessage
					});
                }

				var files = Request.Form.Files;
				if (files.Count == 0) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ảnh đại diện bài viết không được để trống" };
				}
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
				if (res > 0)
				{
					// lịch sử
					var his = new HISNews();
					his.ObjectId = res;
					his.Type = 1;
					his.Status = STATUS_HISNEWS.CREATE;
					await HISNewsInsert(his);
					his.Status = STATUS_HISNEWS.COMPILE;
					await HISNewsInsert(his);
					if (_nENewsInsertIN.Status == 1) {
						his.Status = STATUS_HISNEWS.PUBLIC;
						await HISNewsInsert(his);
					}
					// thông báo
					if (_nENewsInsertIN.IsNotification == true)
					{
						await SYNotificationInsertTypeNews(res, _nENewsInsertIN.Title, true);
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

					return new ResultApi { Success = ResultCode.OK, Result = res, Message = "Thêm mới thành công" };
				}
				else if (res == -1)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Tiêu đề đã tồn tại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Tiêu đề đã tồn tại" };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Thêm mới thất bại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Thêm mới thất bại" };
				}

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		///  cập nhập  tin tức 
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update"), DisableRequestSizeLimit]
		public async Task<ActionResult<object>> NENewsUpdateBase()
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

				var ErrorMessage = ValidationForFormData.validObject(_nENewsUpdateIN);
				if (ErrorMessage != null)
				{
					return StatusCode(400, new ResultApi
					{
						Success = ResultCode.ORROR,
						Result = 0,
						Message = ErrorMessage
					});
				}

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

				int res = Int32.Parse((await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN)).ToString());
				
				if (res > 0)
				{
					var his = new HISNews();
					his.ObjectId = res;
					his.Type = 1;
					his.Status = STATUS_HISNEWS.UPDATE;
					await HISNewsInsert(his);
					if (_nENewsUpdateIN.Status == 1)
					{
						his.Status = STATUS_HISNEWS.PUBLIC;
						await HISNewsInsert(his);
					}
					if (_nENewsUpdateIN.Status == 0)
					{
						his.Status = STATUS_HISNEWS.CANCEL;
						await HISNewsInsert(his);
					}
					// thông báo
					if (_nENewsUpdateIN.IsNotification == true)
					{
						await SYNotificationInsertTypeNews(res, _nENewsUpdateIN.Title, false);
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = res, Message = "Cập nhập thành công" };
				}
				else if (res == -1)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Tiêu đề đã tồn tại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Tiêu đề đã tồn tại" };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, "Cập nhập thất bại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = res, Message = "Cập nhập thất bại" };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// thay đổi trạng thái bài viết
		/// </summary>
		/// <param name="NewsId"></param>
		/// <param name="Status"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("change-status-news")]
		public async Task<ActionResult<object>> NERelateGetAllBase(int? NewsId, int? Status)
		{
			try
			{
				await new NENews(_appSetting).NENewsChangeIsPublish(NewsId, Status);
				return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK};
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết tin tức 
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết tin tức trang chủ
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách tin tức liên quan theo tin tức đó
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		private async Task<bool> HISNewsInsert(HISNews _hISNews)
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
				await new HISNews(_appSetting).HISNewsInsert(_hISNews);
				return true;
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return false;
			}
		}


		private async Task<bool> SYNotificationInsertTypeNews(int Id, string Title, bool isCreateNews)
		{
			try
			{
				//lấy tất cả danh sách người dùng là cá nhân, doanh nghiệp
				List<SYUserGetNonSystem> lstUser = await new SYUserGetNonSystem(_appSetting).SYUserGetNonSystemDAO();
				if (lstUser.Count > 0)
				{
					string senderName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
					foreach (var user in lstUser)
					{
						var model = new SYNotificationModel();
						model.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
						model.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
						model.ReceiveId = user.Id;
						model.ReceiveOrgId = user.UnitId;
						model.DataId = Id;
						model.SendDate = DateTime.Now;
						model.Type = TYPENOTIFICATION.NEWS;
						model.Title = isCreateNews == true ? senderName + " vừa đăng một bài viết mới" : senderName + " vừa cập nhập một bài viết";
						model.Content = Title;
						model.IsViewed = true;
						model.IsReaded = true;
						// insert vào db-
						await new SYNotification(_appSetting,_configuration).InsertNotification(model);
					}
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return false;
			}
		}

		/// <summary>
		/// danh sách lịch sử  tin tức 
		/// </summary>
		/// <param name="NewsId"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
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
