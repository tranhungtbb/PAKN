using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static PAKNAPI.Common.ClientRestfull;

namespace PAKNAPI.Models.SyncData
{
    public class KienNghiCuTriSync
    {
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public KienNghiCuTriSync(IAppSetting appSetting, IWebHostEnvironment hostingEnvironment)
        {
            _appSetting = appSetting;
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task<ActionResult<object>> SyncKienNghiCuTri() {
            try
            {
                var fieldKNCT = await new CAFieldKNCTGetDropdown(_appSetting).CAFieldKNCTGetDropdownDAO();

                // tìm trong datbase object có id nhỏ nhất
                MR_CuTriTinhKhanhHoa objectTop = await new MR_CuTriTinhKhanhHoa(_appSetting).MR_Sync_CuTriTinhKhanhHoaGetTopOrderbyElectorId();

                var results = new HttpResponseMessage();
                /// header
                var TkeyHeader = new List<KeyValuePair<string, string>>();
                HeaderRess header = new HeaderRess();
                header.Tkey = TkeyHeader;
                header.ContentType = "application/json";
                var request = new RequestKienNghiCuTri();

                // chỉ sử dụng khi clone ban đầu, ko phải chạy job
                //if (objectTop != null)
                //{
                //    request.PageIndex = Convert.ToInt32(Math.Floor(Convert.ToDecimal(objectTop.TotalCount / request.PageSize)));
                //    request.PageIndex = request.PageIndex < 0 ? 0 : request.PageIndex;
                //}
                //else {
                //    request.PageIndex = 0;
                //}

                string jsonData = "";
                ResponseKienNghiCuTri model = new ResponseKienNghiCuTri();

                while (true)
                {
                    request.PageIndex++;
                    jsonData = JsonConvert.SerializeObject(request);
                    results = PostData("https://kiennghicutri.khanhhoa.gov.vn:96", "/api/RequestOut/PublishUserSearch", header, jsonData);
                    if (results.StatusCode == HttpStatusCode.OK)
                    {
                        model = JsonConvert.DeserializeObject<ResponseKienNghiCuTri>(results.Content.ReadAsStringAsync().Result);

                        if (model.dataGrid.Count > 0)
                        {
                            if ((objectTop != null && model.dataGrid.Exists(x => x.Id > objectTop.ElectorId && x.trangThai != 1)) || objectTop == null)
                            {
                                foreach (var item in model.dataGrid)
                                {

                                    if (objectTop != null && item.Id <= objectTop.ElectorId && item.trangThai != 1)
                                    {
                                        continue;
                                    }

                                    MR_CuTriTinhKhanhHoa modelInsert = new MR_CuTriTinhKhanhHoa();
                                    modelInsert.ElectorId = item.Id;
                                    modelInsert.Content = item.noiDungKienNghi;
                                    modelInsert.Result = item.ketQua;
                                    modelInsert.Status = item.trangThai;
                                    modelInsert.CategoryName = item.phanLoai;
                                    var filed = fieldKNCT.FirstOrDefault(x => x.Text == item.linhVuc);
                                    if (filed != null)
                                    {
                                        modelInsert.FieldId = filed.Value;
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
                                    if (item.tepDinhKem.Count > 0)
                                    {
                                        MR_SyncFileAttach fileInsert = new MR_SyncFileAttach();
                                        foreach (var file in item.tepDinhKem)
                                        {
                                            // lưu file
                                            string folder = "Upload\\KienNghiCuTri\\" + item.Id;
                                            string folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, folder);
                                            if (!Directory.Exists(folderPath))
                                            {
                                                Directory.CreateDirectory(folderPath);
                                            }
                                            using (WebClient webClient = new WebClient())
                                            {
                                                string[] s = file.name.Split(".");
                                                fileInsert.ObjectId = item.Id;
                                                fileInsert.Type = GetFileTypes.GetFileTypeExtension("." + s.FirstOrDefault(x => x == s[s.Length - 1]).ToString());
                                                fileInsert.FileName = Path.GetFileName(file.name).Replace("+", "");
                                                fileInsert.FilePath = Path.Combine(folder, fileInsert.FileName);
                                                webClient.DownloadFileAsync(new Uri("https://kiennghicutri.khanhhoa.gov.vn:96/" + file.duongDan), Path.Combine(folderPath, fileInsert.FileName));
                                                await new MR_SyncFileAttach(_appSetting).MR_Sync_CuTriTinhKhanhHoaFileAttachInsertDAO(fileInsert);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                            // insert data base

                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
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

    }
}
