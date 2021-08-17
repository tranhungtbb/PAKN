using Bugsnag;
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
using System.IO;
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

		[HttpPost]
		[Route("ImportDataInvididual")]
		[Authorize]
		public async Task<ActionResult<object>> ImportDataInvididual(string folder = null)
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

			try
			{
				// get number of rows and columns in the sheet
				int rows = worksheet.Dimension.Rows; // 20
				int columns = worksheet.Dimension.Columns; // 7

				//create a list to hold all the values
				//List<Models.BusinessIndividual.BIIndividualInsertIN> individualList = new List<Models.BusinessIndividual.BIIndividualInsertIN>();
				int count = 0;
				// loop through the worksheet rows and columns

				for (int i = 3; i <= rows; i++)
				{
					Models.BusinessIndividual.BIIndividualInsertIN ind = new Models.BusinessIndividual.BIIndividualInsertIN();
					ind.FullName = worksheet.Cells[i, 1].Value == null ? null : worksheet.Cells[i, 1].Value.ToString();
					if (string.IsNullOrEmpty(ind.FullName)) { continue; }
					ind.Email = worksheet.Cells[i, 2].Value == null ? null : worksheet.Cells[i, 2].Value.ToString();
					//if (string.IsNullOrEmpty(ind.Email)) { continue; }
					ind.Phone = worksheet.Cells[i, 3].Value == null ? null : worksheet.Cells[i, 3].Value.ToString();
					if (string.IsNullOrEmpty(ind.Phone)) { continue; }
					if (worksheet.Cells[i, 4].Value != null)
					{
						ind.Gender = worksheet.Cells[i, 4].Value.ToString() == "Nam" ? true : false;
					}
					else { continue; }
					ind.IDCard = worksheet.Cells[i, 5].Value == null ? null : worksheet.Cells[i, 5].Value.ToString();
					if (string.IsNullOrEmpty(ind.IDCard)) { continue; }
					if (worksheet.Cells[i, 6].Value != null)
					{
						ind.DateOfIssue = Convert.ToDateTime(worksheet.Cells[i, 6].Value.ToString());
					}

					ind.IssuedPlace = worksheet.Cells[i, 7].Value == null ? null : worksheet.Cells[i, 7].Value.ToString();
					if (worksheet.Cells[i, 8].Value == null) { continue; }
					else {
						ind.Nation = worksheet.Cells[i, 8].Value.ToString();
					}
					var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(ind.Phone);
					if (hasOne != null && hasOne.Any()) continue;

					// check exist:Phone,Email,IDCard
					var checkExists = await new BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", ind.Phone, 0);
					if (checkExists[0].Exists.Value)
						continue;
					if (!string.IsNullOrEmpty(ind.Email))
					{
						checkExists = await new BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", ind.Email, 0);
						if (checkExists[0].Exists.Value)
							continue;
					}
					checkExists = await new BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", ind.IDCard, 0);
					if (checkExists[0].Exists.Value)
						continue;
					ind.ProvinceId = null;
					ind.DistrictId = null;
					ind.WardsId = null;
					//var sc = worksheet.Cells[i, 8].Value;
					if (worksheet.Cells[i, 9].Value == "" || worksheet.Cells[i, 9].Value == null)
					{
						continue;
					}
					else
					{
						List<CAAdministrativeUnitGetByNameLevel> ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 9].Value.ToString(), 1, null);
						if (ltsAdmintrative.Count > 0)
						{
							ind.ProvinceId = ltsAdmintrative.FirstOrDefault().Id;
							if (worksheet.Cells[i, 10].Value != null)
							{
								ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 10].Value.ToString(), 2, ind.ProvinceId);
								if (ltsAdmintrative.Count > 0)
								{
									ind.DistrictId = ltsAdmintrative.FirstOrDefault().Id;
									if (worksheet.Cells[i, 11].Value != null)
									{
										ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 11].Value.ToString(), 3, ind.DistrictId);
										if (ltsAdmintrative.Count > 0) { ind.WardsId = ltsAdmintrative.FirstOrDefault().Id; }
									}
									else
									{
										continue;
									}
								}

                            }
                            else
                            {
								continue;
                            }
						}
					}

					ind.Address = worksheet.Cells[i, 12].Value == null ? null : worksheet.Cells[i, 12].Value.ToString();
					if (string.IsNullOrEmpty(ind.Address)) { continue; }
					if (worksheet.Cells[i, 13].Value != null)
					{
						ind.BirthDay = Convert.ToDateTime(worksheet.Cells[i, 13].Value.ToString());
					}
					else { continue; }
					if (worksheet.Cells[i, 14].Value != null)
					{
						ind.Status = worksheet.Cells[i, 14].Value.ToString().ToLower() == "hiệu lực" ? 1 : 0;
					}
					else { continue; }

					ind.IsActived = ind.Status == 1 ? true : false;
					ind.IsDeleted = false;

					string defaultPwd = "abc123";
					var pwd = GeneratePwdModelBase.generatePassword(defaultPwd);
					var account = new SYUserInsertIN
					{
						Password = pwd.Password,
						Salt = pwd.Salt,
						Phone = ind.Phone,
						Email = ind.Email,
						UserName = ind.Phone,
						FullName = ind.FullName,
						Gender = ind.Gender,
						Address = ind.Address,//
						TypeId = 2,
						Type = 2,
						IsActived = true,
						IsDeleted = false,
						CountLock = 0,
						LockEndOut = DateTime.Now,
						IsSuperAdmin = false,
					};
					await new SYUserInsert(_appSetting).SYUserInsertDAO(account);
					var accRs = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(account.UserName);
					ind.CreatedDate = DateTime.Now;
					ind.CreatedBy = Convert.ToInt32(new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext));
					ind.UpdatedBy = 0;
					ind.UpdatedDate = null;
					//ind.Status = 1;
					ind.IsDeleted = false;
					ind.UserId = accRs[0].Id;
					var s = await new Models.BusinessIndividual.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(ind);
					if (s > 0)
					{
						count++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"CountSuccess", count},
					{"CountError", rows -2 - count }
				};

				// lưu log

				SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
				{
					UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext),
					FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext),
					Action = "",
					IPAddress = "",
					MACAddress = "",
					Description = "Import file Người dân",
					CreatedDate = DateTime.Now,
					Status = 1,
					Exception = null
				};
				if (count == 0) { sYSystemLogInsertIN.Status = 0; }
				await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);

				return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception e)
			{
				SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
				{
					UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext),
					FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext),
					Action = "Import file",
					IPAddress = "",
					MACAddress = "",
					Description = "Import file Người dân",
					CreatedDate = DateTime.Now,
					Status = 0,
					Exception = e.Message.ToString()
				};
				await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = e.Message };
			}
			finally
			{
				// delete file luôn
				// 
				System.IO.File.Delete(fileNamePath);
			}			
		}

		[HttpPost, DisableRequestSizeLimit]
		[Route("ImportDataBusiness")]
		[Authorize]
		public async Task<ActionResult<object>> ImportDataBusiness(string folder = null)
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

			try
			{
				// get number of rows and columns in the sheet
				int rows = worksheet.Dimension.Rows; // 20

				//create a list to hold all the values
				//List<BI_BusinessInsertIN> businessList = new List<BI_BusinessInsertIN>();
				int count = 0;
				// loop through the worksheet rows and columns
				for (int i = 3; i <= rows; i++)
				{
					BI_BusinessInsertIN model = new BI_BusinessInsertIN();

					model.RepresentativeName = worksheet.Cells[i, 1].Value == null ? null : worksheet.Cells[i, 1].Value.ToString();
					if (String.IsNullOrEmpty(model.RepresentativeName)) { continue; }
					model.Email = worksheet.Cells[i, 2].Value == null ? null : worksheet.Cells[i, 2].Value.ToString();
					model.Phone = worksheet.Cells[i, 3].Value == null ? null : worksheet.Cells[i, 3].Value.ToString();
					// check phone table user và business, email

					var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
					if (hasOne != null && hasOne.Count > 0) continue;

					var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
					if (checkExists[0].Exists.Value) continue;
					if (!string.IsNullOrEmpty(model.Email))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
						if (checkExists[0].Exists.Value) continue;
					}
					if (worksheet.Cells[i, 4].Value == null) { continue; }
					model.RepresentativeGender = worksheet.Cells[i, 4].Value.ToString() == "Nam" ? true : false;
					if (worksheet.Cells[i, 5].Value != null)
					{
						model.RepresentativeBirthDay = Convert.ToDateTime(worksheet.Cells[i, 5].Value.ToString());
					}
					model.Address = worksheet.Cells[i, 6].Value == null ? null : worksheet.Cells[i, 6].Value.ToString();
					if (worksheet.Cells[i, 7].Value == null) { continue; }
					else
					{
						model.Nation = worksheet.Cells[i, 7].Value.ToString();
					}
					//model.Nation = worksheet.Cells[i, 7].Value == null ? null : worksheet.Cells[i, 7].Value.ToString();
					model.ProvinceId = null;
					model.DistrictId = null;
					model.WardsId = null;
					var s = worksheet.Cells[i, 8].Value;
					if (worksheet.Cells[i, 8].Value == "" || worksheet.Cells[i, 8].Value == null)
					{
						continue;
					}
					else
					{
						List<CAAdministrativeUnitGetByNameLevel> ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 8].Value.ToString(), 1, null);
						if (ltsAdmintrative.Count > 0)
						{
							model.ProvinceId = ltsAdmintrative.FirstOrDefault().Id;
							if (worksheet.Cells[i, 9].Value != null)
							{
								ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 9].Value.ToString(), 2, model.ProvinceId);
								if (ltsAdmintrative.Count > 0)
								{
									model.DistrictId = ltsAdmintrative.FirstOrDefault().Id;
									if (worksheet.Cells[i, 10].Value != null)
									{
										ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 10].Value.ToString(), 3, model.DistrictId);
										if (ltsAdmintrative.Count > 0) { model.WardsId = ltsAdmintrative.FirstOrDefault().Id; }
									}
									else
                            {
								continue;
                            }
								}
							}
							else
							{
								continue;
							}
						}
					}

					model.Business = worksheet.Cells[i, 11].Value == null ? null : worksheet.Cells[i, 11].Value.ToString();
					if (string.IsNullOrEmpty(model.Business)) { continue; }
					model.BusinessRegistration = worksheet.Cells[i, 12].Value == null ? null : worksheet.Cells[i, 12].Value.ToString();
					model.DecisionOfEstablishing = worksheet.Cells[i, 13].Value == null ? null : worksheet.Cells[i, 13].Value.ToString();
					if (worksheet.Cells[i, 14].Value != null)
					{
						model.DateOfIssue = Convert.ToDateTime(worksheet.Cells[i, 14].Value.ToString());
					}

					model.Tax = worksheet.Cells[i, 15].Value == null ? null : worksheet.Cells[i, 15].Value.ToString();

					// check OrgPhone, OrgEmail, BusinessRegistration, DecisionOfEstablishing, Tax
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, 0);
					if (checkExists[0].Exists.Value) continue;
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", model.DecisionOfEstablishing, 0);
					if (checkExists[0].Exists.Value) continue;
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", model.Tax, 0);
					if (checkExists[0].Exists.Value) continue;

					model.OrgProvinceId = null;
					model.OrgDistrictId = null;
					model.OrgWardsId = null;
					if (worksheet.Cells[i, 16].Value == "" || worksheet.Cells[i, 16].Value == null)
					{
						continue;
					}
					else
					{
						List<CAAdministrativeUnitGetByNameLevel> ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 16].Value.ToString(), 1, null);
						if (ltsAdmintrative.Count > 0)
						{
							model.OrgProvinceId = ltsAdmintrative.FirstOrDefault().Id;
							if (worksheet.Cells[i, 17].Value != null)
							{
								ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 17].Value.ToString(), 2, model.OrgProvinceId);
								if (ltsAdmintrative.Count > 0)
								{
									model.OrgDistrictId = ltsAdmintrative.FirstOrDefault().Id;
									if (worksheet.Cells[i, 18].Value != null)
									{
										ltsAdmintrative = await new CAAdministrativeUnitGetByNameLevel(_appSetting).CAAdministrativeUnitsGetByNameDAO(worksheet.Cells[i, 18].Value.ToString(), 3, model.OrgDistrictId);
										if (ltsAdmintrative.Count > 0) { model.OrgWardsId = ltsAdmintrative.FirstOrDefault().Id; }
									}
									else
									{
										continue;
									}
								}
							}
							else
							{
								continue;
							}
						}
					}

					model.OrgAddress = worksheet.Cells[i, 19].Value == null ? null : worksheet.Cells[i, 19].Value.ToString();
					if (string.IsNullOrEmpty(model.OrgAddress)) { continue; }
					model.OrgPhone = worksheet.Cells[i, 20].Value == null ? null : worksheet.Cells[i, 20].Value.ToString();
					if (string.IsNullOrEmpty(model.OrgPhone)) { continue; }
					model.OrgEmail = worksheet.Cells[i, 21].Value == null ? null : worksheet.Cells[i, 21].Value.ToString();
					if (worksheet.Cells[i, 22].Value != null)
					{
						model.Status = worksheet.Cells[i, 22].Value.ToString().ToLower() == "hiệu lực" ? 1 : 0;
					}
					else { continue; }
					//if (string.IsNullOrEmpty(model.OrgEmail)) { continue; }
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", model.OrgPhone, 0);
					if (checkExists[0].Exists.Value) continue;
					if (!string.IsNullOrEmpty(model.OrgEmail))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, 0);
						if (checkExists[0].Exists.Value) continue;
					}

					string defaultPwd = "abc123";
					var pwd = GeneratePwdModelBase.generatePassword(defaultPwd);
					var account = new SYUserInsertIN
					{
						Password = pwd.Password,
						Salt = pwd.Salt,
						Phone = model.Phone,
						Email = model.Email,
						UserName = model.Phone,
						FullName = model.Business,
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
					model.CreatedDate = DateTime.Now;
					model.CreatedBy = 0;
					model.UpdatedBy = 0;
					model.UpdatedDate = null;
					model.IsDeleted = false;
					model.UserId = accRs[0].Id;
					model.IsActived = true;
					model.IsDeleted = false;
					var rs2 = await new BI_BusinessInsert(_appSetting).BusinessInsertDAO(model);
					if (rs2 > 0)
					{
						count++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
				{
					{"CountSuccess", count},
					{"CountError", rows - 2 - count}
				};
					
				// lưu log


				SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
				{
					UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext),
					FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext),
					Action = "Import file",
					IPAddress = "",
					MACAddress = "",
					Description = "Import file Doanh nghiệp",
					CreatedDate = DateTime.Now,
					Status = 1,
					Exception = null
				};
				if (count == 0) { sYSystemLogInsertIN.Status = 0; }
				await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);

				return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception e)
			{
				SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
				{
					UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext),
					FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext),
					Action = "Import file",
					IPAddress = "",
					MACAddress = "",
					Description = "Import file Doanh nghiệp",
					CreatedDate = DateTime.Now,
					Status = 0,
					Exception = e.Message.ToString()
				};
				await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = e.Message };
			}
			finally {
				System.IO.File.Delete(fileNamePath);
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
		public async Task<object> InvididualRegister([FromBody] Models.BusinessIndividual.BIIndividualInsertIN_Cus model)
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

				//if(model.ProvinceId == 0)
    //            {
				//	model.ProvinceId = null;
				//	model.DistrictId = null;
				//	model.WardsId = null;
    //            }

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
				model.CreatedBy = Convert.ToInt32(new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext));
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
		public async Task<ActionResult<object>> InvididualUpdate(BI_InvididualUpdateIN_body _bI_InvididualUpdateIN)
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
				if (DateTime.TryParseExact(_bI_InvididualUpdateIN._DateOfIssue, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime _dateOfIssue))
				{
					dateOfIssue = _dateOfIssue;
					//return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Định dạng ngày cấp không hợp lệ" };
				}
				//if (string.IsNullOrEmpty(_DateOfIssue)) _bI_InvididualUpdateIN.DateOfIssue = null;
				//else _bI_InvididualUpdateIN.DateOfIssue = dateOfIssue;
				//if (string.IsNullOrEmpty(_BirthDay)) _bI_InvididualUpdateIN.BirthDate = null;
				//else _bI_InvididualUpdateIN.BirthDate = birthDay;
				//if (_bI_InvididualUpdateIN.ProvinceId == 0)
				//{
				//	_bI_InvididualUpdateIN.ProvinceId = null;
				//	_bI_InvididualUpdateIN.DistrictId = null;
				//	_bI_InvididualUpdateIN.WardsId = null;
				//}

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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
		[Route("BusinessRegister")]
		public async Task<object> BusinessRegister([FromBody] BI_BusinessInsertIN_Cus model)
		{
			try
			{
				var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
				if (hasOne != null && hasOne.Any()) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại tài khoản đăng nhập đã tồn tại" };

				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
                if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
                    checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
                    if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
                }
    //            checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, 0);
				//if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
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

     //           if (model.DistrictId == 0)
     //           {
					//model.DistrictId = null;
					//model.ProvinceId = null;
					//model.WardsId = null;
					////model.OrgDistrictId = null;
					////model.OrgProvinceId = null;
					////model.OrgWardsId = null;
     //           }

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
					FullName = model.Business,
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

				//if (string.IsNullOrEmpty(model._DateOfIssue)) model.DateOfIssue = null;
				//else model.DateOfIssue = dateOfIssue;
				//if (string.IsNullOrEmpty(model._RepresentativeBirthDay)) model.RepresentativeBirthDay = null;
				//else model.RepresentativeBirthDay = birdDay;
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
		[Authorize]
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


				//if (model.DistrictId == 0)
				//{
				//	model.DistrictId = null;
				//	model.ProvinceId = null;
				//	model.WardsId = null;
				//	//model.OrgDistrictId = null;
				//	//model.OrgProvinceId = null;
				//	//model.OrgWardsId = null;
				//}

				var rsUpdateAcc = new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(new SYUserUpdateInfoIN
				{
					Id = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext),
					FullName = model.Business,
					Address = model.Address,
				});

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
