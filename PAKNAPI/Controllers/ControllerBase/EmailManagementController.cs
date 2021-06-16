using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
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

		public EmailManagementController(IAppSetting appSetting, IClient bugsnag, IFileService fileService, IHttpContextAccessor httpContextAccessor)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_fileService = fileService;
			_httpContextAccessor = httpContextAccessor;
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("Update"), DisableRequestSizeLimit]
		public async Task<ActionResult<object>> Update()
		{
			try
			{
				var users = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims; //FindFirst(ClaimTypes.NameIdentifier);
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
				EmailManagementModelBase model = null;
				//data.UserUpdateId = int.Parse(userId);
				if (data.Id.HasValue && data.Id > 0)
					model = await new EmailMangementADO(_appSetting).Update(data);
				else
                {
					//data.UserCreatedId = int.Parse(userId);
					model = await new EmailMangementADO(_appSetting).Insert(data);
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("GetPagedList")]
		public async Task<ActionResult<object>> GetPagedList(
			string title,
			int? unit,
			int? objectId,
			short? status,
			int pageIndex = 1,
			int pageSize = 20)
		{
			try
			{


				var listPaged = await new EmailMangementADO(_appSetting).GetPagedList(title,unit,objectId,status,pageIndex,pageSize);

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


		private void sendEmail()
        {

        }
	}
}
