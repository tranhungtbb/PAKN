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
	public class PURecommendationGetAllOnPage
	{
		private SQLCon _sQLCon;

		public PURecommendationGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public PURecommendationGetAllOnPage()
		{
		}

		public int? RowNumber;
		public int Id;
		public string Name;
		public string Title;
		public string Content;
		public string Hour;
		public string Date;

		public async Task<List<PURecommendationGetAllOnPage>> PURecommendationGetAllOnPageDAO(string KeySearch, int? Status, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("KeySearch", KeySearch);
			DP.Add("Status", Status);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<PURecommendationGetAllOnPage>("PU_RecommendationGetAllOnPage", DP)).ToList();
		}
	}
}
