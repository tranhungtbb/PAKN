using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
			//var result = await requirement.isPass(_contextAccessor, context);

			//check IsAuthorized
			var IsAuthenticated = context.User.Identity.IsAuthenticated;

			//check userIsAnonymous
			//var userIsAnonymous = false;
			//var endpoint = _contextAccessor.HttpContext.GetEndpoint();
			//if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
			//{
			//	userIsAnonymous = true;
			//}

			//var api = _contextAccessor.HttpContext.Request.Path;
			//var user = context.User;
			//var user = context.User HttpContext.Items["User"];
			System.Security.Claims.ClaimsPrincipal currentUser = context.User;
			string APIName = _contextAccessor.HttpContext.Request.Path;
			Claim userId = context.User.Claims.FirstOrDefault(claim => claim.Type == "UserId");
			//Insert to API table, only using for dev enviroment
			SYAPIInsertIN _sYAPIInsertIN = new SYAPIInsertIN { Name = APIName, Authorize = true };
			await new SYAPIInsert(_appSetting).SYAPIInsertDAO(_sYAPIInsertIN);

			var mvcContext = context.Resource as AuthorizationHandlerContext;
			var authorizationFilterContext = context.Resource as HttpContext;
			//var descriptor = mvcContext?.ActionDescriptor as ControllerActionDescriptor;
			//bool isAuthroized = base.IsAuthorized(context);
			//var currentUserName = currentUser.FindFirst(JwtRegisteredClaimNames.FamilyName).Value;
			//string Sub = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub];
			//string UniqueName = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.UniqueName];
			//string FamilyName = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.FamilyName];
			//string GivenName = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.GivenName];

			//Claim username = context.User.Claims.FirstOrDefault(claim => claim.Type == Sub);
			//Claim fullnam = context.User.Claims.FirstOrDefault(claim => claim.Type == UniqueName);
			//Claim email = context.User.Claims.FirstOrDefault(claim => claim.Type == FamilyName);
			//Claim userId = context.User.Claims.FirstOrDefault(claim => claim.Type == GivenName);
			if (IsAuthenticated)
				context.Succeed(requirement);
			else
				context.Fail();
		}
	}
}
