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

    }

    public class ExportHisUser
    {
        public int UserId { get; set; }

    }
    public class ExportUserByUnit
    {
        public string UnitName { get; set; }
        public int UnitId { get; set; }

        

    }
}
