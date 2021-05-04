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
}
