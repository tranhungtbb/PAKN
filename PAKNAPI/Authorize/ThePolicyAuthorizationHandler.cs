using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace PAKNAPI.Authorize
{
	public class ThePolicyAuthorizationHandler : AuthorizationHandler<ThePolicyRequirement>
	{
		//readonly HttpActionContext _appContext;
		readonly IHttpContextAccessor _contextAccessor;
		private readonly IAppSetting _appSetting;

		public ThePolicyAuthorizationHandler(IHttpContextAccessor ca, IAppSetting appSetting)
		{
			//_appContext = c;
			_contextAccessor = ca;
			_appSetting = appSetting;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ThePolicyRequirement requirement)
		{
			var result = await requirement.isPass(_contextAccessor, context);

			//check IsAuthorized
			var IsAuthenticated = context.User.Identity.IsAuthenticated;

			string APIName = _contextAccessor.HttpContext.Request.Path;
			Claim userId = context.User.Claims.FirstOrDefault(claim => claim.Type == "Id");
			var filterContext = context.Resource as AuthorizationFilterContext;
			var response = filterContext?.HttpContext.Response;

			bool permission = false;
			if (userId != null)
			{
				//Insert to API table, only using for dev enviroment
				SYAPIInsertIN _sYAPIInsertIN = new SYAPIInsertIN { Name = APIName, Authorize = true };
				//await new SYAPIInsert(_appSetting).SYAPIInsertDAO(_sYAPIInsertIN);

				//Check permission
				List<SYPermissionCheckByUserId> rsSYPermissionCheckByUserId = await new SYPermissionCheckByUserId(_appSetting).SYPermissionCheckByUserIdDAO(Int32.Parse(userId.Value.ToString()), APIName);

				if (rsSYPermissionCheckByUserId.Count > 0) permission = rsSYPermissionCheckByUserId[0].Permission > 0;

				// check user isActived

				var user = await new SYUserGetByID(_appSetting).SYUserGetByIDDAO(Convert.ToInt64(userId.Value));
				if (user.Count() == 0)
				{
					response?.OnStarting(async () =>
					{
						filterContext.HttpContext.Response.StatusCode = 401;
					});
					return;
				}
				else {
					if (user[0].IsActived == false) {
						response?.OnStarting(async () =>
						{
							filterContext.HttpContext.Response.StatusCode = 401;
						});
						return;
					}
				}

				// check thiết bị

				LogHelper logHelper = new LogHelper(_appSetting);
				BaseRequest baseRequest = logHelper.ReadHeaderFromRequest(_contextAccessor.HttpContext.Request);
				SYUserUserAgent query = new SYUserUserAgent(Convert.ToInt32(userId.Value), _contextAccessor.HttpContext.Request.Headers["User-Agent"], baseRequest.ipAddress);
				var check = (await new SYUserUserAgent(_appSetting).SYUserUserAgentGetDAO(query)).FirstOrDefault();

				if (check != null) {
					if (check.Status == true)
					{
						context.Succeed(requirement);
					}
					else {
						response?.OnStarting(async () =>
						{
							filterContext.HttpContext.Response.StatusCode = 401;
						});
						return;
					}
				}
				
			}
			context.Succeed(requirement);
		}
	}
}
