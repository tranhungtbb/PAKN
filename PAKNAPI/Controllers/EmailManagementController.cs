using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using PAKNAPI.Services.EmailService;
using PAKNAPI.Services.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/email-management")]
    [ApiController]
	[ValidateModel]
	public class EmailManagementController : BaseApiController
    {
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly IFileService _fileService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private IMailService _mailService;

		private MailSettings mailSetting;

		public EmailManagementController(IAppSetting appSetting, IClient bugsnag, IFileService fileService, IHttpContextAccessor httpContextAccessor,
			IWebHostEnvironment webHostEnvironment)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_fileService = fileService;
			_httpContextAccessor = httpContextAccessor;
			_webHostEnvironment = webHostEnvironment;
			
		}
		/// <summary>
		/// cập nhập Email
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update"), DisableRequestSizeLimit]
		public async Task<ActionResult<object>> Update([FromQuery] string userId)
		{
			try
			{
				var req = Request.Query;
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims;
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};

				var form = Request.Form;

				var data = JsonConvert.DeserializeObject<EmailManagementModelBase>(form["Data"].ToString(), jss);
				var listAttachemntDel = JsonConvert.DeserializeObject<IEnumerable<EmailManagementAttachmentModel>>(form["ListAttachmentDel"].ToString(), jss);
				var listAttachemntNew = JsonConvert.DeserializeObject<IEnumerable<EmailManagementAttachmentModel>>(form["ListAttachmentNew"].ToString(), jss);
				var listBusinessIndividual = JsonConvert.DeserializeObject<IEnumerable<EmailIndividualBusinessModel>>(form["ListBusinessIndividual"].ToString(), jss);
				var files = form.Files;
				///
				EmailManagementHisModel hisModel = new EmailManagementHisModel { CreatedBy = int.Parse(userId) };
				EmailManagementModelBase model = null;
				data.UserUpdateId = int.Parse(userId);
				if (data.Id.HasValue && data.Id > 0)
                {
					model = await new EmailMangementADO(_appSetting).Update(data);
					hisModel.Status = STATUS_HIS_SMS.UPDATE;
				}
				else
                {
					data.UserCreatedId = int.Parse(userId);
					model = await new EmailMangementADO(_appSetting).Insert(data);
					hisModel.Status = STATUS_HIS_SMS.CREATE;
				}					

				///attachments
				///
				if (listAttachemntDel != null && listAttachemntDel.Any())
				{
					var ids = string.Join(",", listAttachemntDel.Select(c => c.Id));
					var delFileRs = await new EmailManagementAttachmentADO(_appSetting).Delete(ids);
					await _fileService.Remove(listAttachemntDel.Select(c=>c.FileAttach).ToArray());
				}
				if (files != null && files.Any())
                {
					var listSaved = await _fileService.Save(files,$"EmailManagement/{model.Id}");
					if(listSaved != null && listSaved.Any())
						foreach(var item in listSaved)
                        {
							var info = listAttachemntNew.FirstOrDefault(c=>c.Name == item.Name);
							var fileInfo = new EmailManagementAttachmentModel
							{
								EmailId = model.Id,
								FileAttach = item.Path,
								Name = item.Name,
								FileType = info?.FileType??0,
							};
							var fileRs = await new EmailManagementAttachmentADO(_appSetting).Insert(fileInfo);
                        }
                }
				// delete all invididual , business 
				await new EmailManagementIndividualADO(_appSetting).DeleteByEmailId(model.Id);
				await new EmailManagementBusinessADO(_appSetting).DeleteByEmailId(model.Id);

				// insert 
				if (listBusinessIndividual.Count() > 0) {
					foreach (var item in listBusinessIndividual) {
						if (item.Category == 2)
						{
							// doanh nghiệp
							var businessInsert = new EmailManagementBusinessModel();
							businessInsert.EmailId = model.Id;
							businessInsert.AdUnitId = item.AdmintrativeUnitId;
							businessInsert.UnitName = item.UnitName;
							businessInsert.BusinessId = item.ObjectId;

							await new EmailManagementBusinessADO(_appSetting).Insert(businessInsert);
						}
						else if (item.Category == 1)
						{
							// cá nhân
							var invididualInsert = new EmailManagementIndividualModel();
							invididualInsert.EmailId = model.Id;
							invididualInsert.AdUnitId = item.AdmintrativeUnitId;
							invididualInsert.UnitName = item.UnitName;
							invididualInsert.IndividualId = item.ObjectId;

							await new EmailManagementIndividualADO(_appSetting).Insert(invididualInsert);
						}
						else {
							continue;
						}
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", model},
				};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				///insert his
				///
				hisModel.ObjectId = model.Id;
				await insertHis(hisModel,userId);

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
		/// chi tiết Email
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> GetById(long id)
        {
            try
            {


				var model = await new EmailMangementADO(_appSetting).GetById(id);
				var listAttachment = await new EmailManagementAttachmentADO(_appSetting).GetByEmailId(id);
				var listIndividual = await new EmailManagementIndividualADO(_appSetting).GetByEmailId(id);
				var listBusiness = await new EmailManagementBusinessADO(_appSetting).GetByEmailId(id);
				var listBusinessIndividual = new List<EmailIndividualBusinessModel>();

				listIndividual.ForEach(item => {
					var itemAdd = new EmailIndividualBusinessModel();
					itemAdd.Id = item.Id;
					itemAdd.EmailId = item.EmailId;
					itemAdd.ObjectId = item.IndividualId;
					itemAdd.ObjectName = item.IndividualFullName;
					itemAdd.UnitName = item.UnitName;
					itemAdd.Category = 1;
					itemAdd.AdmintrativeUnitId = Convert.ToInt32(item.IndividualId);
					listBusinessIndividual.Add(itemAdd);
				});
				listBusiness.ForEach(item => {
					var itemAdd = new EmailIndividualBusinessModel();
					itemAdd.Id = item.Id;
					itemAdd.EmailId = item.EmailId;
					itemAdd.ObjectId = item.BusinessId;
					itemAdd.ObjectName = item.BusinessName;
					itemAdd.UnitName = item.UnitName;
					itemAdd.Category = 2;
					itemAdd.AdmintrativeUnitId = Convert.ToInt32(item.BusinessId);
					listBusinessIndividual.Add(itemAdd);
				});

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", model.FirstOrDefault()},
					{"ListAttachment", listAttachment},
					{"ListBusinessIndividual", listBusinessIndividual},
				};
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch(Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
        }
		/// <summary>
		///  danh sách email
		/// </summary>
		/// <param name="title"></param>
		/// <param name="unit"></param>
		/// <param name="objectId"></param>
		/// <param name="status"></param>
		/// <param name="unitName"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-email-on-page")]
		public async Task<ActionResult<object>> GetPagedList(
			string title,
			int? unit,
			int? objectId,
			short? status,
			string unitName,
			int pageIndex = 1,
			int pageSize = 20)
		{
			try
			{
				

				var listPaged = await new EmailMangementADO(_appSetting).GetPagedList(title,unit,objectId,status, unitName, pageIndex,pageSize);

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", listPaged}
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

		/// <summary>
		/// xóa Email
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> Delete(long id)
		{
			try
			{

				var listAttachment = await new EmailManagementAttachmentADO(_appSetting).GetByEmailId(id);
				if (listAttachment != null && listAttachment.Any())
                {
					foreach (var file in listAttachment)
					{
						await _fileService.Remove(listAttachment.Select(c => c.FileAttach).ToArray());
					}
				}

				var listPaged = await new EmailMangementADO(_appSetting).Delete(id);

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", null}
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

		/// <summary>
		/// gửi Email
		/// </summary>
		/// <param name="id"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("send-email")]
		public async Task<ActionResult<object>> SendEmail(long id, [FromQuery] string userId)
		{
			try
			{
				//var userId = int.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

				var rs = await new EmailMangementADO(_appSetting).UpdateSendStatus(id, int.Parse(userId));
				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", null}
				};


				//await sendEmail(id);


				///insert his
				///
				var hisModel = new EmailManagementHisModel
				{
					CreatedBy = int.Parse(userId),
					Status = STATUS_HIS_SMS.SEND,
					ObjectId = id
				};
				await insertHis(hisModel,userId);
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
		/// <summary>
		/// danh sách lịch sử Email
		/// </summary>
		/// <param name="objectId"></param>
		/// <param name="content"></param>
		/// <param name="createdBy"></param>
		/// <param name="createdDate"></param>
		/// <param name="status"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("list-his")]
		public async Task<ActionResult<object>> GetHisPagedList(
			int objectId,
			string content,
			string createdBy,
			string createdDate,
			int? status,
			int pageIndex = 1,
			int pageSize = 20)
		{
			try
			{
				var rs = await new EmailManagemnetHisADO(_appSetting).GetPagedList(objectId, content,createdBy,createdDate,status,pageIndex,pageSize);
				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", rs}
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


		private async Task<int> insertHis(EmailManagementHisModel model,string userId)
        {
			var currentUser = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(long.Parse(userId));

			model.CreatedBy = currentUser[0].Id;
			string userFullName = currentUser[0].FullName;

			switch (model.Status)
			{
				case STATUS_HIS_SMS.CREATE:
					model.Content = userFullName + " đã khởi tạo Email";
					break;
				case STATUS_HIS_SMS.UPDATE:
					model.Content = userFullName + " đã cập nhập Email";
					break;
				case STATUS_HIS_SMS.SEND:
					model.Content = userFullName + " đã gửi Email";
					break;
			}
			return await new EmailManagemnetHisADO(_appSetting).Insert(model);
		}
		private async Task sendEmail(long id)
        {
			await LoadEmailConfigAsync();
			_mailService = new MailService(mailSetting, _webHostEnvironment);

			var model = await new EmailMangementADO(_appSetting).GetById(id);
			var listAttachment = await new EmailManagementAttachmentADO(_appSetting).GetByEmailId(id);
			var listIndividualEmail = await new EmailManagementIndividualADO(_appSetting).GetAllEmailAddressByEmailId(id);
			var listBusinessEmail = await new EmailManagementBusinessADO(_appSetting).GetAllEmailAddressByEmailId(id);

			string rootHostPath = _webHostEnvironment.ContentRootPath;

			var content = model.FirstOrDefault();
			var toEmails = new List<string>();
			toEmails.AddRange(listIndividualEmail);
			toEmails.AddRange(listBusinessEmail);

			var attachments = listAttachment.Select(c =>
			{
				return $"{rootHostPath}/{c.FileAttach}";
			});

			var req = new MailRequest
			{
				Attachments = attachments,
				Body = content.Content,
				Subject = content.Title,
				ToEmails = toEmails
			};

			await _mailService.SendEmailAsync(req);
		}

		private async Task LoadEmailConfigAsync()
        {
			var rs = await new SYConfig(_appSetting).SYConfigGetByTypeDAO(1);
			mailSetting = JsonConvert.DeserializeObject<MailSettings>(rs.FirstOrDefault().Content);
        }
	}
}
