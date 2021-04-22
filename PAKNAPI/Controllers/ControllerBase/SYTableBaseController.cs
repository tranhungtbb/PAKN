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

		#region SYAPI

		[HttpGet]
		[Authorize]
		[Route("SYAPIGetByID")]
		public async Task<ActionResult<object>> SYAPIGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYAPI(_appSetting).SYAPIGetByID(Id) };
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
		[Route("SYAPIGetAll")]
		public async Task<ActionResult<object>> SYAPIGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYAPI(_appSetting).SYAPIGetAll() };
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
		[Route("SYAPIGetAllOnPage")]
		public async Task<ActionResult<object>> SYAPIGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYAPIOnPage> rsSYAPIOnPage = await new SYAPI(_appSetting).SYAPIGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYAPI", rsSYAPIOnPage},
						{"TotalCount", rsSYAPIOnPage != null && rsSYAPIOnPage.Count > 0 ? rsSYAPIOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYAPIOnPage != null && rsSYAPIOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYAPIOnPage != null && rsSYAPIOnPage.Count > 0 ? PageSize : 0},
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
		[Route("SYAPIInsert")]
		public async Task<ActionResult<object>> SYAPIInsert(SYAPI _sYAPI)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYAPI(_appSetting).SYAPIInsert(_sYAPI) };
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
		[Route("SYAPIListInsert")]
		public async Task<ActionResult<object>> SYAPIListInsert(List<SYAPI> _sYAPIs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYAPI _sYAPI in _sYAPIs)
				{
					int? result = await new SYAPI(_appSetting).SYAPIInsert(_sYAPI);
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
		[Authorize]
		[Route("SYAPIUpdate")]
		public async Task<ActionResult<object>> SYAPIUpdate(SYAPI _sYAPI)
		{
			try
			{
				int count = await new SYAPI(_appSetting).SYAPIUpdate(_sYAPI);
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
		[Authorize]
		[Route("SYAPIDelete")]
		public async Task<ActionResult<object>> SYAPIDelete(SYAPI _sYAPI)
		{
			try
			{
				int count = await new SYAPI(_appSetting).SYAPIDelete(_sYAPI);
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
		[Authorize]
		[Route("SYAPIListDelete")]
		public async Task<ActionResult<object>> SYAPIListDelete(List<SYAPI> _sYAPIs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYAPI _sYAPI in _sYAPIs)
				{
					var result = await new SYAPI(_appSetting).SYAPIDelete(_sYAPI);
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
		[Route("SYAPIDeleteAll")]
		public async Task<ActionResult<object>> SYAPIDeleteAll()
		{
			try
			{
				int count = await new SYAPI(_appSetting).SYAPIDeleteAll();
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
		[Authorize]
		[Route("SYAPICount")]
		public async Task<ActionResult<object>> SYAPICount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYAPI(_appSetting).SYAPICount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYAPI

		#region SYCaptCha

		[HttpGet]
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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

		#region SYPermissionAPI

		[HttpGet]
		[Authorize]
		[Route("SYPermissionAPIGetByID")]
		public async Task<ActionResult<object>> SYPermissionAPIGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionAPI(_appSetting).SYPermissionAPIGetByID(Id) };
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
		[Route("SYPermissionAPIGetAll")]
		public async Task<ActionResult<object>> SYPermissionAPIGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionAPI(_appSetting).SYPermissionAPIGetAll() };
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
		[Route("SYPermissionAPIGetAllOnPage")]
		public async Task<ActionResult<object>> SYPermissionAPIGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<SYPermissionAPIOnPage> rsSYPermissionAPIOnPage = await new SYPermissionAPI(_appSetting).SYPermissionAPIGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SYPermissionAPI", rsSYPermissionAPIOnPage},
						{"TotalCount", rsSYPermissionAPIOnPage != null && rsSYPermissionAPIOnPage.Count > 0 ? rsSYPermissionAPIOnPage[0].RowNumber : 0},
						{"PageIndex", rsSYPermissionAPIOnPage != null && rsSYPermissionAPIOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSYPermissionAPIOnPage != null && rsSYPermissionAPIOnPage.Count > 0 ? PageSize : 0},
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
		[Route("SYPermissionAPIInsert")]
		public async Task<ActionResult<object>> SYPermissionAPIInsert(SYPermissionAPI _sYPermissionAPI)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionAPI(_appSetting).SYPermissionAPIInsert(_sYPermissionAPI) };
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
		[Route("SYPermissionAPIListInsert")]
		public async Task<ActionResult<object>> SYPermissionAPIListInsert(List<SYPermissionAPI> _sYPermissionAPIs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionAPI _sYPermissionAPI in _sYPermissionAPIs)
				{
					int? result = await new SYPermissionAPI(_appSetting).SYPermissionAPIInsert(_sYPermissionAPI);
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
		[Authorize]
		[Route("SYPermissionAPIUpdate")]
		public async Task<ActionResult<object>> SYPermissionAPIUpdate(SYPermissionAPI _sYPermissionAPI)
		{
			try
			{
				int count = await new SYPermissionAPI(_appSetting).SYPermissionAPIUpdate(_sYPermissionAPI);
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
		[Authorize]
		[Route("SYPermissionAPIDelete")]
		public async Task<ActionResult<object>> SYPermissionAPIDelete(SYPermissionAPI _sYPermissionAPI)
		{
			try
			{
				int count = await new SYPermissionAPI(_appSetting).SYPermissionAPIDelete(_sYPermissionAPI);
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
		[Authorize]
		[Route("SYPermissionAPIListDelete")]
		public async Task<ActionResult<object>> SYPermissionAPIListDelete(List<SYPermissionAPI> _sYPermissionAPIs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (SYPermissionAPI _sYPermissionAPI in _sYPermissionAPIs)
				{
					var result = await new SYPermissionAPI(_appSetting).SYPermissionAPIDelete(_sYPermissionAPI);
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
		[Route("SYPermissionAPIDeleteAll")]
		public async Task<ActionResult<object>> SYPermissionAPIDeleteAll()
		{
			try
			{
				int count = await new SYPermissionAPI(_appSetting).SYPermissionAPIDeleteAll();
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
		[Authorize]
		[Route("SYPermissionAPICount")]
		public async Task<ActionResult<object>> SYPermissionAPICount()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new SYPermissionAPI(_appSetting).SYPermissionAPICount() };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion SYPermissionAPI

		#region SYPermissionCategory

		[HttpGet]
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
		[Authorize]
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
