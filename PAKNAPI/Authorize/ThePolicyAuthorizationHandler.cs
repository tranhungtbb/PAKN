using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using PAKNAPI.Common;
using PAKNAPI.Model;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using PAKNAPI.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace PAKNAPI.Authorize
{
	public class ThePolicyAuthorizationHandler : AuthorizationHandler<ThePolicyRequirement>
	{
		//readonly HttpActionContext _appContext;
		readonly IHttpContextAccessor _contextAccessor;
		private readonly IAppSetting _appSetting;
		private readonly IOptions<IpRateLimitOptions> _options;
		private readonly IIpPolicyStore _ipPolicyStore;
		private readonly IConfiguration _config;
		public ThePolicyAuthorizationHandler(IHttpContextAccessor ca, IAppSetting appSetting, IOptions<IpRateLimitOptions> options, IIpPolicyStore ipPolicyStore ,IConfiguration config)
		{
			//_appContext = c;
			_contextAccessor = ca;
			_appSetting = appSetting;
			_options = options;
			_ipPolicyStore = ipPolicyStore;
			_config = config;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ThePolicyRequirement requirement)
		{
			var result = requirement.isPass(_contextAccessor, context);
			var json = JsonSerializer.Serialize(new ResultApi { Success = ResultCode.ORROR, Message = "Authorization fail" });
			var bytes = Encoding.UTF8.GetBytes(json);
			//check IsAuthorized
			var IsAuthenticated = context.User.Identity.IsAuthenticated;

			//string APIName = _contextAccessor.HttpContext.Request.Path;
			Claim userId = context.User.Claims.FirstOrDefault(claim => claim.Type == "Id");
			var filterContext = context.Resource as AuthorizationFilterContext;
			var response = filterContext?.HttpContext.Response;

			new ClientRateLimit(_options, _ipPolicyStore, _contextAccessor, _config).AddClientRateLimitAsync().Wait();

			//bool permission = false;
			if (userId != null)
			{
				//Insert to API table, only using for dev enviroment
				//SYAPIInsertIN _sYAPIInsertIN = new SYAPIInsertIN { Name = APIName, Authorize = true };
				//await new SYAPIInsert(_appSetting).SYAPIInsertDAO(_sYAPIInsertIN);

				//Check permission
				//List<SYPermissionCheckByUserId> rsSYPermissionCheckByUserId = await new SYPermissionCheckByUserId(_appSetting).SYPermissionCheckByUserIdDAO(Int32.Parse(userId.Value.ToString()), APIName).;

				//if (rsSYPermissionCheckByUserId.Count > 0) permission = rsSYPermissionCheckByUserId[0].Permission > 0;

				// check unit is Active
				var unitId = context.User.Claims.FirstOrDefault(claim => claim.Type == "UnitId").Value;
				if (!string.IsNullOrEmpty(unitId))
				{
					var unit = new SYUnit(_appSetting).SYUnitGetByID(Convert.ToInt32(unitId));
					if (unit != null)
					{
						if (unit.Result != null && !unit.Result.IsActived) {
							_contextAccessor.HttpContext.Response.StatusCode = 401;
							_contextAccessor.HttpContext.Response.ContentType = "application/json; charset=utf-8";
							_contextAccessor.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
							return Task.CompletedTask;
						}
					}
				}


				// check user isActived

				var user = new SYUserGetByID(_appSetting).SYUserGetByIDDAO(Convert.ToInt64(userId.Value));
				if (user.Result.Count() == 0)
				{
					_contextAccessor.HttpContext.Response.StatusCode = 401;
					_contextAccessor.HttpContext.Response.ContentType = "application/json; charset=utf-8";
					_contextAccessor.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
					return Task.CompletedTask;
				}
				else {
					if (user.Result[0].IsActived == false) {
						_contextAccessor.HttpContext.Response.StatusCode = 401;
						_contextAccessor.HttpContext.Response.ContentType = "application/json; charset=utf-8";
						_contextAccessor.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
						return Task.CompletedTask;
					}
				}

				LogHelper logHelper = new LogHelper(_appSetting);

				// check đã logout

				var s = new RefreshTokens(_appSetting).GetByJwtToken(context.User.Claims.FirstOrDefault(claim => claim.Type == "jti").Value);
				if (s.Result.IsRevoked) {
					// token đã thu hồi
					_contextAccessor.HttpContext.Response.StatusCode = 401;
					_contextAccessor.HttpContext.Response.ContentType = "application/json; charset=utf-8";
					_contextAccessor.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
					return Task.CompletedTask;
				}

				// check thiết bị

				
				SYUserUserAgent query = new SYUserUserAgent(Convert.ToInt32(userId.Value), _contextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1]);
				var check = (new SYUserUserAgent(_appSetting).SYUserUserAgentGetDAO(query)).Result.FirstOrDefault();

				if (check != null) {
					if (check.Status == true)
					{
						context.Succeed(requirement);
					}
					else {
						_contextAccessor.HttpContext.Response.StatusCode = 401;
						_contextAccessor.HttpContext.Response.ContentType = "application/json; charset=utf-8";
						_contextAccessor.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length);
						return Task.CompletedTask;
					}
				}
				
			}
			context.Succeed(requirement);
			return Task.CompletedTask;
		}
	}
}
