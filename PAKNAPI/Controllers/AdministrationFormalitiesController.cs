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

namespace PAKNAPI.Controller
{
    [Route("api/AdministrationFormalities")]
    [ApiController]
    public class AdministrationFormalitiesController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public AdministrationFormalitiesController(IWebHostEnvironment hostingEnvironment, IAppSetting appSetting)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpGet]
        [Authorize]
        [Route("AdministrationFormalitiesGetByID")]
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

        //[HttpGet]
        //[Authorize]
        //[Route("AdministrationFormalitiesGetList")]
        //public async Task<ActionResult<object>> AdministrationFormalitiesGetByIDView(int? Id)
        //{
        //    try
        //    {
        //        DAMAdministrationFilesGetByAdministrationId data = new AdministrationFormalitiesGetByIDViewResponse();
        //        return new ResultApi { Success = ResultCode.OK, Result = await new AdministrationFormalitiesDAO(_appSetting).AdministrationFormalitiesGetByIDView(Id) };
        //    }
        //    catch (Exception ex)
        //    {
        //        new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

        //        return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
        //    }
        //}


        [HttpPost]
        [Authorize]
        [Route("AdministrationFormalitiesInsert")]
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


        [HttpPost,DisableRequestSizeLimit]
        [Authorize]
        [Route("AdministrationFormalitiesUpdate")]
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
                                itemCreate.CopyForm = item.CopyForm;
                                itemCreate.IsBind = item.IsBind;
                                itemCreate.NameExhibit = item.NameExhibit;
                                itemCreate.OriginalForm = item.OriginalForm;
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
    }
}
