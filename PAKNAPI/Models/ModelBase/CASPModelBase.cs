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
	public class CAAdministrativeUnitsGetDropDown
	{
		private SQLCon _sQLCon;

		public CAAdministrativeUnitsGetDropDown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAAdministrativeUnitsGetDropDown()
		{
		}

		public short? Id { get; set; }
		public string Name { get; set; }
		public byte? Levels { get; set; }

		public int? ParentId { get; set; }

		public async Task<List<CAAdministrativeUnitsGetDropDown>> CAAdministrativeUnitsGetDropDownDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAAdministrativeUnitsGetDropDown>("CA_AdministrativeUnitsGetDropDown", DP)).ToList();
		}
	}

	public class AdministrativeUnitGetAllById
	{
		private SQLCon _sQLCon;

		public AdministrativeUnitGetAllById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public AdministrativeUnitGetAllById()
		{
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public byte? Levels { get; set; }

		public async Task<List<AdministrativeUnitGetAllById>> AdministrativeUnitsGetDropDownDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<AdministrativeUnitGetAllById>("[CA_AdministrativeUnitGetAllById]", DP)).ToList();
		}
	}

	public class CADistrictGetAll
	{
		private SQLCon _sQLCon;

		public CADistrictGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADistrictGetAll()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		public async Task<List<CADistrictGetAll>> CADistrictGetAllDAO(byte? ProvinceId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ProvinceId", ProvinceId);

			return (await _sQLCon.ExecuteListDapperAsync<CADistrictGetAll>("CA_DistrictGetAll", DP)).ToList();
		}
	}

	public class CAFieldDAMGetDropdown
	{
		private SQLCon _sQLCon;

		public CAFieldDAMGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldDAMGetDropdown()
		{
		}

		public int? Value { get; set; }
		public string Text { get; set; }
		public int? ParentId { get; set; }

		public async Task<List<CAFieldDAMGetDropdown>> CAFieldDAMGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldDAMGetDropdown>("CA_FieldDAMGetDropdown", DP)).ToList();
		}
	}

	public class CAFieldDAMInsert
	{
		private SQLCon _sQLCon;

		public CAFieldDAMInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldDAMInsert()
		{
		}

		public async Task<int?> CAFieldDAMInsertDAO(CAFieldDAMInsertIN _cAFieldDAMInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FieldDAMId", _cAFieldDAMInsertIN.FieldDAMId);
			DP.Add("Name", _cAFieldDAMInsertIN.Name);
			DP.Add("ParentId", _cAFieldDAMInsertIN.ParentId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_FieldDAMInsert", DP);
		}
	}

	public class CAFieldDAMInsertIN
	{
		public int? FieldDAMId { get; set; }
		public string Name { get; set; }
		public int? ParentId { get; set; }
	}

	public class CAFieldGetDropdown
	{
		private SQLCon _sQLCon;

		public CAFieldGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }

		public async Task<List<CAFieldGetDropdown>> CAFieldGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetDropdown>("CA_FieldGetDropdown", DP)).ToList();
		}
	}

	public class CAHashtagGetAll
	{
		private SQLCon _sQLCon;

		public CAHashtagGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagGetAll()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }

		public async Task<List<CAHashtagGetAll>> CAHashtagGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagGetAll>("CA_HashtagGetAll", DP)).ToList();
		}
	}

	public class CAHashtagGetDropdown
	{
		private SQLCon _sQLCon;

		public CAHashtagGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }

		public async Task<List<CAHashtagGetDropdown>> CAHashtagGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagGetDropdown>("CA_HashtagGetDropdown", DP)).ToList();
		}
	}

	public class CAProvinceGetAll
	{
		private SQLCon _sQLCon;

		public CAProvinceGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAProvinceGetAll()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		public async Task<List<CAProvinceGetAll>> CAProvinceGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAProvinceGetAll>("CA_ProvinceGetAll", DP)).ToList();
		}
	}

	public class CAVillageGetAll
	{
		private SQLCon _sQLCon;

		public CAVillageGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAVillageGetAll()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		public async Task<List<CAVillageGetAll>> CAVillageGetAllDAO(short? ProvinceId, short? DistrictId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ProvinceId", ProvinceId);
			DP.Add("DistrictId", DistrictId);

			return (await _sQLCon.ExecuteListDapperAsync<CAVillageGetAll>("CA_VillageGetAll", DP)).ToList();
		}
	}
}
