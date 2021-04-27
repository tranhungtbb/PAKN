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

namespace PAKNAPI.ModelBase
{
	public class BIBusinessOnPage
	{
		public long Id { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Representative { get; set; }
		public string IDCard { get; set; }
		public string Place { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }
		public string Nation { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string BusinessRegistration { get; set; }
		public string DecisionOfEstablishing { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public string Tax { get; set; }
		public int? OrgProvinceId { get; set; }
		public int? OrgDistrictId { get; set; }
		public int? OrgWardsId { get; set; }
		public string OrgAddress { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		public DateTime RepresentativeBirthDay { get; set; }
		public string Business { get; set; }
		public long UserId { get; set; }
		public int? RowNumber; // int, null
	}

	public class BIBusiness
	{
		private SQLCon _sQLCon;

		public BIBusiness(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusiness()
		{
		}

		public long Id { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Representative { get; set; }
		public string IDCard { get; set; }
		public string Place { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }
		public string Nation { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string BusinessRegistration { get; set; }
		public string DecisionOfEstablishing { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public string Tax { get; set; }
		public int? OrgProvinceId { get; set; }
		public int? OrgDistrictId { get; set; }
		public int? OrgWardsId { get; set; }
		public string OrgAddress { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		public DateTime RepresentativeBirthDay { get; set; }
		public string Business { get; set; }
		public long UserId { get; set; }

		public async Task<BIBusiness> BIBusinessGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusiness>("BI_BusinessGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<BIBusiness>> BIBusinessGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<BIBusiness>("BI_BusinessGetAll", DP)).ToList();
		}

		public async Task<List<BIBusinessOnPage>> BIBusinessGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessOnPage>("BI_BusinessGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> BIBusinessInsert(BIBusiness _bIBusiness)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RepresentativeName", _bIBusiness.RepresentativeName);
			DP.Add("IsActived", _bIBusiness.IsActived);
			DP.Add("IsDeleted", _bIBusiness.IsDeleted);
			DP.Add("OrgPhone", _bIBusiness.OrgPhone);
			DP.Add("OrgEmail", _bIBusiness.OrgEmail);
			DP.Add("RepresentativeBirthDay", _bIBusiness.RepresentativeBirthDay);
			DP.Add("UserId", _bIBusiness.UserId);
			DP.Add("Business", _bIBusiness.Business);
			DP.Add("ProvinceId", _bIBusiness.ProvinceId);
			DP.Add("WardsId", _bIBusiness.WardsId);
			DP.Add("DistrictId", _bIBusiness.DistrictId);
			DP.Add("CreatedDate", _bIBusiness.CreatedDate);
			DP.Add("UpdatedDate", _bIBusiness.UpdatedDate);
			DP.Add("CreatedBy", _bIBusiness.CreatedBy);
			DP.Add("UpdatedBy", _bIBusiness.UpdatedBy);
			DP.Add("Status", _bIBusiness.Status);
			DP.Add("RepresentativeGender", _bIBusiness.RepresentativeGender);
			DP.Add("BusinessRegistration", _bIBusiness.BusinessRegistration);
			DP.Add("DecisionOfEstablishing", _bIBusiness.DecisionOfEstablishing);
			DP.Add("DateOfIssue", _bIBusiness.DateOfIssue);
			DP.Add("Tax", _bIBusiness.Tax);
			DP.Add("OrgProvinceId", _bIBusiness.OrgProvinceId);
			DP.Add("OrgDistrictId", _bIBusiness.OrgDistrictId);
			DP.Add("OrgWardsId", _bIBusiness.OrgWardsId);
			DP.Add("OrgAddress", _bIBusiness.OrgAddress);
			DP.Add("Code", _bIBusiness.Code);
			DP.Add("Address", _bIBusiness.Address);
			DP.Add("Email", _bIBusiness.Email);
			DP.Add("Phone", _bIBusiness.Phone);
			DP.Add("Representative", _bIBusiness.Representative);
			DP.Add("IDCard", _bIBusiness.IDCard);
			DP.Add("Place", _bIBusiness.Place);
			DP.Add("NativePlace", _bIBusiness.NativePlace);
			DP.Add("PermanentPlace", _bIBusiness.PermanentPlace);
			DP.Add("Nation", _bIBusiness.Nation);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessInsert", DP));
		}

		public async Task<int> BIBusinessUpdate(BIBusiness _bIBusiness)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RepresentativeName", _bIBusiness.RepresentativeName);
			DP.Add("IsActived", _bIBusiness.IsActived);
			DP.Add("IsDeleted", _bIBusiness.IsDeleted);
			DP.Add("OrgPhone", _bIBusiness.OrgPhone);
			DP.Add("OrgEmail", _bIBusiness.OrgEmail);
			DP.Add("Id", _bIBusiness.Id);
			DP.Add("RepresentativeBirthDay", _bIBusiness.RepresentativeBirthDay);
			DP.Add("UserId", _bIBusiness.UserId);
			DP.Add("Business", _bIBusiness.Business);
			DP.Add("ProvinceId", _bIBusiness.ProvinceId);
			DP.Add("WardsId", _bIBusiness.WardsId);
			DP.Add("DistrictId", _bIBusiness.DistrictId);
			DP.Add("CreatedDate", _bIBusiness.CreatedDate);
			DP.Add("UpdatedDate", _bIBusiness.UpdatedDate);
			DP.Add("CreatedBy", _bIBusiness.CreatedBy);
			DP.Add("UpdatedBy", _bIBusiness.UpdatedBy);
			DP.Add("Status", _bIBusiness.Status);
			DP.Add("RepresentativeGender", _bIBusiness.RepresentativeGender);
			DP.Add("BusinessRegistration", _bIBusiness.BusinessRegistration);
			DP.Add("DecisionOfEstablishing", _bIBusiness.DecisionOfEstablishing);
			DP.Add("DateOfIssue", _bIBusiness.DateOfIssue);
			DP.Add("Tax", _bIBusiness.Tax);
			DP.Add("OrgProvinceId", _bIBusiness.OrgProvinceId);
			DP.Add("OrgDistrictId", _bIBusiness.OrgDistrictId);
			DP.Add("OrgWardsId", _bIBusiness.OrgWardsId);
			DP.Add("OrgAddress", _bIBusiness.OrgAddress);
			DP.Add("Code", _bIBusiness.Code);
			DP.Add("Address", _bIBusiness.Address);
			DP.Add("Email", _bIBusiness.Email);
			DP.Add("Phone", _bIBusiness.Phone);
			DP.Add("Representative", _bIBusiness.Representative);
			DP.Add("IDCard", _bIBusiness.IDCard);
			DP.Add("Place", _bIBusiness.Place);
			DP.Add("NativePlace", _bIBusiness.NativePlace);
			DP.Add("PermanentPlace", _bIBusiness.PermanentPlace);
			DP.Add("Nation", _bIBusiness.Nation);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessUpdate", DP));
		}

		public async Task<int> BIBusinessDelete(BIBusiness _bIBusiness)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bIBusiness.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessDelete", DP));
		}

		public async Task<int> BIBusinessDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessDeleteAll", DP));
		}

		public async Task<int> BIBusinessCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("BI_BusinessCount", DP));
		}
	}

	public class BIIndividualOnPage
	{
		public long Id { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string FullName { get; set; }
		public string Code { get; set; }
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
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public long UserId { get; set; }
		public int? RowNumber; // int, null
	}

	public class BIIndividual
	{
		private SQLCon _sQLCon;

		public BIIndividual(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIIndividual()
		{
		}

		public long Id { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string FullName { get; set; }
		public string Code { get; set; }
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
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
		public long UserId { get; set; }

		public async Task<BIIndividual> BIIndividualGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BIIndividual>("BI_IndividualGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<BIIndividual>> BIIndividualGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<BIIndividual>("BI_IndividualGetAll", DP)).ToList();
		}

		public async Task<List<BIIndividualOnPage>> BIIndividualGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<BIIndividualOnPage>("BI_IndividualGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> BIIndividualInsert(BIIndividual _bIIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FullName", _bIIndividual.FullName);
			DP.Add("IsActived", _bIIndividual.IsActived);
			DP.Add("IsDeleted", _bIIndividual.IsDeleted);
			DP.Add("UserId", _bIIndividual.UserId);
			DP.Add("ProvinceId", _bIIndividual.ProvinceId);
			DP.Add("WardsId", _bIIndividual.WardsId);
			DP.Add("DistrictId", _bIIndividual.DistrictId);
			DP.Add("DateOfIssue", _bIIndividual.DateOfIssue);
			DP.Add("CreatedDate", _bIIndividual.CreatedDate);
			DP.Add("UpdatedDate", _bIIndividual.UpdatedDate);
			DP.Add("CreatedBy", _bIIndividual.CreatedBy);
			DP.Add("UpdatedBy", _bIIndividual.UpdatedBy);
			DP.Add("Status", _bIIndividual.Status);
			DP.Add("Code", _bIIndividual.Code);
			DP.Add("Address", _bIIndividual.Address);
			DP.Add("Email", _bIIndividual.Email);
			DP.Add("Phone", _bIIndividual.Phone);
			DP.Add("IDCard", _bIIndividual.IDCard);
			DP.Add("IssuedPlace", _bIIndividual.IssuedPlace);
			DP.Add("NativePlace", _bIIndividual.NativePlace);
			DP.Add("PermanentPlace", _bIIndividual.PermanentPlace);
			DP.Add("Nation", _bIIndividual.Nation);
			DP.Add("BirthDay", _bIIndividual.BirthDay);
			DP.Add("Gender", _bIIndividual.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualInsert", DP));
		}

		public async Task<int> BIIndividualUpdate(BIIndividual _bIIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FullName", _bIIndividual.FullName);
			DP.Add("IsActived", _bIIndividual.IsActived);
			DP.Add("IsDeleted", _bIIndividual.IsDeleted);
			DP.Add("Id", _bIIndividual.Id);
			DP.Add("UserId", _bIIndividual.UserId);
			DP.Add("ProvinceId", _bIIndividual.ProvinceId);
			DP.Add("WardsId", _bIIndividual.WardsId);
			DP.Add("DistrictId", _bIIndividual.DistrictId);
			DP.Add("DateOfIssue", _bIIndividual.DateOfIssue);
			DP.Add("CreatedDate", _bIIndividual.CreatedDate);
			DP.Add("UpdatedDate", _bIIndividual.UpdatedDate);
			DP.Add("CreatedBy", _bIIndividual.CreatedBy);
			DP.Add("UpdatedBy", _bIIndividual.UpdatedBy);
			DP.Add("Status", _bIIndividual.Status);
			DP.Add("Code", _bIIndividual.Code);
			DP.Add("Address", _bIIndividual.Address);
			DP.Add("Email", _bIIndividual.Email);
			DP.Add("Phone", _bIIndividual.Phone);
			DP.Add("IDCard", _bIIndividual.IDCard);
			DP.Add("IssuedPlace", _bIIndividual.IssuedPlace);
			DP.Add("NativePlace", _bIIndividual.NativePlace);
			DP.Add("PermanentPlace", _bIIndividual.PermanentPlace);
			DP.Add("Nation", _bIIndividual.Nation);
			DP.Add("BirthDay", _bIIndividual.BirthDay);
			DP.Add("Gender", _bIIndividual.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualUpdate", DP));
		}

		public async Task<int> BIIndividualDelete(BIIndividual _bIIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bIIndividual.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualDelete", DP));
		}

		public async Task<int> BIIndividualDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualDeleteAll", DP));
		}

		public async Task<int> BIIndividualCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("BI_IndividualCount", DP));
		}
	}
}
