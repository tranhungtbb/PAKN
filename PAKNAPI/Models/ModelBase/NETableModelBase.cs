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
	public class NENewsOnPage
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public bool PostType { get; set; }
		public bool IsPublished { get; set; }
		public int Status { get; set; }
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
	}

	public class NENews
	{
		private SQLCon _sQLCon;

		public NENews(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENews()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
		public bool PostType { get; set; }
		public bool IsPublished { get; set; }
		public int Status { get; set; }
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

		public async Task<NENews> NENewsGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NENews>("NE_NewsGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<NENews>> NENewsGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<NENews>("NE_NewsGetAll", DP)).ToList();
		}

		public async Task<List<NENewsOnPage>> NENewsGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<NENewsOnPage>("NE_NewsGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> NENewsInsert(NENews _nENews)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PostType", _nENews.PostType);
			DP.Add("IsPublished", _nENews.IsPublished);
			DP.Add("Status", _nENews.Status);
			DP.Add("Title", _nENews.Title);
			DP.Add("Summary", _nENews.Summary);
			DP.Add("Contents", _nENews.Contents);
			DP.Add("ImagePath", _nENews.ImagePath);
			DP.Add("NewsType", _nENews.NewsType);
			DP.Add("ViewCount", _nENews.ViewCount);
			DP.Add("Url", _nENews.Url);
			DP.Add("CreatedBy", _nENews.CreatedBy);
			DP.Add("CreatedDate", _nENews.CreatedDate);
			DP.Add("UpdatedBy", _nENews.UpdatedBy);
			DP.Add("UpdatedDate", _nENews.UpdatedDate);
			DP.Add("PublishedBy", _nENews.PublishedBy);
			DP.Add("PublishedDate", _nENews.PublishedDate);
			DP.Add("WithdrawBy", _nENews.WithdrawBy);
			DP.Add("WithdrawDate", _nENews.WithdrawDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsInsert", DP));
		}

		public async Task<int> NENewsUpdate(NENews _nENews)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PostType", _nENews.PostType);
			DP.Add("IsPublished", _nENews.IsPublished);
			DP.Add("Status", _nENews.Status);
			DP.Add("Id", _nENews.Id);
			DP.Add("Title", _nENews.Title);
			DP.Add("Summary", _nENews.Summary);
			DP.Add("Contents", _nENews.Contents);
			DP.Add("ImagePath", _nENews.ImagePath);
			DP.Add("NewsType", _nENews.NewsType);
			DP.Add("ViewCount", _nENews.ViewCount);
			DP.Add("Url", _nENews.Url);
			DP.Add("CreatedBy", _nENews.CreatedBy);
			DP.Add("CreatedDate", _nENews.CreatedDate);
			DP.Add("UpdatedBy", _nENews.UpdatedBy);
			DP.Add("UpdatedDate", _nENews.UpdatedDate);
			DP.Add("PublishedBy", _nENews.PublishedBy);
			DP.Add("PublishedDate", _nENews.PublishedDate);
			DP.Add("WithdrawBy", _nENews.WithdrawBy);
			DP.Add("WithdrawDate", _nENews.WithdrawDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsUpdate", DP));
		}

		public async Task<int> NENewsDelete(NENews _nENews)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _nENews.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsDelete", DP));
		}

		public async Task<int> NENewsDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsDeleteAll", DP));
		}
	}
}
