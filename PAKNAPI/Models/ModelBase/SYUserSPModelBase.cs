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

		public async Task<List<SYUSRGetPermissionByUserId>> SYUSRGetPermissionByUserIdDAO(int? UserId)
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
		public string PositionName { get; set; }
		public int? UnitId { get; set; }
		public string UnitName { get; set; }

		public async Task<List<SYUSRLogin>> SYUSRLoginDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteListDapperAsync<SYUSRLogin>("SY_USR_Login", DP)).ToList();
		}
	}
}
