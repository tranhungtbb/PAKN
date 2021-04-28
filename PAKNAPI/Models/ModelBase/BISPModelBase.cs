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
	public class BIBusinessCheckExists
	{
		private SQLCon _sQLCon;

		public BIBusinessCheckExists(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessCheckExists()
		{
		}

		public bool? Exists { get; set; }
		public string Value { get; set; }

		public async Task<List<BIBusinessCheckExists>> BIBusinessCheckExistsDAO(string Field, string Value)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Field", Field);
			DP.Add("Value", Value);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessCheckExists>("BI_Business_CheckExists", DP)).ToList();
		}
	}

	public class BIBusinessGetDropdown
	{
		private SQLCon _sQLCon;

		public BIBusinessGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetDropdown()
		{
		}

		public long Value { get; set; }
		public string Text { get; set; }

		public async Task<List<BIBusinessGetDropdown>> BIBusinessGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetDropdown>("BI_BusinessGetDropdown", DP)).ToList();
		}
	}

	public class BIBusinessGetRepresentativeById
	{
		private SQLCon _sQLCon;

		public BIBusinessGetRepresentativeById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BIBusinessGetRepresentativeById()
		{
		}

		public int? RowNumber { get; set; }
		public int? WardsId { get; set; }
		public int? DistrictId { get; set; }
		public string RepresentativeName { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public long Id { get; set; }
		public DateTime RepresentativeBirthDay { get; set; }
		public int? ProvinceId { get; set; }
		public int? Status { get; set; }
		public bool? RepresentativeGender { get; set; }
		public DateTime? DateOfIssue { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Representative { get; set; }
		public string IDCard { get; set; }
		public string Place { get; set; }
		public string NativePlace { get; set; }
		public string PermanentPlace { get; set; }
		public string Nation { get; set; }

		public async Task<List<BIBusinessGetRepresentativeById>> BIBusinessGetRepresentativeByIdDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<BIBusinessGetRepresentativeById>("BI_BusinessGetRepresentativeById", DP)).ToList();
		}
	}
}
