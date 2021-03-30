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
	}

	public class MRRecommendationOnPage
	{
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
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendation
	{
		private SQLCon _sQLCon;

		public MRRecommendation(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendation()
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
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public async Task<MRRecommendation> MRRecommendationGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendation>("MR_RecommendationGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendation>> MRRecommendationGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendation>("MR_RecommendationGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationOnPage>> MRRecommendationGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationOnPage>("MR_RecommendationGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationInsert(MRRecommendation _mRRecommendation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", _mRRecommendation.Code);
			DP.Add("Title", _mRRecommendation.Title);
			DP.Add("Content", _mRRecommendation.Content);
			DP.Add("Field", _mRRecommendation.Field);
			DP.Add("UnitId", _mRRecommendation.UnitId);
			DP.Add("TypeObject", _mRRecommendation.TypeObject);
			DP.Add("SendId", _mRRecommendation.SendId);
			DP.Add("Name", _mRRecommendation.Name);
			DP.Add("Status", _mRRecommendation.Status);
			DP.Add("SendDate", _mRRecommendation.SendDate);
			DP.Add("CreatedBy", _mRRecommendation.CreatedBy);
			DP.Add("CreatedDate", _mRRecommendation.CreatedDate);
			DP.Add("UpdatedBy", _mRRecommendation.UpdatedBy);
			DP.Add("UpdatedDate", _mRRecommendation.UpdatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationInsert", DP));
		}

		public async Task<int> MRRecommendationUpdate(MRRecommendation _mRRecommendation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendation.Id);
			DP.Add("Code", _mRRecommendation.Code);
			DP.Add("Title", _mRRecommendation.Title);
			DP.Add("Content", _mRRecommendation.Content);
			DP.Add("Field", _mRRecommendation.Field);
			DP.Add("UnitId", _mRRecommendation.UnitId);
			DP.Add("TypeObject", _mRRecommendation.TypeObject);
			DP.Add("SendId", _mRRecommendation.SendId);
			DP.Add("Name", _mRRecommendation.Name);
			DP.Add("Status", _mRRecommendation.Status);
			DP.Add("SendDate", _mRRecommendation.SendDate);
			DP.Add("CreatedBy", _mRRecommendation.CreatedBy);
			DP.Add("CreatedDate", _mRRecommendation.CreatedDate);
			DP.Add("UpdatedBy", _mRRecommendation.UpdatedBy);
			DP.Add("UpdatedDate", _mRRecommendation.UpdatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationUpdate", DP));
		}

		public async Task<int> MRRecommendationDelete(MRRecommendation _mRRecommendation)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendation.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationDelete", DP));
		}

		public async Task<int> MRRecommendationDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_RecommendationDeleteAll", DP));
		}
	}

	public class MRRecommendationConclusionOnPage
	{
		public int Id { get; set; }
		public int RecommendationId { get; set; }
		public long UserCreatedId { get; set; }
		public int? UnitCreatedId { get; set; }
		public long? ReceiverId { get; set; }
		public int? UnitReceiverId { get; set; }
		public byte? Status { get; set; }
		public string Content { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendationConclusion
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusion(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusion()
		{
		}

		public int Id { get; set; }
		public int RecommendationId { get; set; }
		public long UserCreatedId { get; set; }
		public int? UnitCreatedId { get; set; }
		public long? ReceiverId { get; set; }
		public int? UnitReceiverId { get; set; }
		public byte? Status { get; set; }
		public string Content { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }

		public async Task<MRRecommendationConclusion> MRRecommendationConclusionGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusion>("MR_Recommendation_ConclusionGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendationConclusion>> MRRecommendationConclusionGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusion>("MR_Recommendation_ConclusionGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationConclusionOnPage>> MRRecommendationConclusionGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionOnPage>("MR_Recommendation_ConclusionGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationConclusionInsert(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationConclusion.RecommendationId);
			DP.Add("UserCreatedId", _mRRecommendationConclusion.UserCreatedId);
			DP.Add("UnitCreatedId", _mRRecommendationConclusion.UnitCreatedId);
			DP.Add("ReceiverId", _mRRecommendationConclusion.ReceiverId);
			DP.Add("UnitReceiverId", _mRRecommendationConclusion.UnitReceiverId);
			DP.Add("Status", _mRRecommendationConclusion.Status);
			DP.Add("Content", _mRRecommendationConclusion.Content);
			DP.Add("SendDate", _mRRecommendationConclusion.SendDate);
			DP.Add("ExpiredDate", _mRRecommendationConclusion.ExpiredDate);
			DP.Add("ProcessingDate", _mRRecommendationConclusion.ProcessingDate);
			DP.Add("IsViewed", _mRRecommendationConclusion.IsViewed);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ConclusionInsert", DP));
		}

		public async Task<int> MRRecommendationConclusionUpdate(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationConclusion.Id);
			DP.Add("RecommendationId", _mRRecommendationConclusion.RecommendationId);
			DP.Add("UserCreatedId", _mRRecommendationConclusion.UserCreatedId);
			DP.Add("UnitCreatedId", _mRRecommendationConclusion.UnitCreatedId);
			DP.Add("ReceiverId", _mRRecommendationConclusion.ReceiverId);
			DP.Add("UnitReceiverId", _mRRecommendationConclusion.UnitReceiverId);
			DP.Add("Status", _mRRecommendationConclusion.Status);
			DP.Add("Content", _mRRecommendationConclusion.Content);
			DP.Add("SendDate", _mRRecommendationConclusion.SendDate);
			DP.Add("ExpiredDate", _mRRecommendationConclusion.ExpiredDate);
			DP.Add("ProcessingDate", _mRRecommendationConclusion.ProcessingDate);
			DP.Add("IsViewed", _mRRecommendationConclusion.IsViewed);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ConclusionUpdate", DP));
		}

		public async Task<int> MRRecommendationConclusionDelete(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationConclusion.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ConclusionDelete", DP));
		}

		public async Task<int> MRRecommendationConclusionDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ConclusionDeleteAll", DP));
		}
	}

	public class MRRecommendationConclusionFilesOnPage
	{
		public int Id { get; set; }
		public int? ConclusionId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendationConclusionFiles
	{
		private SQLCon _sQLCon;

		public MRRecommendationConclusionFiles(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationConclusionFiles()
		{
		}

		public int Id { get; set; }
		public int? ConclusionId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }

		public async Task<MRRecommendationConclusionFiles> MRRecommendationConclusionFilesGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionFiles>("MR_Recommendation_Conclusion_FilesGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendationConclusionFiles>> MRRecommendationConclusionFilesGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionFiles>("MR_Recommendation_Conclusion_FilesGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationConclusionFilesOnPage>> MRRecommendationConclusionFilesGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationConclusionFilesOnPage>("MR_Recommendation_Conclusion_FilesGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationConclusionFilesInsert(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ConclusionId", _mRRecommendationConclusionFiles.ConclusionId);
			DP.Add("Name", _mRRecommendationConclusionFiles.Name);
			DP.Add("FileType", _mRRecommendationConclusionFiles.FileType);
			DP.Add("FilePath", _mRRecommendationConclusionFiles.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_Conclusion_FilesInsert", DP));
		}

		public async Task<int> MRRecommendationConclusionFilesUpdate(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationConclusionFiles.Id);
			DP.Add("ConclusionId", _mRRecommendationConclusionFiles.ConclusionId);
			DP.Add("Name", _mRRecommendationConclusionFiles.Name);
			DP.Add("FileType", _mRRecommendationConclusionFiles.FileType);
			DP.Add("FilePath", _mRRecommendationConclusionFiles.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_Conclusion_FilesUpdate", DP));
		}

		public async Task<int> MRRecommendationConclusionFilesDelete(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationConclusionFiles.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_Conclusion_FilesDelete", DP));
		}

		public async Task<int> MRRecommendationConclusionFilesDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_Conclusion_FilesDeleteAll", DP));
		}
	}

	public class MRRecommendationFilesOnPage
	{
		public int Id { get; set; }
		public int? RecommendationId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendationFiles
	{
		private SQLCon _sQLCon;

		public MRRecommendationFiles(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationFiles()
		{
		}

		public int Id { get; set; }
		public int? RecommendationId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FilePath { get; set; }

		public async Task<MRRecommendationFiles> MRRecommendationFilesGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFiles>("MR_Recommendation_FilesGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendationFiles>> MRRecommendationFilesGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFiles>("MR_Recommendation_FilesGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationFilesOnPage>> MRRecommendationFilesGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationFilesOnPage>("MR_Recommendation_FilesGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationFilesInsert(MRRecommendationFiles _mRRecommendationFiles)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationFiles.RecommendationId);
			DP.Add("Name", _mRRecommendationFiles.Name);
			DP.Add("FileType", _mRRecommendationFiles.FileType);
			DP.Add("FilePath", _mRRecommendationFiles.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FilesInsert", DP));
		}

		public async Task<int> MRRecommendationFilesUpdate(MRRecommendationFiles _mRRecommendationFiles)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationFiles.Id);
			DP.Add("RecommendationId", _mRRecommendationFiles.RecommendationId);
			DP.Add("Name", _mRRecommendationFiles.Name);
			DP.Add("FileType", _mRRecommendationFiles.FileType);
			DP.Add("FilePath", _mRRecommendationFiles.FilePath);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FilesUpdate", DP));
		}

		public async Task<int> MRRecommendationFilesDelete(MRRecommendationFiles _mRRecommendationFiles)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationFiles.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FilesDelete", DP));
		}

		public async Task<int> MRRecommendationFilesDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_FilesDeleteAll", DP));
		}
	}

	public class MRRecommendationForwardOnPage
	{
		public int Id { get; set; }
		public int RecommendationId { get; set; }
		public long? UserSendId { get; set; }
		public int? UnitSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }
		public byte? Status { get; set; }
		public string Content { get; set; }
		public string ReasonDeny { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendationForward
	{
		private SQLCon _sQLCon;

		public MRRecommendationForward(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationForward()
		{
		}

		public int Id { get; set; }
		public int RecommendationId { get; set; }
		public long? UserSendId { get; set; }
		public int? UnitSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }
		public byte? Status { get; set; }
		public string Content { get; set; }
		public string ReasonDeny { get; set; }
		public DateTime? SendDate { get; set; }
		public DateTime? ExpiredDate { get; set; }
		public DateTime? ProcessingDate { get; set; }
		public bool? IsViewed { get; set; }

		public async Task<MRRecommendationForward> MRRecommendationForwardGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationForward>("MR_Recommendation_ForwardGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendationForward>> MRRecommendationForwardGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationForward>("MR_Recommendation_ForwardGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationForwardOnPage>> MRRecommendationForwardGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationForwardOnPage>("MR_Recommendation_ForwardGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationForwardInsert(MRRecommendationForward _mRRecommendationForward)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationForward.RecommendationId);
			DP.Add("UserSendId", _mRRecommendationForward.UserSendId);
			DP.Add("UnitSendId", _mRRecommendationForward.UnitSendId);
			DP.Add("ReceiveId", _mRRecommendationForward.ReceiveId);
			DP.Add("UnitReceiveId", _mRRecommendationForward.UnitReceiveId);
			DP.Add("Status", _mRRecommendationForward.Status);
			DP.Add("Content", _mRRecommendationForward.Content);
			DP.Add("ReasonDeny", _mRRecommendationForward.ReasonDeny);
			DP.Add("SendDate", _mRRecommendationForward.SendDate);
			DP.Add("ExpiredDate", _mRRecommendationForward.ExpiredDate);
			DP.Add("ProcessingDate", _mRRecommendationForward.ProcessingDate);
			DP.Add("IsViewed", _mRRecommendationForward.IsViewed);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardInsert", DP));
		}

		public async Task<int> MRRecommendationForwardUpdate(MRRecommendationForward _mRRecommendationForward)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationForward.Id);
			DP.Add("RecommendationId", _mRRecommendationForward.RecommendationId);
			DP.Add("UserSendId", _mRRecommendationForward.UserSendId);
			DP.Add("UnitSendId", _mRRecommendationForward.UnitSendId);
			DP.Add("ReceiveId", _mRRecommendationForward.ReceiveId);
			DP.Add("UnitReceiveId", _mRRecommendationForward.UnitReceiveId);
			DP.Add("Status", _mRRecommendationForward.Status);
			DP.Add("Content", _mRRecommendationForward.Content);
			DP.Add("ReasonDeny", _mRRecommendationForward.ReasonDeny);
			DP.Add("SendDate", _mRRecommendationForward.SendDate);
			DP.Add("ExpiredDate", _mRRecommendationForward.ExpiredDate);
			DP.Add("ProcessingDate", _mRRecommendationForward.ProcessingDate);
			DP.Add("IsViewed", _mRRecommendationForward.IsViewed);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardUpdate", DP));
		}

		public async Task<int> MRRecommendationForwardDelete(MRRecommendationForward _mRRecommendationForward)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationForward.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardDelete", DP));
		}

		public async Task<int> MRRecommendationForwardDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_ForwardDeleteAll", DP));
		}
	}

	public class MRRecommendationGenCodeOnPage
	{
		public int Id { get; set; }
		public double CurrentNumber { get; set; }
		public int Year { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendationGenCode
	{
		private SQLCon _sQLCon;

		public MRRecommendationGenCode(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationGenCode()
		{
		}

		public int Id { get; set; }
		public double CurrentNumber { get; set; }
		public int Year { get; set; }

		public async Task<MRRecommendationGenCode> MRRecommendationGenCodeGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGenCode>("MR_Recommendation_GenCodeGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendationGenCode>> MRRecommendationGenCodeGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGenCode>("MR_Recommendation_GenCodeGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationGenCodeOnPage>> MRRecommendationGenCodeGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationGenCodeOnPage>("MR_Recommendation_GenCodeGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationGenCodeInsert(MRRecommendationGenCode _mRRecommendationGenCode)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("CurrentNumber", _mRRecommendationGenCode.CurrentNumber);
			DP.Add("Year", _mRRecommendationGenCode.Year);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_GenCodeInsert", DP));
		}

		public async Task<int> MRRecommendationGenCodeUpdate(MRRecommendationGenCode _mRRecommendationGenCode)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationGenCode.Id);
			DP.Add("CurrentNumber", _mRRecommendationGenCode.CurrentNumber);
			DP.Add("Year", _mRRecommendationGenCode.Year);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_GenCodeUpdate", DP));
		}

		public async Task<int> MRRecommendationGenCodeDelete(MRRecommendationGenCode _mRRecommendationGenCode)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationGenCode.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_GenCodeDelete", DP));
		}

		public async Task<int> MRRecommendationGenCodeDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_GenCodeDeleteAll", DP));
		}
	}

	public class MRRecommendationHashtagOnPage
	{
		public long Id { get; set; }
		public int RecommendationId { get; set; }
		public int HashtagId { get; set; }
		public string HashtagName { get; set; }
		public int? RowNumber; // int, null
	}

	public class MRRecommendationHashtag
	{
		private SQLCon _sQLCon;

		public MRRecommendationHashtag(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public MRRecommendationHashtag()
		{
		}

		public long Id { get; set; }
		public int RecommendationId { get; set; }
		public int HashtagId { get; set; }
		public string HashtagName { get; set; }

		public async Task<MRRecommendationHashtag> MRRecommendationHashtagGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtag>("MR_Recommendation_HashtagGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<MRRecommendationHashtag>> MRRecommendationHashtagGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtag>("MR_Recommendation_HashtagGetAll", DP)).ToList();
		}

		public async Task<List<MRRecommendationHashtagOnPage>> MRRecommendationHashtagGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<MRRecommendationHashtagOnPage>("MR_Recommendation_HashtagGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> MRRecommendationHashtagInsert(MRRecommendationHashtag _mRRecommendationHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RecommendationId", _mRRecommendationHashtag.RecommendationId);
			DP.Add("HashtagId", _mRRecommendationHashtag.HashtagId);
			DP.Add("HashtagName", _mRRecommendationHashtag.HashtagName);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_HashtagInsert", DP));
		}

		public async Task<int> MRRecommendationHashtagUpdate(MRRecommendationHashtag _mRRecommendationHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationHashtag.Id);
			DP.Add("RecommendationId", _mRRecommendationHashtag.RecommendationId);
			DP.Add("HashtagId", _mRRecommendationHashtag.HashtagId);
			DP.Add("HashtagName", _mRRecommendationHashtag.HashtagName);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_HashtagUpdate", DP));
		}

		public async Task<int> MRRecommendationHashtagDelete(MRRecommendationHashtag _mRRecommendationHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _mRRecommendationHashtag.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_HashtagDelete", DP));
		}

		public async Task<int> MRRecommendationHashtagDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("MR_Recommendation_HashtagDeleteAll", DP));
		}
	}
}
