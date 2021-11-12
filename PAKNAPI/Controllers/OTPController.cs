using Bugsnag;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
		public async Task<ActionResult<object>> GetAuthenticationCodeForApp([FromForm] OtpRegisterRequest request)
		{
			try
			{
				var accCheckExist = await new SYUserCheckExists(_appSetting).SYUserCheckExistsDAO("Phone", request.Phone, 0);
				if (accCheckExist[0].Exists.Value)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
				}
				// người dân
				if (request.Type == 0)
				{
					var checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Phone", request.Phone, 0);
					if (checkExists[0].Exists.Value) {
						return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
					}
					if (string.IsNullOrEmpty(request.IdCard)) {
						return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD không được để trống" };
					}

					checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("IDCard", request.IdCard, 0);
					if (checkExists[0].Exists.Value)
						return new ResultApi { Success = ResultCode.ORROR, Message = "Số CMND / CCCD đã tồn tại" };
					if (!string.IsNullOrEmpty(request.Email))
					{
						checkExists = await new BIIndividualCheckExists(_appSetting).BIIndividualCheckExistsDAO("Email", request.Email, 0);
						if (checkExists[0].Exists.Value)
						{
							return new ResultApi { Success = ResultCode.ORROR, Message = "Email đã tồn tại" };
						}
					}

				}
				// doanh nghiệp
				else
				{

					var checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Phone", request.Phone, 0);
					if (checkExists[0].Exists.Value)
					{
						return new ResultApi { Success = ResultCode.ORROR, Message = "Số điện thoại đã tồn tại" };
					}
					
					if (!string.IsNullOrEmpty(request.Email))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("Email", request.Email, 0);
						if (checkExists[0].Exists.Value)
						{
							return new ResultApi { Success = ResultCode.ORROR, Message = "Email người đại diện đã tồn tại" };
						}
					}
					if (!string.IsNullOrEmpty(request.OrgEmail))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("OrgEmail", request.OrgEmail, 0);
						if (checkExists[0].Exists.Value)
						{
							return new ResultApi { Success = ResultCode.ORROR, Message = "Email doanh nghiệp đã tồn tại" };
						}
					}
					if (!string.IsNullOrEmpty(request.BusinessRegistration))
					{
						checkExists = await new BIBusinessCheckExists(_appSetting).BIBusinessCheckExistsDAO("BusinessRegistration", request.BusinessRegistration, 0);
						if (checkExists[0].Exists.Value)
						{
							return new ResultApi { Success = ResultCode.ORROR, Message = "Số đăng ký kinh doanh đã tồn tại" };
						}
					}
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
				if (rsSYUserCheckExists <= 0)
				{
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

				if (otp == null)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mã xác thực không đúng" };
				}
				if (otp.IsUse)
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Mã xác thực này đã được sử dụng" };
				}
				if (otp.ExpireDate < DateTime.Now)
				{
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
