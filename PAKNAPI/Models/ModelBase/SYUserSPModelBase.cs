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
	public class SYUserRoleMapInsert
	{
		private SQLCon _sQLCon;

		public SYUserRoleMapInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserRoleMapInsert()
		{
		}

		public async Task<int?> SYUserRoleMapInsertDAO(SYUserRoleMapInsertIN _sYUserRoleMapInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYUserRoleMapInsertIN.UserId);
			DP.Add("RoleId", _sYUserRoleMapInsertIN.RoleId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_User_Role_MapInsert", DP);
		}
	}

	public class SYUserRoleMapInsertIN
	{
		public long? UserId { get; set; }
		public long? RoleId { get; set; }
	}

	public class SYUserChangePwd
	{
		private SQLCon _sQLCon;

		public SYUserChangePwd(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserChangePwd()
		{
		}

		public async Task<int> SYUserChangePwdDAO(SYUserChangePwdIN _sYUserChangePwdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserChangePwdIN.Id);
			DP.Add("Password", _sYUserChangePwdIN.Password);
			DP.Add("Salt", _sYUserChangePwdIN.Salt);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserChangePwd", DP));
		}
	}

	public class SYUserChangePwdIN
	{
		public long? Id { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
	}

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

	public class SYUserGetAllByRoleId
	{
		private SQLCon _sQLCon;

		public SYUserGetAllByRoleId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetAllByRoleId()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public byte Type { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public string Address { get; set; }

		public async Task<List<SYUserGetAllByRoleId>> SYUserGetAllByRoleIdDAO(int? RoleId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoleId", RoleId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetAllByRoleId>("SY_UserGetAllByRoleId", DP)).ToList();
		}
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

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }

		public async Task<List<SYUserGetAllOnPage>> SYUserGetAllOnPageDAO(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActived, int? UnitId, int? TypeId, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("UserName", UserName);
			DP.Add("FullName", FullName);
			DP.Add("Phone", Phone);
			DP.Add("IsActived", IsActived);
			DP.Add("UnitId", UnitId);
			DP.Add("TypeId", TypeId);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

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

		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }
		public int TypeId { get; set; }
		public string RoleIds { get; set; }

		public async Task<List<SYUserGetByID>> SYUserGetByIDDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByID>("SY_UserGetByID", DP)).ToList();
		}
	}

	public class SYUserGetByRoleIdAllOnPage
	{
		private SQLCon _sQLCon;

		public SYUserGetByRoleIdAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetByRoleIdAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public byte Type { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public string Address { get; set; }

		public async Task<List<SYUserGetByRoleIdAllOnPage>> SYUserGetByRoleIdAllOnPageDAO(int? PageSize, int? PageIndex, int? RoleId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("RoleId", RoleId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByRoleIdAllOnPage>("SY_UserGetByRoleIdAllOnPage", DP)).ToList();
		}
	}

	public class SYUserGetIsSystem
	{
		private SQLCon _sQLCon;

		public SYUserGetIsSystem(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetIsSystem()
		{
		}

		public long value { get; set; }
		public string text { get; set; }

		public async Task<List<SYUserGetIsSystem>> SYUserGetIsSystemDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetIsSystem>("SY_UserGetIsSystem", DP)).ToList();
		}
	}

	public class SYUserGetIsSystem2
	{
		private SQLCon _sQLCon;

		public SYUserGetIsSystem2(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetIsSystem2()
		{
		}

		public long? Id { get; set; }
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string UnitName { get; set; }
		public string PositionName { get; set; }
		public string Avatar { get; set; }

		public async Task<List<SYUserGetIsSystem2>> SYUserGetIsSystem2DAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetIsSystem2>("SY_UserGetIsSystem2", DP)).ToList();
		}
	}

	public class SYUserGetNameById
	{
		private SQLCon _sQLCon;

		public SYUserGetNameById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetNameById()
		{
		}

		public string FullName { get; set; }

		public async Task<List<SYUserGetNameById>> SYUserGetNameByIdDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetNameById>("SY_UserGetNameById", DP)).ToList();
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
			DP.Add("TypeId", _sYUserInsertIN.TypeId);
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
		public int? TypeId { get; set; }
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

	public class SYUserRoleMapDelete
	{
		private SQLCon _sQLCon;

		public SYUserRoleMapDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserRoleMapDelete()
		{
		}

		public async Task<int> SYUserRoleMapDeleteDAO(SYUserRoleMapDeleteIN _sYUserRoleMapDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYUserRoleMapDeleteIN.UserId);
			DP.Add("RoleId", _sYUserRoleMapDeleteIN.RoleId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserRoleMapDelete", DP));
		}
	}

	public class SYUserRoleMapDeleteIN
	{
		public long? UserId { get; set; }
		public long? RoleId { get; set; }
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
			DP.Add("TypeId", _sYUserUpdateIN.TypeId);
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
		public int? TypeId { get; set; }
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

	public class SYUserUpdateInfo
	{
		private SQLCon _sQLCon;

		public SYUserUpdateInfo(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserUpdateInfo()
		{
		}

		public async Task<int> SYUserUpdateInfoDAO(SYUserUpdateInfoIN _sYUserUpdateInfoIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserUpdateInfoIN.Id);
			DP.Add("FullName", _sYUserUpdateInfoIN.FullName);
			DP.Add("Address", _sYUserUpdateInfoIN.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUpdateInfo", DP));
		}
	}

	public class SYUserUpdateInfoIN
	{
		public long? Id { get; set; }
		public string FullName { get; set; }
		public string Address { get; set; }
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

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

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

		public long Id { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Avatar { get; set; }
		public bool Gender { get; set; }
		public string Phone { get; set; }
		public int? PositionId { get; set; }
		public byte Type { get; set; }
		public string PositionName { get; set; }
		public int? UnitId { get; set; }
		public string UnitName { get; set; }
		public int TypeId { get; set; }
		public bool? IsMain { get; set; }
		public int TypeObject { get; set; }

		public async Task<List<SYUSRLogin>> SYUSRLoginDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteListDapperAsync<SYUSRLogin>("SY_USR_Login", DP)).ToList();
		}
	}
}
