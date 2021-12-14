using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
	public class CAHashtagListPage
	{
		private SQLCon _sQLCon;

		public CAHashtagListPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagListPage()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public int? RowNumber; // int, null
		public int? QuantityUser { get; set; }

		public async Task<List<CAHashtagListPage>> CAHashtagGetAllOnPage()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagListPage>("CA_HashtagGetAllOnPage", DP)).ToList();
		}
	}
}
