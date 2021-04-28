using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models
{
    public class AccountInfoModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get;set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Nation { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardsId { get; set; }
        public string Address { get; set; }
        public string IdCard { get; set; }
        public string IssuedPlace { get; set; }
        public string IssuedDate { get; set; }
        public bool Gender { get; set; }
    }

    public class BusinessAccountInfoModel
    {
        public BusinessAccountInfoModel() { }
        public BusinessAccountInfoModel(BIBusinessGetByUserId entity)
        {
            DistrictId = entity.DistrictId;
            RepresentativeName = entity.RepresentativeName;
            Code = entity.Code;
            IsActived = entity.IsActived;
            IsDeleted= entity.IsDeleted;
            Id= entity.Id;
            RepresentativeBirthDay = entity.RepresentativeBirthDay.ToString("dd/MM/yyyy");
            ProvinceId= entity.ProvinceId;
            Status= entity.Status;
            RepresentativeGender = entity.RepresentativeGender;
            DateOfIssue= entity.DateOfIssue.Value.ToString("dd/MM/yyyy");
            Address= entity.Address;
            Email= entity.Email;
            Phone= entity.Phone;
            Representative= entity.Representative;
            IDCard= entity.IDCard;
            Place= entity.Place;
            NativePlace= entity.NativePlace;
            PermanentPlace= entity.PermanentPlace;
            Nation= entity.Nation;
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
        //public int? WardsId { get; set; }
        public int? DistrictId { get; set; }
        public string RepresentativeName { get; set; }
        public string Code { get; set; }
        public bool IsActived { get; set; }
        public bool IsDeleted { get; set; }
        public long Id { get; set; }
        public string RepresentativeBirthDay { get; set; }
        public int? ProvinceId { get; set; }
        public int? Status { get; set; }
        public bool? RepresentativeGender { get; set; }
        public string DateOfIssue { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Representative { get; set; }
        public string IDCard { get; set; }
        public string Place { get; set; }
        public string NativePlace { get; set; }
        public string PermanentPlace { get; set; }
        public string Nation { get; set; }

        public string BusinessRegistration { get; set; }
        public string DecisionOfEstablishing { get; set; }
        public string Tax { get; set; }
        public int? OrgProvinceId { get; set; }
        public int? OrgDistrictId { get; set; }
        public int? OrgWardsId { get; set; }
        public string OrgAddress { get; set; }
        public string OrgPhone { get; set; }
        public string OrgEmail { get; set; }
        public string Business { get; set; }
    }
}
