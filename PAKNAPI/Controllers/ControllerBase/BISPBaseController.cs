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
	[Route("api/BISPBase")]
	[ApiController]
	public class BISPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;

		public BISPBaseController(IAppSetting appSetting, IClient bugsnag)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
		}

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("BIBusinessCheckExistsBase")]
		public async Task<ActionResult<object>> BIBusinessCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<BIBusinessCheckExists> rsBIBusinessCheckExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessCheckExists", rsBIBusinessCheckExists},
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
		[Route("BIBusinessGetByUserIdBase")]
		public async Task<ActionResult<object>> BIBusinessGetByUserIdBase(long? UserId)
		{
			try
			{
				List<BIBusinessGetByUserId> rsBIBusinessGetByUserId = await new BIBusinessGetByUserId(_appSetting).BIBusinessGetByUserIdDAO(UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetByUserId", rsBIBusinessGetByUserId},
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
		[Route("BIBusinessGetDropdownBase")]
		public async Task<ActionResult<object>> BIBusinessGetDropdownBase()
		{
			try
			{
				List<BIBusinessGetDropdown> rsBIBusinessGetDropdown = await new BIBusinessGetDropdown(_appSetting).BIBusinessGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetDropdown", rsBIBusinessGetDropdown},
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
		[Route("BIBusinessGetRepresentativeByIdBase")]
		public async Task<ActionResult<object>> BIBusinessGetRepresentativeByIdBase(long? Id)
		{
			try
			{
				List<BIBusinessGetRepresentativeById> rsBIBusinessGetRepresentativeById = await new BIBusinessGetRepresentativeById(_appSetting).BIBusinessGetRepresentativeByIdDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetRepresentativeById", rsBIBusinessGetRepresentativeById},
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
		[Route("BIBusinessGetRepresentativeEmailBase")]
		public async Task<ActionResult<object>> BIBusinessGetRepresentativeEmailBase(string Email)
		{
			try
			{
				List<BIBusinessGetRepresentativeEmail> rsBIBusinessGetRepresentativeEmail = await new BIBusinessGetRepresentativeEmail(_appSetting).BIBusinessGetRepresentativeEmailDAO(Email);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIBusinessGetRepresentativeEmail", rsBIBusinessGetRepresentativeEmail},
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
		[Route("BIBusinessInsertBase")]
		public async Task<ActionResult<object>> BIBusinessInsertBase(BIBusinessInsertIN _bIBusinessInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusinessInsert(_appSetting).BIBusinessInsertDAO(_bIBusinessInsertIN) };
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
		[Route("BIBusinessUpdateInfoBase")]
		public async Task<ActionResult<object>> BIBusinessUpdateInfoBase(BIBusinessUpdateInfoIN _bIBusinessUpdateInfoIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIBusinessUpdateInfo(_appSetting).BIBusinessUpdateInfoDAO(_bIBusinessUpdateInfoIN) };
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
		[Route("BIIndividualCheckExistsBase")]
		public async Task<ActionResult<object>> BIIndividualCheckExistsBase(string Field, string Value, long? Id)
		{
			try
			{
				List<BIIndividualCheckExists> rsBIIndividualCheckExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO(Field, Value, Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividualCheckExists", rsBIIndividualCheckExists},
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
		[Route("BIIndividualGetByUserIdBase")]
		public async Task<ActionResult<object>> BIIndividualGetByUserIdBase(long? UserId)
		{
			try
			{
				List<BIIndividualGetByUserId> rsBIIndividualGetByUserId = await new BIIndividualGetByUserId(_appSetting).BIIndividualGetByUserIdDAO(UserId);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividualGetByUserId", rsBIIndividualGetByUserId},
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
		[Route("BIIndividualInsertBase")]
		public async Task<ActionResult<object>> BIIndividualInsertBase(BIIndividualInsertIN _bIIndividualInsertIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIIndividualInsert(_appSetting).BIIndividualInsertDAO(_bIIndividualInsertIN) };
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
		[Route("BIIndividualOrBusinessGetDropListByProviceIdBase")]
		public async Task<ActionResult<object>> BIIndividualOrBusinessGetDropListByProviceIdBase(int? Id, int? Type)
		{
			try
			{
				List<BIIndividualOrBusinessGetDropListByProviceId> rsBIIndividualOrBusinessGetDropListByProviceId = await new BIIndividualOrBusinessGetDropListByProviceId(_appSetting).BIIndividualOrBusinessGetDropListByProviceIdDAO(Id, Type);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"BIIndividualOrBusinessGetDropListByProviceId", rsBIIndividualOrBusinessGetDropListByProviceId},
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
		[Route("BIInvididualUpdateInfoBase")]
		public async Task<ActionResult<object>> BIInvididualUpdateInfoBase(BIInvididualUpdateInfoIN _bIInvididualUpdateInfoIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new BIInvididualUpdateInfo(_appSetting).BIInvididualUpdateInfoDAO(_bIInvididualUpdateInfoIN) };
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
