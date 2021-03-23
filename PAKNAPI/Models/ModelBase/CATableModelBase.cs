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
	public class CAPositionOnPage
	{
		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
	}
}
