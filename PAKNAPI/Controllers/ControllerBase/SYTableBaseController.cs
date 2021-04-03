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
	[Route("api/SYTableBase")]
	[ApiController]
	public class SYTableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public SYTableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region SYCaptCha

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYCaptChaGetByID")]
		public async Task<ActionResult<object>> SYCaptChaGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptCha(_appSetting).SYCaptChaGetByID(Id) };
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
		[Route("SYCaptChaGetAll")]
		public async Task<ActionResult<object>> SYCaptChaGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptCha(_appSetting).SYCaptChaGetAll() };
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
		[Route("SYCaptChaGetAllOnPage")]
		public async Task<ActionResult<object>> SYCaptChaGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYCaptChaOnPage> rsSYCaptChaOnPage = await new SYCaptCha(_appSetting).SYCaptChaGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYCaptCha", rsSYCaptChaOnPage},
						{"TotalCount", rsSYCaptChaOnPage != null && rsSYCaptChaOnPage.Count > 0 ? rsSYCaptChaOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYCaptChaOnPage != null && rsSYCaptChaOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYCaptChaOnPage != null && rsSYCaptChaOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYCaptChaInsert")]
		public async Task<ActionResult<object>> SYCaptChaInsert(SYCaptCha _sYCaptCha)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptCha(_appSetting).SYCaptChaInsert(_sYCaptCha) };
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
		[Route("SYCaptChaListInsert")]
		public async Task<ActionResult<object>> SYCaptChaListInsert(List<SYCaptCha> _sYCaptChas)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYCaptCha _sYCaptCha in _sYCaptChas)
				{
					int? result = await new SYCaptCha(_appSetting).SYCaptChaInsert(_sYCaptCha);
					if (result != null)
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
		[Route("SYCaptChaUpdate")]
		public async Task<ActionResult<object>> SYCaptChaUpdate(SYCaptCha _sYCaptCha)
		{
			try
			{
				int count = await new SYCaptCha(_appSetting).SYCaptChaUpdate(_sYCaptCha);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYCaptChaDelete")]
		public async Task<ActionResult<object>> SYCaptChaDelete(SYCaptCha _sYCaptCha)
		{
			try
			{
				int count = await new SYCaptCha(_appSetting).SYCaptChaDelete(_sYCaptCha);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYCaptChaListDelete")]
		public async Task<ActionResult<object>> SYCaptChaListDelete(List<SYCaptCha> _sYCaptChas)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYCaptCha _sYCaptCha in _sYCaptChas)
				{
					var result = await new SYCaptCha(_appSetting).SYCaptChaDelete(_sYCaptCha);
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
		[Route("SYCaptChaDeleteAll")]
		public async Task<ActionResult<object>> SYCaptChaDeleteAll()
		{
			try
			{
				int count = await new SYCaptCha(_appSetting).SYCaptChaDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYCaptChaCount")]
		public async Task<ActionResult<object>> SYCaptChaCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYCaptCha(_appSetting).SYCaptChaCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYCaptCha

		#region SYGroupUser

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYGroupUserGetByID")]
		public async Task<ActionResult<object>> SYGroupUserGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYGroupUser(_appSetting).SYGroupUserGetByID(Id) };
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
		[Route("SYGroupUserGetAll")]
		public async Task<ActionResult<object>> SYGroupUserGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYGroupUser(_appSetting).SYGroupUserGetAll() };
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
		[Route("SYGroupUserGetAllOnPage")]
		public async Task<ActionResult<object>> SYGroupUserGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYGroupUserOnPage> rsSYGroupUserOnPage = await new SYGroupUser(_appSetting).SYGroupUserGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYGroupUser", rsSYGroupUserOnPage},
						{"TotalCount", rsSYGroupUserOnPage != null && rsSYGroupUserOnPage.Count > 0 ? rsSYGroupUserOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYGroupUserOnPage != null && rsSYGroupUserOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYGroupUserOnPage != null && rsSYGroupUserOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYGroupUserInsert")]
		public async Task<ActionResult<object>> SYGroupUserInsert(SYGroupUser _sYGroupUser)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYGroupUser(_appSetting).SYGroupUserInsert(_sYGroupUser) };
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
		[Route("SYGroupUserListInsert")]
		public async Task<ActionResult<object>> SYGroupUserListInsert(List<SYGroupUser> _sYGroupUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYGroupUser _sYGroupUser in _sYGroupUsers)
				{
					int? result = await new SYGroupUser(_appSetting).SYGroupUserInsert(_sYGroupUser);
					if (result != null)
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
		[Route("SYGroupUserUpdate")]
		public async Task<ActionResult<object>> SYGroupUserUpdate(SYGroupUser _sYGroupUser)
		{
			try
			{
				int count = await new SYGroupUser(_appSetting).SYGroupUserUpdate(_sYGroupUser);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYGroupUserDelete")]
		public async Task<ActionResult<object>> SYGroupUserDelete(SYGroupUser _sYGroupUser)
		{
			try
			{
				int count = await new SYGroupUser(_appSetting).SYGroupUserDelete(_sYGroupUser);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYGroupUserListDelete")]
		public async Task<ActionResult<object>> SYGroupUserListDelete(List<SYGroupUser> _sYGroupUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYGroupUser _sYGroupUser in _sYGroupUsers)
				{
					var result = await new SYGroupUser(_appSetting).SYGroupUserDelete(_sYGroupUser);
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
		[Route("SYGroupUserDeleteAll")]
		public async Task<ActionResult<object>> SYGroupUserDeleteAll()
		{
			try
			{
				int count = await new SYGroupUser(_appSetting).SYGroupUserDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYGroupUserCount")]
		public async Task<ActionResult<object>> SYGroupUserCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYGroupUser(_appSetting).SYGroupUserCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYGroupUser

		#region SYPermission

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYPermissionGetByID")]
		public async Task<ActionResult<object>> SYPermissionGetByID(short? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermission(_appSetting).SYPermissionGetByID(Id) };
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
		[Route("SYPermissionGetAll")]
		public async Task<ActionResult<object>> SYPermissionGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermission(_appSetting).SYPermissionGetAll() };
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
		[Route("SYPermissionGetAllOnPage")]
		public async Task<ActionResult<object>> SYPermissionGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYPermissionOnPage> rsSYPermissionOnPage = await new SYPermission(_appSetting).SYPermissionGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermission", rsSYPermissionOnPage},
						{"TotalCount", rsSYPermissionOnPage != null && rsSYPermissionOnPage.Count > 0 ? rsSYPermissionOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYPermissionOnPage != null && rsSYPermissionOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYPermissionOnPage != null && rsSYPermissionOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYPermissionInsert")]
		public async Task<ActionResult<object>> SYPermissionInsert(SYPermission _sYPermission)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermission(_appSetting).SYPermissionInsert(_sYPermission) };
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
		[Route("SYPermissionListInsert")]
		public async Task<ActionResult<object>> SYPermissionListInsert(List<SYPermission> _sYPermissions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermission _sYPermission in _sYPermissions)
				{
					int? result = await new SYPermission(_appSetting).SYPermissionInsert(_sYPermission);
					if (result != null)
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
		[Route("SYPermissionUpdate")]
		public async Task<ActionResult<object>> SYPermissionUpdate(SYPermission _sYPermission)
		{
			try
			{
				int count = await new SYPermission(_appSetting).SYPermissionUpdate(_sYPermission);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionDelete")]
		public async Task<ActionResult<object>> SYPermissionDelete(SYPermission _sYPermission)
		{
			try
			{
				int count = await new SYPermission(_appSetting).SYPermissionDelete(_sYPermission);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionListDelete")]
		public async Task<ActionResult<object>> SYPermissionListDelete(List<SYPermission> _sYPermissions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermission _sYPermission in _sYPermissions)
				{
					var result = await new SYPermission(_appSetting).SYPermissionDelete(_sYPermission);
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
		[Route("SYPermissionDeleteAll")]
		public async Task<ActionResult<object>> SYPermissionDeleteAll()
		{
			try
			{
				int count = await new SYPermission(_appSetting).SYPermissionDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionCount")]
		public async Task<ActionResult<object>> SYPermissionCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermission(_appSetting).SYPermissionCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYPermission

		#region SYPermissionCategory

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYPermissionCategoryGetByID")]
		public async Task<ActionResult<object>> SYPermissionCategoryGetByID(short? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionCategory(_appSetting).SYPermissionCategoryGetByID(Id) };
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
		[Route("SYPermissionCategoryGetAll")]
		public async Task<ActionResult<object>> SYPermissionCategoryGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionCategory(_appSetting).SYPermissionCategoryGetAll() };
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
		[Route("SYPermissionCategoryGetAllOnPage")]
		public async Task<ActionResult<object>> SYPermissionCategoryGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYPermissionCategoryOnPage> rsSYPermissionCategoryOnPage = await new SYPermissionCategory(_appSetting).SYPermissionCategoryGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionCategory", rsSYPermissionCategoryOnPage},
						{"TotalCount", rsSYPermissionCategoryOnPage != null && rsSYPermissionCategoryOnPage.Count > 0 ? rsSYPermissionCategoryOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYPermissionCategoryOnPage != null && rsSYPermissionCategoryOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYPermissionCategoryOnPage != null && rsSYPermissionCategoryOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYPermissionCategoryInsert")]
		public async Task<ActionResult<object>> SYPermissionCategoryInsert(SYPermissionCategory _sYPermissionCategory)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionCategory(_appSetting).SYPermissionCategoryInsert(_sYPermissionCategory) };
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
		[Route("SYPermissionCategoryListInsert")]
		public async Task<ActionResult<object>> SYPermissionCategoryListInsert(List<SYPermissionCategory> _sYPermissionCategorys)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionCategory _sYPermissionCategory in _sYPermissionCategorys)
				{
					int? result = await new SYPermissionCategory(_appSetting).SYPermissionCategoryInsert(_sYPermissionCategory);
					if (result != null)
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
		[Route("SYPermissionCategoryUpdate")]
		public async Task<ActionResult<object>> SYPermissionCategoryUpdate(SYPermissionCategory _sYPermissionCategory)
		{
			try
			{
				int count = await new SYPermissionCategory(_appSetting).SYPermissionCategoryUpdate(_sYPermissionCategory);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionCategoryDelete")]
		public async Task<ActionResult<object>> SYPermissionCategoryDelete(SYPermissionCategory _sYPermissionCategory)
		{
			try
			{
				int count = await new SYPermissionCategory(_appSetting).SYPermissionCategoryDelete(_sYPermissionCategory);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionCategoryListDelete")]
		public async Task<ActionResult<object>> SYPermissionCategoryListDelete(List<SYPermissionCategory> _sYPermissionCategorys)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionCategory _sYPermissionCategory in _sYPermissionCategorys)
				{
					var result = await new SYPermissionCategory(_appSetting).SYPermissionCategoryDelete(_sYPermissionCategory);
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
		[Route("SYPermissionCategoryDeleteAll")]
		public async Task<ActionResult<object>> SYPermissionCategoryDeleteAll()
		{
			try
			{
				int count = await new SYPermissionCategory(_appSetting).SYPermissionCategoryDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionCategoryCount")]
		public async Task<ActionResult<object>> SYPermissionCategoryCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionCategory(_appSetting).SYPermissionCategoryCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYPermissionCategory

		#region SYPermissionFunction

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYPermissionFunctionGetByID")]
		public async Task<ActionResult<object>> SYPermissionFunctionGetByID(short? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionFunction(_appSetting).SYPermissionFunctionGetByID(Id) };
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
		[Route("SYPermissionFunctionGetAll")]
		public async Task<ActionResult<object>> SYPermissionFunctionGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionFunction(_appSetting).SYPermissionFunctionGetAll() };
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
		[Route("SYPermissionFunctionGetAllOnPage")]
		public async Task<ActionResult<object>> SYPermissionFunctionGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYPermissionFunctionOnPage> rsSYPermissionFunctionOnPage = await new SYPermissionFunction(_appSetting).SYPermissionFunctionGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionFunction", rsSYPermissionFunctionOnPage},
						{"TotalCount", rsSYPermissionFunctionOnPage != null && rsSYPermissionFunctionOnPage.Count > 0 ? rsSYPermissionFunctionOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYPermissionFunctionOnPage != null && rsSYPermissionFunctionOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYPermissionFunctionOnPage != null && rsSYPermissionFunctionOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYPermissionFunctionInsert")]
		public async Task<ActionResult<object>> SYPermissionFunctionInsert(SYPermissionFunction _sYPermissionFunction)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionFunction(_appSetting).SYPermissionFunctionInsert(_sYPermissionFunction) };
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
		[Route("SYPermissionFunctionListInsert")]
		public async Task<ActionResult<object>> SYPermissionFunctionListInsert(List<SYPermissionFunction> _sYPermissionFunctions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionFunction _sYPermissionFunction in _sYPermissionFunctions)
				{
					int? result = await new SYPermissionFunction(_appSetting).SYPermissionFunctionInsert(_sYPermissionFunction);
					if (result != null)
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
		[Route("SYPermissionFunctionUpdate")]
		public async Task<ActionResult<object>> SYPermissionFunctionUpdate(SYPermissionFunction _sYPermissionFunction)
		{
			try
			{
				int count = await new SYPermissionFunction(_appSetting).SYPermissionFunctionUpdate(_sYPermissionFunction);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionFunctionDelete")]
		public async Task<ActionResult<object>> SYPermissionFunctionDelete(SYPermissionFunction _sYPermissionFunction)
		{
			try
			{
				int count = await new SYPermissionFunction(_appSetting).SYPermissionFunctionDelete(_sYPermissionFunction);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionFunctionListDelete")]
		public async Task<ActionResult<object>> SYPermissionFunctionListDelete(List<SYPermissionFunction> _sYPermissionFunctions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionFunction _sYPermissionFunction in _sYPermissionFunctions)
				{
					var result = await new SYPermissionFunction(_appSetting).SYPermissionFunctionDelete(_sYPermissionFunction);
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
		[Route("SYPermissionFunctionDeleteAll")]
		public async Task<ActionResult<object>> SYPermissionFunctionDeleteAll()
		{
			try
			{
				int count = await new SYPermissionFunction(_appSetting).SYPermissionFunctionDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionFunctionCount")]
		public async Task<ActionResult<object>> SYPermissionFunctionCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionFunction(_appSetting).SYPermissionFunctionCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYPermissionFunction

		#region SYPermissionGroupUser

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYPermissionGroupUserGetByID")]
		public async Task<ActionResult<object>> SYPermissionGroupUserGetByID(short? PermissionId)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserGetByID(PermissionId) };
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
		[Route("SYPermissionGroupUserGetAll")]
		public async Task<ActionResult<object>> SYPermissionGroupUserGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserGetAll() };
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
		[Route("SYPermissionGroupUserGetAllOnPage")]
		public async Task<ActionResult<object>> SYPermissionGroupUserGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYPermissionGroupUserOnPage> rsSYPermissionGroupUserOnPage = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionGroupUser", rsSYPermissionGroupUserOnPage},
						{"TotalCount", rsSYPermissionGroupUserOnPage != null && rsSYPermissionGroupUserOnPage.Count > 0 ? rsSYPermissionGroupUserOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYPermissionGroupUserOnPage != null && rsSYPermissionGroupUserOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYPermissionGroupUserOnPage != null && rsSYPermissionGroupUserOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYPermissionGroupUserInsert")]
		public async Task<ActionResult<object>> SYPermissionGroupUserInsert(SYPermissionGroupUser _sYPermissionGroupUser)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserInsert(_sYPermissionGroupUser) };
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
		[Route("SYPermissionGroupUserListInsert")]
		public async Task<ActionResult<object>> SYPermissionGroupUserListInsert(List<SYPermissionGroupUser> _sYPermissionGroupUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionGroupUser _sYPermissionGroupUser in _sYPermissionGroupUsers)
				{
					int? result = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserInsert(_sYPermissionGroupUser);
					if (result != null)
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
		[Route("SYPermissionGroupUserUpdate")]
		public async Task<ActionResult<object>> SYPermissionGroupUserUpdate(SYPermissionGroupUser _sYPermissionGroupUser)
		{
			try
			{
				int count = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserUpdate(_sYPermissionGroupUser);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionGroupUserDelete")]
		public async Task<ActionResult<object>> SYPermissionGroupUserDelete(SYPermissionGroupUser _sYPermissionGroupUser)
		{
			try
			{
				int count = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserDelete(_sYPermissionGroupUser);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionGroupUserListDelete")]
		public async Task<ActionResult<object>> SYPermissionGroupUserListDelete(List<SYPermissionGroupUser> _sYPermissionGroupUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionGroupUser _sYPermissionGroupUser in _sYPermissionGroupUsers)
				{
					var result = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserDelete(_sYPermissionGroupUser);
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
		[Route("SYPermissionGroupUserDeleteAll")]
		public async Task<ActionResult<object>> SYPermissionGroupUserDeleteAll()
		{
			try
			{
				int count = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionGroupUserCount")]
		public async Task<ActionResult<object>> SYPermissionGroupUserCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionGroupUser(_appSetting).SYPermissionGroupUserCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYPermissionGroupUser

		#region SYPermissionUser

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYPermissionUserGetByID")]
		public async Task<ActionResult<object>> SYPermissionUserGetByID(long? UserId)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionUser(_appSetting).SYPermissionUserGetByID(UserId) };
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
		[Route("SYPermissionUserGetAll")]
		public async Task<ActionResult<object>> SYPermissionUserGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionUser(_appSetting).SYPermissionUserGetAll() };
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
		[Route("SYPermissionUserGetAllOnPage")]
		public async Task<ActionResult<object>> SYPermissionUserGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYPermissionUserOnPage> rsSYPermissionUserOnPage = await new SYPermissionUser(_appSetting).SYPermissionUserGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionUser", rsSYPermissionUserOnPage},
						{"TotalCount", rsSYPermissionUserOnPage != null && rsSYPermissionUserOnPage.Count > 0 ? rsSYPermissionUserOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYPermissionUserOnPage != null && rsSYPermissionUserOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYPermissionUserOnPage != null && rsSYPermissionUserOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYPermissionUserInsert")]
		public async Task<ActionResult<object>> SYPermissionUserInsert(SYPermissionUser _sYPermissionUser)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionUser(_appSetting).SYPermissionUserInsert(_sYPermissionUser) };
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
		[Route("SYPermissionUserListInsert")]
		public async Task<ActionResult<object>> SYPermissionUserListInsert(List<SYPermissionUser> _sYPermissionUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionUser _sYPermissionUser in _sYPermissionUsers)
				{
					int? result = await new SYPermissionUser(_appSetting).SYPermissionUserInsert(_sYPermissionUser);
					if (result != null)
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
		[Route("SYPermissionUserUpdate")]
		public async Task<ActionResult<object>> SYPermissionUserUpdate(SYPermissionUser _sYPermissionUser)
		{
			try
			{
				int count = await new SYPermissionUser(_appSetting).SYPermissionUserUpdate(_sYPermissionUser);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionUserDelete")]
		public async Task<ActionResult<object>> SYPermissionUserDelete(SYPermissionUser _sYPermissionUser)
		{
			try
			{
				int count = await new SYPermissionUser(_appSetting).SYPermissionUserDelete(_sYPermissionUser);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionUserListDelete")]
		public async Task<ActionResult<object>> SYPermissionUserListDelete(List<SYPermissionUser> _sYPermissionUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionUser _sYPermissionUser in _sYPermissionUsers)
				{
					var result = await new SYPermissionUser(_appSetting).SYPermissionUserDelete(_sYPermissionUser);
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
		[Route("SYPermissionUserDeleteAll")]
		public async Task<ActionResult<object>> SYPermissionUserDeleteAll()
		{
			try
			{
				int count = await new SYPermissionUser(_appSetting).SYPermissionUserDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYPermissionUserCount")]
		public async Task<ActionResult<object>> SYPermissionUserCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionUser(_appSetting).SYPermissionUserCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYPermissionUser

		#region SYRole

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYRoleGetByID")]
		public async Task<ActionResult<object>> SYRoleGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYRole(_appSetting).SYRoleGetByID(Id) };
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
		[Route("SYRoleGetAll")]
		public async Task<ActionResult<object>> SYRoleGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYRole(_appSetting).SYRoleGetAll() };
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
		[Route("SYRoleGetAllOnPage")]
		public async Task<ActionResult<object>> SYRoleGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYRoleOnPage> rsSYRoleOnPage = await new SYRole(_appSetting).SYRoleGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYRole", rsSYRoleOnPage},
						{"TotalCount", rsSYRoleOnPage != null && rsSYRoleOnPage.Count > 0 ? rsSYRoleOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYRoleOnPage != null && rsSYRoleOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYRoleOnPage != null && rsSYRoleOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYRoleInsert")]
		public async Task<ActionResult<object>> SYRoleInsert(SYRole _sYRole)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYRole(_appSetting).SYRoleInsert(_sYRole) };
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
		[Route("SYRoleListInsert")]
		public async Task<ActionResult<object>> SYRoleListInsert(List<SYRole> _sYRoles)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYRole _sYRole in _sYRoles)
				{
					int? result = await new SYRole(_appSetting).SYRoleInsert(_sYRole);
					if (result != null)
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
		[Route("SYRoleUpdate")]
		public async Task<ActionResult<object>> SYRoleUpdate(SYRole _sYRole)
		{
			try
			{
				int count = await new SYRole(_appSetting).SYRoleUpdate(_sYRole);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYRoleDelete")]
		public async Task<ActionResult<object>> SYRoleDelete(SYRole _sYRole)
		{
			try
			{
				int count = await new SYRole(_appSetting).SYRoleDelete(_sYRole);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYRoleListDelete")]
		public async Task<ActionResult<object>> SYRoleListDelete(List<SYRole> _sYRoles)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYRole _sYRole in _sYRoles)
				{
					var result = await new SYRole(_appSetting).SYRoleDelete(_sYRole);
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
		[Route("SYRoleDeleteAll")]
		public async Task<ActionResult<object>> SYRoleDeleteAll()
		{
			try
			{
				int count = await new SYRole(_appSetting).SYRoleDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYRoleCount")]
		public async Task<ActionResult<object>> SYRoleCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYRole(_appSetting).SYRoleCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYRole

		#region SYSystemLog

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYSystemLogGetByID")]
		public async Task<ActionResult<object>> SYSystemLogGetByID(long? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYSystemLog(_appSetting).SYSystemLogGetByID(Id) };
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
		[Route("SYSystemLogGetAll")]
		public async Task<ActionResult<object>> SYSystemLogGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYSystemLog(_appSetting).SYSystemLogGetAll() };
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
		[Route("SYSystemLogGetAllOnPage")]
		public async Task<ActionResult<object>> SYSystemLogGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYSystemLogOnPage> rsSYSystemLogOnPage = await new SYSystemLog(_appSetting).SYSystemLogGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYSystemLog", rsSYSystemLogOnPage},
						{"TotalCount", rsSYSystemLogOnPage != null && rsSYSystemLogOnPage.Count > 0 ? rsSYSystemLogOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYSystemLogOnPage != null && rsSYSystemLogOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYSystemLogOnPage != null && rsSYSystemLogOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYSystemLogInsert")]
		public async Task<ActionResult<object>> SYSystemLogInsert(SYSystemLog _sYSystemLog)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYSystemLog(_appSetting).SYSystemLogInsert(_sYSystemLog) };
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
		[Route("SYSystemLogListInsert")]
		public async Task<ActionResult<object>> SYSystemLogListInsert(List<SYSystemLog> _sYSystemLogs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYSystemLog _sYSystemLog in _sYSystemLogs)
				{
					int? result = await new SYSystemLog(_appSetting).SYSystemLogInsert(_sYSystemLog);
					if (result != null)
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
		[Route("SYSystemLogUpdate")]
		public async Task<ActionResult<object>> SYSystemLogUpdate(SYSystemLog _sYSystemLog)
		{
			try
			{
				int count = await new SYSystemLog(_appSetting).SYSystemLogUpdate(_sYSystemLog);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYSystemLogDelete")]
		public async Task<ActionResult<object>> SYSystemLogDelete(SYSystemLog _sYSystemLog)
		{
			try
			{
				int count = await new SYSystemLog(_appSetting).SYSystemLogDelete(_sYSystemLog);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYSystemLogListDelete")]
		public async Task<ActionResult<object>> SYSystemLogListDelete(List<SYSystemLog> _sYSystemLogs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYSystemLog _sYSystemLog in _sYSystemLogs)
				{
					var result = await new SYSystemLog(_appSetting).SYSystemLogDelete(_sYSystemLog);
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
		[Route("SYSystemLogDeleteAll")]
		public async Task<ActionResult<object>> SYSystemLogDeleteAll()
		{
			try
			{
				int count = await new SYSystemLog(_appSetting).SYSystemLogDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYSystemLogCount")]
		public async Task<ActionResult<object>> SYSystemLogCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYSystemLog(_appSetting).SYSystemLogCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYSystemLog

		#region SYUnit

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUnitGetByID")]
		public async Task<ActionResult<object>> SYUnitGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUnit(_appSetting).SYUnitGetByID(Id) };
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
		[Route("SYUnitGetAll")]
		public async Task<ActionResult<object>> SYUnitGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUnit(_appSetting).SYUnitGetAll() };
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
		[Route("SYUnitGetAllOnPage")]
		public async Task<ActionResult<object>> SYUnitGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYUnitOnPage> rsSYUnitOnPage = await new SYUnit(_appSetting).SYUnitGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUnit", rsSYUnitOnPage},
						{"TotalCount", rsSYUnitOnPage != null && rsSYUnitOnPage.Count > 0 ? rsSYUnitOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUnitOnPage != null && rsSYUnitOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUnitOnPage != null && rsSYUnitOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYUnitInsert")]
		public async Task<ActionResult<object>> SYUnitInsert(SYUnit _sYUnit)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUnit(_appSetting).SYUnitInsert(_sYUnit) };
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
		[Route("SYUnitListInsert")]
		public async Task<ActionResult<object>> SYUnitListInsert(List<SYUnit> _sYUnits)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUnit _sYUnit in _sYUnits)
				{
					int? result = await new SYUnit(_appSetting).SYUnitInsert(_sYUnit);
					if (result != null)
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
		[Route("SYUnitUpdate")]
		public async Task<ActionResult<object>> SYUnitUpdate(SYUnit _sYUnit)
		{
			try
			{
				int count = await new SYUnit(_appSetting).SYUnitUpdate(_sYUnit);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUnitDelete")]
		public async Task<ActionResult<object>> SYUnitDelete(SYUnit _sYUnit)
		{
			try
			{
				int count = await new SYUnit(_appSetting).SYUnitDelete(_sYUnit);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUnitListDelete")]
		public async Task<ActionResult<object>> SYUnitListDelete(List<SYUnit> _sYUnits)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUnit _sYUnit in _sYUnits)
				{
					var result = await new SYUnit(_appSetting).SYUnitDelete(_sYUnit);
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
		[Route("SYUnitDeleteAll")]
		public async Task<ActionResult<object>> SYUnitDeleteAll()
		{
			try
			{
				int count = await new SYUnit(_appSetting).SYUnitDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUnitCount")]
		public async Task<ActionResult<object>> SYUnitCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUnit(_appSetting).SYUnitCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYUnit

		#region SYUser

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserGetByID")]
		public async Task<ActionResult<object>> SYUserGetByID(long? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUser(_appSetting).SYUserGetByID(Id) };
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
		[Route("SYUserGetAll")]
		public async Task<ActionResult<object>> SYUserGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUser(_appSetting).SYUserGetAll() };
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
		[Route("SYUserGetAllOnPage")]
		public async Task<ActionResult<object>> SYUserGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYUserOnPage> rsSYUserOnPage = await new SYUser(_appSetting).SYUserGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUser", rsSYUserOnPage},
						{"TotalCount", rsSYUserOnPage != null && rsSYUserOnPage.Count > 0 ? rsSYUserOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserOnPage != null && rsSYUserOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserOnPage != null && rsSYUserOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYUserInsert")]
		public async Task<ActionResult<object>> SYUserInsert(SYUser _sYUser)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUser(_appSetting).SYUserInsert(_sYUser) };
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
		[Route("SYUserListInsert")]
		public async Task<ActionResult<object>> SYUserListInsert(List<SYUser> _sYUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUser _sYUser in _sYUsers)
				{
					int? result = await new SYUser(_appSetting).SYUserInsert(_sYUser);
					if (result != null)
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
		[Route("SYUserUpdate")]
		public async Task<ActionResult<object>> SYUserUpdate(SYUser _sYUser)
		{
			try
			{
				int count = await new SYUser(_appSetting).SYUserUpdate(_sYUser);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserDelete")]
		public async Task<ActionResult<object>> SYUserDelete(SYUser _sYUser)
		{
			try
			{
				int count = await new SYUser(_appSetting).SYUserDelete(_sYUser);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserListDelete")]
		public async Task<ActionResult<object>> SYUserListDelete(List<SYUser> _sYUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUser _sYUser in _sYUsers)
				{
					var result = await new SYUser(_appSetting).SYUserDelete(_sYUser);
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
		[Route("SYUserDeleteAll")]
		public async Task<ActionResult<object>> SYUserDeleteAll()
		{
			try
			{
				int count = await new SYUser(_appSetting).SYUserDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserCount")]
		public async Task<ActionResult<object>> SYUserCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUser(_appSetting).SYUserCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYUser

		#region SYUserRoleMap

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserRoleMapGetByID")]
		public async Task<ActionResult<object>> SYUserRoleMapGetByID(int? UserId)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMap(_appSetting).SYUserRoleMapGetByID(UserId) };
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
		[Route("SYUserRoleMapGetAll")]
		public async Task<ActionResult<object>> SYUserRoleMapGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMap(_appSetting).SYUserRoleMapGetAll() };
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
		[Route("SYUserRoleMapGetAllOnPage")]
		public async Task<ActionResult<object>> SYUserRoleMapGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYUserRoleMapOnPage> rsSYUserRoleMapOnPage = await new SYUserRoleMap(_appSetting).SYUserRoleMapGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserRoleMap", rsSYUserRoleMapOnPage},
						{"TotalCount", rsSYUserRoleMapOnPage != null && rsSYUserRoleMapOnPage.Count > 0 ? rsSYUserRoleMapOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserRoleMapOnPage != null && rsSYUserRoleMapOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserRoleMapOnPage != null && rsSYUserRoleMapOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYUserRoleMapInsert")]
		public async Task<ActionResult<object>> SYUserRoleMapInsert(SYUserRoleMap _sYUserRoleMap)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMap(_appSetting).SYUserRoleMapInsert(_sYUserRoleMap) };
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
		[Route("SYUserRoleMapListInsert")]
		public async Task<ActionResult<object>> SYUserRoleMapListInsert(List<SYUserRoleMap> _sYUserRoleMaps)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserRoleMap _sYUserRoleMap in _sYUserRoleMaps)
				{
					int? result = await new SYUserRoleMap(_appSetting).SYUserRoleMapInsert(_sYUserRoleMap);
					if (result != null)
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
		[Route("SYUserRoleMapUpdate")]
		public async Task<ActionResult<object>> SYUserRoleMapUpdate(SYUserRoleMap _sYUserRoleMap)
		{
			try
			{
				int count = await new SYUserRoleMap(_appSetting).SYUserRoleMapUpdate(_sYUserRoleMap);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserRoleMapDelete")]
		public async Task<ActionResult<object>> SYUserRoleMapDelete(SYUserRoleMap _sYUserRoleMap)
		{
			try
			{
				int count = await new SYUserRoleMap(_appSetting).SYUserRoleMapDelete(_sYUserRoleMap);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserRoleMapListDelete")]
		public async Task<ActionResult<object>> SYUserRoleMapListDelete(List<SYUserRoleMap> _sYUserRoleMaps)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserRoleMap _sYUserRoleMap in _sYUserRoleMaps)
				{
					var result = await new SYUserRoleMap(_appSetting).SYUserRoleMapDelete(_sYUserRoleMap);
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
		[Route("SYUserRoleMapDeleteAll")]
		public async Task<ActionResult<object>> SYUserRoleMapDeleteAll()
		{
			try
			{
				int count = await new SYUserRoleMap(_appSetting).SYUserRoleMapDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserRoleMapCount")]
		public async Task<ActionResult<object>> SYUserRoleMapCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserRoleMap(_appSetting).SYUserRoleMapCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYUserRoleMap

		#region SYUserGroupUser

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserGroupUserGetByID")]
		public async Task<ActionResult<object>> SYUserGroupUserGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserGroupUser(_appSetting).SYUserGroupUserGetByID(Id) };
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
		[Route("SYUserGroupUserGetAll")]
		public async Task<ActionResult<object>> SYUserGroupUserGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserGroupUser(_appSetting).SYUserGroupUserGetAll() };
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
		[Route("SYUserGroupUserGetAllOnPage")]
		public async Task<ActionResult<object>> SYUserGroupUserGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYUserGroupUserOnPage> rsSYUserGroupUserOnPage = await new SYUserGroupUser(_appSetting).SYUserGroupUserGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserGroupUser", rsSYUserGroupUserOnPage},
						{"TotalCount", rsSYUserGroupUserOnPage != null && rsSYUserGroupUserOnPage.Count > 0 ? rsSYUserGroupUserOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserGroupUserOnPage != null && rsSYUserGroupUserOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserGroupUserOnPage != null && rsSYUserGroupUserOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYUserGroupUserInsert")]
		public async Task<ActionResult<object>> SYUserGroupUserInsert(SYUserGroupUser _sYUserGroupUser)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserGroupUser(_appSetting).SYUserGroupUserInsert(_sYUserGroupUser) };
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
		[Route("SYUserGroupUserListInsert")]
		public async Task<ActionResult<object>> SYUserGroupUserListInsert(List<SYUserGroupUser> _sYUserGroupUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserGroupUser _sYUserGroupUser in _sYUserGroupUsers)
				{
					int? result = await new SYUserGroupUser(_appSetting).SYUserGroupUserInsert(_sYUserGroupUser);
					if (result != null)
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
		[Route("SYUserGroupUserUpdate")]
		public async Task<ActionResult<object>> SYUserGroupUserUpdate(SYUserGroupUser _sYUserGroupUser)
		{
			try
			{
				int count = await new SYUserGroupUser(_appSetting).SYUserGroupUserUpdate(_sYUserGroupUser);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserGroupUserDelete")]
		public async Task<ActionResult<object>> SYUserGroupUserDelete(SYUserGroupUser _sYUserGroupUser)
		{
			try
			{
				int count = await new SYUserGroupUser(_appSetting).SYUserGroupUserDelete(_sYUserGroupUser);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserGroupUserListDelete")]
		public async Task<ActionResult<object>> SYUserGroupUserListDelete(List<SYUserGroupUser> _sYUserGroupUsers)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserGroupUser _sYUserGroupUser in _sYUserGroupUsers)
				{
					var result = await new SYUserGroupUser(_appSetting).SYUserGroupUserDelete(_sYUserGroupUser);
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
		[Route("SYUserGroupUserDeleteAll")]
		public async Task<ActionResult<object>> SYUserGroupUserDeleteAll()
		{
			try
			{
				int count = await new SYUserGroupUser(_appSetting).SYUserGroupUserDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserGroupUserCount")]
		public async Task<ActionResult<object>> SYUserGroupUserCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserGroupUser(_appSetting).SYUserGroupUserCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYUserGroupUser

		#region SYUserUnit

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("SYUserUnitGetByID")]
		public async Task<ActionResult<object>> SYUserUnitGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUnit(_appSetting).SYUserUnitGetByID(Id) };
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
		[Route("SYUserUnitGetAll")]
		public async Task<ActionResult<object>> SYUserUnitGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUnit(_appSetting).SYUserUnitGetAll() };
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
		[Route("SYUserUnitGetAllOnPage")]
		public async Task<ActionResult<object>> SYUserUnitGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYUserUnitOnPage> rsSYUserUnitOnPage = await new SYUserUnit(_appSetting).SYUserUnitGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYUserUnit", rsSYUserUnitOnPage},
						{"TotalCount", rsSYUserUnitOnPage != null && rsSYUserUnitOnPage.Count > 0 ? rsSYUserUnitOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYUserUnitOnPage != null && rsSYUserUnitOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYUserUnitOnPage != null && rsSYUserUnitOnPage.Count > 0 ? PageSize : 0},
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
		[Authorize("ThePolicy")]
		[Route("SYUserUnitInsert")]
		public async Task<ActionResult<object>> SYUserUnitInsert(SYUserUnit _sYUserUnit)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUnit(_appSetting).SYUserUnitInsert(_sYUserUnit) };
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
		[Route("SYUserUnitListInsert")]
		public async Task<ActionResult<object>> SYUserUnitListInsert(List<SYUserUnit> _sYUserUnits)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserUnit _sYUserUnit in _sYUserUnits)
				{
					int? result = await new SYUserUnit(_appSetting).SYUserUnitInsert(_sYUserUnit);
					if (result != null)
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
		[Route("SYUserUnitUpdate")]
		public async Task<ActionResult<object>> SYUserUnitUpdate(SYUserUnit _sYUserUnit)
		{
			try
			{
				int count = await new SYUserUnit(_appSetting).SYUserUnitUpdate(_sYUserUnit);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserUnitDelete")]
		public async Task<ActionResult<object>> SYUserUnitDelete(SYUserUnit _sYUserUnit)
		{
			try
			{
				int count = await new SYUserUnit(_appSetting).SYUserUnitDelete(_sYUserUnit);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserUnitListDelete")]
		public async Task<ActionResult<object>> SYUserUnitListDelete(List<SYUserUnit> _sYUserUnits)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYUserUnit _sYUserUnit in _sYUserUnits)
				{
					var result = await new SYUserUnit(_appSetting).SYUserUnitDelete(_sYUserUnit);
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
		[Route("SYUserUnitDeleteAll")]
		public async Task<ActionResult<object>> SYUserUnitDeleteAll()
		{
			try
			{
				int count = await new SYUserUnit(_appSetting).SYUserUnitDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
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
		[Route("SYUserUnitCount")]
		public async Task<ActionResult<object>> SYUserUnitCount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYUserUnit(_appSetting).SYUserUnitCount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYUserUnit
	}
}
