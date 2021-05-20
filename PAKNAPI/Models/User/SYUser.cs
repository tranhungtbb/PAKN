using Dapper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.User
{
	public class SYUserGetAllOnPageList
	{
		private SQLCon _sQLCon;

		public SYUserGetAllOnPageList(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetAllOnPageList()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }

		public string UnitName { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }

		public string PositionName { get; set; }

		public async Task<List<SYUserGetAllOnPageList>> SYUserGetAllOnPageDAO(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActived, int? UnitId, int? TypeId, int? PositionId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("UserName", UserName);
			DP.Add("FullName", FullName);
			DP.Add("Phone", Phone);
			DP.Add("IsActived", IsActived);
			DP.Add("UnitId", UnitId);
			DP.Add("TypeId", TypeId);
			DP.Add("PositionId", PositionId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetAllOnPageList>("[SY_UserGetAllOnPageList]", DP)).ToList();
		}
	}
	public class SYUserDropList {
		public string text { get; set; }
		public long value { get; set; }
		public List<SYUserDropList> children { get; set; }

		public SYUserDropList() { }
		public SYUserDropList(string text, long value) {
			this.text = text;
			this.value = value;
			this.children = null;
		}

		public SYUserDropList(string text, long value, IList<SYUserDropList> chil) {
			this.text = text;
			this.value = value;
			this.children = (List<SYUserDropList>)chil;
		}
	}

	
}
