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
	public class INVFileAttachDeleteById
	{
		private SQLCon _sQLCon;

		public INVFileAttachDeleteById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVFileAttachDeleteById()
		{
		}

		public async Task<int> INVFileAttachDeleteByIdDAO(INVFileAttachDeleteByIdIN _iNVFileAttachDeleteByIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _iNVFileAttachDeleteByIdIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("INV_FileAttachDeleteById", DP));
		}
	}

	public class INVFileAttachDeleteByIdIN
	{
		public int? Id { get; set; }
	}

	public class INVFileAttachDeleteByInvitationId
	{
		private SQLCon _sQLCon;

		public INVFileAttachDeleteByInvitationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVFileAttachDeleteByInvitationId()
		{
		}

		public async Task<int> INVFileAttachDeleteByInvitationIdDAO(INVFileAttachDeleteByInvitationIdIN _iNVFileAttachDeleteByInvitationIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InvitationId", _iNVFileAttachDeleteByInvitationIdIN.InvitationId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("INV_FileAttachDeleteByInvitationId", DP));
		}
	}

	public class INVFileAttachDeleteByInvitationIdIN
	{
		public int? InvitationId { get; set; }
	}

	public class INVFileAttachGetAllByInvitationId
	{
		private SQLCon _sQLCon;

		public INVFileAttachGetAllByInvitationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVFileAttachGetAllByInvitationId()
		{
		}

		public int id { get; set; }
		public int InvitationId { get; set; }
		public string FileAttach { get; set; }
		public string Name { get; set; }
		public byte FileType { get; set; }

		public async Task<List<INVFileAttachGetAllByInvitationId>> INVFileAttachGetAllByInvitationIdDAO(int? InvitationId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InvitationId", InvitationId);

			return (await _sQLCon.ExecuteListDapperAsync<INVFileAttachGetAllByInvitationId>("INV_FileAttachGetAllByInvitationId", DP)).ToList();
		}
	}

	public class INVFileAttachInsert
	{
		private SQLCon _sQLCon;

		public INVFileAttachInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVFileAttachInsert()
		{
		}

		public async Task<int> INVFileAttachInsertDAO(INVFileAttachInsertIN _iNVFileAttachInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InvitationId", _iNVFileAttachInsertIN.InvitationId);
			DP.Add("FileAttach", _iNVFileAttachInsertIN.FileAttach);
			DP.Add("Name", _iNVFileAttachInsertIN.Name);
			DP.Add("FileType", _iNVFileAttachInsertIN.FileType);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("INV_FileAttachInsert", DP));
		}
	}

	public class INVFileAttachInsertIN
	{
		public int? InvitationId { get; set; }
		public string FileAttach { get; set; }
		public string Name { get; set; }
		public int? FileType { get; set; }
	}

	public class INVInvitationUserMapDeleteByInvitationId
	{
		private SQLCon _sQLCon;

		public INVInvitationUserMapDeleteByInvitationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationUserMapDeleteByInvitationId()
		{
		}

		public async Task<int> INVInvitationUserMapDeleteByInvitationIdDAO(INVInvitationUserMapDeleteByInvitationIdIN _iNVInvitationUserMapDeleteByInvitationIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InvitationId", _iNVInvitationUserMapDeleteByInvitationIdIN.InvitationId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("INV_Invitation_User_MapDeleteByInvitationId", DP));
		}
	}

	public class INVInvitationUserMapDeleteByInvitationIdIN
	{
		public int? InvitationId { get; set; }
	}

	public class INVInvitationUserMapGetByInvitationId
	{
		private SQLCon _sQLCon;

		public INVInvitationUserMapGetByInvitationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationUserMapGetByInvitationId()
		{
		}

		public int UserId { get; set; }
		public int InvitationId { get; set; }
		public bool Watched { get; set; }
		public bool SendEmail { get; set; }
		public bool SendSMS { get; set; }

		public async Task<List<INVInvitationUserMapGetByInvitationId>> INVInvitationUserMapGetByInvitationIdDAO(int? InvitationId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InvitationId", InvitationId);

			return (await _sQLCon.ExecuteListDapperAsync<INVInvitationUserMapGetByInvitationId>("INV_Invitation_User_MapGetByInvitationId", DP)).ToList();
		}
	}

	public class INVInvitationUserMapInsert
	{
		private SQLCon _sQLCon;

		public INVInvitationUserMapInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationUserMapInsert()
		{
		}

		public async Task<decimal?> INVInvitationUserMapInsertDAO(INVInvitationUserMapInsertIN _iNVInvitationUserMapInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _iNVInvitationUserMapInsertIN.UserId);
			DP.Add("InvitationId", _iNVInvitationUserMapInsertIN.InvitationId);
			DP.Add("Watched", _iNVInvitationUserMapInsertIN.Watched);
			DP.Add("SendEmail", _iNVInvitationUserMapInsertIN.SendEmail);
			DP.Add("SendSMS", _iNVInvitationUserMapInsertIN.SendSMS);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("INV_Invitation_User_MapInsert", DP);
		}
	}

	public class INVInvitationUserMapInsertIN
	{
		public int? UserId { get; set; }
		public int? InvitationId { get; set; }
		public bool? Watched { get; set; }
		public bool? SendEmail { get; set; }
		public bool? SendSMS { get; set; }
	}

	public class INVInvitationDelete
	{
		private SQLCon _sQLCon;

		public INVInvitationDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationDelete()
		{
		}

		public async Task<int> INVInvitationDeleteDAO(INVInvitationDeleteIN _iNVInvitationDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _iNVInvitationDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("INV_InvitationDelete", DP));
		}
	}

	public class INVInvitationDeleteIN
	{
		public int? Id { get; set; }
	}

	public class INVInvitationGetAllOnPage
	{
		private SQLCon _sQLCon;

		public INVInvitationGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Place { get; set; }
		public byte Status { get; set; }
		public int? AmountWatched { get; set; }

		public async Task<List<INVInvitationGetAllOnPage>> INVInvitationGetAllOnPageDAO(int? PageSize, int? PageIndex, string Title, DateTime? StartDate, DateTime? EndDate, string Place, byte? Status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Title", Title);
			DP.Add("StartDate", StartDate);
			DP.Add("EndDate", EndDate);
			DP.Add("Place", Place);
			DP.Add("Status", Status);

			return (await _sQLCon.ExecuteListDapperAsync<INVInvitationGetAllOnPage>("INV_InvitationGetAllOnPage", DP)).ToList();
		}
	}

	public class SYUserReadedInvitationGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYUserReadedInvitationGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserReadedInvitationGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Avatar { get; set; }
		public DateTime? WatchedDate { get; set; }

		public async Task<List<SYUserReadedInvitationGetAllOnPage>> INVInvitationGetAllOnPageDAO(int InvitationId , string UserName, DateTime? WatchedDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InvitationId", InvitationId);
			DP.Add("UserName", UserName);
			DP.Add("WatchedDate", WatchedDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserReadedInvitationGetAllOnPage>("SY_UserReadedInvitationGetAllOnPage", DP)).ToList();
		}
	}

	public class INVInvitationGetById
	{
		private SQLCon _sQLCon;

		public INVInvitationGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationGetById()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Content { get; set; }
		public string Place { get; set; }
		public string Note { get; set; }
		public DateTime CreateDate { get; set; }
		public int UserCreateId { get; set; }
		public DateTime? SendDate { get; set; }
		public byte Status { get; set; }
		public long IsView { get; set; }
		public int? Member { get; set; }

		public async Task<List<INVInvitationGetById>> INVInvitationGetByIdDAO(int? id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("id", id);

			return (await _sQLCon.ExecuteListDapperAsync<INVInvitationGetById>("INV_InvitationGetById", DP)).ToList();
		}
	}

	public class INVInvitationInsert
	{
		private SQLCon _sQLCon;

		public INVInvitationInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationInsert()
		{
		}

		public async Task<decimal?> INVInvitationInsertDAO(INVInvitationInsertIN _iNVInvitationInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Title", _iNVInvitationInsertIN.Title);
			DP.Add("StartDate", _iNVInvitationInsertIN.StartDate);
			DP.Add("EndDate", _iNVInvitationInsertIN.EndDate);
			DP.Add("Content", _iNVInvitationInsertIN.Content);
			DP.Add("Place", _iNVInvitationInsertIN.Place);
			DP.Add("Note", _iNVInvitationInsertIN.Note);
			DP.Add("CreateDate", _iNVInvitationInsertIN.CreateDate);
			DP.Add("UserCreateId", _iNVInvitationInsertIN.UserCreateId);
			DP.Add("SendDate", _iNVInvitationInsertIN.SendDate);
			DP.Add("Status", _iNVInvitationInsertIN.Status);
			DP.Add("IsView", _iNVInvitationInsertIN.IsView);
			DP.Add("Member", _iNVInvitationInsertIN.Member);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("INV_InvitationInsert", DP);
		}
	}

	public class INVInvitationInsertIN
	{
		public string Title { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Content { get; set; }
		public string Place { get; set; }
		public string Note { get; set; }
		public DateTime? CreateDate { get; set; }
		public int? UserCreateId { get; set; }
		public DateTime? SendDate { get; set; }
		public int? Status { get; set; }
		public int? IsView { get; set; }
		public int? Member { get; set; }
	}

	public class INVInvitationUpdate
	{
		private SQLCon _sQLCon;

		public INVInvitationUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public INVInvitationUpdate()
		{
		}

		public async Task<int?> INVInvitationUpdateDAO(INVInvitationUpdateIN _iNVInvitationUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _iNVInvitationUpdateIN.Id);
			DP.Add("Title", _iNVInvitationUpdateIN.Title);
			DP.Add("StartDate", _iNVInvitationUpdateIN.StartDate);
			DP.Add("EndDate", _iNVInvitationUpdateIN.EndDate);
			DP.Add("Content", _iNVInvitationUpdateIN.Content);
			DP.Add("Place", _iNVInvitationUpdateIN.Place);
			DP.Add("Note", _iNVInvitationUpdateIN.Note);
			DP.Add("UpdateDate", _iNVInvitationUpdateIN.UpdateDate);
			DP.Add("UserUpdate", _iNVInvitationUpdateIN.UserUpdate);
			DP.Add("Status", _iNVInvitationUpdateIN.Status);
			DP.Add("IsView", _iNVInvitationUpdateIN.IsView);
			DP.Add("Member", _iNVInvitationUpdateIN.Member);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("INV_InvitationUpdate", DP);
		}
	}

	public class INVInvitationUpdateIN
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string Content { get; set; }
		public string Place { get; set; }
		public string Note { get; set; }
		public DateTime? UpdateDate { get; set; }
		public int? UserUpdate { get; set; }
		public int? Status { get; set; }
		public int? IsView { get; set; }
		public int? Member { get; set; }
	}
}
