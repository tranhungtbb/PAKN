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
		public string Code { get; set; }

		public async Task<List<SYRoleGetAll>> SYRoleGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetAll>("SY_RoleGetAll", DP)).ToList();
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

		public async Task<List<SYUnitGetDropdown>> SYUnitGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdown>("SY_UnitGetDropdown", DP)).ToList();
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

		public int Value { get; set; }
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
}
