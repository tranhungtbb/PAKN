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
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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

		public int? RowNumber { get; set; }
		public string PostType { get; set; }
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

	public class NENewsGetAllRelates
	{
		private SQLCon _sQLCon;

		public NENewsGetAllRelates(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsGetAllRelates()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string ImagePath { get; set; }

		public async Task<List<NENewsGetAllRelates>> NENewsGetAllRelatesDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NENewsGetAllRelates>("NE_NewsGetAllRelates", DP)).ToList();
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

		public string PostType { get; set; }
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
		public bool? IsNotification { get; set; }
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
		private SQLCon _sQLCon;

		public NENewsGetByIDOnJoin(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsGetByIDOnJoin()
		{
		}

		public string PostType { get; set; }
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
		public int? NewsRelates_Id { get; set; }
		public string NewsRelates_Title { get; set; }
		public string NewsRelates_ImagePath { get; set; }

		public async Task<List<NENewsGetByIDOnJoin>> NENewsGetByIDOnJoinDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NENewsGetByIDOnJoin>("NE_NewsGetByIDOnJoin", DP)).ToList();
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

		public async Task<int?> NENewsInsertDAO(NENewsInsertIN _nENewsInsertIN)
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
			DP.Add("IsNotification", _nENewsInsertIN.IsNotification);

			return await _sQLCon.ExecuteScalarDapperAsync<int>("NE_NewsInsert", DP);
		}
	}

	public class NENewsInsertIN
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Kiểu bài viết không được để trống")]
		public string PostType { get; set; }
		public bool? IsPublished { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Trạng thái không đúng định dạng")]
		public int? Status { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Tiêu đề bài viết không được để trống")]
		[StringLength(500, ErrorMessage = "Tiêu đề bài viết không vượt quá 500 kí tự")]
		public string Title { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung tóm tắt bài viết không được để trống")]
		public string Summary { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung bài viết không được để trống")]
		public string Contents { get; set; }
		public string ImagePath { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Kiểu bài viết không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Kiểu bài viết không đúng định dạng")]
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

		public bool? IsNotification { get; set; }
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
			DP.Add("IsNotification", _nENewsUpdateIN.IsNotification);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("NE_NewsUpdate", DP));
		}
	}

	public class NENewsUpdateIN
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Kiểu bài viết không được để trống")]
		public string PostType { get; set; }
		public bool? IsPublished { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Trạng thái không đúng định dạng")]
		public int? Status { get; set; }
		public int? Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Tiêu đề bài viết không được để trống")]
		[StringLength(500, ErrorMessage = "Tiêu đề bài viết không vượt quá 500 kí tự")]
		public string Title { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung tóm tắt bài viết không được để trống")]
		public string Summary { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Nội dung bài viết không được để trống")]
		public string Contents { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Đường dẫn ảnh không được để trống")]
		public string ImagePath { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Kiểu bài viết không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Kiểu bài viết không đúng định dạng")]
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

		public bool? IsNotification { get; set; }
		
	}

	public class NENewsViewDetail
	{
		private SQLCon _sQLCon;

		public NENewsViewDetail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NENewsViewDetail()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }

		public int Status { get; set; }
		public string Summary { get; set; }
		public string Contents { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string ImagePath { get; set; }
		public string NewsRelateIds { get; set; }

		public async Task<List<NENewsViewDetail>> NENewsViewDetailDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<NENewsViewDetail>("NE_NewsViewDetail", DP)).ToList();
		}
	}

	public class NERelateGetAll
	{
		private SQLCon _sQLCon;

		public NERelateGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public NERelateGetAll()
		{
		}

		public int Id { get; set; }
		public int? NewsId { get; set; }
		public int? NewsIdRelate { get; set; }

		public async Task<List<NERelateGetAll>> NERelateGetAllDAO(int? NewsId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("NewsId", NewsId);

			return (await _sQLCon.ExecuteListDapperAsync<NERelateGetAll>("NE_RelateGetAll", DP)).ToList();
		}
	}
}
