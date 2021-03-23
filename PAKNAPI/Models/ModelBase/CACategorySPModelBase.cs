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

		public async Task<List<CAPositionGetAllOnPage>> CAPositionGetAllOnPageDAO(int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

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
}
