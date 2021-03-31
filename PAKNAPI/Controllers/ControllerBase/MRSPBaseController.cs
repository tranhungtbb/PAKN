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
	[Route("api/MRSPBase")]
	[ApiController]
	public class MRSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public MRSPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize]
		[Route("HISRecommendationGetByObjectIdBase")]
		public async Task<ActionResult<object>> HISRecommendationGetByObjectIdBase(int? Id)
		{
			try
			{
				List<HISRecommendationGetByObjectId> rsHISRecommendationGetByObjectId = await new HISRecommendationGetByObjectId(_appSetting).HISRecommendationGetByObjectIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISRecommendationGetByObjectId", rsHISRecommendationGetByObjectId},
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
		[Route("HISRecommendationInsertBase")]
		public async Task<ActionResult<object>> HISRecommendationInsertBase(HISRecommendationInsertIN _hISRecommendationInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(_hISRecommendationInsertIN) };
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
		[Route("HISRecommendationInsertListBase")]
		public async Task<ActionResult<object>> HISRecommendationInsertListBase(List<HISRecommendationInsertIN> _hISRecommendationInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _hISRecommendationInsertIN in _hISRecommendationInsertINs)
				{
					var result = await new HISRecommendationInsert(_appSetting).HISRecommendationInsertDAO(_hISRecommendationInsertIN);
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
		[Route("MRRecommendationCheckExistedCodeBase")]
		public async Task<ActionResult<object>> MRRecommendationCheckExistedCodeBase(string Code)
		{
			try
			{
				List<MRRecommendationCheckExistedCode> rsMRRecommendationCheckExistedCode = await new MRRecommendationCheckExistedCode(_appSetting).MRRecommendationCheckExistedCodeDAO(Code);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationCheckExistedCode", rsMRRecommendationCheckExistedCode},
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
		[Route("MRRecommendationFilesDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesDeleteBase(MRRecommendationFilesDeleteIN _mRRecommendationFilesDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFilesDelete(_appSetting).MRRecommendationFilesDeleteDAO(_mRRecommendationFilesDeleteIN) };
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
		[Route("MRRecommendationFilesDeleteListBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesDeleteListBase(List<MRRecommendationFilesDeleteIN> _mRRecommendationFilesDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationFilesDeleteIN in _mRRecommendationFilesDeleteINs)
				{
					var result = await new MRRecommendationFilesDelete(_appSetting).MRRecommendationFilesDeleteDAO(_mRRecommendationFilesDeleteIN);
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
		[Route("MRRecommendationFilesGetByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetByRecommendationIdBase(int? Id)
		{
			try
			{
				List<MRRecommendationFilesGetByRecommendationId> rsMRRecommendationFilesGetByRecommendationId = await new MRRecommendationFilesGetByRecommendationId(_appSetting).MRRecommendationFilesGetByRecommendationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationFilesGetByRecommendationId", rsMRRecommendationFilesGetByRecommendationId},
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
		[Route("MRRecommendationFilesInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesInsertBase(MRRecommendationFilesInsertIN _mRRecommendationFilesInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(_mRRecommendationFilesInsertIN) };
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
		[Route("MRRecommendationFilesInsertListBase")]
		public async Task<ActionResult<object>> MRRecommendationFilesInsertListBase(List<MRRecommendationFilesInsertIN> _mRRecommendationFilesInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationFilesInsertIN in _mRRecommendationFilesInsertINs)
				{
					var result = await new MRRecommendationFilesInsert(_appSetting).MRRecommendationFilesInsertDAO(_mRRecommendationFilesInsertIN);
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
		[Route("MRRecommendationForwardGetByIDBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetByIDBase(int? Id)
		{
			try
			{
				List<MRRecommendationForwardGetByID> rsMRRecommendationForwardGetByID = await new MRRecommendationForwardGetByID(_appSetting).MRRecommendationForwardGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationForwardGetByID", rsMRRecommendationForwardGetByID},
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
		[Route("MRRecommendationForwardInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardInsertBase(MRRecommendationForwardInsertIN _mRRecommendationForwardInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN) };
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
		[Route("MRRecommendationForwardInsertListBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardInsertListBase(List<MRRecommendationForwardInsertIN> _mRRecommendationForwardInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationForwardInsertIN in _mRRecommendationForwardInsertINs)
				{
					var result = await new MRRecommendationForwardInsert(_appSetting).MRRecommendationForwardInsertDAO(_mRRecommendationForwardInsertIN);
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
		[Route("MRRecommendationForwardProcessBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardProcessBase(MRRecommendationForwardProcessIN _mRRecommendationForwardProcessIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(_mRRecommendationForwardProcessIN) };
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
		[Route("MRRecommendationForwardProcessListBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardProcessListBase(List<MRRecommendationForwardProcessIN> _mRRecommendationForwardProcessINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationForwardProcessIN in _mRRecommendationForwardProcessINs)
				{
					var result = await new MRRecommendationForwardProcess(_appSetting).MRRecommendationForwardProcessDAO(_mRRecommendationForwardProcessIN);
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
		[Route("MRRecommendationForwardUpdateBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdateBase(MRRecommendationForwardUpdateIN _mRRecommendationForwardUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardUpdate(_appSetting).MRRecommendationForwardUpdateDAO(_mRRecommendationForwardUpdateIN) };
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
		[Route("MRRecommendationForwardUpdateListBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdateListBase(List<MRRecommendationForwardUpdateIN> _mRRecommendationForwardUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationForwardUpdateIN in _mRRecommendationForwardUpdateINs)
				{
					var result = await new MRRecommendationForwardUpdate(_appSetting).MRRecommendationForwardUpdateDAO(_mRRecommendationForwardUpdateIN);
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
		[Route("MRRecommendationForwardUpdateIsviewedBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdateIsviewedBase(MRRecommendationForwardUpdateIsviewedIN _mRRecommendationForwardUpdateIsviewedIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForwardUpdateIsviewed(_appSetting).MRRecommendationForwardUpdateIsviewedDAO(_mRRecommendationForwardUpdateIsviewedIN) };
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
		[Route("MRRecommendationForwardUpdateIsviewedListBase")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdateIsviewedListBase(List<MRRecommendationForwardUpdateIsviewedIN> _mRRecommendationForwardUpdateIsviewedINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationForwardUpdateIsviewedIN in _mRRecommendationForwardUpdateIsviewedINs)
				{
					var result = await new MRRecommendationForwardUpdateIsviewed(_appSetting).MRRecommendationForwardUpdateIsviewedDAO(_mRRecommendationForwardUpdateIsviewedIN);
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
		[Route("MRRecommendationGenCodeGetCodeBase")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeGetCodeBase()
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCodeGetCode(_appSetting).MRRecommendationGenCodeGetCodeDAO() };
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
		[Route("MRRecommendationGenCodeUpdateNumberBase")]
		public async Task<ActionResult<object>> MRRecommendationGenCodeUpdateNumberBase()
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationGenCodeUpdateNumber(_appSetting).MRRecommendationGenCodeUpdateNumberDAO() };
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
		[Route("MRRecommendationHashtagDeleteByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagDeleteByRecommendationIdBase(MRRecommendationHashtagDeleteByRecommendationIdIN _mRRecommendationHashtagDeleteByRecommendationIdIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(_mRRecommendationHashtagDeleteByRecommendationIdIN) };
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
		[Route("MRRecommendationHashtagDeleteByRecommendationIdListBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagDeleteByRecommendationIdListBase(List<MRRecommendationHashtagDeleteByRecommendationIdIN> _mRRecommendationHashtagDeleteByRecommendationIdINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationHashtagDeleteByRecommendationIdIN in _mRRecommendationHashtagDeleteByRecommendationIdINs)
				{
					var result = await new MRRecommendationHashtagDeleteByRecommendationId(_appSetting).MRRecommendationHashtagDeleteByRecommendationIdDAO(_mRRecommendationHashtagDeleteByRecommendationIdIN);
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
		[Route("MRRecommendationHashtagGetByRecommendationIdBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagGetByRecommendationIdBase(long? Id)
		{
			try
			{
				List<MRRecommendationHashtagGetByRecommendationId> rsMRRecommendationHashtagGetByRecommendationId = await new MRRecommendationHashtagGetByRecommendationId(_appSetting).MRRecommendationHashtagGetByRecommendationIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationHashtagGetByRecommendationId", rsMRRecommendationHashtagGetByRecommendationId},
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
		[Route("MRRecommendationHashtagInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagInsertBase(MRRecommendationHashtagInsertIN _mRRecommendationHashtagInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN) };
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
		[Route("MRRecommendationHashtagInsertListBase")]
		public async Task<ActionResult<object>> MRRecommendationHashtagInsertListBase(List<MRRecommendationHashtagInsertIN> _mRRecommendationHashtagInsertINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationHashtagInsertIN in _mRRecommendationHashtagInsertINs)
				{
					var result = await new MRRecommendationHashtagInsert(_appSetting).MRRecommendationHashtagInsertDAO(_mRRecommendationHashtagInsertIN);
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
		[Route("MRRecommendationDeleteBase")]
		public async Task<ActionResult<object>> MRRecommendationDeleteBase(MRRecommendationDeleteIN _mRRecommendationDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteDAO(_mRRecommendationDeleteIN) };
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
		[Route("MRRecommendationDeleteListBase")]
		public async Task<ActionResult<object>> MRRecommendationDeleteListBase(List<MRRecommendationDeleteIN> _mRRecommendationDeleteINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationDeleteIN in _mRRecommendationDeleteINs)
				{
					var result = await new MRRecommendationDelete(_appSetting).MRRecommendationDeleteDAO(_mRRecommendationDeleteIN);
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
		[Route("MRRecommendationGetAllOnPageBase")]
		public async Task<ActionResult<object>> MRRecommendationGetAllOnPageBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetAllOnPage> rsMRRecommendationGetAllOnPage = await new MRRecommendationGetAllOnPage(_appSetting).MRRecommendationGetAllOnPageDAO(Code, SendName, Content, UnitId, Field, Status, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetAllOnPage", rsMRRecommendationGetAllOnPage},
						{"TotalCount", rsMRRecommendationGetAllOnPage != null && rsMRRecommendationGetAllOnPage.Count > 0 ? rsMRRecommendationGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationGetAllOnPage != null && rsMRRecommendationGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationGetAllOnPage != null && rsMRRecommendationGetAllOnPage.Count > 0 ? PageSize : 0},
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
		[Route("MRRecommendationGetAllWithProcessBase")]
		public async Task<ActionResult<object>> MRRecommendationGetAllWithProcessBase(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, int? PageSize, int? PageIndex)
		{
			try
			{
				List<MRRecommendationGetAllWithProcess> rsMRRecommendationGetAllWithProcess = await new MRRecommendationGetAllWithProcess(_appSetting).MRRecommendationGetAllWithProcessDAO(Code, SendName, Content, UnitId, Field, Status, UnitProcessId, PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetAllWithProcess", rsMRRecommendationGetAllWithProcess},
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
		[Route("MRRecommendationGetByIDBase")]
		public async Task<ActionResult<object>> MRRecommendationGetByIDBase(int? Id)
		{
			try
			{
				List<MRRecommendationGetByID> rsMRRecommendationGetByID = await new MRRecommendationGetByID(_appSetting).MRRecommendationGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationGetByID", rsMRRecommendationGetByID},
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
		[Route("MRRecommendationInsertBase")]
		public async Task<ActionResult<object>> MRRecommendationInsertBase(MRRecommendationInsertIN _mRRecommendationInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationInsert(_appSetting).MRRecommendationInsertDAO(_mRRecommendationInsertIN) };
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
		[Route("MRRecommendationUpdateBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateBase(MRRecommendationUpdateIN _mRRecommendationUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationUpdate(_appSetting).MRRecommendationUpdateDAO(_mRRecommendationUpdateIN) };
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
		[Route("MRRecommendationUpdateListBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateListBase(List<MRRecommendationUpdateIN> _mRRecommendationUpdateINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationUpdateIN in _mRRecommendationUpdateINs)
				{
					var result = await new MRRecommendationUpdate(_appSetting).MRRecommendationUpdateDAO(_mRRecommendationUpdateIN);
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
		[Route("MRRecommendationUpdateReactionaryWordBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateReactionaryWordBase(MRRecommendationUpdateReactionaryWordIN _mRRecommendationUpdateReactionaryWordIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationUpdateReactionaryWord(_appSetting).MRRecommendationUpdateReactionaryWordDAO(_mRRecommendationUpdateReactionaryWordIN) };
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
		[Route("MRRecommendationUpdateReactionaryWordListBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateReactionaryWordListBase(List<MRRecommendationUpdateReactionaryWordIN> _mRRecommendationUpdateReactionaryWordINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationUpdateReactionaryWordIN in _mRRecommendationUpdateReactionaryWordINs)
				{
					var result = await new MRRecommendationUpdateReactionaryWord(_appSetting).MRRecommendationUpdateReactionaryWordDAO(_mRRecommendationUpdateReactionaryWordIN);
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
		[Route("MRRecommendationUpdateStatusBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateStatusBase(MRRecommendationUpdateStatusIN _mRRecommendationUpdateStatusIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN) };
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
		[Route("MRRecommendationUpdateStatusListBase")]
		public async Task<ActionResult<object>> MRRecommendationUpdateStatusListBase(List<MRRecommendationUpdateStatusIN> _mRRecommendationUpdateStatusINs)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (var _mRRecommendationUpdateStatusIN in _mRRecommendationUpdateStatusINs)
				{
					var result = await new MRRecommendationUpdateStatus(_appSetting).MRRecommendationUpdateStatusDAO(_mRRecommendationUpdateStatusIN);
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
