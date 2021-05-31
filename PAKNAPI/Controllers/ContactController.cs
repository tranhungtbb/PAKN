using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
	[Route("api/Contact")]
	[ApiController]
	public class ContactController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		private const string _defaultAvatar = "/Content/images/avatardefaultactived_blue.png";
		private readonly IHttpContextAccessor _context;

		private IConfiguration _config;
		private readonly Bugsnag.IClient _bugsnag;

		public ContactController(IAppSetting appSetting, IHttpContextAccessor context, IConfiguration config, Bugsnag.IClient client)
		{
			_appSetting = appSetting;
			_context = context;
			_config = config;
			_bugsnag = client;
		}

		[Route("Login")]
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
					PasswordHasher hasher = new PasswordHasher();
					if (hasher.AuthenticateUser(loginIN.Password, user[0].Password, user[0].Salt))
					{

						var tokenString = GenerateJSONWebToken(user[0]);
						List<SYUSRGetPermissionByUserId> rsSYUSRGetPermissionByUserId = await new SYUSRGetPermissionByUserId(_appSetting).SYUSRGetPermissionByUserIdDAO(Int32.Parse(user[0].Id.ToString()));
						IDictionary<string, object> json = new Dictionary<string, object>
						{
							{ "Id", user[0].Id },
							{ "UserName", user[0].UserName },
							{ "FullName", user[0].FullName },
							{ "Email", user[0].Email },
							{ "Phone", user[0].Phone },
							{ "AccessToken", tokenString},
							{ "Permissions", rsSYUSRGetPermissionByUserId},
						};



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
							Status = 1,
							Exception = null
						};

						if (user[0].IsActived == false) { sYSystemLogInsertIN.Status = 0; };
						await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);


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
							TypeObject = user[0].TypeObject,
							AccessToken = tokenString,
							IsHaveToken = true,
							Permissions = rsSYUSRGetPermissionByUserId,
						};
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
						return new ResultApi { Success = ResultCode.INCORRECT, Message = "User/Pass not found", };
					}
				}
				else
				{
                    //new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception(), loginIN.UserName);
                    return new ResultApi { Success = ResultCode.INCORRECT, Message = "User/Pass not found", };
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

		[HttpPost]
		[Route("LogOut")]
		[Authorize]
		public async Task<ActionResult<object>> LogOut(EditUserRequest request)
		{
            try
			{
				long? UserId = new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK };
			}
            catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				return new ResultApi { Success = ResultCode.ORROR, Message = "An error occurred" };
			}
		}
	}
}
