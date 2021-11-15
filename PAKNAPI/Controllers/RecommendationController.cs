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
using PAKNAPI.Models.Recommendation;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Bugsnag;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.Threading;

namespace PAKNAPI.Controller
{
    [Route("api/recommendation")]
    [ApiController]
    [ValidateModel]
    public class RecommendationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public RecommendationController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting, IClient bugsnag, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
            _bugsnag = bugsnag;
            _configuration = configuration;
        }

        /// <summary>
        /// get data for thêm mới pakn
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("get-data-for-create")]
        public async Task<ActionResult<object>> RecommendationGetDataForCreate()
        {
            try
            {
                var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForCreate(unitId) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// get data for chuyển tiếp pakn
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-data-for-forward")]
        public async Task<ActionResult<object>> RecommendationGetDataForForward()
        {
            try
            {
                var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForForward(unitId) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// get data for process
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-data-for-process")]
        public async Task<ActionResult<object>> RecommendationGetDataForProcess(int? UnitId)
        {
            try
            {
                var userId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForProcess(UnitId, userId) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// chi tiết pakn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-by-id")]
        public async Task<ActionResult<object>> RecommendationGetByID(int? Id)
        {
            try
            {
                RecommendationGetByIDResponse data = new RecommendationGetByIDResponse();
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetByID(Id) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// chi tiết pakn màn view chi tiết
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-detail-by-id")]
        public async Task<ActionResult<object>> RecommendationGetByIDView(int? Id)
        {
            try
            {
                RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
                var userProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                var unitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetByIDView(Id,userProcessId,unitProcessId) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-detail-public-by-id")]
        public async Task<ActionResult<object>> RecommendationGetByIDViewPublic(int? Id)
        {
            try
            {
                RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetByIDView(Id) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("check-file")]
        public async Task<ActionResult<object>> CheckFile()
        {
            try
            {
                var content = PdfTextExtractorCustom.PerformOCR("D:/SV_2021/PhanAnhKienNghi/Source/pakn/PAKNAPI/Upload/Recommendation/3369/undefined05112021082450.pdf", _hostingEnvironment);
                return new ResultApi { Success = ResultCode.OK, Result = content };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// thêm mới pakn
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("insert")]
        public async Task<ActionResult<object>> RecommendationInsert()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                RecommendationInsertRequest request = new RecommendationInsertRequest();
                request.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                request.UserType = new LogHelper(_appSetting).GetTypeFromRequest(HttpContext);
                request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                request.Data = JsonConvert.DeserializeObject<MRRecommendationInsertIN>(Request.Form["Data"].ToString(), jss);

                var ErrorMessage = ValidationForFormData.validObject(request.Data);
                if (ErrorMessage != null)
                {
                    return StatusCode(400, new ResultApi
                    {
                        Success = ResultCode.ORROR,
                        Result = 0,
                        Message = ErrorMessage
                    });
                }
                var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                var unitSend = request.Data.UnitId;
                if (unitId == 0)
                {
                    // người dân, doanh nghiệp gửi
                    if (request.Data.UnitId == null)
                    {
                        //var syUnitByField = await new SYUnitGetByField(_appSetting).SYUnitGetByFieldDAO(request.Data.Field);
                        //if (syUnitByField.Count == 0)
                        //{
                        //    request.Data.UnitId = dataMain.Id;
                        //}
                        //else
                        //{
                        //    request.Data.UnitId = syUnitByField.FirstOrDefault().Id;
                        //}
                        request.Data.UnitId = dataMain.Id;
                    }
                    if (request.Data.Status > 1 && dataMain != null && dataMain.Id != request.Data.UnitId && request.UserType != 1)
                    {
                        request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                    }
                }
                else
                {
                    // quản trị gửi
                    var unitInfo = new SYUnit(_appSetting).SYUnitGetByID(unitId);
                    if (request.Data.Status > 1)
                    {
                        request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                        request.Data.UnitId = unitId;
                    }

                }


                request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
                request.Files = Request.Form.Files;
                request.Data.CreatedBy = request.UserId;
                request.Data.CreatedDate = DateTime.Now;
                request.Data.CreateByType = new LogHelper(_appSetting).GetTypeFromRequest(HttpContext);
                request.Data.IsClone = false;
                MRRecommendationCheckExistedCode rsMRRecommendationCheckExistedCode = (await new MRRecommendationCheckExistedCode(_appSetting).MRRecommendationCheckExistedCodeDAO(request.Data.Code)).FirstOrDefault();
                if (rsMRRecommendationCheckExistedCode.Total > 0)
                {
                    request.Data.Code = await new MRRecommendationGenCodeGetCode(_appSetting).MRRecommendationGenCodeGetCodeDAO();
                }

                int? Id = Int32.Parse((await new MRRecommendationInsert(_appSetting).MRRecommendationInsertDAO(request.Data)).ToString());
                if (Id > 0)
                {
                    await new MRRecommendationGenCodeUpdateNumber(_appSetting).MRRecommendationGenCodeUpdateNumberDAO();
                    if (request.Data.Status > 1)
                    {
                        MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN = new MRRecommendationForwardInsertIN();

                        _mRRecommendationForwardInsertIN.RecommendationId = Id;
                        _mRRecommendationForwardInsertIN.UserSendId = request.UserId;
                        _mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
                        if (request.UserType != 1)
                        {
                            if (dataMain != null && dataMain.Id != request.Data.UnitId)
                            {
                                _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                            }
                            else
                            {
                                _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                            }
                            _mRRecommendationForwardInsertIN.UnitReceiveId = request.Data.UnitId;
                            _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                            _mRRecommendationForwardInsertIN.IsViewed = false;
                        }
                        else
                        {
                            _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                            _mRRecommendationForwardInsertIN.UnitReceiveId = unitSend;
                            _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.APPROVED;
                            _mRRecommendationForwardInsertIN.UserSendId = request.UserId;
                            _mRRecommendationForwardInsertIN.ProcessingDate = DateTime.Now;
                            _mRRecommendationForwardInsertIN.IsViewed = true;
                            _mRRecommendationForwardInsertIN.UnitSendId = request.Data.UnitId != null ? request.Data.UnitId : dataMain.Id;
                        }
                        await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);
                    }
                    if (request.Files != null && request.Files.Count > 0)
                    {
                        string folder = "Upload\\Recommendation\\" + Id;
                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        foreach (var item in request.Files)
                        {
                            MRRecommendationFilesInsertIN file = new MRRecommendationFilesInsertIN();
                            file.RecommendationId = Id;
                            file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                            string filePath = Path.Combine(folderPath, file.Name);
                            file.FilePath = Path.Combine(folder, file.Name);
                            file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            int FileId = Int32.Parse((await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(file)).ToString());
                            string contentType = "";
                            string content = "";
                            contentType = FileContentType.GetTypeOfFile(filePath);
                            bool isHasFullText = false;
                            Thread t = new Thread(async () => {
                                switch (contentType)
                                {
                                    case ".pdf":
                                        // không đọc được từ file scan
                                        //content = FileUtils.ExtractDataFromPDFFile(filePath);
                                        //content = PdfTextExtractorCustom.ReadPdfFile(filePath);
                                        content = PdfTextExtractorCustom.PerformOCR(filePath, _hostingEnvironment);
                                        isHasFullText = true;
                                        break;

                                    case ".docx":
                                        content = FileUtils.ReadFileDocExtension(filePath);
                                        isHasFullText = true;
                                        break;

                                    case ".doc":
                                        content = FileUtils.ExtractDocFile(filePath);
                                        isHasFullText = true;
                                        break;
                                }
                                if (isHasFullText)
                                {
                                    MRRecommendationFullTextInsertIN fulltext = new MRRecommendationFullTextInsertIN();
                                    fulltext.RecommendationId = Id;
                                    fulltext.FileId = FileId;
                                    fulltext.FullText = content;
                                    await new MRRecommendationFullTextInsert(_appSetting).MRRecommendationFullTextInsertDAO(fulltext);
                                }
                            });
                            t.Start();
                        }
                    }
                    MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                    foreach (var item in request.ListHashTag)
                    {
                        _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                        _mRRecommendationHashtagInsertIN.RecommendationId = Id;
                        _mRRecommendationHashtagInsertIN.HashtagId = item.Value;
                        _mRRecommendationHashtagInsertIN.HashtagName = item.Text;
                        await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
                    }

                    HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                    hisData.ObjectId = Id;
                    hisData.Type = 1;
                    hisData.Content = "";
                    hisData.Status = STATUS_RECOMMENDATION.CREATED;
                    hisData.CreatedBy = request.UserId;
                    hisData.CreatedDate = DateTime.Now;
                    await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                    if (request.UserType != 1 && request.Data.Status != STATUS_RECOMMENDATION.CREATED)
                    {
                        hisData = new HISRecommendationInsertIN();
                        hisData.ObjectId = Id;
                        hisData.Type = 1;
                        hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(request.Data.UnitId)).FirstOrDefault().Name;
                        hisData.Status = request.Data.Status;
                        hisData.CreatedBy = request.UserId;
                        hisData.CreatedDate = DateTime.Now;
                        await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                        await SYNotificationInsertTypeRecommendation(Id);
                    }
                    else
                    {
                        if (request.Data.Status != STATUS_RECOMMENDATION.CREATED)
                        {
                            hisData = new HISRecommendationInsertIN();
                            hisData.ObjectId = Id;
                            hisData.Type = 1;
                            hisData.Content = "";
                            hisData.Status = STATUS_RECOMMENDATION.RECEIVE_APPROVED;
                            hisData.CreatedBy = request.UserId;
                            hisData.CreatedDate = DateTime.Now;
                            await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);

                            // đã chuyển đến xxx

                            hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(unitSend)).FirstOrDefault().Name;
                            hisData.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                            await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                            await SYNotificationInsertTypeRecommendation(Id);
                        }

                    }
                }
                if (unitId != 0) {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                }
                return new ResultApi { Success = ResultCode.OK, Result = Id };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// cập nhập pakn
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("update")]
        public async Task<ActionResult<object>> RecommendationUpdate()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };



                RecommendationUpdateRequest request = new RecommendationUpdateRequest();
                request.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                request.UserType = new LogHelper(_appSetting).GetTypeFromRequest(HttpContext);
                request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                request.Data = JsonConvert.DeserializeObject<MRRecommendationUpdateIN>(Request.Form["Data"].ToString(), jss);

                var ErrorMessage = ValidationForFormData.validObject(request.Data);
                if (ErrorMessage != null)
                {
                    return StatusCode(400, new ResultApi
                    {
                        Success = ResultCode.ORROR,
                        Result = 0,
                        Message = ErrorMessage
                    });
                }
                SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();

                var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                var unitSend = request.Data.UnitId;
                if (unitId == 0)
                {
                    // người dân, doanh nghiệp gửi
                    if (request.Data.UnitId == null)
                    {
                        //var syUnitByField = await new SYUnitGetByField(_appSetting).SYUnitGetByFieldDAO(request.Data.Field);
                        //if (syUnitByField.Count == 0)
                        //{
                        //    request.Data.UnitId = dataMain.Id;
                        //}
                        //else
                        //{
                        //    request.Data.UnitId = syUnitByField.FirstOrDefault().Id;
                        //}
                        request.Data.UnitId = dataMain.Id;
                    }
                    if (request.Data.Status > 1 && dataMain != null && dataMain.Id != request.Data.UnitId && request.UserType != 1)
                    {
                        request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                    }
                }
                else
                {
                    // quản trị gửi
                    var unitInfo = new SYUnit(_appSetting).SYUnitGetByID(unitId);
                    if (request.Data.Status > 1)
                    {
                        request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                        request.Data.UnitId = unitId;
                    }
                }
                

                request.LstXoaFile = JsonConvert.DeserializeObject<List<MRRecommendationFiles>>(Request.Form["LstXoaFile"].ToString(), jss);
                request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
                request.Files = Request.Form.Files;
                request.Data.UpdatedBy = request.UserId;
                request.Data.UpdatedDate = DateTime.Now;
                var oldRecommendation = await new RecommendationDAO(_appSetting).RecommendationGetByID(request.Data.Id);
                

                if (request.Data.Status > 1 && dataMain != null && dataMain.Id != request.Data.UnitId && request.UserType != 1) //
                {
                    request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                }

                await new MRRecommendationUpdate(_appSetting).MRRecommendationUpdateDAO(request.Data);

                if (request.Data.Status > 1 && oldRecommendation.Model.Status == 1)
                {
                    MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN = new MRRecommendationForwardInsertIN();

                    _mRRecommendationForwardInsertIN.RecommendationId = request.Data.Id;
                    _mRRecommendationForwardInsertIN.UserSendId = request.UserId;
                    _mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
                    if (request.UserType != 1)
                    {
                        if (dataMain != null && dataMain.Id != request.Data.UnitId)
                        {
                            _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                        }
                        else
                        {
                            _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                        }
                        _mRRecommendationForwardInsertIN.UnitReceiveId = request.Data.UnitId;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                        _mRRecommendationForwardInsertIN.IsViewed = false;
                    }
                    else
                    {
                        _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                        _mRRecommendationForwardInsertIN.UnitReceiveId = unitSend;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.APPROVED;
                        _mRRecommendationForwardInsertIN.UserSendId = request.UserId;
                        _mRRecommendationForwardInsertIN.ProcessingDate = DateTime.Now;
                        _mRRecommendationForwardInsertIN.IsViewed = true;
                        _mRRecommendationForwardInsertIN.UnitSendId = request.Data.UnitId != null ? request.Data.UnitId : dataMain.Id;
                    }
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);
                }

                if (request.LstXoaFile.Count > 0)
                {

                    Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                    string webRootPath = _hostingEnvironment.ContentRootPath;

                    if (string.IsNullOrWhiteSpace(webRootPath))
                    {
                        webRootPath = Path.Combine(Directory.GetCurrentDirectory());
                    }

                    foreach (var item in request.LstXoaFile)
                    {
                        string filePath = Path.Combine(webRootPath, decrypt.DecryptData(item.FilePath));

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        MRRecommendationFilesDeleteIN fileDel = new MRRecommendationFilesDeleteIN();
                        fileDel.Id = item.Id;
                        await new MRRecommendationFilesDelete(_appSetting).MRRecommendationFilesDeleteDAO(fileDel);
                        MRRecommendationFullTextDeleteByRecommendationIdIN fulltextDel = new MRRecommendationFullTextDeleteByRecommendationIdIN();
                        fulltextDel.RecommendationId = item.RecommendationId;
                        fulltextDel.FileId = item.Id;
                        await new MRRecommendationFullTextDeleteByRecommendationId(_appSetting).MRRecommendationFullTextDeleteByRecommendationIdDAO(fulltextDel);
                    }

                }
                if (request.Files != null && request.Files.Count > 0)
                {
                    string folder = "Upload\\Recommendation\\" + request.Data.Id;
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    foreach (var item in request.Files)
                    {
                        MRRecommendationFilesInsertIN file = new MRRecommendationFilesInsertIN();
                        file.RecommendationId = request.Data.Id;
                        file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                        string filePath = Path.Combine(folderPath, file.Name);
                        file.FilePath = Path.Combine(folder, file.Name);
                        file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                        int FileId = Int32.Parse((await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(file)).ToString());
                        string contentType = "";
                        string content = "";
                        contentType = FileContentType.GetTypeOfFile(filePath);
                        bool isHasFullText = false;
                        Thread t = new Thread(async () => {
                            switch (contentType)
                            {
                                case ".pdf":
                                    //content = FileUtils.ExtractDataFromPDFFile(filePath);
                                    //content = PdfTextExtractorCustom.ReadPdfFile(filePath);
                                    content = PdfTextExtractorCustom.PerformOCR(filePath, _hostingEnvironment);
                                    isHasFullText = true;
                                    break;

                                case ".docx":
                                    content = FileUtils.ReadFileDocExtension(filePath);
                                    isHasFullText = true;
                                    break;

                                case ".doc":
                                    content = FileUtils.ExtractDocFile(filePath);
                                    isHasFullText = true;
                                    break;
                            }
                            if (isHasFullText)
                            {
                                MRRecommendationFullTextInsertIN fulltext = new MRRecommendationFullTextInsertIN();
                                fulltext.RecommendationId = request.Data.Id;
                                fulltext.FileId = FileId;
                                fulltext.FullText = content;
                                await new MRRecommendationFullTextInsert(_appSetting).MRRecommendationFullTextInsertDAO(fulltext);
                            }
                        });
                        t.Start();
                    }
                }
                MRRecommendationHashtagDeleteByRecommendationIdIN hashtagDeleteByRecommendationIdIN = new MRRecommendationHashtagDeleteByRecommendationIdIN();
                hashtagDeleteByRecommendationIdIN.Id = request.Data.Id;
                await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(hashtagDeleteByRecommendationIdIN);
                MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                foreach (var item in request.ListHashTag)
                {
                    _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                    _mRRecommendationHashtagInsertIN.RecommendationId = request.Data.Id;
                    _mRRecommendationHashtagInsertIN.HashtagId = item.Value;
                    _mRRecommendationHashtagInsertIN.HashtagName = item.Text;
                    await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
                }

                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = request.Data.Id;
                hisData.Type = 1;
                hisData.Content = "";
                hisData.Status = STATUS_RECOMMENDATION.UPDATED;
                hisData.CreatedBy = request.UserId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                if (request.UserType != 1 && request.Data.Status != STATUS_RECOMMENDATION.CREATED)
                {
                    hisData = new HISRecommendationInsertIN();
                    hisData.ObjectId = request.Data.Id;
                    hisData.Type = 1;
                    hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(request.Data.UnitId)).FirstOrDefault().Name;
                    hisData.Status = request.Data.Status;
                    hisData.CreatedBy = request.UserId;
                    hisData.CreatedDate = DateTime.Now;
                    await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                    await SYNotificationInsertTypeRecommendation(request.Data.Id);
                }
                else {
                    if (request.Data.Status != STATUS_RECOMMENDATION.CREATED)
                    {
                        hisData = new HISRecommendationInsertIN();
                        hisData.ObjectId = request.Data.Id;
                        hisData.Type = 1;
                        hisData.Content = "";
                        hisData.Status = STATUS_RECOMMENDATION.RECEIVE_APPROVED;
                        hisData.CreatedBy = request.UserId;
                        hisData.CreatedDate = DateTime.Now;
                        await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);

                        hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(unitSend)).FirstOrDefault().Name;
                        hisData.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                        await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);

                        await SYNotificationInsertTypeRecommendation(request.Data.Id);
                    }
                }
                if (unitId != 0)
                {
                    new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                }
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// chuyển tiếp pakn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("recommendation-forward")]
        public async Task<ActionResult<object>> RecommendationForward(RecommendationForwardRequest request)
        {
            try
            {
                //await new MRRecommendationDeleteByStep(_appSetting).MRRecommendationDeleteByStepDAO(request._mRRecommendationForwardInsertIN.RecommendationId, request._mRRecommendationForwardInsertIN.Step);

                request._mRRecommendationForwardInsertIN.UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                request._mRRecommendationForwardInsertIN.UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request._mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
                await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(request._mRRecommendationForwardInsertIN);

                if (!request.IsList)
                {
                    MRRecommendationHashtagDeleteByRecommendationIdIN hashtagDeleteByRecommendationIdIN = new MRRecommendationHashtagDeleteByRecommendationIdIN();
                    hashtagDeleteByRecommendationIdIN.Id = request._mRRecommendationForwardInsertIN.RecommendationId;
                    await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(hashtagDeleteByRecommendationIdIN);
                    MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                    foreach (var item in request.ListHashTag)
                    {
                        _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                        _mRRecommendationHashtagInsertIN.RecommendationId = request._mRRecommendationForwardInsertIN.RecommendationId;
                        _mRRecommendationHashtagInsertIN.HashtagId = item.Value;
                        _mRRecommendationHashtagInsertIN.HashtagName = item.Text;
                        await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
                    }
                }

                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                _mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardInsertIN.RecommendationId;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = request._mRRecommendationForwardInsertIN.RecommendationId;
                hisData.Type = 1;
                hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(request._mRRecommendationForwardInsertIN.UnitReceiveId)).FirstOrDefault().Name; ;
                hisData.Status = request.RecommendationStatus;
                hisData.CreatedBy = request._mRRecommendationForwardInsertIN.UserSendId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                await SYNotificationInsertTypeRecommendation(request._mRRecommendationForwardInsertIN.RecommendationId);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("recommendation-on-process")]
        public async Task<ActionResult<object>> MRRecommendationOnProcess(RecommendationForwardProcess request)
        {
            try
            {
                long UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                int UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request._mRRecommendationForwardProcessIN.ProcessingDate = DateTime.Now;
                request._mRRecommendationForwardProcessIN.UserId = UserSendId;
                if (request.IsForwardUnitChild == true)
                {
                    await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardUpdateStatusForwardDAO(request._mRRecommendationForwardProcessIN.RecommendationId);
                }
                if (request.RecommendationStatus == STATUS_RECOMMENDATION.FINISED) {
                    request._mRRecommendationForwardProcessIN.UnitReceiveId = UnitSendId;
                }
                await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(request._mRRecommendationForwardProcessIN);
                // được chuyển tiếp từ đơn vị cấp trên
                
                if (request.ReactionaryWord == true)
                {
                    MRRecommendationUpdateReactionaryWordIN _mRRecommendationUpdateReactionaryWordIN = new MRRecommendationUpdateReactionaryWordIN();
                    _mRRecommendationUpdateReactionaryWordIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
                    _mRRecommendationUpdateReactionaryWordIN.ReactionaryWord = request.ReactionaryWord;
                    await new MRRecommendationUpdateReactionaryWord(_appSetting).MRRecommendationUpdateReactionaryWordDAO(_mRRecommendationUpdateReactionaryWordIN);
                    MRRecommendationGroupWordInsertByListIN _mRRecommendationGroupWordInsertByListIN = new MRRecommendationGroupWordInsertByListIN();
                    _mRRecommendationGroupWordInsertByListIN.RecommendationId = request._mRRecommendationForwardProcessIN.RecommendationId;
                    _mRRecommendationGroupWordInsertByListIN.lstid = request.ListGroupWordSelected;
                    _mRRecommendationGroupWordInsertByListIN.UnitId = UnitSendId;
                    await new MRRecommendationGroupWordInsertByList(_appSetting).MRRecommendationGroupWordInsertByListDAO(_mRRecommendationGroupWordInsertByListIN);
                }

                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                _mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
                _mRRecommendationUpdateStatusIN.IsFakeImage = _mRRecommendationUpdateStatusIN.IsFakeImage == null ? false : request.IsFakeImage;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);

                if (!request.IsList)
                {
                    MRRecommendationHashtagDeleteByRecommendationIdIN hashtagDeleteByRecommendationIdIN = new MRRecommendationHashtagDeleteByRecommendationIdIN();
                    hashtagDeleteByRecommendationIdIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
                    await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(hashtagDeleteByRecommendationIdIN);
                    MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                    foreach (var item in request.ListHashTag)
                    {
                        _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                        _mRRecommendationHashtagInsertIN.RecommendationId = request._mRRecommendationForwardProcessIN.RecommendationId;
                        _mRRecommendationHashtagInsertIN.HashtagId = item.Value;
                        _mRRecommendationHashtagInsertIN.HashtagName = item.Text;
                        await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
                    }
                }
                // từ chối và của đơn vị và chuyển về trung tâm
                MRRecommendationForwardInsertIN _dataForward = new MRRecommendationForwardInsertIN();
                _dataForward.RecommendationId = request._mRRecommendationForwardProcessIN.RecommendationId;
                _dataForward.UserSendId = UserSendId;
                _dataForward.SendDate = DateTime.Now;
                if (request.RecommendationStatus == STATUS_RECOMMENDATION.PROCESS_DENY && request.IsForwardMain == true) {

                    SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                    
                    _dataForward.UnitReceiveId = dataMain.Id;
                    _dataForward.Step = STEP_RECOMMENDATION.RECEIVE;
                    _dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                    _mRRecommendationUpdateStatusIN.Status = STATUS_RECOMMENDATION.RECEIVE_WAIT;
                    _dataForward.IsViewed = false;
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_dataForward);
                    _mRRecommendationUpdateStatusIN.IsFakeImage = _mRRecommendationUpdateStatusIN.IsFakeImage == null ? false : request.IsFakeImage;
                    await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
                }

                if(request.IsForwardProcess == true 
                    && request._mRRecommendationForwardProcessIN.Status == PROCESS_STATUS_RECOMMENDATION.FORWARD 
                    && request._mRRecommendationForwardProcessIN.Step == STEP_RECOMMENDATION.FORWARD)
                {
                    var unit = (await new CAUnitGetByID(_appSetting).CAUnitGetByIDDAO(request._mRRecommendationForwardProcessIN.UnitReceiveId)).FirstOrDefault();
                    
                    _dataForward.UnitReceiveId = request._mRRecommendationForwardProcessIN.UnitReceiveId;
                    if (unit.IsMain)
                    {
                        // chờ xl với trung tâm
                        _dataForward.Step = STEP_RECOMMENDATION.RECEIVE;
                        _dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                        _mRRecommendationUpdateStatusIN.Status = STATUS_RECOMMENDATION.RECEIVE_WAIT;
                    }
                    else {
                        // chờ giải quyết với đơn vị khác
                        _dataForward.Step = STEP_RECOMMENDATION.PROCESS;
                        _dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                        _mRRecommendationUpdateStatusIN.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                    }
                    _dataForward.IsViewed = false;
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_dataForward);
                    _mRRecommendationUpdateStatusIN.IsFakeImage = _mRRecommendationUpdateStatusIN.IsFakeImage == null ? false : request.IsFakeImage;
                    await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);

                }

                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = request._mRRecommendationForwardProcessIN.RecommendationId;
                hisData.Type = 1;
                hisData.Content = "";
                if (request.RecommendationStatus == STATUS_RECOMMENDATION.APPROVE_DENY || request.RecommendationStatus == STATUS_RECOMMENDATION.PROCESS_DENY || request.RecommendationStatus == STATUS_RECOMMENDATION.RECEIVE_DENY)
                {
                    hisData.Content = "Với lý do: " + request._mRRecommendationForwardProcessIN.ReasonDeny;
                }
                else if (request.IsForwardProcess == true
                  && request._mRRecommendationForwardProcessIN.Status == PROCESS_STATUS_RECOMMENDATION.FORWARD
                  && request._mRRecommendationForwardProcessIN.Step == STEP_RECOMMENDATION.FORWARD
                  && request._mRRecommendationForwardProcessIN.ReasonDeny != "")
                {
                    var unit = await new SYUnit(_appSetting).SYUnitGetByID(_dataForward.UnitReceiveId);
                    hisData.Content = "Đến:" + unit.Name + " <br/ > "+
                        "Với nội dung: " + request._mRRecommendationForwardProcessIN.ReasonDeny;
                }
                hisData.Status = request.RecommendationStatus;
                hisData.CreatedBy = UserSendId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                await SYNotificationInsertTypeRecommendation(request._mRRecommendationForwardProcessIN.RecommendationId);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// giải quyết pakn
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("recommendation-on-process-conclusion")]
        public async Task<ActionResult<object>> RecommendationOnProcessConclusion()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                RecommendationOnProcessConclusionProcess request = new RecommendationOnProcessConclusionProcess();
                request.DataConclusion = JsonConvert.DeserializeObject<MRRecommendationConclusionInsertIN>(Request.Form["DataConclusion"].ToString(), jss);
                request.RecommendationStatus = JsonConvert.DeserializeObject<byte>(Request.Form["RecommendationStatus"].ToString(), jss);
                request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
                request.Files = Request.Form.Files;
                long UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                int UnitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request.DataConclusion.UserCreatedId = UserId;
                request.DataConclusion.UnitCreatedId = UnitId;
                int? IdConclusion = Int32.Parse((await new MRRecommendationConclusionInsert(_appSetting).MRRecommendationConclusionInsertDAO(request.DataConclusion)).ToString());

                if (request.Files != null && request.Files.Count > 0)
                {
                    string folder = "Upload\\Recommendation\\Conclusion\\" + IdConclusion;
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    foreach (var item in request.Files)
                    {
                        MRRecommendationConclusionFilesInsertIN file = new MRRecommendationConclusionFilesInsertIN();
                        file.ConclusionId = IdConclusion;
                        file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                        string filePath = Path.Combine(folderPath, file.Name);
                        file.FilePath = Path.Combine(folder, file.Name);
                        file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                        }
                        await new MRRecommendationConclusionFilesInsert(_appSetting).MRRecommendationConclusionFilesInsertDAO(file);
                    }
                }

                await new MRRecommendationDeleteByStep(_appSetting).MRRecommendationDeleteByStepDAO(request.DataConclusion.RecommendationId, STEP_RECOMMENDATION.APPROVE);
                MRRecommendationForwardInsertIN dataForward = new MRRecommendationForwardInsertIN();
                dataForward.RecommendationId = request.DataConclusion.RecommendationId;
                dataForward.UserSendId = UserId;
                dataForward.UnitSendId = UnitId;
                dataForward.ReceiveId = request.DataConclusion.ReceiverId;
                dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                dataForward.Step = STEP_RECOMMENDATION.APPROVE;
                dataForward.SendDate = DateTime.Now;
                dataForward.IsViewed = false;
                await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(dataForward);

                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                _mRRecommendationUpdateStatusIN.Id = request.DataConclusion.RecommendationId;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);


                MRRecommendationHashtagDeleteByRecommendationIdIN hashtagDeleteByRecommendationIdIN = new MRRecommendationHashtagDeleteByRecommendationIdIN();
                hashtagDeleteByRecommendationIdIN.Id = request.DataConclusion.RecommendationId;
                await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(hashtagDeleteByRecommendationIdIN);
                MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                foreach (var item in request.ListHashTag)
                {
                    _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                    _mRRecommendationHashtagInsertIN.RecommendationId = request.DataConclusion.RecommendationId;
                    _mRRecommendationHashtagInsertIN.HashtagId = item.Value;
                    _mRRecommendationHashtagInsertIN.HashtagName = item.Text;
                    await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
                }

                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = request.DataConclusion.RecommendationId;
                hisData.Type = 1;
                hisData.Content = "Đến: " + (await new SYUserGetNameById(_appSetting).SYUserGetNameByIdDAO(request.DataConclusion.ReceiverId)).FirstOrDefault().FullName;
                hisData.Status = request.RecommendationStatus;
                hisData.CreatedBy = UserId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                await SYNotificationInsertTypeRecommendation (request.DataConclusion.RecommendationId);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// cập nhập trạng thái pakn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("recommendation-update-status")]
        public async Task<ActionResult<object>> RecommendationUpdateStatus(RecommendationSendProcess request)
        {
            try
            {
                var oldRecommendation = await new RecommendationDAO(_appSetting).RecommendationGetByID(request.id);
                SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                if (oldRecommendation.Model.Status == 1 && request.status == 5) {
                    // insert forwa
                    MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN = new MRRecommendationForwardInsertIN();
                    var userInfo = await new SYUser(_appSetting).SYUserGetByID(oldRecommendation.Model.SendId);

                    _mRRecommendationForwardInsertIN.RecommendationId = request.id;
                    _mRRecommendationForwardInsertIN.UserSendId = oldRecommendation.Model.SendId;
                    _mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
                    if (userInfo.TypeId != 1)
                    {
                        _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                        _mRRecommendationForwardInsertIN.UnitReceiveId = oldRecommendation.Model.UnitId;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                        _mRRecommendationForwardInsertIN.IsViewed = false;
                    }
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);

                    // insert his
                    var hisData = new HISRecommendationInsertIN();
                    hisData.ObjectId = oldRecommendation.Model.Id;
                    hisData.Type = 1;
                    hisData.CreatedBy = oldRecommendation.Model.CreatedBy;
                    hisData.CreatedDate = DateTime.Now;
                    hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(oldRecommendation.Model.UnitId)).FirstOrDefault().Name;
                    hisData.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                    await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                }
                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Status = request.status;
                _mRRecommendationUpdateStatusIN.Id = request.id;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
                await SYNotificationInsertTypeRecommendation(request.id);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// danh sách pakn chứa ảnh giả
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SendName"></param>
        /// <param name="Content"></param>
        /// <param name="UnitId"></param>
        /// <param name="Field"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-recommentdation-fake-image")]
        public async Task<ActionResult<object>> MRRecommendationFakeImage(string Code, string SendName, string Content, int? UnitId, int? Field, int? PageSize, int? PageIndex)
        {
            try
            {
                var unitProcess = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                var userProcess = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                List<MRRecommendationGetAllOnPage> mrRecommendationFakeImage = await new MRRecommendationGetAllOnPage(_appSetting).MRRecommendationFakeImageDAO(Code, SendName, Content, UnitId, Field, STATUS_RECOMMENDATION.RECEIVE_DENY, userProcess ,unitProcess , PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationFakeImage", mrRecommendationFakeImage},
                        {"TotalCount", mrRecommendationFakeImage != null && mrRecommendationFakeImage.Count > 0 ? mrRecommendationFakeImage[0].RowNumber : 0},
                        {"PageIndex", mrRecommendationFakeImage != null && mrRecommendationFakeImage.Count > 0 ? PageIndex : 0},
                        {"PageSize", mrRecommendationFakeImage != null && mrRecommendationFakeImage.Count > 0 ? PageSize : 0},

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
        /// danh sách pakn
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SendName"></param>
        /// <param name="Content"></param>
        /// <param name="UnitId"></param>
        /// <param name="Field"></param>
        /// <param name="Status"></param>
        /// <param name="UnitProcessId"></param>
        /// <param name="UserProcessId"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-recommentdation-process-on-page")]
        public async Task<ActionResult<object>> MRRecommendationGetAllWithProcessBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
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
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// danh sách pakn chứa từ cấm
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SendName"></param>
        /// <param name="Content"></param>
        /// <param name="UnitId"></param>
        /// <param name="Field"></param>
        /// <param name="Status"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-recommentdation-reactionary-word")]
        public async Task<ActionResult<object>> MRRecommendationGetAllReactionaryWordBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status,int? GroupWord, int? PageSize, int? PageIndex)
        {
            try
            {
                int? UnitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                long? UserProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                List<MRRecommendationGetAllReactionaryWord> rsMRRecommendationGetAllReactionaryWord = await new MRRecommendationGetAllReactionaryWord(_appSetting).MRRecommendationGetAllReactionaryWordDAO(Code, SendName, Content, UnitId, Field, Status, UnitProcessId, UserProcessId, GroupWord,  PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetAllReactionaryWord", rsMRRecommendationGetAllReactionaryWord},
                        {"TotalCount", rsMRRecommendationGetAllReactionaryWord != null && rsMRRecommendationGetAllReactionaryWord.Count > 0 ? rsMRRecommendationGetAllReactionaryWord[0].RowNumber : 0},
                        {"PageIndex", rsMRRecommendationGetAllReactionaryWord != null && rsMRRecommendationGetAllReactionaryWord.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsMRRecommendationGetAllReactionaryWord != null && rsMRRecommendationGetAllReactionaryWord.Count > 0 ? PageSize : 0},
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
        /// lịch sử pakn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-his-by-recommentdation")]
        public async Task<ActionResult<object>> HISRecommendationGetByObjectIdBase(int? Id)
        {
            try
            {
                List<HISRecommendationGetByObjectId> rsHISRecommendationGetByObjectId = await new HISRecommendationGetByObjectId(_appSetting).HISRecommendationGetByObjectIdDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"HISRecommendationGetByObjectId", rsHISRecommendationGetByObjectId},
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
        /// xóa pakn
        /// </summary>
        /// <param name="_mRRecommendationDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("delete")]
        public async Task<ActionResult<object>> MRRecommendationDeleteBase(MRRecommendationDeleteIN _mRRecommendationDeleteIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteDAO(_mRRecommendationDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách gợi ý pakn theo tiêu đề
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>


        [HttpGet]
        [Route("recommendation-get-suggest-create")]
        public async Task<ActionResult<object>> MRRecommendationGetSuggestCreateBase(string Title)
        {
            try
            {
                List<MRRecommendationGetSuggestCreate> rsMRRecommendationGetSuggestCreate = await new MRRecommendationGetSuggestCreate(_appSetting).MRRecommendationGetSuggestCreateDAO(Title);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetSuggestCreate", rsMRRecommendationGetSuggestCreate},
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
        /// danh sách pakn gợi ý theo hastag
        /// </summary>
        /// <param name="ListIdHashtag"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("recommendation-get-suggest-reply")]
        public async Task<ActionResult<object>> MRRecommendationGetSuggestReplyBase(string ListIdHashtag, int? PageSize, int? PageIndex)
        {
            try
            {
                List<MRRecommendationGetSuggestReply> rsMRRecommendationGetSuggestReply = await new MRRecommendationGetSuggestReply(_appSetting).MRRecommendationGetSuggestReplyDAO(ListIdHashtag, PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetSuggestReply", rsMRRecommendationGetSuggestReply},
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
        /// thống kê pakn
        /// </summary>
        /// <param name="UnitProcessId"></param>
        /// <param name="UserProcessId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("recommendation-get-data-graph")]
        public async Task<ActionResult<object>> MRRecommendationGetDataGraphBase(int? UnitProcessId, long? UserProcessId)
        {
            try
            {
                List<MRRecommendationGetDataGraph> rsMRRecommendationGetDataGraph = await new MRRecommendationGetDataGraph(_appSetting).MRRecommendationGetDataGraphDAO(UnitProcessId, UserProcessId);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetDataGraph", rsMRRecommendationGetDataGraph},
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
        /// thống kê pakn
        /// </summary>
        /// <param name="SendId"></param>
        /// <param name="SendDateFrom"></param>
        /// <param name="SendDateTo"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("recommendation-get-send-user-data-graph")]
        public async Task<ActionResult<object>> MRRecommendationGetSendUserDataGraphBase(long? SendId, DateTime? SendDateFrom, DateTime? SendDateTo)
        {
            try
            {
                List<MRRecommendationGetSendUserDataGraph> rsMRRecommendationGetSendUserDataGraph = await new MRRecommendationGetSendUserDataGraph(_appSetting).MRRecommendationGetSendUserDataGraphDAO(SendId, SendDateFrom, SendDateTo);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetSendUserDataGraph", rsMRRecommendationGetSendUserDataGraph},
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
        /// danh sách pakn theo hashtag
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SendName"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <param name="Status"></param>
        /// <param name="UnitId"></param>
        /// <param name="HashtagId"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-recommendation-by-hashtag-on-page")]
        public async Task<ActionResult<object>> MRRecommendationGetByHashtagAllOnPageBase(string Code, string SendName, string Title, string Content, int? Status, int? UnitId, int? HashtagId, int? PageSize, int? PageIndex)
        {
            try
            {
                var userId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                List<MRRecommendationGetByHashtagAllOnPage> rsMRRecommendationGetByHashtagAllOnPage = await new MRRecommendationGetByHashtagAllOnPage(_appSetting).MRRecommendationGetByHashtagAllOnPageDAO(Code, SendName, Title, Content, Status, userId, UnitId, HashtagId, PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetByHashtagAllOnPage", rsMRRecommendationGetByHashtagAllOnPage},
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
        /// lí do từ chối pakn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("recommendation-get-deny-contents")]
        public async Task<ActionResult<object>> MRRecommendationGetDenyContentsBase(int? Id)
        {
            try
            {
                List<MRRecommendationGetDenyContentsBase> rsHISRecommendationGetByObjectId = await new MRRecommendationGetDenyContentsBase(_appSetting).MRRecommendationGetDenyContentsBaseDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetDenyContentsBase", rsHISRecommendationGetByObjectId},
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
        /// thêm mới hashtag cho pakn
        /// </summary>
        /// <param name="_mRRecommendationHashtagInsertIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("insert-hashtag-for-recommentdation")]
        public async Task<ActionResult<object>> MRInsertHashtagForRecommentdation(MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN)
        {
            try
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// xóa hashtag cho pakn
        /// </summary>
        /// <param name="_mRRecommendationHashtagDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("delete-hashtag-for-recommentdation")]
        public async Task<ActionResult<object>> MRDeleteHashtagForRecommentdation(MRRecommendationHashtagDelete _mRRecommendationHashtagDeleteIN)
        {
            try
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagDelete(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// thêm bình luận theo pakn
        /// </summary>
        /// <param name="_mRCommnentInsertIN"></param>
        /// <returns></returns>


        [HttpPost]
        [Route("insert-comment")]
        public async Task<ActionResult<object>> MRCommentInsertBase(MRCommentInsertIN _mRCommnentInsertIN)
        {
            try
            {
                _mRCommnentInsertIN.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                _mRCommnentInsertIN.FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);

                var result = await new MRCommentInsert(_appSetting).MRCommnentInsertDAO(_mRCommnentInsertIN);

                if (result == -1)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Bạn không có quyền bình luận phản ánh kiến nghị này" };
                }
                // nếu người dân bình luận thì gửi thông báo cho đơn vị giải quyết
                if (_mRCommnentInsertIN.IsPublish == true && new LogHelper(_appSetting).GetTypeFromRequest(HttpContext) != 1)
                {
                    var recommendation = new RecommendationDAO(_appSetting).RecommendationGetByID((int)_mRCommnentInsertIN.RecommendationId).Result.Model;
                    
                    RecommendationForward lstRMForward =
                        (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId((int)_mRCommnentInsertIN.RecommendationId))
                        .FirstOrDefault(x => x.Step == STEP_RECOMMENDATION.APPROVE && x.Status == PROCESS_STATUS_RECOMMENDATION.APPROVED);
                    
                    if (lstRMForward != null) {

                        // danh sách người dùng
                        List<SYUserGetByUnitId> lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO((int)lstRMForward.UnitSendId);
                        var tasks = new List<Task>();
                        SYNotificationModel notification = new SYNotificationModel();
                        notification.SenderId = (long)_mRCommnentInsertIN.UserId;
                        notification.DataId = _mRCommnentInsertIN.RecommendationId;
                        notification.SendDate = DateTime.Now;
                        notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                        notification.TypeSend = STATUS_RECOMMENDATION.FINISED;
                        notification.IsViewed = true;
                        notification.IsReaded = true;
                        notification.ReceiveOrgId = lstRMForward.UnitSendId;
                        notification.Title = "BÌNH LUẬN PHẢN ÁNH KIẾN NGHỊ";
                        notification.Content = _mRCommnentInsertIN.FullName + " vừa bình luận PAKN số " + recommendation.Code;
                        foreach (var item in lstUser) {
                            notification.ReceiveId = item.Id;
                            tasks.Add( new SYNotification(_appSetting, _configuration).InsertNotification(notification));
                        }
                        Task.WaitAll(tasks.ToArray());
                    }
                }

                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK, Result =  result};
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// danh sách bình luận theo pakn
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecommendationId"></param>
        /// <param name="IsPublish"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get-all-comment")]
        public async Task<ActionResult<object>> MRCommentGetAllOnPageBase(int? PageSize, int? PageIndex, long? RecommendationId, bool IsPublish)
        {
            try
            {
                List<MRCommentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommentGetAllOnPage(_appSetting).MRCommentGetAllOnPageDAO(PageSize, PageIndex, RecommendationId, IsPublish);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRCommnentGetAllOnPage", rsMRCommnentGetAllOnPage},
                        {"TotalCount", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? rsMRCommnentGetAllOnPage[0].RowNumber : 0},
                        {"PageIndex", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? PageSize : 0},
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


        [HttpPost]
        [Route("update-status-comment")]
        public async Task<ActionResult<object>> MRCommentUpdateStatusBase(MRCommentUpdateIN _mRCommnentUpdateIN)
        {
            try
            {
                _mRCommnentUpdateIN.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);

                var result = await new MRCommentUpdateStatus(_appSetting).MRCommnentUpdateDAO(_mRCommnentUpdateIN);
                if (result == -1)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Bạn không có quyền công bố-thu hồi bình luận này" };
                }
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
        /// tk pakn 7 ngày qua
        /// </summary>
        /// <param name="UnitProcessId"></param>
        /// <param name="UserProcessId"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("recommendation7daygraph")]
        public async Task<ActionResult<object>> MRRecommendation7dayGraph(int? UnitProcessId,long? UserProcessId)
        {
            try
            {

                var ado = new MrRecommendationGetGraphBase(_appSetting);

                var res = await ado.Get7DayGraphData(UnitProcessId, UserProcessId);
                var res2 = await ado.GetGraphData(UnitProcessId, UserProcessId);

                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"data7day", res},
                        {"data", res2}
                    };
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        // private notification

        private async Task<bool> SYNotificationInsertTypeRecommendation(int? recommendationId)
        {
            try
            {
                // thông tin PAKN
                var recommendation = new RecommendationDAO(_appSetting).RecommendationGetByID(recommendationId).Result.Model;

                //thông tin người gửi PAKN
                SYUser sender = await new SYUser(_appSetting).SYUserGetByID(recommendation.SendId);

                // danh sách người thuộc đơn vị mà PAKN gửi đến
                List<SYUserGetByUnitId> lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO((int)recommendation.UnitId);

                // lưu trạng thái các kiểu
                List<RecommendationForward> lstRMForward = new List<RecommendationForward>();
                lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();

                int unitReceiveId, receiveId;

                // danh sách người dùng thuộc đơn vị mà PAKN gửi đến đơn vị đó
                List<SYUserGetByUnitId> listUserReceiveResolve = new List<SYUserGetByUnitId>();

                // thông tin người giải quyết
                SYUser approver = new SYUser();

                // thông tin đơn vị giải quyết
                SYUnit unitReceive = new SYUnit();

                // thông tin đơn vị tiếp nhận PAKN
                SYUnit unit = await new SYUnit(_appSetting).SYUnitGetByID(recommendation.UnitId);

                // lấy thông tin đơn vị người đăng nhập
                SYUserGetByID userInfo = (await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext))).FirstOrDefault();

                // obj thông báo
                SYNotificationModel notification = new SYNotificationModel();
                notification.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                if (sender.Id != userInfo.Id)
                {
                    notification.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                }
                notification.DataId = recommendation.Id;
                notification.SendDate = DateTime.Now;
                notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                notification.TypeSend = recommendation.Status;
                notification.IsViewed = true;
                notification.IsReaded = true;


                switch (recommendation.Status)
                {
                    case STATUS_RECOMMENDATION.RECEIVE_WAIT: //2 Chờ xử lý

                        foreach (var item in lstUser)
                        {

                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            notification.Title = "PAKN CHỜ XỬ LÝ";
                            notification.Content =
                                recommendation.SendId != item.Id ?
                                sender.FullName + " vừa gửi một PAKN" : "Bạn vừa tạo một PAKN";
                            // insert notification
                            await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        }

                        // người gửi PAKN

                        break;
                    case STATUS_RECOMMENDATION.RECEIVE_DENY: //3 Từ chối xử lý

                        //foreach (var item in lstUser)
                        //{
                        //    notification.ReceiveId = item.Id;
                        //    notification.ReceiveOrgId = item.UnitId;
                        //    notification.Title = "PAKN số " + recommendation.Code + " đã bị từ chối xử lý";
                        //    notification.Content =
                        //        recommendation.SendId != item.Id ?
                        //        sender.FullName + " vừa gửi một PAKN." : "Bạn vừa tạo một PAKN.";
                        //    // insert notification
                        //    await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        //}

                        // người gửi PAKN

                        notification.ReceiveId = sender.Id;
                        notification.Title = "PAKN BỊ TỪ CHỐI";
                        notification.Content = "Phản ánh kiến nghị số " + recommendation.Code + " của bạn đã bị từ chối tiếp nhận";
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.RECEIVE_APPROVED: //4 Đã tiếp nhận
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 1 && x.Status == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ TIẾP NHẬN";
                        notification.Content = "PAKN số " + recommendation.Code + " đã được đơn vị " + unitReceive.Name + " tiếp nhận giải quyết";
                        //foreach (var item in lstUser)
                        //{
                        //    notification.ReceiveId = item.Id;
                        //    notification.ReceiveOrgId = item.UnitId;
                        //    // insert notification
                        //    await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        //}

                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_WAIT: //5 Chờ giải quyết
                        var check = lstRMForward.Where(x => x.Step == 1).FirstOrDefault();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        listUserReceiveResolve = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(unitReceiveId);
                        notification.Title = "PAKN ĐANG CHỜ GIẢI QUYẾT";
                        if (check == null)
                        {
                            // người dân doanh nghiệp gửi luôn PAKN cho đơn vị đã xác định thì phải tạo thông báo của status trước đó
                            // status 4
                            //notification.Title = "PAKN ĐÃ TIẾP NHẬN";
                            //notification.Content = "PAKN số" + recommendation.Code + " đã được tiếp nhận giải quyết.";
                            //foreach (var item in lstUser)
                            //{
                            //    notification.ReceiveId = item.Id;
                            //    notification.ReceiveOrgId = item.UnitId;
                            //    // insert notification
                            //    await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            //}

                            //// người gửi PAKN
                            //notification.ReceiveId = sender.Id;
                            //await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            notification.Content = sender.FullName + " vừa gửi một PAKN";
                            foreach (var item in listUserReceiveResolve)
                            {
                                notification.ReceiveId = item.Id;
                                notification.ReceiveOrgId = item.UnitId;
                                // insert notification
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                            break;

                        }
                        
                        notification.Content = "PAKN số " + recommendation.Code + " yêu cầu giải quyết được gửi từ đơn vị " + unit.Name + " được gửi tới yêu cầu giải quyết";

                        foreach (var item in listUserReceiveResolve)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        }

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_DENY: //6 Từ chối giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();

                        if (lstRMForward.FirstOrDefault(x => x.Step == 2) == null)
                        {
                            unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 1).UnitReceiveId;
                        }
                        else
                        {
                            unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        }
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);
                        // gửi cho đơn vị tiếp nhận ban đầu
                        notification.Title = "PAKN BỊ TỪ CHỐI GIẢI QUYẾT";
                        notification.Content = "PAKN số " + recommendation.Code + " đã bị " + unitReceive.Name + " từ chối giải quyết";
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        }

                        // người gửi PAKN

                        //notification.Content = "PAKN của bạn đã bị " + unitReceive.Name + " từ chối giải quyết";
                        //notification.ReceiveId = sender.Id;
                        //notification.ReceiveOrgId = null;
                        //await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.PROCESSING: //7 Đang giải quyết

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        if (unitReceive.IsMain) {
                            foreach (var item in lstUser)
                            {
                                notification.ReceiveId = item.Id;
                                notification.ReceiveOrgId = item.UnitId;
                                notification.Title = "PAKN ĐANG GIẢI QUYẾT";
                                notification.Content = "PAKN số " + recommendation.Code + " đã được đơn vị " + unitReceive.Name + " giải quyết";
                                // insert notification
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                        }

                        break;
                    case STATUS_RECOMMENDATION.APPROVE_WAIT: //8 Chờ phê duyệt
                        // bạn có 1 PAKN chờ phê duyệt
                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        receiveId = lstRMForward.FirstOrDefault(x => x.Step == 3).ReceiveId;
                        approver = await new SYUser(_appSetting).SYUserGetByID(receiveId);

                        // gửi cho lãnh đạo
                        notification.Title = "PAKN CHỜ PHÊ DUYỆT";
                        notification.Content = "Bạn có PAKN số " + recommendation.Code + " chờ phê duyệt";
                        notification.ReceiveId = approver.Id;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.APPROVE_DENY: //9 Từ chối phê duyệt

                        lstRMForward = (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(recommendationId)).ToList();
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3 && x.Status == 3).UnitSendId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ BỊ TỪ CHỐI PHÊ DUYỆT";
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối kết quả giải quyết PAKN số" + recommendation.Code;
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        }
                        // gửi cho người tiếp nhận PAKN -chưa chắc là người giải quyết nhá

                        notification.ReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).ReceiveId;
                        notification.ReceiveOrgId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        //người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = null;
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối phê duyệt PAKN số " + recommendation.Code + " của bạn";
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.FINISED: //10 Đã giải quyết

                        notification.Title = "PAKN ĐÃ GIẢI QUYẾT XONG";
                        notification.Content = unitReceive.Name + " đã giải quyết PAKN số " + recommendation.Code;

                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId;
                            // insert notification
                            await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        }
                        


                        // cán bộ gửi yêu cầu phê duyệt

                        notification.ReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3 && x.Status == 2).UserSendId;
                        notification.ReceiveOrgId = lstRMForward.FirstOrDefault(x => x.Step == 3 && x.Status == 2).UnitSendId;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);


                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = null;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

}
