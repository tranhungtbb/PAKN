using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.BusinessIndividual
{
	#region BI_IndividualGetAllOnPage
	public class BI_IndividualGetAllOnPage
	{
		private SQLCon _sQLCon;

		public BI_IndividualGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_IndividualGetAllOnPage()
		{
		}
		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public int? Status { get; set; }


		public async Task<List<BI_IndividualGetAllOnPage>> BI_IndividualGetAllOnPageDAO(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, int? Status, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FullName", FullName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("Status", Status);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

			return (await _sQLCon.ExecuteListDapperAsync<BI_IndividualGetAllOnPage>("BI_IndividualGetAllOnPage", DP)).ToList();
		}
	}
	#endregion

	#region BI_IndivialDelete
	public class BI_IndivialDelete
	{
		private SQLCon _sQLCon;

		public BI_IndivialDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_IndivialDelete()
		{
		}

		public async Task<int> BI_IndivialDeleteDAO(BI_IndivialDeleteIN _bi_IndivialDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _bi_IndivialDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualDelete", DP));
		}
	}

	public class BI_IndivialDeleteIN
	{
		public int? Id { get; set; }
	}

	#endregion

	#region BI_IndivialChageStatus

	public class BI_IndivialChageStatus
	{
		private SQLCon _sQLCon;

		public BI_IndivialChageStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_IndivialChageStatus()
		{
		}

		public async Task<int> IndivialChageStatusDAO(BI_IndivialChageStatusIN _indivialChageStatusIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _indivialChageStatusIN.Id);
			DP.Add("Status", _indivialChageStatusIN.Status);;

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualChageStatus", DP));
		}
	}

	public class BI_IndivialChageStatusIN
	{
		public int? Id { get; set; }
		public byte? Status { get; set; }
	}

	#endregion

	#region BI_IndividualCheckExists
	public class BI_IndividualCheckExists
	{
		private SQLCon _sQLCon;

		public BI_IndividualCheckExists(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_IndividualCheckExists()
		{
		}

		public bool? Exists { get; set; }
		public string Value { get; set; }
		public long? Id { get; set; }

		public async Task<List<BI_IndividualCheckExists>> BIIndividualCheckExistsDAO(string Field, string Value, long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Field", Field);
			DP.Add("Value", Value);
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BI_IndividualCheckExists>("BI_Individual_CheckExists", DP)).ToList();
		}
	}

	#endregion

	#region BIIndividualInsert

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
		public long? UserId { get; set; }
	}

	#endregion

	#region BI_InvididualUpdate

	public class BI_InvididualUpdate
	{
		private SQLCon _sQLCon;

		public BI_InvididualUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_InvididualUpdate()
		{
		}

		public async Task<int> BI_InvididualUpdateDAO(BI_InvididualUpdateIN _bI_InvididualUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			//DP.Add("Id", _bI_InvididualUpdateIN.Id);
			DP.Add("FullName", _bI_InvididualUpdateIN.FullName);
			DP.Add("DateOfBirth", _bI_InvididualUpdateIN.BirthDay);
			DP.Add("Email", _bI_InvididualUpdateIN.Email);
			DP.Add("Nation", _bI_InvididualUpdateIN.Nation);
			DP.Add("ProvinceId", _bI_InvididualUpdateIN.ProvinceId);
			DP.Add("DistrictId", _bI_InvididualUpdateIN.DistrictId);
			DP.Add("WardsId", _bI_InvididualUpdateIN.WardsId);
			DP.Add("Address", _bI_InvididualUpdateIN.Address);
			DP.Add("IdCard", _bI_InvididualUpdateIN.IDCard);
			DP.Add("IssuedPlace", _bI_InvididualUpdateIN.IssuedPlace);
			DP.Add("IssuedDate", _bI_InvididualUpdateIN.DateOfIssue);
			DP.Add("Gender", _bI_InvididualUpdateIN.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_InvididualUpdate", DP));
		}
	}

	public class BI_InvididualUpdateIN
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
		public long? UserId { get; set; }
	}

	#endregion

	#region BI_InvididualGetByID
	public class BI_InvididualGetByID
	{
		private SQLCon _sQLCon;

		public BI_InvididualGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BI_InvididualGetByID()
		{
		}
		public long? Id { get; set; }
		public string FullName { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public int? ProvinceId { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public DateTime? BirthDay { get; set; }
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
		public bool? Gender { get; set; }
		public long? UserId { get; set; }

		public async Task<List<BI_InvididualGetByID>> BI_InvididualGetByIDDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BI_InvididualGetByID>("BI_IndividualGetByID", DP)).ToList();
		}
	}
	#endregion

}
