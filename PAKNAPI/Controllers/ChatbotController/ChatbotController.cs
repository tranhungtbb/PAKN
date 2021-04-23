﻿using Bugsnag;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public ChatbotController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("ChatbotDeleteBase")]
        public async Task<ActionResult<object>> ChatbotDeleteBase(ChatbotDeleteIN _ChatbotDeleteIN)
        {
            try
            {
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotDelete(_appSetting).ChatbotDeleteDAO(_ChatbotDeleteIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpGet]
        [Authorize("ThePolicy")]
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
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

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
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Route("ChatbotInsertQuestion")]
        public async Task<object> ChatbotInsertQuestion(ChatbotInsertIN _chatbotInsertIN)
        {
            try
            {
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotInsert(_appSetting).ChatbotInsertDAO(_chatbotInsertIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

        [HttpPost]
        [Authorize("ThePolicy")]
        [Route("ChatbotUpdateBase")]
        public async Task<ActionResult<object>> ChatbotUpdateBase(ChatbotUpdateIN ChatbotUpdateIN)
        {
            try
            {
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

                return new Models.Results.ResultApi { Success = ResultCode.OK, Result = await new ChatbotUpdate(_appSetting).ChatbotUpdateDAO(ChatbotUpdateIN) };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                // new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

                return new Models.Results.ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

    }
}
