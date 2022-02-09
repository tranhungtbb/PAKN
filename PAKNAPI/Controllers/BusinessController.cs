using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
	/// <summary>
	/// doanh nghiệp
	/// </summary>
    [Route("api/business")]
	[ApiController]
	[ValidateModel]
	[OpenApiTag("Doanh nghiệp", Description = "Doanh nghiệp")]
	public class BusinessController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IClient _bugsnag;

		public BusinessController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_hostingEnvironment = hostingEnvironment;
			_bugsnag = bugsnag;
		}
		/// <summary>
		/// impport excel - Authorize
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		[HttpPost, DisableRequestSizeLimit]
		[Route("import-data-business")]
		[Authorize("ThePolicy")]
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


		/// <summary>
		/// danh sách doanh nghiệp - Authorize
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="RepresentativeName"></param>
		/// <param name="Address"></param>
		/// <param name="Phone"></param>
		/// <param name="Email"></param>
		/// <param name="Status"></param>
		/// <param name="SortDir"></param>
		/// <param name="SortField"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-business-on-page")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa doanh nghiệp - Authorize
		/// </summary>
		/// <param name="_bI_BusinessDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> BusinessDelete(BI_BusinessDeleteIN _bI_BusinessDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_BusinessDelete(_appSetting).BusinessDeleteDAO(_bI_BusinessDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập trạng thái doanh nghiệp = Authorize
		/// </summary>
		/// <param name="_bI_BusinessChageStatusIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("change-status")]
		public async Task<ActionResult<object>> BusinessChageStatus(BI_BusinessChageStatusIN _bI_BusinessChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_BusinessChageStatus(_appSetting).BI_BusinessChageStatusDAO(_bI_BusinessChageStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// thêm mới doanh nghiệp - Authorize
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<object> BusinessRegister([FromBody] BI_BusinessInsertIN model)
		{
			try
			{
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, 0);
                if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
                if (!string.IsNullOrEmpty(model.Email))
                {
                    checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, 0);
                    if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
                }
				//checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, 0);
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
				
				//add login info
				string defaultPwd = "abc123";
				var pwd = GeneratePwdModelBase.generatePassword(defaultPwd);
				var account = new SYUserInsertIN
				{
					Password = pwd.Password,
					Salt = pwd.Salt,
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.BusinessRegistration,
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
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;
				var rs2 = await new BI_BusinessInsert(_appSetting).BusinessInsertDAO(model);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new Models.Results.ResultApi { Success = ResultCode.OK };
		}

		/// <summary>
		/// chi tiết doanh nghiệp - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
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

		/// <summary>
		///  cập nhập doanh nghiệp
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> BusinessUpdate([FromBody] BI_BusinessUpdateInfoIN model)
		{
			try
			{
				// validate


				//if (model.RepresentativeBirthDay != null && model.RepresentativeBirthDay >= DateTime.Now)
				//{
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh người đại diện không được lớn hơn ngày hiện tại" };
				//}

				//check ton tai
				var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", model.Phone, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				if (!string.IsNullOrEmpty(model.Email))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", model.Email, model.Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
				}
				//checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("IDCard", model.IDCard, model.Id);
				//if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgPhone", model.OrgPhone, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại doanh nghiệp đã tồn tại" };
				if (!string.IsNullOrEmpty(model.OrgEmail))
				{
					checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", model.OrgEmail, model.Id);
					if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
				}
				checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", model.BusinessRegistration, model.Id);
				if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
				//checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("DecisionOfEstablishing", model.DecisionOfEstablishing, model.Id);
				//if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Số quyết định thành lập đã tồn tại" };
				//checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Tax", model.Tax, model.Id);
				//if (checkExists[0].Exists.Value) return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "Mã số thuế đã tồn tại" };

			

				var rsUpdateAcc = new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(new SYUserUpdateInfoIN
				{
					Id = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext),
					FullName = model.Business,
					Address = model.Address,
				});

				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = await new BI_BusinessUpdateInfo(_appSetting).BI_BusinessUpdateInfoDAO(model) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách cá nhân doanh nghiệp theo đơn vị hành chính - Authorize
		/// </summary>
		/// <param name="LtsAdministrativeId"></param>
		/// <param name="Type"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-individual-business-by-provice-id")]
		public async Task<ActionResult<object>> BIIndividualOrBusinessGetDropListByProviceIdBase(string LtsAdministrativeId, int? Type)
		{
			try
			{
				List<BIIndividualOrBusinessGetDropListByProviceId> rsBIIndividualOrBusinessGetDropListByProviceId = await new BIIndividualOrBusinessGetDropListByProviceId(_appSetting).BIIndividualOrBusinessGetDropListByProviceIdDAO(LtsAdministrativeId, Type);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividualOrBusinessGetDropListByProviceId", rsBIIndividualOrBusinessGetDropListByProviceId},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// get-drop-list-individual-business - Authorize
		/// </summary>
		/// <param name="SmsId"></param>
		/// <param name="Type"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-drop-list-individual-business")]
		public async Task<ActionResult<object>> BIIndividualBusinessGetDrop(int? SmsId, int? Type)
		{
			try
			{
				List<IndividualBusinessGetDrop> result = await new IndividualBusinessGetDrop(_appSetting).BIIndividualBusinessGetDrop(SmsId, Type);
				
				return new ResultApi { Success = ResultCode.OK, Result = result };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// check doanh nghiệp đã tồn tại
		/// </summary>
		/// <param name="Field"></param>
		/// <param name="Value"></param>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("check-exists")]
		public async Task<ActionResult<object>> BIBusinessCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<BIBusinessCheckExists> rsBIBusinessCheckExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessCheckExists", rsBIBusinessCheckExists},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}

}
