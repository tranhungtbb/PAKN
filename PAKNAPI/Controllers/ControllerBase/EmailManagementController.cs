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
    [Route("api/[controller]")]
    [ApiController]
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

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("Update"), DisableRequestSizeLimit]
		public async Task<ActionResult<object>> Update([FromQuery] string userId)
		{
			try
			{
				var req = Request.Query;
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);
				//var userId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext) +"";
				//var userId = users.FirstOrDefault(c => c.Type.Equals("Id", StringComparison.OrdinalIgnoreCase)).Value;
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
				var listIndividualDel = JsonConvert.DeserializeObject<IEnumerable<EmailManagementIndividualModel>>(form["ListIndividualDel"].ToString(), jss);
				var listIndividualNew = JsonConvert.DeserializeObject<IEnumerable<EmailManagementIndividualModel>>(form["ListIndividualNew"].ToString(), jss);
				var listBusinessDel = JsonConvert.DeserializeObject<IEnumerable<EmailManagementBusinessModel>>(form["ListBusinessDel"].ToString(), jss);
				var listBusinessNew = JsonConvert.DeserializeObject<IEnumerable<EmailManagementBusinessModel>>(form["ListBusinessNew"].ToString(), jss);
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
				///individual
				///
				if (listIndividualDel != null && listIndividualDel.Any())
				{
					var ids = string.Join(",", listIndividualDel.Select(c => c.Id));
					var delFileRs = await new EmailManagementIndividualADO(_appSetting).Delete(ids);
				}
				if (listIndividualNew != null && listIndividualNew.Any())
				{
                    foreach (var item in listIndividualNew)
                    {
						item.EmailId = model.Id;
						await new EmailManagementIndividualADO(_appSetting).Update(item);
                    }
				}

				///business
				///
				if (listBusinessDel != null && listBusinessDel.Any())
				{
					var ids = string.Join(",", listBusinessDel.Select(c => c.Id));
					var delFileRs = await new EmailManagementBusinessADO(_appSetting).Delete(ids);
				}
				if (listBusinessNew != null && listBusinessNew.Any())
				{
					foreach (var item in listBusinessNew)
					{
						item.EmailId = model.Id;
						await new EmailManagementBusinessADO(_appSetting).Update(item);
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



		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("GetById")]
		public async Task<ActionResult<object>> GetById(long id)
        {
            try
            {


				var model = await new EmailMangementADO(_appSetting).GetById(id);
				var listAttachment = await new EmailManagementAttachmentADO(_appSetting).GetByEmailId(id);
				var listIndividual = await new EmailManagementIndividualADO(_appSetting).GetByEmailId(id);
				var listBusiness = await new EmailManagementBusinessADO(_appSetting).GetByEmailId(id);


				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"Data", model.FirstOrDefault()},
					{"ListAttachment", listAttachment},
					{"ListIndividual", listIndividual},
					{"ListBusiness", listBusiness},
				};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch(Exception ex)
            {
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
        }

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("GetPagedList")]
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
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("Delete")]
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SendEmail")]
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


				await sendEmail(id);


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
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("GetHisPagedList")]
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
