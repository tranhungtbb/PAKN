using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PAKNAPI.Controllers
{
    [Route("api/sendsms")]
    [ApiController]
    [ValidateModel]
    public class SendSmsController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

		public SendSmsController(IAppSetting appSetting, IClient bugsnag, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
			_configuration = configuration;
		}
		[HttpPost]
		[Route("send")]
		[AllowAnonymous]
		public async Task<ActionResult<object>> OnSendSMS(SendMessageRequest request)
		{
			try
			{
				string accountSid = _configuration["TwilioAccountSid"];
				string authToken = _configuration["TwilioAuthToken"];
				string phoneFrom = _configuration["TwilioAuthPhone"];
				TwilioClient.Init(accountSid, authToken);
				var to = new PhoneNumber(request.phoneTo);
				var from = new PhoneNumber(phoneFrom);
				var resmessage = MessageResource.Create(
					to: to,
					from: from,
					body: request.message
					);
				var result =  Content(resmessage.Sid);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
				return new ResultApi { Success = ResultCode.OK, Result = result };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		public class SendMessageRequest:BaseRequest
        {
			public string phoneTo { get; set; }
			public string message { get; set; }
        }
	}
}
