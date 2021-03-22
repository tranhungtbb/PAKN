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
		public async Task<ActionResult<object>> LoginThe(LoginIN loginIN)
		{
			var httpRequest = _context.HttpContext.Request;

			List<SYLogin> user = await new SYLogin(_appSetting).SYLoginDAO(loginIN.UserName, loginIN.Password);

			if (user != null && user.Count > 0)
			{
				var tokenString = GenerateJSONWebToken(user[0]);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{ "Id", user[0].Id },
						{ "UserName", user[0].UserName },
						{ "FullName", user[0].FullName },
						{ "Email", user[0].Email },
						{ "Phone", user[0].Phone },
						{ "Token", tokenString},
					};
				return JsonConvert.SerializeObject(json);
			}
			else
			{
				return new ResultApi { Message = "User/Pass not found", };
			}
		}

		[Route("Login")]
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<object>> Login(LoginIN loginIN)
		{
			var httpRequest = _context.HttpContext.Request;

			List<SYLogin> user = await new SYLogin(_appSetting).SYLoginDAO(loginIN.UserName, loginIN.Password);

			if (user != null && user.Count > 0)
			{
				var tokenString = GenerateJSONWebToken(user[0]);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{ "Id", user[0].Id },
						{ "UserName", user[0].UserName },
						{ "FullName", user[0].FullName },
						{ "Email", user[0].Email },
						{ "Phone", user[0].Phone },
						{ "Token", tokenString},
					};
				return JsonConvert.SerializeObject(json);
			}
			else
			{
				return new ResultApi { Message = "User/Pass not found", };
			}
		}

		private string GenerateJSONWebToken(SYLogin userInfo)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[] {
				new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
				new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.FullName),
				new Claim(JwtRegisteredClaimNames.FamilyName, userInfo.Email),
				new Claim(JwtRegisteredClaimNames.GivenName, userInfo.Id.ToString())
			};

			var token = new JwtSecurityToken(
				_config["Jwt:Issuer"],
				_config["Jwt:Issuer"],
				claims,
				expires: DateTime.Now.AddDays(20),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}

	public class LoginIN
	{
		public string Password { get; set; }
		public string UserName { get; set; }
	}
}
