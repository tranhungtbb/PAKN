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
				request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
				request.Data = JsonConvert.DeserializeObject<MRRecommendationInsertIN>(Request.Form["Data"].ToString(), jss);
				request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
				request.Files = Request.Form.Files;
				request.Data.CreatedBy = request.UserId;
				request.Data.CreatedDate = DateTime.Now;
				int? Id = Int32.Parse((await new MRRecommendationInsert(_appSetting).MRRecommendationInsertDAO(request.Data)).ToString());
				if (Id > 0)
				{
					await new MRRecommendationGenCodeUpdateNumber(_appSetting).MRRecommendationGenCodeUpdateNumberDAO();
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
					hisData.Content = "Người dùng " + request.UserFullName + " đã khởi tạo phản ánh, kiến nghị";
					hisData.Status = STATUS_RECOMMENDATION.CREATED;
					hisData.CreatedBy = request.UserId;
					hisData.CreatedDate = DateTime.Now;
					await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
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
				hisData.Content = "Người dùng " + request.UserFullName + " đã cập nhật phản ánh, kiến nghị";
				hisData.Status = STATUS_RECOMMENDATION.CREATED;
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
	}
}
