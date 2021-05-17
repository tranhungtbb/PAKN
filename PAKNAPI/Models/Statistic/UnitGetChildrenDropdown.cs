using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Statistic
{
    public class UnitGetChildrenDropdown
    {

		private SQLCon _sQLCon;

		public UnitGetChildrenDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public UnitGetChildrenDropdown()
		{
		}

		public int UnitId { get; set; }
		public string UnitName { get; set; }
		public int Levers { get; set; }
		public int ParentId { get; set; }

		public async Task<List<UnitGetChildrenDropdown>> UnitGetChildrenDropdownDAO(int? id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", id);

			return (await _sQLCon.ExecuteListDapperAsync<UnitGetChildrenDropdown>("[SY_UnitGetChildrenDropdown]", DP)).ToList();
		}
	}
}
