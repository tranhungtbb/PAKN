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
	public class PURecommendationStatisticsGetByUserId
	{
		private SQLCon _sQLCon;

		public PURecommendationStatisticsGetByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public PURecommendationStatisticsGetByUserId()
		{
		}

		public int? ReceiveWait { get; set; }
		public int? ReceiveApproved { get; set; }
		public int? Finised { get; set; }
		public int? Approve { get; set; }

		public async Task<List<PURecommendationStatisticsGetByUserId>> PURecommendationStatisticsGetByUserIdDAO(int? UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);

			return (await _sQLCon.ExecuteListDapperAsync<PURecommendationStatisticsGetByUserId>("PU_Recommendation_StatisticsGetByUserId", DP)).ToList();
		}
	}

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

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public int? CountClick { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime? CreatedDate { get; set; }

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
	public class PUSupportModelBase
	{
		private SQLCon _sQLCon;

		public PUSupportModelBase(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public PUSupportModelBase()
		{
		}
		public string Title {get;set;}
		public int? Category {get;set;}
		public string Content { get; set; }
		public string FilePath { get; set; }
		public int? FileType { get; set; }
		public DateTime? Date { get; set; }

		public async Task<List<PUSupportModelBase>> PUSupportModelBaseDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<PUSupportModelBase>("SY_SupportMenu_GetPublishContent", DP)).ToList();
		}
	}
}
