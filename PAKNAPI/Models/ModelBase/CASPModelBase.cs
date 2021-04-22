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
	public class CAFieldKNCTInsert
	{
		private SQLCon _sQLCon;

		public CAFieldKNCTInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldKNCTInsert()
		{
		}

		public async Task<int?> CAFieldKNCTInsertDAO(CAFieldKNCTInsertIN _cAFieldKNCTInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAFieldKNCTInsertIN.Name);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_FieldKNCTInsert", DP);
		}
	}

	public class CAFieldKNCTInsertIN
	{
		public string Name { get; set; }
	}
}
