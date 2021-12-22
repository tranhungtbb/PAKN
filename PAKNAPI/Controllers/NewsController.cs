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
using System.IO;
using Microsoft.AspNetCore.Hosting;

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
		private readonly IWebHostEnvironment _hostEnvironment;

		public NewsController(IAppSetting appSetting, IClient bugsnag, IFileService fileService, Microsoft.Extensions.Configuration.IConfiguration configuration, IWebHostEnvironment hostEnvironment)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_fileService = fileService;
			_configuration = configuration;
			_hostEnvironment = hostEnvironment;
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
				var files = await new NEFileAttach(_appSetting).NENewsFileGetByNewsIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsGetByID", rsNENewsGetByID},
						{"NENewsFiles" , files }
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
				// delete file
				await new NEFileAttach(_appSetting).NENewsFileDeleteByNewIdDAO(_nENewsDeleteIN.Id);
				// delete folder
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

				var fileAvatar = Request.Form.Files.Where(x=>x.Name == "avatar").ToList();
				if (fileAvatar.Count == 0) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Ảnh đại diện bài viết không được để trống" };
				}
				

				// insert avatar

				string folder = "Upload\\News\\Avatar";
				var folderPath = Path.Combine(_hostEnvironment.ContentRootPath, folder);
				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}
				string filePath = Path.Combine(folder, Path.GetFileName(fileAvatar[0].FileName.Replace("+", "")));


				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					fileAvatar[0].CopyTo(stream);
				}
				_nENewsInsertIN.ImagePath = filePath;

				// insert news
				int res = Int32.Parse((await new NENewsInsert(_appSetting).NENewsInsertDAO(_nENewsInsertIN)).ToString());

				// copy for folder upload
				folder = "Upload\\News\\Media\\" + res;

				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}

				List<Task> tasks = new List<Task>();
				foreach (var item in Request.Form.Files.Where(x => x.Name == "files"))
				{
					NEFileAttach file = new NEFileAttach();
					file.NewsId = res;
					file.Name = Path.GetFileName(item.FileName).Replace("+", "");
					filePath = Path.Combine(folderPath, file.Name);
					file.FileAttach = Path.Combine(folder, file.Name);
					file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						item.CopyTo(stream);
					}
					tasks.Add(new NEFileAttach(_appSetting).NENewsFileInsertDAO(file));
				}
				await Task.WhenAll(tasks);


				// file media



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

				var fileAvatar = Request.Form.Files.Where(x => x.Name == "avatar").ToList();
				string folder = "Upload\\News\\Avatar";
				var folderPath = Path.Combine(_hostEnvironment.ContentRootPath, folder);
				string filePath = string.Empty;

				// avatar
				if (fileAvatar.Count() > 0) {

					deletefile(_nENewsUpdateIN.ImagePath);
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
					filePath = Path.Combine(folder, Path.GetFileName(fileAvatar[0].FileName.Replace("+", "")));
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						fileAvatar[0].CopyTo(stream);
					}
					_nENewsUpdateIN.ImagePath = filePath;
				}

				int res = Int32.Parse((await new NENewsUpdate(_appSetting).NENewsUpdateDAO(_nENewsUpdateIN)).ToString());

				// delete file remove
				List<NEFileAttach> filesDelete = JsonConvert.DeserializeObject<List<NEFileAttach>>(Request.Form["fileDelete"].ToString(), jss);
				foreach (var item in filesDelete) {
					await new NEFileAttach(_appSetting).NENewsFileDeleteDAO(item.Id);
					deletefile(item.FileAttach);
				}

				// insert file


				folder = "Upload\\News\\Media\\" + res;

				if (!Directory.Exists(folderPath))
				{
					Directory.CreateDirectory(folderPath);
				}

				List<Task> tasks = new List<Task>();
				foreach (var item in Request.Form.Files.Where(x => x.Name == "files"))
				{
					NEFileAttach file = new NEFileAttach();
					file.NewsId = res;
					file.Name = Path.GetFileName(item.FileName).Replace("+", "");
					filePath = Path.Combine(folderPath, file.Name);
					file.FileAttach = Path.Combine(folder, file.Name);
					file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						item.CopyTo(stream);
					}
					tasks.Add(new NEFileAttach(_appSetting).NENewsFileInsertDAO(file));
				}
				await Task.WhenAll(tasks);





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
        public async Task<ActionResult<object>> NENewsViewDetailBase(int? Id)
        {
            try
            {
				NENewsViewDetail rsNENewsViewDetail = (await new NENewsViewDetail(_appSetting).NENewsViewDetailDAO(Id)).FirstOrDefault();
				Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
				var files = await new NEFileAttach(_appSetting).NENewsFileGetByNewsIdDAO(Id);
				files.ForEach(item =>
				{
					item.FileAttach = decrypt.EncryptData(item.FileAttach);
				});
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"NENewsViewDetail", rsNENewsViewDetail},
						{"Files", files},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

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
		public async Task<ActionResult<object>> NENewsViewDetailPublicBase(int? Id)
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
					Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
					var files = await new NEFileAttach(_appSetting).NENewsFileGetByNewsIdDAO(Id);
					files.ForEach(item =>
					{
						item.FileAttach = decrypt.EncryptData(item.FileAttach);
					});
					IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"NENewsViewDetail", rsNENewsViewDetail},
                        {"Files", files},
                    };
                    return new ResultApi { Success = ResultCode.OK, Result = json };
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
						_hISNews.Content = userName + " đã khởi tạo thông báo";
						break;
					case STATUS_HISNEWS.UPDATE:
						_hISNews.Content = userName + " đã cập nhập thông báo";
						break;
					case STATUS_HISNEWS.COMPILE:
						_hISNews.Content = userName + " đang soạn thảo thông báo";
						break;
					case STATUS_HISNEWS.PUBLIC:
						_hISNews.Content = userName + " đã công bố thông báo";
						break;
					case STATUS_HISNEWS.CANCEL:
						_hISNews.Content = userName + " đã hủy công bố thông báo";
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
						model.Title = "Bạn vừa nhận được một thông báo từ chính quyền";
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

		private bool deletefile(string fname)
		{
			try
			{
				string _imageToBeDeleted = Path.Combine(_hostEnvironment.WebRootPath, fname);
				if ((System.IO.File.Exists(_imageToBeDeleted)))
				{
					System.IO.File.Delete(_imageToBeDeleted);
				}
				return true;
			}
			catch (Exception ex) { return false; }
		}

	}

}
