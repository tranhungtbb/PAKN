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
	public class QLDoanhNghiepOnPage
	{
		public int Id { get; set; }
		public string RepresentativeName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? Nation { get; set; }
		public int? Province { get; set; }
		public int? District { get; set; }
		public int? Village { get; set; }
		public string Address { get; set; }
		public bool Gender { get; set; }
		public DateTime DOB { get; set; }
		public string Business { get; set; }
		public string RegistrationNum { get; set; }
		public string DecisionFoundation { get; set; }
		public DateTime? DateIssue { get; set; }
		public string Tax { get; set; }
		public byte Status { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public int UpdatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }
		public int? OrgProvince { get; set; }
		public int? OrgDistrict { get; set; }
		public int? OrgVillage { get; set; }
		public int? OrgAddress { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }
		public int? RowNumber; // int, null
	}

	public class QLDoanhNghiep
	{
		private SQLCon _sQLCon;

		public QLDoanhNghiep(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public QLDoanhNghiep()
		{
		}

		public int Id { get; set; }
		public string RepresentativeName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? Nation { get; set; }
		public int? Province { get; set; }
		public int? District { get; set; }
		public int? Village { get; set; }
		public string Address { get; set; }
		public bool Gender { get; set; }
		public DateTime DOB { get; set; }
		public string Business { get; set; }
		public string RegistrationNum { get; set; }
		public string DecisionFoundation { get; set; }
		public DateTime? DateIssue { get; set; }
		public string Tax { get; set; }
		public byte Status { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public int UpdatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }
		public int? OrgProvince { get; set; }
		public int? OrgDistrict { get; set; }
		public int? OrgVillage { get; set; }
		public int? OrgAddress { get; set; }
		public string OrgPhone { get; set; }
		public string OrgEmail { get; set; }

		public async Task<QLDoanhNghiep> QLDoanhNghiepGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<QLDoanhNghiep>("QL_DoanhNghiepGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<QLDoanhNghiep>> QLDoanhNghiepGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<QLDoanhNghiep>("QL_DoanhNghiepGetAll", DP)).ToList();
		}

		public async Task<List<QLDoanhNghiepOnPage>> QLDoanhNghiepGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<QLDoanhNghiepOnPage>("QL_DoanhNghiepGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> QLDoanhNghiepInsert(QLDoanhNghiep _qLDoanhNghiep)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Phone", _qLDoanhNghiep.Phone);
			DP.Add("Gender", _qLDoanhNghiep.Gender);
			DP.Add("DOB", _qLDoanhNghiep.DOB);
			DP.Add("Business", _qLDoanhNghiep.Business);
			DP.Add("Tax", _qLDoanhNghiep.Tax);
			DP.Add("Status", _qLDoanhNghiep.Status);
			DP.Add("CreatedBy", _qLDoanhNghiep.CreatedBy);
			DP.Add("CreatedDate", _qLDoanhNghiep.CreatedDate);
			DP.Add("UpdatedBy", _qLDoanhNghiep.UpdatedBy);
			DP.Add("UpdatedDate", _qLDoanhNghiep.UpdatedDate);
			DP.Add("RepresentativeName", _qLDoanhNghiep.RepresentativeName);
			DP.Add("Email", _qLDoanhNghiep.Email);
			DP.Add("OrgProvince", _qLDoanhNghiep.OrgProvince);
			DP.Add("OrgDistrict", _qLDoanhNghiep.OrgDistrict);
			DP.Add("OrgVillage", _qLDoanhNghiep.OrgVillage);
			DP.Add("OrgAddress", _qLDoanhNghiep.OrgAddress);
			DP.Add("OrgPhone", _qLDoanhNghiep.OrgPhone);
			DP.Add("OrgEmail", _qLDoanhNghiep.OrgEmail);
			DP.Add("RegistrationNum", _qLDoanhNghiep.RegistrationNum);
			DP.Add("DecisionFoundation", _qLDoanhNghiep.DecisionFoundation);
			DP.Add("DateIssue", _qLDoanhNghiep.DateIssue);
			DP.Add("Nation", _qLDoanhNghiep.Nation);
			DP.Add("Province", _qLDoanhNghiep.Province);
			DP.Add("District", _qLDoanhNghiep.District);
			DP.Add("Village", _qLDoanhNghiep.Village);
			DP.Add("Address", _qLDoanhNghiep.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_DoanhNghiepInsert", DP));
		}

		public async Task<int> QLDoanhNghiepUpdate(QLDoanhNghiep _qLDoanhNghiep)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Phone", _qLDoanhNghiep.Phone);
			DP.Add("Gender", _qLDoanhNghiep.Gender);
			DP.Add("DOB", _qLDoanhNghiep.DOB);
			DP.Add("Business", _qLDoanhNghiep.Business);
			DP.Add("Tax", _qLDoanhNghiep.Tax);
			DP.Add("Status", _qLDoanhNghiep.Status);
			DP.Add("CreatedBy", _qLDoanhNghiep.CreatedBy);
			DP.Add("CreatedDate", _qLDoanhNghiep.CreatedDate);
			DP.Add("UpdatedBy", _qLDoanhNghiep.UpdatedBy);
			DP.Add("UpdatedDate", _qLDoanhNghiep.UpdatedDate);
			DP.Add("Id", _qLDoanhNghiep.Id);
			DP.Add("RepresentativeName", _qLDoanhNghiep.RepresentativeName);
			DP.Add("Email", _qLDoanhNghiep.Email);
			DP.Add("OrgProvince", _qLDoanhNghiep.OrgProvince);
			DP.Add("OrgDistrict", _qLDoanhNghiep.OrgDistrict);
			DP.Add("OrgVillage", _qLDoanhNghiep.OrgVillage);
			DP.Add("OrgAddress", _qLDoanhNghiep.OrgAddress);
			DP.Add("OrgPhone", _qLDoanhNghiep.OrgPhone);
			DP.Add("OrgEmail", _qLDoanhNghiep.OrgEmail);
			DP.Add("RegistrationNum", _qLDoanhNghiep.RegistrationNum);
			DP.Add("DecisionFoundation", _qLDoanhNghiep.DecisionFoundation);
			DP.Add("DateIssue", _qLDoanhNghiep.DateIssue);
			DP.Add("Nation", _qLDoanhNghiep.Nation);
			DP.Add("Province", _qLDoanhNghiep.Province);
			DP.Add("District", _qLDoanhNghiep.District);
			DP.Add("Village", _qLDoanhNghiep.Village);
			DP.Add("Address", _qLDoanhNghiep.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_DoanhNghiepUpdate", DP));
		}

		public async Task<int> QLDoanhNghiepDelete(QLDoanhNghiep _qLDoanhNghiep)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _qLDoanhNghiep.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_DoanhNghiepDelete", DP));
		}

		public async Task<int> QLDoanhNghiepDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_DoanhNghiepDeleteAll", DP));
		}
	}

	public class QLNguoiDanOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int Identity { get; set; }
		public string PlaceIssue { get; set; }
		public DateTime? DateIssue { get; set; }
		public int? Nation { get; set; }
		public int? Province { get; set; }
		public int? District { get; set; }
		public int? Village { get; set; }
		public string Address { get; set; }
		public bool Gender { get; set; }
		public int DOB { get; set; }
		public byte Status { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public int UpdatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }
		public int? RowNumber; // int, null
	}

	public class QLNguoiDan
	{
		private SQLCon _sQLCon;

		public QLNguoiDan(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public QLNguoiDan()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int Identity { get; set; }
		public string PlaceIssue { get; set; }
		public DateTime? DateIssue { get; set; }
		public int? Nation { get; set; }
		public int? Province { get; set; }
		public int? District { get; set; }
		public int? Village { get; set; }
		public string Address { get; set; }
		public bool Gender { get; set; }
		public int DOB { get; set; }
		public byte Status { get; set; }
		public int CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; }
		public int UpdatedBy { get; set; }
		public DateTime UpdatedDate { get; set; }

		public async Task<QLNguoiDan> QLNguoiDanGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<QLNguoiDan>("QL_NguoiDanGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<QLNguoiDan>> QLNguoiDanGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<QLNguoiDan>("QL_NguoiDanGetAll", DP)).ToList();
		}

		public async Task<List<QLNguoiDanOnPage>> QLNguoiDanGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<QLNguoiDanOnPage>("QL_NguoiDanGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> QLNguoiDanInsert(QLNguoiDan _qLNguoiDan)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Phone", _qLNguoiDan.Phone);
			DP.Add("Identity", _qLNguoiDan.Identity);
			DP.Add("Gender", _qLNguoiDan.Gender);
			DP.Add("DOB", _qLNguoiDan.DOB);
			DP.Add("Status", _qLNguoiDan.Status);
			DP.Add("CreatedBy", _qLNguoiDan.CreatedBy);
			DP.Add("CreatedDate", _qLNguoiDan.CreatedDate);
			DP.Add("UpdatedBy", _qLNguoiDan.UpdatedBy);
			DP.Add("UpdatedDate", _qLNguoiDan.UpdatedDate);
			DP.Add("Name", _qLNguoiDan.Name);
			DP.Add("Email", _qLNguoiDan.Email);
			DP.Add("PlaceIssue", _qLNguoiDan.PlaceIssue);
			DP.Add("DateIssue", _qLNguoiDan.DateIssue);
			DP.Add("Nation", _qLNguoiDan.Nation);
			DP.Add("Province", _qLNguoiDan.Province);
			DP.Add("District", _qLNguoiDan.District);
			DP.Add("Village", _qLNguoiDan.Village);
			DP.Add("Address", _qLNguoiDan.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_NguoiDanInsert", DP));
		}

		public async Task<int> QLNguoiDanUpdate(QLNguoiDan _qLNguoiDan)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Phone", _qLNguoiDan.Phone);
			DP.Add("Identity", _qLNguoiDan.Identity);
			DP.Add("Gender", _qLNguoiDan.Gender);
			DP.Add("DOB", _qLNguoiDan.DOB);
			DP.Add("Status", _qLNguoiDan.Status);
			DP.Add("CreatedBy", _qLNguoiDan.CreatedBy);
			DP.Add("CreatedDate", _qLNguoiDan.CreatedDate);
			DP.Add("UpdatedBy", _qLNguoiDan.UpdatedBy);
			DP.Add("UpdatedDate", _qLNguoiDan.UpdatedDate);
			DP.Add("Id", _qLNguoiDan.Id);
			DP.Add("Name", _qLNguoiDan.Name);
			DP.Add("Email", _qLNguoiDan.Email);
			DP.Add("PlaceIssue", _qLNguoiDan.PlaceIssue);
			DP.Add("DateIssue", _qLNguoiDan.DateIssue);
			DP.Add("Nation", _qLNguoiDan.Nation);
			DP.Add("Province", _qLNguoiDan.Province);
			DP.Add("District", _qLNguoiDan.District);
			DP.Add("Village", _qLNguoiDan.Village);
			DP.Add("Address", _qLNguoiDan.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_NguoiDanUpdate", DP));
		}

		public async Task<int> QLNguoiDanDelete(QLNguoiDan _qLNguoiDan)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _qLNguoiDan.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_NguoiDanDelete", DP));
		}

		public async Task<int> QLNguoiDanDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("QL_NguoiDanDeleteAll", DP));
		}
	}
}
