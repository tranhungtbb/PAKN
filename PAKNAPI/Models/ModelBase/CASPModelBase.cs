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

	public class CAAdministrativeUnitGetByNameLevel
	{
		private SQLCon _sQLCon;

		public CAAdministrativeUnitGetByNameLevel(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAAdministrativeUnitGetByNameLevel()
		{
		}

		public int? Id { get; set; }
		public string Name { get; set; }
		public int? Level { get; set; }

		public async Task<List<CAAdministrativeUnitGetByNameLevel>> CAAdministrativeUnitsGetByNameDAO(string? Name, int Level, int? ParentId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", Name);
			DP.Add("Level", Level);
			DP.Add("ParentId", ParentId);

			return (await _sQLCon.ExecuteListDapperAsync<CAAdministrativeUnitGetByNameLevel>("[CA_AdministrativeUnitGetByName-Level]", DP)).ToList();
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

		public async Task<List<CAFieldDAMGetDropdown>> CAUnitDAMGetDropdownDAO(string Keyword)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Keyword", Keyword);
			return (await _sQLCon.ExecuteListDapperAsync<CAFieldDAMGetDropdown>("CA_UnitDAMGetDropdown", DP)).ToList();
		}
		public async Task<int?> CAUnitDAMDeleteAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();
			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitDAMDeleteAll", DP));
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

	public class CAFieldDAMDeleteAll
	{
		private SQLCon _sQLCon;

		public CAFieldDAMDeleteAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldDAMDeleteAll()
		{
		}

		public async Task<int?> CAFieldDAMDeleteAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDAMDeleteAll", DP);
		}
	}

	public class CAUnitDAMInsert
	{
		private SQLCon _sQLCon;
		public int UnitId { get; set; }
		public string Name { get; set; }
		public CAUnitDAMInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitDAMInsert()
		{
		}

		public async Task<int?> CAUnitDAMInsertDAO(CAUnitDAMInsert cAUnitDAMInsert)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitId", cAUnitDAMInsert.UnitId);
			DP.Add("Name", cAUnitDAMInsert.Name);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_UnitDAMInsert", DP);
		}
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
        public async Task<List<CAFieldGetDropdown>> CAFieldGetDropdownForCreateUnitDAO()
        {
            DynamicParameters DP = new DynamicParameters();

            return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetDropdown>("[CA_FieldGetDropdownForCreateUnit]", DP)).ToList();
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

	public class CAGetAllByProvinceId
	{
		private SQLCon _sQLCon;

		public CAGetAllByProvinceId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAGetAllByProvinceId()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int DistrictId { get; set; }
		public int Level { get; set; }

		public async Task<List<CAGetAllByProvinceId>> GetAll(short? ProvinceId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ProvinceId", ProvinceId);

			return (await _sQLCon.ExecuteListDapperAsync<CAGetAllByProvinceId>("CA_AdministrativeUnits_GetAllByProvinceId", DP)).ToList();
		}
	}
}
