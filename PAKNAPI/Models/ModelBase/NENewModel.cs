using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
	public class NENewsGetListHomePage
	{
		private SQLCon _sQLCon;

		public NENewsGetListHomePage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsGetListHomePage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string PostType { get; set; }
		public bool IsPublished { get; set; }
		public int Status { get; set; }
		public string Summary { get; set; }
		public string ImagePath { get; set; }


		public DateTime CreatedDate { get; set; }


		public async Task<List<NENewsGetListHomePage>> PU_NewsGetListHomePage(bool? check)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Check", check);
			return (await _sQLCon.ExecuteListDapperAsync<NENewsGetListHomePage>("[PU_NewsGetList]", DP)).ToList();
		}

	}
}
