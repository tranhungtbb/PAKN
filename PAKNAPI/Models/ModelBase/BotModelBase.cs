using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.ModelBase
{
    public class BOTAnonymousUser
    {
		private SQLCon _sQLCon;

		public BOTAnonymousUser(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BOTAnonymousUser()
		{
		}
		public int Id { get; set; }
		public string UserName { get; set; }

		public async Task<decimal?> BOTAnonymousUserInsertDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteScalarDapperAsync<decimal?>("BOT_AnonymousUserInsert", DP));
		}
		public async Task<BOTAnonymousUser> BOTAnonymousUserGetByUserName(string UserName)
		{
            try
            {
				DynamicParameters DP = new DynamicParameters();
				DP.Add("UserName", UserName);
				return (await _sQLCon.ExecuteListDapperAsync<BOTAnonymousUser>("BOT_AnonymousUserGetByUserName", DP)).FirstOrDefault();
			}
            catch (Exception ex)
            {

				return null;
            }
			
		}
	}
	public class BOTRoomGetAllOnPage
	{
		private SQLCon _sQLCon;

		public BOTRoomGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BOTRoomGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }

		public int? AnonymousId { get; set; }
		public string Name { get; set; }

		public int? Type { get; set; }
		public DateTime? CreatedDate { get; set; }

		public async Task<List<BOTRoomGetAllOnPage>> SYUserGetByRoleIdAllOnPageDAO(int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			//DP.Add("PageSize", PageSize);
			//DP.Add("PageIndex", PageIndex);
			

			return (await _sQLCon.ExecuteListDapperAsync<BOTRoomGetAllOnPage>("BOT_RoomGetAllOnPage", DP)).ToList();
		}
	}


	public class BOTRoom
	{
		private SQLCon _sQLCon;

		public BOTRoom(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BOTRoom()
		{
		}

		public BOTRoom(string Name,int AnonymousId, int Type) {
			this.Name = Name;
			this.Type = Type;
			this.AnonymousId = AnonymousId;
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public int Type { get; set; }

		public int AnonymousId { get; set; }
		public async Task<decimal?> BOTRoomInsertDAO(BOTRoom _bOTRoom)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _bOTRoom.Name);
			DP.Add("AnonymousId", _bOTRoom.AnonymousId);
			DP.Add("Type", _bOTRoom.Type);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("BOT_RoomInsert", DP);
		}
		public async Task<BOTRoom> BOTRoomGetByName(string roomName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", roomName);

			return (await _sQLCon.ExecuteListDapperAsync<BOTRoom>("BOT_RoomGetByName", DP)).FirstOrDefault();
		}
		public async Task<BOTRoom> BOTRoomGetById(int roomId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", roomId);

			return (await _sQLCon.ExecuteListDapperAsync<BOTRoom>("BOT_RoomGetById", DP)).FirstOrDefault();
		}
	}


	public class BOTRoomUserLink
	{
		private SQLCon _sQLCon;

		public BOTRoomUserLink(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BOTRoomUserLink()
		{
		}

		public int Id { get; set; }
		public int RoomId { get; set; }
		
		public int UserId { get; set; }
		

		public async Task<int> BOTRoomUserLinkInsertDAO(BOTRoomUserLink _bOTRoomUserLink)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", _bOTRoomUserLink.RoomId);
			DP.Add("UserId", _bOTRoomUserLink.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("[BOT_RoomUserLinkInsert]", DP));
		}

		public async Task<int> BOTCheckUserExistInRoom(int userId,int roomId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", userId);
			DP.Add("RoomId", roomId);

			return (await _sQLCon.ExecuteListDapperAsync<int>("BOT_CheckUserExistInRoom", DP)).FirstOrDefault();
		}

	}


	public class BOTMessage
	{
		private SQLCon _sQLCon;

		public BOTMessage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BOTMessage()
		{
		}

		public int Id { get; set; }
		public string MessageContent { get; set; }
		public int RoomId { get; set; }
		public int FromUserId { get; set; }
		public DateTime DateSend { get; set; }


		public async Task<int> BOTMessageInsertDAO(string MessageContent,
			 int FromUserId,
			 int RoomId,
			  DateTime DateSend)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", RoomId);
			DP.Add("FromUserId", FromUserId);
			DP.Add("MessageContent", MessageContent);
			DP.Add("DateSend", DateSend);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BOT_MessageInsert", DP));
		}

		public async Task<List<BOTMessage>> BOTMessageGetAllDAO(BOTMessage _bOTMessage)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", _bOTMessage.RoomId);

			return (await _sQLCon.ExecuteListDapperAsync<BOTMessage>("BOT_MessageGetAll", DP)).ToList();
		}
	}
	public class ChatbotGetByRoomId
	{
		private SQLCon _sQLCon;
		private readonly IAppSetting _appSetting;

		public ChatbotGetByRoomId(IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public ChatbotGetByRoomId()
		{
		}
		public int Id { get; set; }
		public string MessageContent { get; set; }
		public int RoomId { get; set; }
		public int FromUserId { get; set; }
		public DateTime DateSend { get; set; }
		public int? RowNumber { get; set; }

		public async Task<List<ChatbotGetByRoomId>> ChatbotGetByRoomIdDAO(int RoomId, int PageIndex, int PageSize)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", RoomId);
			DP.Add("PageIndex", PageIndex);
			DP.Add("PageSize", PageSize);
			return (await _sQLCon.ExecuteListDapperAsync<ChatbotGetByRoomId>("BOT_Message_GetByRoomId", DP)).ToList();
		}
	}
}
