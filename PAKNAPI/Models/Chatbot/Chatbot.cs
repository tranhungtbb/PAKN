using Dapper;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace PAKNAPI.Models.Chatbot
{

	#region ChatbotDelete
	public class ChatbotDelete
	{
		private SQLCon _sQLCon;
		private string _filePath;

		public ChatbotDelete(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
			string folder = "Chatbot\\binaries\\aiml\\chatbot.aiml";
			_filePath = System.IO.Path.Combine(webHostEnvironment.ContentRootPath, folder);
		}

		public ChatbotDelete()
		{
		}

		public async Task<int> ChatbotDeleteDAO(ChatbotDeleteIN _chatbotDeleteIN)
		{
			await ChatbotDeleteQuestion(_chatbotDeleteIN);

			return await DeleteChatbotDAO(_chatbotDeleteIN);
		}

		private async Task<int> DeleteChatbotDAO(ChatbotDeleteIN _chatbotDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _chatbotDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("ChatbotDelete", DP));
		}

		public async Task<bool> ChatbotDeleteQuestion(ChatbotDeleteIN _chatbotDeleteIN)
		{
			XDocument xDocument = XDocument.Load(_filePath);
			var categoryFilter = xDocument.Descendants("category").Where(c => c.Attribute("id").Value.Equals(_chatbotDeleteIN.CategoryId.ToString())).FirstOrDefault();
			if (categoryFilter == null)
            {
				return false;
			}
			categoryFilter.Remove();
			xDocument.Save(_filePath);
			await Task.Delay(10);
			return true;
		}

	}

	public class ChatbotDeleteIN
	{
		public int? Id { get; set; }

		public int CategoryId { get; set; }
	}
	#endregion

	#region ChatbotGetAllOnPage
	public class ChatbotGetAllOnPage
	{
		private SQLCon _sQLCon;

		public ChatbotGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public ChatbotGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public int CategoryId { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<ChatbotGetAllOnPage>> ChatbotGetAllOnPageDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<ChatbotGetAllOnPage>("ChatbotGetAllOnPage", DP)).ToList();
		}
	}
	#endregion

	#region HistoryChatbotGetAllOnPage
	public class HistoryChatbotGetAllOnPage
	{
		private SQLCon _sQLCon;

		public HistoryChatbotGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public HistoryChatbotGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public long UserId { get; set; }
		public string FullName { get; set; }
		public string Kluid { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }

		public DateTime CreatedDate { get; set; }

		public async Task<List<HistoryChatbotGetAllOnPage>> HistoryChatbotGetAllOnPageDAO(int? PageSize, int? PageIndex, string FullName, string Question, string Answer, DateTime? CreatedDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FullName", FullName);
			DP.Add("Question", Question);
			DP.Add("Answer", Answer);
			DP.Add("CreateDate", CreatedDate);

			return (await _sQLCon.ExecuteListDapperAsync<HistoryChatbotGetAllOnPage>("HistoryChatbotGetAllOnPage", DP)).ToList();
		}
	}

	#endregion

	#region ChatbotGetByID
	public class ChatbotGetByID
	{
		private SQLCon _sQLCon;

		public ChatbotGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public ChatbotGetByID()
		{
		}

		public int Id { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public int CategoryId { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<ChatbotGetByID>> ChatbotGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<ChatbotGetByID>("ChatbotGetByID", DP)).ToList();
		}
	}
	#endregion

	#region ChatbotInsertData
	public class ChatbotInsertData
    {
		private SQLCon _sQLCon;
		private readonly IAppSetting _appSetting;

		public ChatbotInsertData(IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public ChatbotInsertData()
		{
		}

		public async Task<int> InsertDataChatbotDAO(ChatbotDataInsertIN _chatbotDataInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Kluid", _chatbotDataInsertIN.Kluid);
			DP.Add("UserId", _chatbotDataInsertIN.UserId);
			DP.Add("FullName", _chatbotDataInsertIN.FullName);
			DP.Add("Question", _chatbotDataInsertIN.Question.Trim());
			DP.Add("Answer", _chatbotDataInsertIN.Answer.Trim());
			return (await _sQLCon.ExecuteNonQueryDapperAsync("DataChatbotInsert", DP));
		}
	}

	public class ChatbotDataInsertIN
	{
		public string Id { get; set; }
		public string Kluid { get; set; }
		public string UserId { get; set; }
		public string FullName { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }

	}
	#endregion

	#region ChatbotInsert
	public class ChatbotInsert
	{
		private SQLCon _sQLCon;
		private string _filePath;
		private readonly IAppSetting _appSetting;

		public ChatbotInsert(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting)
		{
			_appSetting = appSetting;
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
			string folder = "Chatbot\\binaries\\aiml\\chatbot.aiml";
			_filePath = System.IO.Path.Combine(webHostEnvironment.ContentRootPath, folder);
		}


		public ChatbotInsert()
		{
		}

		public async Task<int> ChatbotInsertDAO(ChatbotInsertIN _chatbotInsertIN)
		{
			string nextCategoryId = await new ChatbotGetNextCategoryId(_appSetting).ChatbotGetMaxCategoryIdDAO();
			_chatbotInsertIN.CategoryId = Convert.ToInt32(nextCategoryId);

			await ChatbotInsertFile(_chatbotInsertIN);
			return await InsertChatbotDAO(_chatbotInsertIN);
		}

		private async Task<int> InsertChatbotDAO(ChatbotInsertIN _chatbotInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Question", _chatbotInsertIN.Question.Trim());
			DP.Add("Answer", _chatbotInsertIN.Answer.Trim());
			DP.Add("CategoryId", _chatbotInsertIN.CategoryId);
			DP.Add("IsActived", _chatbotInsertIN.IsActived);
			DP.Add("IsDeleted", _chatbotInsertIN.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("ChatbotInsert", DP));
		}

		public async Task<string> ChatbotInsertFile(ChatbotInsertIN _chatbotInsertIN)
		{
			string categoryId = await GetcategoryId(_chatbotInsertIN.CategoryId.ToString());

			if (!string.IsNullOrEmpty(categoryId))
            {
				AddQuestion(_chatbotInsertIN);
			}
			else
            {
				WriteFileChatbot(_chatbotInsertIN);
			}

			return categoryId;
		}

		private async Task<string> GetcategoryId(string categoryId)
		{
			if (!System.IO.File.Exists(_filePath))
			{
				return string.Empty;
			}

			XDocument testXML = XDocument.Load(_filePath);
			XElement category = testXML.Descendants("category").Where(c => c.Attribute("id").Value.Equals(categoryId)).FirstOrDefault();
			testXML.Save(_filePath);
			await Task.Delay(0);
			return categoryId;
		}

		private void WriteFileChatbot(ChatbotInsertIN _chatbotInsertIN)
        {

			if (!System.IO.File.Exists(_filePath))
			{
				using (XmlTextWriter writer = new XmlTextWriter(_filePath, Encoding.UTF8))
				{
					writer.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
					writer.WriteStartElement("aiml");
					writer.WriteAttributeString("version", "1.0");
					writer.WriteEndElement();

					writer.Close();
				}
			}

			using (XmlTextReader read = new XmlTextReader(_filePath))
			{
				using (XmlTextWriter write = new XmlTextWriter(_filePath + "2.aiml", Encoding.UTF8))
				{
					write.WriteProcessingInstruction("xml", "version='1.0' encoding='UTF-8'");
					write.WriteStartElement("aiml");
					write.WriteAttributeString("version", "1.0");

					while (read.Read())
					{
						if (read.NodeType == XmlNodeType.Element && read.Name.ToLower() == "aiml")
						{
							// open tag category
							write.WriteStartElement("category");
							write.WriteAttributeString("id", "0");

							// add tag children pattern
							write.WriteElementString("pattern", "*");

							// open tag template
							write.WriteStartElement("template");

							// open tag random
							write.WriteStartElement("random");

							// add tag children li
							write.WriteElementString("li", "Bạn vui lòng hỏi câu khác...");
							write.WriteElementString("li", "Bạn vui lòng hỏi câu hỏi có trong thư viện.");

							// end tag random
							write.WriteEndElement();

							// end tag template
							write.WriteEndElement();

							// end tag category
							write.WriteEndElement();

							write.WriteStartElement("category");
							write.WriteAttributeString("id", _chatbotInsertIN.CategoryId.ToString());
							write.WriteElementString("pattern", _chatbotInsertIN.Question.Trim());
							write.WriteElementString("template", _chatbotInsertIN.Answer.Trim());
							write.WriteEndElement();
						}
					}

					write.WriteEndElement();
				}
			}

			System.IO.File.Delete(_filePath);
			System.IO.File.Move(_filePath + "2.aiml", _filePath);
		}

		private void AddQuestion(ChatbotInsertIN _chatbotInsertIN)
        {
			XDocument xDocument = XDocument.Load(_filePath);

			XElement xElement = new XElement("category");
			xElement.SetAttributeValue("id", _chatbotInsertIN.CategoryId);
			xElement.SetElementValue("pattern", _chatbotInsertIN.Question.Trim());
			xElement.SetElementValue("template", _chatbotInsertIN.Answer.Trim());
			xDocument.Element("aiml").Add(xElement);
			xDocument.Save(_filePath);
		}
	}

	public class ChatbotInsertIN
	{
		public string Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Câu hỏi không được để trống")]

		public string Question { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Câu trả lời không được để trống")]
		public string Answer { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Loại câu hỏi không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Loại câu hỏi không đúng định dạng")]
		public int CategoryId { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
	}

	#endregion

	#region ChatbotGetNextCategoryId
	public class ChatbotGetNextCategoryId
	{
		private SQLCon _sQLCon;

		public ChatbotGetNextCategoryId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public ChatbotGetNextCategoryId()
		{
		}

		public async Task<string> ChatbotGetMaxCategoryIdDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<int>("ChatbotGetNextCategoryId", DP)).FirstOrDefault().ToString();
		}
	}

	#endregion

	#region ChatbotUpdate
	public class ChatbotUpdate
	{
		private SQLCon _sQLCon;
		private string _filePath;

		public ChatbotUpdate(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
			string folder = "Chatbot\\binaries\\aiml\\chatbot.aiml";
			_filePath = System.IO.Path.Combine(webHostEnvironment.ContentRootPath, folder);
		}

		public ChatbotUpdate()
		{
		}

		public async Task<int> ChatbotUpdateDAO(ChatbotUpdateIN _chatbotUpdateIN)
		{
			if (_chatbotUpdateIN.IsActived == true)
            {
				await UpdateFileQuestion(_chatbotUpdateIN);
			}
			else
            {
				await DeleteFileQuestion(_chatbotUpdateIN);
			}
			
			return await UpdateDAOQuestion(_chatbotUpdateIN); 
		}

		private async Task<int> UpdateDAOQuestion(ChatbotUpdateIN _chatbotUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _chatbotUpdateIN.Id);
			DP.Add("Question", _chatbotUpdateIN.Question.Trim());
			DP.Add("Answer", _chatbotUpdateIN.Answer.Trim());
			DP.Add("CategoryId", _chatbotUpdateIN.CategoryId);
			DP.Add("IsActived", _chatbotUpdateIN.IsActived);
			DP.Add("IsDeleted", _chatbotUpdateIN.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("ChatbotUpdate", DP));
		}

		public async Task<int> DeleteFileQuestion(ChatbotUpdateIN _chatbotUpdateIN)
		{
			XDocument xDocument = XDocument.Load(_filePath);
			var categoryFilter = xDocument.Descendants("category").Where(c => c.Attribute("id").Value.Equals(_chatbotUpdateIN.CategoryId.ToString())).FirstOrDefault();
			if (categoryFilter == null)
			{
				return _chatbotUpdateIN.CategoryId;
			}
			categoryFilter.Remove();
			xDocument.Save(_filePath);
			await Task.Delay(10);
			return _chatbotUpdateIN.CategoryId;
		}

		public async Task<int> UpdateFileQuestion(ChatbotUpdateIN _chatbotUpdateIN)
		{
			XDocument xDocument = XDocument.Load(_filePath);
			var categoryFilter = xDocument.Descendants("category").Where(c => c.Attribute("id").Value.Equals(_chatbotUpdateIN.CategoryId.ToString())).FirstOrDefault();
			if (categoryFilter == null)
            {
				AddFileQuestion(_chatbotUpdateIN);
				return _chatbotUpdateIN.CategoryId;
			}
			categoryFilter.SetElementValue("pattern", _chatbotUpdateIN.Question.Trim());
			categoryFilter.SetElementValue("template", _chatbotUpdateIN.Answer.Trim());
			xDocument.Save(_filePath);
			await Task.Delay(10);
			return _chatbotUpdateIN.CategoryId;
		}

		private void AddFileQuestion(ChatbotUpdateIN _chatbotUpdateIN)
		{
			XDocument xDocument = XDocument.Load(_filePath);

			XElement xElement = new XElement("category");
			xElement.SetAttributeValue("id", _chatbotUpdateIN.CategoryId);
			xElement.SetElementValue("pattern", _chatbotUpdateIN.Question.Trim());
			xElement.SetElementValue("template", _chatbotUpdateIN.Answer.Trim());
			xDocument.Element("aiml").Add(xElement);
			xDocument.Save(_filePath);
		}
	}

	public class ChatbotUpdateIN
	{
		public int? Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Câu hỏi không được để trống")]

		public string Question { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Câu trả lời không được để trống")]
		public string Answer { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Loại câu hỏi không được để trống")]
		[Range(0, int.MaxValue, ErrorMessage = "Loại câu hỏi không đúng định dạng")]
		public int CategoryId { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
	}
	#endregion

}
