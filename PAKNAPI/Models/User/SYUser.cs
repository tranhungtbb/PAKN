using Dapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PAKNAPI.Common;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.User
{
	public class SYUserGetAllOnPageList
	{
		private SQLCon _sQLCon;

		public SYUserGetAllOnPageList(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetAllOnPageList()
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

		public string UnitName { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }

		public string PositionName { get; set; }

		public async Task<List<SYUserGetAllOnPageList>> SYUserGetAllOnPageDAO(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActived, int? UnitId, int? TypeId, int? PositionId)
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
			DP.Add("PositionId", PositionId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetAllOnPageList>("[SY_UserGetAllOnPageList]", DP)).ToList();
		}
		public async Task<UsersGetDataForCreateResponse> UsersGetDataForCreate()
		{
			UsersGetDataForCreateResponse data = new UsersGetDataForCreateResponse();
			DynamicParameters DP = new DynamicParameters();
			data.lstUnit = (await _sQLCon.ExecuteListDapperAsync<DropdownTree>("SY_UnitGetDropdownLevel", DP)).ToList();
			data.lstPossition = (await _sQLCon.ExecuteListDapperAsync<DropdownObject>("CA_PositionGetDropdown", DP)).ToList();
			data.lstRoles = (await _sQLCon.ExecuteListDapperAsync<DropdownPermissionObject>("SY_RoleGetDropdown", DP)).ToList();
            foreach (var role in data.lstRoles)
			{
				DP = new DynamicParameters();
				DP.Add("GroupUserId", role.Value);
				role.permissionIds = (await _sQLCon.ExecuteListDapperAsync<int>("SY_PermissionGroupUser_GetByGroupId", DP)).ToList();
			}
			DP = new DynamicParameters();
			data.lstPermissionCategories = (await _sQLCon.ExecuteListDapperAsync<PermissionCategoryObject>("SY_PermissionCategory_Get", DP)).ToList();
			foreach (var cat in data.lstPermissionCategories)
			{
				DP = new DynamicParameters();
				DP.Add("Id", cat.Id);
				cat.Function = (await _sQLCon.ExecuteListDapperAsync<FunctionObject>("SY_PermissionFunction_GetByCategory", DP)).ToList();
				foreach (var per in cat.Function)
				{
					DP = new DynamicParameters();
					DP.Add("Id", per.Id);
					per.Permission = (await _sQLCon.ExecuteListDapperAsync<PermissionObject>("SY_Permission_GetByFunction", DP)).ToList();
				}
			}
			return data;
		}
	}

	public class DropListTreeView {
		public string text { get; set; }
		public long value { get; set; }
		public List<DropListTreeView> children { get; set; }

		public DropListTreeView() { }
		public DropListTreeView(string text, long value) {
			this.text = text;
			this.value = value;
			this.children = null;
		}

		public DropListTreeView(string text, long value, IList<DropListTreeView> chil) {
			this.text = text;
			this.value = value;
			this.children = (List<DropListTreeView>)chil;
		}
	}
	public class UsersGetDataForCreateResponse
	{
		public List<DropdownTree> lstUnit { get; set; }
		public List<DropdownObject> lstPossition { get; set; }
		public List<DropdownPermissionObject> lstRoles { get; set; }
		public List<PermissionCategoryObject> lstPermissionCategories { get; set; }
	}
	public class DropdownPermissionObject
	{
		public int Value { get; set; }
		public string Text { get; set; }
		public List<int> permissionIds { get; set; }
	}


}
