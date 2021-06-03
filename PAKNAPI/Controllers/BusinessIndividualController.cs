﻿using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.BusinessIndividual;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
		[Route("ImportDataInvididual")]
		[Authorize]
		public async Task<ActionResult<object>> ImportDataInvididual(string folder = null)
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
					ind.IsActived = true;
					ind.IsDeleted = false;
					ind.UserId = Convert.ToInt64(worksheet.Cells[i, 13].Value.ToString());

					individualList.Add(ind);
				}

				foreach (Models.BusinessIndividual.BIIndividualInsertIN ins in individualList)
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

		[HttpPost, DisableRequestSizeLimit]
		[Route("ImportDataBusiness")]
		[Authorize]
		public async Task<ActionResult<object>> ImportDataBusiness(string folder = null)
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
					ind.IsActived = true;
					ind.IsDeleted = false;
					ind.UserId = Convert.ToInt64(worksheet.Cells[i, 13].Value.ToString());

					individualList.Add(ind);
				}

				foreach (Models.BusinessIndividual.BIIndividualInsertIN ins in individualList)
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
		public async Task<ActionResult<object>> IndividualGetAllOnPage(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, int? Status, string SortDir, string SortField)
		{
			try
			{
                List<BI_IndividualGetAllOnPage> rsIndividualGetAllOnPageBase = await new BI_IndividualGetAllOnPage(_appSetting).BI_IndividualGetAllOnPageDAO(PageSize, PageIndex, FullName, Address, Phone, Email, Status, SortDir, SortField);
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
		public async Task<ActionResult<object>> IndivialDelete(BI_IndivialDeleteIN _bi_IndivialDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_IndivialDelete(_appSetting).BI_IndivialDeleteDAO(_bi_IndivialDeleteIN) };
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
		[Route("IndivialChangeStatus")]
		public async Task<ActionResult<object>> IndivialChangeStatus(BI_IndivialChageStatusIN _bI_IndivialChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_IndivialChageStatus(_appSetting).IndivialChageStatusDAO(_bI_IndivialChageStatusIN) };
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
		[Route("InvididualRegister")]
		public async Task<object> InvididualRegister(
			[FromBody] Models.BusinessIndividual.BIIndividualInsertIN_Cus model)
		{
			try
			{

                var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
                if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };

				// check exist:Phone,Email,IDCard
				var checkExists = await new Models.BusinessIndividual.BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, 0);
                if (checkExists[0].Exists.Value)
                    return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
                    checkExists = await new Models.BusinessIndividual.BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, 0);
                    if (checkExists[0].Exists.Value)
                        return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
                }
                checkExists = await new Models.BusinessIndividual.BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IDCard, 0);
                if (checkExists[0].Exists.Value)
                    return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };

				DateTime birthDay, dateOfIssue;
				if (!DateTime.TryParseExact(model._BirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birthDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(model._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				if(model.ProvinceId == 0)
                {
					model.ProvinceId = null;
					model.DistrictId = null;
					model.WardsId = null;
                }

				//add login info
				string defaultPwd = "abc123";
				var pwd = GeneratePwdModelBase.generatePassword(defaultPwd);
				var account = new SYUserInsertIN
				{
					Password = pwd.Password,
					Salt = pwd.Salt,
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.Phone,
					FullName = model.FullName,
					Gender = model.Gender,
					Address = model.Address,//
					TypeId = 2,
					Type = 2,
					IsActived = true,
					IsDeleted = false,
					CountLock = 0,
					LockEndOut = DateTime.Now,
					IsSuperAdmin = false,

				};
				var rs1 = await new SYUserInsert(_appSetting).SYUserInsertDAO(account);
				var accRs = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(account.UserName);

				if (string.IsNullOrEmpty(model._DateOfIssue)) model.DateOfIssue = null;
				else model.DateOfIssue = dateOfIssue;
				if (string.IsNullOrEmpty(model._BirthDay)) model.BirthDay = null;
				else model.BirthDay = birthDay;
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;
				await new Models.BusinessIndividual.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		[HttpPost]
		[Authorize]
		[Route("InvididualUpdate")]
		public async Task<ActionResult<object>> InvididualUpdate(
			[FromBody] Models.BusinessIndividual.BI_InvididualUpdateIN_body _bI_InvididualUpdateIN
			//[FromForm] string _BirthDay,
			//[FromForm] string _DateOfIssue
			)
		{
			try
			{

				// check exist:Phone,Email,IDCard
				var checkExists = await new Models.BusinessIndividual.BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", _bI_InvididualUpdateIN.Phone, _bI_InvididualUpdateIN.Id);
				if (checkExists[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(_bI_InvididualUpdateIN.Email))
				{
					checkExists = await new Models.BusinessIndividual.BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", _bI_InvididualUpdateIN.Email, _bI_InvididualUpdateIN.Id);
					if (checkExists[0].Exists.Value)
						return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
				}
				checkExists = await new Models.BusinessIndividual.BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", _bI_InvididualUpdateIN.IDCard, _bI_InvididualUpdateIN.Id);
				if (checkExists[0].Exists.Value)
					return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };

				DateTime? birthDay =null, dateOfIssue=null;
				if (DateTime.TryParseExact(_bI_InvididualUpdateIN._BirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _birthDay))
				{
					birthDay = _birthDay;
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(_bI_InvididualUpdateIN._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _dateOfIssue))
				{
					dateOfIssue = _dateOfIssue;
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}
				//if (string.IsNullOrEmpty(_DateOfIssue)) _bI_InvididualUpdateIN.DateOfIssue = null;
				//else _bI_InvididualUpdateIN.DateOfIssue = dateOfIssue;
				//if (string.IsNullOrEmpty(_BirthDay)) _bI_InvididualUpdateIN.BirthDate = null;
				//else _bI_InvididualUpdateIN.BirthDate = birthDay;
				if (_bI_InvididualUpdateIN.ProvinceId == 0)
				{
					_bI_InvididualUpdateIN.ProvinceId = null;
					_bI_InvididualUpdateIN.DistrictId = null;
					_bI_InvididualUpdateIN.WardsId = null;
				}

                //if (string.IsNullOrEmpty(_DateOfIssue)) _bI_InvididualUpdateIN.DateOfIssue = null;
                //else _bI_InvididualUpdateIN.DateOfIssue = dateOfIssue;
                //if (string.IsNullOrEmpty(_BirthDay)) _bI_InvididualUpdateIN.BirthDate = null;
                //else _bI_InvididualUpdateIN.BirthDate = birthDay;
				_bI_InvididualUpdateIN.DateOfIssue = dateOfIssue;
				_bI_InvididualUpdateIN.BirthDate = birthDay;
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = await new BI_InvididualUpdate(_appSetting).BI_InvididualUpdateDAO(_bI_InvididualUpdateIN) };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
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
				List<BI_InvididualGetByID> rsInvididualGetByID = await new BI_InvididualGetByID(_appSetting).BI_InvididualGetByIDDAO(Id);
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
		[Route("BIInvididualCheckExistsBase")]
		public async Task<ActionResult<object>> BIInvididualCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<BIInvididualCheckExists> rsInvididualCheckExists = await new BIInvididualCheckExists(_appSetting).BIInvididualCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIInvididualCheckExists", rsInvididualCheckExists},
					};
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
		[Route("BusinessGetAllOnPage")]
		public async Task<ActionResult<object>> BusinessGetAllOnPage(int? PageSize, int? PageIndex, string RepresentativeName, string Address, string Phone, string Email, byte? Status, string SortDir, string SortField)
		{
			try
			{
				List<BI_BusinessGetAllOnPage> rsBusinessGetAllOnPageBase = await new BI_BusinessGetAllOnPage(_appSetting).BI_BusinessGetAllOnPageDAO(PageSize, PageIndex, RepresentativeName, Address, Phone, Email, Status, SortDir, SortField);
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
		public async Task<ActionResult<object>> BusinessDelete(BI_BusinessDeleteIN _bI_BusinessDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_BusinessDelete(_appSetting).BusinessDeleteDAO(_bI_BusinessDeleteIN) };
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
		[Route("BusinessChangeStatus")]
		public async Task<ActionResult<object>> BusinessChageStatus(BI_BusinessChageStatusIN _bI_BusinessChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_BusinessChageStatus(_appSetting).BI_BusinessChageStatusDAO(_bI_BusinessChageStatusIN) };
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
		public async Task<object> BusinessRegister([FromBody] BI_BusinessInsertIN_Cus model)
		{
			try
			{
				var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
				if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại tài khoản đăng nhập đã tồn tại" };

				DateTime birdDay, dateOfIssue;
				if (!DateTime.TryParseExact(model._RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out birdDay))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày sinh không hợp lệ" };
				}
				if (!DateTime.TryParseExact(model._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out dateOfIssue))
				{
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}

				///Phone,Email,IDCard
				///check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
                if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
                    checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
                    if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
                }
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
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", model.Tax, 0);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mã số thuế đã tồn tại" };

                if (model.DistrictId == 0)
                {
					model.DistrictId = null;
					model.ProvinceId = null;
					model.WardsId = null;
					model.OrgDistrictId = null;
					model.OrgProvinceId = null;
					model.OrgWardsId = null;
                }

				//add login info
				string defaultPwd = "abc123";
				var pwd = GeneratePwdModelBase.generatePassword(defaultPwd);
				var account = new SYUserInsertIN
				{
					Password = pwd.Password,
					Salt = pwd.Salt,
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.Phone,
					FullName = model.RepresentativeName,
					Gender = model.RepresentativeGender,
					Address = model.Address,//
					TypeId = 3,
					Type = 3,
					IsActived = true,
					IsDeleted = false,
					CountLock = 0,
					LockEndOut = DateTime.Now,
					IsSuperAdmin = false,

				};
				var rs1 = await new SYUserInsert(_appSetting).SYUserInsertDAO(account);
				var accRs = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(account.UserName);

				if (string.IsNullOrEmpty(model._DateOfIssue)) model.DateOfIssue = null;
				else model.DateOfIssue = dateOfIssue;
				if (string.IsNullOrEmpty(model._RepresentativeBirthDay)) model.RepresentativeBirthDay = null;
				else model.RepresentativeBirthDay = birdDay;
				model.CreatedDate = DateTime.Now;
				model.CreatedBy = 0;
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;
				var rs2 = await new BI_BusinessInsert(_appSetting).BusinessInsertDAO(model);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
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
				List<BI_BusinessGetById> rsBusinessGetById = await new BI_BusinessGetById(_appSetting).BusinessGetByIdDAO(Id);
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
		public async Task<ActionResult<object>> BusinessUpdate([FromBody] BI_BusinessUpdateInfoIN_body model)
		{
			try
			{
				///check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(model.Email))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, model.Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", model.OrgPhone, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
				if (!string.IsNullOrEmpty(model.OrgEmail))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, model.Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", model.DecisionOfEstablishing, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", model.Tax, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mã số thuế đã tồn tại" };

				//DateTime? birdDay=null, dateOfIssue=null;
				if (DateTime.TryParseExact(model._RepresentativeBirthDay, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _birdDay))
				{
					model.RepresentativeBirthDay = _birdDay;
				}
				if (DateTime.TryParseExact(model._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _dateOfIssue))
				{
					model.DateOfIssue = _dateOfIssue;
				}


				if (model.DistrictId == 0)
				{
					model.DistrictId = null;
					model.ProvinceId = null;
					model.WardsId = null;
					model.OrgDistrictId = null;
					model.OrgProvinceId = null;
					model.OrgWardsId = null;
				}
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = await new BI_BusinessUpdateInfo(_appSetting).BI_BusinessUpdateInfoDAO(model) };
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
