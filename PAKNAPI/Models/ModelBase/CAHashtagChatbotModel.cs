using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
	public class CAHashtagChatbotListPage
	{
		private SQLCon _sQLCon;

		public CAHashtagChatbotListPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagChatbotListPage()
		{
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public int? RowNumber; // int, null
		public int? QuantityUser { get; set; }

		public async Task<List<CAHashtagChatbotListPage>> CAHashtagChatbotGetAllOnPage()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagChatbotListPage>("CA_HashtagGetAllOnPage", DP)).ToList();
		}
	}

	public class CAHashtagChatbotInsert
	{
		private SQLCon _sQLCon;

		public CAHashtagChatbotInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagChatbotInsert()
		{
		}

		public async Task<long?> CAHashtagChatbotInsertDAO(CAHashtagChatbotInsertIN _cAHashtagInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAHashtagInsertIN.Name);
			DP.Add("IsActived", _cAHashtagInsertIN.IsActived);

			return await _sQLCon.ExecuteScalarDapperAsync<long?>("CA_HashtagChatbotInsert", DP);
		}
	}



	/// <example>
	/// {
	///		"Name": "demo name",
	///		"IsActived" : true
	/// }
	/// </example>
	public class CAHashtagChatbotInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
	}

	public class CAHashtagChatbotUpdate
	{
		private SQLCon _sQLCon;

		public CAHashtagChatbotUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagChatbotUpdate()
		{
		}

		public async Task<long?> CAHashtagChatbotUpdateDAO(CAHashtagChatbotUpdateIN _cAHashtagUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtagUpdateIN.Id);
			DP.Add("Name", _cAHashtagUpdateIN.Name);
			DP.Add("IsActived", _cAHashtagUpdateIN.IsActived);

			return await _sQLCon.ExecuteScalarDapperAsync<long?>("CA_HashtagChatbotUpdate", DP);
		}
	}

	/// <example>
	/// { 
	///		"Id": 2067,
	///		"Name": "demo name",
	///		"IsActived" : true
	/// }
	/// </example>
	public class CAHashtagChatbotUpdateIN
	{
		public long? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
	}

	public class CAHashtagChatbotGetByID
	{
		private SQLCon _sQLCon;

		public CAHashtagChatbotGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagChatbotGetByID()
		{
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }

		public async Task<List<CAHashtagChatbotGetByID>> CAHashtagChatbotGetByIDDAO(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagChatbotGetByID>("CA_HashtagChatbotGetByID", DP)).ToList();

		}
	}
	public class CAHashtagChatbotGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAHashtagChatbotGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagChatbotGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public int? QuantityUser { get; set; }

		public async Task<List<CAHashtagChatbotGetAllOnPage>> CAHashtagChatbotGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, int? QuantityUser, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("QuantityUser", QuantityUser);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagChatbotGetAllOnPage>("CA_HashtagChatbotGetAllOnPage", DP)).ToList();
		}
	}
	public class CAHashtagChatbot
	{
		private SQLCon _sQLCon;

		public CAHashtagChatbot(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagChatbot()
		{
		}

		public long Id { get; set; }
		public string Name { get; set; }

		public bool IsActived { get; set; }

		public async Task<List<CAHashtagChatbot>> CAHashtagChatbotGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagChatbot>("CA_HashtagGetAll", DP)).ToList();
		}

		public async Task<int> CAHashtagChatbotUpdate(CAHashtagChatbotUpdateIN _CAHashtagChatbot)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _CAHashtagChatbot.Id);
			DP.Add("Name", _CAHashtagChatbot.Name);
			DP.Add("IsActived", _CAHashtagChatbot.IsActived);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagChatbotUpdate", DP));
		}


		public async Task<int> CAHashtagChatbotDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagChatbotDeleteAll", DP));
		}

		public async Task<int> CAHashtagChatbotCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_HashtagChatbotCount", DP));
		}

		public async Task<int> CAHashtagChatbotDelete(HashtagChatbotDelete _cAHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtag.id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagChatbotDelete", DP));
		}
	}
	/// <example>
	/// { 
	///		"Id": 2067
	/// }
	/// </example>
	public class HashtagChatbotDelete
	{
		public int? id { get; set; }
	}
}
