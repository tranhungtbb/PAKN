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

		public int Id;
		public int ObjectId;
		public int? Type;
		public string Content;
		public byte? Status;
		public long? CreatedBy;
		public string CreatedByName;
		public DateTime? CreatedDate;

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

		public int? Total;

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

		public int Id;
		public int? ConclusionId;
		public string Name;
		public short? FileType;
		public string FilePath;

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

		public int Id;
		public int RecommendationId;
		public long UserCreatedId;
		public int? UnitCreatedId;
		public long? ReceiverId;
		public int? UnitReceiverId;
		public string Content;

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

		public int Id;
		public int? RecommendationId;
		public string Name;
		public short? FileType;
		public string FilePath;

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

		public async Task<int> MRRecommendationFilesInsertDAO(MRRecommendationFilesInsertIN _mRRecommendationFilesInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationFilesInsertIN.RecommendationId);
			DP.Add("Name", _mRRecommendationFilesInsertIN.Name);
			DP.Add("FileType", _mRRecommendationFilesInsertIN.FileType);
			DP.Add("FilePath", _mRRecommendationFilesInsertIN.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FilesInsert", DP));
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

		public byte Status;
		public int Id;
		public int RecommendationId;
		public long? UserSendId;
		public int? UnitSendId;
		public long? ReceiveId;
		public int? UnitReceiveId;
		public byte? Step;
		public string Content;
		public string ReasonDeny;
		public DateTime? SendDate;
		public DateTime? ExpiredDate;
		public DateTime? ProcessingDate;
		public bool? IsViewed;

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

		public int Value;
		public string Text;

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

		public int? RowNumber;
		public int Id;
		public string Code;
		public string Title;
		public string Content;
		public int? Field;
		public string FieldName;
		public int? UnitId;
		public string UnitName;
		public short? TypeObject;
		public long? SendId;
		public string Name;
		public byte? Status;
		public DateTime? SendDate;
		public long? CreatedBy;
		public DateTime? CreatedDate;
		public long? UpdatedBy;
		public DateTime? UpdatedDate;

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

		public int? RowNumber;
		public int Id;
		public string Code;
		public string Title;
		public string Content;
		public int? Field;
		public string FieldName;
		public int? UnitId;
		public string UnitName;
		public short? TypeObject;
		public long? SendId;
		public string Name;
		public byte? Status;
		public DateTime? SendDate;
		public long? CreatedBy;
		public DateTime? CreatedDate;
		public long? UpdatedBy;
		public DateTime? UpdatedDate;
		public int ProcessId;

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

		public int Id;
		public string Code;
		public string Title;
		public string Content;
		public int? Field;
		public int? UnitId;
		public short? TypeObject;
		public long? SendId;
		public string Name;
		public byte? Status;
		public DateTime? SendDate;
		public bool? ReactionaryWord;
		public long? CreatedBy;
		public DateTime? CreatedDate;
		public long? UpdatedBy;
		public DateTime? UpdatedDate;

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

		public int Id;
		public string Code;
		public string Title;
		public string Content;
		public int? Field;
		public int? UnitId;
		public short? TypeObject;
		public long? SendId;
		public string Name;
		public byte? Status;
		public DateTime? SendDate;
		public bool? ReactionaryWord;
		public long? CreatedBy;
		public DateTime? CreatedDate;
		public long? UpdatedBy;
		public DateTime? UpdatedDate;
		public string UnitName;
		public string FieldName;
		public int? UnitActive;
		public long? UserActive;

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
