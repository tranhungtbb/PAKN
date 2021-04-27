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
	[Route("api/DAMSPBase")]
	[ApiController]
	public class DAMSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public DAMSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("DAMAdministrationDeleteBase")]
		public async Task<ActionResult<object>> DAMAdministrationDeleteBase(DAMAdministrationDeleteIN _dAMAdministrationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationDelete(_appSetting).DAMAdministrationDeleteDAO(_dAMAdministrationDeleteIN) };
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
		[Route("DAMAdministrationDeleteListBase")]
		public async Task<ActionResult<object>> DAMAdministrationDeleteListBase(List<DAMAdministrationDeleteIN> _dAMAdministrationDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMAdministrationDeleteIN in _dAMAdministrationDeleteINs)
				{
					var result = await new DAMAdministrationDelete(_appSetting).DAMAdministrationDeleteDAO(_dAMAdministrationDeleteIN);
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
		[Route("DAMAdministrationFilesDeleteBase")]
		public async Task<ActionResult<object>> DAMAdministrationFilesDeleteBase(DAMAdministrationFilesDeleteIN _dAMAdministrationFilesDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationFilesDelete(_appSetting).DAMAdministrationFilesDeleteDAO(_dAMAdministrationFilesDeleteIN) };
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
		[Route("DAMAdministrationFilesDeleteListBase")]
		public async Task<ActionResult<object>> DAMAdministrationFilesDeleteListBase(List<DAMAdministrationFilesDeleteIN> _dAMAdministrationFilesDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMAdministrationFilesDeleteIN in _dAMAdministrationFilesDeleteINs)
				{
					var result = await new DAMAdministrationFilesDelete(_appSetting).DAMAdministrationFilesDeleteDAO(_dAMAdministrationFilesDeleteIN);
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
		[Authorize("ThePolicy")]
		[Route("DAMAdministrationFilesGetByAdministrationIdBase")]
		public async Task<ActionResult<object>> DAMAdministrationFilesGetByAdministrationIdBase(int? Id)
		{
			try
			{
				List<DAMAdministrationFilesGetByAdministrationId> rsDAMAdministrationFilesGetByAdministrationId = await new DAMAdministrationFilesGetByAdministrationId(_appSetting).DAMAdministrationFilesGetByAdministrationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMAdministrationFilesGetByAdministrationId", rsDAMAdministrationFilesGetByAdministrationId},
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
		[Route("DAMAdministrationFilesInsertBase")]
		public async Task<ActionResult<object>> DAMAdministrationFilesInsertBase(DAMAdministrationFilesInsertIN _dAMAdministrationFilesInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationFilesInsert(_appSetting).DAMAdministrationFilesInsertDAO(_dAMAdministrationFilesInsertIN) };
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
		[Route("DAMAdministrationFilesInsertListBase")]
		public async Task<ActionResult<object>> DAMAdministrationFilesInsertListBase(List<DAMAdministrationFilesInsertIN> _dAMAdministrationFilesInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMAdministrationFilesInsertIN in _dAMAdministrationFilesInsertINs)
				{
					var result = await new DAMAdministrationFilesInsert(_appSetting).DAMAdministrationFilesInsertDAO(_dAMAdministrationFilesInsertIN);
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
		[Authorize("ThePolicy")]
		[Route("DAMAdministrationGetByIdBase")]
		public async Task<ActionResult<object>> DAMAdministrationGetByIdBase(int? Id)
		{
			try
			{
				List<DAMAdministrationGetById> rsDAMAdministrationGetById = await new DAMAdministrationGetById(_appSetting).DAMAdministrationGetByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMAdministrationGetById", rsDAMAdministrationGetById},
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
		[Route("DAMAdministrationGetListBase")]
		public async Task<ActionResult<object>> DAMAdministrationGetListBase(string Code, string Name, string Object, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex, int? TotalRecords)
		{
			try
			{
				List<DAMAdministrationGetList> rsDAMAdministrationGetList = await new DAMAdministrationGetList(_appSetting).DAMAdministrationGetListDAO(Code, Name, Object, UnitId, Field, Status, PageSize, PageIndex, TotalRecords);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMAdministrationGetList", rsDAMAdministrationGetList},
						{"TotalCount", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? rsDAMAdministrationGetList[0].RowNumber : 0},
						{"PageIndex", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? PageIndex : 0},
						{"PageSize", rsDAMAdministrationGetList != null && rsDAMAdministrationGetList.Count > 0 ? PageSize : 0},
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
		[Route("DAMAdministrationInsertBase")]
		public async Task<ActionResult<object>> DAMAdministrationInsertBase(DAMAdministrationInsertIN _dAMAdministrationInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationInsert(_appSetting).DAMAdministrationInsertDAO(_dAMAdministrationInsertIN) };
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
		[Route("DAMAdministrationUpdateBase")]
		public async Task<ActionResult<object>> DAMAdministrationUpdateBase(DAMAdministrationUpdateIN _dAMAdministrationUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMAdministrationUpdate(_appSetting).DAMAdministrationUpdateDAO(_dAMAdministrationUpdateIN) };
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
		[Route("DAMAdministrationUpdateListBase")]
		public async Task<ActionResult<object>> DAMAdministrationUpdateListBase(List<DAMAdministrationUpdateIN> _dAMAdministrationUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMAdministrationUpdateIN in _dAMAdministrationUpdateINs)
				{
					var result = await new DAMAdministrationUpdate(_appSetting).DAMAdministrationUpdateDAO(_dAMAdministrationUpdateIN);
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
		[Authorize("ThePolicy")]
		[Route("DAMAdministrationGetListHomePageBase")]
		public async Task<ActionResult<object>> DAMAdministrationGetListHomePageBase()
		{
			try
			{
				List<DAMAdministrationGetListHomePage> rsDAMAdministrationGetListHomePage = await new DAMAdministrationGetListHomePage(_appSetting).DAMAdministrationGetListHomePageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMAdministrationGetListHomePage", rsDAMAdministrationGetListHomePage},
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
		[Route("DAMChargesCreateBase")]
		public async Task<ActionResult<object>> DAMChargesCreateBase(DAMChargesCreateIN _dAMChargesCreateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMChargesCreate(_appSetting).DAMChargesCreateDAO(_dAMChargesCreateIN) };
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
		[Route("DAMChargesCreateListBase")]
		public async Task<ActionResult<object>> DAMChargesCreateListBase(List<DAMChargesCreateIN> _dAMChargesCreateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMChargesCreateIN in _dAMChargesCreateINs)
				{
					var result = await new DAMChargesCreate(_appSetting).DAMChargesCreateDAO(_dAMChargesCreateIN);
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
		[Route("DAMChargesDeleteByIdBase")]
		public async Task<ActionResult<object>> DAMChargesDeleteByIdBase(DAMChargesDeleteByIdIN _dAMChargesDeleteByIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMChargesDeleteById(_appSetting).DAMChargesDeleteByIdDAO(_dAMChargesDeleteByIdIN) };
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
		[Route("DAMChargesDeleteByIdListBase")]
		public async Task<ActionResult<object>> DAMChargesDeleteByIdListBase(List<DAMChargesDeleteByIdIN> _dAMChargesDeleteByIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMChargesDeleteByIdIN in _dAMChargesDeleteByIdINs)
				{
					var result = await new DAMChargesDeleteById(_appSetting).DAMChargesDeleteByIdDAO(_dAMChargesDeleteByIdIN);
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
		[Authorize("ThePolicy")]
		[Route("DAMChargesGetByAdministrationBase")]
		public async Task<ActionResult<object>> DAMChargesGetByAdministrationBase(int? Id)
		{
			try
			{
				List<DAMChargesGetByAdministration> rsDAMChargesGetByAdministration = await new DAMChargesGetByAdministration(_appSetting).DAMChargesGetByAdministrationDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMChargesGetByAdministration", rsDAMChargesGetByAdministration},
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
		[Route("DAMChargesGetByIdBase")]
		public async Task<ActionResult<object>> DAMChargesGetByIdBase(int? Id)
		{
			try
			{
				List<DAMChargesGetById> rsDAMChargesGetById = await new DAMChargesGetById(_appSetting).DAMChargesGetByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMChargesGetById", rsDAMChargesGetById},
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
		[Route("DAMChargesUpdateBase")]
		public async Task<ActionResult<object>> DAMChargesUpdateBase(DAMChargesUpdateIN _dAMChargesUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMChargesUpdate(_appSetting).DAMChargesUpdateDAO(_dAMChargesUpdateIN) };
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
		[Route("DAMChargesUpdateListBase")]
		public async Task<ActionResult<object>> DAMChargesUpdateListBase(List<DAMChargesUpdateIN> _dAMChargesUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMChargesUpdateIN in _dAMChargesUpdateINs)
				{
					var result = await new DAMChargesUpdate(_appSetting).DAMChargesUpdateDAO(_dAMChargesUpdateIN);
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
		[Route("DAMCompositionProfileCreateBase")]
		public async Task<ActionResult<object>> DAMCompositionProfileCreateBase(DAMCompositionProfileCreateIN _dAMCompositionProfileCreateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMCompositionProfileCreate(_appSetting).DAMCompositionProfileCreateDAO(_dAMCompositionProfileCreateIN) };
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
		[Route("DAMCompositionProfileFileFilesDeleteBase")]
		public async Task<ActionResult<object>> DAMCompositionProfileFileFilesDeleteBase(DAMCompositionProfileFileFilesDeleteIN _dAMCompositionProfileFileFilesDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMCompositionProfileFileFilesDelete(_appSetting).DAMCompositionProfileFileFilesDeleteDAO(_dAMCompositionProfileFileFilesDeleteIN) };
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
		[Route("DAMCompositionProfileFileFilesDeleteListBase")]
		public async Task<ActionResult<object>> DAMCompositionProfileFileFilesDeleteListBase(List<DAMCompositionProfileFileFilesDeleteIN> _dAMCompositionProfileFileFilesDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMCompositionProfileFileFilesDeleteIN in _dAMCompositionProfileFileFilesDeleteINs)
				{
					var result = await new DAMCompositionProfileFileFilesDelete(_appSetting).DAMCompositionProfileFileFilesDeleteDAO(_dAMCompositionProfileFileFilesDeleteIN);
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
		[Authorize("ThePolicy")]
		[Route("DAMCompositionProfileFileFilesGetByCompositionProfileIdBase")]
		public async Task<ActionResult<object>> DAMCompositionProfileFileFilesGetByCompositionProfileIdBase(int? Id)
		{
			try
			{
				List<DAMCompositionProfileFileFilesGetByCompositionProfileId> rsDAMCompositionProfileFileFilesGetByCompositionProfileId = await new DAMCompositionProfileFileFilesGetByCompositionProfileId(_appSetting).DAMCompositionProfileFileFilesGetByCompositionProfileIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMCompositionProfileFileFilesGetByCompositionProfileId", rsDAMCompositionProfileFileFilesGetByCompositionProfileId},
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
		[Route("DAMCompositionProfileFileFilesInsertBase")]
		public async Task<ActionResult<object>> DAMCompositionProfileFileFilesInsertBase(DAMCompositionProfileFileFilesInsertIN _dAMCompositionProfileFileFilesInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMCompositionProfileFileFilesInsert(_appSetting).DAMCompositionProfileFileFilesInsertDAO(_dAMCompositionProfileFileFilesInsertIN) };
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
		[Route("DAMCompositionProfileFileFilesInsertListBase")]
		public async Task<ActionResult<object>> DAMCompositionProfileFileFilesInsertListBase(List<DAMCompositionProfileFileFilesInsertIN> _dAMCompositionProfileFileFilesInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMCompositionProfileFileFilesInsertIN in _dAMCompositionProfileFileFilesInsertINs)
				{
					var result = await new DAMCompositionProfileFileFilesInsert(_appSetting).DAMCompositionProfileFileFilesInsertDAO(_dAMCompositionProfileFileFilesInsertIN);
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
		[Route("DAMImplementationProcessCreateBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessCreateBase(DAMImplementationProcessCreateIN _dAMImplementationProcessCreateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMImplementationProcessCreate(_appSetting).DAMImplementationProcessCreateDAO(_dAMImplementationProcessCreateIN) };
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
		[Route("DAMImplementationProcessCreateListBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessCreateListBase(List<DAMImplementationProcessCreateIN> _dAMImplementationProcessCreateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMImplementationProcessCreateIN in _dAMImplementationProcessCreateINs)
				{
					var result = await new DAMImplementationProcessCreate(_appSetting).DAMImplementationProcessCreateDAO(_dAMImplementationProcessCreateIN);
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
		[Route("DAMImplementationProcessDeleteByIdBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessDeleteByIdBase(DAMImplementationProcessDeleteByIdIN _dAMImplementationProcessDeleteByIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMImplementationProcessDeleteById(_appSetting).DAMImplementationProcessDeleteByIdDAO(_dAMImplementationProcessDeleteByIdIN) };
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
		[Route("DAMImplementationProcessDeleteByIdListBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessDeleteByIdListBase(List<DAMImplementationProcessDeleteByIdIN> _dAMImplementationProcessDeleteByIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMImplementationProcessDeleteByIdIN in _dAMImplementationProcessDeleteByIdINs)
				{
					var result = await new DAMImplementationProcessDeleteById(_appSetting).DAMImplementationProcessDeleteByIdDAO(_dAMImplementationProcessDeleteByIdIN);
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
		[Authorize("ThePolicy")]
		[Route("DAMImplementationProcessGetByAdministrationBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessGetByAdministrationBase(int? Id)
		{
			try
			{
				List<DAMImplementationProcessGetByAdministration> rsDAMImplementationProcessGetByAdministration = await new DAMImplementationProcessGetByAdministration(_appSetting).DAMImplementationProcessGetByAdministrationDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"DAMImplementationProcessGetByAdministration", rsDAMImplementationProcessGetByAdministration},
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
		[Route("DAMImplementationProcessUpdateBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessUpdateBase(DAMImplementationProcessUpdateIN _dAMImplementationProcessUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new DAMImplementationProcessUpdate(_appSetting).DAMImplementationProcessUpdateDAO(_dAMImplementationProcessUpdateIN) };
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
		[Route("DAMImplementationProcessUpdateListBase")]
		public async Task<ActionResult<object>> DAMImplementationProcessUpdateListBase(List<DAMImplementationProcessUpdateIN> _dAMImplementationProcessUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _dAMImplementationProcessUpdateIN in _dAMImplementationProcessUpdateINs)
				{
					var result = await new DAMImplementationProcessUpdate(_appSetting).DAMImplementationProcessUpdateDAO(_dAMImplementationProcessUpdateIN);
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
	}
}
