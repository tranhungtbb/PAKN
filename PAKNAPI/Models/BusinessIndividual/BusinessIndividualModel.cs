using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.BusinessIndividual
{
	#region IndividualGetAllOnPage
	public class IndividualGetAllOnPage
	{
		private SQLCon _sQLCon;

		public IndividualGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public IndividualGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }


		public async Task<List<IndividualGetAllOnPage>> IndividualGetAllOnPageDAO(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FullName", FullName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("IsActived", IsActived);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

			return (await _sQLCon.ExecuteListDapperAsync<IndividualGetAllOnPage>("BI_IndividualGetAllOnPage", DP)).ToList();
		}
	}
	#endregion

	#region IndivialDelete
	public class IndivialDelete
	{
		private SQLCon _sQLCon;

		public IndivialDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public IndivialDelete()
		{
		}

		public async Task<int> IndivialDeleteDAO(IndivialDeleteIN _indivialDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _indivialDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualDelete", DP));
		}
	}

	public class IndivialDeleteIN
	{
		public int? Id { get; set; }
	}

	#endregion

	#region IndivialChageStatus

	public class IndivialChageStatus
	{
		private SQLCon _sQLCon;

		public IndivialChageStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public IndivialChageStatus()
		{
		}

		public async Task<int> IndivialChageStatusDAO(IndivialChageStatusIN _indivialChageStatusIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _indivialChageStatusIN.Id);
			DP.Add("IsActived", _indivialChageStatusIN.IsActived);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BI_IndividualChageStatus", DP));
		}
	}

	public class IndivialChageStatusIN
	{
		public int? Id { get; set; }
		public bool? IsActived { get; set; }
	}

	#endregion

	// Individual
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
		public long? Id { get; set; }

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

	public class InvididualUpdate
	{
		private SQLCon _sQLCon;

		public InvididualUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public InvididualUpdate()
		{
		}

		public async Task<int> InvididualUpdateDAO(InvididualUpdateIN _invididualUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _invididualUpdateIN.Id);
			DP.Add("FullName", _invididualUpdateIN.FullName);
			DP.Add("DateOfBirth", _invididualUpdateIN.DateOfBirth);
			DP.Add("Email", _invididualUpdateIN.Email);
			DP.Add("Nation", _invididualUpdateIN.Nation);
			DP.Add("ProvinceId", _invididualUpdateIN.ProvinceId);
			DP.Add("DistrictId", _invididualUpdateIN.DistrictId);
			DP.Add("WardsId", _invididualUpdateIN.WardsId);
			DP.Add("Address", _invididualUpdateIN.Address);
			DP.Add("IdCard", _invididualUpdateIN.IdCard);
			DP.Add("IssuedPlace", _invididualUpdateIN.IssuedPlace);
			DP.Add("IssuedDate", _invididualUpdateIN.IssuedDate);
			DP.Add("Gender", _invididualUpdateIN.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("InvididualUpdate", DP));
		}
	}

	public class InvididualUpdateIN
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

	#region InvididualGetByID
	public class InvididualGetByID
	{
		private SQLCon _sQLCon;

		public InvididualGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public InvididualGetByID()
		{
		}

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

		public async Task<List<InvididualGetByID>> InvididualGetByIDDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<InvididualGetByID>("BI_IndividualGetByID", DP)).ToList();
		}
	}
	#endregion

}
