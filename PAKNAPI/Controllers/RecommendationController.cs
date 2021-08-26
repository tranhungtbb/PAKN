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

namespace PAKNAPI.Controller
{
    [Route("api/recommendation")]
    [ApiController]
    public class RecommendationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;
        public RecommendationController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
            _bugsnag = bugsnag;
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
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForCreate() };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// get data for chuyển tiếp pakn
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("get-data-for-forward")]
        public async Task<ActionResult<object>> RecommendationGetDataForForward()
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForForward() };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// get data for process
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
        [Route("get-data-for-process")]
        public async Task<ActionResult<object>> RecommendationGetDataForProcess(int? UnitId)
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForProcess(UnitId) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// chi tiết pakn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// chi tiết pakn màn view chi tiết
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
        [Route("get-detail-by-id")]
        public async Task<ActionResult<object>> RecommendationGetByIDView(int? Id)
        {
            try
            {
                RecommendationGetByIDViewResponse data = new RecommendationGetByIDViewResponse();
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetByIDView(Id) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// thêm mới pakn
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
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
                if (request.Data.UnitId == null) {
                    var syUnitByField = await new SYUnitGetByField(_appSetting).SYUnitGetByFieldDAO(request.Data.Field);
                    if (syUnitByField.Count == 0) {
                        request.Data.UnitId = dataMain.Id;
                    }
                    else {
                        request.Data.UnitId = syUnitByField.FirstOrDefault().Id;
                    }
                }
                //request.Data.UnitId = request.Data.UnitId != null ? request.Data.UnitId : dataMain.Id;
                request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
                request.Files = Request.Form.Files;
                request.Data.CreatedBy = request.UserId;
                request.Data.CreatedDate = DateTime.Now;
                MRRecommendationCheckExistedCode rsMRRecommendationCheckExistedCode = (await new MRRecommendationCheckExistedCode(_appSetting).MRRecommendationCheckExistedCodeDAO(request.Data.Code)).FirstOrDefault();
                if (rsMRRecommendationCheckExistedCode.Total > 0)
                {
                    request.Data.Code = await new MRRecommendationGenCodeGetCode(_appSetting).MRRecommendationGenCodeGetCodeDAO();
                }
                if(request.Data.Status > 1 && dataMain != null && dataMain.Id != request.Data.UnitId && request.UserType != 1) // 
                {
                    request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
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
                            if(dataMain!= null && dataMain.Id != request.Data.UnitId)
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
                            _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                            _mRRecommendationForwardInsertIN.UnitReceiveId = request.Data.UnitId != null ? request.Data.UnitId : dataMain.Id;
                            _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.APPROVED;
                            _mRRecommendationForwardInsertIN.ReceiveId = request.UserId;
                            _mRRecommendationForwardInsertIN.ProcessingDate = DateTime.Now;
                            _mRRecommendationForwardInsertIN.IsViewed = true;
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
                            switch (contentType)
                            {
                                case ".pdf":
                                    // không đọc được từ file scan
                                    //content = FileUtils.ExtractDataFromPDFFile(filePath);
                                    content = PdfTextExtractorCustom.ReadPdfFile(filePath);
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
                    }
                    else
                    {
                        hisData = new HISRecommendationInsertIN();
                        hisData.ObjectId = Id;
                        hisData.Type = 1;
                        hisData.Content = "";
                        hisData.Status = STATUS_RECOMMENDATION.RECEIVE_APPROVED;
                        hisData.CreatedBy = request.UserId;
                        hisData.CreatedDate = DateTime.Now;
                        await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                    }
                }
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK, Result = Id };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// cập nhập pakn
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
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
                SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                if (request.Data.UnitId == null)
                {
                    var syUnitByField = await new SYUnitGetByField(_appSetting).SYUnitGetByFieldDAO(request.Data.Field);
                    if (syUnitByField.Count == 0)
                    {
                        request.Data.UnitId = dataMain.Id;
                    }
                    else
                    {
                        request.Data.UnitId = syUnitByField.FirstOrDefault().Id;
                    }
                }

                request.LstXoaFile = JsonConvert.DeserializeObject<List<MRRecommendationFiles>>(Request.Form["LstXoaFile"].ToString(), jss);
                request.ListHashTag = JsonConvert.DeserializeObject<List<DropdownObject>>(Request.Form["Hashtags"].ToString(), jss);
                request.Files = Request.Form.Files;
                request.Data.UpdatedBy = request.UserId;
                request.Data.UpdatedDate = DateTime.Now;
                var oldRecommendation = await new RecommendationDAO(_appSetting).RecommendationGetByID(request.Data.Id);

                await new MRRecommendationUpdate(_appSetting).MRRecommendationUpdateDAO(request.Data);

                if (request.Data.Status > 1 && dataMain != null && dataMain.Id != request.Data.UnitId && request.UserType != 1) //
                {
                    request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                }
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
                        _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                        _mRRecommendationForwardInsertIN.UnitReceiveId = request.Data.UnitId != null ? request.Data.UnitId : dataMain.Id;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.APPROVED;
                        _mRRecommendationForwardInsertIN.ReceiveId = request.UserId;
                        _mRRecommendationForwardInsertIN.ProcessingDate = DateTime.Now;
                        _mRRecommendationForwardInsertIN.IsViewed = true;
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
                        switch (contentType)
                        {
                            case ".pdf":
                                //content = FileUtils.ExtractDataFromPDFFile(filePath);
                                content = PdfTextExtractorCustom.ReadPdfFile(filePath);
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
                }
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// chuyển tiếp pakn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpPost]
        [Authorize]
        [Route("recommendation-on-process")]
        public async Task<ActionResult<object>> MRRecommendationOnProcess(RecommendationForwardProcess request)
        {
            try
            {
                long UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                int UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request._mRRecommendationForwardProcessIN.ProcessingDate = DateTime.Now;
                request._mRRecommendationForwardProcessIN.UserId = UserSendId;
                await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(request._mRRecommendationForwardProcessIN);
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

                if(request.IsForwardProcess == true 
                    && request._mRRecommendationForwardProcessIN.Status == PROCESS_STATUS_RECOMMENDATION.FORWARD 
                    && request._mRRecommendationForwardProcessIN.Step == STEP_RECOMMENDATION.PROCESS)
                {
                    SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                    MRRecommendationForwardInsertIN _dataForward = new MRRecommendationForwardInsertIN();

                    _dataForward.RecommendationId = request._mRRecommendationForwardProcessIN.RecommendationId;
                    _dataForward.UserSendId = UserSendId;
                    _dataForward.SendDate = DateTime.Now;
                    _dataForward.Step = STEP_RECOMMENDATION.RECEIVE;
                    _dataForward.UnitReceiveId = dataMain.Id;
                    _dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                    _dataForward.IsViewed = false;
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_dataForward);
                }

                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = request._mRRecommendationForwardProcessIN.RecommendationId;
                hisData.Type = 1;
                hisData.Content = "";
                if (request.RecommendationStatus == STATUS_RECOMMENDATION.APPROVE_DENY || request.RecommendationStatus == STATUS_RECOMMENDATION.PROCESS_DENY || request.RecommendationStatus == STATUS_RECOMMENDATION.RECEIVE_DENY)
                {
                    hisData.Content = "Với lý do: " + request._mRRecommendationForwardProcessIN.ReasonDeny;
                } else if (request.IsForwardProcess == true
                    && request._mRRecommendationForwardProcessIN.Status == PROCESS_STATUS_RECOMMENDATION.FORWARD
                    && request._mRRecommendationForwardProcessIN.Step == STEP_RECOMMENDATION.PROCESS
                    && request._mRRecommendationForwardProcessIN.ReasonDeny != "")
                {
                    hisData.Content = "Với nội dung: " + request._mRRecommendationForwardProcessIN.ReasonDeny;
                }
                hisData.Status = request.RecommendationStatus;
                hisData.CreatedBy = UserSendId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// giải quyết pakn
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
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
                request.Files = Request.Form.Files; ;
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// cập nhập trạng thái pakn
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        [Route("recommendation-update-status")]
        public async Task<ActionResult<object>> RecommendationUpdateStatus(RecommendationSendProcess request)
        {
            try
            {
                var oldRecommendation = await new RecommendationDAO(_appSetting).RecommendationGetByID(request.id);
                SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                if (oldRecommendation.Model.Status == 1 && request.status == 2) {
                    // insert forwa
                    MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN = new MRRecommendationForwardInsertIN();
                    var userInfo = await new SYUser(_appSetting).SYUserGetByID(oldRecommendation.Model.SendId);

                    _mRRecommendationForwardInsertIN.RecommendationId = request.id;
                    _mRRecommendationForwardInsertIN.UserSendId = oldRecommendation.Model.SendId;
                    _mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
                    if (userInfo.TypeId != 1)
                    {
                        if (dataMain != null && dataMain.Id != oldRecommendation.Model.UnitId)
                        {
                            _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                        }
                        else
                        {
                            _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                        }
                        _mRRecommendationForwardInsertIN.UnitReceiveId = oldRecommendation.Model.UnitId;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                        _mRRecommendationForwardInsertIN.IsViewed = false;
                    }
                    else
                    {
                        _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                        _mRRecommendationForwardInsertIN.UnitReceiveId = oldRecommendation.Model.UnitId != null ? oldRecommendation.Model.UnitId : dataMain.Id;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.APPROVED;
                        //_mRRecommendationForwardInsertIN.ReceiveId = ;
                        _mRRecommendationForwardInsertIN.ProcessingDate = DateTime.Now;
                        _mRRecommendationForwardInsertIN.IsViewed = true;
                    }
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);
                }
                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Status = request.status;
                _mRRecommendationUpdateStatusIN.Id = request.id;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// danh sách pakn - không dùng
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
        [Authorize]
        [Route("get-list-recommentdation-on-page")]
        public async Task<ActionResult<object>> MRRecommendationGetAllOnPageBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex)
        {
            try
            {
                List<MRRecommendationGetAllOnPage> rsMRRecommendationGetAllOnPage = await new MRRecommendationGetAllOnPage(_appSetting).MRRecommendationGetAllOnPageDAO(Code, SendName, Content, UnitId, Field, Status, PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationGetAllOnPage", rsMRRecommendationGetAllOnPage},
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
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
        /// <param name="UnitProcessId"></param>
        /// <param name="UserProcessId"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-recommentdation-reactionary-word")]
        public async Task<ActionResult<object>> MRRecommendationGetAllReactionaryWordBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
        {
            try
            {
                List<MRRecommendationGetAllReactionaryWord> rsMRRecommendationGetAllReactionaryWord = await new MRRecommendationGetAllReactionaryWord(_appSetting).MRRecommendationGetAllReactionaryWordDAO(Code, SendName, Content, UnitId, Field, Status, UnitProcessId, UserProcessId, PageSize, PageIndex);
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// lịch sử pakn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// xóa pakn
        /// </summary>
        /// <param name="_mRRecommendationDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        [Route("delete")]
        public async Task<ActionResult<object>> MRRecommendationDeleteBase(MRRecommendationDeleteIN _mRRecommendationDeleteIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteDAO(_mRRecommendationDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
        [Authorize("ThePolicy")]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// lí do từ chối pakn
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// thêm mới hashtag cho pakn
        /// </summary>
        /// <param name="_mRRecommendationHashtagInsertIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        [Route("insert-hashtag-for-recommentdation")]
        public async Task<ActionResult<object>> MRInsertHashtagForRecommentdation(MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN)
        {
            try
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// xóa hashtag cho pakn
        /// </summary>
        /// <param name="_mRRecommendationHashtagDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        [Route("delete-hashtag-for-recommentdation")]
        public async Task<ActionResult<object>> MRDeleteHashtagForRecommentdation(MRRecommendationHashtagDelete _mRRecommendationHashtagDeleteIN)
        {
            try
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagDelete(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// thêm bình luận theo pakn
        /// </summary>
        /// <param name="_mRCommnentInsertIN"></param>
        /// <returns></returns>


        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("insert-commnent")]
        public async Task<ActionResult<object>> MRCommnentInsertBase(MRCommnentInsertIN _mRCommnentInsertIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK, Result = await new MRCommnentInsert(_appSetting).MRCommnentInsertDAO(_mRCommnentInsertIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
        [Authorize("ThePolicy")]
        [Route("get-all-commnent")]
        public async Task<ActionResult<object>> MRCommnentGetAllOnPageBase(int? PageSize, int? PageIndex, long? RecommendationId, bool IsPublish)
        {
            try
            {
                List<MRCommnentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommnentGetAllOnPage(_appSetting).MRCommnentGetAllOnPageDAO(PageSize, PageIndex, RecommendationId, IsPublish);
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
        [Authorize]
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
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

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
