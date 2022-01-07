using Dapper;
using PAKNAPI.Common;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

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
		public string FromAvatar { get; set; }
		public string FromFullName { get; set; }
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

	public class BOTRoomGetAllByStatus
	{
		private SQLCon _sQLCon;
		public long Id { get; set; }
		public string Name { get; set; }

		public DateTime? CreatedDate { get; set; }

		public BOTRoomGetAllByStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BOTRoomGetAllByStatus()
		{
		}

		public async Task<List<BOTRoomGetAllByStatus>> BOTRoomGetAllByStatusDAO()
		{
			DynamicParameters DP = new DynamicParameters();
			return (await _sQLCon.ExecuteListDapperAsync<BOTRoomGetAllByStatus>("BOT_RoomGetAllByStatus", DP)).ToList();
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

		public async Task<int?> BOTRoomUpdateStatus(long roomId, bool status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", roomId);
			DP.Add("Status", status);
			return (await _sQLCon.ExecuteNonQueryDapperAsync("BOT_RoomUpdateStatus", DP));
		}

		public async Task<int> BOTRoomEnableBot(string roomName,int type)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Type", type);
			DP.Add("Name", roomName);
			return (await _sQLCon.ExecuteNonQueryDapperAsync("BOT_UpdateRoomType", DP)); 
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
			 string FromFullName,
			 string FromAvatar,
			  DateTime DateSend)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RoomId", RoomId);
			DP.Add("FromUserId", FromUserId);
			DP.Add("MessageContent", MessageContent);
			DP.Add("DateSend", DateSend);
			DP.Add("FromFullName", FromFullName);
			DP.Add("FromAvatar", FromAvatar);
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

		public string fromFullName { get; set; }

		public string fromAvatar { get; set; }


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
	public class BotGetLibrary
	{
		private SQLCon _sQLCon;
		private readonly IAppSetting _appSetting;

		public BotGetLibrary(IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BotGetLibrary()
		{
		}
		public int Id { get; set; }
		public string Title { get; set; }
		public string Question { get; set; }
		public int? IdSuggetLibrary { get; set; }
		public byte TypeChat { get; set; }
		public byte? TypeSuggest { get; set; }
		public string LinkSuggest { get; set; }
		public string Answers { get; set; }
		public string TitleAnswers { get; set; }
		public string QuestionAnswers { get; set; }

		public class BotLib
		{
			public int id { get; set; }
			public string pattern { get; set; }
			public template template { get; set; }
		}
		public class template
		{
			public string title { get; set; }
			public string pakn { get; set; }
		}

		private List<BotLib> changeDataNew(List<BotGetLibrary> libraries)
		{
			List<BotLib> lstLib = new List<BotLib>();

			foreach (BotGetLibrary item in libraries)
			{
				if (lstLib.Count == 0)
				{
					BotLib lib = new BotLib();
					lib.id = item.Id;
					lib.pattern = item.Title;
					lib.template = new template();
					lib.template.title = item.Question;
					lib.template.pakn = SetJson(item.Id, libraries);
					lstLib.Add(lib);
				}
				else
				{
					bool checkDuplicate = false;
					foreach (BotLib itemLib in lstLib)
					{
						if (itemLib.id == item.Id)
						{
							checkDuplicate = true;
							break;
						}
					}
					if (!checkDuplicate)
					{
						BotLib lib = new BotLib();
						lib.id = item.Id;
						lib.pattern = item.Title;
						lib.template = new template();
						lib.template.title = item.Question;
						lib.template.pakn = SetJson(item.Id, libraries);
						lstLib.Add(lib);
					}
				}
			}
			return lstLib;
		}

		private string changeData(List<BotGetLibrary> libraries)
		{
			string result = "";
			List<BotLib> lstLib = new List<BotLib>();

			foreach (BotGetLibrary item in libraries)
			{
				if (lstLib.Count == 0)
				{
					BotLib lib = new BotLib();
					lib.id = item.Id;
					lib.pattern = item.Title;
					lib.template = new template();
					lib.template.title = item.Question;
					lib.template.pakn = SetJson(item.Id, libraries);
					lstLib.Add(lib);
				}
				else
				{
					bool checkDuplicate = false;
					foreach (BotLib itemLib in lstLib)
					{
						if (itemLib.id == item.Id)
						{
							checkDuplicate = true;
							break;
						}
					}
					if (!checkDuplicate)
					{
						BotLib lib = new BotLib();
						lib.id = item.Id;
						lib.pattern = item.Title;
						lib.template = new template();
						lib.template.title = item.Question;
						lib.template.pakn = SetJson(item.Id, libraries);
						lstLib.Add(lib);
					}
				}
			}
			result = SetResult(lstLib);

			return result;
		}

		private string SetResult(List<BotLib> lstLib)
		{
			string result = "";
			result = @"<?xml version=""1.0"" encoding=""utf-8""?>"
					+ @"<aiml version=""1.0"">";
			foreach (BotLib item in lstLib)
			{
				result += "<category>"
						+ "<pattern>" + item.pattern + "</pattern>"
						+ "<template>"
						+ item.template.title
						+ (item.template.pakn == "" ? "<pakn/>" : "<pakn>" + item.template.pakn + "</pakn>")
						+ "</template>"
						+ "</category>";

			}
			result += "</aiml>";
			
			return result;
		}

		private string SetJson(int idLib, List<BotGetLibrary> getLibraries)
		{
			string result = "";
			foreach (var item in getLibraries)
			{
				if (item.Id == idLib)
				{
					if (result == "")
						result += @"{""title"":""" + item.Answers + @""",""image"":"""", ""hiddenAnswer"":""" + item.QuestionAnswers + @""", ""typeSuggest"":""" + item.TypeSuggest + @""", ""linkSuggest"":""" + item.LinkSuggest + @""", ""idSuggetLibrary"":""" + item.IdSuggetLibrary + @"""}";
					else
						result += @",{""title"":""" + item.Answers + @""",""image"":"""", ""hiddenAnswer"":""" + item.QuestionAnswers + @""", ""typeSuggest"":""" + item.TypeSuggest + @""", ""linkSuggest"":""" + item.LinkSuggest + @""", ""idSuggetLibrary"":""" + item.IdSuggetLibrary + @"""}";
				}
			}
			if (result != "")
			{
				result = @"{""type"":""carousel"",""data"":[" + result + "]}";
			}
			return result;
		}


		public async Task<string> BotGetAllLibrary()
		{
			DynamicParameters DP = new DynamicParameters();
			List<BotGetLibrary> libraries = (await _sQLCon.ExecuteListDapperAsync<BotGetLibrary>("SY_Chatbot_Library_GetAll", DP)).ToList();
			//
			string result = changeData(libraries);

           
            try
            {
				string path = Path.Combine(Environment.CurrentDirectory, "customaiml.xml");
				XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
				doc.LoadXml(result);
				doc.Save(path);
            }
            catch (Exception ex)
            {

            }

            return result;
		}

		public List<ResultBotNew> BotGetLibraryByInput(string input)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("InputString", input);
			List<BotGetLibrary> libraries = (_sQLCon.ExecuteListDapper<BotGetLibrary>("SY_Chatbot_Library_GetByInput", DP)).ToList();
			//
			var result = changeDataNew(libraries);

			List<ResultBotNew> rs = new List<ResultBotNew>();
            foreach (var item in result)
            {
                ResultBotNew rb = new ResultBotNew();
                rb.Answer = item.template.title;
                rb.SubTags = item.template.pakn;
				rs.Add(rb);
            }
            return rs;
		}

		public List<ResultBotNew> BotGetLibraryById(string idSuggestLibrary)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("IdSuggestLibrary", idSuggestLibrary);
			List<BotGetLibrary> libraries = (_sQLCon.ExecuteListDapper<BotGetLibrary>("SY_Chatbot_Library_GetByIdSuggest", DP)).ToList();
			//
			var result = changeDataNew(libraries);

			List<ResultBotNew> rs = new List<ResultBotNew>();
            foreach (var item in result)
            {
                ResultBotNew rb = new ResultBotNew();
                rb.Answer = item.template.title;
                rb.SubTags = item.template.pakn;
				rs.Add(rb);
            }
            return rs;
		}
	}
}
