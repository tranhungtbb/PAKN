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
	public class SYCaptChaDelete
	{
		private SQLCon _sQLCon;

		public SYCaptChaDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYCaptChaDelete()
		{
		}

		public async Task<int> SYCaptChaDeleteDAO(SYCaptChaDeleteIN _sYCaptChaDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", _sYCaptChaDeleteIN.Code);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_CaptChaDelete", DP));
		}
	}

	public class SYCaptChaDeleteIN
	{
		public string Code { get; set; }
	}

	public class SYCaptChaInsertData
	{
		private SQLCon _sQLCon;

		public SYCaptChaInsertData(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYCaptChaInsertData()
		{
		}

		public async Task<int> SYCaptChaInsertDataDAO(SYCaptChaInsertDataIN _sYCaptChaInsertDataIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", _sYCaptChaInsertDataIN.Code);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_CaptChaInsertData", DP));
		}
	}

	public class SYCaptChaInsertDataIN
	{
		public string Code { get; set; }
	}

	public class SYCaptChaValidator
	{
		private SQLCon _sQLCon;

		public SYCaptChaValidator(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYCaptChaValidator()
		{
		}

		public async Task<List<SYCaptChaValidator>> SYCaptChaValidatorDAO(string Code)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);

			return (await _sQLCon.ExecuteListDapperAsync<SYCaptChaValidator>("SY_CaptChaValidator", DP)).ToList();
		}
	}
}
