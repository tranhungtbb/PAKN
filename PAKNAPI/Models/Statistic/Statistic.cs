using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Statistic
{
	public class StatisticRecommendationByUnitGetAllOnPage
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByUnitGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByUnitGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int UnitId { get; set; }
		public string UnitName { get; set; }

		public int Total { get; set; }
		public int ReceiveWait { get; set; }
		public int ReceiveApproved { get; set; }
		public int ReceiveDeny { get; set; }
		public int ProcessWait { get; set; }

		public int Processing { get; set; }

		public int Finised { get; set; }

		public async Task<List<StatisticRecommendationByUnitGetAllOnPage>> StatisticRecommendationByUnitGetAllOnPageDAO(string LtsUnitId, int? Year, int? Timeline, DateTime? FromDate,DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("Year", Year);
			DP.Add("Timeline", Timeline);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByUnitGetAllOnPage>("[TK_ListRecommendationByUnit]", DP)).ToList();
		}
	}

	public class StatisticRecommendationByFieldGetAllOnPage
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByFieldGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByFieldGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int FieldId { get; set; }
		public string FieldName { get; set; }

		public int Total { get; set; }
		public int ReceiveWait { get; set; }
		public int ReceiveApproved { get; set; }
		public int ReceiveDeny { get; set; }
		public int ProcessWait { get; set; }

		public int Processing { get; set; }

		public int Finised { get; set; }

		public async Task<List<StatisticRecommendationByFieldGetAllOnPage>> StatisticRecommendationByFieldGetAllOnPageDAO(string LtsUnitId, int? Year, int? Timeline, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("Year", Year);
			DP.Add("Timeline", Timeline);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByFieldGetAllOnPage>("[TK_ListRecommendationByField]", DP)).ToList();
		}
	}

	public class StatisticRecommendationByGroupWordGetAllOnPage
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByGroupWordGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByGroupWordGetAllOnPage()
		{
		}
		public int GroupWordId { get; set; }
		public string GroupWordName { get; set; }

		public double? Total { get; set; }

		public async Task<List<StatisticRecommendationByGroupWordGetAllOnPage>> StatisticRecommendationByGroupWordGetAllOnPageDAO(string LtsUnitId, int? Year, int? Timeline, DateTime? FromDate, DateTime? ToDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("Year", Year);
			DP.Add("Timeline", Timeline);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByGroupWordGetAllOnPage>("[TK_ListRecommendationByGroupWord]", DP)).ToList();
		}
	}
}
