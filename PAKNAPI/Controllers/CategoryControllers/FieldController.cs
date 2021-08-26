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
    [Route("api/field")]
    [ApiController]
	[ValidateModel]
	public class FieldController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public FieldController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }
		/// <summary>
		/// xóa lĩnh vực
		/// </summary>
		/// <param name="_cAFieldDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("delete")]
		public async Task<ActionResult<object>> CAFieldDeleteBase(CAFieldDeleteIN _cAFieldDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldDelete(_appSetting).CAFieldDeleteDAO(_cAFieldDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// dánh sách lĩnh vực
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Name"></param>
		/// <param name="Description"></param>
		/// <param name="IsActived"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize]
		[Route("get-list-field-on-page")]
		public async Task<ActionResult<object>> CAFieldGetAllOnPageBase(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			try
			{
				List<CAFieldGetAllOnPage> rsCAFieldGetAllOnPage = await new CAFieldGetAllOnPage(_appSetting).CAFieldGetAllOnPageDAO(PageSize, PageIndex, Name, Description, IsActived);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetAllOnPage", rsCAFieldGetAllOnPage},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// chi tiết lĩnh vực
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAFieldGetByIDBase(int? Id)
		{
			try
			{
				List<CAFieldGetByID> rsCAFieldGetByID = await new CAFieldGetByID(_appSetting).CAFieldGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetByID", rsCAFieldGetByID},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// :D
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Authorize]
		[Route("get-drop-down")]
		public async Task<ActionResult<object>> CAFieldGetDropdownBase()
		{
			try
			{
				List<CAFieldGetDropdown> rsCAFieldGetDropdown = await new CAFieldGetDropdown(_appSetting).CAFieldGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetDropdown", rsCAFieldGetDropdown},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới lĩnh vực
		/// </summary>
		/// <param name="_cAFieldInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("insert")]
		public async Task<ActionResult<object>> CAFieldInsertBase(CAFieldInsertIN _cAFieldInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldInsert(_appSetting).CAFieldInsertDAO(_cAFieldInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập lĩnh vực
		/// </summary>
		/// <param name="_cAFieldUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize]
		[Route("update")]
		public async Task<ActionResult<object>> CAFieldUpdateBase(CAFieldUpdateIN _cAFieldUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldUpdate(_appSetting).CAFieldUpdateDAO(_cAFieldUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// dropdown lĩnh vực knct
		/// </summary>
		/// <returns></returns>

        [HttpGet]
        [Authorize("ThePolicy")]
        [Route("field-knct-get-dropdown")]
        public async Task<ActionResult<object>> CAFieldKNCTGetDropdownBase()
        {
            try
            {
                List<CAFieldKNCTGetDropdown> rsCAFieldKNCTGetDropdown = await new CAFieldKNCTGetDropdown(_appSetting).CAFieldKNCTGetDropdownDAO();
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"CAFieldKNCTGetDropdown", rsCAFieldKNCTGetDropdown},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
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
