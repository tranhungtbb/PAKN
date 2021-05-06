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

namespace PAKNAPI.ControllerBase
{
	[Route("api/SYUserSPBase")]
	[ApiController]
	public class SYUserSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SYUserSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserRoleMapInsertBase")]
		public async Task<ActionResult<object>> SYUserRoleMapInsertBase(SYUserRoleMapInsertIN _sYUserRoleMapInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMapInsert(_appSetting).SYUserRoleMapInsertDAO(_sYUserRoleMapInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYUserChangePwdBase")]
		public async Task<ActionResult<object>> SYUserChangePwdBase(SYUserChangePwdIN _sYUserChangePwdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_sYUserChangePwdIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYUserChangePwdListBase")]
		public async Task<ActionResult<object>> SYUserChangePwdListBase(List<SYUserChangePwdIN> _sYUserChangePwdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserChangePwdIN in _sYUserChangePwdINs)
				{
					var result = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_sYUserChangePwdIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserDeleteBase")]
		public async Task<ActionResult<object>> SYUserDeleteBase(SYUserDeleteIN _sYUserDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserDelete(_appSetting).SYUserDeleteDAO(_sYUserDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserDeleteListBase")]
		public async Task<ActionResult<object>> SYUserDeleteListBase(List<SYUserDeleteIN> _sYUserDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserDeleteIN in _sYUserDeleteINs)
				{
					var result = await new SYUserDelete(_appSetting).SYUserDeleteDAO(_sYUserDeleteIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
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
		[Route("SYUserGetAllByRoleIdBase")]
		public async Task<ActionResult<object>> SYUserGetAllByRoleIdBase(int? RoleId)
		{
			try
			{
				List<SYUserGetAllByRoleId> rsSYUserGetAllByRoleId = await new SYUserGetAllByRoleId(_appSetting).SYUserGetAllByRoleIdDAO(RoleId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllByRoleId", rsSYUserGetAllByRoleId},
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

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserGetAllOnPageBase")]
		public async Task<ActionResult<object>> SYUserGetAllOnPageBase(int? PageSize, int? PageIndex, string UserName, string FullName, string Phone, bool? IsActived, int? UnitId, int? TypeId, string SortDir, string SortField)
		{
			try
			{
				List<SYUserGetAllOnPage> rsSYUserGetAllOnPage = await new SYUserGetAllOnPage(_appSetting).SYUserGetAllOnPageDAO(PageSize, PageIndex, UserName, FullName, Phone, IsActived, UnitId, TypeId, SortDir, SortField);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetAllOnPage", rsSYUserGetAllOnPage},
						{"TotalCount", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? rsSYUserGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserGetAllOnPage != null && rsSYUserGetAllOnPage.Count > 0 ? PageSize : 0},
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

		[HttpGet]
		[Authorize]
		[Route("SYUserGetByIDBase")]
		public async Task<ActionResult<object>> SYUserGetByIDBase(long? Id)
		{
			try
			{
				List<SYUserGetByID> rsSYUserGetByID = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByID", rsSYUserGetByID},
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

		[HttpGet]
		[Authorize]
		[Route("SYUserGetByRoleIdAllOnPageBase")]
		public async Task<ActionResult<object>> SYUserGetByRoleIdAllOnPageBase(int? PageSize, int? PageIndex, int? RoleId)
		{
			try
			{
				List<SYUserGetByRoleIdAllOnPage> rsSYUserGetByRoleIdAllOnPage = await new SYUserGetByRoleIdAllOnPage(_appSetting).SYUserGetByRoleIdAllOnPageDAO(PageSize, PageIndex, RoleId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetByRoleIdAllOnPage", rsSYUserGetByRoleIdAllOnPage},
						{"TotalCount", rsSYUserGetByRoleIdAllOnPage != null && rsSYUserGetByRoleIdAllOnPage.Count > 0 ? rsSYUserGetByRoleIdAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserGetByRoleIdAllOnPage != null && rsSYUserGetByRoleIdAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserGetByRoleIdAllOnPage != null && rsSYUserGetByRoleIdAllOnPage.Count > 0 ? PageSize : 0},
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

		[HttpGet]
		[Authorize]
		[Route("SYUserGetIsSystemBase")]
		public async Task<ActionResult<object>> SYUserGetIsSystemBase()
		{
			try
			{
				List<SYUserGetIsSystem> rsSYUserGetIsSystem = await new SYUserGetIsSystem(_appSetting).SYUserGetIsSystemDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetIsSystem", rsSYUserGetIsSystem},
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

		[HttpGet]
		[Authorize]
		[Route("SYUserGetNameByIdBase")]
		public async Task<ActionResult<object>> SYUserGetNameByIdBase(long? Id)
		{
			try
			{
				List<SYUserGetNameById> rsSYUserGetNameById = await new SYUserGetNameById(_appSetting).SYUserGetNameByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGetNameById", rsSYUserGetNameById},
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

		[HttpPost]
		[Authorize]
		[Route("SYUserInsertBase")]
		public async Task<ActionResult<object>> SYUserInsertBase(SYUserInsertIN _sYUserInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserInsert(_appSetting).SYUserInsertDAO(_sYUserInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserInsertListBase")]
		public async Task<ActionResult<object>> SYUserInsertListBase(List<SYUserInsertIN> _sYUserInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserInsertIN in _sYUserInsertINs)
				{
					var result = await new SYUserInsert(_appSetting).SYUserInsertDAO(_sYUserInsertIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserRoleMapDeleteBase")]
		public async Task<ActionResult<object>> SYUserRoleMapDeleteBase(SYUserRoleMapDeleteIN _sYUserRoleMapDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMapDelete(_appSetting).SYUserRoleMapDeleteDAO(_sYUserRoleMapDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserRoleMapDeleteListBase")]
		public async Task<ActionResult<object>> SYUserRoleMapDeleteListBase(List<SYUserRoleMapDeleteIN> _sYUserRoleMapDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserRoleMapDeleteIN in _sYUserRoleMapDeleteINs)
				{
					var result = await new SYUserRoleMapDelete(_appSetting).SYUserRoleMapDeleteDAO(_sYUserRoleMapDeleteIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserUpdateBase")]
		public async Task<ActionResult<object>> SYUserUpdateBase(SYUserUpdateIN _sYUserUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(_sYUserUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SYUserUpdateListBase")]
		public async Task<ActionResult<object>> SYUserUpdateListBase(List<SYUserUpdateIN> _sYUserUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserUpdateIN in _sYUserUpdateINs)
				{
					var result = await new SYUserUpdate(_appSetting).SYUserUpdateDAO(_sYUserUpdateIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYUserUpdateInfoBase")]
		public async Task<ActionResult<object>> SYUserUpdateInfoBase(SYUserUpdateInfoIN _sYUserUpdateInfoIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(_sYUserUpdateInfoIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("SYUserUpdateInfoListBase")]
		public async Task<ActionResult<object>> SYUserUpdateInfoListBase(List<SYUserUpdateInfoIN> _sYUserUpdateInfoINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _sYUserUpdateInfoIN in _sYUserUpdateInfoINs)
				{
					var result = await new SYUserUpdateInfo(_appSetting).SYUserUpdateInfoDAO(_sYUserUpdateInfoIN);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
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
		[Route("SYUSRGetPermissionByUserIdBase")]
		public async Task<ActionResult<object>> SYUSRGetPermissionByUserIdBase(long? UserId)
		{
			try
			{
				List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId = await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUSRGetPermissionByUserId", rsSYUSRGetPermissionByUserId},
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
