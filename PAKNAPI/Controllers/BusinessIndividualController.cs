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


		[HttpGet]
        [Authorize]
        [Route("BusinessIndividualGetDataForCreate")]
        public async Task<ActionResult<object>> BusinessIndividualGetDataForCreate()
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new BusinessIndividualDAO(_appSetting).BusinessIndividualGetDataForCreate() };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

		[HttpGet]
		[Authorize]
		[Route("BusinessIndividualGetAllWithProcessBase")]
		public async Task<ActionResult<object>> BusinessIndividualGetAllWithProcessBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetAllWithProcess> rsMRRecommendationGetAllWithProcess = await new MRRecommendationGetAllWithProcess(_appSetting).MRRecommendationGetAllWithProcessDAO(Code, SendName, Content, UnitId, Field, Status, UnitProcessId, UserProcessId, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetAllWithProcess", rsMRRecommendationGetAllWithProcess},
						{"TotalCount", rsMRRecommendationGetAllWithProcess != null && rsMRRecommendationGetAllWithProcess.Count > 0 ? rsMRRecommendationGetAllWithProcess[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationGetAllWithProcess != null && rsMRRecommendationGetAllWithProcess.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationGetAllWithProcess != null && rsMRRecommendationGetAllWithProcess.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				//_bugsnag.Notify(ex);
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
		public async Task<ActionResult<object>> BusinessGetAllOnPageBase(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			try
			{
				List<BusinessGetAllOnPage> rsBusinessGetAllOnPageBase = await new BusinessGetAllOnPage(_appSetting).BusinessGetAllOnPageDAO(PageSize, PageIndex, FullName, Address, Phone, Email, IsActived, SortDir, SortField);
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
