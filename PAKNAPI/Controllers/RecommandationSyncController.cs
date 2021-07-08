using Bugsnag;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
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
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch(Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("CongThongTinDienTuTinhGetById")]
        [HttpGet]
        public async Task<object> CongThongTinDienTuTinhGetByIdBase(int Id)
        {
            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.CongThongTinDienTuTinhGetByIdDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("MR_Sync_CuTriTinhKhanhHoaGetListBase")]
        public async Task<ActionResult<object>> MR_Sync_CuTriTinhKhanhHoaGetList(string Content, string Unit, string Place, int? Field, int? Status, int? PageSize, int? PageIndex)
        {
            try
            {
                List<MR_CuTriTinhKhanhHoaGetPage> rsMRRecommendationKNCTGetAllWithProcess = await new MR_CuTriTinhKhanhHoaGetPage(_appSetting).MR_Sync_CuTriTinhKhanhHoaGetListDAO(Content, Unit, Place, Field, Status, PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationKNCTGetAllWithProcess", rsMRRecommendationKNCTGetAllWithProcess},
                        {"TotalCount", rsMRRecommendationKNCTGetAllWithProcess != null && rsMRRecommendationKNCTGetAllWithProcess.Count > 0 ? rsMRRecommendationKNCTGetAllWithProcess[0].RowNumber : 0},
                        {"PageIndex", rsMRRecommendationKNCTGetAllWithProcess != null && rsMRRecommendationKNCTGetAllWithProcess.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsMRRecommendationKNCTGetAllWithProcess != null && rsMRRecommendationKNCTGetAllWithProcess.Count > 0 ? PageSize : 0},
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
        [Route("MR_Sync_CuTriTinhKhanhHoaGetById")]
        public async Task<ActionResult<object>> MRRecommendationKNCTGetByIdBase(int Id)
        {
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                List<MR_CuTriTinhKhanhHoa> rsMRRecommendationKNCTGetById = await new MR_CuTriTinhKhanhHoa(_appSetting).MR_Sync_CuTriTinhKhanhHoaGetByIdDAO(Id);
                List<MR_SyncFileAttach> files = await new MR_SyncFileAttach(_appSetting).MR_Sync_CuTriTinhKhanhHoaFileAttachGetByKNCTIdDAO(Id);

                foreach (var item in files) {
                    item.FilePath = decrypt.EncryptData(item.FilePath);
                }

                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationKNCTGetById", rsMRRecommendationKNCTGetById},
                        {"FileAttach", files},
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
                    {"Data", rs},
                    {"TotalCount", rs != null && rs.Count > 0 ? rs[0].RowNumber : 0},
                    {"PageIndex", rs != null && rs.Count > 0 ? pageIndex : 0},
                    {"PageSize", rs != null && rs.Count > 0 ? pageSize : 0},
                };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("CongThongTinDichVuHCCGetById")]
        [HttpGet]
        public async Task<object> CongThongTinDichVuHCCGetByIdBase(int Id)
        {
            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.CongThongTinDichVuHCCGetById(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [Route("HeThongPANKChinhPhuPagedList")]
        [HttpGet]
        public async Task<object> HeThongPANKChinhPhuPagedList(
            string Questioner,
            string Question,
            int PageIndex = 1,
            int PageSize = 20)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.HeThongPANKChinhPhuGetPagedList(Questioner, Question, PageIndex, PageSize);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs},
                    {"TotalCount", rs != null && rs.Count > 0 ? rs[0].RowNumber : 0},
                    {"PageIndex", rs != null && rs.Count > 0 ? PageIndex : 0},
                    {"PageSize", rs != null && rs.Count > 0 ? PageSize : 0},
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
        [Route("MR_Sync_PANKChinhPhuGetByObjectId")]
        public async Task<ActionResult<object>> MR_Sync_PANKChinhPhuGetByObjectId(int Id)
        {
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                List<DichViCongQuocGia> rsMRRecommendationDVCGetById = await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaGetByObjecId(Id);
                List<MR_SyncFileAttach> files = await new MR_SyncFileAttach(_appSetting).MR_Sync_DichVuCongQuocGiaFileAttachGetByDVCIdDAO(Id);

                foreach (var item in files)
                {
                    item.FilePath = decrypt.EncryptData(item.FilePath);
                }

                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationPAKNCPGetById", rsMRRecommendationDVCGetById},
                        {"FileAttach", files},
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




        [Route("PUGetPagedList")]
        [HttpGet]
        [Authorize]
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
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("PUGetGetById")]
        [HttpGet]
        [Authorize]
        public async Task<object> PUGetGetById(
            long id,
            int src = 1)
        {

            try
            {
                var repository = new RecommandationSyncDAO(_appSetting);

                var rs = await repository.PURecommandationSyncGetDetail(id, src);
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs}
                };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

    }
}
