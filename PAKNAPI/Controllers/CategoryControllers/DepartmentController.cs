﻿using PAKNAPI.Common;
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
    [Route("api/department")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Sở ngành", Description = "Danh mục sở ngành - Authorize")]
	[Authorize(Policy = "ThePolicy", Roles = RoleSystem.ADMIN)]
	public class DepartmentController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public DepartmentController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }

		/// <summary>
		/// xóa sở ngành - Authorize
		/// </summary>
		/// <param name="_cADepartmentDeleteIN"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("delete")]
		[ProducesResponseType(typeof(ResultApi), 200)]

		public async Task<ActionResult<object>> CADepartmentDeleteBase(CADepartmentDeleteIN _cADepartmentDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentDelete(_appSetting).CADepartmentDeleteDAO(_cADepartmentDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách sở ngành - Authorize
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[ProducesResponseType(typeof(ResultApi), 200)]
		[Route("get-list-department-on-page")]
		public async Task<ActionResult<object>> CADepartmentGetAllOnPageBase()
		{
			try
			{
				List<CADepartmentGetAllOnPage> rsCADepartmentGetAllOnPage = await new CADepartmentGetAllOnPage(_appSetting).CADepartmentGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGetAllOnPage", rsCADepartmentGetAllOnPage},
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
		///  chi tiết sở ngành - Authorize
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Route("get-by-id")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentGetByIDBase(int? Id)
		{
			try
			{
				List<CADepartmentGetByID> rsCADepartmentGetByID = await new CADepartmentGetByID(_appSetting).CADepartmentGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CADepartmentGetByID", rsCADepartmentGetByID},
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
		/// thêm mới sở ngành - Authorize
		/// </summary>
		/// <param name="_cADepartmentInsertIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("insert")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentInsertBase(CADepartmentInsertIN _cADepartmentInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentInsert(_appSetting).CADepartmentInsertDAO(_cADepartmentInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập sở ngành - Authorize
		/// </summary>
		/// <param name="_cADepartmentUpdateIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("update")]
		[ProducesResponseType(typeof(ResultApi), 200)]
		public async Task<ActionResult<object>> CADepartmentUpdateBase(CADepartmentUpdateIN _cADepartmentUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CADepartmentUpdate(_appSetting).CADepartmentUpdateDAO(_cADepartmentUpdateIN) };
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
