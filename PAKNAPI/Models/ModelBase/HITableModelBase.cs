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
	public class HISRecommendationOnPage
	{
		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? RowNumber; // int, null
	}

	public class HISRecommendation
	{
		private SQLCon _sQLCon;

		public HISRecommendation(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISRecommendation()
		{
		}

		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<HISRecommendation> HISRecommendationGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<HISRecommendation>("HIS_RecommendationGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<HISRecommendation>> HISRecommendationGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<HISRecommendation>("HIS_RecommendationGetAll", DP)).ToList();
		}

		public async Task<List<HISRecommendationOnPage>> HISRecommendationGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<HISRecommendationOnPage>("HIS_RecommendationGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> HISRecommendationInsert(HISRecommendation _hISRecommendation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISRecommendation.ObjectId);
			DP.Add("Type", _hISRecommendation.Type);
			DP.Add("Content", _hISRecommendation.Content);
			DP.Add("Status", _hISRecommendation.Status);
			DP.Add("CreatedBy", _hISRecommendation.CreatedBy);
			DP.Add("CreatedDate", _hISRecommendation.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_RecommendationInsert", DP));
		}

		public async Task<int> HISRecommendationUpdate(HISRecommendation _hISRecommendation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISRecommendation.Id);
			DP.Add("ObjectId", _hISRecommendation.ObjectId);
			DP.Add("Type", _hISRecommendation.Type);
			DP.Add("Content", _hISRecommendation.Content);
			DP.Add("Status", _hISRecommendation.Status);
			DP.Add("CreatedBy", _hISRecommendation.CreatedBy);
			DP.Add("CreatedDate", _hISRecommendation.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_RecommendationUpdate", DP));
		}

		public async Task<int> HISRecommendationDelete(HISRecommendation _hISRecommendation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISRecommendation.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_RecommendationDelete", DP));
		}

		public async Task<int> HISRecommendationDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_RecommendationDeleteAll", DP));
		}

		public async Task<int> HISRecommendationCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("HIS_RecommendationCount", DP));
		}
	}
}
