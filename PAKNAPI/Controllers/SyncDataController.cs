using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;
using PAKNAPI.Models.Recommendation;
using System.Net.Http;
using static PAKNAPI.Common.ClientRestfull;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Web;
using System.Text.RegularExpressions;
using Bugsnag;
using PAKNAPI.Models.SyncData;
using System.Threading;

namespace PAKNAPI.Controllers
{
    [Route("api/sync-data")]
    [ApiController]
    public class SyncDataController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;

        public SyncDataController(IAppSetting appSetting, IWebHostEnvironment hostingEnvironment, IClient bugsnag)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
            _bugsnag = bugsnag;

        }

        [Route("sync-gop-y-kien-nghi")]
        [HttpGet]
        public async Task<ActionResult<object>> SyncKhanhHoa()
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };//Load trang web, nạp html vào document
            Task<HtmlDocument> document = htmlWeb.LoadFromWebAsync("https://www.khanhhoa.gov.vn/module/gop-y");

            var items = new List<GopYKienNghi>();
            var threadItems = document.Result.DocumentNode.Descendants("div")
                .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "content-feedback")
                .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "row").First()
                .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "item col-xs-12 col-sm-6 col-md-4").ToList();
            // delete all
            //await new RecommendationDAO(_appSetting).SyncKhanhHoaDeleteAll();
            foreach (var item in threadItems)
            {
                var objectAdd = new GopYKienNghi();
                var linkNode = item.Descendants("a").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("title"));
                var link = linkNode.Attributes["href"].Value;
                var textHead = linkNode.InnerText.Split("\r\n");
                string[] s = textHead[0].Trim().Split(" ");
                objectAdd.Questioner = string.Join(" ", s.Where(x => x != s[s.Length - 1]).ToArray());
                if (textHead.Length > 1)
                {
                    objectAdd.Question = textHead[1].Trim();
                }
                objectAdd.CreatedDate = item.Descendants("span").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedday")).InnerText;
                Task<HtmlDocument> documentDetail = htmlWeb.LoadFromWebAsync("https://www.khanhhoa.gov.vn/" + link);

                var threadItemsChild = documentDetail.Result.DocumentNode.Descendants("div")
                .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "print-chitiet");
                objectAdd.QuestionContent = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("chitietbaiviet")).InnerHtml;
                objectAdd.Reply = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedback-traloi-content")).InnerHtml.Trim();
                objectAdd.ReplyDate = documentDetail.Result.DocumentNode.Descendants("div")
                    .FirstOrDefault(x => x.Attributes.Contains("class") && x.Attributes["class"].Value == "feedback-traloi-bottom").InnerText.Split(":")[1];

                var id = await new RecommendationDAO(_appSetting).SyncKhanhHoaInsert(objectAdd);
                if (Convert.ToInt32(id) < 0) { continue; };

                var files = documentDetail.Result.GetElementbyId("ctl38_ctl03_pnTep");
                if (files != null)
                {
                    string url = files.ChildNodes.Descendants("a").FirstOrDefault().Attributes["href"].Value;
                    string fileName = url.Split("/")[url.Split("/").Length - 1];
                    MR_SyncFileAttach fileInsert = new MR_SyncFileAttach();

                    string folder = "Upload\\CongThongTinDienTu\\" + id;
                    string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    using (WebClient webClient = new WebClient())
                    {
                        string[] fileNameArr = url.Split(".");
                        fileInsert.ObjectId = Convert.ToInt32(id);
                        fileInsert.Type = GetFileTypes.GetFileTypeExtension("." + fileNameArr.FirstOrDefault(x => x == fileNameArr[fileNameArr.Length - 1]).ToString());
                        fileInsert.FileName = Path.GetFileName(fileName).Replace("+", "");
                        fileInsert.FilePath = Path.Combine(folder, fileInsert.FileName);
                        webClient.DownloadFileAsync(new Uri("https://www.khanhhoa.gov.vn/" + url), Path.Combine(folderPath, fileInsert.FileName));
                        await new MR_SyncFileAttach(_appSetting).RecommentdationSyncFileAttachInsertDAO(fileInsert);
                    }
                }

            }
            new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
            return new ResultApi
            {
                Success = ResultCode.OK
            };
        }

        [Route("sync-cong-dich-vu-cong-quoc-gia")] // sync-cong-dich-vu-cong-quoc-gia
        [HttpGet]
        public async Task<ActionResult<object>> SyncCongDichVuCongQuocGia()
        {
            return await new DichVuCongQuocGiaSync(_appSetting, _hostingEnvironment).SyncDichVuCongQuocGia();
        }

        [Route("sync-hop-thu-gop-y-khanh-hoa")]
        [HttpGet]
        public ActionResult<object> SyncHopThuGopYKhanhHoa()
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };//Load trang web, nạp html vào document
            try
            {
                Task<HtmlDocument> document = htmlWeb.LoadFromWebAsync("https://thongtin.hanhchinhcong.khanhhoa.gov.vn/module/hop-thu-gop-y");

                var items = new List<GopYKienNghi>();
                var threadItems = document.Result.DocumentNode.Descendants("div")
                    .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "content-feedback")
                    .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "row").First()
                    .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "item col-xs-12 col-sm-6 col-md-4").ToList();

                foreach (var item in threadItems)
                {
                    var objectAdd = new GopYKienNghi();
                    var linkNode = item.Descendants("a").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("title"));
                    var link = linkNode.Attributes["href"].Value;
                    var textHead = linkNode.InnerText.Split("\r\n");
                    string[] s = textHead[0].Trim().Split(" ");
                    objectAdd.Questioner = string.Join(" ", s.Where(x => x != s[s.Length - 1]).ToArray());
                    if (textHead.Length > 1)
                    {
                        objectAdd.Question = textHead[1].Trim();
                    }
                    //objectAdd.CreatedDate = "09/10/2018 | 10:02-AM";//item.Descendants("span").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedday")).InnerText;
                    Task<HtmlDocument> documentDetail = htmlWeb.LoadFromWebAsync("https://thongtin.hanhchinhcong.khanhhoa.gov.vn/" + link);
                    var dates = documentDetail.Result.GetElementbyId("datearticle").InnerText.Split(":");
                    objectAdd.CreatedDate = string.Join(" ", dates.Where(x => x != dates[0]).ToArray());
                    var threadItemsChild = documentDetail.Result.DocumentNode.Descendants("div")
                    .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "print-chitiet");
                    objectAdd.QuestionContent = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("chitietbaiviet")).InnerHtml;
                    objectAdd.Reply = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedback-traloi-content")).InnerHtml.Trim();
                    items.Add(objectAdd);
                }
                new RecommendationDAO(_appSetting).SyncHopThuGopYKhanhHoa(items);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
                return new ResultApi
                {
                    Success = ResultCode.OK
                };
            }
            catch (Exception ex) {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new ResultApi
                {
                    Success = ResultCode.ORROR
                };
            }

        }

        [Route("sync-quan-ly-kien-nghi-cu-tri")]
        [HttpGet]
        public async Task<ActionResult<object>> SyncQuanLyKienNghiCuTriAsync()
        {
            return await new KienNghiCuTriSync(_appSetting, _hostingEnvironment).SyncKienNghiCuTri();
        }



        [Route("get-list-cong-thong-tin-dien-tu-tinh-on-page")]
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
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("cong-thong-tin-dien-tu-tinh-get-by-id")]
        [HttpGet]
        public async Task<object> CongThongTinDienTuTinhGetByIdBase(int Id)
        {
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                var repository = new RecommandationSyncDAO(_appSetting);
                var rs = await repository.CongThongTinDienTuTinhGetByIdDAO(Id);
                var files = await new MR_SyncFileAttach(_appSetting).RecommentdationSyncFileAttachGetByObjectIdDAO(Id);
                foreach (var item in files)
                {
                    item.FilePath = decrypt.EncryptData(item.FilePath);
                }
                IDictionary<string, object> json = new Dictionary<string, object>
                {
                    {"Data", rs},
                    {"FileAttach", files},
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
        [Route("get-list-quan-ly-kien-nghi-cu-tri-on-page")]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }


        [HttpGet]
        [Route("cu-tri-khanh-hoa-get-by-id")]
        public async Task<ActionResult<object>> MRRecommendationKNCTGetByIdBase(int Id)
        {
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                List<MR_CuTriTinhKhanhHoa> rsMRRecommendationKNCTGetById = await new MR_CuTriTinhKhanhHoa(_appSetting).MR_Sync_CuTriTinhKhanhHoaGetByIdDAO(Id);
                List<MR_SyncFileAttach> files = await new MR_SyncFileAttach(_appSetting).MR_Sync_CuTriTinhKhanhHoaFileAttachGetByKNCTIdDAO(Id);

                foreach (var item in files)
                {
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [Route("get-list-cong-thong-tin-dv-hcc-on-page")]
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

        [Route("cong-thong-tin-dv-hcc-get-by-id")]
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


        [Route("get-list-pakn-chinh-phu-on-page")]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Route("pakn-chinh-phu-get-by-id")]
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
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }




        //[Route("PUGetPagedList")]
        //[HttpGet]
        //[Authorize]
        //public async Task<object> PUGetPagedList(
        //    string keyword,
        //    int src = 1,
        //    int pageIndex = 1,
        //    int pageSize = 20)
        //{

        //    try
        //    {
        //        var repository = new RecommandationSyncDAO(_appSetting);

        //        var rs = await repository.PURecommandationSyncGetPagedList(keyword, src, pageIndex, pageSize);
        //        IDictionary<string, object> json = new Dictionary<string, object>
        //        {
        //            {"Data", rs}
        //        };
        //        return new ResultApi { Success = ResultCode.OK, Result = json };
        //    }
        //    catch (Exception ex)
        //    {
        //        _bugsnag.Notify(ex);

        //        return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
        //    }
        //}

        //[Route("PUGetGetById")]
        //[HttpGet]
        //[Authorize]
        //public async Task<object> PUGetGetById(
        //    long id,
        //    int src = 1)
        //{

        //    try
        //    {
        //        var repository = new RecommandationSyncDAO(_appSetting);

        //        var rs = await repository.PURecommandationSyncGetDetail(id, src);
        //        IDictionary<string, object> json = new Dictionary<string, object>
        //        {
        //            {"Data", rs}
        //        };
        //        return new ResultApi { Success = ResultCode.OK, Result = json };
        //    }
        //    catch (Exception ex)
        //    {
        //        _bugsnag.Notify(ex);

        //        return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
        //    }
        //}

        private async Task<List<ResponseDropdownKienNghiCuTri>> SyncFieldKienNghiCuTriAsync()
        {
            try
            {
                var results = new HttpResponseMessage();
                /// header
                var TkeyHeader = new List<KeyValuePair<string, string>>();
                HeaderRess header = new HeaderRess();
                header.Tkey = TkeyHeader;
                header.ContentType = "application/json";
                results = GetStringAsync("https://kiennghicutri.khanhhoa.gov.vn:96", "/api/RequestOut/getDropDownDataSearchOutView", header);

                if (results.StatusCode == HttpStatusCode.OK)
                {
                    var responseModel = JsonConvert.DeserializeObject<ResponseFieldKienNghiCuTri>(results.Content.ReadAsStringAsync().Result);
                    if (responseModel.lstLinhVuc.Count > 0) {
                        // xóa hết
                        await new CAFieldKNCTModel(_appSetting).CAFieldKNCTDeleteAll();
                        foreach (var item in responseModel.lstLinhVuc) {
                            CAFieldKNCTInsertIN fieldInsert = new CAFieldKNCTInsertIN();
                            fieldInsert.Name = item.text;
                            fieldInsert.Code = item.value;
                            await new CAFieldKNCTModel(_appSetting).CAFieldKNCTInsertDAO(fieldInsert);
                        }
                    }

                    return responseModel.lstLinhVuc;
                }
                else {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// sync lĩnh vực thủ tục hành chính
        /// </summary>
        /// <returns></returns>
        [Route("sync-field-tthc")]
        [HttpGet]
        public async Task<ActionResult<object>> SyncFieldTTHCAsync()
        {
            try
            {
                var results = new HttpResponseMessage();
                /// header
                var TkeyHeader = new List<KeyValuePair<string, string>>();
                HeaderRess header = new HeaderRess();
                header.Tkey = TkeyHeader;
                header.ContentType = "application/json";
                results = GetStringAsync("https://tthckhapi.azurewebsites.net", "/api/v1/LinhVucs/GetTree?page=1&pageSize=10&sort=Ma+ASC", header);

                if (results.StatusCode == HttpStatusCode.OK)
                {
                    var responseModel = JsonConvert.DeserializeObject<LinhVucTTHCResponse>(results.Content.ReadAsStringAsync().Result);
                    if (responseModel.DanhSachLinhVuc.Count > 0)
                    {
                        // xóa hết
                        await new CAFieldDAMDeleteAll(_appSetting).CAFieldDAMDeleteAllDAO();
                        foreach (var item in responseModel.DanhSachLinhVuc)
                        {
                            CAFieldDAMInsertIN fieldInsert = new CAFieldDAMInsertIN();
                            fieldInsert.Name = item.Ten.Replace("-", string.Empty).Replace("|", string.Empty);
                            fieldInsert.FieldDAMId = item.Id;
                            fieldInsert.ParentId = item.LinhVucChaId;
                            await new CAFieldDAMInsert(_appSetting).CAFieldDAMInsertDAO(fieldInsert);
                        }
                    }

                    return responseModel.DanhSachLinhVuc;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
