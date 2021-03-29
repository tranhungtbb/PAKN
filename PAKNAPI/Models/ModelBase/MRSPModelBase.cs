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

		public async Task<decimal> MRRecommendationInsertDAO(MRRecommendationInsertIN _mRRecommendationInsertIN)
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
			DP.Add("CreatedBy", _mRRecommendationInsertIN.CreatedBy);
			DP.Add("CreatedDate", _mRRecommendationInsertIN.CreatedDate);
			DP.Add("UpdatedBy", _mRRecommendationInsertIN.UpdatedBy);
			DP.Add("UpdatedDate", _mRRecommendationInsertIN.UpdatedDate);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal>("MR_RecommendationInsert", DP);
		}
	}

	public class MRRecommendationInsertIN
	{
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public int? UnitId { get; set; }
		public bool? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
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
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
	}
}
