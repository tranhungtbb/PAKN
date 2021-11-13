﻿using Dapper;
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

		public async Task<int> BOTAnonymousUserInsertDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BOT_AnonymousUserInsert", DP));
		}
		public async Task<BOTAnonymousUser> BOTAnonymousUserGetByUserName(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteListDapperAsync<BOTAnonymousUser>("BOT_AnonymousUserGetByUserName", DP)).FirstOrDefault();
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

		public BOTRoom(string Name, int Type) {
			this.Name = Name;
			this.Type = Type;
		}
		public int Id { get; set; }
		public string Name { get; set; }
		public int Type { get; set; }

		public async Task<int> BOTRoomInsertDAO(BOTRoom _bOTRoom)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", _bOTRoom.Name);
			DP.Add("Type", _bOTRoom.Type);

			return await _sQLCon.ExecuteNonQueryDapperAsync("BOT_RoomInsert", DP);
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
		public int AnonymousId { get; set; }
		public int UserId { get; set; }
		

		public async Task<int> BOTRoomUserLinkInsertDAO(BOTRoomUserLink _bOTRoomUserLink)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", _bOTRoomUserLink.RoomId);
			DP.Add("AnonymousId", _bOTRoomUserLink.AnonymousId);
			DP.Add("UserId", _bOTRoomUserLink.UserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BOT_RoomUserLinkInsert", DP));
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


		public async Task<int> BOTMessageInsertDAO(BOTMessage _bOTMessage)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", _bOTMessage.RoomId);
			DP.Add("FromUserId", _bOTMessage.FromUserId);
			DP.Add("MessageContent", _bOTMessage.MessageContent);
			DP.Add("DateSend", _bOTMessage.DateSend);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("BOT_MessageInsert", DP));
		}

		public async Task<List<BOTMessage>> BOTMessageGetAllDAO(BOTMessage _bOTMessage)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", _bOTMessage.RoomId);
			DP.Add("FromUserId", _bOTMessage.FromUserId);
			DP.Add("MessageContent", _bOTMessage.MessageContent);
			DP.Add("DateSend", _bOTMessage.DateSend);

			return (await _sQLCon.ExecuteListDapperAsync<BOTMessage>("BOT_MessageInsert", DP)).ToList();
		}
	}




}
