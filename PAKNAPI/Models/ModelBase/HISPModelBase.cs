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
	public class HISRecommendationGetByObjectId
	{
		private SQLCon _sQLCon;

		public HISRecommendationGetByObjectId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISRecommendationGetByObjectId()
		{
		}

		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public string CreatedByName { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<List<HISRecommendationGetByObjectId>> HISRecommendationGetByObjectIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<HISRecommendationGetByObjectId>("HIS_RecommendationGetByObjectId", DP)).ToList();
		}
	}

	public class HISRecommendationInsert
	{
		private SQLCon _sQLCon;

		public HISRecommendationInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISRecommendationInsert()
		{
		}

		public async Task<int> HISRecommendationInsertDAO(HISRecommendationInsertIN _hISRecommendationInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISRecommendationInsertIN.ObjectId);
			DP.Add("Type", _hISRecommendationInsertIN.Type);
			DP.Add("Content", _hISRecommendationInsertIN.Content);
			DP.Add("Status", _hISRecommendationInsertIN.Status);
			DP.Add("CreatedBy", _hISRecommendationInsertIN.CreatedBy);
			DP.Add("CreatedDate", _hISRecommendationInsertIN.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_RecommendationInsert", DP));
		}
	}

	public class HISRecommendationInsertIN
	{
		public int? ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
