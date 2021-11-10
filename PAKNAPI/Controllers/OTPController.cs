using Bugsnag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
	[Route("api/otp")]
	[ValidateModel]
	public class OTPController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IClient _bugsnag;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;


		public OTPController(IAppSetting appSetting, IClient bugsnag, Microsoft.Extensions.Configuration.IConfiguration configuration)
		{
			_appSetting = appSetting;
			_bugsnag = bugsnag;
			_configuration = configuration;
		}
		/// <summary>
		/// api send otp app
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("otp-register")]
		[HttpPost]
		public async  Task<ActionResult<object>> GetAuthenticationCodeForApp(UpdateTokenFireBaseRequest request)
		{
			try
			{
				var otpCode = new Captcha(_appSetting).GenerateOTPCode(); 
				SYNotificationModel notification = new SYNotificationModel();
				notification.DataId = 0;
				notification.SenderId = 0;
				notification.SendOrgId = 0;
				notification.ReceiveId = 0;
				notification.ReceiveOrgId = 0;
				notification.SendDate = DateTime.Now;
				notification.Type = 0;
				notification.TypeSend = STATUS_RECOMMENDATION.FINISED;
				notification.IsViewed = true;
				notification.IsReaded = true;
				notification.Title = "Mã xác thực";
				notification.Content = "Mã xác thực của bạn là " + otpCode +" có hiệu lực trong 10 phút. Lưu ý: Không chia sẻ bất kỳ mã xác thực này với ai!";
				List<string> token = new List<string>();
				token.Add(request.Token);
				new SYNotification(_appSetting, _configuration).NotifiDocumentJob(token, notification);

				// insert code trong db
				OTP otp = new OTP(otpCode, Request.Headers["User-Agent"].ToString());
				await new SYOTP(_appSetting).InsertOTPDAO(otp);

				return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK };

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// api send otp forget password
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[Route("otp-forget-password")]
		[HttpPost]
		public async Task<ActionResult<object>> GetAuthenticationCodeForgetPassword([FromForm] OtpForgetPasswordRequest request)
		{
			try
			{
				// check phone SY_User_CheckExists
				var rsSYUserCheckExists = await new SYUserCheckExists(_appSetting).SYUserCheckExistsForgetDAO(request.Phone);
				if (rsSYUserCheckExists <= 0) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại không tồn tại" };
				}

				var otpCode = new Captcha(_appSetting).GenerateOTPCode();
				SYNotificationModel notification = new SYNotificationModel();
				notification.DataId = 0;
				notification.SenderId = 0;
				notification.SendOrgId = 0;
				notification.ReceiveId = 0;
				notification.ReceiveOrgId = 0;
				notification.SendDate = DateTime.Now;
				notification.Type = 0;
				notification.TypeSend = STATUS_RECOMMENDATION.FINISED;
				notification.IsViewed = true;
				notification.IsReaded = true;
				notification.Title = "Mã xác thực";
				notification.Content = "Mã xác thực của bạn là " + otpCode + " có hiệu lực trong 10 phút. Lưu ý: Không chia sẻ bất kỳ mã xác thực này với ai!";
				List<string> token = new List<string>();
				token.Add(request.Token);
				new SYNotification(_appSetting, _configuration).NotifiDocumentJob(token, notification);

				// insert code trong db
				OTP otp = new OTP(otpCode, Request.Headers["User-Agent"].ToString());
				await new SYOTP(_appSetting).InsertOTPDAO(otp);

				return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK };

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// api validate otp
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[Route("otp-validate")]
		[HttpGet]
		public async Task<ActionResult<object>> ValidateAuthenticationCodeForApp(string code)
		{
			try
			{
				var otp = await new SYOTP(_appSetting).GetOTPByCodeDAO(code, Request.Headers["User-Agent"].ToString());

				if (otp == null) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mã xác thực không đúng" };
				}
				if (otp.IsUse) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mã xác thực này đã được sử dụng" };
				}
				if (otp.ExpireDate < DateTime.Now) {
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mã xác thực này đã hết hạn" };
				}

				// udate lại isuser
				otp.IsUse = true;
				await new SYOTP(_appSetting).UpdateOTPDAO(otp);

				return new ResultApi { Success = ResultCode.OK, Message = ResultMessage.OK };

			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}
}
