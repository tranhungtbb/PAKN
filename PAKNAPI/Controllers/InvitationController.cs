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
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Invitation;
using PAKNAPI.Job;
using System.Threading;


namespace PAKNAPI.Controllers
{
    [Route("api/invitation")]
    [ApiController]
	[ValidateModel]
	public class InvitationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public InvitationController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }
		/// <summary>
		/// xóa thư mời
		/// </summary>
		/// <param name="_iNVInvitationDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> INVInvitationDeleteBase(INVInvitationDeleteIN _iNVInvitationDeleteIN)
		{
			try
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				// delete file
				List <INVFileAttachGetAllByInvitationId> rsINVFileAttachGetAllByInvitationId = await new INVFileAttachGetAllByInvitationId(_appSetting).INVFileAttachGetAllByInvitationIdDAO(_iNVInvitationDeleteIN.Id);

				foreach (INVFileAttachGetAllByInvitationId item in rsINVFileAttachGetAllByInvitationId) {
					// xóa file trong folder
					deletefile(item.FileAttach);
				}
				// xóa trong db
				INVFileAttachDeleteByInvitationIdIN model = new INVFileAttachDeleteByInvitationIdIN();
				model.InvitationId = _iNVInvitationDeleteIN.Id;
				await new INVFileAttachDeleteByInvitationId(_appSetting).INVFileAttachDeleteByInvitationIdDAO(model);

				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = await new INVInvitationDelete(_appSetting).INVInvitationDeleteDAO(_iNVInvitationDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách người dùng đã xem thư mời
		/// </summary>
		/// <param name="InvitationId"></param>
		/// <param name="UserName"></param>
		/// <param name="WatchedDate"></param>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-user-readed-invitation-on-page")]
		public async Task<ActionResult<object>> SYUserReadedInvitationGetAllOnPage(int InvitationId, string UserName, DateTime? WatchedDate, int? PageSize, int? PageIndex)
		{
			try
			{
				List<SYUserReadedInvitationGetAllOnPage> syUserReadedInvitationGetAllOnPage = await new SYUserReadedInvitationGetAllOnPage(_appSetting).INVInvitationGetAllOnPageDAO(InvitationId, UserName, WatchedDate, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserReadedInvitationGetAllOnPage", syUserReadedInvitationGetAllOnPage},
						{"TotalCount", syUserReadedInvitationGetAllOnPage != null && syUserReadedInvitationGetAllOnPage.Count > 0 ? syUserReadedInvitationGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", syUserReadedInvitationGetAllOnPage != null && syUserReadedInvitationGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", syUserReadedInvitationGetAllOnPage != null && syUserReadedInvitationGetAllOnPage.Count > 0 ? PageSize : 0},
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

		/// <summary>
		/// danh sách thư mời
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Title"></param>
		/// <param name="StartDate"></param>
		/// <param name="EndDate"></param>
		/// <param name="Place"></param>
		/// <param name="Status"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-invitation-on-page")]
		public async Task<ActionResult<object>> INVInvitationGetAllOnPageBase(int? PageSize, int? PageIndex, string Title, DateTime? StartDate, DateTime? EndDate, string Place, byte? Status)
		{
			try
			{
				long UserProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				List<INVInvitationGetAllOnPage> rsINVInvitationGetAllOnPage = await new INVInvitationGetAllOnPage(_appSetting).INVInvitationGetAllOnPageDAO(PageSize, PageIndex, Title, StartDate, EndDate, Place, Status, UserProcessId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"INVInvitationGetAllOnPage", rsINVInvitationGetAllOnPage},
						{"TotalCount", rsINVInvitationGetAllOnPage != null && rsINVInvitationGetAllOnPage.Count > 0 ? rsINVInvitationGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsINVInvitationGetAllOnPage != null && rsINVInvitationGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsINVInvitationGetAllOnPage != null && rsINVInvitationGetAllOnPage.Count > 0 ? PageSize : 0},
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
		/// <summary>
		/// thêm mới thư mời
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<object> INVInvitationInsert()
		{
			try
			{
				INVInvitationInsertModel invInvitation = new INVInvitationInsertModel();
				invInvitation.Model = JsonConvert.DeserializeObject<INVInvitationInsertIN>(Request.Form["Model"].ToString());

				var ErrorMessage = ValidationForFormData.validObject(invInvitation.Model);
				if (ErrorMessage != null)
				{
					return StatusCode(400, new ResultApi
					{
						Success = ResultCode.ORROR,
						Result = 0,
						Message = ErrorMessage
					});
				}

				invInvitation.Model.CreateDate = DateTime.Now;
				invInvitation.Model.UserCreateId = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);

				invInvitation.InvitationUserMap = JsonConvert.DeserializeObject<List<INVInvitationUserMapInsertIN>>(Request.Form["InvitationUserMap"].ToString());
				invInvitation.Files = Request.Form.Files;

				if (invInvitation.Model.Status == 2 && invInvitation.InvitationUserMap.Count == 0)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = 0, Message = "Vui lòng chọn thành phần tham dự" };
				}

				invInvitation.Model.IsView = 0;
				invInvitation.Model.Member = invInvitation.InvitationUserMap == null ? 0 : invInvitation.InvitationUserMap.Count();
				
				int id = Int32.Parse((await new INVInvitationInsert(_appSetting).INVInvitationInsertDAO(invInvitation.Model)).ToString());

				if (id > 0)
				{
					// insert file và InvitationMap
					List<string> filesPath = new List<string>();
					if (invInvitation.Files != null && invInvitation.Files.Count > 0)
					{
						string folder = "Upload\\Invitation\\" + id;
						string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
						if (!Directory.Exists(folderPath))
						{
							Directory.CreateDirectory(folderPath);
						}
						foreach (var item in invInvitation.Files)
						{
							INVFileAttachInsertIN file = new INVFileAttachInsertIN();
							file.InvitationId = id;
							file.Name = Path.GetFileName(item.FileName).Replace("+", "");
							string filePath = Path.Combine(folderPath, file.Name);
							filesPath.Add(filePath);
							file.FileAttach = Path.Combine(folder, file.Name);
							file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
							using (var stream = new FileStream(filePath, FileMode.Create))
							{
								item.CopyTo(stream);
							}
							await new INVFileAttachInsert(_appSetting).INVFileAttachInsertDAO(file);
						}

					}

					// insert map user
					Dictionary<string, string> lstUserSend = new Dictionary<string, string>();
																 
					string senderName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
					foreach (var item in invInvitation.InvitationUserMap) {
						item.InvitationId = id;
						await new INVInvitationUserMapInsert(_appSetting).INVInvitationUserMapInsertDAO(item);

						// tạo thông báo
						if (invInvitation.Model.Status == 2) { // gửi
							var model = new SYNotificationModel();
							model.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
							model.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
							model.ReceiveId = (int)item.UserId;
							//model.ReceiveOrgId = user.UnitId;
							model.DataId = id;
							model.SendDate = DateTime.Now;
							model.Type = TYPENOTIFICATION.INVITATION;
							model.Title = "Bạn vừa nhận được một thư mời từ " + senderName;
							model.Content = invInvitation.Model.Title;
							model.IsViewed = true;
							model.IsReaded = true;
							// insert vào db-
							await new SYNotification(_appSetting).SYNotificationInsertDAO(model);

							if (item.SendEmail == true) {
								var userSend = await new SYUser(_appSetting).SYUserGetByID(item.UserId);
								if (userSend != null && userSend.IsActived == true) { lstUserSend.Add(userSend.FullName,userSend.Email); }
							}
						}
						// send email nếu có
					}
					// send mail
					if (lstUserSend.Count() > 0) {
						Thread t = new Thread(async () => {
							var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL)).FirstOrDefault();
							if (config != null) {
								var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config.Content);
								string content = "<p><span style='font-family:times new roman,times,serif;'><strong>Kính gửi: </strong>&nbsp;{FullName},<br />" +
										"Thư mời họp <br />" +
										"<strong> Thời gian bắt đầu </strong > : {StartDate}<br />" +
										"<strong> Thời gian kết thúc </strong > : {EndDate}<br />" +
										"<strong> Địa điểm </strong > : {Place}<br />" +
										"<strong> Nội dung </strong > :<br />" +
										"{Content}</span></p>";
								content = content.Replace("{StartDate}", invInvitation.Model.StartDate.ToString("dd/MM/yyyy HH:ss"));
								content = content.Replace("{EndDate}", invInvitation.Model.EndDate.ToString("dd/MM/yyyy HH:ss"));
								content = content.Replace("{Place}", invInvitation.Model.Place);
								content = content.Replace("{Content}", invInvitation.Model.Content);
								//content = content.Replace("\n", "<br />");
								foreach (var item in lstUserSend) {
									string itemContent = content.Replace("{FullName}", item.Key);
									itemContent = itemContent.Replace("\n", "<br />");
									MailHelper.SendMail(configEmail, item.Value, invInvitation.Model.Title, itemContent, filesPath);
								}
							}
						});
						t.Start();
					}
					// insert his
					var history = new HISInsertIN();
					history.ObjectId = id;
					history.CreatedBy = invInvitation.Model.UserCreateId;
					history.Content = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext) + " đã khởi tạo thư mời.";
					history.CreatedDate = DateTime.Now;
					history.Status = STATUS_HIS_INVITATION.CREATE;

					await new HISInvitationInsert(_appSetting).HISSMSInsertDAO(history);

					if (invInvitation.Model.Status == 2) // is send
					{
						history.Content = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext) + " đã gửi thư mời.";
						history.Status = STATUS_HIS_INVITATION.SEND;
						await new HISInvitationInsert(_appSetting).HISSMSInsertDAO(history);
					}

					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK};

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "Tiêu đề thư mời đã tồn tại" };
				}
			}
			catch (Exception ex) {
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// chi tiết thư mời
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-detail")]
		public async Task<object> INVInvitationDetail(int id)
		{
			try
			{
				Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
				INVInvitationDetailModel invInvitation = new INVInvitationDetailModel();
				invInvitation.Model = (await new INVInvitationDetail(_appSetting).INVInvitationDetailDAO(id, new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext))).FirstOrDefault();
				invInvitation.INVFileAttach
					 = await new INVFileAttachGetAllByInvitationId(_appSetting).INVFileAttachGetAllByInvitationIdDAO(id);
				invInvitation.INVFileAttach.ForEach(item => {
					item.FileAttach = decrypt.EncryptData(item.FileAttach);
				});
				var user = new SYUser();
				if (invInvitation.Model.UserUpdate != null)
				{
					user = await new SYUser(_appSetting).SYUserGetByID(invInvitation.Model.UserUpdate);
					if (user != null) {
						invInvitation.SenderName = user.FullName;
					}
				}
				else {
					user = await new SYUser(_appSetting).SYUserGetByID(invInvitation.Model.UserCreateId);
					if (user != null)
					{
						invInvitation.SenderName = user.FullName;
					}
				}
				// 
				return new ResultApi { Success = ResultCode.OK, Result = invInvitation };
			}
			catch (Exception ex)
			{
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết thư mời (để update)
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<object> INVInvitationUpdate(int id)
		{
			try
			{
				INVInvitationUpdateModel invInvitation = new INVInvitationUpdateModel();
				invInvitation.Model = (await new INVInvitationGetById(_appSetting).INVInvitationGetByIdDAO(id)).FirstOrDefault();
				invInvitation.InvitationUserMap = await new INVInvitationUserMapGetByInvitationId(_appSetting).INVInvitationUserMapGetByInvitationIdDAO(id);
				invInvitation.INVFileAttach
					 = await new INVFileAttachGetAllByInvitationId(_appSetting).INVFileAttachGetAllByInvitationIdDAO(id);

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK , Result = invInvitation };
			}
			catch (Exception ex)
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập thư mời
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<object> INVInvitationUpdate()
		{
			try
			{
				INVInvitationUpdateModel invInvitation = new INVInvitationUpdateModel();
				invInvitation.Model = JsonConvert.DeserializeObject<INVInvitationGetById>(Request.Form["Model"].ToString());

				var ErrorMessage = ValidationForFormData.validObject(invInvitation.Model);
				if (ErrorMessage != null)
				{
					return StatusCode(400, new ResultApi
					{
						Success = ResultCode.ORROR,
						Result = 0,
						Message = ErrorMessage
					});
				}
				invInvitation.InvitationUserMap = JsonConvert.DeserializeObject<List<INVInvitationUserMapGetByInvitationId>>(Request.Form["InvitationUserMap"].ToString());
				invInvitation.LtsDeleteFile = JsonConvert.DeserializeObject<List<InvitationFileAttach>>(Request.Form["LstFileDelete"].ToString());
				invInvitation.Files = Request.Form.Files;

				if (invInvitation.Model.Status == 2 && invInvitation.InvitationUserMap.Count ==0) {
					return new ResultApi { Success = ResultCode.ORROR, Result = invInvitation.Model.Id, Message = "Vui lòng chọn thành phần tham dự" };
				}

				INVInvitationUpdateIN invUpdate = new INVInvitationUpdateIN();
				invUpdate.Id = invInvitation.Model.Id;
				invUpdate.Title = invInvitation.Model.Title;
				invUpdate.StartDate = invInvitation.Model.StartDate;
				invUpdate.EndDate = invInvitation.Model.EndDate;
				invUpdate.Content = invInvitation.Model.Content;

				invUpdate.Place = invInvitation.Model.Place;
				invUpdate.Note = invInvitation.Model.Note;
				invUpdate.Status = invInvitation.Model.Status;
				invUpdate.Content = invInvitation.Model.Content;
				invUpdate.UpdateDate = DateTime.Now;
				invUpdate.IsView = (int)invInvitation.Model.IsView;
				invUpdate.UserUpdate = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				invUpdate.Member = invInvitation.InvitationUserMap == null ? 0 : invInvitation.InvitationUserMap.Count();

				int id = Int32.Parse((await new INVInvitationUpdate(_appSetting).INVInvitationUpdateDAO(invUpdate)).ToString());

				if (id > 0)
				{
					List<string> filesPath = new List<string>();
					if (invInvitation.LtsDeleteFile.Count > 0)
					{
						foreach (var item in invInvitation.LtsDeleteFile)
						{
							deletefile(item.FileAttach);
							INVFileAttachDeleteByIdIN fileDel = new INVFileAttachDeleteByIdIN();
							fileDel.Id = item.Id;
							await new INVFileAttachDeleteById(_appSetting).INVFileAttachDeleteByIdDAO(fileDel);
						}

					}
					// insert file và InvitationMap
					if (invInvitation.Files != null && invInvitation.Files.Count > 0)
					{
						string folder = "Upload\\Invitation\\" + invInvitation.Model.Id;
						string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
						if (!Directory.Exists(folderPath))
						{
							Directory.CreateDirectory(folderPath);
						}
						foreach (var item in invInvitation.Files)
						{
							INVFileAttachInsertIN file = new INVFileAttachInsertIN();
							file.InvitationId = id;
							file.Name = Path.GetFileName(item.FileName).Replace("+", "");
							string filePath = Path.Combine(folderPath, file.Name);
							filesPath.Add(filePath);
							file.FileAttach = Path.Combine(folder, file.Name);
							file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
							using (var stream = new FileStream(filePath, FileMode.Create))
							{
								item.CopyTo(stream);
							}
							await new INVFileAttachInsert(_appSetting).INVFileAttachInsertDAO(file);
						}

					}
					// delete map
					INVInvitationUserMapDeleteByInvitationIdIN deleteMap = new INVInvitationUserMapDeleteByInvitationIdIN();
					deleteMap.InvitationId = invInvitation.Model.Id;
					await new INVInvitationUserMapDeleteByInvitationId(_appSetting).INVInvitationUserMapDeleteByInvitationIdDAO(deleteMap);
					Dictionary<string, string> lstUserSend = new Dictionary<string, string>();
					// insert map user
					if (true) {
						string senderName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
						foreach (var item in invInvitation.InvitationUserMap)
						{
							INVInvitationUserMapInsertIN ins = new INVInvitationUserMapInsertIN();
							ins.InvitationId = invInvitation.Model.Id;
							ins.SendSMS = item.SendSMS;
							ins.SendEmail = item.SendEmail;
							ins.UserId = item.UserId;
							ins.Watched = item.Watched;
							await new INVInvitationUserMapInsert(_appSetting).INVInvitationUserMapInsertDAO(ins);

							// tạo thông báo
							if (invInvitation.Model.Status == 2)
							{ // gửi
								var model = new SYNotificationModel();
								model.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
								model.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
								model.ReceiveId = (int)item.UserId;
								//model.ReceiveOrgId = user.UnitId;
								model.DataId = id;
								model.SendDate = DateTime.Now;
								model.Type = TYPENOTIFICATION.INVITATION;
								model.Title = "Bạn vừa nhận được một thư mời từ " + senderName;
								model.Content = invInvitation.Model.Title;
								model.IsViewed = true;
								model.IsReaded = true;
								// insert vào db-
								await new SYNotification(_appSetting).SYNotificationInsertDAO(model);

								if (item.SendEmail == true)
								{
									var userSend = await new SYUser(_appSetting).SYUserGetByID(item.UserId);
									if (userSend != null && userSend.IsActived == true) { lstUserSend.Add(userSend.FullName, userSend.Email); }
								}
							}
							// send email nếu có
						}
						// send mail
						if (lstUserSend.Count() > 0)
						{
							Thread t = new Thread(async () => {
								var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL)).FirstOrDefault();
								if (config != null)
								{
									var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config.Content);
									string content = "<p><span style='font-family:times new roman,times,serif;'><strong>Kính gửi: </strong>&nbsp;{FullName},<br />" +
											"Thư mời họp <br />" +
											"<strong> Thời gian bắt đầu </strong > : {StartDate}<br />" +
											"<strong> Thời gian kết thúc </strong > : {EndDate}<br />" +
											"<strong> Địa điểm </strong > : {Place}<br />" +
											"<strong> Nội dung </strong > :<br />" +
											"{Content}</span></p>";
									content = content.Replace("{StartDate}", invInvitation.Model.StartDate.ToString("dd/MM/yyyy HH:ss"));
									content = content.Replace("{EndDate}", invInvitation.Model.EndDate.ToString("dd/MM/yyyy HH:ss"));
									content = content.Replace("{Place}", invInvitation.Model.Place);
									content = content.Replace("{Content}", invInvitation.Model.Content);
									foreach (var item in lstUserSend)
									{
										string itemContent = content.Replace("{FullName}", item.Key);
										itemContent = itemContent.Replace("\n", "<br />");
										MailHelper.SendMail(configEmail, item.Value, invInvitation.Model.Title, itemContent, null);
									}
								}
							});
							t.Start();
						}
					}

					// insert his
					var history = new HISInsertIN();
					history.ObjectId = id;
					history.CreatedBy = invInvitation.Model.UserCreateId;
					history.CreatedDate = DateTime.Now;


					if (invInvitation.Model.Status == 1) // is update
					{
						history.Content = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext) + " đã cập nhập mời.";
						history.Status = STATUS_HIS_INVITATION.UPDATE;
						
					}
					else if(invInvitation.Model.Status == 2) {
						history.Content = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext) + " đã gửi thư mời.";
						history.Status = STATUS_HIS_INVITATION.SEND;
					}

					await new HISInvitationInsert(_appSetting).HISSMSInsertDAO(history);

					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK };

				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "Tiêu đề thư mời đã tồn tại" };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// lịch sử thư mời
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="ObjectId"></param>
		/// <param name="Content"></param>
		/// <param name="UserName"></param>
		/// <param name="CreateDate"></param>
		/// <param name="Status"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-his")]
		public async Task<ActionResult<object>> HISInvitationGetByInvitationIdOnPageBase(int? PageSize, int? PageIndex, int? ObjectId, string Content, string UserName, DateTime? CreateDate, int? Status)
		{
			try
			{
				List<HISInvitationGetByInvitationIdOnPage> rsHIS = await new HISInvitationGetByInvitationIdOnPage(_appSetting).HISInvitationGetByInvitationIdOnPageDAO(PageSize, PageIndex, ObjectId, Content, UserName, CreateDate, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISInvitationGetByInvitaionIdOnPage", rsHIS},
						{"TotalCount", rsHIS != null && rsHIS.Count > 0 ? rsHIS[0].RowNumber : 0},
						{"PageIndex", rsHIS != null && rsHIS.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHIS != null && rsHIS.Count > 0 ? PageSize : 0},
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



		public bool deletefile(string fname)
		{
			try
			{
				string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, fname);
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
