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
	public class SYCAPositionGetOnPage
	{
		private SQLCon _sQLCon;

		public SYCAPositionGetOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYCAPositionGetOnPage()
		{
		}

		public int? RowNumber;
		public int Id;
		public string Name;
		public string Code;
		public bool IsActived;
		public bool IsDeleted;
		public string Description;
		public int? OrderNumber;

		public async Task<List<SYCAPositionGetOnPage>> SYCAPositionGetOnPageDAO(int? PageSize, int? PageIndex, string Search)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Search", Search);

			return (await _sQLCon.ExecuteListDapperAsync<SYCAPositionGetOnPage>("SY_CA_Position_GetOnPage", DP)).ToList();
		}
	}

	public class SYCAUnitGetOnPage
	{
		private SQLCon _sQLCon;

		public SYCAUnitGetOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYCAUnitGetOnPage()
		{
		}

		public int? RowNumber;
		public int Id;
		public string Name;
		public byte UnitLevel;
		public bool IsActived;
		public bool IsDeleted;
		public int? ParentId;
		public string Description;
		public string Email;
		public string Phone;
		public string Address;

		public async Task<List<SYCAUnitGetOnPage>> SYCAUnitGetOnPageDAO(int? PageSize, int? PageIndex, string Search)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Search", Search);

			return (await _sQLCon.ExecuteListDapperAsync<SYCAUnitGetOnPage>("SY_CA_Unit_GetOnPage", DP)).ToList();
		}
	}
}
