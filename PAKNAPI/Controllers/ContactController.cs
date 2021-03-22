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

		public ContactController(IAppSetting appSetting, IHttpContextAccessor context, IConfiguration config)
		{
			_appSetting = appSetting;
			_context = context;
			_config = config;
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
						new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
						return new LoginResponse
						{
							Success = ResultCode.OK,
							UserName = user[0].UserName,
							FullName = user[0].FullName,
							Email = user[0].Email,
							Phone = user[0].UserName,
							AccessToken = tokenString,
							Permissions = rsSYUSRGetPermissionByUserId,
						};
					}
					else
					{
						return new ResultApi { Message = "User/Pass not found", };
					}
				}
				else
				{
					return new ResultApi { Message = "User/Pass not found", };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);
				return new ResultApi { Message = "An error occurred", };
			}
		}

		private string GenerateJSONWebToken(SYUSRLogin userInfo)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[] {
				new Claim("UserName", userInfo.UserName),
				new Claim("FullName", userInfo.FullName),
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

            }
            catch (Exception ex)
			{
				return new ResultApi { Message = "An error occurred", };
			}
		}
	}
}
