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
	public class SYLogin
	{
		private SQLCon _sQLCon;

		public SYLogin(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYLogin()
		{
		}

		public long Id; // bigint, not null
		public string FullName; // nvarchar(256), null
		public string UserName; // nvarchar(256), null
		public string Address; // nvarchar(256), null
		public string Email; // nvarchar(256), null
		public string Avatar; // nvarchar(256), null
		public bool? Gender; // bit, null
		public string Phone; // nvarchar(256), null
		public long? PositionId; // bigint, null
		public string PositionName; // nvarchar(256), null
		public long? OrganizationId; // bigint, null
		public string OrganizationName; // nvarchar(256), null

		public async Task<List<SYLogin>> SYLoginDAO(string UserName, string Password)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);
			DP.Add("Password", Password);

			return (await _sQLCon.ExecuteListDapperAsync<SYLogin>("SYLogin", DP)).ToList();
		}
	}
}
