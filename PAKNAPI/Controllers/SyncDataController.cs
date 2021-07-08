﻿using PAKNAPI.Common;
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

namespace PAKNAPI.Controllers
{
	[Route("api/SyncData")]
	[ApiController]
	public class SyncDataController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public SyncDataController(IAppSetting appSetting, IWebHostEnvironment hostingEnvironment)
		{
			_appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;

        }

		[Route("SyncKhanhHoa")]
		[HttpGet]
        [AllowAnonymous]
        public List<GopYKienNghi> SyncKhanhHoa()
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };//Load trang web, nạp html vào document
            HtmlDocument document = htmlWeb.Load("https://www.khanhhoa.gov.vn/module/gop-y");

            var items = new List<GopYKienNghi>();
            var threadItems = document.DocumentNode.Descendants("div")
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
                objectAdd.CreatedDate = item.Descendants("span").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedday")).InnerText;
                HtmlDocument documentDetail = htmlWeb.Load("https://www.khanhhoa.gov.vn/" + link);

                var threadItemsChild = documentDetail.DocumentNode.Descendants("div")
                .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "print-chitiet");
                objectAdd.QuestionContent = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("chitietbaiviet")).InnerHtml;
                objectAdd.Reply = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedback-traloi-content")).InnerHtml.Trim();
                items.Add(objectAdd);
            }
            new RecommendationDAO(_appSetting).SyncKhanhHoa(items);
            return items;
        }

        [Route("SyncCongDichVuCongQuocGia")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<object>> SyncCongDichVuCongQuocGiaAsync()
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };
            MR_SyncFileAttach fileInsert = new MR_SyncFileAttach();
            //Load trang web, nạp html vào document
            await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaDeleteAll();
            Task<HtmlDocument> document;
            int record_per_page = 40;
            int page_index = 0;
            try {
                while (true)
                {
                    page_index++;
                    document = htmlWeb.LoadFromWebAsync("https://dichvucong.gov.vn/p/phananhkiennghi/jsp/pakn_just_answer.jsp?status=1&keyword=&record_per_page=" + record_per_page + "&page_index=" + page_index);

                    var threadItems = document.Result.DocumentNode.Descendants("div")
                        .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "list-pakn")
                        .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "item").ToList();
                    if (threadItems.Count == 0)
                    {
                        break;
                    }
                    foreach (var item in threadItems)
                    {
                        var objectAdd = new DichViCongQuocGia();
                        var linkNode = item.Descendants("h3").First()
                            .ChildNodes.FirstOrDefault();
                        var id = linkNode.Attributes["href"].Value.Split("?")[1].Split("=")[1];
                        objectAdd.Question = linkNode.InnerText.ToString();

                        Task<HtmlDocument> documentDetail = htmlWeb.LoadFromWebAsync("https://dichvucong.gov.vn/p/phananhkiennghi/jsp/pakn-detail.jsp?id=" + id);


                        string[] info = documentDetail.Result.DocumentNode.Descendants("div")
                            .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "post-info").InnerHtml.Trim().Split("-");
                        objectAdd.Questioner = info[0];
                        objectAdd.CreatedDate = info[1];
                        objectAdd.QuestionContent = documentDetail.Result.DocumentNode.Descendants("textarea")
                            .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "showContent").InnerHtml;
                        objectAdd.Status = "Đã trả lời";
                        Task<HtmlDocument> response = htmlWeb.LoadFromWebAsync("https://dichvucong.gov.vn/p/phananhkiennghi/jsp/pakn_answer_byid.jsp?pakn_id=" + id);
                        objectAdd.Reply = response.Result.DocumentNode.Descendants("textarea")
                            .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "showContent").InnerHtml;
                        objectAdd.ObjectId = Convert.ToInt32(id);
                        // lưu database
                        await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaInsert(objectAdd);

                        var elementFiles = response.Result.GetElementbyId("list-file-attach");
                        if (elementFiles == null) { continue; };

                        var files = elementFiles.ChildNodes.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "content")
                            .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "link").ToList();
                        foreach (var file in files)
                        {
                            // lưu file
                            string folder = "Upload\\DichVuCongQuocGia\\" + id;
                            string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                            if (!Directory.Exists(folderPath))
                            {
                                Directory.CreateDirectory(folderPath);
                            }

                            using (WebClient webClient = new WebClient())
                            {
                                string[] s = file.InnerText.Split(".");
                                fileInsert.ObjectId = Convert.ToInt32(id);
                                fileInsert.Type = GetFileTypes.GetFileTypeExtension("." + s.FirstOrDefault(x => x == s[s.Length - 1]).ToString().ToLower());
                                fileInsert.FileName = Path.GetFileName(file.InnerText).Replace("+", "");
                                fileInsert.FilePath = Path.Combine(folder, fileInsert.FileName);
                                webClient.DownloadFileAsync(new Uri("https://dichvucong.gov.vn/" + file.Attributes["href"].Value), Path.Combine(folderPath, fileInsert.FileName));
                                await new MR_SyncFileAttach(_appSetting).MR_Sync_DichVuCongQuocGiaFileAttachInsertDAO(fileInsert);
                            }

                        }

                    }
                }
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi
                {
                    Success = ResultCode.OK
                };
            }
            catch (Exception e) {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, e);
                return new ResultApi
                {
                    Success = ResultCode.ORROR
                };
            }
        }

        [Route("SyncHopThuGopYKhanhHoa")]
        [HttpGet]
        //[AllowAnonymous]
        public ActionResult<object> SyncHopThuGopYKhanhHoa()
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };//Load trang web, nạp html vào document
            try
            {
                HtmlDocument document = htmlWeb.Load("https://thongtin.hanhchinhcong.khanhhoa.gov.vn/module/hop-thu-gop-y");

                var items = new List<GopYKienNghi>();
                var threadItems = document.DocumentNode.Descendants("div")
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
                    objectAdd.CreatedDate = "09/10/2018 | 10:02-AM";//item.Descendants("span").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedday")).InnerText;
                    HtmlDocument documentDetail = htmlWeb.Load("https://thongtin.hanhchinhcong.khanhhoa.gov.vn/" + link);

                    var threadItemsChild = documentDetail.DocumentNode.Descendants("div")
                    .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "print-chitiet");
                    objectAdd.QuestionContent = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("chitietbaiviet")).InnerHtml;
                    objectAdd.Reply = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedback-traloi-content")).InnerHtml.Trim();
                    items.Add(objectAdd);


                }
                new RecommendationDAO(_appSetting).SyncHopThuGopYKhanhHoa(items);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new ResultApi
                {
                    Success = ResultCode.OK
                };
            }
            catch (Exception ex) {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new ResultApi
                {
                    Success = ResultCode.ORROR
                };
            }
            
        }

        [Route("SyncQuanLyKienNghiCuTri")]
        [HttpGet]
        public async Task<ActionResult<object>> SyncQuanLyKienNghiCuTriAsync()
        {
            try
            {
                var fieldKNCT = SyncFieldKienNghiCuTriAsync();
                if (fieldKNCT.Result.Count <= 0) {
                    return new ResultApi
                    {
                        Success = ResultCode.ORROR,
                        Message = "error while clone fieldKNCT"
                    };
                }
                var results = new HttpResponseMessage();
                /// header
                var TkeyHeader = new List<KeyValuePair<string, string>>();
                //TkeyHeader.Add(new KeyValuePair<string, string>("Accept-Language", lang));
                //TkeyHeader.Add(new KeyValuePair<string, string>("Authorization"," Bearer " + Access_Token.Access_Token));
                HeaderRess header = new HeaderRess();
                header.Tkey = TkeyHeader;
                header.ContentType = "application/json";
                var request = new RequestKienNghiCuTri();
                request.PageIndex = 0;
                string jsonData = "";
                ResponseKienNghiCuTri model = new ResponseKienNghiCuTri();
                // xóa hết
                await new MR_CuTriTinhKhanhHoa(_appSetting).MR_Sync_CuTriTinhKhanhHoaDeleteAllDAO();
                // xóa file
                //string[] files = Directory.GetFiles("Upload\\KienNghiCuTri\\");
                //foreach (string file in files)
                //{
                //    Directory.Delete(file);
                //}

                while (true) {
                    request.PageIndex++;
                    jsonData = JsonConvert.SerializeObject(request);
                    results = PostData("https://kiennghicutri.khanhhoa.gov.vn:96", "/api/RequestOut/PublishUserSearch", header, jsonData);
                    if (results.StatusCode == HttpStatusCode.OK)
                    {
                        model = JsonConvert.DeserializeObject<ResponseKienNghiCuTri>(results.Content.ReadAsStringAsync().Result);
                        if (model.dataGrid.Count > 0)
                        {
                            // insert data base
                            foreach (var item in model.dataGrid) {
                                MR_CuTriTinhKhanhHoa modelInsert = new MR_CuTriTinhKhanhHoa();
                                modelInsert.ElectorId = item.Id;
                                modelInsert.Content = item.noiDungKienNghi;
                                modelInsert.Result = item.ketQua;
                                modelInsert.Status = item.trangThai;
                                modelInsert.CategoryName = item.phanLoai;
                                var filed = fieldKNCT.Result.FirstOrDefault(x => x.text == item.linhVuc);
                                if (filed != null) {
                                    modelInsert.FieldId = filed.value;
                                }
                                modelInsert.RecommendationPlace = item.noiCoKienNghi;
                                modelInsert.Term = item.nhiemKy;
                                modelInsert.UserProcess = item.tenCaNhanDonVi;
                                modelInsert.UnitPreside = item.coQuanChuTri;
                                modelInsert.UnitCombination = item.coQuanPhoiHop;
                                modelInsert.Progress = item.tienDoGiaiQuyet;
                                modelInsert.Response = item.noiDungTraLoi;
                                modelInsert.EndDate = item.ngayKetThuc == null ? null : item.ngayKetThuc;

                                await new MR_CuTriTinhKhanhHoa(_appSetting).MR_Sync_CuTriTinhKhanhHoaInsertDAO(modelInsert);
                                if (item.tepDinhKem.Count > 0) {
                                    MR_SyncFileAttach fileInsert = new MR_SyncFileAttach();
                                    foreach (var file in item.tepDinhKem) {
                                        // lưu file
                                        string folder = "Upload\\KienNghiCuTri\\" + item.Id;
                                        string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                                        if (!Directory.Exists(folderPath))
                                        {
                                            Directory.CreateDirectory(folderPath);
                                        }
                                        using (WebClient webClient = new WebClient()) {
                                            string[] s = file.name.Split(".");
                                            fileInsert.ObjectId = item.Id;
                                            fileInsert.Type = GetFileTypes.GetFileTypeExtension("."+ s.FirstOrDefault(x=>x == s[s.Length -1]).ToString());
                                            fileInsert.FileName = Path.GetFileName(file.name).Replace("+", "");
                                            fileInsert.FilePath = Path.Combine(folder, fileInsert.FileName);
                                            webClient.DownloadFileAsync(new Uri("https://kiennghicutri.khanhhoa.gov.vn:96/" + file.duongDan), Path.Combine(folderPath, fileInsert.FileName));
                                            await new MR_SyncFileAttach(_appSetting).MR_Sync_CuTriTinhKhanhHoaFileAttachInsertDAO(fileInsert);
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            break;
                        }
                    }
                    else {
                        return new ResultApi
                        {
                            Success = ResultCode.ORROR,
                            Message = "Status code : " + results.StatusCode.ToString()
                        };
                    }
                }
                
                return new ResultApi
                {
                    Success = ResultCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ResultApi
                {
                    Success = ResultCode.ORROR,
                    Message = ex.Message
                };
            }
        }

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

    }
}
