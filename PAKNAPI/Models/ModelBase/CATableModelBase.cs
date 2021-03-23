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
	public class CAFieldOnPage
	{
		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAField
	{
		private SQLCon _sQLCon;

		public CAField(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAField()
		{
		}

		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAField> CAFieldGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAField>("CA_FieldGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAField>> CAFieldGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAField>("CA_FieldGetAll", DP)).ToList();
		}

		public async Task<List<CAFieldOnPage>> CAFieldGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAFieldOnPage>("CA_FieldGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAFieldInsert(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAField.Name);
			DP.Add("Code", _cAField.Code);
			DP.Add("IsActived", _cAField.IsActived);
			DP.Add("IsDeleted", _cAField.IsDeleted);
			DP.Add("Description", _cAField.Description);
			DP.Add("OrderNumber", _cAField.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldInsert", DP));
		}

		public async Task<int> CAFieldUpdate(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAField.Id);
			DP.Add("Name", _cAField.Name);
			DP.Add("Code", _cAField.Code);
			DP.Add("IsActived", _cAField.IsActived);
			DP.Add("IsDeleted", _cAField.IsDeleted);
			DP.Add("Description", _cAField.Description);
			DP.Add("OrderNumber", _cAField.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldUpdate", DP));
		}

		public async Task<int> CAFieldDelete(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAField.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDelete", DP));
		}

		public async Task<int> CAFieldDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDeleteAll", DP));
		}

		public async Task<int> CAFieldCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_FieldCount", DP));
		}
	}

	public class CAPositionOnPage
	{
		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAPosition
	{
		private SQLCon _sQLCon;

		public CAPosition(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPosition()
		{
		}

		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAPosition> CAPositionGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAPosition>("CA_PositionGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAPosition>> CAPositionGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAPosition>("CA_PositionGetAll", DP)).ToList();
		}

		public async Task<List<CAPositionOnPage>> CAPositionGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAPositionOnPage>("CA_PositionGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAPositionInsert(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAPosition.Name);
			DP.Add("Code", _cAPosition.Code);
			DP.Add("IsActived", _cAPosition.IsActived);
			DP.Add("IsDeleted", _cAPosition.IsDeleted);
			DP.Add("Description", _cAPosition.Description);
			DP.Add("OrderNumber", _cAPosition.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionInsert", DP));
		}

		public async Task<int> CAPositionUpdate(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPosition.Id);
			DP.Add("Name", _cAPosition.Name);
			DP.Add("Code", _cAPosition.Code);
			DP.Add("IsActived", _cAPosition.IsActived);
			DP.Add("IsDeleted", _cAPosition.IsDeleted);
			DP.Add("Description", _cAPosition.Description);
			DP.Add("OrderNumber", _cAPosition.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionUpdate", DP));
		}

		public async Task<int> CAPositionDelete(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPosition.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionDelete", DP));
		}

		public async Task<int> CAPositionDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionDeleteAll", DP));
		}

		public async Task<int> CAPositionCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_PositionCount", DP));
		}
	}
}
