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

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_BusinessInsert", DP));
		}
	}

	public class BIBusinessInsertIN
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

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualInsert", DP));
		}
	}

	public class BIIndividualInsertIN
	{
		public string FullName { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? CreatedBy { get; set; }
		public int? UpdatedBy { get; set; }
		public int? Status { get; set; }
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
	}
}
