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
	public class SYUserDelete
	{
		private SQLCon _sQLCon;

		public SYUserDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserDelete()
		{
		}

		public async Task<int> SYUserDeleteDAO(SYUserDeleteIN _sYUserDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserDelete", DP));
		}
	}

	public class SYUserDeleteIN
	{
		public long? Id { get; set; }
	}

	public class SYUserGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYUserGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetAllOnPage()
		{
		}

		public int? RowNumber;
		public long Id;
		public string FullName;
		public string UserName;
		public string Password;
		public string Salt;
		public bool IsActived;
		public bool IsDeleted;
		public bool Gender;
		public byte Type;
		public bool IsSuperAdmin;
		public string Email;
		public string Phone;
		public int? UnitId;
		public byte? CountLock;
		public DateTime? LockEndOut;
		public string Avatar;
		public string Address;
		public int? PositionId;

		public async Task<List<SYUserGetAllOnPage>> SYUserGetAllOnPageDAO(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActive, int? UnitId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("UserName", UserName);
			DP.Add("FullName", FullName);
			DP.Add("Phone", Phone);
			DP.Add("IsActive", IsActive);
			DP.Add("UnitId", UnitId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetAllOnPage>("SY_UserGetAllOnPage", DP)).ToList();
		}
	}

	public class SYUserGetByID
	{
		private SQLCon _sQLCon;

		public SYUserGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetByID()
		{
		}

		public long Id;
		public string FullName;
		public string UserName;
		public string Password;
		public string Salt;
		public bool IsActived;
		public bool IsDeleted;
		public bool Gender;
		public byte Type;
		public bool IsSuperAdmin;
		public string Email;
		public string Phone;
		public int? UnitId;
		public byte? CountLock;
		public DateTime? LockEndOut;
		public string Avatar;
		public string Address;
		public int? PositionId;
		public string RoleIds;

		public async Task<List<SYUserGetByID>> SYUserGetByIDDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByID>("SY_UserGetByID", DP)).ToList();
		}
	}

	public class SYUserInsert
	{
		private SQLCon _sQLCon;

		public SYUserInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserInsert()
		{
		}

		public async Task<int> SYUserInsertDAO(SYUserInsertIN _sYUserInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FullName", _sYUserInsertIN.FullName);
			DP.Add("UserName", _sYUserInsertIN.UserName);
			DP.Add("Password", _sYUserInsertIN.Password);
			DP.Add("Salt", _sYUserInsertIN.Salt);
			DP.Add("IsActived", _sYUserInsertIN.IsActived);
			DP.Add("IsDeleted", _sYUserInsertIN.IsDeleted);
			DP.Add("Gender", _sYUserInsertIN.Gender);
			DP.Add("Type", _sYUserInsertIN.Type);
			DP.Add("IsSuperAdmin", _sYUserInsertIN.IsSuperAdmin);
			DP.Add("Email", _sYUserInsertIN.Email);
			DP.Add("Phone", _sYUserInsertIN.Phone);
			DP.Add("UnitId", _sYUserInsertIN.UnitId);
			DP.Add("CountLock", _sYUserInsertIN.CountLock);
			DP.Add("LockEndOut", _sYUserInsertIN.LockEndOut);
			DP.Add("Avatar", _sYUserInsertIN.Avatar);
			DP.Add("Address", _sYUserInsertIN.Address);
			DP.Add("PositionId", _sYUserInsertIN.PositionId);
			DP.Add("RoleIds", _sYUserInsertIN.RoleIds);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserInsert", DP));
		}
	}

	public class SYUserInsertIN
	{
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? Gender { get; set; }
		public byte? Type { get; set; }
		public bool? IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }
		public string RoleIds { get; set; }
	}

	public class SYUserUpdate
	{
		private SQLCon _sQLCon;

		public SYUserUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserUpdate()
		{
		}

		public async Task<int> SYUserUpdateDAO(SYUserUpdateIN _sYUserUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserUpdateIN.Id);
			DP.Add("FullName", _sYUserUpdateIN.FullName);
			DP.Add("UserName", _sYUserUpdateIN.UserName);
			DP.Add("Password", _sYUserUpdateIN.Password);
			DP.Add("Salt", _sYUserUpdateIN.Salt);
			DP.Add("IsActived", _sYUserUpdateIN.IsActived);
			DP.Add("IsDeleted", _sYUserUpdateIN.IsDeleted);
			DP.Add("Gender", _sYUserUpdateIN.Gender);
			DP.Add("Type", _sYUserUpdateIN.Type);
			DP.Add("IsSuperAdmin", _sYUserUpdateIN.IsSuperAdmin);
			DP.Add("Email", _sYUserUpdateIN.Email);
			DP.Add("Phone", _sYUserUpdateIN.Phone);
			DP.Add("UnitId", _sYUserUpdateIN.UnitId);
			DP.Add("CountLock", _sYUserUpdateIN.CountLock);
			DP.Add("LockEndOut", _sYUserUpdateIN.LockEndOut);
			DP.Add("Avatar", _sYUserUpdateIN.Avatar);
			DP.Add("Address", _sYUserUpdateIN.Address);
			DP.Add("PositionId", _sYUserUpdateIN.PositionId);
			DP.Add("RoleIds", _sYUserUpdateIN.RoleIds);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUpdate", DP));
		}
	}

	public class SYUserUpdateIN
	{
		public long? Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? Gender { get; set; }
		public byte? Type { get; set; }
		public bool? IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }
		public string RoleIds { get; set; }
	}

	public class SYUSRGetPermissionByUserId
	{
		private SQLCon _sQLCon;

		public SYUSRGetPermissionByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUSRGetPermissionByUserId()
		{
		}

		public short Id;
		public string Name;
		public string Code;

		public async Task<List<SYUSRGetPermissionByUserId>> SYUSRGetPermissionByUserIdDAO(long? UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUSRGetPermissionByUserId>("SY_USR_GetPermissionByUserId", DP)).ToList();
		}
	}

	public class SYUSRLogin
	{
		private SQLCon _sQLCon;

		public SYUSRLogin(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUSRLogin()
		{
		}

		public long Id;
		public string Password;
		public string Salt;
		public string FullName;
		public string UserName;
		public string Address;
		public string Email;
		public string Avatar;
		public bool Gender;
		public string Phone;
		public int? PositionId;
		public string PositionName;
		public int? UnitId;
		public string UnitName;

		public async Task<List<SYUSRLogin>> SYUSRLoginDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteListDapperAsync<SYUSRLogin>("SY_USR_Login", DP)).ToList();
		}
	}
}
