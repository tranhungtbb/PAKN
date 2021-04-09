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
	[Route("api/QLTableBase")]
	[ApiController]
	public class QLTableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public QLTableBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		#region QLDoanhNghiep

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("QLDoanhNghiepGetByID")]
		public async Task<ActionResult<object>> QLDoanhNghiepGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepGetByID(Id) };
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
		[Route("QLDoanhNghiepGetAll")]
		public async Task<ActionResult<object>> QLDoanhNghiepGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepGetAll() };
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
		[Route("QLDoanhNghiepGetAllOnPage")]
		public async Task<ActionResult<object>> QLDoanhNghiepGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<QLDoanhNghiepOnPage> rsQLDoanhNghiepOnPage = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"QLDoanhNghiep", rsQLDoanhNghiepOnPage},
						{"TotalCount", rsQLDoanhNghiepOnPage != null && rsQLDoanhNghiepOnPage.Count > 0 ? rsQLDoanhNghiepOnPage[0].RowNumber : 0},
						{"PageIndex", rsQLDoanhNghiepOnPage != null && rsQLDoanhNghiepOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsQLDoanhNghiepOnPage != null && rsQLDoanhNghiepOnPage.Count > 0 ? PageSize : 0},
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
		[Route("QLDoanhNghiepInsert")]
		public async Task<ActionResult<object>> QLDoanhNghiepInsert(QLDoanhNghiep _qLDoanhNghiep)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepInsert(_qLDoanhNghiep) };
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
		[Route("QLDoanhNghiepListInsert")]
		public async Task<ActionResult<object>> QLDoanhNghiepListInsert(List<QLDoanhNghiep> _qLDoanhNghieps)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (QLDoanhNghiep _qLDoanhNghiep in _qLDoanhNghieps)
				{
					int? result = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepInsert(_qLDoanhNghiep);
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
		[Route("QLDoanhNghiepUpdate")]
		public async Task<ActionResult<object>> QLDoanhNghiepUpdate(QLDoanhNghiep _qLDoanhNghiep)
		{
			try
			{
				int count = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepUpdate(_qLDoanhNghiep);
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
		[Route("QLDoanhNghiepDelete")]
		public async Task<ActionResult<object>> QLDoanhNghiepDelete(QLDoanhNghiep _qLDoanhNghiep)
		{
			try
			{
				int count = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepDelete(_qLDoanhNghiep);
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
		[Route("QLDoanhNghiepListDelete")]
		public async Task<ActionResult<object>> QLDoanhNghiepListDelete(List<QLDoanhNghiep> _qLDoanhNghieps)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (QLDoanhNghiep _qLDoanhNghiep in _qLDoanhNghieps)
				{
					var result = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepDelete(_qLDoanhNghiep);
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
		[Route("QLDoanhNghiepDeleteAll")]
		public async Task<ActionResult<object>> QLDoanhNghiepDeleteAll()
		{
			try
			{
				int count = await new QLDoanhNghiep(_appSetting).QLDoanhNghiepDeleteAll();
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

		#endregion QLDoanhNghiep

		#region QLNguoiDan

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("QLNguoiDanGetByID")]
		public async Task<ActionResult<object>> QLNguoiDanGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new QLNguoiDan(_appSetting).QLNguoiDanGetByID(Id) };
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
		[Route("QLNguoiDanGetAll")]
		public async Task<ActionResult<object>> QLNguoiDanGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new QLNguoiDan(_appSetting).QLNguoiDanGetAll() };
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
		[Route("QLNguoiDanGetAllOnPage")]
		public async Task<ActionResult<object>> QLNguoiDanGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<QLNguoiDanOnPage> rsQLNguoiDanOnPage = await new QLNguoiDan(_appSetting).QLNguoiDanGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"QLNguoiDan", rsQLNguoiDanOnPage},
						{"TotalCount", rsQLNguoiDanOnPage != null && rsQLNguoiDanOnPage.Count > 0 ? rsQLNguoiDanOnPage[0].RowNumber : 0},
						{"PageIndex", rsQLNguoiDanOnPage != null && rsQLNguoiDanOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsQLNguoiDanOnPage != null && rsQLNguoiDanOnPage.Count > 0 ? PageSize : 0},
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
		[Route("QLNguoiDanInsert")]
		public async Task<ActionResult<object>> QLNguoiDanInsert(QLNguoiDan _qLNguoiDan)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new QLNguoiDan(_appSetting).QLNguoiDanInsert(_qLNguoiDan) };
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
		[Route("QLNguoiDanListInsert")]
		public async Task<ActionResult<object>> QLNguoiDanListInsert(List<QLNguoiDan> _qLNguoiDans)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (QLNguoiDan _qLNguoiDan in _qLNguoiDans)
				{
					int? result = await new QLNguoiDan(_appSetting).QLNguoiDanInsert(_qLNguoiDan);
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
		[Route("QLNguoiDanUpdate")]
		public async Task<ActionResult<object>> QLNguoiDanUpdate(QLNguoiDan _qLNguoiDan)
		{
			try
			{
				int count = await new QLNguoiDan(_appSetting).QLNguoiDanUpdate(_qLNguoiDan);
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
		[Route("QLNguoiDanDelete")]
		public async Task<ActionResult<object>> QLNguoiDanDelete(QLNguoiDan _qLNguoiDan)
		{
			try
			{
				int count = await new QLNguoiDan(_appSetting).QLNguoiDanDelete(_qLNguoiDan);
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
		[Route("QLNguoiDanListDelete")]
		public async Task<ActionResult<object>> QLNguoiDanListDelete(List<QLNguoiDan> _qLNguoiDans)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (QLNguoiDan _qLNguoiDan in _qLNguoiDans)
				{
					var result = await new QLNguoiDan(_appSetting).QLNguoiDanDelete(_qLNguoiDan);
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
		[Route("QLNguoiDanDeleteAll")]
		public async Task<ActionResult<object>> QLNguoiDanDeleteAll()
		{
			try
			{
				int count = await new QLNguoiDan(_appSetting).QLNguoiDanDeleteAll();
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

		#endregion QLNguoiDan
	}
}
