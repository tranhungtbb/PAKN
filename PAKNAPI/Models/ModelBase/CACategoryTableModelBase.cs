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
	public class CAGroupWordOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? RowNumber; // int, null
	}

	public class CAGroupWord
	{
		private SQLCon _sQLCon;

		public CAGroupWord(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAGroupWord()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<CAGroupWord> CAGroupWordGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAGroupWord>("CA_GroupWordGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAGroupWord>> CAGroupWordGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAGroupWord>("CA_GroupWordGetAll", DP)).ToList();
		}

		public async Task<List<CAGroupWordOnPage>> CAGroupWordGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAGroupWordOnPage>("CA_GroupWordGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAGroupWordInsert(CAGroupWord _cAGroupWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAGroupWord.Name);
			DP.Add("IsActived", _cAGroupWord.IsActived);
			DP.Add("IsDeleted", _cAGroupWord.IsDeleted);
			DP.Add("Description", _cAGroupWord.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_GroupWordInsert", DP));
		}

		public async Task<int> CAGroupWordUpdate(CAGroupWord _cAGroupWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAGroupWord.Id);
			DP.Add("Name", _cAGroupWord.Name);
			DP.Add("IsActived", _cAGroupWord.IsActived);
			DP.Add("IsDeleted", _cAGroupWord.IsDeleted);
			DP.Add("Description", _cAGroupWord.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_GroupWordUpdate", DP));
		}

		public async Task<int> CAGroupWordDelete(CAGroupWord _cAGroupWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAGroupWord.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_GroupWordDelete", DP));
		}

		public async Task<int> CAGroupWordDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_GroupWordDeleteAll", DP));
		}
	}
}
