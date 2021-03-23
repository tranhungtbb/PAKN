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
	public class CAFieldDelete
	{
		private SQLCon _sQLCon;

		public CAFieldDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldDelete()
		{
		}


		public async Task<int> CAFieldDeleteDAO(CAFieldDeleteIN _cAFieldDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAFieldDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDelete", DP));
		}
	}

	public class CAFieldDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAFieldGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAFieldGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldGetAllOnPage()
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

		public async Task<List<CAFieldGetAllOnPage>> CAFieldGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Code, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Code", Code);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetAllOnPage>("CA_FieldGetAllOnPage", DP)).ToList();
		}
	}

	public class CAFieldGetByID
	{
		private SQLCon _sQLCon;

		public CAFieldGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }

		public async Task<List<CAFieldGetByID>> CAFieldGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetByID>("CA_FieldGetByID", DP)).ToList();
		}
	}

	public class CAFieldInsert
	{
		private SQLCon _sQLCon;

		public CAFieldInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldInsert()
		{
		}


		public async Task<int> CAFieldInsertDAO(CAFieldInsertIN _cAFieldInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAFieldInsertIN.Name);
			DP.Add("Code", _cAFieldInsertIN.Code);
			DP.Add("IsActived", _cAFieldInsertIN.IsActived);
			DP.Add("IsDeleted", _cAFieldInsertIN.IsDeleted);
			DP.Add("Description", _cAFieldInsertIN.Description);
			DP.Add("OrderNumber", _cAFieldInsertIN.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldInsert", DP));
		}
	}

	public class CAFieldInsertIN
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }
	}

	public class CAFieldUpdate
	{
		private SQLCon _sQLCon;

		public CAFieldUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldUpdate()
		{
		}


		public async Task<int> CAFieldUpdateDAO(CAFieldUpdateIN _cAFieldUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAFieldUpdateIN.Id);
			DP.Add("Name", _cAFieldUpdateIN.Name);
			DP.Add("Code", _cAFieldUpdateIN.Code);
			DP.Add("IsActived", _cAFieldUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAFieldUpdateIN.IsDeleted);
			DP.Add("Description", _cAFieldUpdateIN.Description);
			DP.Add("OrderNumber", _cAFieldUpdateIN.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldUpdate", DP));
		}
	}

	public class CAFieldUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }
	}

	public class CAPositionDelete
	{
		private SQLCon _sQLCon;

		public CAPositionDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionDelete()
		{
		}


		public async Task<int> CAPositionDeleteDAO(CAPositionDeleteIN _cAPositionDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPositionDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionDelete", DP));
		}
	}

	public class CAPositionDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAPositionGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAPositionGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionGetAllOnPage()
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

		public async Task<List<CAPositionGetAllOnPage>> CAPositionGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Code, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Code", Code);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAPositionGetAllOnPage>("CA_PositionGetAllOnPage", DP)).ToList();
		}
	}

	public class CAPositionGetByID
	{
		private SQLCon _sQLCon;

		public CAPositionGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }

		public async Task<List<CAPositionGetByID>> CAPositionGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAPositionGetByID>("CA_PositionGetByID", DP)).ToList();
		}
	}

	public class CAPositionInsert
	{
		private SQLCon _sQLCon;

		public CAPositionInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionInsert()
		{
		}


		public async Task<int> CAPositionInsertDAO(CAPositionInsertIN _cAPositionInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAPositionInsertIN.Name);
			DP.Add("Code", _cAPositionInsertIN.Code);
			DP.Add("IsActived", _cAPositionInsertIN.IsActived);
			DP.Add("IsDeleted", _cAPositionInsertIN.IsDeleted);
			DP.Add("Description", _cAPositionInsertIN.Description);
			DP.Add("OrderNumber", _cAPositionInsertIN.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionInsert", DP));
		}
	}

	public class CAPositionInsertIN
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }
	}

	public class CAPositionUpdate
	{
		private SQLCon _sQLCon;

		public CAPositionUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionUpdate()
		{
		}


		public async Task<int> CAPositionUpdateDAO(CAPositionUpdateIN _cAPositionUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPositionUpdateIN.Id);
			DP.Add("Name", _cAPositionUpdateIN.Name);
			DP.Add("Code", _cAPositionUpdateIN.Code);
			DP.Add("IsActived", _cAPositionUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAPositionUpdateIN.IsDeleted);
			DP.Add("Description", _cAPositionUpdateIN.Description);
			DP.Add("OrderNumber", _cAPositionUpdateIN.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionUpdate", DP));
		}
	}

	public class CAPositionUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
		public int? OrderNumber { get; set; }
	}

	public class CAUnitDelete
	{
		private SQLCon _sQLCon;

		public CAUnitDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitDelete()
		{
		}


		public async Task<int> CAUnitDeleteDAO(CAUnitDeleteIN _cAUnitDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAUnitDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitDelete", DP));
		}
	}

	public class CAUnitDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAUnitGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAUnitGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitGetAllOnPage()
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

		public async Task<List<CAUnitGetAllOnPage>> CAUnitGetAllOnPageDAO(int? PageSize, int? PageIndex, string Sort, int? ParentId, byte? Level, string SearchName, string SearchPhone, string SearchEmail, string SearchAddress, bool? SearchActive)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Sort", Sort);
			DP.Add("ParentId", ParentId);
			DP.Add("Level", Level);
			DP.Add("SearchName", SearchName);
			DP.Add("SearchPhone", SearchPhone);
			DP.Add("SearchEmail", SearchEmail);
			DP.Add("SearchAddress", SearchAddress);
			DP.Add("SearchActive", SearchActive);

			return (await _sQLCon.ExecuteListDapperAsync<CAUnitGetAllOnPage>("CA_UnitGetAllOnPage", DP)).ToList();
		}
	}

	public class CAUnitGetByID
	{
		private SQLCon _sQLCon;

		public CAUnitGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitGetByID()
		{
		}

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

		public async Task<List<CAUnitGetByID>> CAUnitGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAUnitGetByID>("CA_UnitGetByID", DP)).ToList();
		}
	}

	public class CAUnitInsert
	{
		private SQLCon _sQLCon;

		public CAUnitInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitInsert()
		{
		}


		public async Task<int> CAUnitInsertDAO(CAUnitInsertIN _cAUnitInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAUnitInsertIN.Name);
			DP.Add("UnitLevel", _cAUnitInsertIN.UnitLevel);
			DP.Add("ParentId", _cAUnitInsertIN.ParentId);
			DP.Add("Description", _cAUnitInsertIN.Description);
			DP.Add("Email", _cAUnitInsertIN.Email);
			DP.Add("Phone", _cAUnitInsertIN.Phone);
			DP.Add("Address", _cAUnitInsertIN.Address);
			DP.Add("IsActived", _cAUnitInsertIN.IsActived);
			DP.Add("IsDeleted", _cAUnitInsertIN.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitInsert", DP));
		}
	}

	public class CAUnitInsertIN
	{
		public string Name { get; set; }
		public byte? UnitLevel { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
	}

	public class CAUnitUpdate
	{
		private SQLCon _sQLCon;

		public CAUnitUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitUpdate()
		{
		}


		public async Task<int> CAUnitUpdateDAO(CAUnitUpdateIN _cAUnitUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAUnitUpdateIN.Id);
			DP.Add("Name", _cAUnitUpdateIN.Name);
			DP.Add("UnitLevel", _cAUnitUpdateIN.UnitLevel);
			DP.Add("ParentId", _cAUnitUpdateIN.ParentId);
			DP.Add("Description", _cAUnitUpdateIN.Description);
			DP.Add("Email", _cAUnitUpdateIN.Email);
			DP.Add("Phone", _cAUnitUpdateIN.Phone);
			DP.Add("Address", _cAUnitUpdateIN.Address);
			DP.Add("IsActived", _cAUnitUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAUnitUpdateIN.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitUpdate", DP));
		}
	}

	public class CAUnitUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public byte? UnitLevel { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
	}
}
