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
	public class BIBusinessGetDropdown
	{
		private SQLCon _sQLCon;

		public BIBusinessGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetDropdown()
		{
		}

		public long Value { get; set; }
		public string Text { get; set; }

		public async Task<List<BIBusinessGetDropdown>> BIBusinessGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetDropdown>("BI_BusinessGetDropdown", DP)).ToList();
		}
	}

	public class BIIndividualGetDropdown
	{
		private SQLCon _sQLCon;

		public BIIndividualGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIIndividualGetDropdown()
		{
		}

		public long Value { get; set; }
		public string Text { get; set; }

		public async Task<List<BIIndividualGetDropdown>> BIIndividualGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<BIIndividualGetDropdown>("BI_IndividualGetDropdown", DP)).ToList();
		}
	}
}
