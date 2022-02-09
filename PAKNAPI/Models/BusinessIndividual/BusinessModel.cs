using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAKNAPI.ModelBase;
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.Models.BusinessIndividual
{
    #region IndividualGetAllOnPage
    public class BI_BusinessGetAllOnPage
	{
		private SQLCon _sQLCon;

		public BI_BusinessGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_BusinessGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string RepresentativeName { get; set; }
		public string OrgAddress { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public int? Status { get; set; }
		public bool IsDeleted { get; set; }
		public string Business { get; set; }
		public string Tax { get; set; }
		public string BusinessRegistration { get; set; }

		public async Task<List<BI_BusinessGetAllOnPage>> BI_BusinessGetAllOnPageDAO(int? PageSize, int? PageIndex, string RepresentativeName, string Address, string Phone, string Email, byte? Status, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("RepresentativeName", RepresentativeName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("Status", Status);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

			return (await _sQLCon.ExecuteListDapperAsync<BI_BusinessGetAllOnPage>("BI_BusinessGetAllOnPage", DP)).ToList();
		}
	}
	#endregion

	#region BI_BusinessDelete
	public class BI_BusinessDelete
	{
		private SQLCon _sQLCon;

		public BI_BusinessDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_BusinessDelete()
		{
		}

		public async Task<int> BusinessDeleteDAO(BI_BusinessDeleteIN _bI_BusinessDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bI_BusinessDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessDelete", DP));
		}
	}

	public class BI_BusinessDeleteIN
	{
		/// <example>
		/// 1
		/// </example>
		public long? Id { get; set; }
	}

	#endregion

	#region  BI_BusinessChageStatus

	public class BI_BusinessChageStatus
	{
		private SQLCon _sQLCon;

		public BI_BusinessChageStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_BusinessChageStatus()
		{
		}

		public async Task<int> BI_BusinessChageStatusDAO(BI_BusinessChageStatusIN _bI_BusinessChageStatusIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bI_BusinessChageStatusIN.Id);
			DP.Add("Status", _bI_BusinessChageStatusIN.Status);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessChageStatus", DP));
		}
	}


	/// <example>
	/// { "Id": 1230, "Status" : 1}
	/// </example>

	public class BI_BusinessChageStatusIN
	{
		public long? Id { get; set; }
		public byte? Status { get; set; }
	}

	#endregion

	#region BI_BusinessInsert
	public class BI_BusinessInsert
	{
		private SQLCon _sQLCon;

		public BI_BusinessInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_BusinessInsert()
		{
		}

		public async Task<int> BusinessInsertDAO(BI_BusinessInsertIN _businessInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("WardsId", _businessInsertIN.WardsId);
			DP.Add("DistrictId", _businessInsertIN.DistrictId);
			DP.Add("RepresentativeName", _businessInsertIN.RepresentativeName);
			DP.Add("Code", _businessInsertIN.Code);
			DP.Add("IsActived", _businessInsertIN.IsActived);
			DP.Add("IsDeleted", _businessInsertIN.IsDeleted);
			DP.Add("OrgPhone", _businessInsertIN.OrgPhone);
			DP.Add("OrgEmail", _businessInsertIN.OrgEmail);
			DP.Add("RepresentativeBirthDay", _businessInsertIN.RepresentativeBirthDay);
			DP.Add("ProvinceId", _businessInsertIN.ProvinceId);
			DP.Add("CreatedDate", _businessInsertIN.CreatedDate);
			DP.Add("UpdatedDate", _businessInsertIN.UpdatedDate);
			DP.Add("CreatedBy", _businessInsertIN.CreatedBy);
			DP.Add("UpdatedBy", _businessInsertIN.UpdatedBy);
			DP.Add("Status", _businessInsertIN.Status);
			DP.Add("RepresentativeGender", _businessInsertIN.RepresentativeGender);
			DP.Add("BusinessRegistration", _businessInsertIN.BusinessRegistration);
			DP.Add("DecisionOfEstablishing", _businessInsertIN.DecisionOfEstablishing);
			DP.Add("DateOfIssue", _businessInsertIN.DateOfIssue);
			DP.Add("Tax", _businessInsertIN.Tax);
			DP.Add("OrgProvinceId", _businessInsertIN.OrgProvinceId);
			DP.Add("OrgDistrictId", _businessInsertIN.OrgDistrictId);
			DP.Add("OrgWardsId", _businessInsertIN.OrgWardsId);
			DP.Add("OrgAddress", _businessInsertIN.OrgAddress);
			DP.Add("Address", _businessInsertIN.Address);
			DP.Add("Email", _businessInsertIN.Email);
			DP.Add("Phone", _businessInsertIN.Phone);
			DP.Add("Representative", _businessInsertIN.Representative);
			DP.Add("IDCard", _businessInsertIN.IDCard);
			DP.Add("Place", _businessInsertIN.Place);
			DP.Add("NativePlace", _businessInsertIN.NativePlace);
			DP.Add("PermanentPlace", _businessInsertIN.PermanentPlace);
			DP.Add("Nation", _businessInsertIN.Nation);
			DP.Add("Business", _businessInsertIN.Business);
			DP.Add("UserId", _businessInsertIN.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BusinessInsert", DP));
		}
	}


	/// <example>
	///{
	///		"Address": "Hà nội",
	///		"Business": "Công ty b",
	///		"BusinessRegistration": "1234567890",
	///		"DateOfIssue": null,
	///		"DecisionOfEstablishing": "",
	///		"Email": "",
	///		"IsDeleted": false,
	///		"Nation": "Việt Nam",
	///		"OrgAddress": "Hà nội",
	///		"OrgEmail": "hungtd@thanglonginc.com",
	///		"OrgPhone": "0923423423",
	///		"RepresentativeGender": true,
	///		"RepresentativeName": "Trần Đình Hùng",
	///		"Status": 1,
	///		"isActived": true,
	///		"phone": "0982343242",
	///}
	/// </example>

	

	public class BI_BusinessInsertIN
	{
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên người đại diện không được để trống")]
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại văn phòng đại diện không đúng định dạng")]
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

		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		public int? Status { get; set; }
		//[Required(AllowEmptyStrings = false, ErrorMessage = "Giới tính không được để trống")]
		public bool? RepresentativeGender { get; set; }

		[StringLength(20, ErrorMessage = "Số chứng nhận đăng kí không vượt quá 20 kí tự")]
		public string BusinessRegistration { get; set; }
		public string DecisionOfEstablishing { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "Ngày cấp không đúng định dạng")]
		public DateTime? DateOfIssue { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Mã số thuế không được để trống")]
		//[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Mã số thuế không đúng định dạng")]
		[StringLength(13, ErrorMessage = "Mã số thuế không vượt quá 13 kí tự")]
		public string Tax { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Tỉnh/Thành phố văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Tỉnh/Thành phố văn phòng đại diện không đúng định dạng")]
		public int? OrgProvinceId { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Quận/Huyện/Thị xã văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.NUMBER, ErrorMessage = "Quận/Huyện/Thị xã văn phòng đại diện không đúng định dạng")]
		public int? OrgDistrictId { get; set; }

		//[Required(AllowEmptyStrings = false, ErrorMessage = "Xã phường/Thị trấn văn phòng đại diện không được để trống")]
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

		[Required(AllowEmptyStrings = false, ErrorMessage = "Quốc tịch không được để trống")]
		public string Nation { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên tổ chức doanh nghiệp không được để trống")]
		public string Business { get; set; }
		public long? UserId { get; set; }
	}
	#endregion


	/// <example>
	///{
	///		"id" : 100167,
	///		"Address": "Hà nội",
	///		"Business": "Công ty b",
	///		"BusinessRegistration": "1234567890",
	///		"DateOfIssue": null,
	///		"DecisionOfEstablishing": "",
	///		"Email": "",
	///		"IsDeleted": false,
	///		"Nation": "Việt Nam",
	///		"OrgAddress": "Hà nội",
	///		"OrgEmail": "hungtd@thanglonginc.com",
	///		"OrgPhone": "0923423423",
	///		"RepresentativeGender": true,
	///		"RepresentativeName": "Trần Đình Hùng",
	///		"Status": 1,
	///		"isActived": true,
	///		"phone": "0982343242",
	///}
	/// </example>
	public class BI_BusinessUpdateInfoIN
	{
		public long? Id { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên người đại diện không được để trống")]
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại văn phòng đại diện không được để trống")]
		[RegularExpression(ConstantRegex.PHONE, ErrorMessage = "Số điện thoại văn phòng đại diện không đúng định dạng")]
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

		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		[StringLength(20, ErrorMessage = "Số chứng nhận đăng kí không vượt quá 20 kí tự")]
		public string BusinessRegistration { get; set; }
		public string DecisionOfEstablishing { get; set; }
		[DataType(DataType.DateTime, ErrorMessage = "Ngày cấp không đúng định dạng")]
		public DateTime? DateOfIssue { get; set; }

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
		[Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ văn phòng đại diện không được để trống")]
		public string OrgAddress { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Địa chỉ người đại diện không được để trống")]
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
	}

	#region BI_BusinessGetById
	public class BI_BusinessGetById
	{
		private SQLCon _sQLCon;

		public BI_BusinessGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_BusinessGetById()
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

		public async Task<List<BI_BusinessGetById>> BusinessGetByIdDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BI_BusinessGetById>("BI_BusinessGetById", DP)).ToList();
		}
	}
	#endregion

	public class BI_BusinessUpdateInfo
	{
		private SQLCon _sQLCon;

		public BI_BusinessUpdateInfo(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_BusinessUpdateInfo()
		{
		}

		public async Task<int> BI_BusinessUpdateInfoDAO(BI_BusinessUpdateInfoIN _businessUpdateInfoIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _businessUpdateInfoIN.Id);
			DP.Add("WardsId", _businessUpdateInfoIN.WardsId);
			DP.Add("DistrictId", _businessUpdateInfoIN.DistrictId);
			DP.Add("RepresentativeName", _businessUpdateInfoIN.RepresentativeName);
			DP.Add("Code", _businessUpdateInfoIN.Code);
			DP.Add("IsActived", _businessUpdateInfoIN.IsActived);
			DP.Add("IsDeleted", _businessUpdateInfoIN.IsDeleted);
			DP.Add("OrgPhone", _businessUpdateInfoIN.OrgPhone);
			DP.Add("OrgEmail", _businessUpdateInfoIN.OrgEmail);
			DP.Add("RepresentativeBirthDay", _businessUpdateInfoIN.RepresentativeBirthDay);
			DP.Add("ProvinceId", _businessUpdateInfoIN.ProvinceId);
			DP.Add("CreatedDate", _businessUpdateInfoIN.CreatedDate);
			DP.Add("UpdatedDate", _businessUpdateInfoIN.UpdatedDate);
			DP.Add("CreatedBy", _businessUpdateInfoIN.CreatedBy);
			DP.Add("UpdatedBy", _businessUpdateInfoIN.UpdatedBy);
			DP.Add("Status", _businessUpdateInfoIN.Status);
			DP.Add("RepresentativeGender", _businessUpdateInfoIN.RepresentativeGender);
			DP.Add("BusinessRegistration", _businessUpdateInfoIN.BusinessRegistration);
			DP.Add("DecisionOfEstablishing", _businessUpdateInfoIN.DecisionOfEstablishing);
			DP.Add("DateOfIssue", _businessUpdateInfoIN.DateOfIssue);
			DP.Add("Tax", _businessUpdateInfoIN.Tax);
			DP.Add("OrgProvinceId", _businessUpdateInfoIN.OrgProvinceId);
			DP.Add("OrgDistrictId", _businessUpdateInfoIN.OrgDistrictId);
			DP.Add("OrgWardsId", _businessUpdateInfoIN.OrgWardsId);
			DP.Add("OrgAddress", _businessUpdateInfoIN.OrgAddress);
			DP.Add("Address", _businessUpdateInfoIN.Address);
			DP.Add("Email", _businessUpdateInfoIN.Email);
			DP.Add("Phone", _businessUpdateInfoIN.Phone);
			DP.Add("Representative", _businessUpdateInfoIN.Representative);
			DP.Add("IDCard", _businessUpdateInfoIN.IDCard);
			DP.Add("Place", _businessUpdateInfoIN.Place);
			DP.Add("NativePlace", _businessUpdateInfoIN.NativePlace);
			DP.Add("PermanentPlace", _businessUpdateInfoIN.PermanentPlace);
			DP.Add("Nation", _businessUpdateInfoIN.Nation);
			DP.Add("Business", _businessUpdateInfoIN.Business);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessUpdate", DP));
		}
	}
	
}
