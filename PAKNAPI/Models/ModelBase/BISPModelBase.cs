using System;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using PAKNAPI.Common;
using PAKNAPI.Models.Results;
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.ModelBase
{
	public class BIBusinessCheckExists
	{
		private SQLCon _sQLCon;

		public BIBusinessCheckExists(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessCheckExists()
		{
		}

		public bool? Exists { get; set; }
		public string Value { get; set; }

		public async Task<List<BIBusinessCheckExists>> BIBusinessCheckExistsDAO(string Field, string Value, long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Field", Field);
			DP.Add("Value", Value);
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessCheckExists>("BI_Business_CheckExists", DP)).ToList();
		}
	}

	public class BIBusinessGetByUserId
	{
		private SQLCon _sQLCon;

		public BIBusinessGetByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetByUserId()
		{
		}

		public long? Id { get; set; }
		public int? WardsId { get; set; }
		public string WardsName { get; set; }
		public int? DistrictId { get; set; }

		public string DistrictName { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public DateTime? RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
		public string ProvinceName { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		public string BusinessRegistration { get; set; }
		public string DecisionOfEstablishing { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public string Tax { get; set; }
		public int? OrgProvinceId { get; set; }

		public string OrgProvinceName { get; set; }
		public int? OrgDistrictId { get; set; }
		public string OrgWardsName { get; set; }
		public int? OrgWardsId { get; set; }
		public string OrgDistrictName { get; set; }
		public string OrgAddress { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Representative { get; set; }
		public string IDCard { get; set; }
		public string Place { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }
		public string Nation { get; set; }
		public string Business { get; set; }
		public long? UserId { get; set; }

		public async Task<List<BIBusinessGetByUserId>> BIBusinessGetByUserIdDAO(long? UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetByUserId>("BI_BusinessGetByUserId", DP)).ToList();
		}
	}

	public class BIBusinessGetDropdown
	{
		private SQLCon _sQLCon;

		public BIBusinessGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetDropdown()
		{
		}

		public long Value { get; set; }
		public string Text { get; set; }

		public async Task<List<BIBusinessGetDropdown>> BIBusinessGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetDropdown>("BI_BusinessGetDropdown", DP)).ToList();
		}
	}

	public class BIBusinessGetRepresentativeById
	{
		private SQLCon _sQLCon;

		public BIBusinessGetRepresentativeById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetRepresentativeById()
		{
		}

		public int? RowNumber { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public long Id { get; set; }
		public DateTime? RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Representative { get; set; }
		public string IDCard { get; set; }
		public string Place { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }
		public string Nation { get; set; }

		public async Task<List<BIBusinessGetRepresentativeById>> BIBusinessGetRepresentativeByIdDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetRepresentativeById>("BI_BusinessGetRepresentativeById", DP)).ToList();
		}
	}

	public class BIBusinessGetRepresentativeEmail
	{
		private SQLCon _sQLCon;

		public BIBusinessGetRepresentativeEmail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetRepresentativeEmail()
		{
		}

		public int? RowNumber { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public long Id { get; set; }
		public DateTime? RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		public DateTime? DateOfIssue { get; set; }
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

		public async Task<List<BIBusinessGetRepresentativeEmail>> BIBusinessGetRepresentativeEmailDAO(string Email)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Email", Email);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetRepresentativeEmail>("BI_BusinessGetRepresentativeEmail", DP)).ToList();
		}
	}

	public class BIBusinessInsert
	{
		private SQLCon _sQLCon;

		public BIBusinessInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessInsert()
		{
		}

		public async Task<int> BIBusinessInsertDAO(BIBusinessInsertIN _bIBusinessInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("WardsId", _bIBusinessInsertIN.WardsId);
			DP.Add("DistrictId", _bIBusinessInsertIN.DistrictId);
			DP.Add("RepresentativeName", _bIBusinessInsertIN.RepresentativeName);
			DP.Add("Code", _bIBusinessInsertIN.Code);
			DP.Add("IsActived", _bIBusinessInsertIN.IsActived);
			DP.Add("IsDeleted", _bIBusinessInsertIN.IsDeleted);
			DP.Add("OrgPhone", _bIBusinessInsertIN.OrgPhone);
			DP.Add("OrgEmail", _bIBusinessInsertIN.OrgEmail);
			DP.Add("RepresentativeBirthDay", _bIBusinessInsertIN.RepresentativeBirthDay);
			DP.Add("ProvinceId", _bIBusinessInsertIN.ProvinceId);
			DP.Add("CreatedDate", _bIBusinessInsertIN.CreatedDate);
			DP.Add("UpdatedDate", _bIBusinessInsertIN.UpdatedDate);
			DP.Add("CreatedBy", _bIBusinessInsertIN.CreatedBy);
			DP.Add("UpdatedBy", _bIBusinessInsertIN.UpdatedBy);
			DP.Add("Status", _bIBusinessInsertIN.Status);
			DP.Add("RepresentativeGender", _bIBusinessInsertIN.RepresentativeGender);
			DP.Add("BusinessRegistration", _bIBusinessInsertIN.BusinessRegistration);
			DP.Add("DecisionOfEstablishing", _bIBusinessInsertIN.DecisionOfEstablishing);
			DP.Add("DateOfIssue", _bIBusinessInsertIN.DateOfIssue);
			DP.Add("Tax", _bIBusinessInsertIN.Tax);
			DP.Add("OrgProvinceId", _bIBusinessInsertIN.OrgProvinceId);
			DP.Add("OrgDistrictId", _bIBusinessInsertIN.OrgDistrictId);
			DP.Add("OrgWardsId", _bIBusinessInsertIN.OrgWardsId);
			DP.Add("OrgAddress", _bIBusinessInsertIN.OrgAddress);
			DP.Add("Address", _bIBusinessInsertIN.Address);
			DP.Add("Email", _bIBusinessInsertIN.Email);
			DP.Add("Phone", _bIBusinessInsertIN.Phone);
			DP.Add("Representative", _bIBusinessInsertIN.Representative);
			DP.Add("IDCard", _bIBusinessInsertIN.IDCard);
			DP.Add("Place", _bIBusinessInsertIN.Place);
			DP.Add("NativePlace", _bIBusinessInsertIN.NativePlace);
			DP.Add("PermanentPlace", _bIBusinessInsertIN.PermanentPlace);
			DP.Add("Nation", _bIBusinessInsertIN.Nation);
			DP.Add("Business", _bIBusinessInsertIN.Business);
			DP.Add("UserId", _bIBusinessInsertIN.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessInsert", DP));
		}
	}

	public class BIBusinessInsertIN
	{
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên người đại diện không được để trống")]
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại văn phòng đại diện không được để trống")]
		//[RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại văn phòng đại diện không đúng định dạng")]
		public string OrgPhone { get; set; }
		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail văn phòng đại diện không đúng định dạng")]
		public string OrgEmail { get; set; }
		[DataType(DataType.DateTime, ErrorMessage = "Ngày sinh người đại diện không đúng định dạng")]
		public DateTime? RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Giới tính không được để trống")]
		public bool? RepresentativeGender { get; set; }

		[StringLength(20, ErrorMessage = "Số chứng nhận đăng kí không vượt quá 20 kí tự")]
		public string BusinessRegistration { get; set; }
		public string DecisionOfEstablishing { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "Ngày cấp không đúng định dạng")]
		public DateTime? DateOfIssue { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Mã số thuế không được để trống")]
		//[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Mã số thuế không đúng định dạng")]
		//[StringLength(13, ErrorMessage = "Mã số thuế không vượt quá 13 kí tự")]
		public string Tax { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Tỉnh/Thành phố văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Tỉnh/Thành phố văn phòng đại diện không đúng định dạng")]
		public int? OrgProvinceId { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Quận/Huyện/Thị xã văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Quận/Huyện/Thị xã văn phòng đại diện không đúng định dạng")]

		public int? OrgDistrictId { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Xã phường/Thị trấn văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Xã phường/Thị trấn văn phòng đại diện không đúng định dạng")]
		public int? OrgWardsId { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ văn phòng đại diện không được để trống")]

		public string OrgAddress { get; set; }
		public string Address { get; set; }

		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail người đại diện không đúng định dạng")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại người đại diện không được để trống")]
		[RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại người đại diện không đúng định dạng")]
		public string Phone { get; set; }
		public string Representative { get; set; }
		public string IDCard { get; set; }
		public string Place { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Quốc tịch không được để trống")]
		public string Nation { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên tổ chức doanh nghiệp không được để trống")]
		public string Business { get; set; }
		public long? UserId { get; set; }
	}

	public class BIBusinessUpdateInfo
	{
		private SQLCon _sQLCon;

		public BIBusinessUpdateInfo(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessUpdateInfo()
		{
		}

		public async Task<int> BIBusinessUpdateInfoDAO(BIBusinessUpdateInfoIN _bIBusinessUpdateInfoIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bIBusinessUpdateInfoIN.Id);
			DP.Add("FullName", _bIBusinessUpdateInfoIN.FullName);
			DP.Add("DateOfBirth", _bIBusinessUpdateInfoIN.DateOfBirth);
			DP.Add("Email", _bIBusinessUpdateInfoIN.Email);
			DP.Add("Nation", _bIBusinessUpdateInfoIN.Nation);
			DP.Add("ProvinceId", _bIBusinessUpdateInfoIN.ProvinceId);
			DP.Add("DistrictId", _bIBusinessUpdateInfoIN.DistrictId);
			DP.Add("WardsId", _bIBusinessUpdateInfoIN.WardsId);
			DP.Add("Address", _bIBusinessUpdateInfoIN.Address);
			DP.Add("IdCard", _bIBusinessUpdateInfoIN.IdCard);
			DP.Add("IssuedPlace", _bIBusinessUpdateInfoIN.IssuedPlace);
			DP.Add("IssuedDate", _bIBusinessUpdateInfoIN.IssuedDate);
			DP.Add("Gender", _bIBusinessUpdateInfoIN.Gender);
			DP.Add("BusinessRegistration", _bIBusinessUpdateInfoIN.BusinessRegistration);
			DP.Add("DecisionOfEstablishing", _bIBusinessUpdateInfoIN.DecisionOfEstablishing);
			DP.Add("Tax", _bIBusinessUpdateInfoIN.Tax);
			DP.Add("OrgProvinceId", _bIBusinessUpdateInfoIN.OrgProvinceId);
			DP.Add("OrgDistrictId", _bIBusinessUpdateInfoIN.OrgDistrictId);
			DP.Add("OrgWardsId", _bIBusinessUpdateInfoIN.OrgWardsId);
			DP.Add("OrgAddress", _bIBusinessUpdateInfoIN.OrgAddress);
			DP.Add("OrgPhone", _bIBusinessUpdateInfoIN.OrgPhone);
			DP.Add("OrgEmail", _bIBusinessUpdateInfoIN.OrgEmail);
			DP.Add("Business", _bIBusinessUpdateInfoIN.Business);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessUpdateInfo", DP));
		}
	}

	public class BIBusinessUpdateInfoIN
	{
		public long? Id { get; set; }
		public string FullName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Email { get; set; }
		public string Nation { get; set; }
		public int? ProvinceId { get; set; }
		public int? DistrictId { get; set; }
		public int? WardsId { get; set; }
		public string Address { get; set; }
		public string IdCard { get; set; }
		public string IssuedPlace { get; set; }
		public DateTime? IssuedDate { get; set; }
		public bool? Gender { get; set; }
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

	public class BIIndividualCheckExists
	{
		private SQLCon _sQLCon;

		public BIIndividualCheckExists(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIIndividualCheckExists()
		{
		}

		public bool? Exists { get; set; }
		public string Value { get; set; }

		public async Task<List<BIIndividualCheckExists>> BIIndividualCheckExistsDAO(string Field, string Value, long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Field", Field);
			DP.Add("Value", Value);
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BIIndividualCheckExists>("BI_Individual_CheckExists", DP)).ToList();
		}
	}

	public class BIIndividualGetByUserId
	{
		private SQLCon _sQLCon;

		public BIIndividualGetByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIIndividualGetByUserId()
		{
		}

		public string FullName { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public long Id { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string IDCard { get; set; }
		public string IssuedPlace { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }
		public string Nation { get; set; }
		public DateTime? BirthDay { get; set; }
		public bool? Gender { get; set; }
		public long UserId { get; set; }

		public async Task<List<BIIndividualGetByUserId>> BIIndividualGetByUserIdDAO(long? UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);

			return (await _sQLCon.ExecuteListDapperAsync<BIIndividualGetByUserId>("BI_IndividualGetByUserId", DP)).ToList();
		}
	}

	public class BIIndividualInsert
	{
		private SQLCon _sQLCon;

		public BIIndividualInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIIndividualInsert()
		{
		}

		public async Task<int> BIIndividualInsertDAO(BIIndividualInsertIN _bIIndividualInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FullName", _bIIndividualInsertIN.FullName);
			DP.Add("IsActived", _bIIndividualInsertIN.IsActived);
			DP.Add("IsDeleted", _bIIndividualInsertIN.IsDeleted);
			DP.Add("ProvinceId", _bIIndividualInsertIN.ProvinceId);
			DP.Add("WardsId", _bIIndividualInsertIN.WardsId);
			DP.Add("DistrictId", _bIIndividualInsertIN.DistrictId);
			DP.Add("DateOfIssue", _bIIndividualInsertIN.DateOfIssue);
			DP.Add("CreatedDate", _bIIndividualInsertIN.CreatedDate);
			DP.Add("UpdatedDate", _bIIndividualInsertIN.UpdatedDate);
			DP.Add("CreatedBy", _bIIndividualInsertIN.CreatedBy);
			DP.Add("UpdatedBy", _bIIndividualInsertIN.UpdatedBy);
			DP.Add("Status", _bIIndividualInsertIN.Status);
			DP.Add("Code", _bIIndividualInsertIN.Code);
			DP.Add("Address", _bIIndividualInsertIN.Address);
			DP.Add("Email", _bIIndividualInsertIN.Email);
			DP.Add("Phone", _bIIndividualInsertIN.Phone);
			DP.Add("IDCard", _bIIndividualInsertIN.IDCard);
			DP.Add("IssuedPlace", _bIIndividualInsertIN.IssuedPlace);
			DP.Add("NativePlace", _bIIndividualInsertIN.NativePlace);
			DP.Add("PermanentPlace", _bIIndividualInsertIN.PermanentPlace);
			DP.Add("Nation", _bIIndividualInsertIN.Nation);
			DP.Add("BirthDay", _bIIndividualInsertIN.BirthDay);
			DP.Add("Gender", _bIIndividualInsertIN.Gender);
			DP.Add("UserId", _bIIndividualInsertIN.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualInsert", DP));
		}
	}

	public class BIIndividualInsertIN
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Họ tên không được để trống")]
		public string FullName { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "Ngày cấp không đúng định dạng")]
		public DateTime? DateOfIssue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public string Code { get; set; }
		public string Address { get; set; }

		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng định dạng")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được để trống")]
		[RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại không đúng định dạng")]

		public string Phone { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Số CMND/CCCD/Hộ chiếu không được để trống")]
		[RegularExpression(ConstantRegex.CMT, ErrorMessage = "Số CMND/CCCD/Hộ chiếu không đúng định dạng")]
		public string IDCard { get; set; }
		public string IssuedPlace { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Quốc tịch không được để trống")]
		public string Nation { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Ngày sinh không được để trống")]
		//[DataType(DataType.DateTime, ErrorMessage = "Ngày sinh không đúng định dạng")]

		public DateTime? BirthDay { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Giới tính không được để trống")]
		public bool? Gender { get; set; }
		public long? UserId { get; set; }
	}

	public class BIIndividualOrBusinessGetDropListByProviceId
	{
		private SQLCon _sQLCon;

		public BIIndividualOrBusinessGetDropListByProviceId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIIndividualOrBusinessGetDropListByProviceId()
		{
		}

		public long Id { get; set; }
		public int Category { get; set; }
		public string Name { get; set; }
		public string AdministrativeUnitName { get; set; }
		public short? AdministrativeUnitId { get; set; }

		public async Task<List<BIIndividualOrBusinessGetDropListByProviceId>> BIIndividualOrBusinessGetDropListByProviceIdDAO(string? LtsAdministrativeId, int? Type)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ltsAdministrativeId", LtsAdministrativeId);
			DP.Add("Type", Type);

			return (await _sQLCon.ExecuteListDapperAsync<BIIndividualOrBusinessGetDropListByProviceId>("BI_IndividualOrBusinessGetDropListByProviceId", DP)).ToList();
		}
	}

	public class BIInvididualUpdateInfo
	{
		private SQLCon _sQLCon;

		public BIInvididualUpdateInfo(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIInvididualUpdateInfo()
		{
		}

		public async Task<int> BIInvididualUpdateInfoDAO(BIInvididualUpdateInfoIN _bIInvididualUpdateInfoIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bIInvididualUpdateInfoIN.Id);
			DP.Add("FullName", _bIInvididualUpdateInfoIN.FullName);
			DP.Add("DateOfBirth", _bIInvididualUpdateInfoIN.DateOfBirth);
			DP.Add("Email", _bIInvididualUpdateInfoIN.Email);
			DP.Add("Nation", _bIInvididualUpdateInfoIN.Nation);
			DP.Add("ProvinceId", _bIInvididualUpdateInfoIN.ProvinceId);
			DP.Add("DistrictId", _bIInvididualUpdateInfoIN.DistrictId);
			DP.Add("WardsId", _bIInvididualUpdateInfoIN.WardsId);
			DP.Add("Address", _bIInvididualUpdateInfoIN.Address);
			DP.Add("IdCard", _bIInvididualUpdateInfoIN.IdCard);
			DP.Add("IssuedPlace", _bIInvididualUpdateInfoIN.IssuedPlace);
			DP.Add("IssuedDate", _bIInvididualUpdateInfoIN.IssuedDate);
			DP.Add("Gender", _bIInvididualUpdateInfoIN.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_InvididualUpdateInfo", DP));
		}
	}

	public class BIInvididualUpdateInfoIN
	{
		public long? Id { get; set; }
		public string FullName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string Email { get; set; }
		public string Nation { get; set; }
		public int? ProvinceId { get; set; }
		public int? DistrictId { get; set; }
		public int? WardsId { get; set; }
		public string Address { get; set; }
		public string IdCard { get; set; }
		public string IssuedPlace { get; set; }
		public DateTime? IssuedDate { get; set; }
		public bool? Gender { get; set; }
	}
}
