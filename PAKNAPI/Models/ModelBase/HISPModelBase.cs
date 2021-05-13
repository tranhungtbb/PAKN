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
	public class HISSMSDeleteBySMSId
	{
		private SQLCon _sQLCon;

		public HISSMSDeleteBySMSId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISSMSDeleteBySMSId()
		{
		}

		public async Task<int> HISSMSDeleteBySMSIdDAO(HISSMSDeleteBySMSIdIN _hISSMSDeleteBySMSIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _hISSMSDeleteBySMSIdIN.SMSId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_SMSDeleteBySMSId", DP));
		}
	}

	public class HISSMSDeleteBySMSIdIN
	{
		public int? SMSId { get; set; }
	}

	public class HISSMSGetBySMSIdOnPage
	{
		private SQLCon _sQLCon;

		public HISSMSGetBySMSIdOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISSMSGetBySMSIdOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Content { get; set; }
		public byte Status { get; set; }
		public DateTime CreatedDate { get; set; }
		public long CreatedBy { get; set; }
		public string CreateName { get; set; }

		public async Task<List<HISSMSGetBySMSIdOnPage>> HISSMSGetBySMSIdOnPageDAO(int? PageSize, int? PageIndex, int? SMSId, string Content, string UserName, DateTime? CreateDate, int? Status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("SMSId", SMSId);
			DP.Add("Content", Content);
			DP.Add("UserName", UserName);
			DP.Add("CreateDate", CreateDate);
			DP.Add("Status", Status);

			return (await _sQLCon.ExecuteListDapperAsync<HISSMSGetBySMSIdOnPage>("HIS_SMSGetBySMSIdOnPage", DP)).ToList();
		}
	}

	public class HISSMSInsert
	{
		private SQLCon _sQLCon;

		public HISSMSInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HISSMSInsert()
		{
		}

		public async Task<int> HISSMSInsertDAO(HISSMSInsertIN _hISSMSInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ObjectId", _hISSMSInsertIN.ObjectId);
			DP.Add("Type", _hISSMSInsertIN.Type);
			DP.Add("Content", _hISSMSInsertIN.Content);
			DP.Add("Status", _hISSMSInsertIN.Status);
			DP.Add("CreatedBy", _hISSMSInsertIN.CreatedBy);
			DP.Add("CreatedName", _hISSMSInsertIN.CreatedName);
			DP.Add("CreatedDate", _hISSMSInsertIN.CreatedDate);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("HIS_SMSInsert", DP));
		}
	}

	public class HISSMSInsertIN
	{
		public string ObjectId { get; set; }
		public int? Type { get; set; }
		public string Content { get; set; }
		public int? Status { get; set; }
		public int? CreatedBy { get; set; }
		public string CreatedName { get; set; }
		public DateTime? CreatedDate { get; set; }
	}
}
