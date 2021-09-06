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

namespace PAKNAPI.Controllers
{
    [Route("api/contact")]
	[ApiController]
	[ValidateModel]
	public class ContactController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private const string _defaultAvatar = "/Content/images/avatardefaultactived_blue.png";
		private readonly IHttpContextAccessor _context;

		private static string textRamdom = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static string numberRamdom = "1234567890";
		private static string specialCharacters = "!@#$%^&*";

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
			try
			{
				var httpRequest = _context.HttpContext.Request;

				List<SYUSRLogin> user = await new SYUSRLogin(_appSetting).SYUSRLoginDAO(loginIN.UserName);

				if (user != null && user.Count > 0)
				{
					if (!(bool)user[0].IsActived) {
						return new ResultApi
						{
							Success = ResultCode.ORROR,
							Result = 0,
							Message = "Tài khoản của bạn đang hết hiệu lực"
						};
					}
					PasswordHasher hasher = new PasswordHasher();
					if (hasher.AuthenticateUser(loginIN.Password, user[0].Password, user[0].Salt))
					{

						var tokenString = GenerateJSONWebToken(user[0]);
						List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId = 
							await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(Int32.Parse(user[0].Id.ToString()));
						if (rsSYUSRGetPermissionByUserId != null && rsSYUSRGetPermissionByUserId.Count > 0)
						{
							BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(HttpContext.Request);
							//var s = Request.Headers;
							SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
							{
								UserId = user[0].Id,
								FullName = user[0].FullName,
								Action = "Login",
								IPAddress = baseRequest.ipAddress,
								MACAddress = baseRequest.macAddress,
								Description = baseRequest.logAction,
								CreatedDate = DateTime.Now,
								Status = 1,
								Exception = null
							};

							

							if (user[0].IsActived == false) { sYSystemLogInsertIN.Status = 0; };
							await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
							SYUserUserAgent query = new SYUserUserAgent(user[0].Id, Request.Headers["User-Agent"].ToString(), baseRequest.ipAddress, true);
							await new SYUserUserAgent(_appSetting).SYUserUserAgentInsertDAO(query);

							return new LoginResponse
							{
								Success = ResultCode.OK,
								UserId = user[0].Id,
								UserName = user[0].UserName,
								FullName = user[0].FullName,
								Email = user[0].Email,
								IsActive = user[0].IsActived,
								Phone = user[0].UserName,
								UnitId = user[0].UnitId,
								UnitName = user[0].UnitName,
								IsMain = user[0].IsMain,
								IsAdmin = user[0].IsAdmin,
								TypeObject = user[0].TypeObject,
								AccessToken = tokenString,
								IsHaveToken = true,
								Permissions = rsSYUSRGetPermissionByUserId[0].Permissions,
								PermissionCategories = rsSYUSRGetPermissionByUserId[0].PermissionCategories,
								PermissionFunctions = rsSYUSRGetPermissionByUserId[0].PermissionFunctions
							};
						}
						else {
							BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(HttpContext.Request);

							SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
							{
								UserId = user[0].Id,
								FullName = user[0].FullName,
								Action = "Login",
								IPAddress = baseRequest.ipAddress,
								MACAddress = baseRequest.macAddress,
								Description = baseRequest.logAction,
								CreatedDate = DateTime.Now,
								Status = 0,
								Exception = null
							};
							await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
							return new ResultApi { Success = ResultCode.INCORRECT, Message = "User not permissions", };

						}

					}
					else
					{
						BaseRequest baseRequest = new LogHelper(_appSetting).ReadBodyFromRequest(HttpContext.Request);

						SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
						{
							UserId = user[0].Id,
							FullName = user[0].FullName,
							Action = "Login",
							IPAddress = baseRequest.ipAddress,
							MACAddress = baseRequest.macAddress,
							Description = baseRequest.logAction,
							CreatedDate = DateTime.Now,
							Status = 0,
							Exception = null
						};
						await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
						//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception(), loginIN.UserName);

						return new ResultApi
						{
							Success = ResultCode.ORROR,
							Result = 0,
							Message = "Tên tài khoản hoặc mật khẩu sai"
						};
					}
				}
				else
				{
					return StatusCode(200, new ResultApi
					{
						Success = ResultCode.ORROR,
						Result = 0,
						Message = "Tên tài khoản hoặc mật khẩu sai"
					});
				}
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = "ERROR: " + ex.Message, };
			}
		}

		private string GenerateJSONWebToken(SYUSRLogin userInfo)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			if (userInfo.UnitId == null) { userInfo.UnitId = 0; };
			if (userInfo.Email == null) { userInfo.Email = ""; };
			var claims = new[] {
				new Claim("UserName", userInfo.UserName),
				new Claim("FullName", userInfo.FullName),
				new Claim("Type", userInfo.Type.ToString()),
				new Claim("UnitId", userInfo.UnitId.ToString()),
				new Claim("Email", userInfo.Email),
				new Claim("Id", userInfo.Id.ToString())
			};

			var token = new JwtSecurityToken(
				_config["Jwt:Issuer"],
				_config["Jwt:Issuer"],
				claims,
				expires: DateTime.Now.AddDays(20),
				signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		/// <summary>
		/// đăng xuất
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("log-out")]
		[Authorize("ThePolicy")]
		public async Task<ActionResult<object>> LogOut(EditUserRequest request)
		{
            try
			{
				//long? UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				//JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
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
				List<SYUSRLogin> user = await new SYUSRLogin(_appSetting).SYUSRLoginDAO(request.Email);
				if (user.Count == 0)
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = 0, Message = "Không tồn tại email" };
				}
				else {
					int length = 3;
					
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
					length = 2;
					while (0 < length--)
					{
						res.Append(specialCharacters[rnd.Next(specialCharacters.Length)]);
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
						await new SYUserUserAgent(_appSetting).SYUserUserAgentUpdateStatusDAO(user[0].Id);

						SYUserUserAgent sy_UserAgent = new SYUserUserAgent(user[0].Id, Request.Headers["User-Agent"].ToString(), Request.Headers["ipAddress"].ToString(), true);
						await new SYUserUserAgent(_appSetting).SYUserUserAgentInsertDAO(sy_UserAgent);


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
	}
}
