using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models.Chatbot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers.ChatbotController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public ChatbotController(IWebHostEnvironment webHostEnvironment, IAppSetting appSetting, IClient bugsnag)
        {
            _webHostEnvironment = webHostEnvironment;
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

        [HttpPost]
        [Authorize]
        [Route("ChatbotDeleteBase")]
        public async Task<ActionResult<object>> ChatbotDeleteBase(ChatbotDeleteIN _ChatbotDeleteIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotDelete(_webHostEnvironment, _appSetting).ChatbotDeleteDAO(_ChatbotDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Authorize]
        [Route("ChatbotGetAllOnPageBase")]
        public async Task<ActionResult<object>> ChatbotGetAllOnPageBase(int? PageSize, int? PageIndex, string Question, string Answer, bool? IsActived)
        {
            try
            {
                List<ChatbotGetAllOnPage> rsChatbotGetAllOnPage = await new ChatbotGetAllOnPage(_appSetting).ChatbotGetAllOnPageDAO(PageSize, PageIndex, Question, Answer, IsActived);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ChatbotGetAllOnPage", rsChatbotGetAllOnPage},
                        {"TotalCount", rsChatbotGetAllOnPage != null && rsChatbotGetAllOnPage.Count > 0 ? rsChatbotGetAllOnPage[0].RowNumber : 0},
                        {"PageIndex", rsChatbotGetAllOnPage != null && rsChatbotGetAllOnPage.Count > 0 ? PageIndex : 0},
                        {"PageSize", rsChatbotGetAllOnPage != null && rsChatbotGetAllOnPage.Count > 0 ? PageSize : 0},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("HistoryChatbotGetAllOnPage")]
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

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("ChatbotGetByID")]
        public async Task<ActionResult<object>> ChatbotGetByIDBase(int? Id)
        {
            try
            {
                List<ChatbotGetByID> rsChatbotGetByID = await new ChatbotGetByID(_appSetting).ChatbotGetByIDDAO(Id);
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"ChatbotGetByID", rsChatbotGetByID},
                    };
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("ChatbotInsertQuestion")]
        public async Task<object> ChatbotInsertQuestion(ChatbotInsertIN _chatbotInsertIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotInsert(_webHostEnvironment,_appSetting).ChatbotInsertDAO(_chatbotInsertIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("ChatbotUpdateBase")]
        public async Task<ActionResult<object>> ChatbotUpdateBase(ChatbotUpdateIN ChatbotUpdateIN)
        {
            try
            {
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotUpdate(_webHostEnvironment, _appSetting).ChatbotUpdateDAO(ChatbotUpdateIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize]
        [Route("ChatbotInsertData")]
        public async Task<object> ChatbotInsertData(ChatbotDataInsertIN _chatbotDataInsertIN)
        {
            try
            {
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotInsertData(_appSetting).InsertDataChatbotDAO(_chatbotDataInsertIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

    }
}
