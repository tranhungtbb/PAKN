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
	public class SYAPIInsert
	{
		private SQLCon _sQLCon;

		public SYAPIInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYAPIInsert()
		{
		}

		public async Task<int> SYAPIInsertDAO(SYAPIInsertIN _sYAPIInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYAPIInsertIN.Name);
			DP.Add("Authorize", _sYAPIInsertIN.Authorize);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_APIInsert", DP));
		}
	}

	public class SYAPIInsertIN
	{
		public string Name { get; set; }
		public bool? Authorize { get; set; }
	}

	public class SYEmailGetFirst
	{
		private SQLCon _sQLCon;

		public SYEmailGetFirst(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYEmailGetFirst()
		{
		}

		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Port { get; set; }
		public string Server { get; set; }

		public async Task<List<SYEmailGetFirst>> SYEmailGetFirstDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYEmailGetFirst>("SY_EmailGetFirst", DP)).ToList();
		}
	}

	public class SYEmailInsert
	{
		private SQLCon _sQLCon;

		public SYEmailInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYEmailInsert()
		{
		}

		public async Task<int?> SYEmailInsertDAO(SYEmailInsertIN _sYEmailInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Email", _sYEmailInsertIN.Email);
			DP.Add("Password", _sYEmailInsertIN.Password);
			DP.Add("Server", _sYEmailInsertIN.Server);
			DP.Add("Port", _sYEmailInsertIN.Port);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_EmailInsert", DP);
		}
	}

	public class SYEmailInsertIN
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Server { get; set; }
		public string Port { get; set; }
	}

	public class SYPermissionGetByFunction
	{
		private SQLCon _sQLCon;

		public SYPermissionGetByFunction(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionGetByFunction()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short FunctionId { get; set; }
		public short? ParentId { get; set; }

		public async Task<List<SYPermissionGetByFunction>> SYPermissionGetByFunctionDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionGetByFunction>("SY_Permission_GetByFunction", DP)).ToList();
		}
	}

	public class SYPermissionCategoryGet
	{
		private SQLCon _sQLCon;

		public SYPermissionCategoryGet(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionCategoryGet()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		public async Task<List<SYPermissionCategoryGet>> SYPermissionCategoryGetDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCategoryGet>("SY_PermissionCategory_Get", DP)).ToList();
		}
	}

	public class SYPermissionCheckByUserId
	{
		private SQLCon _sQLCon;

		public SYPermissionCheckByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionCheckByUserId()
		{
		}

		public int Permission { get; set; }

		public async Task<List<SYPermissionCheckByUserId>> SYPermissionCheckByUserIdDAO(int? UserId, string APIName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			DP.Add("APIName", APIName);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCheckByUserId>("SY_PermissionCheckByUserId", DP)).ToList();
		}
	}

	public class SYPermissionFunctionGetByCategory
	{
		private SQLCon _sQLCon;

		public SYPermissionFunctionGetByCategory(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionFunctionGetByCategory()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short CategoryId { get; set; }

		public async Task<List<SYPermissionFunctionGetByCategory>> SYPermissionFunctionGetByCategoryDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionFunctionGetByCategory>("SY_PermissionFunction_GetByCategory", DP)).ToList();
		}
	}

	public class SYPermissionGroupUserGetByGroupId
	{
		private SQLCon _sQLCon;

		public SYPermissionGroupUserGetByGroupId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionGroupUserGetByGroupId()
		{
		}

		public short PermissionId { get; set; }

		public async Task<List<SYPermissionGroupUserGetByGroupId>> SYPermissionGroupUserGetByGroupIdDAO(int? GroupUserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("GroupUserId", GroupUserId);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionGroupUserGetByGroupId>("SY_PermissionGroupUser_GetByGroupId", DP)).ToList();
		}
	}

	public class SYPermissionGroupUserInsertByList
	{
		private SQLCon _sQLCon;

		public SYPermissionGroupUserInsertByList(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionGroupUserInsertByList()
		{
		}

		public async Task<int> SYPermissionGroupUserInsertByListDAO(SYPermissionGroupUserInsertByListIN _sYPermissionGroupUserInsertByListIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("lstid", _sYPermissionGroupUserInsertByListIN.lstid);
			DP.Add("GroupUserId", _sYPermissionGroupUserInsertByListIN.GroupUserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionGroupUser_InsertByList", DP));
		}
	}

	public class SYPermissionGroupUserInsertByListIN
	{
		public string lstid { get; set; }
		public int? GroupUserId { get; set; }
	}

	public class SYRoleDelete
	{
		private SQLCon _sQLCon;

		public SYRoleDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleDelete()
		{
		}

		public async Task<int> SYRoleDeleteDAO(SYRoleDeleteIN _sYRoleDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYRoleDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_RoleDelete", DP));
		}
	}

	public class SYRoleDeleteIN
	{
		public int? Id { get; set; }
	}

	public class SYRoleGetAll
	{
		private SQLCon _sQLCon;

		public SYRoleGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleGetAll()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }

		public async Task<List<SYRoleGetAll>> SYRoleGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetAll>("SY_RoleGetAll", DP)).ToList();
		}
	}

	public class SYRoleGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYRoleGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public int? UserCount { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<SYRoleGetAllOnPage>> SYRoleGetAllOnPageDAO(int? PageSize, int? PageIndex, int? UserCount, string Name, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("UserCount", UserCount);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetAllOnPage>("SY_RoleGetAllOnPage", DP)).ToList();
		}
	}

	public class SYRoleGetByID
	{
		private SQLCon _sQLCon;

		public SYRoleGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleGetByID()
		{
		}

		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<SYRoleGetByID>> SYRoleGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetByID>("SY_RoleGetByID", DP)).ToList();
		}
	}

	public class SYRoleInsert
	{
		private SQLCon _sQLCon;

		public SYRoleInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleInsert()
		{
		}

		public async Task<int?> SYRoleInsertDAO(SYRoleInsertIN _sYRoleInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("OrderNumber", _sYRoleInsertIN.OrderNumber);
			DP.Add("Name", _sYRoleInsertIN.Name);
			DP.Add("IsActived", _sYRoleInsertIN.IsActived);
			DP.Add("IsDeleted", _sYRoleInsertIN.IsDeleted);
			DP.Add("Description", _sYRoleInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_RoleInsert", DP);
		}
	}

	public class SYRoleInsertIN
	{
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class SYRoleUpdate
	{
		private SQLCon _sQLCon;

		public SYRoleUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleUpdate()
		{
		}

		public async Task<int?> SYRoleUpdateDAO(SYRoleUpdateIN _sYRoleUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYRoleUpdateIN.Id);
			DP.Add("OrderNumber", _sYRoleUpdateIN.OrderNumber);
			DP.Add("Name", _sYRoleUpdateIN.Name);
			DP.Add("IsActived", _sYRoleUpdateIN.IsActived);
			DP.Add("IsDeleted", _sYRoleUpdateIN.IsDeleted);
			DP.Add("Description", _sYRoleUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_RoleUpdate", DP);
		}
	}

	public class SYRoleUpdateIN
	{
		public int? Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class SYSMSGetFirst
	{
		private SQLCon _sQLCon;

		public SYSMSGetFirst(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSMSGetFirst()
		{
		}

		public int Id { get; set; }
		public string Linkwebservice { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Code { get; set; }
		public string ServiceID { get; set; }
		public string CommandCode { get; set; }
		public bool? ContenType { get; set; }

		public async Task<List<SYSMSGetFirst>> SYSMSGetFirstDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYSMSGetFirst>("SY_SMSGetFirst", DP)).ToList();
		}
	}

	public class SYSMSInsert
	{
		private SQLCon _sQLCon;

		public SYSMSInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSMSInsert()
		{
		}

		public async Task<int?> SYSMSInsertDAO(SYSMSInsertIN _sYSMSInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Linkwebservice", _sYSMSInsertIN.Linkwebservice);
			DP.Add("User", _sYSMSInsertIN.User);
			DP.Add("Password", _sYSMSInsertIN.Password);
			DP.Add("Code", _sYSMSInsertIN.Code);
			DP.Add("ServiceID", _sYSMSInsertIN.ServiceID);
			DP.Add("CommandCode", _sYSMSInsertIN.CommandCode);
			DP.Add("ContenType", _sYSMSInsertIN.ContenType);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_SMSInsert", DP);
		}
	}

	public class SYSMSInsertIN
	{
		public string Linkwebservice { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Code { get; set; }
		public string ServiceID { get; set; }
		public string CommandCode { get; set; }
		public bool? ContenType { get; set; }
	}

	public class SYSystemLogDelete
	{
		private SQLCon _sQLCon;

		public SYSystemLogDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLogDelete()
		{
		}

		public async Task<int> SYSystemLogDeleteDAO(SYSystemLogDeleteIN _sYSystemLogDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYSystemLogDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_SystemLogDelete", DP));
		}
	}

	public class SYSystemLogDeleteIN
	{
		public int? Id { get; set; }
	}

	public class SYSystemLogGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYSystemLogGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLogGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
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

		public async Task<List<SYSystemLogGetAllOnPage>> SYSystemLogGetAllOnPageDAO(int? UserId, int? PageSize, int? PageIndex, DateTime? FromDate, DateTime? ToDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);

			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLogGetAllOnPage>("SY_SystemLogGetAllOnPage", DP)).ToList();
		}
	}

	public class SYSystemLogGetAllOnPageAdmin
	{
		private SQLCon _sQLCon;

		public SYSystemLogGetAllOnPageAdmin(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLogGetAllOnPageAdmin()
		{
		}

		public int? RowNumber { get; set; }
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

		public async Task<List<SYSystemLogGetAllOnPageAdmin>> SYSystemLogGetAllOnPageAdminDAO(int? UserId, int? PageSize, int? PageIndex, DateTime? CreateDate, byte? Status, string Description)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("CreateDate", CreateDate);
			DP.Add("Status", Status);
			DP.Add("Description", Description);

			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLogGetAllOnPageAdmin>("SY_SystemLogGetAllOnPageAdmin", DP)).ToList();
		}
	}

	public class SYUnitGetDropdown
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }
		public int? ParentId { get; set; }
		public byte UnitLevel { get; set; }

		public async Task<List<SYUnitGetDropdown>> SYUnitGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdown>("SY_UnitGetDropdown", DP)).ToList();
		}
	}

	public class SYUnitGetDropdownLevel
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdownLevel(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdownLevel()
		{
		}

		public int? Value { get; set; }
		public string Text { get; set; }

		public async Task<List<SYUnitGetDropdownLevel>> SYUnitGetDropdownLevelDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdownLevel>("SY_UnitGetDropdownLevel", DP)).ToList();
		}
	}

	public class SYUnitGetDropdownNotMain
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdownNotMain(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdownNotMain()
		{
		}

		public int? Value { get; set; }
		public string Text { get; set; }

		public async Task<List<SYUnitGetDropdownNotMain>> SYUnitGetDropdownNotMainDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdownNotMain>("SY_UnitGetDropdownNotMain", DP)).ToList();
		}
	}

	public class SYUnitGetMainId
	{
		private SQLCon _sQLCon;

		public SYUnitGetMainId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetMainId()
		{
		}

		public int Id { get; set; }

		public async Task<List<SYUnitGetMainId>> SYUnitGetMainIdDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetMainId>("SY_UnitGetMainId", DP)).ToList();
		}
	}

	public class SYUnitGetNameById
	{
		private SQLCon _sQLCon;

		public SYUnitGetNameById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetNameById()
		{
		}

		public string Name { get; set; }

		public async Task<List<SYUnitGetNameById>> SYUnitGetNameByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetNameById>("SY_UnitGetNameById", DP)).ToList();
		}
	}

	public class SYUserGetByUnitId
	{
		private SQLCon _sQLCon;

		public SYUserGetByUnitId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetByUnitId()
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
		public int TypeId { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }

		public async Task<List<SYUserGetByUnitId>> SYUserGetByUnitIdDAO(int? UnitId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitId", UnitId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByUnitId>("SY_UserGetByUnitId", DP)).ToList();
		}
	}

	public class SYUserGetByUserName
	{
		private SQLCon _sQLCon;

		public SYUserGetByUserName(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetByUserName()
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

		public async Task<List<SYUserGetByUserName>> SYUserGetByUserNameDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByUserName>("SY_UserGetByUserName", DP)).ToList();
		}
	}

	public class SYUserGetNonSystem
	{
		private SQLCon _sQLCon;

		public SYUserGetNonSystem(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetNonSystem()
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

		public async Task<List<SYUserGetNonSystem>> SYUserGetNonSystemDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetNonSystem>("SY_UserGetNonSystem", DP)).ToList();
		}
	}

	public class SYNotificationDelete
	{
		private SQLCon _sQLCon;

		public SYNotificationDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYNotificationDelete()
		{
		}

		public async Task<int> SYNotificationDeleteDAO(SYNotificationDeleteIN _sYNotificationDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYNotificationDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SYNotificationDelete", DP));
		}
	}

	public class SYNotificationDeleteIN
	{
		public int? Id { get; set; }
	}
}
