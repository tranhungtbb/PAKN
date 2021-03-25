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
	public class SYLOGGetByID
	{
		private SQLCon _sQLCon;

		public SYLOGGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYLOGGetByID()
		{
		}

		public long Id;
		public long UserId;
		public byte Status;
		public string Action;
		public string Exception;
		public string FullName;
		public string IPAddress;
		public string MACAddress;
		public string Description;
		public DateTime? CreatedDate;

		public async Task<List<SYLOGGetByID>> SYLOGGetByIDDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYLOGGetByID>("SY_LOG_GetByID", DP)).ToList();
		}
	}

	public class SYLOGInsert
	{
		private SQLCon _sQLCon;

		public SYLOGInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYLOGInsert()
		{
		}

		public async Task<int> SYLOGInsertDAO(SYLOGInsertIN _sYLOGInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", _sYLOGInsertIN.UserId);
			DP.Add("Status", _sYLOGInsertIN.Status);
			DP.Add("Action", _sYLOGInsertIN.Action);
			DP.Add("Exception", _sYLOGInsertIN.Exception);
			DP.Add("FullName", _sYLOGInsertIN.FullName);
			DP.Add("IPAddress", _sYLOGInsertIN.IPAddress);
			DP.Add("MACAddress", _sYLOGInsertIN.MACAddress);
			DP.Add("Description", _sYLOGInsertIN.Description);
			DP.Add("CreatedDate", _sYLOGInsertIN.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_LOG_Insert", DP));
		}
	}

	public class SYLOGInsertIN
	{
		public long? UserId { get; set; }
		public byte? Status { get; set; }
		public string Action { get; set; }
		public string Exception { get; set; }
		public string FullName { get; set; }
		public string IPAddress { get; set; }
		public string MACAddress { get; set; }
		public string Description { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
