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


namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/HISNews")]
    [ApiController]
    public class HISNewsController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public HISNewsController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }


		[HttpPost]
		[Authorize]
		[Route("HISNewsInsert")]
		public async Task<ActionResult<object>> HISNewsInsert(HISNews _hISNews)
		{
			try
			{
				_hISNews.CreatedBy = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				_hISNews.CreatedDate = DateTime.Now;
				string userName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);

				switch (_hISNews.Status)
				{
					case STATUS_HISNEWS.CREATE:
						_hISNews.Content = userName + " đã khởi tạo bài viết";
						break;
					case STATUS_HISNEWS.UPDATE:
						_hISNews.Content = userName + " đã cập nhập bài viết";
						break;
					case STATUS_HISNEWS.COMPILE:
						_hISNews.Content = userName + " đang soạn thảo bài viết";
						break;
					case STATUS_HISNEWS.PUBLIC:
						_hISNews.Content = userName + " đã công bố bài viết";
						break;
					case STATUS_HISNEWS.CANCEL:
						_hISNews.Content = userName + " đã hủy công bố bài viết";
						break;
				}

				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISNews(_appSetting).HISNewsInsert(_hISNews) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize]
		[Route("HISNewsGetByNewsId")]
		public async Task<ActionResult<object>> HISNewsGetByNewsId(int NewsId)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new HISNewsModel(_appSetting).HISNewsGetByNewsId(NewsId) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}
}
