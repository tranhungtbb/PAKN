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
	public class CAFieldGetDropdown
	{
		private SQLCon _sQLCon;

		public CAFieldGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }

		public async Task<List<CAFieldGetDropdown>> CAFieldGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetDropdown>("CA_FieldGetDropdown", DP)).ToList();
		}
	}

	public class CAHashtagGetAll
	{
		private SQLCon _sQLCon;

		public CAHashtagGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagGetAll()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }

		public async Task<List<CAHashtagGetAll>> CAHashtagGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagGetAll>("CA_HashtagGetAll", DP)).ToList();
		}
	}

	public class CAHashtagGetDropdown
	{
		private SQLCon _sQLCon;

		public CAHashtagGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }

		public async Task<List<CAHashtagGetDropdown>> CAHashtagGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagGetDropdown>("CA_HashtagGetDropdown", DP)).ToList();
		}
	}
}
