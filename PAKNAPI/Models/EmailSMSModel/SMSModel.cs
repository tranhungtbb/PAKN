using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.EmailSMSModel
{
    public class SMSInsertModel
    {
        public SMSQuanLyTinNhanInsertIN model { get; set; }

        public List<IndividualBusinessInfo> IndividualBusinessInfo { get; set; }
    }

    public class SMSUpdateModel
    {
        public SMSQuanLyTinNhanGetById model { get; set; }

        public List<SMSGetListIndividualBusinessBySMSId> IndividualBusinessInfo { get; set; }
    }

    public class SMSUpdateRequestModel
    {
        public SMSQuanLyTinNhanUpdateModel model { get; set; }

        public List<IndividualBusinessInfo> IndividualBusinessInfo { get; set; }
    }

    public class IndividualBusinessInfo {
        public int Id { get; set; }
        public int Category { get; set; }

        public int AdmintrativeUnitId { get; set; }
    }

    public class SMSQuanLyTinNhanUpdateModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Signature { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UserCreateId { get; set; }
        public DateTime? SendDate { get; set; }
        public int? UserSend { get; set; }
        public int? Status { get; set; }
        public string Type { get; set; }
    }
}
