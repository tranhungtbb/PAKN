using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class ExportRecommendation
    {
        public string TitleReport { get; set; }
        public string Code{get;set;}
        public string SendName{get;set;}
        public string Content{get;set;}
        public int? UnitId{get;set;}
        public int? Field{get;set;}
        public int? Status { get; set; }
        public int? UnitProcessId { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }
    }

    public class ExportIndividual
    {
        public string TitleReport { get; set; } 
        public string Address { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int? Status { get; set; }
        public string RowNumber { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }

    }

    public class ExportBusiness
    {
        public string TitleReport { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string RepresentativeName { get; set; }
        public string Phone { get; set; }
        public int? Status { get; set; }
        public string RowNumber { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }

    }

    public class Statistic_Recommendation_ByGroupWord
    {
        public string Code { get; set; }
        public string SendName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? UnitId { get; set; }
        public int? GroupWordId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }

    }

    public class ExportHisUser
    {
        public int UserId { get; set; }
        public int? UserProcessId { get; set; }
        public string UserProcessName { get; set; }

        public string Content { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Status { get; set; }

    }
    public class ExportUserByUnit
    {
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }


    }

    public class ExportRecomdationByUnit
    {
        private string _TitleReport = "BÁO CÁO PHẢN ÁNH KIẾN NGHỊ THEO ĐƠN VỊ";
        public string TitleReport { get { return _TitleReport; } set { _TitleReport = value; } }
        public int? pageIndex { get; set; }
        public int? pageSize { get; set; }
        public string ltsUnitId { get; set; }
        public int? year { get; set; }
        public int? Timeline { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }
    }

    public class ExportRecomdationByUnitDetail
    {
        public string TitleReport { get; set; }

        public string Code { get; set; }
        public string CreateName { get; set; }
        public string TitleMR { get; set; }
        public int? Status { get; set; }
        public int? UnitId { get; set; }
        public int? Field { get; set; }
        public int? UnitProcessId { get; set; }
        public int? UserProcessId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string UserProcessName { get; set; }
    }

    public class ExportRecomdationByFields
    {
        private string _TitleReport = "BÁO CÁO PHẢN ÁNH KIẾN NGHỊ THEO LĨNH VỰC";
        public string TitleReport { get { return _TitleReport; } set { _TitleReport = value; } }
        public int? pageIndex { get; set; }
        public int? pageSize { get; set; }
        public string ltsUnitId { get; set; }
        public int? year { get; set; }
        public int? Timeline { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }
    }
    public class ExportRecomdationByFieldDetail
    {
        public string TitleReport { get; set; }
        public string Code { get; set; }
        public string SendName { get; set; }
        public string Title { get; set; }
        public int? Status { get; set; }
        public string UnitProcessId { get; set; }
        public int UserProcessId { get; set; }
        public string LstUnitId { get; set; }
        public int Field { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string UserProcessName { get; set; }
    }
    public class ExportUserReadedInvitationGetList
    {
        public string InvitationId { get; set; }
        public string TitleReport { get; set; }
        public long? UserProcessId { get; set; }
        public string UserProcessName { get; set; }
    }
}
