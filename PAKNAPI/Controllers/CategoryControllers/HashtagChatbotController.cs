using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.Models;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using System;
using Dapper;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Bugsnag;
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.ModelBase;
using NSwag.Annotations;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/hashtagchatbot")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Nhãn dữ liệu chat bot - Authorize")]
	public class HashtagChatbotController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public HashtagChatbotController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		/// <summary>
		/// danh sách hashtag - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-hashtag-on-page")]
		public async Task<ActionResult<object>> CAHashtagChatbotGetAllOnPage()
		{
			try
			{
				List<CAHashtagChatbotListPage> rsCAHashtagChatbotOnPage = await new CAHashtagChatbotListPage(_appSetting).CAHashtagChatbotGetAllOnPage();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagChatbot", rsCAHashtagChatbotOnPage},
						{"TotalCount", rsCAHashtagChatbotOnPage != null && rsCAHashtagChatbotOnPage.Count > 0 ? rsCAHashtagChatbotOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách hashtag
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-all")]
		public async Task<ActionResult<object>> CAHashtagChatbotGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtagChatbot(_appSetting).CAHashtagChatbotGetAll() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết hashtag - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAHashtagChatbotGetByIDBase(int? Id)
		{
			try
			{
				List<CAHashtagChatbotGetByID> rsCAHashtagChatbotGetByID = await new CAHashtagChatbotGetByID(_appSetting).CAHashtagChatbotGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAHashtagChatbotGetByID", rsCAHashtagChatbotGetByID},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới hashtag chatbot - Authorize
		/// </summary>
		/// <param name="_CAHashtagChatbot"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<ActionResult<object>> CAHashtagChatbotInsert(CAHashtagChatbotInsertIN _CAHashtagChatbot)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAHashtagChatbotInsert(_appSetting).CAHashtagChatbotInsertDAO(_CAHashtagChatbot) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// update hashtag - Authorize
		/// </summary>
		/// <param name="_CAHashtagChatbot"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> CAHashtagChatbotUpdate(CAHashtagChatbotUpdateIN _CAHashtagChatbot)
		{
			try
			{
				int count = await new CAHashtagChatbot(_appSetting).CAHashtagChatbotUpdate(_CAHashtagChatbot);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = count, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// xóa hashtag
		/// </summary>
		/// <param name="_CAHashtagChatbot"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> CAHashtagChatbotDelete(HashtagChatbotDelete _CAHashtagChatbot)
		{
			try
			{
				int count = await new CAHashtagChatbot(_appSetting).CAHashtagChatbotDelete(_CAHashtagChatbot);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
	}
}
