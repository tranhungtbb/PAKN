﻿using Bugsnag;
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
    [Route("api/individual")]
	[ApiController]
	[ValidateModel]
	[OpenApiTag("Cá nhân", Description = "Cá nhân")]
	public class IndividualController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IClient _bugsnag;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _config;
		public IndividualController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting, Microsoft.Extensions.Configuration.IConfiguration config)
		{
			_appSetting = appSetting;
			_hostingEnvironment = hostingEnvironment;
			_config = config;
		}
		/// <summary>
		/// import cá nhân với excel - Authorize
		/// </summary>
		/// <param name="folder"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("import-data-individual")]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
				return new ResultApi { Success = ResultCode.ORROR, Message = e.Message };
			}
			finally
			{
				// delete file luôn
				// 
				System.IO.File.Delete(fileNamePath);
			}			
		}
		/// <summary>
		/// danh sách người dân - Authorize
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="FullName"></param>
		/// <param name="Address"></param>
		/// <param name="Phone"></param>
		/// <param name="Email"></param>
		/// <param name="Status"></param>
		/// <param name="SortDir"></param>
		/// <param name="SortField"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-list-individual-on-page")]
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
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa người dân - Authorize
		/// </summary>
		/// <param name="_bi_IndivialDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("delete")]
		public async Task<ActionResult<object>> IndivialDelete(BI_IndivialDeleteIN _bi_IndivialDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_IndivialDelete(_appSetting).BI_IndivialDeleteDAO(_bi_IndivialDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập trạng thái người dân - Authorize
		/// </summary>
		/// <param name="_bI_IndivialChageStatusIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("change-status")]
		public async Task<ActionResult<object>> IndivialChangeStatus(BI_IndivialChageStatusIN _bI_IndivialChageStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BI_IndivialChageStatus(_appSetting).IndivialChageStatusDAO(_bI_IndivialChageStatusIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// thêm mới người dân - Authorize
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("insert")]
		public async Task<object> InvididualRegister([FromBody] Models.BusinessIndividual.BIIndividualInsertIN model)
		{
			try
			{
				// validate


				//if (model.BirthDay >= DateTime.Now)
				//{
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn hoặc bằng ngày hiện tại" };
				//}

				//if (model.DateOfIssue != null && model.DateOfIssue < model.BirthDay)
				//{
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn ngày thành lập" };
				//}

				//var hasOne = await new SYUserGetByUserName(_appSetting).SYUserGetByUserNameDAO(model.Phone);
				//if (hasOne != null && hasOne.Any()) {
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				//}

				// check exist:Phone,Email,IDCard
				var checkExists = await new BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", model.Phone, 0);
				if (checkExists[0].Exists.Value) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				}

                if (!string.IsNullOrEmpty(model.Email))
                {
                    checkExists = await new BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", model.Email, 0);
					if (checkExists[0].Exists.Value) {
						return new ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
					}   
                }
                checkExists = await new BI_IndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", model.IDCard, 0);
				if (checkExists[0].Exists.Value) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
				}

				//add login info
				var pwd = GeneratePwdModelBase.generatePassword(_config["PasswordDefault"]);
				var account = new SYUserInsertIN
				{
					Password = pwd.Password,
					Salt = pwd.Salt,
					Phone = model.Phone,
					Email = model.Email,
					UserName = model.IDCard,
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

				model.CreatedDate = DateTime.Now;
				model.CreatedBy = Convert.ToInt32(new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext));
				model.UpdatedBy = 0;
				model.UpdatedDate = DateTime.Now;
				model.Status = 1;
				model.IsDeleted = false;
				model.UserId = accRs[0].Id;
				await new Models.BusinessIndividual.BIIndividualInsert(_appSetting).BIIndividualInsertDAO(model);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}

			return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK };
		}

		/// <summary>
		/// cập nhập người dân - Authorize
		/// </summary>
		/// <param name="_bI_InvididualUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("update")]
		public async Task<ActionResult<object>> InvididualUpdate(BI_InvididualUpdateIN _bI_InvididualUpdateIN)
		{
			try
			{
				// validate
				
				//if (_bI_InvididualUpdateIN.BirthDay >= DateTime.Now)
				//{
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn hoặc bằng ngày hiện tại" };
				//}

				//if (_bI_InvididualUpdateIN.DateOfIssue != null && _bI_InvididualUpdateIN.DateOfIssue < _bI_InvididualUpdateIN.BirthDay)
				//{
				//	return new ResultApi { Success = ResultCode.ORROR, Message = "Ngày sinh không được lớn hơn ngày thành lập" };
				//}

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

				
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = await new BI_InvididualUpdate(_appSetting).BI_InvididualUpdateDAO(_bI_InvididualUpdateIN) };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết người dân - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		[Route("get-by-id")]
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

		//[HttpGet]
		//[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
		//[Route("IndividualCheckExistsBase")]
		//public async Task<ActionResult<object>> BIInvididualCheckExistsBase(string Field, string Value, long? Id)
		//{
		//	try
		//	{
		//		List<BIInvididualCheckExists> rsInvididualCheckExists = await new BIInvididualCheckExists(_appSetting).BIInvididualCheckExistsDAO(Field, Value, Id);
		//		IDictionary<string, object> json = new Dictionary<string, object>
		//			{
		//				{"BIInvididualCheckExists", rsInvididualCheckExists},
		//			};
		//		return new ResultApi { Success = ResultCode.OK, Result = json };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}

		/// <summary>
		/// check tồn tại người dân
		/// </summary>
		/// <param name="Field"></param>
		/// <param name="Value"></param>
		/// <param name="Id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("check-exists")]
		public async Task<ActionResult<object>> BIIndividualCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<BIIndividualCheckExists> rsBIIndividualCheckExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividualCheckExists", rsBIIndividualCheckExists},
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
