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

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }

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

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public byte UnitLevel { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }

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
