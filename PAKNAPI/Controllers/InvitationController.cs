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

namespace PAKNAPI.Controllers
{
    [Route("api/INVInvitation")]
    [ApiController]
   
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

		[HttpPost]
		[Authorize]
		[Route("INVInvitationDelete")]
		public async Task<ActionResult<object>> INVInvitationDeleteBase(INVInvitationDeleteIN _iNVInvitationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
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


		[HttpPost]
		[Authorize]
		[Route("INVInvitationInsert")]
		public async Task<object> INVInvitationInsert()
		{
			try
			{
				INVInvitationInsertModel invInvitation = new INVInvitationInsertModel();
				invInvitation.Model = JsonConvert.DeserializeObject<INVInvitationInsertIN>(Request.Form["Model"].ToString());
				invInvitation.Model.CreateDate = DateTime.Now;
				invInvitation.Model.UserCreateId = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);

				invInvitation.InvitationUserMap = JsonConvert.DeserializeObject<List<INVInvitationUserMapInsertIN>>(Request.Form["InvitationUserMap"].ToString());
				invInvitation.Files = Request.Form.Files;

				invInvitation.Model.IsView = 0;
				invInvitation.Model.Member = invInvitation.InvitationUserMap == null ? 0 : invInvitation.InvitationUserMap.Count();
				
				int id = Int32.Parse((await new INVInvitationInsert(_appSetting).INVInvitationInsertDAO(invInvitation.Model)).ToString());

				if (id > 0)
				{
					// insert file và InvitationMap
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
					foreach(var item in invInvitation.InvitationUserMap) {
						item.InvitationId = id;
						await new INVInvitationUserMapInsert(_appSetting).INVInvitationUserMapInsertDAO(item);
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK};

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "title already exists" };
				}
			}
			catch (Exception ex) {
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize]
		[Route("INVInvitationUpdate")]
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

		[HttpPost]
		[Authorize]
		[Route("INVInvitationUpdate")]
		public async Task<object> INVInvitationUpdate()
		{
			try
			{
				INVInvitationUpdateModel invInvitation = new INVInvitationUpdateModel();
				invInvitation.Model = JsonConvert.DeserializeObject<INVInvitationGetById>(Request.Form["Model"].ToString());
				invInvitation.InvitationUserMap = JsonConvert.DeserializeObject<List<INVInvitationUserMapGetByInvitationId>>(Request.Form["InvitationUserMap"].ToString());
				invInvitation.LtsDeleteFile = JsonConvert.DeserializeObject<List<InvitationFileAttach>>(Request.Form["LstFileDelete"].ToString());
				invInvitation.Files = Request.Form.Files;

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
					//
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
						// delete file 
						//deletefile(folder);
						// delete in db
						//INVFileAttachDeleteByInvitationIdIN deleteFile = new INVFileAttachDeleteByInvitationIdIN();
						//deleteFile.InvitationId = invInvitation.Model.Id;
						//await new INVFileAttachDeleteByInvitationId(_appSetting).INVFileAttachDeleteByInvitationIdDAO(deleteFile);


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

					// insert map user

					foreach (var item in invInvitation.InvitationUserMap)
					{
						INVInvitationUserMapInsertIN ins = new INVInvitationUserMapInsertIN();
						ins.InvitationId = invInvitation.Model.Id;
						ins.SendSMS = item.SendSMS;
						ins.SendEmail = item.SendEmail;
						ins.UserId = item.UserId;
						ins.Watched = item.Watched;
						await new INVInvitationUserMapInsert(_appSetting).INVInvitationUserMapInsertDAO(ins);
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK };

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "title already exists" };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}



		public bool deletefile(string fname)
		{
			try
			{
				string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, fname);
				//string _imageToBeDeleted = Path.Combine(_hostingEnvironment.WebRootPath, "Invitation\\", fname);
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
