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
    [Route("api/group-word")]
    [ApiController]
	[ValidateModel]
	public class GroupWordController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public GroupWordController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }
		/// <summary>
		/// xóa nhóm thư viện từ
		/// </summary>
		/// <param name="_cAGroupWordDeleteIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> CAGroupWordDeleteBase(CAGroupWordDeleteIN _cAGroupWordDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAGroupWordDelete(_appSetting).CAGroupWordDeleteDAO(_cAGroupWordDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách nhóm thư viện từ
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		/// <param name="IsActived"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-group-word-on-page")]
		public async Task<ActionResult<object>> CAGroupWordGetAllOnPageBase()
		{
			try
			{
				List<CAGroupWordGetAllOnPage> rsCAGroupWordGetAllOnPage = await new CAGroupWordGetAllOnPage(_appSetting).CAGroupWordGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWordGetAllOnPage", rsCAGroupWordGetAllOnPage},
						{"TotalCount", rsCAGroupWordGetAllOnPage != null && rsCAGroupWordGetAllOnPage.Count > 0 ? rsCAGroupWordGetAllOnPage[0].RowNumber : 0}
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
		/// chi tiết nhóm thư viện từ
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAGroupWordGetByIDBase(int? Id)
		{
			try
			{
				List<CAGroupWordGetByID> rsCAGroupWordGetByID = await new CAGroupWordGetByID(_appSetting).CAGroupWordGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWordGetByID", rsCAGroupWordGetByID},
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


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-suggest")]
		public async Task<ActionResult<object>> CAGroupWordGetListSuggestBase()
		{
			try
			{
				List<CAGroupWordGetListSuggest> rsCAGroupWordGetListSuggest = await new CAGroupWordGetListSuggest(_appSetting).CAGroupWordGetListSuggestDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAGroupWordGetListSuggest", rsCAGroupWordGetListSuggest},
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
		/// thêm mới nhóm thư viện từ
		/// </summary>
		/// <param name="_cAGroupWordInsertIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<ActionResult<object>> CAGroupWordInsertBase(CAGroupWordInsertIN _cAGroupWordInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var result = Convert.ToInt32(await new CAGroupWordInsert(_appSetting).CAGroupWordInsertDAO(_cAGroupWordInsertIN));
				if (result > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Thêm mới thành công" };
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = result, Message = "Đã tồn tại nhóm thư viện từ ngữ" };
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
		/// cập nhập nhóm thư viện từ
		/// </summary>
		/// <param name="_cAGroupWordUpdateIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> CAGroupWordUpdateBase(CAGroupWordUpdateIN _cAGroupWordUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				var result = Convert.ToInt32(await new CAGroupWordUpdate(_appSetting).CAGroupWordUpdateDAO(_cAGroupWordUpdateIN));
				if (result > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = result, Message = "Thêm mới thành công" };
				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = result, Message = "Đã tồn tại nhóm thư viện từ ngữ" };
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
