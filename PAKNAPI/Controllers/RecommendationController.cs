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
using static PAKNAPI.Common.FakeImageDetection;
using System.Text;
using PAKNAPI.Services;
using NSwag.Annotations;
using PAKNAPI.Job;

namespace PAKNAPI.Controller
{
    [Route("api/recommendation")]
    [ApiController]
    [ValidateModel]
    [OpenApiTag("Phản ánh kiến nghị", Description = "Phản ánh kiến nghị")]
    public class RecommendationController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public static List<CaptchaObject> captChaCode = new List<CaptchaObject>();

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
                var unitId = 0;
                try
                {
                    unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                }
                catch (Exception e) {
                }
                
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForCreate(unitId) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// get data for search public
        /// </summary>
        /// <returns></returns>
        [HttpGet]

        [Route("get-data-for-search")]
        public async Task<ActionResult<object>> RecommendationGetDataForSearch()
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new RecommendationDAO(_appSetting).RecommendationGetDataForSearch() };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// get data for chuyển tiếp pakn - Authorize
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// get data for process - Authorize
        /// </summary>
        /// <param name="UnitId"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// chi tiết pakn - Authorize
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// chi tiết pakn màn view chi tiết - Authorize
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("get-detail-by-id")]
        public async Task<ActionResult<object>> RecommendationGetByIDView(int? Id)
        {
            try
            {
                var userProcessId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                var unitProcessId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                // check đơn vị có được gửi phối hợp hay không
                var check = await new SYUnit(_appSetting).SYUnitCheckCombine(Id, unitProcessId);
                if (check > 0)
                {
                    return new ResultApi {
                        Success = ResultCode.OK,
                        Result = await new RecommendationDAO(_appSetting).RecommendationCombineGetByIDView(Id, userProcessId, unitProcessId)
                    };
                }
                else {
                    return new ResultApi { 
                        Success = ResultCode.OK, 
                        Result = await new RecommendationDAO(_appSetting).RecommendationGetByIDView(Id, userProcessId, unitProcessId) 
                    };
                }
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// chi tiết pakn cho phản ánh kiến nghị của tôi - Authorize
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "ThePolicy")]
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


        /// <summary>
        /// api check ảnh giả cho app - Authorize
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy")]
        [Route("check-recommendation")]

        public async Task<ActionResult<object>> RecommendationCheckFakeImage([FromForm] RecommendationCheckCaptcha model)
        {
            try
            {
                TimeSpan time = TimeSpan.FromMilliseconds(model.Milliseconds);
                DateTime createdDAte = new DateTime(1970, 1, 1) + time;
                
                if (!new Captcha(_appSetting).ValidateCaptchaCode(model.Captcha, captChaCode, createdDAte))
                {
                    await new Captcha(_appSetting).DeleteCaptcha("", createdDAte);
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Vui lòng nhập lại mã xác thực" };
                }
                else
                {
                    await new Captcha(_appSetting).DeleteCaptcha(model.Captcha, createdDAte);

                    var json = System.IO.File.ReadAllText("fake-images.json");
                    var sampleData = JsonConvert.DeserializeObject<Dictionary<long, FakeResult>>(json);
                    var fakeImage = new FakeImageDetection(sampleData);

                    string folder = "Upload\\Recommendation\\Image";
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    bool isFakeImage = false;
                    var fakeImageResults = new List<List<FakeResult>>();
                    List<string> filesNameError = new List<string>();
                    foreach (var item in Request.Form.Files)
                    {
                        string fileName = Path.GetFileName(item.FileName).Replace("+", "");
                        string filePath = Path.Combine(folderPath, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            item.CopyTo(stream);
                            var s = new System.Drawing.Bitmap(stream);
                            var outResult = new List<FakeResult>();
                            if (fakeImage.IsFake(s, ref outResult))
                            {
                                isFakeImage = true;
                                filesNameError.Add(item.FileName);
                                fakeImageResults.Add(outResult);
                            }
                        }
                    }
                    if (isFakeImage)
                    {
                        return new ResultApi
                        {
                            Success = ResultCode.INCORRECT,
                            Message = "Ảnh " + string.Join(", ", filesNameError) + " đã qua chỉnh sửa",
                            Result = fakeImageResults
                        };
                    }
                    else
                    {
                        return new ResultApi { Success = ResultCode.OK };
                    }
                }
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
            finally {
                System.IO.DirectoryInfo di = new DirectoryInfo("Upload\\Recommendation\\Image");

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
        }


        /// <summary>
        /// thêm mới pakn - Authorize
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy")]
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
                request.Data.IsForwardChild = false;
                request.Data.SendDate = DateTime.Now;
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

                        // fake image
                        var json = System.IO.File.ReadAllText("fake-images.json");
                        var sampleData = JsonConvert.DeserializeObject<Dictionary<long, FakeResult>>(json);
                        var fakeImage = new FakeImageDetection(sampleData);

                        string folder = "Upload\\Recommendation\\" + Id;
                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        bool isFakeImage = false;
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

                            if (file.FileType == 4 && request.Data.ReceptionType != ReceptionType.App && fakeImage.IsFake(file.FilePath))
                            {
                                isFakeImage = true;
                            }

                            Thread t = new Thread(async () => {
                                switch (contentType)
                                {
                                    case ".pdf":
                                        // không đọc được từ file scan
                                        //content = FileUtils.ExtractDataFromPDFFile(filePath);
                                        content = PdfTextExtractorCustom.ReadPdfFile(filePath);
                                        if (string.IsNullOrEmpty(content)) {
                                            content = PdfTextExtractorCustom.PerformOCR(filePath, _hostingEnvironment);
                                        }
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
                        if (isFakeImage) {
                            MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                            _mRRecommendationUpdateStatusIN.Status = request.Data.Status;
                            _mRRecommendationUpdateStatusIN.Id = Id;
                            _mRRecommendationUpdateStatusIN.IsFakeImage = true;
                            await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
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
        /// thêm mới pakn by chat bot
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Route("insert-recommendation")]
        public async Task<ActionResult<object>> RecommendationInsertNotAuthorize()
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
                request.UserId = 0;
                request.UserType = 0;
                //request.UserFullName = ;
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
                //var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                var unitSend = request.Data.UnitId;
                if (request.Data.UnitId == null)
                {
                    request.Data.UnitId = dataMain.Id;
                }
                if (request.Data.Status > 1 && dataMain != null && dataMain.Id != request.Data.UnitId && request.UserType != 1)
                {
                    request.Data.Status = STATUS_RECOMMENDATION.PROCESS_WAIT;
                }
                request.Data.CreatedBy = request.UserId;
                request.Data.CreatedDate = DateTime.Now;
                request.Data.CreateByType = 2;
                request.Data.IsForwardChild = false;
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
                    
                    MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN = new MRRecommendationHashtagInsertIN();
                    

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
                return new ResultApi { Success = ResultCode.OK, Result = Id };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// cập nhập pakn - Authorize
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy")]
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
                request.Data.SendDate = DateTime.Now;
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
                    var json = System.IO.File.ReadAllText("fake-images.json");
                    var sampleData = JsonConvert.DeserializeObject<Dictionary<long, FakeResult>>(json);
                    var fakeImage = new FakeImageDetection(sampleData);

                    bool isFakeImage = false;
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
                        if (file.FileType == 4 && request.Data.ReceptionType != ReceptionType.App && fakeImage.IsFake(file.FilePath))
                        {
                            isFakeImage = true;
                        }
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
                    if (isFakeImage)
                    {
                        MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                        _mRRecommendationUpdateStatusIN.Status = request.Data.Status;
                        _mRRecommendationUpdateStatusIN.Id = request.Data.Id;
                        _mRRecommendationUpdateStatusIN.IsFakeImage = true;
                        await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
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
        /// chuyển tiếp pakn - Authorize
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
                if (request.IsForwardUnitChild == true)
                {
                    await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardUpdateStatusForwardDAO(request._mRRecommendationForwardInsertIN.RecommendationId);
                }
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

                MRRecommendationUpdateStatusByForwardIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusByForwardIN();
                _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                _mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardInsertIN.RecommendationId;
                _mRRecommendationUpdateStatusIN.IsForwardChild = request.IsForwardUnitChild == null ? false : request.IsForwardUnitChild;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusByForwardDAO(_mRRecommendationUpdateStatusIN);
                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = request._mRRecommendationForwardInsertIN.RecommendationId;
                hisData.Type = 1;
                hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(request._mRRecommendationForwardInsertIN.UnitReceiveId)).FirstOrDefault().Name;
                if (! String.IsNullOrEmpty(request._mRRecommendationForwardInsertIN.Content)) {
                    hisData.Content += " <br/ > Với nội dung: " + request._mRRecommendationForwardInsertIN.Content;
                }

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

        /// <summary>
        /// chuyển tiếp phối hợp giải quyết nhiều đơn vị - Authorize
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("recommendation-combine-insert")]
        public async Task<ActionResult<object>> RecommendationCombine(RecommendationCombineRequest request)
        {
            try
            {
                request.RecommendationCombination.UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                request.RecommendationCombination.UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request.RecommendationCombination.RecommendationId = request.RecommendationCombination.RecommendationId;
                request.RecommendationCombination.SendDate = DateTime.Now;

                if (request.ListUnit != null) {
                    foreach (var item in request.ListUnit) {
                        request.RecommendationCombination.UnitReceiveId = item;
                        await new MRRecommendationCombination(_appSetting).MRRecommendationCombinationInsertDAO(request.RecommendationCombination);
                    }
                }
                // update mr
                MRRecommendationUpdateStatusByCombine _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusByCombine();
                _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                _mRRecommendationUpdateStatusIN.Id = (int)request.RecommendationCombination.RecommendationId;
                _mRRecommendationUpdateStatusIN.IsCombine =  true;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusByCombineDAO(_mRRecommendationUpdateStatusIN);

                // update forward
                await new MRRecommendationForwardProcess(_appSetting)
                    .MRRecommendationForwardUpdateStatusCombineDAO((int)request.ProcessId, request.RecommendationCombination.RecommendationId, STEP_RECOMMENDATION.PROCESS, PROCESS_STATUS_RECOMMENDATION.APPROVED, DateTime.Now);


                HISRecommendationInsertIN hisData = new HISRecommendationInsertIN();
                hisData.ObjectId = (int)request.RecommendationCombination.RecommendationId;
                hisData.Type = 1;
                hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByListIdDAO(string.Join(", ", request.ListUnit))).FirstOrDefault().Name + " <br/ > " +
                        "Với nội dung: " + request.RecommendationCombination.Content;
                hisData.Status = STATUS_RECOMMENDATION.COMBINE;
                hisData.CreatedBy = request.RecommendationCombination.UserSendId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);

                // notificationpi

                await new Notification(_appSetting, HttpContext, _configuration).NotificationInsertForCombine((int)request.RecommendationCombination.RecommendationId, request.ListUnit);

                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// xử lý phản ánh kiến nghị - Authorize
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("recommendation-on-process")]
        public async Task<ActionResult<object>> MRRecommendationOnProcess(RecommendationForwardProcess request)
        {
            try
            {
                long UserSendId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                int UnitSendId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);

                // check pakn có trùng vị trí hay không
                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();

                if (request.LstRecommendaionSelected != null && request.LstRecommendaionSelected.Count > 0 &&
                    (request.RecommendationStatus == STATUS_RECOMMENDATION.RECEIVE_APPROVED || request.RecommendationStatus == STATUS_RECOMMENDATION.PROCESSING))
                {

                    var mr = await new MRRecommendation(_appSetting).MRRecommendationGetByID(request._mRRecommendationForwardProcessIN.RecommendationId);
                    // save vào table MR_Same
                    MRRecommendationSame same = new MRRecommendationSame();
                    same.RecommendationId = (long)request._mRRecommendationForwardProcessIN.RecommendationId;
                    same.UserId = mr.SendId;
                    for (int i = 0; i < request.LstRecommendaionSelected.Count; i++)
                    {
                        same.RecommendationSameId = request.LstRecommendaionSelected[i];
                        await new MRRecommendationSame(_appSetting).MRRecommendationSameInsertDAO(same);
                    }


                    // update lại Status pakn 14
                    request._mRRecommendationForwardProcessIN.ProcessingDate = DateTime.Now;
                    request._mRRecommendationForwardProcessIN.UserId = UserSendId;
                    await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(request._mRRecommendationForwardProcessIN);

                    _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                    _mRRecommendationUpdateStatusIN.Status = STATUS_RECOMMENDATION.RECEIVE_FINISED;
                    _mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
                    _mRRecommendationUpdateStatusIN.IsFakeImage = false;
                    _mRRecommendationUpdateStatusIN.Field = request.Field;
                    await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);

                    // thông báo cho người gửi pakn : pakn của bạn đã bị trùng

                    await new Notification(_appSetting, HttpContext, _configuration).NotificationInsertForReceiveFinisd(mr.Code, request._mRRecommendationForwardProcessIN.RecommendationId, mr.SendId, request.LstRecommendaionSelected);

                    return new ResultApi { Success = ResultCode.OK };
                }

                // check don vi xem co phai duoc gui den tham muu hay khong
                var check = await new SYUnit(_appSetting).SYUnitCheckCombine(request._mRRecommendationForwardProcessIN.RecommendationId, UnitSendId);
                if (check > 0)
                {

                    var rRecommendationCombinationUpdate = new MRRecommendationCombinationUpdate();
                    rRecommendationCombinationUpdate.RecommendationId = request._mRRecommendationForwardProcessIN.RecommendationId;
                    rRecommendationCombinationUpdate.UnitReceiveId = UnitSendId;
                    rRecommendationCombinationUpdate.ReceiveId = UserSendId;
                    rRecommendationCombinationUpdate.Status = request.RecommendationStatus;
                    rRecommendationCombinationUpdate.ReasonDeny = request._mRRecommendationForwardProcessIN.ReasonDeny;
                    rRecommendationCombinationUpdate.ProcessingDate = DateTime.Now;
                    if (request.RecommendationStatus == STATUS_RECOMMENDATION.APPROVE_DENY)
                    {
                        rRecommendationCombinationUpdate.DenyId = UserSendId;
                    }
                    await new MRRecommendationCombination(_appSetting).MRRecommendationCombinationUpdateDAO(rRecommendationCombinationUpdate);

                    // insert notification
                    await new Notification(_appSetting, HttpContext, _configuration).NotificationInsertForCombineProcess(request._mRRecommendationForwardProcessIN.RecommendationId, request.RecommendationStatus);

                    return new ResultApi { Success = ResultCode.OK };
                }

                request._mRRecommendationForwardProcessIN.ProcessingDate = DateTime.Now;
                request._mRRecommendationForwardProcessIN.UserId = UserSendId;
                if (request.IsForwardUnitChild == true)
                {
                    await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardUpdateStatusForwardDAO(request._mRRecommendationForwardProcessIN.RecommendationId);
                }
                if (request.RecommendationStatus == STATUS_RECOMMENDATION.FINISED)
                {
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

                _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                _mRRecommendationUpdateStatusIN.Id = request._mRRecommendationForwardProcessIN.RecommendationId;
                _mRRecommendationUpdateStatusIN.IsFakeImage = _mRRecommendationUpdateStatusIN.IsFakeImage == null ? false : request.IsFakeImage;
                _mRRecommendationUpdateStatusIN.Field = request.Field;
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
                if (request.RecommendationStatus == STATUS_RECOMMENDATION.RECEIVE_DENY && request.IsForwardMain == true)
                {

                    SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                    _dataForward.UnitSendId = UnitSendId;
                    _dataForward.UnitReceiveId = dataMain.Id;
                    _dataForward.Step = STEP_RECOMMENDATION.RECEIVE;
                    _dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                    _mRRecommendationUpdateStatusIN.Status = STATUS_RECOMMENDATION.RECEIVE_WAIT;
                    _dataForward.IsViewed = false;
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_dataForward);
                    _mRRecommendationUpdateStatusIN.IsFakeImage = _mRRecommendationUpdateStatusIN.IsFakeImage == null ? false : request.IsFakeImage;
                    await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
                }

                if (request.IsForwardProcess == true
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
                    else
                    {
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
                    hisData.Content = "Đến:" + unit.Name + " <br/ > " +
                        "Với nội dung: " + request._mRRecommendationForwardProcessIN.ReasonDeny;
                }
                hisData.Status = request.RecommendationStatus;
                hisData.CreatedBy = UserSendId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                await SYNotificationInsertTypeRecommendation(request._mRRecommendationForwardProcessIN.RecommendationId);


                // coppy ảnh giả vào 1 thư mục

                if (_mRRecommendationUpdateStatusIN.IsFakeImage == true) {
                    
                    string path = "Upload\\Recommendation\\" + request._mRRecommendationForwardProcessIN.RecommendationId;
                    string destFile = String.Empty;

                    if (System.IO.Directory.Exists(path))
                    {
                        string[] files = System.IO.Directory.GetFiles(path)
                            .Where(fn => Path.GetExtension(fn) == ".png" ||
                                Path.GetExtension(fn) == ".jpg" ||
                                Path.GetExtension(fn) == ".jpeg" ||
                                Path.GetExtension(fn) == ".gif")
                            .ToArray();
                        string folder = "Upload\\Recommendation\\FakeImage\\" + request._mRRecommendationForwardProcessIN.RecommendationId;
                        if (files.Length > 0) {
                            string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }
                        }
                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string fileName = System.IO.Path.GetFileName(s);
                            destFile = System.IO.Path.Combine(folder, fileName);
                            System.IO.File.Copy(s, destFile, true);
                        }
                    }
                }
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
        /// giải quyết pakn - Authorize
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
                request.FilesDelete = JsonConvert.DeserializeObject<List<MRRecommendationConclusionFilesGetByConclusionId>>(Request.Form["FileDelete"].ToString(), jss);
                request.Files = Request.Form.Files;
                long UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                int UnitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request.DataConclusion.UserCreatedId = UserId;
                request.DataConclusion.UnitCreatedId = UnitId;
                int? IdConclusion = Int32.Parse((await new MRRecommendationConclusionInsert(_appSetting).MRRecommendationConclusionInsertDAO(request.DataConclusion)).ToString());

                if (request.FilesDelete != null)
                {
                    foreach (var file in request.FilesDelete)
                    {
                        await new MRRecommendationConclusionFilesDelete(_appSetting).MRRecommendationConclusionFilesDeleteDAO(file.Id);

                    }
                }

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

                //await new MRRecommendationDeleteByStep(_appSetting).MRRecommendationDeleteByStepDAO(request.DataConclusion.RecommendationId, STEP_RECOMMENDATION.APPROVE);
                var forwards= await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId(request.DataConclusion.RecommendationId);
                var userApprove = forwards.Where(x => x.Step == STEP_RECOMMENDATION.APPROVE && x.Status == PROCESS_STATUS_RECOMMENDATION.WAIT).FirstOrDefault();

                MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusIN();
                _mRRecommendationUpdateStatusIN.Id = request.DataConclusion.RecommendationId;

                bool isUserApprove = false;
                if (userApprove != null && userApprove.ReceiveId == new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext))
                {
                    // update MRForward
                    isUserApprove = true;
                    _mRRecommendationUpdateStatusIN.Status = STATUS_RECOMMENDATION.FINISED;
                    await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardUpdateStatusApproveDAO(request.DataConclusion.RecommendationId);

                }
                else {
                    // insert
                    MRRecommendationForwardInsertIN dataForward = new MRRecommendationForwardInsertIN();
                    dataForward.RecommendationId = request.DataConclusion.RecommendationId;
                    dataForward.UserSendId = UserId;
                    dataForward.UnitSendId = UnitId;
                    dataForward.ReceiveId = request.DataConclusion.ReceiverId;
                    dataForward.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                    dataForward.Step = STEP_RECOMMENDATION.APPROVE;
                    dataForward.SendDate = DateTime.Now;
                    dataForward.IsViewed = false;
                    dataForward.ProcessingDate = DateTime.Now;
                    await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(dataForward);

                    _mRRecommendationUpdateStatusIN.Status = request.RecommendationStatus;
                }
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
                hisData.Content = isUserApprove == true ? "" : "Đến: " + (await new SYUserGetNameById(_appSetting).SYUserGetNameByIdDAO(request.DataConclusion.ReceiverId)).FirstOrDefault().FullName;
                hisData.Status = isUserApprove == true ? (byte)STATUS_RECOMMENDATION.FINISED : request.RecommendationStatus;
                hisData.CreatedBy = UserId;
                hisData.CreatedDate = DateTime.Now;
                await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);
                await SYNotificationInsertTypeRecommendation (request.DataConclusion.RecommendationId);


                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                return new ResultApi { Success = ResultCode.OK , Result = IdConclusion };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// giải quyết phản ánh kiến nghị cho đơn vị được phối hợp - Authorize
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("recommendation-on-process-conclusion-combine")]
        public async Task<ActionResult<object>> RecommendationOnUpdateProcessConclusion()
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
                request.FilesDelete = JsonConvert.DeserializeObject<List<MRRecommendationConclusionFilesGetByConclusionId>>(Request.Form["FileDelete"].ToString(), jss);
                request.Files = Request.Form.Files;
                long UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                int UnitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                request.DataConclusion.UserCreatedId = UserId;
                request.DataConclusion.UnitCreatedId = UnitId;
                int? IdConclusion = Int32.Parse((await new MRRecommendationConclusionInsert(_appSetting).MRRecommendationConclusionCombineInsertDAO(request.DataConclusion)).ToString());

                if (request.FilesDelete != null)
                {
                    foreach (var file in request.FilesDelete)
                    {
                        await new MRRecommendationConclusionFilesDelete(_appSetting).MRRecommendationConclusionFilesDeleteDAO(file.Id);

                    }
                }

                var rRecommendationCombinationUpdate = new MRRecommendationCombinationUpdate();
                rRecommendationCombinationUpdate.RecommendationId = request.DataConclusion.RecommendationId;
                rRecommendationCombinationUpdate.UnitReceiveId = UnitId;
                rRecommendationCombinationUpdate.ReceiveId = request.DataConclusion.ReceiverId;
                rRecommendationCombinationUpdate.Status = rRecommendationCombinationUpdate.ReceiveId != UserId ? 
                    (byte) STATUS_RECOMMENDATION.APPROVE_WAIT : (byte)STATUS_RECOMMENDATION.FINISED;
                rRecommendationCombinationUpdate.ReasonDeny = string.Empty;
                rRecommendationCombinationUpdate.ProcessingDate = DateTime.Now;

                await new MRRecommendationCombination(_appSetting).MRRecommendationCombinationUpdateDAO(rRecommendationCombinationUpdate);

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
                if (rRecommendationCombinationUpdate.ReceiveId != UserId)
                {
                    await new Notification(_appSetting, HttpContext, _configuration).NotificationInsertForCombineApprove(request.DataConclusion.RecommendationId, rRecommendationCombinationUpdate.ReceiveId);
                }
                else {
                    await new Notification(_appSetting, HttpContext, _configuration).NotificationInsertForCombineProcess(request.DataConclusion.RecommendationId, STATUS_RECOMMENDATION.FINISED);
                }
                return new ResultApi { Success = ResultCode.OK , Result = IdConclusion };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// cập nhập trạng thái pakn - cho người dân tại màn danh sách pakn của tôi - Authorize
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.OTHER)]
        [Route("recommendation-update-status")]
        public async Task<ActionResult<object>> RecommendationUpdateStatus(RecommendationSendProcess request)
        {
            try
            {
                var oldRecommendation = await new RecommendationDAO(_appSetting).RecommendationGetByID(request.id);
                SYUnitGetMainId dataMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                if (oldRecommendation.Model.UnitId == dataMain.Id) {
                    request.status = STATUS_RECOMMENDATION.RECEIVE_WAIT;
                }
                MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN = new MRRecommendationForwardInsertIN();
                var userInfo = await new SYUser(_appSetting).SYUserGetByID(oldRecommendation.Model.SendId);

                _mRRecommendationForwardInsertIN.RecommendationId = request.id;
                _mRRecommendationForwardInsertIN.UserSendId = oldRecommendation.Model.SendId;
                _mRRecommendationForwardInsertIN.SendDate = DateTime.Now;
                if (oldRecommendation.Model.Status == 1 && request.status == STATUS_RECOMMENDATION.PROCESS_WAIT)
                {
                    // insert forwa
                    if (userInfo.TypeId != 1)
                    {
                        _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.PROCESS;
                        _mRRecommendationForwardInsertIN.UnitReceiveId = oldRecommendation.Model.UnitId;
                        _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                        _mRRecommendationForwardInsertIN.IsViewed = false;
                    }
                    // insert his
                }
                else {
                    _mRRecommendationForwardInsertIN.Step = STEP_RECOMMENDATION.RECEIVE;
                    _mRRecommendationForwardInsertIN.UnitReceiveId = oldRecommendation.Model.UnitId;
                    _mRRecommendationForwardInsertIN.Status = PROCESS_STATUS_RECOMMENDATION.WAIT;
                    _mRRecommendationForwardInsertIN.IsViewed = false;
                }
                await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);

                var hisData = new HISRecommendationInsertIN();
                    hisData.ObjectId = oldRecommendation.Model.Id;
                    hisData.Type = 1;
                    hisData.CreatedBy = oldRecommendation.Model.CreatedBy;
                    hisData.CreatedDate = DateTime.Now;
                    hisData.Content = "Đến: " + (await new SYUnitGetNameById(_appSetting).SYUnitGetNameByIdDAO(oldRecommendation.Model.UnitId)).FirstOrDefault().Name;
                    hisData.Status = request.status;
                    await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(hisData);

                MRRecommendationUpdateStatusBySendIN _mRRecommendationUpdateStatusIN = new MRRecommendationUpdateStatusBySendIN();
                _mRRecommendationUpdateStatusIN.Status = request.status;
                _mRRecommendationUpdateStatusIN.Id = request.id;
                _mRRecommendationUpdateStatusIN.SendDate = DateTime.Now;
                await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusBySendDAO(_mRRecommendationUpdateStatusIN);
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
        /// danh sách pakn chứa ảnh giả - Authorize
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
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// danh sách pakn - Authorize
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
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// danh sach pakn ket hop xu ly - Authorize
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
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("get-list-recommentdation-combination-on-page")]
        public async Task<ActionResult<object>> MRRecommendationCombinationGetAllBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex)
        {
            try
            {
                var unitProcess = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                List<MRRecommendationGetAllWithProcess> rsMRRecommendationCombine = await new MRRecommendationGetAllWithProcess(_appSetting).MRRecommendationCombinationGetAllDAO(Code, SendName, Content, UnitId, Field, Status, unitProcess, PageSize, PageIndex);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRRecommendationCombine", rsMRRecommendationCombine},
                        {"TotalCount", rsMRRecommendationCombine != null && rsMRRecommendationCombine.Count > 0 ? rsMRRecommendationCombine[0].RowNumber : 0},
                        {"PageIndex", rsMRRecommendationCombine != null && rsMRRecommendationCombine.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsMRRecommendationCombine != null && rsMRRecommendationCombine.Count > 0 ? PageSize : 0},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách pakn chứa từ cấm - Authorize
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="SendName"></param>
        /// <param name="Content"></param>
        /// <param name="UnitId"></param>
        /// <param name="Field"></param>
        /// <param name="Status"></param>
        /// <param name="GroupWord"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// lịch sử pakn - Authorize
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// xóa pakn - Authorize
        /// </summary>
        /// <param name="_mRRecommendationDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// danh sách pakn gợi ý theo hastag - Authorize
        /// </summary>
        /// <param name="ListIdHashtag"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// danh sách pakn theo hashtag - Authorize
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
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// lí do từ chối pakn - Authorize
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// thêm mới hashtag cho pakn - Authorize
        /// </summary>
        /// <param name="_mRRecommendationHashtagInsertIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// xóa hashtag cho pakn - Authorize
        /// </summary>
        /// <param name="_mRRecommendationHashtagDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
                if (new LogHelper(_appSetting).GetTypeFromRequest(HttpContext) == 1) {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Tài khoản cán bộ không được bình luận" };
                }
                _mRCommnentInsertIN.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);

                var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_COMMENT)).FirstOrDefault();
                if (config != null && config.Content != null)
                {
                    int type = Int32.Parse(config.Content);
                    if (type == TYPECONFIGCOMMENT.PRIVATE) {
                        var mr = await new MRRecommendation(_appSetting).MRRecommendationGetByID((int)_mRCommnentInsertIN.RecommendationId);
                        if (mr.SendId != _mRCommnentInsertIN.UserId)
                        {
                            return new ResultApi { Success = ResultCode.ORROR, Message = "Bạn không được phép bình luận phản ánh kiến nghị này" };
                        }
                        else {
                            goto insert;
                        }
                    }
                    else
                    {
                        goto insert;
                    }
                }
                else {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Không thể bình luận phản ánh kiến nghị của người khác" };
                }


                insert:

                _mRCommnentInsertIN.FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);

                var result = await new MRCommentInsert(_appSetting).MRCommnentInsertDAO(_mRCommnentInsertIN);

                if (result == -1)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Lỗi khi thêm bình luận" };
                }
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
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
        /// danh sách trao đổi theo pakn - Authorize
        /// </summary>
        /// <param name="RecommendationId"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="IsPublish"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get-all-infomation-exchange")]
        [Authorize(Policy = "ThePolicy")]
        public async Task<ActionResult<object>> MRInfomationExchangeGetAllOnPageBase(long? RecommendationId,bool? IsPublish, int? PageIndex, int? PageSize)
        {
            try
            {
                List<MRInfomationExchange> _results = 
                    await new MRInfomationExchange(_appSetting).MRInfomationExchangeGetAllOnPage(RecommendationId, IsPublish, PageIndex, PageSize);

                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRInfomationExchangeAllOnPage", _results},
                        {"TotalCount", _results != null && _results.Count > 0 ? _results[0].RowNumber : 0}
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// thêm trao đổi theo pakn - Authorize
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>


        [HttpPost]
        [Route("insert-infomation-exchange")]
        [Authorize(Policy = "ThePolicy")]
        public async Task<ActionResult<object>> MRInfomationExchangeInsertBase(MRInfomationExchange model)
        {
            try
            {
                model.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                model.FullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                var unitId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);
                model.TypeObject = new LogHelper(_appSetting).GetTypeFromRequest(HttpContext);
                model.CreatedDate = DateTime.Now;

                var result = await new MRInfomationExchange(_appSetting).MRInfomationExchangeInsertDAO(model);

                if (result < 0)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Thêm trao đổi thất bại" };
                }
                var recommendation = (await new RecommendationDAO(_appSetting).RecommendationGetByID((int)model.RecommendationId)).Model;

                if (new LogHelper(_appSetting).GetTypeFromRequest(HttpContext) != 1)
                {
                    RecommendationForward lstRMForward =
                        (await new MR_RecommendationForward(_appSetting).MRRecommendationForwardGetByRecommendationId((int)model.RecommendationId))
                        .FirstOrDefault(x => x.Step == STEP_RECOMMENDATION.APPROVE);

                    if (lstRMForward != null)
                    {

                        // danh sách người dùng
                        List<SYUserGetByUnitId> lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO((int)lstRMForward.UnitSendId);
                        var tasks = new List<Task>();
                        SYNotificationModel notification = new SYNotificationModel();
                        notification.SenderId = (long)model.UserId;
                        notification.SendOrgId = unitId;
                        notification.DataId = model.RecommendationId;
                        notification.SendDate = DateTime.Now;
                        notification.Type = TYPENOTIFICATION.RECOMMENDATION;
                        notification.TypeSend = STATUS_RECOMMENDATION.FINISED;
                        notification.IsViewed = true;
                        notification.IsReaded = true;
                        notification.ReceiveOrgId = lstRMForward.UnitSendId;
                        notification.Title = "TRAO ĐỔI PHẢN ÁNH KIẾN NGHỊ";
                        notification.Content = model.FullName + " vừa thêm trao đổi của PAKN số " + recommendation.Code;
                        foreach (var item in lstUser)
                        {
                            notification.ReceiveId = item.Id;
                            tasks.Add(new SYNotification(_appSetting, _configuration).InsertNotification(notification));
                        }
                        Task.WaitAll(tasks.ToArray());
                    }
                }
                else
                {
                    // send Email

                    var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL)).FirstOrDefault();
                    if (config != null)
                    {
                        var user = await new SYUser(_appSetting).SYUserGetByID(recommendation.SendId);
                        if (!String.IsNullOrEmpty(user.Email)) {
                            var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config.Content);
                            string content = "<p> Cơ quan chức năng vừa thêm trao đổi với phản ánh kiến nghị của bạn! <br>" +
                            "<a href =\"https://tuongtac.khanhhoa.gov.vn/cong-bo/chi-tiet-kien-nghi/{id}\">Xem tại đây</a></p>";
                            content = content.Replace("{id}", recommendation.Id.ToString());
                            MailHelper.SendMail(configEmail, user.Email, "Trao đổi phản ánh kiến nghị " + recommendation.Title, content);
                        }
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
        /// danh sách bình luận quản trị - Authorize
        /// </summary>
        /// <param name="FullName"></param>
        /// <param name="Contents"></param>
        /// <param name="RecommendationTitle"></param>
        /// <param name="IsPublish"></param>
        /// <param name="CreatedDate"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("get-all-comment-on-page")]
        public async Task<ActionResult<object>> MRCommentGetAllOnPageBase(string FullName, string Contents , string RecommendationTitle, bool? IsPublish, DateTime? CreatedDate, int? PageSize, int? PageIndex)
        {
            try
            {
                List<MRCommentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommentGetAllOnPage(_appSetting).MRCommentGetAllOnPageDAO(FullName, Contents, RecommendationTitle, IsPublish, CreatedDate, PageSize, PageIndex);
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// get danh sách bình luận theo bình luận cấp cha (quản trị) - Authorize
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("get-all-comment-by-parent")]
        public async Task<ActionResult<object>> MRCommentGetByParent(int? ParentId)
        {
            try
            {
                List<MRCommentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommentGetAllOnPage(_appSetting).MRCommentGetAllByParentDAO(ParentId);
                
                return new ResultApi { Success = ResultCode.OK, Result = rsMRCommnentGetAllOnPage };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        /// <summary>
        /// danh sách bình luận theo pakn trang coong bo
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="RecommendationId"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get-all-comment")]
        public async Task<ActionResult<object>> MRCommentGetAllOnPageBase(int? PageSize, int? PageIndex, long? RecommendationId)
        {
            try
            {
                List<MRCommentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommentGetAllOnPage(_appSetting).MRCommentGetAllOnPageByRecommendationDAO(PageSize, PageIndex, RecommendationId, true);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"MRCommnentGetAllOnPage", rsMRCommnentGetAllOnPage},
                        {"Total", rsMRCommnentGetAllOnPage != null && rsMRCommnentGetAllOnPage.Count > 0 ? rsMRCommnentGetAllOnPage[0].Total : 0},
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

        /// <summary>
        /// danh sách bình luận theo cấp cha trang công bố
        /// </summary>
        /// <param name="ParentId"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("get-comment-by-parent-on-page")]
        public async Task<ActionResult<object>> MRCommentGetByParentOnPage(int? ParentId, int? PageIndex)
        {
            try
            {
                List<MRCommentGetAllOnPage> rsMRCommnentGetAllOnPage = await new MRCommentGetAllOnPage(_appSetting).MRCommentGetAllByParentOnPageDAO(ParentId, PageIndex);

                return new ResultApi { Success = ResultCode.OK, Result = rsMRCommnentGetAllOnPage };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }



        /// <summary>
        /// Thay doi trang thai binh luan - Authorize
        /// </summary>
        /// <param name="_mRCommnentUpdateIN"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update-status-comment")]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
        /// xóa bình luận - Authorize
        /// </summary>
        /// <param name="mRCommentDelete"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
        [Route("delete-comment")]
        public async Task<ActionResult<object>> MRCommentDeleteBase(MRCommentDeleteIN mRCommentDelete)
        {
            try
            {
                var result = await new MRCommentDelete(_appSetting).MRCommnentDeleteDAO(mRCommentDelete);
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
        /// tk pakn 7 ngày qua - Authorize
        /// </summary>
        /// <param name="UnitProcessId"></param>
        /// <param name="UserProcessId"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
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
                List<int> sendIdSameMR = await new MRRecommendationSame(_appSetting).MRRecommendationSameGetUserByRecommendationIdDAO(recommendationId);
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
                notification.ReceiveOrgId = 0;

                switch (recommendation.Status)
                {
                    case STATUS_RECOMMENDATION.RECEIVE_WAIT: //2 Chờ xử lý

                        foreach (var item in lstUser)
                        {

                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId ?? 0;
                            notification.Title = "PAKN CHỜ XỬ LÝ";
                            notification.SendOrgId = 0;
                            notification.Content =
                                recommendation.SendId != item.Id ?
                                sender.FullName + " vừa gửi một PAKN" : "Anh/Chị vừa tạo một PAKN";
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
                        notification.Content = "Phản ánh kiến nghị số " + recommendation.Code + " của anh/chị đã bị từ chối tiếp nhận";
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        if (sendIdSameMR != null) {
                            foreach (var item in sendIdSameMR)
                            {
                                notification.ReceiveId = item;
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                        }
                        

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
                            notification.SendOrgId = 0;
                            foreach (var item in listUserReceiveResolve)
                            {
                                notification.ReceiveId = item.Id;
                                notification.ReceiveOrgId = item.UnitId??0;
                                // insert notification
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                            break;

                        }
                        
                        notification.Content = "PAKN số " + recommendation.Code + " từ đơn vị " + unit.Name + " được gửi tới yêu cầu giải quyết";

                        foreach (var item in listUserReceiveResolve)
                        {
                            notification.ReceiveId = item.Id;
                            notification.ReceiveOrgId = item.UnitId??0;
                            // insert notification
                            await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        }

                        break;
                    case STATUS_RECOMMENDATION.PROCESS_DENY: //6 Từ chối giải quyết
                        if (lstRMForward.FirstOrDefault(x => x.Step == 2) == null)
                        {
                            unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 1).UnitReceiveId;
                        }
                        else
                        {
                            unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        }
                        //unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);
                        //// gửi cho đơn vị tiếp nhận ban đầu
                        //notification.Title = "PAKN BỊ TỪ CHỐI GIẢI QUYẾT";
                        //notification.Content = "PAKN số " + recommendation.Code + " đã bị " + unitReceive.Name + " từ chối giải quyết";
                        //foreach (var item in lstUser)
                        //{
                        //    notification.ReceiveId = item.Id;
                        //    notification.ReceiveOrgId = item.UnitId;
                        //    // insert notification
                        //    await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        //}

                        // người gửi PAKN

                        //notification.Content = "PAKN của bạn đã bị " + unitReceive.Name + " từ chối giải quyết";
                        //notification.ReceiveId = sender.Id;
                        //notification.ReceiveOrgId = null;
                        //await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.PROCESSING: //7 Đang giải quyết
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 2).UnitReceiveId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        if (unitReceive.IsMain) {
                            foreach (var item in lstUser)
                            {
                                notification.ReceiveId = item.Id;
                                notification.ReceiveOrgId = item.UnitId??0;
                                notification.Title = "PAKN ĐANG GIẢI QUYẾT";
                                notification.Content = "PAKN số " + recommendation.Code + " đã được đơn vị " + unitReceive.Name + " giải quyết";
                                // insert notification
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                        }
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = 0;
                        notification.Title = "PAKN TIẾP NHẬN GIẢI QUYẾT";
                        notification.Content = "PAKN số " + recommendation.Code + " của bạn đã được "+ unitReceive.Name + " tiếp nhận giải quyết";
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        if (sendIdSameMR != null)
                        {
                            foreach (var item in sendIdSameMR)
                            {
                                notification.ReceiveId = item;
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                        }

                        break;
                    case STATUS_RECOMMENDATION.APPROVE_WAIT: //8 Chờ phê duyệt
                        // bạn có 1 PAKN chờ phê duyệt
                        receiveId = lstRMForward.FirstOrDefault(x => x.Step == 3).ReceiveId;
                        approver = await new SYUser(_appSetting).SYUserGetByID(receiveId);

                        // gửi cho lãnh đạo
                        notification.Title = "PAKN CHỜ PHÊ DUYỆT";
                        notification.Content = "Bạn có PAKN số " + recommendation.Code + " chờ phê duyệt";
                        notification.ReceiveId = approver.Id;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.APPROVE_DENY: //9 Từ chối phê duyệt

                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3).UnitSendId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ BỊ TỪ CHỐI PHÊ DUYỆT";
                        notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối kết quả giải quyết PAKN số " + recommendation.Code;
                        //foreach (var item in lstUser)
                        //{
                        //    notification.ReceiveId = item.Id;
                        //    notification.ReceiveOrgId = item.UnitId??0;
                        //    // insert notification
                        //    await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        //}
                        notification.ReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3 && x.Status == 3).UserSendId;
                        notification.ReceiveOrgId = unitReceiveId;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        // gửi cho người tiếp nhận PAKN -chưa chắc là người giải quyết nhá

                        //người gửi PAKN
                        //notification.ReceiveId = sender.Id;
                        //notification.ReceiveOrgId = null;
                        //notification.Content = "Lãnh đạo đơn vị " + unitReceive.Name + " đã từ chối phê duyệt PAKN số " + recommendation.Code + " của bạn";
                        //await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        break;
                    case STATUS_RECOMMENDATION.FINISED: //10 Đã giải quyết
                        unitReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3).UnitSendId;
                        unitReceive = await new SYUnit(_appSetting).SYUnitGetByID(unitReceiveId);

                        notification.Title = "PAKN ĐÃ GIẢI QUYẾT XONG";
                        notification.Content = unitReceive.Name + " đã giải quyết PAKN số " + recommendation.Code;
                        //lstUser = lstUser.Where(x => x.Id != notification.SenderId).ToList();
                        //foreach (var item in lstUser)
                        //{
                        //    notification.ReceiveId = item.Id;
                        //    notification.ReceiveOrgId = item.UnitId??0;
                        //    // insert notification
                        //    await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                        //}
                        


                        // cán bộ gửi yêu cầu phê duyệt

                        notification.ReceiveId = lstRMForward.FirstOrDefault(x => x.Step == 3 && x.Status == 2).UserSendId;
                        notification.ReceiveOrgId = lstRMForward.FirstOrDefault(x => x.Step == 3 && x.Status == 2).UnitSendId;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        // gui trung tam - cu giai quyet xong la gui
                        
                        var unitMain = (await new SYUnitGetMainId(_appSetting).SYUnitGetMainIdDAO()).FirstOrDefault();
                        if (recommendation.UnitId != unitMain.Id) {
                            // khac don vi tiep nhan thi moi gui thong bao
                            lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO((int)unitMain.Id);
                            foreach (var item in lstUser)
                            {
                                notification.ReceiveId = item.Id;
                                notification.ReceiveOrgId = item.UnitId??0;
                                // insert notification
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                        }

                        // người gửi PAKN
                        notification.ReceiveId = sender.Id;
                        notification.ReceiveOrgId = 0;
                        await new SYNotification(_appSetting, _configuration).InsertNotification(notification);

                        if (sendIdSameMR != null)
                        {
                            foreach (var item in sendIdSameMR)
                            {
                                notification.ReceiveId = item;
                                await new SYNotification(_appSetting, _configuration).InsertNotification(notification);
                            }
                        }
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
