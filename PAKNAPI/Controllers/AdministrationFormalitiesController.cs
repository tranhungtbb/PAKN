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
using PAKNAPI.Models.AdministrationFormalities;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.User;
using PAKNAPI.Models.ModelBase;
using Bugsnag;

namespace PAKNAPI.Controller
{
    /// <summary>
    /// thủ tục hành chính
    /// </summary>
    [Route("api/administration-formalities")]
    [ApiController]
    public class AdministrationFormalitiesController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;
        public AdministrationFormalitiesController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
            _bugsnag = bugsnag;
        }

        /// <summary>
        /// Chi tiết thủ tục hành chính
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize]
        [Route("get-by-id")]
        public async Task<ActionResult<object>> AdministrationFormalitiesGetByID(int? Id)
        {
            try
            {
                return new ResultApi { Success = ResultCode.OK, Result = await new AdministrationFormalitiesDAO(_appSetting).AdministrationFormalitiesGetByID(Id) };
            }
            catch (Exception ex)
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// thêm mới thủ tục hành chính
        /// </summary>
        /// <returns></returns>

        [HttpPost]
        [Authorize]
        [Route("insert")]
        public async Task<ActionResult<object>> AdministrationFormalitiesInsert()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                AdministrationFormalitiesInsertRequest request = new AdministrationFormalitiesInsertRequest();
                request.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                request.Data = JsonConvert.DeserializeObject<DAMAdministrationInsertIN>(Request.Form["Data"].ToString(), jss);
                request.LstXoaFile = JsonConvert.DeserializeObject<List<DAMFileObj>>(Request.Form["LstXoaFile"].ToString(), jss);
                request.LstXoaFileForm = JsonConvert.DeserializeObject<List<DAMCompositionProfileObj>>(Request.Form["LstXoaFileForm"].ToString(), jss);
                request.LstCompositionProfile = JsonConvert.DeserializeObject<List<DAMCompositionProfileCreateObj>>(Request.Form["LstCompositionProfile"].ToString(), jss);
                request.LstCharges = JsonConvert.DeserializeObject<List<DAMChargesCreateIN>>(Request.Form["LstCharges"].ToString(), jss);
                request.LstImplementationProcess = JsonConvert.DeserializeObject<List<DAMImplementationProcessCreateIN>>(Request.Form["LstImplementationProcess"].ToString(), jss);
                request.LstDelete = JsonConvert.DeserializeObject<List<DeleteTableObject>>(Request.Form["LstDelete"].ToString(), jss);
                request.Files = Request.Form.Files;
                if (request.UserId.HasValue)
                {
                    request.Data.CreatedBy = int.Parse(request.UserId.Value.ToString());
                }

                int? Id = Int32.Parse((await new DAMAdministrationInsert(_appSetting).DAMAdministrationInsertDAO(request.Data)).ToString());
                if (Id > 0)
                {
                    if (request.Files != null && request.Files.Count > 0)
                    {

                        string folder = "Upload\\AdministrationFormalities\\" + Id;
                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);


                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        foreach (var item in request.Files)
                        {
                            if (item.Name == "File")
                            {
                                DAMAdministrationFilesInsertIN file = new DAMAdministrationFilesInsertIN();
                                file.AdministrationId = Id;
                                file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                                string filePath = Path.Combine(folderPath, file.Name);
                                file.FileAttach = Path.Combine(folder, file.Name);
                                file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                await new DAMAdministrationFilesInsert(_appSetting).DAMAdministrationFilesInsertDAO(file);
                            }
                        }
                    }

                    if (request.LstCompositionProfile != null && request.LstCompositionProfile.Count > 0)
                    {
                        foreach (var item in request.LstCompositionProfile)
                        {
                            item.AdministrationId = Id;
                            int? idCP = Int32.Parse((await new DAMCompositionProfileCreate(_appSetting).DAMCompositionProfileCreateDAO(item)).ToString());

                            if (request.Files != null && request.Files.Count > 0)
                            {

                                string folderForm = "Upload\\AdministrationFormalities\\CompositionProfile" + idCP;
                                string folderPathForm = Path.Combine(_hostingEnvironment.ContentRootPath, folderForm);
                                if (!Directory.Exists(folderPathForm))
                                {
                                    Directory.CreateDirectory(folderPathForm);
                                }
                                foreach (var f in request.Files)
                                {
                                    if (f.Name == "Profile" + item.Index)
                                    {
                                        DAMCompositionProfileFileFilesInsertIN file = new DAMCompositionProfileFileFilesInsertIN();
                                        file.CompositionProfileId = idCP;
                                        file.Name = Path.GetFileName(f.FileName).Replace("+", "");
                                        string filePath = Path.Combine(folderPathForm, f.Name);
                                        file.FileAttach = Path.Combine(folderForm, f.Name);
                                        file.FileType = GetFileTypes.GetFileTypeInt(f.ContentType);
                                        using (var stream = new FileStream(filePath, FileMode.Create))
                                        {
                                            f.CopyTo(stream);
                                        }
                                        await new DAMCompositionProfileFileFilesInsert(_appSetting).DAMCompositionProfileFileFilesInsertDAO(file);
                                    }
                                }
                            }
                        }
                    }

                    if (request.LstImplementationProcess != null && request.LstImplementationProcess.Count > 0)
                    {
                        foreach (var item in request.LstImplementationProcess)
                        {
                            item.AdministrationId = Id;
                            await new DAMImplementationProcessCreate(_appSetting).DAMImplementationProcessCreateDAO(item);
                        }
                    }

                    if (request.LstCharges != null && request.LstCharges.Count > 0)
                    {
                        foreach (var item in request.LstCharges)
                        {
                            item.AdministrationId = Id;
                            await new DAMChargesCreate(_appSetting).DAMChargesCreateDAO(item);
                        }
                    }
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
        /// update thủ tục hành chính
        /// </summary>
        /// <returns></returns>
        [HttpPost,DisableRequestSizeLimit]
        [Authorize]
        [Route("update")]
        public async Task<ActionResult<object>> AdministrationFormalitiesUpdate()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                AdministrationFormalitiesUpdateRequest request = new AdministrationFormalitiesUpdateRequest();
                request.UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                request.UserFullName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                request.Data = JsonConvert.DeserializeObject<DAMAdministrationUpdateIN>(Request.Form["Data"].ToString(), jss);
                request.LstXoaFile = JsonConvert.DeserializeObject<List<DAMFileObj>>(Request.Form["LstXoaFile"].ToString(), jss);
                request.LstXoaFileForm = JsonConvert.DeserializeObject<List<DAMCompositionProfileObj>>(Request.Form["LstXoaFileForm"].ToString(), jss);
                request.LstCompositionProfile = JsonConvert.DeserializeObject<List<DAMCompositionProfileUpdateObj>>(Request.Form["LstCompositionProfile"].ToString(), jss);
                request.LstCharges = JsonConvert.DeserializeObject<List<DAMChargesUpdateIN>>(Request.Form["LstCharges"].ToString(), jss);
                request.LstImplementationProcess = JsonConvert.DeserializeObject<List<DAMImplementationProcessUpdateIN>>(Request.Form["LstImplementationProcess"].ToString(), jss);
                request.LstDelete = JsonConvert.DeserializeObject<List<DeleteTableObject>>(Request.Form["LstDelete"].ToString(), jss);
                request.Files = Request.Form.Files;
                if (request.UserId.HasValue)
                {
                    request.Data.UpdatedBy = int.Parse(request.UserId.Value.ToString());
                }

                await new DAMAdministrationUpdate(_appSetting).DAMAdministrationUpdateDAO(request.Data);
                if (request.Data.Id > 0)
                {
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
                            if (string.IsNullOrWhiteSpace(item.FileAttach)) continue;
                            string filePath = Path.Combine(webRootPath, decrypt.DecryptData(item.FileAttach));

                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            DAMAdministrationFilesDeleteIN fileDel = new DAMAdministrationFilesDeleteIN();
                            fileDel.Id = item.Id;
                            await new DAMAdministrationFilesDelete(_appSetting).DAMAdministrationFilesDeleteDAO(fileDel);
                        }

                    }

                    if (request.LstXoaFileForm.Count > 0)
                    {

                        Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                        string webRootPath = _hostingEnvironment.ContentRootPath;

                        if (string.IsNullOrWhiteSpace(webRootPath))
                        {
                            webRootPath = Path.Combine(Directory.GetCurrentDirectory());
                        }

                        foreach (var item in request.LstXoaFileForm)
                        {
                            if (string.IsNullOrWhiteSpace(item.FileAttach)) continue;
                            string filePath = Path.Combine(webRootPath, item.FileAttach);

                            if (System.IO.File.Exists(filePath))
                            {
                                System.IO.File.Delete(filePath);
                            }
                            DAMCompositionProfileFileFilesDeleteIN fileDel = new DAMCompositionProfileFileFilesDeleteIN();
                            fileDel.Id = item.Id;
                            await new DAMCompositionProfileFileFilesDelete(_appSetting).DAMCompositionProfileFileFilesDeleteDAO(fileDel);
                        }

                    }

                    if (request.LstDelete.Count > 0)
                    {
                        foreach (var item in request.LstDelete)
                        {
                            if (item.Type == 1)
                            {
                                DAMCompositionProfileDeleteByIdIN fileDel = new DAMCompositionProfileDeleteByIdIN();
                                fileDel.Id = item.Id;
                                await new DAMCompositionProfileDeleteById(_appSetting).DAMCompositionProfileDeleteByIdDAO(fileDel);
                            }
                            else if (item.Type == 2)
                            {
                                DAMImplementationProcessDeleteByIdIN fileDel = new DAMImplementationProcessDeleteByIdIN();
                                fileDel.Id = item.Id;
                                await new DAMImplementationProcessDeleteById(_appSetting).DAMImplementationProcessDeleteByIdDAO(fileDel);
                            }
                            else if (item.Type == 3)
                            {
                                DAMChargesDeleteByIdIN fileDel = new DAMChargesDeleteByIdIN();
                                fileDel.Id = item.Id;
                                await new DAMChargesDeleteById(_appSetting).DAMChargesDeleteByIdDAO(fileDel);
                            }
                        }
                    }

                    if (request.Files != null && request.Files.Count > 0)
                    {

                        string folder = "Upload\\AdministrationFormalities\\" + request.Data.Id;
                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);


                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }
                        foreach (var item in request.Files)
                        {
                            if (item.Name == "File")
                            {
                                DAMAdministrationFilesInsertIN file = new DAMAdministrationFilesInsertIN();
                                file.AdministrationId = request.Data.Id;
                                file.Name = Path.GetFileName(item.FileName).Replace("+", "");
                                string filePath = Path.Combine(folderPath, file.Name);
                                file.FileAttach = Path.Combine(folder, file.Name);
                                file.FileType = GetFileTypes.GetFileTypeInt(item.ContentType);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                await new DAMAdministrationFilesInsert(_appSetting).DAMAdministrationFilesInsertDAO(file);
                            }
                        }
                    }

                    if (request.LstCompositionProfile != null && request.LstCompositionProfile.Count > 0)
                    {
                        foreach (var item in request.LstCompositionProfile)
                        {
                            if (item.Id == 0)
                            {
                                DAMCompositionProfileCreateIN itemCreate = new DAMCompositionProfileCreateIN();
                                itemCreate.AdministrationId = item.AdministrationId;
                                itemCreate.CopyForm = item.CopyForm.ToString();
                                itemCreate.IsBind = item.IsBind;
                                itemCreate.NameExhibit = item.NameExhibit;
                                itemCreate.OriginalForm = item.OriginalForm.ToString();
                                int? idCP = Int32.Parse((await new DAMCompositionProfileCreate(_appSetting).DAMCompositionProfileCreateDAO(itemCreate)).ToString());

                                if (request.Files != null && request.Files.Count > 0)
                                {

                                    string folderForm = "Upload\\AdministrationFormalities\\CompositionProfile" + idCP;
                                    string folderPathForm = Path.Combine(_hostingEnvironment.ContentRootPath, folderForm);
                                    if (!Directory.Exists(folderPathForm))
                                    {
                                        Directory.CreateDirectory(folderPathForm);
                                    }
                                    foreach (var f in request.Files)
                                    {
                                        if (f.Name == "Profile" + item.Index)
                                        {
                                            DAMCompositionProfileFileFilesInsertIN file = new DAMCompositionProfileFileFilesInsertIN();
                                            file.CompositionProfileId = idCP;
                                            file.Name = Path.GetFileName(f.FileName).Replace("+", "");
                                            string filePath = Path.Combine(folderPathForm, f.Name);
                                            file.FileAttach = Path.Combine(folderForm, f.Name);
                                            file.FileType = GetFileTypes.GetFileTypeInt(f.ContentType);
                                            using (var stream = new FileStream(filePath, FileMode.Create))
                                            {
                                                f.CopyTo(stream);
                                            }
                                            await new DAMCompositionProfileFileFilesInsert(_appSetting).DAMCompositionProfileFileFilesInsertDAO(file);
                                        }
                                    }
                                }
                            }else
                            {
                                await new DAMCompositionProfileUpdate(_appSetting).DAMCompositionProfileUpdateDAO(item);

                                if (request.Files != null && request.Files.Count > 0)
                                {

                                    string folderForm = "Upload\\AdministrationFormalities\\CompositionProfile" + item.Id;
                                    string folderPathForm = Path.Combine(_hostingEnvironment.ContentRootPath, folderForm);
                                    if (!Directory.Exists(folderPathForm))
                                    {
                                        Directory.CreateDirectory(folderPathForm);
                                    }
                                    foreach (var f in request.Files)
                                    {
                                        if (f.Name == "Profile" + item.Index)
                                        {
                                            DAMCompositionProfileFileFilesInsertIN file = new DAMCompositionProfileFileFilesInsertIN();
                                            file.CompositionProfileId = item.Id;
                                            file.Name = Path.GetFileName(f.FileName).Replace("+", "");
                                            string filePath = Path.Combine(folderPathForm, f.Name);
                                            file.FileAttach = Path.Combine(folderForm, f.Name);
                                            file.FileType = GetFileTypes.GetFileTypeInt(f.ContentType);
                                            using (var stream = new FileStream(filePath, FileMode.Create))
                                            {
                                                f.CopyTo(stream);
                                            }
                                            await new DAMCompositionProfileFileFilesInsert(_appSetting).DAMCompositionProfileFileFilesInsertDAO(file);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (request.LstImplementationProcess != null && request.LstImplementationProcess.Count > 0)
                    {
                        foreach (var item in request.LstImplementationProcess)
                        {
                            if (item.Id == null || item.Id == 0)
                            {
                                DAMImplementationProcessCreateIN itemCreate = new DAMImplementationProcessCreateIN();
                                itemCreate.AdministrationId = request.Data.Id;
                                itemCreate.Name = item.Name;
                                itemCreate.Result = item.Result;
                                itemCreate.Time = item.Time;
                                itemCreate.Unit = item.Unit;
                                await new DAMImplementationProcessCreate(_appSetting).DAMImplementationProcessCreateDAO(itemCreate);
                            }else
                            {
                                await new DAMImplementationProcessUpdate(_appSetting).DAMImplementationProcessUpdateDAO(item);
                            }
                        }
                    }

                    if (request.LstCharges != null && request.LstCharges.Count > 0)
                    {
                        foreach (var item in request.LstCharges)
                        {
                            if (item.Id == null || item.Id == 0)
                            {
                                DAMChargesCreateIN itemCreate = new DAMChargesCreateIN();
                                itemCreate.AdministrationId = request.Data.Id;
                                itemCreate.Charges = item.Charges;
                                itemCreate.Description = item.Description;
                                await new DAMChargesCreate(_appSetting).DAMChargesCreateDAO(itemCreate);
                            }
                            else
                            {
                                await new DAMChargesUpdate(_appSetting).DAMChargesUpdateDAO(item);
                            }
                        }
                    }
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
        /// danh sách thủ tục hành chính
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Object"></param>
        /// <param name="Organization"></param>
        /// <param name="UnitId"></param>
        /// <param name="Field"></param>
        /// <param name="Status"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="TotalRecords"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-administration-formalities-on-page")]
        public async Task<ActionResult<object>> DAMAdministrationGetListBase(string Code, string Name, string Object, string Organization, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex, int? TotalRecords)
        {
            try
            {
                List<DAMAdministrationGetList> rsDAMAdministrationGetList = await new DAMAdministrationGetList(_appSetting).DAMAdministrationGetListDAO(Code, Name, Object, Organization, UnitId, Field, Status, PageSize, PageIndex, TotalRecords);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"DAMAdministrationGetList", rsDAMAdministrationGetList},
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
        /// danh sách chuyển tiếp thủ tục hành chính
        /// </summary>
        /// <param name="Code"></param>
        /// <param name="Name"></param>
        /// <param name="Organization"></param>
        /// <param name="FieldId"></param>
        /// <param name="UnitForward"></param>
        /// <param name="Status"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize]
        [Route("get-list-administration-formalities-forward-on-page")]
        public async Task<ActionResult<object>> DAMAdministrationForwardGetListOnPageBase(string Code, string Name, string Organization, int? FieldId, int? UnitForward, int? Status, int PageSize, int PageIndex)
        {
            try
            {
                var userProcess = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                List<DAMAdministrationForwardGetList> rsDAMAdministrationGetList =
                    await new DAMAdministrationForwardGetList(_appSetting).
                    DAMAdministrationForwardGetListDAO(userProcess, Code, Name, Organization, FieldId, UnitForward, Status, PageSize, PageIndex);

                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"DAMAdministrationForwardGetListOnPage", rsDAMAdministrationGetList},
                        {"TotalCount", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? rsDAMAdministrationGetList[0].RowNumber : 0},
                        {"PageIndex", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? PageSize : 0},
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
        /// danh sách thủ tục hành chính trang chủ
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-administration-formalities-home-page")]
        public async Task<ActionResult<object>> DAMAdministrationGetListTopBase()
        {
            try
            {
                List<DAMAdministrationGetList> rsDAMAdministrationGetListTop = await new DAMAdministrationGetList(_appSetting).DAMAdministrationGetListTopDAO();
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"DAMAdministrationGetListTop", rsDAMAdministrationGetListTop},
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
        /// xóa thủ tục hành chính
        /// </summary>
        /// <param name="_dAMAdministrationDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("delete")]
        public async Task<ActionResult<object>> DAMAdministrationDeleteBase(DAMAdministrationDeleteIN _dAMAdministrationDeleteIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationDelete(_appSetting).DAMAdministrationDeleteDAO(_dAMAdministrationDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// cập nhập trạng thái thủ tục hành chính
        /// </summary>
        /// <param name="_dAMAdministrationUpdateShowIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("update-status")]
        public async Task<ActionResult<object>> DAMAdministrationUpdateShow(DAMAdministrationUpdateShowIN _dAMAdministrationUpdateShowIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationUpdateShow(_appSetting).DAMAdministrationUpdateShowDAO(_dAMAdministrationUpdateShowIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }
        /// <summary>
        /// chuyển tiếp thủ tục hành chính
        /// </summary>
        /// <param name="LstUnitId"></param>
        /// <param name="AdministrationId"></param>
        /// <param name="Content"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("administration-forward")]
        [Authorize]
        public async Task<ActionResult<object>> DAMAdministrationForward(string LstUnitId, int AdministrationId, string Content)
        {
            try
            {
                List<int> lstUnitId = new List<int>();
                lstUnitId = LstUnitId.Split(',').Select(Int32.Parse).Where(x => x != 0).ToList();
                if (lstUnitId.Count == 0)
                {
                    return new ResultApi { Success = ResultCode.ORROR, Message = "Unit is not null" };
                }
                DAMAdministrationForward modelInsert = new DAMAdministrationForward();
                modelInsert.AdministrationId = AdministrationId;
                modelInsert.CreateBy = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                modelInsert.CreatedDate = DateTime.Now;
                modelInsert.Content = Content;

                foreach (var item in lstUnitId)
                {
                    var lstUser = await new SYUserGetByUnitId(_appSetting).SYUserGetByUnitIdDAO(item);
                    if (lstUser.Count() == 0) { continue; }
                    var lstUserId = new List<long>();
                    lstUser.ForEach(item => {
                        lstUserId.Add(item.Id);
                    });
                    modelInsert.UnitId = item;
                    modelInsert.LstUserReceive = String.Join(",", lstUserId);
                    var id = await new DAMAdministrationForward(_appSetting).DAMAdministrationForwardInsertDAO(modelInsert);
                    // tạo thông báo
                    var model = new SYNotificationModel();
                    model.SenderId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
                    model.SendOrgId = new LogHelper(_appSetting).GetUnitIdFromRequest(HttpContext);

                    lstUser.ForEach(async item =>
                    {
                        model.ReceiveId = item.Id;
                        model.DataId = Convert.ToInt32(AdministrationId);
                        model.SendDate = DateTime.Now;
                        model.Type = TYPENOTIFICATION.ADMINISTRATIVE;
                        model.Title = "Tiếp nhận thủ tục hành chính";
                        model.Content = "Bạn vừa nhận được một thủ tục hành chính từ " + new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
                        model.IsViewed = true;
                        model.IsReaded = true;
                        // insert vào db-
                        await new SYNotification(_appSetting).SYNotificationInsertDAO(model);
                    });
                }
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi { Success = ResultCode.OK };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// drop down lĩnh vực thủ tục hành chính
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-drop-down")]
        public async Task<ActionResult<object>> CAFieldDAMGetDropdownBase()
        {
            try
            {
                List<CAFieldDAMGetDropdown> rsCAFieldDAMGetDropdown = await new CAFieldDAMGetDropdown(_appSetting).CAFieldDAMGetDropdownDAO();
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"CAFieldDAMGetDropdown", rsCAFieldDAMGetDropdown},
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

    }
}
