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
				request.Files = Request.Form.Files;
				int? Id = Int32.Parse((await new MRRecommendationInsert(_appSetting).MRRecommendationInsertDAO(request.Data)).ToString());
                if (Id > 0)
                {
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
				return new ResultApi { Success = ResultCode.OK};
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
