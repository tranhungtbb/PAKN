using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAKNAPI.ModelBase;

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
		public string Address { get; set; }
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

	public class BI_BusinessInsertIN
	{
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public DateTime? RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
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
		public int? OrgDistrictId { get; set; }
		public int? OrgWardsId { get; set; }
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
	}
	public class BI_BusinessInsertIN_Cus : BI_BusinessInsertIN
	{
		public string _RepresentativeBirthDay { get; set; }
		public string _DateOfIssue { get; set; }

	}
	#endregion

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

		public async Task<int> BI_BusinessUpdateInfoDAO(BI_BusinessUpdateInfoIN_body _businessUpdateInfoIN)
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
			DP.Add("UserId", _businessUpdateInfoIN.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessUpdate", DP));
		}
	}

	public class BI_BusinessUpdateInfoIN_body : BI_BusinessUpdateInfoIN
	{
		public string _RepresentativeBirthDay { get; set; }
		public string _DateOfIssue { get; set; }

	}

	public class BI_BusinessUpdateInfoIN
	{
		public long? Id { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public DateTime? RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
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
		public int? OrgDistrictId { get; set; }
		public int? OrgWardsId { get; set; }
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
	}
}
