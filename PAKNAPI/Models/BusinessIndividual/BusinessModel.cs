using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.BusinessIndividual
{
    #region IndividualGetAllOnPage
    public class BusinessGetAllOnPage
	{
		private SQLCon _sQLCon;

		public BusinessGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string RepresentativeName { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<BusinessGetAllOnPage>> BusinessGetAllOnPageDAO(int? PageSize, int? PageIndex, string RepresentativeName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("RepresentativeName", RepresentativeName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("IsActived", IsActived);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

			return (await _sQLCon.ExecuteListDapperAsync<BusinessGetAllOnPage>("BI_BusinessGetAllOnPage", DP)).ToList();
		}
	}
	#endregion

	#region BusinessDelete
	public class BusinessDelete
	{
		private SQLCon _sQLCon;

		public BusinessDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessDelete()
		{
		}

		public async Task<int> BusinessDeleteDAO(BusinessDeleteIN _businessDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _businessDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessDelete", DP));
		}
	}

	public class BusinessDeleteIN
	{
		public long? Id { get; set; }
	}

	#endregion

	#region  BusinessChageStatus

	public class BusinessChageStatus
	{
		private SQLCon _sQLCon;

		public BusinessChageStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessChageStatus()
		{
		}

		public async Task<int> BusinessChageStatusDAO(BusinessChageStatusIN _businessChageStatusIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _businessChageStatusIN.Id);
			DP.Add("IsActived", _businessChageStatusIN.IsActived);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessChageStatus", DP));
		}
	}

	public class BusinessChageStatusIN
	{
		public long? Id { get; set; }
		public bool? IsActived { get; set; }
	}

	#endregion

	#region BusinessInsert
	public class BusinessInsert
	{
		private SQLCon _sQLCon;

		public BusinessInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessInsert()
		{
		}

		public async Task<int> BusinessInsertDAO(BusinessInsertIN _businessInsertIN)
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

	public class BusinessInsertIN
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

	#endregion


	public class BusinessGetById
	{
		private SQLCon _sQLCon;

		public BusinessGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessGetById()
		{
		}

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

		public async Task<List<BusinessGetById>> BusinessGetByIdDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BusinessGetById>("BusinessGetById", DP)).ToList();
		}
	}
}
