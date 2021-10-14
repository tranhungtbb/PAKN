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
	public class NEFileAttachOnPage
	{
		public int Id { get; set; }
		public int? NewsId { get; set; }
		public string FileAttach { get; set; }
		public string Name { get; set; }
		public byte? FileType { get; set; }
		public int? RowNumber; // int, null
	}

	public class NEFileAttach
	{
		private SQLCon _sQLCon;

		public NEFileAttach(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NEFileAttach()
		{
		}

		public int Id { get; set; }
		public int? NewsId { get; set; }
		public string FileAttach { get; set; }
		public string Name { get; set; }
		public byte? FileType { get; set; }

		public async Task<NEFileAttach> NEFileAttachGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NEFileAttach>("NE_FileAttachGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<NEFileAttach>> NEFileAttachGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<NEFileAttach>("NE_FileAttachGetAll", DP)).ToList();
		}

		public async Task<List<NEFileAttachOnPage>> NEFileAttachGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<NEFileAttachOnPage>("NE_FileAttachGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> NEFileAttachInsert(NEFileAttach _nEFileAttach)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("NewsId", _nEFileAttach.NewsId);
			DP.Add("FileAttach", _nEFileAttach.FileAttach);
			DP.Add("Name", _nEFileAttach.Name);
			DP.Add("FileType", _nEFileAttach.FileType);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_FileAttachInsert", DP));
		}

		public async Task<int> NEFileAttachUpdate(NEFileAttach _nEFileAttach)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _nEFileAttach.Id);
			DP.Add("NewsId", _nEFileAttach.NewsId);
			DP.Add("FileAttach", _nEFileAttach.FileAttach);
			DP.Add("Name", _nEFileAttach.Name);
			DP.Add("FileType", _nEFileAttach.FileType);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_FileAttachUpdate", DP));
		}

		public async Task<int> NEFileAttachDelete(NEFileAttach _nEFileAttach)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _nEFileAttach.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_FileAttachDelete", DP));
		}

		public async Task<int> NEFileAttachDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_FileAttachDeleteAll", DP));
		}

		public async Task<int> NEFileAttachCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("NE_FileAttachCount", DP));
		}
	}

	public class NENewsOnPage
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public string ImagePath { get; set; }
		public int? NewsType { get; set; }
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
		public string PostType { get; set; }
		public bool? IsNotification { get; set; }
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
		public string PostType { get; set; }
		public bool? IsNotification { get; set; }

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

		public async Task<int> NENewsChangeIsPublish(int? NewsId, int? Status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("NewsId", NewsId);
			DP.Add("Status", Status);
			return await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsChangeIsPublish", DP);
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
			DP.Add("IsPublished", _nENews.IsPublished);
			DP.Add("Status", _nENews.Status);
			DP.Add("Title", _nENews.Title);
			DP.Add("Summary", _nENews.Summary);
			DP.Add("Contents", _nENews.Contents);
			DP.Add("ImagePath", _nENews.ImagePath);
			DP.Add("PostType", _nENews.PostType);
			DP.Add("IsNotification", _nENews.IsNotification);
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
			DP.Add("IsPublished", _nENews.IsPublished);
			DP.Add("Status", _nENews.Status);
			DP.Add("Id", _nENews.Id);
			DP.Add("Title", _nENews.Title);
			DP.Add("Summary", _nENews.Summary);
			DP.Add("Contents", _nENews.Contents);
			DP.Add("ImagePath", _nENews.ImagePath);
			DP.Add("PostType", _nENews.PostType);
			DP.Add("IsNotification", _nENews.IsNotification);
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

		public async Task<int> NENewsCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("NE_NewsCount", DP));
		}
	}

	public class NERelateOnPage
	{
		public int Id { get; set; }
		public int? NewsId { get; set; }
		public int? NewsIdRelate { get; set; }
		public int? RowNumber; // int, null
	}

	public class NERelate
	{
		private SQLCon _sQLCon;

		public NERelate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NERelate()
		{
		}

		public int Id { get; set; }
		public int? NewsId { get; set; }
		public int? NewsIdRelate { get; set; }

		public async Task<NERelate> NERelateGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NERelate>("NE_RelateGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<NERelate>> NERelateGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<NERelate>("NE_RelateGetAll", DP)).ToList();
		}

		public async Task<List<NERelateOnPage>> NERelateGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<NERelateOnPage>("NE_RelateGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> NERelateInsert(NERelate _nERelate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("NewsId", _nERelate.NewsId);
			DP.Add("NewsIdRelate", _nERelate.NewsIdRelate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_RelateInsert", DP));
		}

		public async Task<int> NERelateUpdate(NERelate _nERelate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _nERelate.Id);
			DP.Add("NewsId", _nERelate.NewsId);
			DP.Add("NewsIdRelate", _nERelate.NewsIdRelate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_RelateUpdate", DP));
		}

		public async Task<int> NERelateDelete(NERelate _nERelate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _nERelate.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_RelateDelete", DP));
		}

		public async Task<int> NERelateDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_RelateDeleteAll", DP));
		}

		public async Task<int> NERelateCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("NE_RelateCount", DP));
		}
	}
}
