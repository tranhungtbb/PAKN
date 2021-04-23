using Dapper;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
    public class HISNewsModel
    {
		private SQLCon _sQLCon;

		public HISNewsModel(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISNewsModel()
		{
		}

		public async Task<List<HISNews>> HISNewsGetByNewsId(int ObjectId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", ObjectId);
			return (await _sQLCon.ExecuteListDapperAsync<HISNews>("[HIS_NewsGetListByNewsId]", DP)).ToList();
		}

	}
}
