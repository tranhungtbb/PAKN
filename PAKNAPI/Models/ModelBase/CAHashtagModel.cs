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

		public async Task<List<CAHashtagListPage>> CAHashtagGetAllOnPage(int PageSize, int PageIndex, string Name, int? QuantityUser, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("QuantityUser", QuantityUser);
			DP.Add("IsActived", IsActived);
			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagListPage>("CA_HashtagGetAllOnPage", DP)).ToList();
		}
	}
}
