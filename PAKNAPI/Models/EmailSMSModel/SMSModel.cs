using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.EmailSMSModel
{
    /// <example>
    /// {
    ///"model": {
    ///    "title": "demo thêm mới sms",
    ///    "content": "nội dung sms",
    ///    "signature": "sign",
    ///    "status": 1,
    ///    "type": "1"
    ///},
    ///"IndividualBusinessInfo": [
    ///    {
    ///        "Id": 130420,
    ///        "Category": 1
    ///    },
    ///    {
    ///    "Id": 170436,
    ///            "Category": 1
    ///        }
    ///    ]
    ///}
	/// </example>
    

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

    /// <example>
    /// {
    ///"model": {
    ///    "id": 12,
    ///    "title": "demo sms",
    ///    "content": "nội dung sms",
    ///    "signature": "sign",
    ///    "status": 1,
    ///    "type": "1"
    ///},
    ///"IndividualBusinessInfo": [
    ///    {
    ///        "Id": 130420,
    ///        "Category": 1
    ///    },
    ///    {
    ///    "Id": 170436,
    ///            "Category": 1
    ///        }
    ///    ]
    ///}
    /// </example>

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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tiêu đề SMS không được để trống")]
        [StringLength(50, ErrorMessage = "Tiêu đề SMS không vượt quá 50 kí tự")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung SMS không được để trống")]
        [StringLength(500, ErrorMessage = "Nội dung SMS không vượt quá 500 kí tự")]
        public string Content { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Chữ kí không được để trống")]
        public string Signature { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? UserCreateId { get; set; }
        public DateTime? SendDate { get; set; }
        public int? UserSend { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
        [Range(0, int.MaxValue, ErrorMessage = "Trạng thái không đúng định dạng")]
        public int? Status { get; set; }
        public int? TeamplateId { get; set; }
        public string Type { get; set; }
    }
}
