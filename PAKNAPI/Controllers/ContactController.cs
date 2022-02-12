using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PAKNAPI.Models;
using PAKNAPI.Job;
using PAKNAPI.Model.ModelAuth;
using PAKNAPI.Model;
using static PAKNAPI.Model.RefreshTokens;
using System.Security.Cryptography;
using System.Linq;
using PAKNAPI.Services;
using NSwag.Annotations;

namespace PAKNAPI.Controllers
{
	[Route("api/contact")]
	[ApiController]
	[ValidateModel]
	[OpenApiTag("Login", Description = "Login")]
	public class ContactController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private readonly IHttpContextAccessor _context;

		private static string textRamdom = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static string numberRamdom = "1234567890";

		private IConfiguration _config;
		private readonly Bugsnag.IClient _bugsnag;

		public ContactController(IAppSetting appSetting, IHttpContextAccessor context, IConfiguration config, Bugsnag.IClient client)
		{
			_appSetting = appSetting;
			_context = context;
			_config = config;
			_bugsnag = client;
		}

		/// <summary>
		/// đăng nhập
		/// </summary>
		/// <param name="loginIN"></param>
		/// <returns></returns>

		[Route("login")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<object>> Login(LoginIN loginIN)
		{
			return await new LoginService(_appSetting, _config, _context).AuthenticateAsync(loginIN);
		}

		/// <summary>
		/// RefreshToken
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>

		[AllowAnonymous]
		[HttpPost]
		[Route("refresh-token")]
		public async Task<object> RefreshToken(RefreshTokenRequest model)
		{
			return await new LoginService(_appSetting, _config, _context).RefreshToken(model);
		}



		/// <summary>
		/// RevokeToken - thu hồi token
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost]
		[Route("revoke-token")]
		public async Task<object> RevokeTokenAsync(RevokeTokenRequest model)
		{
			return await new LoginService(_appSetting, _config, _context).RevokeToken(model);
		}



		/// <summary>
		/// đăng xuất - Authorize
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route("log-out")]
		[Authorize("ThePolicy")]
		public async Task<object> LogOut()
		{
			try
			{
				// chỗ này nên get token rồi lấy ra tokenId, rồi cho expire refresh token

				// get jwtId from clame
				var refreshToken = await new RefreshTokens(_appSetting).GetByJwtToken(new LogHelper(_appSetting).GetJwtIdFromRequest(HttpContext));
				if (refreshToken != null) {
					refreshToken.Revoked = DateTime.UtcNow;
					refreshToken.IsUse = true;
					await new RefreshTokens(_appSetting).Update(refreshToken);
				}
				
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);
				return new ResultApi { Success = ResultCode.OK };
			}
            catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = "An error occurred" };
			}
		}

		// lấy lại mk của quản trị qua gmail

		/// <summary>
		/// lấy lại mật khẩu qua email
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("forget-password")]
		[AllowAnonymous]
		public async Task<ActionResult<object>> Forgetpassword(Forgetpassword request)
		{
			try
			{
				List<SYUSRLogin> user = await new SYUSRLogin(_appSetting).SYUSRGetByEmailDAO(request.Email);
				if (user.Count == 0)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = 0, Message = "Không tồn tại email" };
				}
				else {
					int length = 5;
					StringBuilder res = new StringBuilder();
					Random rnd = new Random();
					while (0 < length--)
					{
						res.Append(textRamdom[rnd.Next(textRamdom.Length)]);
					}
					length = 3;
					while (0 < length--)
					{
						res.Append(numberRamdom[rnd.Next(numberRamdom.Length)]);
					}

					// lưu db

					var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL));
					if (config.Count == 0)
					{
						return new ResultApi { Success = ResultCode.ORROR, Result = -1, Message = "Config email not exits" };
					}
					else 
					{
						// gửi mail
						var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config[0].Content);
						string content =
							"<p><span style = 'font-family:times new roman,times,serif'><span style='font-size:16px'>Dear {FullName},</span></span></p>" +
							"<p style ='font-family:times new roman,times,serif'> Đây là mật khẩu mới. Vui lòng đăng nhập lại với tài khoản của bạn</p>" +
							"<br/><strong>Mật khẩu mới</strong><span style = 'font-size:14px'>: {NewPassword}</span>";
						content = content.Replace("{FullName}", user[0].FullName);
						content = content.Replace("{NewPassword}", res.ToString());
						MailHelper.SendMail(configEmail, request.Email, "Lấy lại mật khẩu", content, null);

						// lưu db
						var newPwd = GeneratePassword.generatePassword(res.ToString());
						var _model = new SYUserChangePwdIN
						{
							Password = newPwd["Password"],
							Salt = newPwd["Salt"]
						};
						_model.Id = user[0].Id;
						var rs = await new SYUserChangePwd(_appSetting).SYUserChangePwdDAO(_model);


						// cho trạng thái = false hết
						await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(user[0].Id, null);

						// ghi log

						BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(HttpContext.Request);
						//var s = Request.Headers;
						SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
						{
							UserId = user[0].Id,
							FullName = user[0].FullName,
							Action = "Forgetpassword",
							IPAddress = baseRequest.ipAddress,
							MACAddress = baseRequest.macAddress,
							Description = "Lấy lại mật khẩu",
							CreatedDate = DateTime.Now,
							Status = 1,
							Exception = null
						};

						await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);

						return new ResultApi { Success = ResultCode.OK, Result = 1, Message = ResultMessage.OK };

					}
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = "An error occurred" };
			}
		}

		/// <summary>
		/// get otp forget password
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>

		[HttpPost]
		[Route("get-token-by-email")]
		[AllowAnonymous]
		public async Task<ActionResult<object>> GetTokenByEmail(GetTokenByEmail request)
		{
			try
			{
				
					int length = 6;
					string res = string.Empty;
					Random rnd = new Random();
                    for (int i = 0; i < length; i++)
                    {
						res+=rnd.Next(10);
					}

					// lưu db

					var config = (await new SYConfig(_appSetting).SYConfigGetByTypeDAO(TYPECONFIG.CONFIG_EMAIL));
					if (config.Count == 0)
					{
						return new ResultApi { Success = ResultCode.ORROR, Result = -1, Message = "Config email not exits" };
					}
					else
					{
						// gửi mail
						var configEmail = JsonConvert.DeserializeObject<ConfigEmail>(config[0].Content);
					string content =
						"<p style ='font-family:times new roman,times,serif'>Mã OTP xác thực của bạn là  {OTP}</p>";
						content = content.Replace("{OTP}", res.ToString());
                        if (request.Type == 1)
						{
							MailHelper.SendMail(configEmail, request.Email, "Đăng ký tài khoản", content, null);
						}
						else if (request.Type == 2)
						{
							MailHelper.SendMail(configEmail, request.Email, "Quên mật khẩu", content, null);
						}

						return new ResultApi { Success = ResultCode.OK, Result = res, Message = ResultMessage.OK };

					}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = "An error occurred" };
			}
		}
	}
}
