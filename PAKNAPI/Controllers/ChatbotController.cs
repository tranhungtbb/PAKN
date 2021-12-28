using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.Models.Chatbot;
using SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers.ChatbotController
{
    [Route("api/chat-bot")]
    [ApiController]
    [ValidateModel]
    public class ChatbotController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IManageBots _bots;
        public ChatbotController(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting, IClient bugsnag, IManageBots bots)
        {
            _webHostEnvironment = webHostEnvironment;
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _bots = bots;
        }
        /// <summary>
        /// xóa câu hỏi chatbot
        /// </summary>
        /// <param name="_ChatbotDeleteIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("delete")]
        public async Task<ActionResult<object>> ChatbotDeleteBase(ChatbotDeleteIN _ChatbotDeleteIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotDelete(_webHostEnvironment, _appSetting).ChatbotDeleteDAO(_ChatbotDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách câu hỏi chatbot
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="Question"></param>
        /// <param name="Answer"></param>
        /// <param name="IsActived"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-list-chat-bot-on-page")]
        public async Task<ActionResult<object>> ChatbotGetAllOnPageBase(int PageIndex, int PageSize, string Title, string Question, bool? IsActive)
        {
            try
            {
                List<ChatbotGetAllOnPage> rsChatbotGetAllOnPage =
                    await new ChatbotGetAllOnPage(_appSetting).ChatbotGetAllOnPageDAO(PageIndex, PageSize, Title, Question, IsActive);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ChatbotGetAllOnPage", rsChatbotGetAllOnPage},
                        {"TotalCount", rsChatbotGetAllOnPage != null && rsChatbotGetAllOnPage.Count > 0 ? rsChatbotGetAllOnPage[0].RowNumber : 0},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách lịch sử chatbot
        /// </summary>
        /// <param name="PageSize"></param>
        /// <param name="PageIndex"></param>
        /// <param name="FullName"></param>
        /// <param name="Question"></param>
        /// <param name="Answer"></param>
        /// <param name="CreatedDate"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("list-his")]
        public async Task<ActionResult<object>> HistoryChatbotGetAllOnPage(int? PageSize, int? PageIndex, string FullName, string Question, string Answer, DateTime? CreatedDate)
        {
            try
            {
                List<HistoryChatbotGetAllOnPage> rsHistoryChatbotGetAllOnPage = await new HistoryChatbotGetAllOnPage(_appSetting).HistoryChatbotGetAllOnPageDAO(PageSize, PageIndex, FullName, Question, Answer, CreatedDate);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"HistoryChatbotGetAllOnPage", rsHistoryChatbotGetAllOnPage},
                        {"TotalCount", rsHistoryChatbotGetAllOnPage != null && rsHistoryChatbotGetAllOnPage.Count > 0 ? rsHistoryChatbotGetAllOnPage[0].RowNumber : 0},
                        {"PageIndex", rsHistoryChatbotGetAllOnPage != null && rsHistoryChatbotGetAllOnPage.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsHistoryChatbotGetAllOnPage != null && rsHistoryChatbotGetAllOnPage.Count > 0 ? PageSize : 0},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// danh sách câu hỏi active
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-all-active")]
        public async Task<ActionResult<object>> ChatbotGetAllActvieBase()
        {
            try
            {
                List<ChatbotGetAllActive> ChatbotGetAllActive = await new ChatbotGetAllActive(_appSetting).ChatbotGetAllActiveDAO();
                List<DropdownObject> ListHashtag = await new ChatbotGetAllActive(_appSetting).GetDropdownHashtag();
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ChatbotGetAll", ChatbotGetAllActive},
                        {"ListHashtag", ListHashtag},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// chi tiết câu hỏi chatbot
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("get-by-id")]
        public async Task<ActionResult<object>> ChatbotGetByIDBase(long Id)
        {
            try
            {
                List<ChatbotGetByID> rsChatbotGetByID = await new ChatbotGetByID(_appSetting).ChatbotGetByIDDAO(Id);
                List<ChatbotHashtag> rsChatbotHashtag = await new ChatbotGetByID(_appSetting).ChatbotHashtagGetByChatbotDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ChatbotGetByID", rsChatbotGetByID},
                        {"ListChatbotHashtag", rsChatbotHashtag},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// chi tiết câu trả lời chatbot
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("answer-get-by-libid")]
        public async Task<ActionResult<object>> ChatbotLibGetByIDBase(int? Id)
        {
            try
            {
                List<ChatbotLibGetByID> rsChatbotLibGetByID = await new ChatbotLibGetByID(_appSetting).ChatbotLibGetByIDDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ChatbotLibGetByID", rsChatbotLibGetByID},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// thêm mới câu hỏi chatbot
        /// </summary>
        /// <param name="_chatbotInsertIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("insert-question")]
        public async Task<object> ChatbotInsertQuestion()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                ChatbotInsertIN _chatbotInsertIN = JsonConvert.DeserializeObject<ChatbotInsertIN>(Request.Form["data"].ToString(), jss);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
                var resInsert = await new ChatbotInsert(_webHostEnvironment, _appSetting).ChatbotInsertDAO(_chatbotInsertIN);
                await _bots.ReloadBots();
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = resInsert };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        /// <summary>
        /// cập nhập câu hỏi chatbot
        /// </summary>
        /// <param name="ChatbotUpdateIN"></param>
        /// <returns></returns>

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("update")]
        public async Task<ActionResult<object>> ChatbotUpdateBase()
        {
            try
            {
                var jss = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Local,
                    DateParseHandling = DateParseHandling.DateTimeOffset,
                };
                ChatbotUpdateIN ChatbotUpdateIN = JsonConvert.DeserializeObject<ChatbotUpdateIN>(Request.Form["ChatbotUpdateIN"].ToString(), jss);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
                var resUpdate = await new ChatbotUpdate(_webHostEnvironment, _appSetting).ChatbotUpdateDAO(ChatbotUpdateIN);
                await _bots.ReloadBots();
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = resUpdate };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("insert-data")]
        public async Task<object> ChatbotInsertData(ChatbotDataInsertIN _chatbotDataInsertIN)
        {
            try
            {
                var resInsert = await new ChatbotInsertData(_appSetting).InsertDataChatbotDAO(_chatbotDataInsertIN);
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
                await _bots.ReloadBots();
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = resInsert };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("import-data-chat-bot")]
        [Authorize("ThePolicy")]
        public async Task<ActionResult<object>> ImportData()
        {
            var file = Request.Form.Files[0];

            if (file.Length <= 0)
            {
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = "File not found!" };
            }
            string folder = "Upload\\ChatBot\\";
            string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var name = Path.GetFileName(file.FileName).Replace("+", "");
            string filePath = Path.Combine(folderPath, name);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            FileInfo fileInfo = new FileInfo(filePath);

            OfficeOpenXml.ExcelPackage package = new OfficeOpenXml.ExcelPackage(fileInfo);
            OfficeOpenXml.ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

            try
            {
                // get number of rows and columns in the sheet

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;
                ChatbotInsertIN model = new ChatbotInsertIN();
                model.IsActived = true;
                model.IsDeleted = false;
                model.CategoryId = 0;
                model.TypeChat = 1;

                for (int i = 3; i <= rows; i++)
                {
                    model.Title = worksheet.Cells[i, 2].Value?.ToString();
                    model.Question = worksheet.Cells[i, 3].Value?.ToString();
                    await new ChatbotInsert(_webHostEnvironment, _appSetting).ChatbotInsertDAO(model);
                }

                await _bots.ReloadBots();

                return new Models.Results.ResultApi { Success = ResultCode.OK};
            }
            catch (Exception ex)
            {
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
            finally {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
