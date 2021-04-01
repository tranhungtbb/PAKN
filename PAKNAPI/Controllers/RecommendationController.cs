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
using PAKNAPI.Models.Recommendation;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PAKNAPI.Controller
{
	[Route("api/Recommendation")]
	[ApiController]
	public class RecommendationController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public RecommendationController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_hostingEnvironment = hostingEnvironment;
		}



		[HttpGet]
		[Authorize]
		[Route("RecommendationGetDataForCreate")]
		public async Task<ActionResult<object>> RecommendationGetDataForCreate()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForCreate() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("RecommendationGetDataForForward")]
		public async Task<ActionResult<object>> RecommendationGetDataForForward()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForForward() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("RecommendationGetDataForProcess")]
		public async Task<ActionResult<object>> RecommendationGetDataForProcess(int? UnitId)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForProcess(UnitId) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("RecommendationGetByID")]
		public async Task<ActionResult<object>> RecommendationGetByID(int? Id)
		{
			try
			{
				RecommendationGetByIDResponse data = new RecommendationGetByIDResponse();
				return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("RecommendationGetByIDView")]
		public async Task<ActionResult<object>> RecommendationGetByIDView(int? Id)
		{
			try
			{
				RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
				return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetByIDView(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Authorize]
		[Route("RecommendationInsert")]
		public async Task<ActionResult<object>> RecommendationInsert()
		{
			try
			{
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				RecommendationInsertRequest request = new RecommendationInsertRequest();
				request.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				request.UserType = new LogHelper(_appSetting).GetTypeFromRequest(HttpContext);
				request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
				request.Data = JsonConvert.DeserializeObject<MRRecommendationInsertIN>(Request.Form["Data"].ToString(), jss);
				request.Data.UnitId  = request.Data.UnitId != null ? request.Data.UnitId : (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault().Id;
				request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
				request.Files = Request.Form.Files;
				request.Data.CreatedBy = request.UserId;
				request.Data.CreatedDate = DateTime.Now;
				MRRecommendationCheckExistedCode rsMRRecommendationCheckExistedCode = (await new MRRecommendationCheckExistedCode(_appSetting).MRRecommendationCheckExistedCodeDAO(request.Data.Code)).FirstOrDefault();
				if(rsMRRecommendationCheckExistedCode.Total > 0)
                {
					request.Data.Code = await new MRRecommendationGenCodeGetCode(_appSetting).MRRecommendationGenCodeGetCodeDAO();
				}
				int? Id = Int32.Parse((await new MRRecommendationInsert(_appSetting).MRRecommendationInsertDAO(request.Data)).ToString());
				if (Id > 0)
				{
					await new MRRecommendationGenCodeUpdateNumber(_appSetting).MRRecommendationGenCodeUpdateNumberDAO();
					MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN = new MRRecommendationForwardInsertIN();

					_mRRecommendationForwardInsertIN.RecommendationId = Id;
					_mRRecommendationForwardInsertIN.UserSendId = request.UserId;
					_mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
					_mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
					if (request.UserType != 1)
					{
						_mRRecommendationForwardInsertIN.UnitReceiveId = request.Data.UnitId;
						_mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
						_mRRecommendationForwardInsertIN.IsViewed = false;
                    }
                    else
					{
						_mRRecommendationForwardInsertIN.UnitReceiveId = request.Data.UnitId != null ? request.Data.UnitId : (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault().Id;
						_mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.APPROVED;
						_mRRecommendationForwardInsertIN.ReceiveId = request.UserId;
						_mRRecommendationForwardInsertIN.ProcessingDate = DateTime.Now;
						_mRRecommendationForwardInsertIN.IsViewed = true;
					}
					await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);
					if (request.Files != null && request.Files.Count > 0)
					{
						string folder = "Upload\\Recommendation\\" + Id;
						string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
						if (!Directory.Exists(folderPath))
						{
							Directory.CreateDirectory(folderPath);
						}
						foreach (var item in request.Files)
						{
							MRRecommendationFilesInsertIN file = new MRRecommendationFilesInsertIN();
							file.RecommendationId = Id;
							file.Name = Path.GetFileName(item.FileName).Replace("+", "");
							string filePath = Path.Combine(folderPath, file.Name);
							file.FilePath = Path.Combine(folder, file.Name);
							file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
							using (var stream = new FileStream(filePath, FileMode.Create))
							{
								item.CopyTo(stream);
							}
							await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(file);
						}
					}
					MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
					foreach (var item in request.ListHashTag)
                    {
						_mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
						_mRRecommendationHashtagInsertIN.RecommendationId = Id;
						_mRRecommendationHashtagInsertIN.HashtagId = item.Value;
						_mRRecommendationHashtagInsertIN.HashtagName = item.Text;
						await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
					}

					HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
					hisData.ObjectId = Id;
					hisData.Type = 1;
					hisData.Content = "";
					hisData.Status = STATUS_RECOMMENDATION.CREATED;
					hisData.CreatedBy = request.UserId;
					hisData.CreatedDate = DateTime.Now;
					await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
					if (request.UserType != 1)
					{
						hisData = new HISRecommendationInsertIN();
						hisData.ObjectId = Id;
						hisData.Type = 1;
						hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(request.Data.UnitId)).FirstOrDefault().Name;
						hisData.Status = STATUS_RECOMMENDATION.RECEIVE_WAIT;
						hisData.CreatedBy = request.UserId;
						hisData.CreatedDate = DateTime.Now;
						await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                    }
                    else
					{
						hisData = new HISRecommendationInsertIN();
						hisData.ObjectId = Id;
						hisData.Type = 1;
						hisData.Content = "";
						hisData.Status = STATUS_RECOMMENDATION.RECEIVE_APPROVED;
						hisData.CreatedBy = request.UserId;
						hisData.CreatedDate = DateTime.Now;
						await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
					}
				}
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Authorize]
		[Route("RecommendationUpdate")]
		public async Task<ActionResult<object>> RecommendationUpdate()
		{
			try
			{
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				RecommendationUpdateRequest request = new RecommendationUpdateRequest();
				request.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
				request.Data = JsonConvert.DeserializeObject<MRRecommendationUpdateIN>(Request.Form["Data"].ToString(), jss);
				request.LstXoaFile = JsonConvert.DeserializeObject<List<MRRecommendationFiles>>(Request.Form["LstXoaFile"].ToString(), jss);
				request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
				request.Files = Request.Form.Files;
				request.Data.UpdatedBy = request.UserId;
				request.Data.UpdatedDate = DateTime.Now;
				await new MRRecommendationUpdate(_appSetting).MRRecommendationUpdateDAO(request.Data);

				if (request.LstXoaFile.Count > 0)
				{

					Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
					string webRootPath = _hostingEnvironment.ContentRootPath;

					if (string.IsNullOrWhiteSpace(webRootPath))
					{
						webRootPath = Path.Combine(Directory.GetCurrentDirectory());
					}

					foreach (var item in request.LstXoaFile)
					{
						string filePath = Path.Combine(webRootPath, decrypt.DecryptData(item.FilePath));

						if (System.IO.File.Exists(filePath))
						{
							System.IO.File.Delete(filePath);
						}
						MRRecommendationFilesDeleteIN fileDel = new MRRecommendationFilesDeleteIN();
						fileDel.Id = item.Id;
						await new MRRecommendationFilesDelete(_appSetting).MRRecommendationFilesDeleteDAO(fileDel);
					}

				}
				if (request.Files != null && request.Files.Count > 0)
				{
					string folder = "Upload\\Recommendation\\" + request.Data.Id;
					string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
					foreach (var item in request.Files)
					{
						MRRecommendationFilesInsertIN file = new MRRecommendationFilesInsertIN();
						file.RecommendationId = request.Data.Id;
						file.Name = Path.GetFileName(item.FileName).Replace("+", "");
						string filePath = Path.Combine(folderPath, file.Name);
						file.FilePath = Path.Combine(folder, file.Name);
						file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
						using (var stream = new FileStream(filePath, FileMode.Create))
						{
							item.CopyTo(stream);
						}
						await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(file);
					}
				}
				MRRecommendationHashtagDeleteByRecommendationIdIN hashtagDeleteByRecommendationIdIN = new MRRecommendationHashtagDeleteByRecommendationIdIN();
				hashtagDeleteByRecommendationIdIN.Id = request.Data.Id;
				await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(hashtagDeleteByRecommendationIdIN);
				MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
				foreach (var item in request.ListHashTag)
                {
					_mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
					_mRRecommendationHashtagInsertIN.RecommendationId = request.Data.Id;
					_mRRecommendationHashtagInsertIN.HashtagId = item.Value;
					_mRRecommendationHashtagInsertIN.HashtagName = item.Text;
					await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
				}

				HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
				hisData.ObjectId = request.Data.Id;
				hisData.Type = 1;
				hisData.Content = "";
				hisData.Status = STATUS_RECOMMENDATION.UPDATED;
				hisData.CreatedBy = request.UserId;
				hisData.CreatedDate = DateTime.Now;
				await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Authorize]
		[Route("RecommendationForward")]
		public async Task<ActionResult<object>> RecommendationForward(RecommendationForwardRequest request)
		{
			try
			{
				request._mRRecommendationForwardInsertIN.UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				request._mRRecommendationForwardInsertIN.UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				request._mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
				await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(request._mRRecommendationForwardInsertIN);

				MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
				_mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
				_mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardInsertIN.RecommendationId;
				await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
				HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
				hisData.ObjectId = request._mRRecommendationForwardInsertIN.RecommendationId;
				hisData.Type = 1;
				hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(request._mRRecommendationForwardInsertIN.UnitReceiveId)).FirstOrDefault().Name; ;
				hisData.Status = request.RecommendationStatus;
				hisData.CreatedBy = request._mRRecommendationForwardInsertIN.UserSendId;
				hisData.CreatedDate = DateTime.Now;
				await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Authorize]
		[Route("RecommendationOnProcess")]
		public async Task<ActionResult<object>> MRRecommendationOnProcess(RecommendationForwardProcess request)
		{
			try
			{
				long UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				int UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
				request._mRRecommendationForwardProcessIN.ProcessingDate = DateTime.Now;
				request._mRRecommendationForwardProcessIN.UserId = UserSendId;
				await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(request._mRRecommendationForwardProcessIN);
				if (request.RecommendationStatus == STATUS_RECOMMENDATION.PROCESS_DENY)
                {
					MRRecommendationUpdateReactionaryWordIN _mRRecommendationUpdateReactionaryWordIN = new MRRecommendationUpdateReactionaryWordIN();
					_mRRecommendationUpdateReactionaryWordIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
					_mRRecommendationUpdateReactionaryWordIN.ReactionaryWord = request.ReactionaryWord;
					await new MRRecommendationUpdateReactionaryWord(_appSetting).MRRecommendationUpdateReactionaryWordDAO(_mRRecommendationUpdateReactionaryWordIN);
				}

				MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
				_mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
				_mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
				await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);

				HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
				hisData.ObjectId = request._mRRecommendationForwardProcessIN.RecommendationId;
				hisData.Type = 1;
				hisData.Content = "";
				if(request.RecommendationStatus == STATUS_RECOMMENDATION.APPROVE_DENY || request.RecommendationStatus == STATUS_RECOMMENDATION.PROCESS_DENY || request.RecommendationStatus == STATUS_RECOMMENDATION.RECEIVE_DENY)
				{
					hisData.Content = "Với lý do: " + request._mRRecommendationForwardProcessIN.ReasonDeny;
				}
				hisData.Status = request.RecommendationStatus;
				hisData.CreatedBy = UserSendId;
				hisData.CreatedDate = DateTime.Now;
				await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
