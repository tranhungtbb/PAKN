using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static PAKNAPI.Common.ClientRestfull;

namespace PAKNAPI.Models.AdministrativeSync
{
    public class AdministrativeSync
    {
        private readonly IAppSetting _appSetting;

        public AdministrativeSync(IAppSetting appSetting)
        {
            _appSetting = appSetting;

        }
        // function asyn thủ tục hành chính
        // get list
        public async Task<ActionResult<object>> SyncThuTucHanhChinh()
        {
            try
            {
                // tìm trong datbase object có id nhỏ nhất

                var results = new HttpResponseMessage();
                var TkeyHeader = new List<KeyValuePair<string, string>>();
                HeaderRess header = new HeaderRess();
                header.Tkey = TkeyHeader;
                header.ContentType = "application/json";
                var request = new RequestAdministrative();

                ResponseListAdministrative model = new ResponseListAdministrative();
                DAMAdministrationGetById objectTop = await new DAMAdministrationGetById(_appSetting).DAMAdministrationGetTopOrderByAdministrativeId();
                while (true)
                {
                    request.PageIndex++;
                    results = GetStringAsync("https://tthckhapi.azurewebsites.net",
                        "/api/v1/ThuTucs?expand=&filter=+1%3D1++AND+TinhTrangId%3D3&sort=Id+Desc&pageSize=20&page=" + request.PageIndex, header);
                    if (results.StatusCode == HttpStatusCode.OK)
                    {
                        model = JsonConvert.DeserializeObject<ResponseListAdministrative>(results.Content.ReadAsStringAsync().Result);
                        if (model.ThuTucs.Count > 0)
                        {
                            if ((objectTop != null && model.ThuTucs.Exists(x => x.Id > objectTop.AdministrationId)) || objectTop == null)
                            {
                                foreach (var item in model.ThuTucs)
                                {
                                    if (objectTop != null && item.Id <= objectTop.AdministrationId)
                                    {
                                        continue;
                                    }
                                    // insert data base
                                    await SyncDetailThuTucHanhChinh(item.Id);
                                }
                            }
                            else
                            {
                                break;
                            }

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

        private async Task<bool> SyncDetailThuTucHanhChinh(int Id)
        {
            try
            {
                var results = new HttpResponseMessage();
                var TkeyHeader = new List<KeyValuePair<string, string>>();
                HeaderRess header = new HeaderRess();
                header.Tkey = TkeyHeader;
                header.ContentType = "application/json";

                DetailAdministrative obj = new DetailAdministrative();

                while (true)
                {
                    results = GetStringAsync("https://tthckhapi.azurewebsites.net",
                        "/api/v1/ThuTucs/" + Id, header);
                    if (results.StatusCode == HttpStatusCode.OK)
                    {
                        obj = JsonConvert.DeserializeObject<DetailAdministrative>(results.Content.ReadAsStringAsync().Result);

                        DAMAdministrationInsertIN damAdministrationInsertIN = new DAMAdministrationInsertIN();
                        damAdministrationInsertIN.Name = obj.Ten;
                        damAdministrationInsertIN.Code = obj.Ma;
                        damAdministrationInsertIN.CountryCode = obj.MaQuanLy_QuocGia;
                        damAdministrationInsertIN.Field = obj.LinhVucId;
                        damAdministrationInsertIN.UnitReceive = obj.DonViTiepNhans.Count > 0 ? obj.DonViTiepNhans.FirstOrDefault().Id : -1;
                        
                        //damAdministrationInsertIN.RankReceiveId = obj.TenCapTiepNhan;
                        damAdministrationInsertIN.Lever = obj.MucDo;
                        damAdministrationInsertIN.TypeSend = obj.NopQuaBuuChinh;
                        damAdministrationInsertIN.FileNum = String.IsNullOrEmpty(obj.SoBoHoSo) ? "" : obj.SoBoHoSo;
                        damAdministrationInsertIN.AmountTime = obj.ThoiHanGiaiQuyet.ToString();
                        damAdministrationInsertIN.Proceed = String.IsNullOrEmpty(obj.CachThucThucHien) ? "" : obj.CachThucThucHien;
                        damAdministrationInsertIN.Object = String.IsNullOrEmpty(obj.DoiTuongThucHien) ? "" : obj.DoiTuongThucHien;
                        damAdministrationInsertIN.Organization = String.IsNullOrEmpty(obj.CoQuanGiaiQuyet) ? "" : obj.CoQuanGiaiQuyet;
                        damAdministrationInsertIN.OrganizationDecision = String.IsNullOrEmpty(obj.CoQuanCoThamQuyenQuyetDinh) ? "" : obj.CoQuanCoThamQuyenQuyetDinh;
                        damAdministrationInsertIN.Address = String.IsNullOrEmpty(obj.DiaChiTiepNhanHoSo) ? "" : obj.DiaChiTiepNhanHoSo;
                        damAdministrationInsertIN.OrganizationAuthor = String.IsNullOrEmpty(obj.CoQuanDuocUyQuyen) ? "" : obj.CoQuanDuocUyQuyen;
                        damAdministrationInsertIN.OrganizationCombine = String.IsNullOrEmpty(obj.CoQuanPhoiHop) ? "" : obj.CoQuanPhoiHop;
                        damAdministrationInsertIN.Result = String.IsNullOrEmpty(obj.KetQuaThucHien) ? "" : obj.KetQuaThucHien;
                        damAdministrationInsertIN.LegalGrounds = String.IsNullOrEmpty(obj.CanCuPhapLy) ? "" : obj.CanCuPhapLy;
                        damAdministrationInsertIN.Request = String.IsNullOrEmpty(obj.YeuCauDieuKien) ? "" : obj.YeuCauDieuKien;
                        damAdministrationInsertIN.ImpactAssessment = "";
                        damAdministrationInsertIN.Note = String.IsNullOrEmpty(obj.LuuY) ? "" : obj.LuuY;
                        damAdministrationInsertIN.Status = Convert.ToByte(obj.TinhTrangId);
                        damAdministrationInsertIN.IsShow = true;
                        damAdministrationInsertIN.AdministrationId = obj.Id;
                        await new DAMAdministrationInsert(_appSetting).DAMAdministrationInsertDAO(damAdministrationInsertIN);

                        foreach (var compositionProfile in obj.HoSoes)
                        {
                            DAMCompositionProfileCreateIN cp = new DAMCompositionProfileCreateIN();
                            cp.AdministrationId = obj.Id;
                            cp.NameExhibit = compositionProfile.Ten;
                            cp.OriginalForm = compositionProfile.BanChinh;
                            cp.CopyForm = compositionProfile.BanSao;
                            cp.IsBind = compositionProfile.BatBuoc;
                            cp.CompositionProfileId = compositionProfile.Id;
                            await new DAMCompositionProfileCreate(_appSetting).DAMCompositionProfileCreateDAO(cp);
                            if (compositionProfile.MauDonToKhai != null)
                            {
                                foreach (var cpFile in compositionProfile.MauDonToKhai.FileTepMauDonToKhais)
                                {
                                    DAMCompositionProfileFileFilesInsertIN cpFileItem = new DAMCompositionProfileFileFilesInsertIN();
                                    cpFileItem.CompositionProfileId = compositionProfile.Id;
                                    cpFileItem.FileAttach = cpFile.Path;
                                    cpFileItem.FileType = (short?)cpFile.Loai;
                                    cpFileItem.Name = cpFile.FileName;
                                    await new DAMCompositionProfileFileFilesInsert(_appSetting).DAMCompositionProfileFileFilesInsertDAO(cpFileItem);
                                }
                            }
                        }
                        foreach (var implement in obj.CacBuocThucHiens)
                        {
                            DAMImplementationProcessCreateIN item = new DAMImplementationProcessCreateIN();
                            item.AdministrationId = obj.Id;
                            item.Name = implement.TenBuoc;
                            item.Result = implement.KetQua;
                            item.Time = implement.ThoiGian.ToString();
                            item.Unit = implement.TrachNhiem;
                            await new DAMImplementationProcessCreate(_appSetting).DAMImplementationProcessCreateDAO(item);
                        }
                        foreach (var fee in obj.PhiLePhis)
                        {
                            DAMChargesCreateIN iFee = new DAMChargesCreateIN();
                            iFee.AdministrationId = obj.Id;
                            iFee.Charges = fee.MucPhi.ToString();
                            iFee.Description = fee.MoTa;
                            iFee.ChargesId = fee.Id;
                            await new DAMChargesCreate(_appSetting).DAMChargesCreateDAO(iFee);
                        }
                        break;
                    }
                    else
                    {
                        return false;
                    }
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
