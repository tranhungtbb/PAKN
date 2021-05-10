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
		[Route("IndividualGetAllOnPageBase")]
		public async Task<ActionResult<object>> IndividualGetAllOnPageBase(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
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


		//[HttpGet]
		//[Authorize]
		//[Route("CAUnitGetAllOnPageBase")]
		//public async Task<ActionResult<object>> CAUnitGetAllOnPageBase(int? PageSize, int? PageIndex, int? ParentId, byte? UnitLevel, string Name, string Phone, string Email, string Address, bool? IsActived, bool? IsMain, string SortDir, string SortField)
		//{
		//	try
		//	{
		//		List<CAUnitGetAllOnPage> rsCAUnitGetAllOnPage = await new CAUnitGetAllOnPage(_appSetting).CAUnitGetAllOnPageDAO(PageSize, PageIndex, ParentId, UnitLevel, Name, Phone, Email, Address, IsActived, IsMain, SortDir, SortField);
		//		IDictionary<string, object> json = new Dictionary<string, object>
		//			{
		//				{"CAUnitGetAllOnPage", rsCAUnitGetAllOnPage},
		//				{"TotalCount", rsCAUnitGetAllOnPage != null && rsCAUnitGetAllOnPage.Count > 0 ? rsCAUnitGetAllOnPage[0].RowNumber : 0},
		//				{"PageIndex", rsCAUnitGetAllOnPage != null && rsCAUnitGetAllOnPage.Count > 0 ? PageIndex : 0},
		//				{"PageSize", rsCAUnitGetAllOnPage != null && rsCAUnitGetAllOnPage.Count > 0 ? PageSize : 0},
		//			};
		//		return new ResultApi { Success = ResultCode.OK, Result = json };
		//	}
		//	catch (Exception ex)
		//	{
		//		_bugsnag.Notify(ex);
		//		new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

		//		return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
		//	}
		//}


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



	}
}
