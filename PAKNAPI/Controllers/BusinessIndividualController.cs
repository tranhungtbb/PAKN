using Bugsnag;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Services.FileUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using PAKNAPI.App_Helper;
using PAKNAPI.Models.Results;
using PAKNAPI.Models.Login;
using System.Security.Claims;
using System.Globalization;
using PAKNAPI.Models;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.BusinessIndividual;

namespace PAKNAPI.Controllers
{
	[Route("api/BusinessIndividual")]
	[ApiController]
	public class BusinessIndividualController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IClient _bugsnag;
		public BusinessIndividualController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_hostingEnvironment = hostingEnvironment;
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("upload")]
		[Authorize]
		public async Task<ActionResult<object>> Upload(string folder = null)
		{
			try
			{

				var file = Request.Form.Files[0];

				if (file.Length <= 0)
				{
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "File not found!" };
				}

				string contentRootPath = _hostingEnvironment.ContentRootPath;

				string folderName = string.IsNullOrEmpty(folder) ? "Upload/Orther" : "Upload/" + folder;

				string fileName = $"{DateTime.Now.ToString("ddMMyyyyHHmmss")}-{file.FileName}";
				string folderPath = System.IO.Path.Combine(contentRootPath, folderName);

				if (!System.IO.Directory.Exists(folderPath))
				{
					System.IO.Directory.CreateDirectory(folderPath);
				}

				string fileNamePath = System.IO.Path.Combine(folderPath, fileName);

				using (var memoryStream = System.IO.File.Create(fileNamePath))
				{
					await file.CopyToAsync(memoryStream);
				}

				System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileNamePath);

                OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(fileInfo);
                OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

				// get number of rows and columns in the sheet
				int rows = worksheet.Dimension.Rows; // 20
				int columns = worksheet.Dimension.Columns; // 7

				//create a list to hold all the values
				List<Models.BusinessIndividual.BIIndividualInsertIN> individualList = new List<Models.BusinessIndividual.BIIndividualInsertIN>();

				// loop through the worksheet rows and columns
				for (int i = 1; i <= rows; i++)
				{
					if (i == 1)
                    {
						continue;
                    }						
					Models.BusinessIndividual.BIIndividualInsertIN ind = new Models.BusinessIndividual.BIIndividualInsertIN();
					ind.FullName = worksheet.Cells[i, 1].Value.ToString();
					ind.Email = worksheet.Cells[i, 2].Value.ToString();
					ind.Phone = worksheet.Cells[i, 3].Value.ToString();
					ind.IDCard = worksheet.Cells[i, 4].Value.ToString();
					ind.IssuedPlace = worksheet.Cells[i, 5].Value.ToString();
					ind.Nation = worksheet.Cells[i, 6].Value.ToString();
					ind.ProvinceId = Convert.ToInt32(worksheet.Cells[i, 7].Value.ToString());
					ind.DistrictId = Convert.ToInt32(worksheet.Cells[i, 8].Value.ToString());
					ind.WardsId = Convert.ToInt32(worksheet.Cells[i, 9].Value.ToString());
					ind.PermanentPlace = worksheet.Cells[i, 10].Value.ToString();
					ind.Address = worksheet.Cells[i, 11].Value.ToString();
					ind.BirthDay = DateTime.Now;
					ind.Status = Convert.ToInt32(worksheet.Cells[i, 12].Value.ToString());

					individualList.Add(ind);
				}

				foreach(Models.BusinessIndividual.BIIndividualInsertIN ins in individualList)
                {
					await new Models.BusinessIndividual.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(ins);
				}					

				return new Models.Results.ResultApi { Success = ResultCode.OK, Result = fileInfo };
			}
			catch (Exception e)
			{
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = e.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("IndividualGetAllOnPage")]
		public async Task<ActionResult<object>> IndividualGetAllOnPage(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			try
			{
                List<IndividualGetAllOnPage> rsIndividualGetAllOnPageBase = await new IndividualGetAllOnPage(_appSetting).IndividualGetAllOnPageDAO(PageSize, PageIndex, FullName, Address, Phone, Email, IsActived, SortDir, SortField);
                IDictionary<string, object> json = new Dictionary<string, object>
                        {
                            {"IndividualGetAllOnPage", rsIndividualGetAllOnPageBase},
                            {"TotalCount", rsIndividualGetAllOnPageBase != null && rsIndividualGetAllOnPageBase.Count > 0 ? rsIndividualGetAllOnPageBase[0].RowNumber : 0},
                            {"PageIndex", rsIndividualGetAllOnPageBase != null && rsIndividualGetAllOnPageBase.Count > 0 ? PageIndex : 0},
                            {"PageSize", rsIndividualGetAllOnPageBase != null && rsIndividualGetAllOnPageBase.Count > 0 ? PageSize : 0},
                        };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
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
		[Route("IndivialDelete")]
		public async Task<ActionResult<object>> IndivialDelete(IndivialDeleteIN _indivialDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new IndivialDelete(_appSetting).IndivialDeleteDAO(_indivialDeleteIN) };
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
		[Route("IndivialChageStatus")]
		public async Task<ActionResult<object>> IndivialChageStatus(IndivialChageStatusIN _indivialChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new IndivialChageStatus(_appSetting).IndivialChageStatusDAO(_indivialChageStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Route("InvididualRegister")]
		public async Task<object> InvididualRegister(
			[FromForm] Models.BusinessIndividual.BIIndividualInsertIN model,
			[FromForm] string _BirthDay,
			[FromForm] string _DateOfIssue)
		{
			try
			{
                var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
                if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };

				// check exist:Phone,Email,IDCard
				var checkExists = await new Models.BusinessIndividual.BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, 0);
                if (checkExists[0].Exists.Value)
                    return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
                    checkExists = await new Models.BusinessIndividual.BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, 0);
                    if (checkExists[0].Exists.Value)
                        return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
                }
                checkExists = await new Models.BusinessIndividual.BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IDCard, 0);
                if (checkExists[0].Exists.Value)
                    return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };

                var rs2 = await new Models.BusinessIndividual.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);

			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("InvididualUpdate")]
		public async Task<ActionResult<object>> InvididualUpdate(InvididualUpdateIN _invididualUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new InvididualUpdate(_appSetting).InvididualUpdateDAO(_invididualUpdateIN) };
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
		[Route("InvididualGetByID")]
		public async Task<ActionResult<object>> InvididualGetByID(long? Id)
		{
			try
			{
				List<InvididualGetByID> rsInvididualGetByID = await new InvididualGetByID(_appSetting).InvididualGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"InvididualGetByID", rsInvididualGetByID},
					};
				return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("BusinessGetAllOnPageBase")]
		public async Task<ActionResult<object>> BusinessGetAllOnPageBase(int? PageSize, int? PageIndex, string RepresentativeName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			try
			{
				List<BusinessGetAllOnPage> rsBusinessGetAllOnPageBase = await new BusinessGetAllOnPage(_appSetting).BusinessGetAllOnPageDAO(PageSize, PageIndex, RepresentativeName, Address, Phone, Email, IsActived, SortDir, SortField);
				IDictionary<string, object> json = new Dictionary<string, object>
						{
							{"BusinessGetAllOnPageBase", rsBusinessGetAllOnPageBase},
							{"TotalCount", rsBusinessGetAllOnPageBase != null && rsBusinessGetAllOnPageBase.Count > 0 ? rsBusinessGetAllOnPageBase[0].RowNumber : 0},
							{"PageIndex", rsBusinessGetAllOnPageBase != null && rsBusinessGetAllOnPageBase.Count > 0 ? PageIndex : 0},
							{"PageSize", rsBusinessGetAllOnPageBase != null && rsBusinessGetAllOnPageBase.Count > 0 ? PageSize : 0},
						};
				return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("BusinessDelete")]
		public async Task<ActionResult<object>> BusinessDelete(BusinessDeleteIN _businessDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BusinessDelete(_appSetting).BusinessDeleteDAO(_businessDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("BusinessChageStatus")]
		public async Task<ActionResult<object>> BusinessChageStatus(BusinessChageStatusIN _businessChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BusinessChageStatus(_appSetting).BusinessChageStatusDAO(_businessChageStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("BusinessRegister")]
		public async Task<object> BusinessRegister([FromForm] BusinessInsertIN model,
			[FromForm] string _RepresentativeBirthDay,
			[FromForm] string _DateOfIssue)
		{
			try
			{
				var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
				if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại tài khoản đăng nhập đã tồn tại" };

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(_RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
				//if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				//if (!string.IsNullOrEmpty(model.Email))
				//{
				//	checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
				//	if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
				//}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", model.OrgPhone, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
				if (!string.IsNullOrEmpty(model.OrgEmail))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, 0);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", model.DecisionOfEstablishing, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };

				var rs2 = await new BusinessInsert(_appSetting).BusinessInsertDAO(model);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("BusinessGetByID")]
		public async Task<ActionResult<object>> BusinessGetByID(long? Id)
		{
			try
			{
				List<BusinessGetById> rsBusinessGetById = await new BusinessGetById(_appSetting).BusinessGetByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BusinessGetById", rsBusinessGetById},
					};
				return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("BusinessUpdate")]
		public async Task<ActionResult<object>> BusinessUpdate([FromForm] BusinessUpdateInfoIN businessModel)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BusinessUpdateInfo(_appSetting).BusinessUpdateInfoDAO(businessModel) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}

}
