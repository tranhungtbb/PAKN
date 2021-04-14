using Bugsnag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
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
	[Route("api/Captcha")]
	public class CaptchaController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public static List<CaptchaObject> captChaCode = new List<CaptchaObject>();

		public CaptchaController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[Route("GetCaptchaImage")]
		[HttpGet]
		public IActionResult GetCaptchaImage()
		{
			try
			{
				int width = 200;
				int height = 60;
				var captchaCode = new Captcha(_appSetting).GenerateCaptchaCode();
				var result = new Captcha(_appSetting).GenerateCaptchaImage(width, height, captchaCode);
				new Captcha(_appSetting).InsertCaptcha(result.CaptchaCode);
				Stream s = new MemoryStream(result.CaptchaByteData);
				return new FileStreamResult(s, "image/png");
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new JsonResult(ex);
			}

		}

		public async void SyncDataFromKNCTAPI()
		{
			try
			{
				string url = String.Format("https://kiennghicutri.khanhhoa.gov.vn:96/api/RequestOut/ListTraCuu");
				System.Net.WebRequest requestObjPost = System.Net.WebRequest.Create(url);
				requestObjPost.Method = "POST";
				requestObjPost.ContentType = "application/json";
				string PostData = "{\"PageIndex\":\"1\",\"PageSize\":\"20\",\"IsSuperAdmin\":\"false\",\"IsTongHop\":\"false\",\"MaKn\":\"\"}";

				using (var streamWriter = new StreamWriter(requestObjPost.GetRequestStream()))
				{
					streamWriter.Write(PostData);
					streamWriter.Flush();
					streamWriter.Close();

					var httpResponse = (HttpWebResponse)requestObjPost.GetResponse();
					using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
					{
						var res = streamReader.ReadToEnd();
						var obj = JsonConvert.DeserializeObject<gridRecommendation>(res);
						foreach (var item in obj.gridData)
						{
							MRRecommendationKNCTInsertIN mRRecommendationKNCTInsertIN = new MRRecommendationKNCTInsertIN();
							mRRecommendationKNCTInsertIN.Code = item.maKienNghi;
							mRRecommendationKNCTInsertIN.RecommendationKNCTId = item.id;
							mRRecommendationKNCTInsertIN.CreatedDate = item.ngayTao;
							mRRecommendationKNCTInsertIN.SendDate = item.ngayTiepNhan;
							mRRecommendationKNCTInsertIN.EndDate = item.ngayKetThuc;
							mRRecommendationKNCTInsertIN.District = item.phuongXa;
							mRRecommendationKNCTInsertIN.Content = item.noiDungKienNghi;
							mRRecommendationKNCTInsertIN.Classify = item.phanLoai;
							mRRecommendationKNCTInsertIN.Term = item.nhiemKy;
							CAFieldKNCTInsertIN cAFieldKNCTInsertIN = new CAFieldKNCTInsertIN();
							cAFieldKNCTInsertIN.Name = item.linhVuc;
							mRRecommendationKNCTInsertIN.FieldId = await new CAFieldKNCTInsert(_appSetting).CAFieldKNCTInsertDAO(cAFieldKNCTInsertIN);
							mRRecommendationKNCTInsertIN.Place = item.noiCoKienNghi;
							mRRecommendationKNCTInsertIN.Department = item.coQuanChuTri;
							mRRecommendationKNCTInsertIN.Progress = item.noiDungTraLoi;
							mRRecommendationKNCTInsertIN.Status = item.trangThai;
							MRRecommendationKNCTCheckExistedId rsMRRecommendationCheckExistedId = (await new MRRecommendationKNCTCheckExistedId(_appSetting).MRRecommendationKNCTCheckExistedIdDAO(item.id)).FirstOrDefault();

							if (rsMRRecommendationCheckExistedId.Total > 0)
							{
								MRRecommendationKNCTUpdateIN mRRecommendationKNCTUpdateIN = new MRRecommendationKNCTUpdateIN();
								mRRecommendationKNCTUpdateIN.Code = item.maKienNghi;
								mRRecommendationKNCTUpdateIN.RecommendationKNCTId = item.id;
								mRRecommendationKNCTUpdateIN.CreatedDate = item.ngayTao;
								mRRecommendationKNCTUpdateIN.SendDate = item.ngayTiepNhan;
								mRRecommendationKNCTUpdateIN.EndDate = item.ngayKetThuc;
								mRRecommendationKNCTUpdateIN.District = item.phuongXa;
								mRRecommendationKNCTUpdateIN.Content = item.noiDungKienNghi;
								mRRecommendationKNCTUpdateIN.Classify = item.phanLoai;
								mRRecommendationKNCTUpdateIN.Term = item.nhiemKy;
								CAFieldKNCTInsertIN cAFieldKNCTUpdateIN = new CAFieldKNCTInsertIN();
								cAFieldKNCTUpdateIN.Name = item.linhVuc;
								mRRecommendationKNCTUpdateIN.FieldId = await new CAFieldKNCTInsert(_appSetting).CAFieldKNCTInsertDAO(cAFieldKNCTUpdateIN);
								mRRecommendationKNCTUpdateIN.Place = item.noiCoKienNghi;
								mRRecommendationKNCTUpdateIN.Department = item.coQuanChuTri;
								mRRecommendationKNCTUpdateIN.Progress = item.noiDungTraLoi;
								mRRecommendationKNCTUpdateIN.Status = item.trangThai;
								await new MRRecommendationKNCTUpdate(_appSetting).MRRecommendationKNCTUpdateDAO(mRRecommendationKNCTUpdateIN);
								MRRecommendationKNCTFilesDeleteIN _mRRecommendationKNCTFilesDeleteIN = new MRRecommendationKNCTFilesDeleteIN();
								_mRRecommendationKNCTFilesDeleteIN.Id = item.id;
								await new MRRecommendationKNCTFilesDelete(_appSetting).MRRecommendationKNCTFilesDeleteDAO(_mRRecommendationKNCTFilesDeleteIN);
								foreach (var itemFile in item.tepDinhKem)
								{
									MRRecommendationKNCTFilesInsertIN mRRecommendationKNCTFilesInsertIN = new MRRecommendationKNCTFilesInsertIN();
									mRRecommendationKNCTFilesInsertIN.Name = itemFile.name;
									mRRecommendationKNCTFilesInsertIN.FileType = 1;
									mRRecommendationKNCTFilesInsertIN.FilePath = itemFile.duongDan;
									mRRecommendationKNCTFilesInsertIN.RecommendationKNCTId = item.id;
									await new MRRecommendationKNCTFilesInsert(_appSetting).MRRecommendationKNCTFilesInsertDAO(mRRecommendationKNCTFilesInsertIN);
								}
							}
							else
							{

								foreach (var itemFile in item.tepDinhKem)
								{
									MRRecommendationKNCTFilesInsertIN mRRecommendationKNCTFilesInsertIN = new MRRecommendationKNCTFilesInsertIN();
									mRRecommendationKNCTFilesInsertIN.Name = itemFile.name;
									mRRecommendationKNCTFilesInsertIN.FileType = 1;
									mRRecommendationKNCTFilesInsertIN.FilePath = itemFile.duongDan;
									mRRecommendationKNCTFilesInsertIN.RecommendationKNCTId = item.id;
									await new MRRecommendationKNCTFilesInsert(_appSetting).MRRecommendationKNCTFilesInsertDAO(mRRecommendationKNCTFilesInsertIN);
								}
								await RecommendationKNCTInsert(mRRecommendationKNCTInsertIN);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
			}
		}

		[Route("RecommendationKNCTInsert")]
		[HttpPost]
		public async Task<ActionResult<object>> RecommendationKNCTInsert(MRRecommendationKNCTInsertIN mRRecommendationKNCTInsertIN)
		{
			try
			{
				var m = await new MRRecommendationKNCTInsert(_appSetting).MRRecommendationKNCTInsertDAO(mRRecommendationKNCTInsertIN);
				return new ResultApi { Success = ResultCode.OK };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[Route("ValidatorCaptcha")]
		[HttpGet]
		public ActionResult<object> ValidatorCaptcha(string CaptchaCode)
		{
			try
			{
				if (!new Captcha(_appSetting).ValidateCaptchaCode(CaptchaCode, captChaCode))
				{
					new Captcha(_appSetting).DeleteCaptcha("");
					return new ResultApi { Success = ResultCode.ORROR };
				}
				else
				{
					new Captcha(_appSetting).DeleteCaptcha(CaptchaCode);
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
