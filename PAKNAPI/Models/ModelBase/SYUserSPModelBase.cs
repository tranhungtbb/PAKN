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
