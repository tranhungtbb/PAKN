using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{

    /// <example>
    ///{
    ///     "id" : 210988,
    ///     "userName": "123123123",
    ///     "fullName": "HungTĐ",
    ///     "email": "tran@gmail.com",
    ///     "phone": "0988234234",
    ///     "address": "Hoàng Quốc Việt",
    ///     "idCard": "123123123",
    ///     "gender": true,
    ///}
    /// </example>



    public class AccountInfoModel
    {
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ tên không được để trống")]
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get;set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại không đúng định dạng")]

        public string Phone { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Quốc tịch không được để trống")]
        public string Nation { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardsId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ không được để trống")]
        public string Address { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số CMND/CCCD/Hộ chiếu không được để trống")]
        [RegularExpression(ConstantRegex.CMT, ErrorMessage = "Số CMND/CCCD/Hộ chiếu không đúng định dạng")]

        public string IdCard { get; set; }
        public string IssuedPlace { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Ngày cấp không đúng định dạng")]
        public DateTime? IssuedDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Giới tính không được để trống")]
        public bool? Gender { get; set; }
    }

    /// <example>
    /// {
    ///     "id": 120165,
    ///     "representativeName": "Hùng TĐ",
    ///     "isActived": true,
    ///     "isDeleted": false,
    ///     "orgPhone": "",
    ///     "orgEmail": "trand@gmail.com",
    ///     "representativeBirthDay": "",
    ///     "createdBy": 0,
    ///     "updatedBy": 0,
    ///     "status": 1,
    ///     "representativeGender": true,
    ///     "businessRegistration": "1230128123",
    ///     "address": "",
    ///     "email": "",
    ///     "phone": "0983242342",
    ///     "nation": "Việt Nam",
    ///     "business": "Công ty b",
    ///     "fullName": "Công ty b",
    ///}
    /// </example>


public class BusinessAccountInfoModel
    {
        public BusinessAccountInfoModel() { }
        public BusinessAccountInfoModel(BIBusinessGetByUserId entity)
        {
            DistrictId = entity.DistrictId;
            RepresentativeName = entity.RepresentativeName;
            Code = entity.Code;
            IsActived = (bool)entity.IsActived;
            IsDeleted = (bool)entity.IsDeleted;
            Id = (long)entity.Id;

            RepresentativeBirthDay = entity.RepresentativeBirthDay;
            ProvinceId = entity.ProvinceId;
            Status = entity.Status;
            RepresentativeGender = entity.RepresentativeGender;
            DateOfIssue = entity.DateOfIssue;
            Address = entity.Address;
            Email = entity.Email;
            Phone = entity.Phone;
            Representative = entity.Representative;
            IDCard = entity.IDCard;
            Place = entity.Place;
            NativePlace = entity.NativePlace;
            PermanentPlace = entity.PermanentPlace;
            Nation = entity.Nation;
            BusinessRegistration = entity.BusinessRegistration;
            DecisionOfEstablishing = entity.DecisionOfEstablishing;
            Tax = entity.Tax;
            OrgDistrictId = entity.OrgDistrictId;
            OrgProvinceId = entity.OrgProvinceId;
            OrgWardsId = entity.OrgWardsId;
            OrgAddress = entity.OrgAddress;
            OrgPhone = entity.OrgPhone;
            OrgEmail = entity.OrgEmail;
            Business = entity.Business;
        }

        public string UserName { get; set; }
        public string FullName { get; set; }
        public int? WardsId { get; set; }
        public int? DistrictId { get; set; }
        public int? ProvinceId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên người đại diện không được để trống")]
        public string RepresentativeName { get; set; }
        public string Code { get; set; }

        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public long Id { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Ngày sinh không đúng định dạng")]
        public DateTime? RepresentativeBirthDay { get; set; }
        public int? Status { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Giới tính không được để trống")]
        public bool? RepresentativeGender { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Ngày thành lập không đúng định dạng")]
        public DateTime? DateOfIssue { get; set; }
        public string Address { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail người đại diện không đúng định dạng")]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string Representative { get; set; }
        public string IDCard { get; set; }
        public string Place { get; set; }
        public string NativePlace { get; set; }
        public string PermanentPlace { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Quốc tịch không được để trống")]
        public string Nation { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Số chứng nhận đăng ký kinh doanh không được để trống")]
        public string BusinessRegistration { get; set; }
        public string DecisionOfEstablishing { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Mã số thuế không được để trống")]
        //[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Mã số thuế không đúng định dạng")]
        //[StringLength(13, ErrorMessage = "Mã số thuế không vượt quá 13 kí tự")]
        public string Tax { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Tỉnh/Thành phố văn phòng đại diện không được để trống")]
        //[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Tỉnh/Thành phố văn phòng đại diện không đúng định dạng")]
        public int? OrgProvinceId { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Quận/Huyện/Thị xã văn phòng đại diện không được để trống")]
        //[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Quận/Huyện/Thị xã văn phòng đại diện không đúng định dạng")]
        public int? OrgDistrictId { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Xã phường/Thị trấn văn phòng đại diện không được để trống")]
        //[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Xã phường/Thị trấn văn phòng đại diện không đúng định dạng")]
        public int? OrgWardsId { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ văn phòng đại diện không được để trống")]
        public string OrgAddress { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại văn phòng đại diện không được để trống")]
        //[RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại văn phòng đại diện không đúng định dạng")]
        public string OrgPhone { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "E-mail văn phòng đại diện không được để trống")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail văn phòng đại diện không đúng định dạng")]
        public string OrgEmail { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên tổ chức, doanh nghiệp không được để trống")]
        public string Business { get; set; }
    }

    
}
