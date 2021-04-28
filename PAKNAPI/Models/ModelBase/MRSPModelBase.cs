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

	public class MRRecommendationCheckExistedCode
	{
		private SQLCon _sQLCon;

		public MRRecommendationCheckExistedCode(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationCheckExistedCode()
		{
		}

		public int? Total { get; set; }

		public async Task<List<MRRecommendationCheckExistedCode>> MRRecommendationCheckExistedCodeDAO(string Code)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationCheckExistedCode>("MR_Recommendation_CheckExistedCode", DP)).ToList();
		}
	}

	public class MRRecommendationConclusionFilesDelete
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionFilesDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionFilesDelete()
		{
		}

		public async Task<int> MRRecommendationConclusionFilesDeleteDAO(MRRecommendationConclusionFilesDeleteIN _mRRecommendationConclusionFilesDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationConclusionFilesDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_Conclusion_FilesDelete", DP));
		}
	}

	public class MRRecommendationConclusionFilesDeleteIN
	{
		public int? Id { get; set; }
	}

	public class MRRecommendationConclusionFilesGetByConclusionId
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionFilesGetByConclusionId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionFilesGetByConclusionId()
		{
		}

		public int Id { get; set; }
		public int? ConclusionId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }

		public async Task<List<MRRecommendationConclusionFilesGetByConclusionId>> MRRecommendationConclusionFilesGetByConclusionIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionFilesGetByConclusionId>("MR_Recommendation_Conclusion_FilesGetByConclusionId", DP)).ToList();
		}
	}

	public class MRRecommendationConclusionFilesInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionFilesInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionFilesInsert()
		{
		}

		public async Task<int> MRRecommendationConclusionFilesInsertDAO(MRRecommendationConclusionFilesInsertIN _mRRecommendationConclusionFilesInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ConclusionId", _mRRecommendationConclusionFilesInsertIN.ConclusionId);
			DP.Add("Name", _mRRecommendationConclusionFilesInsertIN.Name);
			DP.Add("FileType", _mRRecommendationConclusionFilesInsertIN.FileType);
			DP.Add("FilePath", _mRRecommendationConclusionFilesInsertIN.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_Conclusion_FilesInsert", DP));
		}
	}

	public class MRRecommendationConclusionFilesInsertIN
	{
		public int? ConclusionId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }
	}

	public class MRRecommendationConclusionDelete
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionDelete()
		{
		}

		public async Task<int> MRRecommendationConclusionDeleteDAO(MRRecommendationConclusionDeleteIN _mRRecommendationConclusionDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationConclusionDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ConclusionDelete", DP));
		}
	}

	public class MRRecommendationConclusionDeleteIN
	{
		public int? Id { get; set; }
	}

	public class MRRecommendationConclusionGetByRecommendationId
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionGetByRecommendationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionGetByRecommendationId()
		{
		}

		public int Id { get; set; }
		public int RecommendationId { get; set; }
		public long UserCreatedId { get; set; }
		public int? UnitCreatedId { get; set; }
		public long? ReceiverId { get; set; }
		public int? UnitReceiverId { get; set; }
		public string Content { get; set; }

		public async Task<List<MRRecommendationConclusionGetByRecommendationId>> MRRecommendationConclusionGetByRecommendationIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionGetByRecommendationId>("MR_Recommendation_ConclusionGetByRecommendationId", DP)).ToList();
		}
	}

	public class MRRecommendationConclusionInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionInsert()
		{
		}

		public async Task<decimal?> MRRecommendationConclusionInsertDAO(MRRecommendationConclusionInsertIN _mRRecommendationConclusionInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationConclusionInsertIN.RecommendationId);
			DP.Add("UserCreatedId", _mRRecommendationConclusionInsertIN.UserCreatedId);
			DP.Add("UnitCreatedId", _mRRecommendationConclusionInsertIN.UnitCreatedId);
			DP.Add("ReceiverId", _mRRecommendationConclusionInsertIN.ReceiverId);
			DP.Add("UnitReceiverId", _mRRecommendationConclusionInsertIN.UnitReceiverId);
			DP.Add("Content", _mRRecommendationConclusionInsertIN.Content);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("MR_Recommendation_ConclusionInsert", DP);
		}
	}

	public class MRRecommendationConclusionInsertIN
	{
		public int? RecommendationId { get; set; }
		public long? UserCreatedId { get; set; }
		public int? UnitCreatedId { get; set; }
		public long? ReceiverId { get; set; }
		public int? UnitReceiverId { get; set; }
		public string Content { get; set; }
	}

	public class MRRecommendationFilesDelete
	{
		private SQLCon _sQLCon;

		public MRRecommendationFilesDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationFilesDelete()
		{
		}

		public async Task<int> MRRecommendationFilesDeleteDAO(MRRecommendationFilesDeleteIN _mRRecommendationFilesDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationFilesDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FilesDelete", DP));
		}
	}

	public class MRRecommendationFilesDeleteIN
	{
		public int? Id { get; set; }
	}

	public class MRRecommendationFilesGetByRecommendationId
	{
		private SQLCon _sQLCon;

		public MRRecommendationFilesGetByRecommendationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationFilesGetByRecommendationId()
		{
		}

		public int Id { get; set; }
		public int? RecommendationId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }

		public async Task<List<MRRecommendationFilesGetByRecommendationId>> MRRecommendationFilesGetByRecommendationIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFilesGetByRecommendationId>("MR_Recommendation_FilesGetByRecommendationId", DP)).ToList();
		}
	}

	public class MRRecommendationFilesInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationFilesInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationFilesInsert()
		{
		}

		public async Task<decimal?> MRRecommendationFilesInsertDAO(MRRecommendationFilesInsertIN _mRRecommendationFilesInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationFilesInsertIN.RecommendationId);
			DP.Add("Name", _mRRecommendationFilesInsertIN.Name);
			DP.Add("FileType", _mRRecommendationFilesInsertIN.FileType);
			DP.Add("FilePath", _mRRecommendationFilesInsertIN.FilePath);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("MR_Recommendation_FilesInsert", DP);
		}
	}

	public class MRRecommendationFilesInsertIN
	{
		public int? RecommendationId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }
	}

	public class MRRecommendationForwardGetByID
	{
		private SQLCon _sQLCon;

		public MRRecommendationForwardGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationForwardGetByID()
		{
		}

		public byte Status { get; set; }
		public int Id { get; set; }
		public int RecommendationId { get; set; }
		public long? UserSendId { get; set; }
		public int? UnitSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }
		public byte? Step { get; set; }
		public string Content { get; set; }
		public string ReasonDeny { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }

		public async Task<List<MRRecommendationForwardGetByID>> MRRecommendationForwardGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationForwardGetByID>("MR_Recommendation_ForwardGetByID", DP)).ToList();
		}
	}

	public class MRRecommendationForwardInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationForwardInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationForwardInsert()
		{
		}

		public async Task<int> MRRecommendationForwardInsertDAO(MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Status", _mRRecommendationForwardInsertIN.Status);
			DP.Add("RecommendationId", _mRRecommendationForwardInsertIN.RecommendationId);
			DP.Add("UserSendId", _mRRecommendationForwardInsertIN.UserSendId);
			DP.Add("UnitSendId", _mRRecommendationForwardInsertIN.UnitSendId);
			DP.Add("ReceiveId", _mRRecommendationForwardInsertIN.ReceiveId);
			DP.Add("UnitReceiveId", _mRRecommendationForwardInsertIN.UnitReceiveId);
			DP.Add("Step", _mRRecommendationForwardInsertIN.Step);
			DP.Add("Content", _mRRecommendationForwardInsertIN.Content);
			DP.Add("ReasonDeny", _mRRecommendationForwardInsertIN.ReasonDeny);
			DP.Add("SendDate", _mRRecommendationForwardInsertIN.SendDate);
			DP.Add("ExpiredDate", _mRRecommendationForwardInsertIN.ExpiredDate);
			DP.Add("ProcessingDate", _mRRecommendationForwardInsertIN.ProcessingDate);
			DP.Add("IsViewed", _mRRecommendationForwardInsertIN.IsViewed);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardInsert", DP));
		}
	}

	public class MRRecommendationForwardInsertIN
	{
		public byte? Status { get; set; }
		public int? RecommendationId { get; set; }
		public long? UserSendId { get; set; }
		public int? UnitSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }
		public byte? Step { get; set; }
		public string Content { get; set; }
		public string ReasonDeny { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }
	}

	public class MRRecommendationForwardProcess
	{
		private SQLCon _sQLCon;

		public MRRecommendationForwardProcess(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationForwardProcess()
		{
		}

		public async Task<int> MRRecommendationForwardProcessDAO(MRRecommendationForwardProcessIN _mRRecommendationForwardProcessIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationForwardProcessIN.Id);
			DP.Add("RecommendationId", _mRRecommendationForwardProcessIN.RecommendationId);
			DP.Add("Step", _mRRecommendationForwardProcessIN.Step);
			DP.Add("Status", _mRRecommendationForwardProcessIN.Status);
			DP.Add("ReasonDeny", _mRRecommendationForwardProcessIN.ReasonDeny);
			DP.Add("ProcessingDate", _mRRecommendationForwardProcessIN.ProcessingDate);
			DP.Add("UserId", _mRRecommendationForwardProcessIN.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardProcess", DP));
		}
	}

	public class MRRecommendationForwardProcessIN
	{
		public int? Id { get; set; }
		public int? RecommendationId { get; set; }
		public byte? Step { get; set; }
		public byte? Status { get; set; }
		public string ReasonDeny { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public long? UserId { get; set; }
	}

	public class MRRecommendationForwardUpdate
	{
		private SQLCon _sQLCon;

		public MRRecommendationForwardUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationForwardUpdate()
		{
		}

		public async Task<int> MRRecommendationForwardUpdateDAO(MRRecommendationForwardUpdateIN _mRRecommendationForwardUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Status", _mRRecommendationForwardUpdateIN.Status);
			DP.Add("Id", _mRRecommendationForwardUpdateIN.Id);
			DP.Add("RecommendationId", _mRRecommendationForwardUpdateIN.RecommendationId);
			DP.Add("UserSendId", _mRRecommendationForwardUpdateIN.UserSendId);
			DP.Add("UnitSendId", _mRRecommendationForwardUpdateIN.UnitSendId);
			DP.Add("ReceiveId", _mRRecommendationForwardUpdateIN.ReceiveId);
			DP.Add("UnitReceiveId", _mRRecommendationForwardUpdateIN.UnitReceiveId);
			DP.Add("Step", _mRRecommendationForwardUpdateIN.Step);
			DP.Add("Content", _mRRecommendationForwardUpdateIN.Content);
			DP.Add("ReasonDeny", _mRRecommendationForwardUpdateIN.ReasonDeny);
			DP.Add("SendDate", _mRRecommendationForwardUpdateIN.SendDate);
			DP.Add("ExpiredDate", _mRRecommendationForwardUpdateIN.ExpiredDate);
			DP.Add("ProcessingDate", _mRRecommendationForwardUpdateIN.ProcessingDate);
			DP.Add("IsViewed", _mRRecommendationForwardUpdateIN.IsViewed);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardUpdate", DP));
		}
	}

	public class MRRecommendationForwardUpdateIN
	{
		public byte? Status { get; set; }
		public int? Id { get; set; }
		public int? RecommendationId { get; set; }
		public long? UserSendId { get; set; }
		public int? UnitSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }
		public byte? Step { get; set; }
		public string Content { get; set; }
		public string ReasonDeny { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }
	}

	public class MRRecommendationFullTextDeleteByRecommendationId
	{
		private SQLCon _sQLCon;

		public MRRecommendationFullTextDeleteByRecommendationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationFullTextDeleteByRecommendationId()
		{
		}

		public async Task<int> MRRecommendationFullTextDeleteByRecommendationIdDAO(MRRecommendationFullTextDeleteByRecommendationIdIN _mRRecommendationFullTextDeleteByRecommendationIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationFullTextDeleteByRecommendationIdIN.RecommendationId);
			DP.Add("FileId", _mRRecommendationFullTextDeleteByRecommendationIdIN.FileId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FullTextDeleteByRecommendationId", DP));
		}
	}

	public class MRRecommendationFullTextDeleteByRecommendationIdIN
	{
		public int? RecommendationId { get; set; }
		public int? FileId { get; set; }
	}

	public class MRRecommendationFullTextInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationFullTextInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationFullTextInsert()
		{
		}

		public async Task<int> MRRecommendationFullTextInsertDAO(MRRecommendationFullTextInsertIN _mRRecommendationFullTextInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationFullTextInsertIN.RecommendationId);
			DP.Add("FileId", _mRRecommendationFullTextInsertIN.FileId);
			DP.Add("FullText", _mRRecommendationFullTextInsertIN.FullText);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FullTextInsert", DP));
		}
	}

	public class MRRecommendationFullTextInsertIN
	{
		public int? RecommendationId { get; set; }
		public int? FileId { get; set; }
		public string FullText { get; set; }
	}

	public class MRRecommendationGenCodeGetCode
	{
		private SQLCon _sQLCon;

		public MRRecommendationGenCodeGetCode(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGenCodeGetCode()
		{
		}

		public async Task<string> MRRecommendationGenCodeGetCodeDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return await _sQLCon.ExecuteScalarDapperAsync<string>("MR_Recommendation_GenCode_GetCode", DP);
		}
	}

	public class MRRecommendationGenCodeUpdateNumber
	{
		private SQLCon _sQLCon;

		public MRRecommendationGenCodeUpdateNumber(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGenCodeUpdateNumber()
		{
		}

		public async Task<int> MRRecommendationGenCodeUpdateNumberDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_GenCode_UpdateNumber", DP));
		}
	}

	public class MRRecommendationGetSuggestCreate
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetSuggestCreate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetSuggestCreate()
		{
		}

		public int? Rank { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string CreatedDate { get; set; }

		public async Task<List<MRRecommendationGetSuggestCreate>> MRRecommendationGetSuggestCreateDAO(string Title)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Title", Title);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetSuggestCreate>("MR_Recommendation_GetSuggestCreate", DP)).ToList();
		}
	}

	public class MRRecommendationGetSuggestReply
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetSuggestReply(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetSuggestReply()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Name { get; set; }
		public DateTime? SendDate { get; set; }
		public string ContentReply { get; set; }
		public int? CountHashtag { get; set; }

		public async Task<List<MRRecommendationGetSuggestReply>> MRRecommendationGetSuggestReplyDAO(string ListIdHashtag, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ListIdHashtag", ListIdHashtag);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetSuggestReply>("MR_Recommendation_GetSuggestReply", DP)).ToList();
		}
	}

	public class MRRecommendationHashtagDeleteByRecommendationId
	{
		private SQLCon _sQLCon;

		public MRRecommendationHashtagDeleteByRecommendationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationHashtagDeleteByRecommendationId()
		{
		}

		public async Task<int> MRRecommendationHashtagDeleteByRecommendationIdDAO(MRRecommendationHashtagDeleteByRecommendationIdIN _mRRecommendationHashtagDeleteByRecommendationIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationHashtagDeleteByRecommendationIdIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_HashtagDeleteByRecommendationId", DP));
		}
	}

	public class MRRecommendationHashtagDeleteByRecommendationIdIN
	{
		public int? Id { get; set; }
	}

	public class MRRecommendationHashtagGetByRecommendationId
	{
		private SQLCon _sQLCon;

		public MRRecommendationHashtagGetByRecommendationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationHashtagGetByRecommendationId()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }

		public async Task<List<MRRecommendationHashtagGetByRecommendationId>> MRRecommendationHashtagGetByRecommendationIdDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtagGetByRecommendationId>("MR_Recommendation_HashtagGetByRecommendationId", DP)).ToList();
		}
	}

	public class MRRecommendationHashtagInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationHashtagInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationHashtagInsert()
		{
		}

		public async Task<int> MRRecommendationHashtagInsertDAO(MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationHashtagInsertIN.RecommendationId);
			DP.Add("HashtagId", _mRRecommendationHashtagInsertIN.HashtagId);
			DP.Add("HashtagName", _mRRecommendationHashtagInsertIN.HashtagName);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_HashtagInsert", DP));
		}
	}

	public class MRRecommendationHashtagInsertIN
	{
		public int? RecommendationId { get; set; }
		public int? HashtagId { get; set; }
		public string HashtagName { get; set; }
	}

	public class MRRecommendationDelete
	{
		private SQLCon _sQLCon;

		public MRRecommendationDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationDelete()
		{
		}

		public async Task<int> MRRecommendationDeleteDAO(MRRecommendationDeleteIN _mRRecommendationDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationDelete", DP));
		}
	}

	public class MRRecommendationDeleteIN
	{
		public int? Id { get; set; }
	}

	public class MRRecommendationGetAllOnPage
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public string FieldName { get; set; }
		public int? UnitId { get; set; }
		public string UnitName { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public async Task<List<MRRecommendationGetAllOnPage>> MRRecommendationGetAllOnPageDAO(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("SendName", SendName);
			DP.Add("Content", Content);
			DP.Add("UnitId", UnitId);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetAllOnPage>("MR_RecommendationGetAllOnPage", DP)).ToList();
		}
	}

	public class MRRecommendationGetAllWithProcess
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetAllWithProcess(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetAllWithProcess()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public string FieldName { get; set; }
		public int? UnitId { get; set; }
		public string UnitName { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? ProcessId { get; set; }
		public int? UnitSendId { get; set; }
		public long? UserSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }

		public async Task<List<MRRecommendationGetAllWithProcess>> MRRecommendationGetAllWithProcessDAO(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("SendName", SendName);
			DP.Add("Content", Content);
			DP.Add("UnitId", UnitId);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetAllWithProcess>("MR_RecommendationGetAllWithProcess", DP)).ToList();
		}
	}

	public class MRRecommendationGetByHashtagAllOnPage
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetByHashtagAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetByHashtagAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public async Task<List<MRRecommendationGetByHashtagAllOnPage>> MRRecommendationGetByHashtagAllOnPageDAO(string Code, string SendName, string Title, string Content, int? Status, int? UnitId, int? HashtagId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("SendName", SendName);
			DP.Add("Title", Title);
			DP.Add("Content", Content);
			DP.Add("Status", Status);
			DP.Add("UnitId", UnitId);
			DP.Add("HashtagId", HashtagId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByHashtagAllOnPage>("MR_RecommendationGetByHashtagAllOnPage", DP)).ToList();
		}
	}

	public class MRRecommendationGetByID
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetByID()
		{
		}

		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public int? UnitId { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public bool? ReactionaryWord { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public async Task<List<MRRecommendationGetByID>> MRRecommendationGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByID>("MR_RecommendationGetByID", DP)).ToList();
		}
	}

	public class MRRecommendationGetByIDView
	{
		private SQLCon _sQLCon;

		public MRRecommendationGetByIDView(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGetByIDView()
		{
		}

		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public int? UnitId { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public bool? ReactionaryWord { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public string UnitName { get; set; }
		public string FieldName { get; set; }
		public int? UnitActive { get; set; }
		public long? UserActive { get; set; }
		public int? IdProcess { get; set; }
		public byte? StepProcess { get; set; }
		public string ReceiveName { get; set; }
		public string ProcessUnitName { get; set; }
		public DateTime? ExpriredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public string ApprovedName { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public string ReasonDeny { get; set; }

		public async Task<List<MRRecommendationGetByIDView>> MRRecommendationGetByIDViewDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGetByIDView>("MR_RecommendationGetByIDView", DP)).ToList();
		}
	}

	public class MRRecommendationInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationInsert()
		{
		}

		public async Task<decimal?> MRRecommendationInsertDAO(MRRecommendationInsertIN _mRRecommendationInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", _mRRecommendationInsertIN.Code);
			DP.Add("Title", _mRRecommendationInsertIN.Title);
			DP.Add("Content", _mRRecommendationInsertIN.Content);
			DP.Add("Field", _mRRecommendationInsertIN.Field);
			DP.Add("UnitId", _mRRecommendationInsertIN.UnitId);
			DP.Add("TypeObject", _mRRecommendationInsertIN.TypeObject);
			DP.Add("SendId", _mRRecommendationInsertIN.SendId);
			DP.Add("Name", _mRRecommendationInsertIN.Name);
			DP.Add("Status", _mRRecommendationInsertIN.Status);
			DP.Add("SendDate", _mRRecommendationInsertIN.SendDate);
			DP.Add("ReactionaryWord", _mRRecommendationInsertIN.ReactionaryWord);
			DP.Add("CreatedBy", _mRRecommendationInsertIN.CreatedBy);
			DP.Add("CreatedDate", _mRRecommendationInsertIN.CreatedDate);
			DP.Add("UpdatedBy", _mRRecommendationInsertIN.UpdatedBy);
			DP.Add("UpdatedDate", _mRRecommendationInsertIN.UpdatedDate);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("MR_RecommendationInsert", DP);
		}
	}

	public class MRRecommendationInsertIN
	{
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public int? UnitId { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public bool? ReactionaryWord { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}

	public class MRRecommendationKNCTCheckExistedId
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTCheckExistedId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTCheckExistedId()
		{
		}

		public int? Total { get; set; }

		public async Task<List<MRRecommendationKNCTCheckExistedId>> MRRecommendationKNCTCheckExistedIdDAO(int? RecommendationKNCTId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationKNCTId", RecommendationKNCTId);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationKNCTCheckExistedId>("MR_RecommendationKNCT_CheckExistedId", DP)).ToList();
		}
	}

	public class MRRecommendationKNCTFilesDelete
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTFilesDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTFilesDelete()
		{
		}

		public async Task<int> MRRecommendationKNCTFilesDeleteDAO(MRRecommendationKNCTFilesDeleteIN _mRRecommendationKNCTFilesDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationKNCTFilesDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationKNCT_FilesDelete", DP));
		}
	}

	public class MRRecommendationKNCTFilesDeleteIN
	{
		public int? Id { get; set; }
	}

	public class MRRecommendationKNCTFilesGetByRecommendationId
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTFilesGetByRecommendationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTFilesGetByRecommendationId()
		{
		}

		public int Id { get; set; }
		public int? RecommendationKNCTId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }

		public async Task<List<MRRecommendationKNCTFilesGetByRecommendationId>> MRRecommendationKNCTFilesGetByRecommendationIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationKNCTFilesGetByRecommendationId>("MR_RecommendationKNCT_FilesGetByRecommendationId", DP)).ToList();
		}
	}

	public class MRRecommendationKNCTFilesInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTFilesInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTFilesInsert()
		{
		}

		public async Task<int> MRRecommendationKNCTFilesInsertDAO(MRRecommendationKNCTFilesInsertIN _mRRecommendationKNCTFilesInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationKNCTId", _mRRecommendationKNCTFilesInsertIN.RecommendationKNCTId);
			DP.Add("Name", _mRRecommendationKNCTFilesInsertIN.Name);
			DP.Add("FileType", _mRRecommendationKNCTFilesInsertIN.FileType);
			DP.Add("FilePath", _mRRecommendationKNCTFilesInsertIN.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationKNCT_FilesInsert", DP));
		}
	}

	public class MRRecommendationKNCTFilesInsertIN
	{
		public int? RecommendationKNCTId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }
	}

	public class MRRecommendationKNCTGetAllWithProcess
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTGetAllWithProcess(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTGetAllWithProcess()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Content { get; set; }
		public int? FieldId { get; set; }
		public string FieldName { get; set; }
		public string Department { get; set; }
		public string Progress { get; set; }
		public string Place { get; set; }
		public string Term { get; set; }
		public int? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string Classify { get; set; }
		public DateTime? EndDate { get; set; }
		public int? RecommendationKNCTId { get; set; }

		public async Task<List<MRRecommendationKNCTGetAllWithProcess>> MRRecommendationKNCTGetAllWithProcessDAO(string Code, string Content, string Unit, string Place, int? Field, int? Status, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("Content", Content);
			DP.Add("Unit", Unit);
			DP.Add("Place", Place);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationKNCTGetAllWithProcess>("MR_RecommendationKNCTGetAllWithProcess", DP)).ToList();
		}
	}

	public class MRRecommendationKNCTGetById
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTGetById()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Content { get; set; }
		public int? FieldId { get; set; }
		public string FieldName { get; set; }
		public string Department { get; set; }
		public string Progress { get; set; }
		public string Place { get; set; }
		public string Term { get; set; }
		public int? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string Classify { get; set; }
		public DateTime? EndDate { get; set; }
		public int? RecommendationKNCTId { get; set; }

		public async Task<List<MRRecommendationKNCTGetById>> MRRecommendationKNCTGetByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationKNCTGetById>("MR_RecommendationKNCTGetById", DP)).ToList();
		}
	}

	public class MRRecommendationKNCTInsert
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTInsert()
		{
		}

		public async Task<decimal?> MRRecommendationKNCTInsertDAO(MRRecommendationKNCTInsertIN _mRRecommendationKNCTInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationKNCTId", _mRRecommendationKNCTInsertIN.RecommendationKNCTId);
			DP.Add("CreatedDate", _mRRecommendationKNCTInsertIN.CreatedDate);
			DP.Add("SendDate", _mRRecommendationKNCTInsertIN.SendDate);
			DP.Add("EndDate", _mRRecommendationKNCTInsertIN.EndDate);
			DP.Add("District", _mRRecommendationKNCTInsertIN.District);
			DP.Add("Code", _mRRecommendationKNCTInsertIN.Code);
			DP.Add("Content", _mRRecommendationKNCTInsertIN.Content);
			DP.Add("Classify", _mRRecommendationKNCTInsertIN.Classify);
			DP.Add("Term", _mRRecommendationKNCTInsertIN.Term);
			DP.Add("FieldId", _mRRecommendationKNCTInsertIN.FieldId);
			DP.Add("Place", _mRRecommendationKNCTInsertIN.Place);
			DP.Add("Department", _mRRecommendationKNCTInsertIN.Department);
			DP.Add("Progress", _mRRecommendationKNCTInsertIN.Progress);
			DP.Add("Status", _mRRecommendationKNCTInsertIN.Status);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("MR_RecommendationKNCTInsert", DP);
		}
	}

	public class MRRecommendationKNCTInsertIN
	{
		public int? RecommendationKNCTId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string District { get; set; }
		public string Code { get; set; }
		public string Content { get; set; }
		public string Classify { get; set; }
		public string Term { get; set; }
		public int? FieldId { get; set; }
		public string Place { get; set; }
		public string Department { get; set; }
		public string Progress { get; set; }
		public int? Status { get; set; }
	}

	public class MRRecommendationKNCTUpdate
	{
		private SQLCon _sQLCon;

		public MRRecommendationKNCTUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationKNCTUpdate()
		{
		}

		public async Task<int> MRRecommendationKNCTUpdateDAO(MRRecommendationKNCTUpdateIN _mRRecommendationKNCTUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationKNCTId", _mRRecommendationKNCTUpdateIN.RecommendationKNCTId);
			DP.Add("CreatedDate", _mRRecommendationKNCTUpdateIN.CreatedDate);
			DP.Add("SendDate", _mRRecommendationKNCTUpdateIN.SendDate);
			DP.Add("EndDate", _mRRecommendationKNCTUpdateIN.EndDate);
			DP.Add("District", _mRRecommendationKNCTUpdateIN.District);
			DP.Add("Code", _mRRecommendationKNCTUpdateIN.Code);
			DP.Add("Content", _mRRecommendationKNCTUpdateIN.Content);
			DP.Add("Classify", _mRRecommendationKNCTUpdateIN.Classify);
			DP.Add("Term", _mRRecommendationKNCTUpdateIN.Term);
			DP.Add("FieldId", _mRRecommendationKNCTUpdateIN.FieldId);
			DP.Add("Place", _mRRecommendationKNCTUpdateIN.Place);
			DP.Add("Department", _mRRecommendationKNCTUpdateIN.Department);
			DP.Add("Progress", _mRRecommendationKNCTUpdateIN.Progress);
			DP.Add("Status", _mRRecommendationKNCTUpdateIN.Status);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationKNCTUpdate", DP));
		}
	}

	public class MRRecommendationKNCTUpdateIN
	{
		public int? RecommendationKNCTId { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string District { get; set; }
		public string Code { get; set; }
		public string Content { get; set; }
		public string Classify { get; set; }
		public string Term { get; set; }
		public int? FieldId { get; set; }
		public string Place { get; set; }
		public string Department { get; set; }
		public string Progress { get; set; }
		public int? Status { get; set; }
	}

	public class MRRecommendationUpdate
	{
		private SQLCon _sQLCon;

		public MRRecommendationUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationUpdate()
		{
		}

		public async Task<int> MRRecommendationUpdateDAO(MRRecommendationUpdateIN _mRRecommendationUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationUpdateIN.Id);
			DP.Add("Code", _mRRecommendationUpdateIN.Code);
			DP.Add("Title", _mRRecommendationUpdateIN.Title);
			DP.Add("Content", _mRRecommendationUpdateIN.Content);
			DP.Add("Field", _mRRecommendationUpdateIN.Field);
			DP.Add("UnitId", _mRRecommendationUpdateIN.UnitId);
			DP.Add("TypeObject", _mRRecommendationUpdateIN.TypeObject);
			DP.Add("SendId", _mRRecommendationUpdateIN.SendId);
			DP.Add("Name", _mRRecommendationUpdateIN.Name);
			DP.Add("Status", _mRRecommendationUpdateIN.Status);
			DP.Add("SendDate", _mRRecommendationUpdateIN.SendDate);
			DP.Add("ReactionaryWord", _mRRecommendationUpdateIN.ReactionaryWord);
			DP.Add("CreatedBy", _mRRecommendationUpdateIN.CreatedBy);
			DP.Add("CreatedDate", _mRRecommendationUpdateIN.CreatedDate);
			DP.Add("UpdatedBy", _mRRecommendationUpdateIN.UpdatedBy);
			DP.Add("UpdatedDate", _mRRecommendationUpdateIN.UpdatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationUpdate", DP));
		}
	}

	public class MRRecommendationUpdateIN
	{
		public int? Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public int? UnitId { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public bool? ReactionaryWord { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}

	public class MRRecommendationUpdateReactionaryWord
	{
		private SQLCon _sQLCon;

		public MRRecommendationUpdateReactionaryWord(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationUpdateReactionaryWord()
		{
		}

		public async Task<int> MRRecommendationUpdateReactionaryWordDAO(MRRecommendationUpdateReactionaryWordIN _mRRecommendationUpdateReactionaryWordIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationUpdateReactionaryWordIN.Id);
			DP.Add("ReactionaryWord", _mRRecommendationUpdateReactionaryWordIN.ReactionaryWord);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationUpdateReactionaryWord", DP));
		}
	}

	public class MRRecommendationUpdateReactionaryWordIN
	{
		public int? Id { get; set; }
		public bool? ReactionaryWord { get; set; }
	}

	public class MRRecommendationUpdateStatus
	{
		private SQLCon _sQLCon;

		public MRRecommendationUpdateStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationUpdateStatus()
		{
		}

		public async Task<int> MRRecommendationUpdateStatusDAO(MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationUpdateStatusIN.Id);
			DP.Add("Status", _mRRecommendationUpdateStatusIN.Status);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationUpdateStatus", DP));
		}
	}

	public class MRRecommendationUpdateStatusIN
	{
		public int? Id { get; set; }
		public byte? Status { get; set; }
	}
}
