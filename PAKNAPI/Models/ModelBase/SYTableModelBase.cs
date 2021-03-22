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
	public class SYGroupUserOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int? UnitId { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYGroupUser
	{
		private SQLCon _sQLCon;

		public SYGroupUser(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYGroupUser()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int? UnitId { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<SYGroupUser> SYGroupUserGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYGroupUser>("SY_GroupUserGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYGroupUser>> SYGroupUserGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYGroupUser>("SY_GroupUserGetAll", DP)).ToList();
		}

		public async Task<List<SYGroupUserOnPage>> SYGroupUserGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYGroupUserOnPage>("SY_GroupUserGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYGroupUserInsert(SYGroupUser _sYGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYGroupUser.Name);
			DP.Add("Code", _sYGroupUser.Code);
			DP.Add("IsActived", _sYGroupUser.IsActived);
			DP.Add("IsDeleted", _sYGroupUser.IsDeleted);
			DP.Add("UnitId", _sYGroupUser.UnitId);
			DP.Add("CreatedBy", _sYGroupUser.CreatedBy);
			DP.Add("CreatedDate", _sYGroupUser.CreatedDate);
			DP.Add("Description", _sYGroupUser.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_GroupUserInsert", DP));
		}

		public async Task<int> SYGroupUserUpdate(SYGroupUser _sYGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYGroupUser.Id);
			DP.Add("Name", _sYGroupUser.Name);
			DP.Add("Code", _sYGroupUser.Code);
			DP.Add("IsActived", _sYGroupUser.IsActived);
			DP.Add("IsDeleted", _sYGroupUser.IsDeleted);
			DP.Add("UnitId", _sYGroupUser.UnitId);
			DP.Add("CreatedBy", _sYGroupUser.CreatedBy);
			DP.Add("CreatedDate", _sYGroupUser.CreatedDate);
			DP.Add("Description", _sYGroupUser.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_GroupUserUpdate", DP));
		}

		public async Task<int> SYGroupUserDelete(SYGroupUser _sYGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYGroupUser.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_GroupUserDelete", DP));
		}

		public async Task<int> SYGroupUserDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_GroupUserDeleteAll", DP));
		}
	}

	public class SYPermissionOnPage
	{
		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short FunctionId { get; set; }
		public short? ParentId { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYPermission
	{
		private SQLCon _sQLCon;

		public SYPermission(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermission()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short FunctionId { get; set; }
		public short? ParentId { get; set; }

		public async Task<SYPermission> SYPermissionGetByID(short? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermission>("SY_PermissionGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYPermission>> SYPermissionGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermission>("SY_PermissionGetAll", DP)).ToList();
		}

		public async Task<List<SYPermissionOnPage>> SYPermissionGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionOnPage>("SY_PermissionGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYPermissionInsert(SYPermission _sYPermission)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYPermission.Name);
			DP.Add("Code", _sYPermission.Code);
			DP.Add("FunctionId", _sYPermission.FunctionId);
			DP.Add("ParentId", _sYPermission.ParentId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionInsert", DP));
		}

		public async Task<int> SYPermissionUpdate(SYPermission _sYPermission)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYPermission.Id);
			DP.Add("Name", _sYPermission.Name);
			DP.Add("Code", _sYPermission.Code);
			DP.Add("FunctionId", _sYPermission.FunctionId);
			DP.Add("ParentId", _sYPermission.ParentId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionUpdate", DP));
		}

		public async Task<int> SYPermissionDelete(SYPermission _sYPermission)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYPermission.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionDelete", DP));
		}

		public async Task<int> SYPermissionDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionDeleteAll", DP));
		}
	}

	public class SYPermissionCategoryOnPage
	{
		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYPermissionCategory
	{
		private SQLCon _sQLCon;

		public SYPermissionCategory(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionCategory()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		public async Task<SYPermissionCategory> SYPermissionCategoryGetByID(short? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCategory>("SY_PermissionCategoryGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYPermissionCategory>> SYPermissionCategoryGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCategory>("SY_PermissionCategoryGetAll", DP)).ToList();
		}

		public async Task<List<SYPermissionCategoryOnPage>> SYPermissionCategoryGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCategoryOnPage>("SY_PermissionCategoryGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYPermissionCategoryInsert(SYPermissionCategory _sYPermissionCategory)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYPermissionCategory.Name);
			DP.Add("Code", _sYPermissionCategory.Code);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionCategoryInsert", DP));
		}

		public async Task<int> SYPermissionCategoryUpdate(SYPermissionCategory _sYPermissionCategory)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYPermissionCategory.Id);
			DP.Add("Name", _sYPermissionCategory.Name);
			DP.Add("Code", _sYPermissionCategory.Code);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionCategoryUpdate", DP));
		}

		public async Task<int> SYPermissionCategoryDelete(SYPermissionCategory _sYPermissionCategory)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYPermissionCategory.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionCategoryDelete", DP));
		}

		public async Task<int> SYPermissionCategoryDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionCategoryDeleteAll", DP));
		}
	}

	public class SYPermissionFunctionOnPage
	{
		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short CategoryId { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYPermissionFunction
	{
		private SQLCon _sQLCon;

		public SYPermissionFunction(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionFunction()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short CategoryId { get; set; }

		public async Task<SYPermissionFunction> SYPermissionFunctionGetByID(short? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionFunction>("SY_PermissionFunctionGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYPermissionFunction>> SYPermissionFunctionGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionFunction>("SY_PermissionFunctionGetAll", DP)).ToList();
		}

		public async Task<List<SYPermissionFunctionOnPage>> SYPermissionFunctionGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionFunctionOnPage>("SY_PermissionFunctionGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYPermissionFunctionInsert(SYPermissionFunction _sYPermissionFunction)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("CategoryId", _sYPermissionFunction.CategoryId);
			DP.Add("Name", _sYPermissionFunction.Name);
			DP.Add("Code", _sYPermissionFunction.Code);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionFunctionInsert", DP));
		}

		public async Task<int> SYPermissionFunctionUpdate(SYPermissionFunction _sYPermissionFunction)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYPermissionFunction.Id);
			DP.Add("CategoryId", _sYPermissionFunction.CategoryId);
			DP.Add("Name", _sYPermissionFunction.Name);
			DP.Add("Code", _sYPermissionFunction.Code);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionFunctionUpdate", DP));
		}

		public async Task<int> SYPermissionFunctionDelete(SYPermissionFunction _sYPermissionFunction)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYPermissionFunction.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionFunctionDelete", DP));
		}

		public async Task<int> SYPermissionFunctionDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionFunctionDeleteAll", DP));
		}
	}

	public class SYPermissionGroupUserOnPage
	{
		public short PermissionId { get; set; }
		public short GroupUserId { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYPermissionGroupUser
	{
		private SQLCon _sQLCon;

		public SYPermissionGroupUser(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionGroupUser()
		{
		}

		public short PermissionId { get; set; }
		public short GroupUserId { get; set; }

		public async Task<SYPermissionGroupUser> SYPermissionGroupUserGetByID(short? PermissionId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PermissionId", PermissionId);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionGroupUser>("SY_PermissionGroupUserGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYPermissionGroupUser>> SYPermissionGroupUserGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionGroupUser>("SY_PermissionGroupUserGetAll", DP)).ToList();
		}

		public async Task<List<SYPermissionGroupUserOnPage>> SYPermissionGroupUserGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionGroupUserOnPage>("SY_PermissionGroupUserGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYPermissionGroupUserInsert(SYPermissionGroupUser _sYPermissionGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionGroupUserInsert", DP));
		}

		public async Task<int> SYPermissionGroupUserUpdate(SYPermissionGroupUser _sYPermissionGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PermissionId", _sYPermissionGroupUser.PermissionId);
			DP.Add("GroupUserId", _sYPermissionGroupUser.GroupUserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionGroupUserUpdate", DP));
		}

		public async Task<int> SYPermissionGroupUserDelete(SYPermissionGroupUser _sYPermissionGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PermissionId", _sYPermissionGroupUser.PermissionId);
			DP.Add("GroupUserId", _sYPermissionGroupUser.GroupUserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionGroupUserDelete", DP));
		}

		public async Task<int> SYPermissionGroupUserDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionGroupUserDeleteAll", DP));
		}
	}

	public class SYPermissionUserOnPage
	{
		public int UserId { get; set; }
		public short PermissionId { get; set; }
		public short FunctionId { get; set; }
		public short CategoryId { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYPermissionUser
	{
		private SQLCon _sQLCon;

		public SYPermissionUser(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionUser()
		{
		}

		public int UserId { get; set; }
		public short PermissionId { get; set; }
		public short FunctionId { get; set; }
		public short CategoryId { get; set; }

		public async Task<SYPermissionUser> SYPermissionUserGetByID(int? UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionUser>("SY_PermissionUserGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYPermissionUser>> SYPermissionUserGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionUser>("SY_PermissionUserGetAll", DP)).ToList();
		}

		public async Task<List<SYPermissionUserOnPage>> SYPermissionUserGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionUserOnPage>("SY_PermissionUserGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYPermissionUserInsert(SYPermissionUser _sYPermissionUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FunctionId", _sYPermissionUser.FunctionId);
			DP.Add("CategoryId", _sYPermissionUser.CategoryId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionUserInsert", DP));
		}

		public async Task<int> SYPermissionUserUpdate(SYPermissionUser _sYPermissionUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYPermissionUser.UserId);
			DP.Add("PermissionId", _sYPermissionUser.PermissionId);
			DP.Add("FunctionId", _sYPermissionUser.FunctionId);
			DP.Add("CategoryId", _sYPermissionUser.CategoryId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionUserUpdate", DP));
		}

		public async Task<int> SYPermissionUserDelete(SYPermissionUser _sYPermissionUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYPermissionUser.UserId);
			DP.Add("PermissionId", _sYPermissionUser.PermissionId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionUserDelete", DP));
		}

		public async Task<int> SYPermissionUserDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionUserDeleteAll", DP));
		}
	}

	public class SYRoleOnPage
	{
		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYRole
	{
		private SQLCon _sQLCon;

		public SYRole(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRole()
		{
		}

		public int Id { get; set; }
		public string OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<SYRole> SYRoleGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYRole>("SY_RoleGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYRole>> SYRoleGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYRole>("SY_RoleGetAll", DP)).ToList();
		}

		public async Task<List<SYRoleOnPage>> SYRoleGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYRoleOnPage>("SY_RoleGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYRoleInsert(SYRole _sYRole)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYRole.Name);
			DP.Add("Code", _sYRole.Code);
			DP.Add("IsActived", _sYRole.IsActived);
			DP.Add("IsDeleted", _sYRole.IsDeleted);
			DP.Add("Description", _sYRole.Description);
			DP.Add("OrderNumber", _sYRole.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_RoleInsert", DP));
		}

		public async Task<int> SYRoleUpdate(SYRole _sYRole)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYRole.Id);
			DP.Add("Name", _sYRole.Name);
			DP.Add("Code", _sYRole.Code);
			DP.Add("IsActived", _sYRole.IsActived);
			DP.Add("IsDeleted", _sYRole.IsDeleted);
			DP.Add("Description", _sYRole.Description);
			DP.Add("OrderNumber", _sYRole.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_RoleUpdate", DP));
		}

		public async Task<int> SYRoleDelete(SYRole _sYRole)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYRole.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_RoleDelete", DP));
		}

		public async Task<int> SYRoleDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_RoleDeleteAll", DP));
		}
	}

	public class SYSystemLogOnPage
	{
		public long Id { get; set; }
		public long UserId { get; set; }
		public string FullName { get; set; }
		public string IPAddress { get; set; }
		public string MACAddress { get; set; }
		public string Description { get; set; }
		public DateTime? CreatedDate { get; set; }
		public byte Status { get; set; }
		public string Action { get; set; }
		public string Exception { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYSystemLog
	{
		private SQLCon _sQLCon;

		public SYSystemLog(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLog()
		{
		}

		public long Id { get; set; }
		public long UserId { get; set; }
		public string FullName { get; set; }
		public string IPAddress { get; set; }
		public string MACAddress { get; set; }
		public string Description { get; set; }
		public DateTime? CreatedDate { get; set; }
		public byte Status { get; set; }
		public string Action { get; set; }
		public string Exception { get; set; }

		public async Task<SYSystemLog> SYSystemLogGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLog>("SY_SystemLogGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYSystemLog>> SYSystemLogGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLog>("SY_SystemLogGetAll", DP)).ToList();
		}

		public async Task<List<SYSystemLogOnPage>> SYSystemLogGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLogOnPage>("SY_SystemLogGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYSystemLogInsert(SYSystemLog _sYSystemLog)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYSystemLog.UserId);
			DP.Add("Status", _sYSystemLog.Status);
			DP.Add("Action", _sYSystemLog.Action);
			DP.Add("Exception", _sYSystemLog.Exception);
			DP.Add("FullName", _sYSystemLog.FullName);
			DP.Add("IPAddress", _sYSystemLog.IPAddress);
			DP.Add("MACAddress", _sYSystemLog.MACAddress);
			DP.Add("Description", _sYSystemLog.Description);
			DP.Add("CreatedDate", _sYSystemLog.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_SystemLogInsert", DP));
		}

		public async Task<int> SYSystemLogUpdate(SYSystemLog _sYSystemLog)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYSystemLog.Id);
			DP.Add("UserId", _sYSystemLog.UserId);
			DP.Add("Status", _sYSystemLog.Status);
			DP.Add("Action", _sYSystemLog.Action);
			DP.Add("Exception", _sYSystemLog.Exception);
			DP.Add("FullName", _sYSystemLog.FullName);
			DP.Add("IPAddress", _sYSystemLog.IPAddress);
			DP.Add("MACAddress", _sYSystemLog.MACAddress);
			DP.Add("Description", _sYSystemLog.Description);
			DP.Add("CreatedDate", _sYSystemLog.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_SystemLogUpdate", DP));
		}

		public async Task<int> SYSystemLogDelete(SYSystemLog _sYSystemLog)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYSystemLog.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_SystemLogDelete", DP));
		}

		public async Task<int> SYSystemLogDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_SystemLogDeleteAll", DP));
		}
	}

	public class SYUnitOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public byte UnitLevel { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYUnit
	{
		private SQLCon _sQLCon;

		public SYUnit(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnit()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public byte UnitLevel { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<SYUnit> SYUnitGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUnit>("SY_UnitGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYUnit>> SYUnitGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnit>("SY_UnitGetAll", DP)).ToList();
		}

		public async Task<List<SYUnitOnPage>> SYUnitGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYUnitOnPage>("SY_UnitGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYUnitInsert(SYUnit _sYUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYUnit.Name);
			DP.Add("UnitLevel", _sYUnit.UnitLevel);
			DP.Add("IsActived", _sYUnit.IsActived);
			DP.Add("IsDeleted", _sYUnit.IsDeleted);
			DP.Add("ParentId", _sYUnit.ParentId);
			DP.Add("Description", _sYUnit.Description);
			DP.Add("Email", _sYUnit.Email);
			DP.Add("Phone", _sYUnit.Phone);
			DP.Add("Address", _sYUnit.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UnitInsert", DP));
		}

		public async Task<int> SYUnitUpdate(SYUnit _sYUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUnit.Id);
			DP.Add("Name", _sYUnit.Name);
			DP.Add("UnitLevel", _sYUnit.UnitLevel);
			DP.Add("IsActived", _sYUnit.IsActived);
			DP.Add("IsDeleted", _sYUnit.IsDeleted);
			DP.Add("ParentId", _sYUnit.ParentId);
			DP.Add("Description", _sYUnit.Description);
			DP.Add("Email", _sYUnit.Email);
			DP.Add("Phone", _sYUnit.Phone);
			DP.Add("Address", _sYUnit.Address);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UnitUpdate", DP));
		}

		public async Task<int> SYUnitDelete(SYUnit _sYUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUnit.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UnitDelete", DP));
		}

		public async Task<int> SYUnitDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UnitDeleteAll", DP));
		}
	}

	public class SYUserOnPage
	{
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public int? PositionId { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYUser
	{
		private SQLCon _sQLCon;

		public SYUser(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUser()
		{
		}

		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public int? PositionId { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }

		public async Task<SYUser> SYUserGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUser>("SY_UserGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYUser>> SYUserGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUser>("SY_UserGetAll", DP)).ToList();
		}

		public async Task<List<SYUserOnPage>> SYUserGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYUserOnPage>("SY_UserGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYUserInsert(SYUser _sYUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FullName", _sYUser.FullName);
			DP.Add("UserName", _sYUser.UserName);
			DP.Add("Password", _sYUser.Password);
			DP.Add("Salt", _sYUser.Salt);
			DP.Add("IsActived", _sYUser.IsActived);
			DP.Add("IsDeleted", _sYUser.IsDeleted);
			DP.Add("Gender", _sYUser.Gender);
			DP.Add("Type", _sYUser.Type);
			DP.Add("IsSuperAdmin", _sYUser.IsSuperAdmin);
			DP.Add("Email", _sYUser.Email);
			DP.Add("Phone", _sYUser.Phone);
			DP.Add("UnitId", _sYUser.UnitId);
			DP.Add("CountLock", _sYUser.CountLock);
			DP.Add("LockEndOut", _sYUser.LockEndOut);
			DP.Add("Avatar", _sYUser.Avatar);
			DP.Add("Address", _sYUser.Address);
			DP.Add("PositionId", _sYUser.PositionId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserInsert", DP));
		}

		public async Task<int> SYUserUpdate(SYUser _sYUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUser.Id);
			DP.Add("FullName", _sYUser.FullName);
			DP.Add("UserName", _sYUser.UserName);
			DP.Add("Password", _sYUser.Password);
			DP.Add("Salt", _sYUser.Salt);
			DP.Add("IsActived", _sYUser.IsActived);
			DP.Add("IsDeleted", _sYUser.IsDeleted);
			DP.Add("Gender", _sYUser.Gender);
			DP.Add("Type", _sYUser.Type);
			DP.Add("IsSuperAdmin", _sYUser.IsSuperAdmin);
			DP.Add("Email", _sYUser.Email);
			DP.Add("Phone", _sYUser.Phone);
			DP.Add("UnitId", _sYUser.UnitId);
			DP.Add("CountLock", _sYUser.CountLock);
			DP.Add("LockEndOut", _sYUser.LockEndOut);
			DP.Add("Avatar", _sYUser.Avatar);
			DP.Add("Address", _sYUser.Address);
			DP.Add("PositionId", _sYUser.PositionId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUpdate", DP));
		}

		public async Task<int> SYUserDelete(SYUser _sYUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUser.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserDelete", DP));
		}

		public async Task<int> SYUserDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserDeleteAll", DP));
		}
	}

	public class SYUserGroupUserOnPage
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public short GroupUserId { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYUserGroupUser
	{
		private SQLCon _sQLCon;

		public SYUserGroupUser(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGroupUser()
		{
		}

		public int Id { get; set; }
		public int UserId { get; set; }
		public short GroupUserId { get; set; }

		public async Task<SYUserGroupUser> SYUserGroupUserGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGroupUser>("SY_UserGroupUserGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYUserGroupUser>> SYUserGroupUserGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGroupUser>("SY_UserGroupUserGetAll", DP)).ToList();
		}

		public async Task<List<SYUserGroupUserOnPage>> SYUserGroupUserGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYUserGroupUserOnPage>("SY_UserGroupUserGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYUserGroupUserInsert(SYUserGroupUser _sYUserGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYUserGroupUser.UserId);
			DP.Add("GroupUserId", _sYUserGroupUser.GroupUserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserGroupUserInsert", DP));
		}

		public async Task<int> SYUserGroupUserUpdate(SYUserGroupUser _sYUserGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserGroupUser.Id);
			DP.Add("UserId", _sYUserGroupUser.UserId);
			DP.Add("GroupUserId", _sYUserGroupUser.GroupUserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserGroupUserUpdate", DP));
		}

		public async Task<int> SYUserGroupUserDelete(SYUserGroupUser _sYUserGroupUser)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserGroupUser.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserGroupUserDelete", DP));
		}

		public async Task<int> SYUserGroupUserDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserGroupUserDeleteAll", DP));
		}
	}

	public class SYUserUnitOnPage
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public short UnitId { get; set; }
		public short? PositionId { get; set; }
		public bool? IsMain { get; set; }
		public int? RowNumber; // int, null
	}

	public class SYUserUnit
	{
		private SQLCon _sQLCon;

		public SYUserUnit(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserUnit()
		{
		}

		public int Id { get; set; }
		public int UserId { get; set; }
		public short UnitId { get; set; }
		public short? PositionId { get; set; }
		public bool? IsMain { get; set; }

		public async Task<SYUserUnit> SYUserUnitGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserUnit>("SY_UserUnitGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYUserUnit>> SYUserUnitGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUserUnit>("SY_UserUnitGetAll", DP)).ToList();
		}

		public async Task<List<SYUserUnitOnPage>> SYUserUnitGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYUserUnitOnPage>("SY_UserUnitGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYUserUnitInsert(SYUserUnit _sYUserUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYUserUnit.UserId);
			DP.Add("UnitId", _sYUserUnit.UnitId);
			DP.Add("PositionId", _sYUserUnit.PositionId);
			DP.Add("IsMain", _sYUserUnit.IsMain);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUnitInsert", DP));
		}

		public async Task<int> SYUserUnitUpdate(SYUserUnit _sYUserUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserUnit.Id);
			DP.Add("UserId", _sYUserUnit.UserId);
			DP.Add("UnitId", _sYUserUnit.UnitId);
			DP.Add("PositionId", _sYUserUnit.PositionId);
			DP.Add("IsMain", _sYUserUnit.IsMain);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUnitUpdate", DP));
		}

		public async Task<int> SYUserUnitDelete(SYUserUnit _sYUserUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUserUnit.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUnitDelete", DP));
		}

		public async Task<int> SYUserUnitDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UserUnitDeleteAll", DP));
		}
	}
}
