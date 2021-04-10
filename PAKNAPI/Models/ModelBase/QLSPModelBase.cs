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
	public class QLDoanhNghiepInsert
	{
		private SQLCon _sQLCon;

		public QLDoanhNghiepInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public QLDoanhNghiepInsert()
		{
		}

		public async Task<int> QLDoanhNghiepInsertDAO(QLDoanhNghiepInsertIN _qLDoanhNghiepInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Phone", _qLDoanhNghiepInsertIN.Phone);
			DP.Add("Gender", _qLDoanhNghiepInsertIN.Gender);
			DP.Add("DOB", _qLDoanhNghiepInsertIN.DOB);
			DP.Add("Business", _qLDoanhNghiepInsertIN.Business);
			DP.Add("Tax", _qLDoanhNghiepInsertIN.Tax);
			DP.Add("Status", _qLDoanhNghiepInsertIN.Status);
			DP.Add("CreatedBy", _qLDoanhNghiepInsertIN.CreatedBy);
			DP.Add("CreatedDate", _qLDoanhNghiepInsertIN.CreatedDate);
			DP.Add("UpdatedBy", _qLDoanhNghiepInsertIN.UpdatedBy);
			DP.Add("UpdatedDate", _qLDoanhNghiepInsertIN.UpdatedDate);
			DP.Add("RepresentativeName", _qLDoanhNghiepInsertIN.RepresentativeName);
			DP.Add("Email", _qLDoanhNghiepInsertIN.Email);
			DP.Add("OrgProvince", _qLDoanhNghiepInsertIN.OrgProvince);
			DP.Add("OrgDistrict", _qLDoanhNghiepInsertIN.OrgDistrict);
			DP.Add("OrgVillage", _qLDoanhNghiepInsertIN.OrgVillage);
			DP.Add("OrgAddress", _qLDoanhNghiepInsertIN.OrgAddress);
			DP.Add("OrgPhone", _qLDoanhNghiepInsertIN.OrgPhone);
			DP.Add("OrgEmail", _qLDoanhNghiepInsertIN.OrgEmail);
			DP.Add("RegistrationNum", _qLDoanhNghiepInsertIN.RegistrationNum);
			DP.Add("DecisionFoundation", _qLDoanhNghiepInsertIN.DecisionFoundation);
			DP.Add("DateIssue", _qLDoanhNghiepInsertIN.DateIssue);
			DP.Add("Nation", _qLDoanhNghiepInsertIN.Nation);
			DP.Add("Province", _qLDoanhNghiepInsertIN.Province);
			DP.Add("District", _qLDoanhNghiepInsertIN.District);
			DP.Add("Village", _qLDoanhNghiepInsertIN.Village);
			DP.Add("Address", _qLDoanhNghiepInsertIN.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_DoanhNghiepInsert", DP));
		}
	}

	public class QLDoanhNghiepInsertIN
	{
		public string Phone { get; set; }
		public bool? Gender { get; set; }
		public DateTime? DOB { get; set; }
		public string Business { get; set; }
		public string Tax { get; set; }
		public byte? Status { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string RepresentativeName { get; set; }
		public string Email { get; set; }
		public int? OrgProvince { get; set; }
		public int? OrgDistrict { get; set; }
		public int? OrgVillage { get; set; }
		public int? OrgAddress { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public string RegistrationNum { get; set; }
		public string DecisionFoundation { get; set; }
		public DateTime? DateIssue { get; set; }
		public int? Nation { get; set; }
		public int? Province { get; set; }
		public int? District { get; set; }
		public int? Village { get; set; }
		public string Address { get; set; }
	}

	public class QLNguoiDanInsert
	{
		private SQLCon _sQLCon;

		public QLNguoiDanInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public QLNguoiDanInsert()
		{
		}

		public async Task<int> QLNguoiDanInsertDAO(QLNguoiDanInsertIN _qLNguoiDanInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Phone", _qLNguoiDanInsertIN.Phone);
			DP.Add("Identity", _qLNguoiDanInsertIN.Identity);
			DP.Add("Gender", _qLNguoiDanInsertIN.Gender);
			DP.Add("DOB", _qLNguoiDanInsertIN.DOB);
			DP.Add("Status", _qLNguoiDanInsertIN.Status);
			DP.Add("CreatedBy", _qLNguoiDanInsertIN.CreatedBy);
			DP.Add("CreatedDate", _qLNguoiDanInsertIN.CreatedDate);
			DP.Add("UpdatedBy", _qLNguoiDanInsertIN.UpdatedBy);
			DP.Add("UpdatedDate", _qLNguoiDanInsertIN.UpdatedDate);
			DP.Add("FullName", _qLNguoiDanInsertIN.FullName);
			DP.Add("Email", _qLNguoiDanInsertIN.Email);
			DP.Add("PlaceIssue", _qLNguoiDanInsertIN.PlaceIssue);
			DP.Add("DateIssue", _qLNguoiDanInsertIN.DateIssue);
			DP.Add("Nation", _qLNguoiDanInsertIN.Nation);
			DP.Add("Province", _qLNguoiDanInsertIN.Province);
			DP.Add("District", _qLNguoiDanInsertIN.District);
			DP.Add("Village", _qLNguoiDanInsertIN.Village);
			DP.Add("Address", _qLNguoiDanInsertIN.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_NguoiDanInsert", DP));
		}
	}

	public class QLNguoiDanInsertIN
	{
		public string Phone { get; set; }
		public int? Identity { get; set; }
		public bool? Gender { get; set; }
		public DateTime? DOB { get; set; }
		public byte? Status { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string PlaceIssue { get; set; }
		public DateTime? DateIssue { get; set; }
		public int? Nation { get; set; }
		public int? Province { get; set; }
		public int? District { get; set; }
		public int? Village { get; set; }
		public string Address { get; set; }
	}
}
