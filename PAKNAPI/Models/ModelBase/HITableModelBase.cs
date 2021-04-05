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
	public class HISIndividualOnPage
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

	public class HISIndividual
	{
		private SQLCon _sQLCon;

		public HISIndividual(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISIndividual()
		{
		}

		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<HISIndividual> HISIndividualGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<HISIndividual>("HIS_IndividualGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<HISIndividual>> HISIndividualGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<HISIndividual>("HIS_IndividualGetAll", DP)).ToList();
		}

		public async Task<List<HISIndividualOnPage>> HISIndividualGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<HISIndividualOnPage>("HIS_IndividualGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> HISIndividualInsert(HISIndividual _hISIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISIndividual.ObjectId);
			DP.Add("Type", _hISIndividual.Type);
			DP.Add("Content", _hISIndividual.Content);
			DP.Add("Status", _hISIndividual.Status);
			DP.Add("CreatedBy", _hISIndividual.CreatedBy);
			DP.Add("CreatedDate", _hISIndividual.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_IndividualInsert", DP));
		}

		public async Task<int> HISIndividualUpdate(HISIndividual _hISIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISIndividual.Id);
			DP.Add("ObjectId", _hISIndividual.ObjectId);
			DP.Add("Type", _hISIndividual.Type);
			DP.Add("Content", _hISIndividual.Content);
			DP.Add("Status", _hISIndividual.Status);
			DP.Add("CreatedBy", _hISIndividual.CreatedBy);
			DP.Add("CreatedDate", _hISIndividual.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_IndividualUpdate", DP));
		}

		public async Task<int> HISIndividualDelete(HISIndividual _hISIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISIndividual.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_IndividualDelete", DP));
		}

		public async Task<int> HISIndividualDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_IndividualDeleteAll", DP));
		}

		public async Task<int> HISIndividualCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("HIS_IndividualCount", DP));
		}
	}

	public class HISInvitationOnPage
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

	public class HISInvitation
	{
		private SQLCon _sQLCon;

		public HISInvitation(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISInvitation()
		{
		}

		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<HISInvitation> HISInvitationGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<HISInvitation>("HIS_InvitationGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<HISInvitation>> HISInvitationGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<HISInvitation>("HIS_InvitationGetAll", DP)).ToList();
		}

		public async Task<List<HISInvitationOnPage>> HISInvitationGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<HISInvitationOnPage>("HIS_InvitationGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> HISInvitationInsert(HISInvitation _hISInvitation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISInvitation.ObjectId);
			DP.Add("Type", _hISInvitation.Type);
			DP.Add("Content", _hISInvitation.Content);
			DP.Add("Status", _hISInvitation.Status);
			DP.Add("CreatedBy", _hISInvitation.CreatedBy);
			DP.Add("CreatedDate", _hISInvitation.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_InvitationInsert", DP));
		}

		public async Task<int> HISInvitationUpdate(HISInvitation _hISInvitation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISInvitation.Id);
			DP.Add("ObjectId", _hISInvitation.ObjectId);
			DP.Add("Type", _hISInvitation.Type);
			DP.Add("Content", _hISInvitation.Content);
			DP.Add("Status", _hISInvitation.Status);
			DP.Add("CreatedBy", _hISInvitation.CreatedBy);
			DP.Add("CreatedDate", _hISInvitation.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_InvitationUpdate", DP));
		}

		public async Task<int> HISInvitationDelete(HISInvitation _hISInvitation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISInvitation.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_InvitationDelete", DP));
		}

		public async Task<int> HISInvitationDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_InvitationDeleteAll", DP));
		}

		public async Task<int> HISInvitationCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("HIS_InvitationCount", DP));
		}
	}

	public class HISNewsOnPage
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

	public class HISNews
	{
		private SQLCon _sQLCon;

		public HISNews(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISNews()
		{
		}

		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<HISNews> HISNewsGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<HISNews>("HIS_NewsGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<HISNews>> HISNewsGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<HISNews>("HIS_NewsGetAll", DP)).ToList();
		}

		public async Task<List<HISNewsOnPage>> HISNewsGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<HISNewsOnPage>("HIS_NewsGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> HISNewsInsert(HISNews _hISNews)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISNews.ObjectId);
			DP.Add("Type", _hISNews.Type);
			DP.Add("Content", _hISNews.Content);
			DP.Add("Status", _hISNews.Status);
			DP.Add("CreatedBy", _hISNews.CreatedBy);
			DP.Add("CreatedDate", _hISNews.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_NewsInsert", DP));
		}

		public async Task<int> HISNewsUpdate(HISNews _hISNews)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISNews.Id);
			DP.Add("ObjectId", _hISNews.ObjectId);
			DP.Add("Type", _hISNews.Type);
			DP.Add("Content", _hISNews.Content);
			DP.Add("Status", _hISNews.Status);
			DP.Add("CreatedBy", _hISNews.CreatedBy);
			DP.Add("CreatedDate", _hISNews.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_NewsUpdate", DP));
		}

		public async Task<int> HISNewsDelete(HISNews _hISNews)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISNews.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_NewsDelete", DP));
		}

		public async Task<int> HISNewsDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_NewsDeleteAll", DP));
		}

		public async Task<int> HISNewsCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("HIS_NewsCount", DP));
		}
	}

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

	public class HISSMSOnPage
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

	public class HISSMS
	{
		private SQLCon _sQLCon;

		public HISSMS(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISSMS()
		{
		}

		public int Id { get; set; }
		public int ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public byte? Status { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<HISSMS> HISSMSGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<HISSMS>("HIS_SMSGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<HISSMS>> HISSMSGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<HISSMS>("HIS_SMSGetAll", DP)).ToList();
		}

		public async Task<List<HISSMSOnPage>> HISSMSGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<HISSMSOnPage>("HIS_SMSGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> HISSMSInsert(HISSMS _hISSMS)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISSMS.ObjectId);
			DP.Add("Type", _hISSMS.Type);
			DP.Add("Content", _hISSMS.Content);
			DP.Add("Status", _hISSMS.Status);
			DP.Add("CreatedBy", _hISSMS.CreatedBy);
			DP.Add("CreatedDate", _hISSMS.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_SMSInsert", DP));
		}

		public async Task<int> HISSMSUpdate(HISSMS _hISSMS)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISSMS.Id);
			DP.Add("ObjectId", _hISSMS.ObjectId);
			DP.Add("Type", _hISSMS.Type);
			DP.Add("Content", _hISSMS.Content);
			DP.Add("Status", _hISSMS.Status);
			DP.Add("CreatedBy", _hISSMS.CreatedBy);
			DP.Add("CreatedDate", _hISSMS.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_SMSUpdate", DP));
		}

		public async Task<int> HISSMSDelete(HISSMS _hISSMS)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _hISSMS.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_SMSDelete", DP));
		}

		public async Task<int> HISSMSDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_SMSDeleteAll", DP));
		}

		public async Task<int> HISSMSCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("HIS_SMSCount", DP));
		}
	}
}
