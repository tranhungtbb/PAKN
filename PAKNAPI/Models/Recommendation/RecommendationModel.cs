using Microsoft.AspNetCore.Http;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.Recommendation
{

    public class RecommendationGetDataForCreateResponse
    {
        public string Code { get; set; }
        public List<DropdownTree> lstUnit { get; set; }
        public List<DropdownObject> lstField { get; set; }
        public List<DropdownObject> lstIndividual { get; set; }
        public List<DropdownObject> lstBusiness { get; set; }
        public List<DropdownObject> lstHashTag { get; set; }
    }

    public class RecommendationGetDataForForwardResponse
    {
        public List<DropdownObject> lstUnitNotMain { get; set; }
    }

    public class RecommendationGetDataForProcessResponse
    {
        public List<DropdownObject> lstHashtag { get; set; }
        public List<DropdownObject> lstUsers { get; set; }
        public List<DropdownObject> lstGroupWord { get; set; }
        public List<DropdownObject> lstUsersProcess { get; set; }
    }
    public class RecommendationInsertRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationInsertIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class RecommendationUpdateRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        public MRRecommendationUpdateIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public List<MRRecommendationFiles> LstXoaFile { get; set; }
        public IFormFileCollection Files { get; set; }
    }
    public class RecommendationGetByIDResponse
    {
        public MRRecommendationGetByID Model { get; set; }
        public List<MRRecommendationHashtagGetByRecommendationId> lstHashtag { get; set; }
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
    }
    public class RecommendationGetByIDViewResponse
    {
        public MRRecommendationGetByIDView Model { get; set; }
        public MRRecommendationConclusionGetByRecommendationId ModelConclusion { get; set; }
        public List<MRRecommendationHashtagGetByRecommendationId> lstHashtag { get; set; }
        public List<MRRecommendationFilesGetByRecommendationId> lstFiles { get; set; }
        public List<MRRecommendationConclusionFilesGetByConclusionId> filesConclusion { get; set; }

        public List<MRRecommendationGetDenyContentsBase> denyContent { get; set; }
    }

    public class RecommendationForwardRequest
    {
        public MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public byte RecommendationStatus { get; set; }
        public bool IsList { get; set; }
    }
    public class RecommendationForwardProcess
    {
        public MRRecommendationForwardProcessIN _mRRecommendationForwardProcessIN { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        public byte RecommendationStatus { get; set; }
        public bool? ReactionaryWord { get; set; }
        public string ListGroupWordSelected { get; set; }
        public bool IsList { get; set; }
        public bool? IsForwardProcess { get; set; }
    }
    public class RecommendationOnProcessConclusionProcess
    {
        public MRRecommendationConclusionInsertIN DataConclusion { get; set; }
        public IFormFileCollection Files { get; set; }
        public byte RecommendationStatus { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
    }

    public class RecommendationSendProcess
    {
        public int? id { get; set; }
        public byte? status { get; set; }
    }
    public class GopYKienNghi
    {
        public int Id { get; set; }
        public string Questioner { get; set; }
        public string Question { get; set; }
        public string QuestionContent { get; set; }
        public string Reply { get; set; }
        public string CreatedDate { get; set; }
        public string ReplyDate { get; set; }
    }

    public class DichViCongQuocGia
    {
        public int Id { get; set; }
        public string Questioner { get; set; }
        public string Question { get; set; }
        public string QuestionContent { get; set; }
        public string Reply { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
        public int ObjectId { get; set; }
        public int? TotalCount { get; set; }

    }

    public class RequestKienNghiCuTri
    {
        private bool isSuperAdmin = false;
        private bool isTongHop = false;
        private string keyWord = "";
        private int tinhTrangGiaiQuyet = -1;
        private int trangThai = 3;
        private int pageSize = 40;
        public int AccountId { get; set; }
        public int DeparmentId { get; set; }
        public string DiaPhuong { get; set; }
        public string DonVi { get; set; }
        public string IpAddress { get; set; }
        public bool IsSuperAdmin
        {
            get { return this.isSuperAdmin; }
            set { this.isSuperAdmin = value; }
        }
        public bool IsTongHop
        {
            get { return this.isTongHop; }
            set { this.isTongHop = value; }
        }
        public string KeyWord
        {
            get { return this.keyWord; }
            set { this.keyWord = value; }
        }
        public string LinhVucId { get; set; }
        public string Nguon { get; set; }
        public string NhiemKy { get; set; }
        public int PageIndex { get; set; }
        public int PageSize
        {
            get { return this.pageSize; }
            set { this.pageSize = value; }
        }
        public string PhanLoai { get; set; }
        public string PhuongXaId { get; set; }
        public int TinhTrangGiaiQuyet
        {
            get { return this.tinhTrangGiaiQuyet; }
            set { this.tinhTrangGiaiQuyet = value; }
        }
        public int TrangThai
        {
            get { return this.trangThai; }
            set { this.trangThai = value; }
        }
        public string UnitId { get; set; }
        public string UserId { get; set; }
    }

    public class ResponseKienNghiCuTri
    {
        public List<KienNghiCuTriObject> dataGrid = new List<KienNghiCuTriObject>();
        public string message { get; set; }
        public int status { get; set; }

    }

    public class KienNghiCuTriObject
    {
        public int Id { get; set; }
        public string noiDungKienNghi { get; set; }
        public string ketQua { get; set; }
        public int trangThai { get; set; }
        public string phanLoai { get; set; }
        public string linhVuc { get; set; }
        public string noiCoKienNghi { get; set; }
        public string nhiemKy { get; set; }
        public string tenCaNhanDonVi { get; set; }
        public string coQuanChuTri { get; set; }
        public string coQuanPhoiHop { get; set; }
        public string tienDoGiaiQuyet { get; set; }
        public DateTime? ngayKetThuc { get; set; }
        public string noiDungTraLoi { get; set; }
        public int total { get; set; }

        public List<FileAttackForKienNghiCuTri> tepDinhKem = new List<FileAttackForKienNghiCuTri>();
    }
    public class FileAttackForKienNghiCuTri
    {
        public string duongDan { get; set; }
        public string name { get; set; }
    }

    public class ResponseFieldKienNghiCuTri {
        public List<ResponseDropdownKienNghiCuTri> lstLinhVuc = new List<ResponseDropdownKienNghiCuTri>();
    }

    public class ResponseDropdownKienNghiCuTri {
        public int value { get; set; }
        public string text { get; set; }
        public int code { get; set; }
    }
}