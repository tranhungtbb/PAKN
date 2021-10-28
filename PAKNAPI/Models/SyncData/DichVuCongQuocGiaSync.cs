using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PAKNAPI.Models.SyncData
{
    public class DichVuCongQuocGiaSync
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public DichVuCongQuocGiaSync(IAppSetting appSetting, IWebHostEnvironment hostingEnvironment)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
        }

        // function sync

        public async Task<ActionResult<object>> SyncDichVuCongQuocGia()
        {
            // lấy totalCount
            //var totalCount = await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaGetTotalCount();
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };
            MR_SyncFileAttach fileInsert = new MR_SyncFileAttach();
            //Load trang web, nạp html vào document
            //await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaDeleteAll();
            Task<HtmlDocument> document;
            int record_per_page = 40;
            int page_index = 1;

            //if (totalCount != 0)
            //{
            //    page_index = Convert.ToInt32(Math.Floor(Convert.ToDecimal(totalCount / record_per_page)));
            //    page_index = page_index < 0 ? 0 : page_index;
            //}
            //else
            //{
            //    page_index = 0;
            //}

            try
            {
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

                        var checkObject = await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaGetByObjecId(Convert.ToInt32(id));
                        if (checkObject.Count > 0)
                        {
                            goto End;
                        }

                        objectAdd.Question = linkNode.InnerText.ToString();

                        Task <HtmlDocument> documentDetail = htmlWeb.LoadFromWebAsync("https://dichvucong.gov.vn/p/phananhkiennghi/jsp/pakn-detail.jsp?id=" + id, Encoding.UTF8);


                        string[] info = documentDetail.Result.DocumentNode.Descendants("div")
                            .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "post-info").InnerHtml.Trim().Split("-");

                        objectAdd.Questioner = HttpUtility.HtmlDecode(info[0]);
                        objectAdd.CreatedDate = info[info.Length - 1];
                        objectAdd.QuestionContent = documentDetail.Result.DocumentNode.Descendants("textarea")
                            .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "showContent").InnerHtml;
                        objectAdd.Status = "Đã trả lời";
                        Task<HtmlDocument> response = htmlWeb.LoadFromWebAsync("https://dichvucong.gov.vn/p/phananhkiennghi/jsp/pakn_answer_byid.jsp?pakn_id=" + id, Encoding.UTF8);
                        var reply = response.Result.DocumentNode.Descendants("textarea").FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "showContent");
                        objectAdd.Reply = reply == null ? "" : reply.InnerHtml;

                        objectAdd.ObjectId = Convert.ToInt32(id);
                        // lưu database
                        await new RecommendationDAO(_appSetting).SyncDichVuCongQuocGiaInsert(objectAdd);

                        // file request

                        var elementFilesRequest = documentDetail.Result.DocumentNode.Descendants("div")
                            .FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "file");
                        if (elementFilesRequest != null)
                        {
                            // insert file
                            var filesRequest = elementFilesRequest.ChildNodes.FirstOrDefault(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "content")
                                .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "link").ToList();
                            foreach (var file in filesRequest)
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
                                    if (s.Length > 1)
                                    {
                                        fileInsert.Type = GetFileTypes.GetFileTypeExtension("." + s.FirstOrDefault(x => x == s[s.Length - 1]).ToString().ToLower());
                                    }
                                    else
                                    {
                                        fileInsert.Type = 4;
                                    }
                                    fileInsert.Type = GetFileTypes.GetFileTypeExtension("." + s.FirstOrDefault(x => x == s[s.Length - 1]).ToString().ToLower());
                                    fileInsert.FileName = HttpUtility.HtmlDecode(Path.GetFileName(file.InnerText).Replace("+", ""));
                                    fileInsert.FilePath = Path.Combine(folder, Path.GetFileName(file.InnerText).Replace("+", ""));
                                    fileInsert.IsReply = false;
                                    webClient.DownloadFileAsync(new Uri("https://dichvucong.gov.vn/" + file.Attributes["href"].Value), Path.Combine(folderPath, fileInsert.FileName));
                                    await new MR_SyncFileAttach(_appSetting).MR_Sync_DichVuCongQuocGiaFileAttachInsertDAO(fileInsert);
                                }

                            }
                        }


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
                                if (s.Length > 1)
                                {
                                    fileInsert.Type = GetFileTypes.GetFileTypeExtension("." + s.FirstOrDefault(x => x == s[s.Length - 1]).ToString().ToLower());
                                }
                                else
                                {
                                    fileInsert.Type = 4;
                                }

                                fileInsert.FileName = HttpUtility.HtmlDecode(Path.GetFileName(file.InnerText).Replace("+", ""));
                                fileInsert.FilePath = Path.Combine(folder, fileInsert.FileName);
                                fileInsert.IsReply = true;
                                webClient.DownloadFileAsync(new Uri("https://dichvucong.gov.vn/" + file.Attributes["href"].Value), Path.Combine(folderPath, fileInsert.FileName));
                                await new MR_SyncFileAttach(_appSetting).MR_Sync_DichVuCongQuocGiaFileAttachInsertDAO(fileInsert);
                            }

                        }

                    }
                }
                End:
                return new ResultApi
                {
                    Success = ResultCode.OK
                };
            }
            catch (Exception e)
            {
                return new ResultApi
                {
                    Success = ResultCode.ORROR
                };
            }

        }
    }
}
