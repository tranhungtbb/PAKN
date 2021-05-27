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
using Bugsnag;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;

namespace PAKNAPI.Controllers
{
    [Route("api/SYIntroduce")]
    [ApiController]
   
    public class IntroduceController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public IntroduceController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

        //'SYIntroduce/IntroduceGetInfo'
        [HttpGet]
        [Authorize]
        [Route("IntroduceGetInfo")]
        public async Task<object> SYIntroduceGetInfo() {
            try {
                IntroduceModel result = new IntroduceModel();

                List<SYIntroduce> lstIntroduce = await new SYIntroduce(_appSetting).SYIntroduceGetInfoDAO();

                if (lstIntroduce.Count > 0) {
                    result.model = lstIntroduce.FirstOrDefault();
                    result.lstIntroduceFunction = await new SYIntroduceFunction(_appSetting).SYIntroduceFunctionGetByIntroductId(result.model.Id);
                    //result.lstIntroduceUnit = await new SYIntroduceUnit(_appSetting).SYIntroduceUnitGetByIntroduceId(result.model.Id);

                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Success" };

                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Introduce not exit" };
                }
            }
            catch(Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("IntroduceUpdate")]
        public async Task<object> SYIntroduceUpdate(IntroduceModel model) {
            try
            {
                await new SYIntroduce(_appSetting).SYIntroduceUpdateDAO(model.model);
                foreach (var item in model.lstIntroduceFunction) {
                    // insert file nữa này
                    await new SYIntroduceFunction(_appSetting).SYIntroduceFunctionUpdateDAO(item);
                }

                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = null };
            }
            catch (Exception ex) {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// introduce unit
        /// 

        [HttpGet]
        [Authorize]
        [Route("IntroduceUnitGetOnPage")]
        public async Task<object> SYIntroduceUnitGetOnPage(int? IntroduceId, int? PageSize, int? PageIndex)
        {
            try
            {
                var syIntroduceUnitGetOnPage = await new SYIntroduceUnit(_appSetting).SYIntroduceUnitGetOnPageByIntroduceId(IntroduceId,PageSize,PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"SYIntroduceUnitGetOnPage", syIntroduceUnitGetOnPage},
                        {"TotalCount", syIntroduceUnitGetOnPage != null && syIntroduceUnitGetOnPage.Count > 0 ? syIntroduceUnitGetOnPage[0].RowNumber : 0},
                        {"PageIndex", syIntroduceUnitGetOnPage != null && syIntroduceUnitGetOnPage.Count > 0 ? PageIndex : 0},
                        {"PageSize", syIntroduceUnitGetOnPage != null && syIntroduceUnitGetOnPage.Count > 0 ? PageSize : 0},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpPost]
        [Authorize]
        [Route("IntroduceUnitInsert")]
        public async Task<object> SYIntroduceUnitInsert(SYIntroduceUnit model)
        {
            try
            {
                var result  = (int)await new SYIntroduceUnit(_appSetting).SYIntroduceUnitInsertDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR,  Result = result};
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpPost]
        [Authorize]
        [Route("IntroduceUnitUpdate")]
        public async Task<object> SYIntroduceUnitUpdate(SYIntroduceUnit model)
        {
            try
            {
                var result = (int)await new SYIntroduceUnit(_appSetting).SYIntroduceUnitUpdateDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else
                {
                    return new ResultApi { Success = ResultCode.ORROR, Result = result };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("IntroduceUnitDetete")]
        public async Task<object> SYIntroduceUnitDelete(SYIntroduceUnitDelete model)
        {
            try
            {
                var result = (int)await new SYIntroduceUnit(_appSetting).SYIntroduceUnitDeleteDAO(model);

                if (result > 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                    return new ResultApi { Success = ResultCode.OK, Result = result };
                }
                else
                {
                    return new ResultApi { Success = ResultCode.ORROR, Result = result };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

    }
}
