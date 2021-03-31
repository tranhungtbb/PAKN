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

		public int Id;
		public string Name;
		public string Code;

		public async Task<List<SYRoleGetAll>> SYRoleGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetAll>("SY_RoleGetAll", DP)).ToList();
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

		public int Id;

		public async Task<List<SYUnitGetMainId>> SYUnitGetMainIdDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetMainId>("SY_UnitGetMainId", DP)).ToList();
		}
	}
}
