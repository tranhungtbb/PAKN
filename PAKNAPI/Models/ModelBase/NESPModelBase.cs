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
	public class NENewsDelete
	{
		private SQLCon _sQLCon;

		public NENewsDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsDelete()
		{
		}

		public async Task<int> NENewsDeleteDAO(NENewsDeleteIN _nENewsDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _nENewsDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsDelete", DP));
		}
	}

	public class NENewsDeleteIN
	{
		public int? Id { get; set; }
	}

	public class NENewsGetAllOnPage
	{
		private SQLCon _sQLCon;

		public NENewsGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsGetAllOnPage()
		{
		}

		public bool PostType { get; set; }
		public bool IsPublished { get; set; }
		public int Status { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public int? ViewCount { get; set; }
		public string Url { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? PublishedBy { get; set; }
		public DateTime? PublishedDate { get; set; }
		public int? WithdrawBy { get; set; }
		public DateTime? WithdrawDate { get; set; }
		public int? RowNumber; // int, null

		public async Task<List<NENewsGetAllOnPage>> NENewsGetAllOnPageDAO(string NewsIds, int? PageSize, int? PageIndex, string Title, int? NewsType, int? Status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("NewsIds", NewsIds);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Title", Title);
			DP.Add("NewsType", NewsType);
			DP.Add("Status", Status);

			return (await _sQLCon.ExecuteListDapperAsync<NENewsGetAllOnPage>("NE_NewsGetAllOnPage", DP)).ToList();
		}
	}

	public class NENewsGetByID
	{
		private SQLCon _sQLCon;

		public NENewsGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsGetByID()
		{
		}

		public bool PostType { get; set; }
		public bool IsPublished { get; set; }
		public int Status { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public int? ViewCount { get; set; }
		public string Url { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? PublishedBy { get; set; }
		public DateTime? PublishedDate { get; set; }
		public int? WithdrawBy { get; set; }
		public DateTime? WithdrawDate { get; set; }
		public string NewsRelateIds { get; set; }

		public async Task<List<NENewsGetByID>> NENewsGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NENewsGetByID>("NE_NewsGetByID", DP)).ToList();
		}
	}

	public class NENewsGetByIDOnJoin
	{
		IAppSetting _appSetting;

		public NENewsGetByIDOnJoin(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		public NENewsGetByIDOnJoin()
		{
		}

		public bool PostType { get; set; }
		public bool IsPublished { get; set; }
		public int Status { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public int? ViewCount { get; set; }
		public string Url { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? PublishedBy { get; set; }
		public DateTime? PublishedDate { get; set; }
		public int? WithdrawBy { get; set; }
		public DateTime? WithdrawDate { get; set; }
		public List<NENewsGetByIDOnJoinNewsRelates> cNENewsGetByIDOnJoinNewsRelates = new List<NENewsGetByIDOnJoinNewsRelates>();

		public class NENewsGetByIDOnJoinNewsRelates
		{
			public int? NewsRelates_Id { get; set; }
			public string NewsRelates_Title { get; set; }
			public string NewsRelates_ImagePath { get; set; }
		}

		public async Task<List<NENewsGetByIDOnJoin>> NENewsGetByIDOnJoinDAO(int? Id)
		{
			List<NENewsGetByIDOnJoin> mNENewsGetByIDOnJoins = new List<NENewsGetByIDOnJoin>();
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			using (SqlConnection conn = new SqlConnection(_appSetting.GetConnectstring()))
			{
				(await conn.QueryAsync<NENewsGetByIDOnJoin, NENewsGetByIDOnJoinNewsRelates, NENewsGetByIDOnJoin>("NENewsGetByIDOnJoin", (mNENewsGetByIDOnJoin, cNENewsGetByIDOnJoinNewsRelates) =>
				{
					var _mNENewsGetByIDOnJoin = mNENewsGetByIDOnJoins.Find(item => item.Id == mNENewsGetByIDOnJoin.Id);
					if (_mNENewsGetByIDOnJoin == null)
					{
						mNENewsGetByIDOnJoins.Add(mNENewsGetByIDOnJoin);
						_mNENewsGetByIDOnJoin = mNENewsGetByIDOnJoin;
					}

					_mNENewsGetByIDOnJoin.cNENewsGetByIDOnJoinNewsRelates = _mNENewsGetByIDOnJoin.cNENewsGetByIDOnJoinNewsRelates ?? new List<NENewsGetByIDOnJoinNewsRelates>();
					if (cNENewsGetByIDOnJoinNewsRelates != null && _mNENewsGetByIDOnJoin.cNENewsGetByIDOnJoinNewsRelates.Find(item => item.NewsRelates_Id == cNENewsGetByIDOnJoinNewsRelates.NewsRelates_Id && item.NewsRelates_Title == cNENewsGetByIDOnJoinNewsRelates.NewsRelates_Title && item.NewsRelates_ImagePath == cNENewsGetByIDOnJoinNewsRelates.NewsRelates_ImagePath) == null)
						_mNENewsGetByIDOnJoin.cNENewsGetByIDOnJoinNewsRelates.Add(cNENewsGetByIDOnJoinNewsRelates);

					return _mNENewsGetByIDOnJoin;
				}, DP, splitOn: "PostType, NewsRelates_Id", commandType: CommandType.StoredProcedure)).ToList();
			}

			return mNENewsGetByIDOnJoins;
		}
	}

	public class NENewsInsert
	{
		private SQLCon _sQLCon;

		public NENewsInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsInsert()
		{
		}

		public async Task<int> NENewsInsertDAO(NENewsInsertIN _nENewsInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PostType", _nENewsInsertIN.PostType);
			DP.Add("IsPublished", _nENewsInsertIN.IsPublished);
			DP.Add("Status", _nENewsInsertIN.Status);
			DP.Add("Title", _nENewsInsertIN.Title);
			DP.Add("Summary", _nENewsInsertIN.Summary);
			DP.Add("Contents", _nENewsInsertIN.Contents);
			DP.Add("ImagePath", _nENewsInsertIN.ImagePath);
			DP.Add("NewsType", _nENewsInsertIN.NewsType);
			DP.Add("ViewCount", _nENewsInsertIN.ViewCount);
			DP.Add("Url", _nENewsInsertIN.Url);
			DP.Add("CreatedBy", _nENewsInsertIN.CreatedBy);
			DP.Add("CreatedDate", _nENewsInsertIN.CreatedDate);
			DP.Add("UpdatedBy", _nENewsInsertIN.UpdatedBy);
			DP.Add("UpdatedDate", _nENewsInsertIN.UpdatedDate);
			DP.Add("PublishedBy", _nENewsInsertIN.PublishedBy);
			DP.Add("PublishedDate", _nENewsInsertIN.PublishedDate);
			DP.Add("WithdrawBy", _nENewsInsertIN.WithdrawBy);
			DP.Add("WithdrawDate", _nENewsInsertIN.WithdrawDate);
			DP.Add("NewsRelateIds", _nENewsInsertIN.NewsRelateIds);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsInsert", DP));
		}
	}

	public class NENewsInsertIN
	{
		public bool? PostType { get; set; }
		public bool? IsPublished { get; set; }
		public int? Status { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public int? ViewCount { get; set; }
		public string Url { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? PublishedBy { get; set; }
		public DateTime? PublishedDate { get; set; }
		public int? WithdrawBy { get; set; }
		public DateTime? WithdrawDate { get; set; }
		public string NewsRelateIds { get; set; }
	}

	public class NENewsUpdate
	{
		private SQLCon _sQLCon;

		public NENewsUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsUpdate()
		{
		}

		public async Task<int> NENewsUpdateDAO(NENewsUpdateIN _nENewsUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PostType", _nENewsUpdateIN.PostType);
			DP.Add("IsPublished", _nENewsUpdateIN.IsPublished);
			DP.Add("Status", _nENewsUpdateIN.Status);
			DP.Add("Id", _nENewsUpdateIN.Id);
			DP.Add("Title", _nENewsUpdateIN.Title);
			DP.Add("Summary", _nENewsUpdateIN.Summary);
			DP.Add("Contents", _nENewsUpdateIN.Contents);
			DP.Add("ImagePath", _nENewsUpdateIN.ImagePath);
			DP.Add("NewsType", _nENewsUpdateIN.NewsType);
			DP.Add("ViewCount", _nENewsUpdateIN.ViewCount);
			DP.Add("Url", _nENewsUpdateIN.Url);
			DP.Add("CreatedBy", _nENewsUpdateIN.CreatedBy);
			DP.Add("CreatedDate", _nENewsUpdateIN.CreatedDate);
			DP.Add("UpdatedBy", _nENewsUpdateIN.UpdatedBy);
			DP.Add("UpdatedDate", _nENewsUpdateIN.UpdatedDate);
			DP.Add("PublishedBy", _nENewsUpdateIN.PublishedBy);
			DP.Add("PublishedDate", _nENewsUpdateIN.PublishedDate);
			DP.Add("WithdrawBy", _nENewsUpdateIN.WithdrawBy);
			DP.Add("WithdrawDate", _nENewsUpdateIN.WithdrawDate);
			DP.Add("NewsRelateIds", _nENewsUpdateIN.NewsRelateIds);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsUpdate", DP));
		}
	}

	public class NENewsUpdateIN
	{
		public bool? PostType { get; set; }
		public bool? IsPublished { get; set; }
		public int? Status { get; set; }
		public int? Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public int? ViewCount { get; set; }
		public string Url { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public int? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? PublishedBy { get; set; }
		public DateTime? PublishedDate { get; set; }
		public int? WithdrawBy { get; set; }
		public DateTime? WithdrawDate { get; set; }
		public string NewsRelateIds { get; set; }
	}
}
