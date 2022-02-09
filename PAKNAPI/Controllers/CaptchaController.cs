﻿using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NSwag.Annotations;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
	[Route("api/captcha")]
	[OpenApiTag("Captcha", Description = "Captcha")]
	public class CaptchaController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

		public static List<CaptchaObject> captChaCode = new List<CaptchaObject>();

		public CaptchaController(IAppSetting appSetting, IClient bugsnag, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_configuration = configuration;
		}

		/// <summary>
		/// mã capcha (ảnh)
		/// </summary>
		/// <param name="IpAddress"></param>
		/// <param name="MillisecondsCurrent"></param>
		/// <returns></returns>
		[Route("get-captcha-image")]
		[HttpGet]
		public async Task<IActionResult> GetCaptchaImageAsync(string IpAddress = null, double MillisecondsCurrent = 0)
		{
			try
			{
				int width = 120;
				int height = 26;
				int number1 = 0;
				int number2 = 0;
				Random random = new Random();
				number1 = random.Next(10);
				number2 = random.Next(10 - number1);
				var captchaCode = $"{" " + number1.ToString()}+{number2.ToString()}=";
				//var captchaCode = new Captcha(_appSetting).GenerateCaptchaCode();

				var result = new Captcha(_appSetting).GenerateCaptchaImage(width, height, captchaCode);
				//await new Captcha(_appSetting).DeleteCaptchaByUserAgent(IpAddress, Request.Headers["User-Agent"].ToString());
				TimeSpan time = TimeSpan.FromMilliseconds(MillisecondsCurrent);
				DateTime createdDAte = new DateTime(1970, 1, 1) + time;
				await new Captcha(_appSetting).InsertCaptcha((number1 + number2).ToString(), IpAddress, Request.Headers["User-Agent"].ToString(), createdDAte);
				Stream s = new MemoryStream(result.CaptchaByteData);
				var r = new FileStreamResult(s, "image/png");
				return r;
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new JsonResult(ex);
			}

		}

		//public async void SyncDataFromKNCTAPI()
		//{
		//	try
		//	{
		//		string url = String.Format("https://kiennghicutri.khanhhoa.gov.vn:96/api/RequestOut/ListTraCuu");
		//		System.Net.WebRequest requestObjPost = System.Net.WebRequest.Create(url);
		//		requestObjPost.Method = "POST";
		//		requestObjPost.ContentType = "application/json";
		//		string PostData = "{\"PageIndex\":\"1\",\"PageSize\":\"20\",\"IsSuperAdmin\":\"false\",\"IsTongHop\":\"false\",\"MaKn\":\"\"}";

		//		using (var streamWriter = new StreamWriter(requestObjPost.GetRequestStream()))
		//		{
		//			streamWriter.Write(PostData);
		//			streamWriter.Flush();
		//			streamWriter.Close();

		//			var httpResponse = (HttpWebResponse)requestObjPost.GetResponse();
		//			using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
		//			{
		//				var res = streamReader.ReadToEnd();
		//				var obj = JsonConvert.DeserializeObject<gridRecommendation>(res);
		//				foreach (var item in obj.gridData)
		//				{
		//					MRRecommendationKNCTInsertIN mRRecommendationKNCTInsertIN = new MRRecommendationKNCTInsertIN();
		//					mRRecommendationKNCTInsertIN.Code = item.maKienNghi;
		//					mRRecommendationKNCTInsertIN.RecommendationKNCTId = item.id;
		//					mRRecommendationKNCTInsertIN.CreatedDate = item.ngayTao;
		//					mRRecommendationKNCTInsertIN.SendDate = item.ngayTiepNhan;
		//					mRRecommendationKNCTInsertIN.EndDate = item.ngayKetThuc;
		//					mRRecommendationKNCTInsertIN.District = item.phuongXa;
		//					mRRecommendationKNCTInsertIN.Content = item.noiDungKienNghi;
		//					mRRecommendationKNCTInsertIN.Classify = item.phanLoai;
		//					mRRecommendationKNCTInsertIN.Term = item.nhiemKy;
		//					CAFieldKNCTInsertIN cAFieldKNCTInsertIN = new CAFieldKNCTInsertIN();
		//					cAFieldKNCTInsertIN.Name = item.linhVuc;
		//					mRRecommendationKNCTInsertIN.FieldId = await new CAFieldKNCTModel(_appSetting).CAFieldKNCTInsertDAO(cAFieldKNCTInsertIN);
		//					mRRecommendationKNCTInsertIN.Place = item.noiCoKienNghi;
		//					mRRecommendationKNCTInsertIN.Department = item.coQuanChuTri;
		//					mRRecommendationKNCTInsertIN.Progress = item.noiDungTraLoi;
		//					mRRecommendationKNCTInsertIN.Status = item.trangThai;
		//					MRRecommendationKNCTCheckExistedId rsMRRecommendationCheckExistedId = (await new MRRecommendationKNCTCheckExistedId(_appSetting).MRRecommendationKNCTCheckExistedIdDAO(item.id)).FirstOrDefault();

		//					if (rsMRRecommendationCheckExistedId.Total > 0)
		//					{
		//						MRRecommendationKNCTUpdateIN mRRecommendationKNCTUpdateIN = new MRRecommendationKNCTUpdateIN();
		//						mRRecommendationKNCTUpdateIN.Code = item.maKienNghi;
		//						mRRecommendationKNCTUpdateIN.RecommendationKNCTId = item.id;
		//						mRRecommendationKNCTUpdateIN.CreatedDate = item.ngayTao;
		//						mRRecommendationKNCTUpdateIN.SendDate = item.ngayTiepNhan;
		//						mRRecommendationKNCTUpdateIN.EndDate = item.ngayKetThuc;
		//						mRRecommendationKNCTUpdateIN.District = item.phuongXa;
		//						mRRecommendationKNCTUpdateIN.Content = item.noiDungKienNghi;
		//						mRRecommendationKNCTUpdateIN.Classify = item.phanLoai;
		//						mRRecommendationKNCTUpdateIN.Term = item.nhiemKy;
		//						CAFieldKNCTInsertIN cAFieldKNCTUpdateIN = new CAFieldKNCTInsertIN();
		//						cAFieldKNCTUpdateIN.Name = item.linhVuc;
		//						mRRecommendationKNCTUpdateIN.FieldId = await new CAFieldKNCTModel(_appSetting).CAFieldKNCTInsertDAO(cAFieldKNCTInsertIN);
		//						mRRecommendationKNCTUpdateIN.Place = item.noiCoKienNghi;
		//						mRRecommendationKNCTUpdateIN.Department = item.coQuanChuTri;
		//						mRRecommendationKNCTUpdateIN.Progress = item.noiDungTraLoi;
		//						mRRecommendationKNCTUpdateIN.Status = item.trangThai;
		//						await new MRRecommendationKNCTUpdate(_appSetting).MRRecommendationKNCTUpdateDAO(mRRecommendationKNCTUpdateIN);
		//						MRRecommendationKNCTFilesDeleteIN _mRRecommendationKNCTFilesDeleteIN = new MRRecommendationKNCTFilesDeleteIN();
		//						_mRRecommendationKNCTFilesDeleteIN.Id = item.id;
		//						await new MRRecommendationKNCTFilesDelete(_appSetting).MRRecommendationKNCTFilesDeleteDAO(_mRRecommendationKNCTFilesDeleteIN);
		//						foreach (var itemFile in item.tepDinhKem)
		//						{
		//							MRRecommendationKNCTFilesInsertIN mRRecommendationKNCTFilesInsertIN = new MRRecommendationKNCTFilesInsertIN();
		//							mRRecommendationKNCTFilesInsertIN.Name = itemFile.name;
		//							mRRecommendationKNCTFilesInsertIN.FileType = 1;
		//							mRRecommendationKNCTFilesInsertIN.FilePath = itemFile.duongDan;
		//							mRRecommendationKNCTFilesInsertIN.RecommendationKNCTId = item.id;
		//							await new MRRecommendationKNCTFilesInsert(_appSetting).MRRecommendationKNCTFilesInsertDAO(mRRecommendationKNCTFilesInsertIN);
		//						}
		//					}
		//					else
		//					{

		//						foreach (var itemFile in item.tepDinhKem)
		//						{
		//							MRRecommendationKNCTFilesInsertIN mRRecommendationKNCTFilesInsertIN = new MRRecommendationKNCTFilesInsertIN();
		//							mRRecommendationKNCTFilesInsertIN.Name = itemFile.name;
		//							mRRecommendationKNCTFilesInsertIN.FileType = 1;
		//							mRRecommendationKNCTFilesInsertIN.FilePath = itemFile.duongDan;
		//							mRRecommendationKNCTFilesInsertIN.RecommendationKNCTId = item.id;
		//							await new MRRecommendationKNCTFilesInsert(_appSetting).MRRecommendationKNCTFilesInsertDAO(mRRecommendationKNCTFilesInsertIN);
		//						}
		//						await RecommendationKNCTInsert(mRRecommendationKNCTInsertIN);
		//					}
		//				}
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//	}
		//}

		//[Route("RecommendationKNCTInsert")]
		//[HttpPost]
		//public async Task<ActionResult<object>> RecommendationKNCTInsert(MRRecommendationKNCTInsertIN mRRecommendationKNCTInsertIN)
		//{
		//	try
		//	{
		//		var m = await new MRRecommendationKNCTInsert(_appSetting).MRRecommendationKNCTInsertDAO(mRRecommendationKNCTInsertIN);
		//		return new ResultApi { Success = ResultCode.OK };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}

		/// <summary>
		/// check isvalid capcha
		/// </summary>
		/// <param name="CaptchaCode"></param>
		/// <param name="MillisecondsCurrent"></param>
		/// <returns></returns>
		[Route("validator-captcha")]
		[HttpGet]
		public async Task<ActionResult<object>> ValidatorCaptchaAsync(string CaptchaCode, double MillisecondsCurrent)
		{
			try
			{
				TimeSpan time = TimeSpan.FromMilliseconds(MillisecondsCurrent);
				DateTime createdDAte = new DateTime(1970, 1, 1) + time;
				if (!new Captcha(_appSetting).ValidateCaptchaCode(CaptchaCode, captChaCode, createdDAte))
				{
					await new Captcha(_appSetting).DeleteCaptcha("", createdDAte);
					return new ResultApi { Success = ResultCode.ORROR };
				}
				else
				{
					await new Captcha(_appSetting).DeleteCaptcha(CaptchaCode, createdDAte);
					return new ResultApi { Success = ResultCode.OK };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
