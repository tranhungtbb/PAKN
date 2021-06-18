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


	public class StatisticRecommendationByUnitDetailGetAllOnPage
	{
		private SQLCon _sQLCon;
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string FieldName { get; set; }
		public int Status { get; set; }
		public int? RowNumber { get; set; }

		public StatisticRecommendationByUnitDetailGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByUnitDetailGetAllOnPage()
		{
		}

		public async Task<List<StatisticRecommendationByUnitDetailGetAllOnPage>> StatisticRecommendationByUnitDetailGetAllOnPageDAO(int UnitId,int UnitProcessId , long UserProcessId, string Code, string CreateName, string Title, int? Field, int? Status, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitId", UnitId);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("Code", Code);
			DP.Add("CreateName", CreateName);
			DP.Add("Title", Title);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByUnitDetailGetAllOnPage>("TK_ListRecommendationByUnit_Detail", DP)).ToList();
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

	public class StatisticRecommendationByFiledDetailGetAllOnPage
	{
		private SQLCon _sQLCon;
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string UnitName { get; set; }
		public int Status { get; set; }
		public int? RowNumber { get; set; }

		public StatisticRecommendationByFiledDetailGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByFiledDetailGetAllOnPage()
		{
		}

		public async Task<List<StatisticRecommendationByFiledDetailGetAllOnPage>> StatisticRecommendationByFieldDetailGetAllOnPageDAO(int FiledId, int UnitProcessId, long UserProcessId, string Code, string CreateName, string Title, string? LstUnitId, int? Status, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LstUnitId", LstUnitId);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("Code", Code);
			DP.Add("CreateName", CreateName);
			DP.Add("Title", Title);
			DP.Add("Field", FiledId);
			DP.Add("Status", Status);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByFiledDetailGetAllOnPage>("TK_ListRecommendationByField_Detail", DP)).ToList();
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
		public int? UnitId { get; set; }
		public double? Total { get; set; }

		public async Task<List<StatisticRecommendationByGroupWordGetAllOnPage>> StatisticRecommendationByGroupWordGetAllOnPageDAO(string LtsUnitId, DateTime? FromDate, DateTime? ToDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByGroupWordGetAllOnPage>("[TK_ListRecommendationByGroupWord]", DP)).ToList();
		}
	}

	public class StatisticRecommendationByGroupWordDetail
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByGroupWordDetail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByGroupWordDetail()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }

		public async Task<List<StatisticRecommendationByGroupWordDetail>> StatisticRecommendationByGroupWordDetailDAO(string Code, string SendName, string Title, string Content, int? UnitId, int? GroupWordId, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("SendName", SendName);
			DP.Add("Title", Title);
			DP.Add("Content", Content);
			DP.Add("UnitId", UnitId);
			DP.Add("GroupWordId", GroupWordId);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByGroupWordDetail>("TK_ListRecommendationByGroupWord_Detail", DP)).ToList();
		}
	}
}
