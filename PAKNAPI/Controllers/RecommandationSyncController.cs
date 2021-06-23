using Bugsnag;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecommandationSyncController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public RecommandationSyncController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

        [Route("CongThongTinDienTuTinhPagedList")]
        [HttpGet]
        public async Task<object> CongThongTinDienTuTinh(
            string questioner,
            string question,
            int pageIndex = 1,
            int pageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.CongThongTinDienTuTinhGetPagedList(questioner, question, pageIndex, pageSize);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch(Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("CongThongTinDichVuHCCPagedList")]
        [HttpGet]
        public async Task<object> CongThongTinDichVuHCCPagedList(
            string questioner,
            string question,
            int pageIndex = 1,
            int pageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.CongThongTinDichVuHCCGetPagedList(questioner, question, pageIndex, pageSize);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("HeThongPANKChinhPhuPagedList")]
        [HttpGet]
        public async Task<object> HeThongPANKChinhPhuPagedList(
            string title,
            string status,
            string createdBy,
            int pageIndex = 1,
            int pageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.HeThongPANKChinhPhuGetPagedList(title, status, createdBy, pageIndex, pageSize);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("HeThongQuanLyKienNghiCuTriPagedList")]
        [HttpGet]
        public async Task<object> HeThongQuanLyKienNghiCuTriPagedList(
            string fields,
            string status,
            int pageIndex = 1,
            int pageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.HeThongQuanLyKienNghiCuTriGetPagedList(fields, status, pageIndex, pageSize);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [Route("PUGetPagedList")]
        [HttpGet]
        public async Task<object> PUGetPagedList(
            string keyword,
            int src = 1,
            int pageIndex = 1,
            int pageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.PURecommandationSyncGetPagedList(keyword, src, pageIndex, pageSize);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("PUGetGetById")]
        [HttpGet]
        public async Task<object> PUGetGetById(
            long id,
            int src = 1,
            int pageIndex = 1,
            int pageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.PURecommandationSyncGetDetail(id, src);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = json };
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
