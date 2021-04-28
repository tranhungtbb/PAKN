using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class ExportRecommendation
    {
        public string Code{get;set;}
        public string SendName{get;set;}
        public string Content{get;set;}
        public int? UnitId{get;set;}
        public int? Field{get;set;}
        public int? Status { get; set; }
        public int? UnitProcessId { get; set; }
        public long? UserProcessId { get; set; }
    }
}
