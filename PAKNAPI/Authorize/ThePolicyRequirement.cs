using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace PAKNAPI.Authorize
{
	public class ThePolicyRequirement : IAuthorizationRequirement
	{
		IHttpContextAccessor _contextAccessor;
		AuthorizationHandlerContext _authHandlerContext;

		public async Task<bool> isPass(IHttpContextAccessor contextAccessor, AuthorizationHandlerContext context)
		{
			//_contextAccessor = contextAccessor;
			//string apiName = actionContext.Request.RequestUri.Segments[3];
			string apiName = "";
			string Sub = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.Sub];
			string UniqueName = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.UniqueName];
			string FamilyName = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.FamilyName];
			string GivenName = JwtSecurityTokenHandler.DefaultInboundClaimTypeMap[JwtRegisteredClaimNames.GivenName];

			//Claim username = context.User.Claims.FirstOrDefault(claim => claim.Type == Sub);
			//Claim fullnam = context.User.Claims.FirstOrDefault(claim => claim.Type == UniqueName);
			//Claim email = context.User.Claims.FirstOrDefault(claim => claim.Type == FamilyName);
			Claim Id = context.User.Claims.FirstOrDefault(claim => claim.Type == "Id");
			Claim UserName = context.User.Claims.FirstOrDefault(claim => claim.Type == "UserName");
			Claim FullName = context.User.Claims.FirstOrDefault(claim => claim.Type == "FullName");
			Claim Type = context.User.Claims.FirstOrDefault(claim => claim.Type == "Type");
			Claim UnitId = context.User.Claims.FirstOrDefault(claim => claim.Type == "UnitId");
			Claim Email = context.User.Claims.FirstOrDefault(claim => claim.Type == "Email");
			Claim exp = context.User.Claims.FirstOrDefault(claim => claim.Type == "exp");
			Claim iss = context.User.Claims.FirstOrDefault(claim => claim.Type == "iss");
			Claim aud = context.User.Claims.FirstOrDefault(claim => claim.Type == "aud");

			_authHandlerContext = context;
			var user = context.User;
			//logic here
			bool check = ValidateKey(apiName);

			return check;
		}

		private bool ValidateKey(string apiName)
		{
			//Guid? employeeID = ContactModel.GetCurrentUserEmployerID();
			//CheckPermissionByEmployeeID rsCheckPermissionByEmployeeID = (new CheckPermissionByEmployeeID().CheckPermissionByUserIDDAO(apiName, employeeID)).FirstOrDefault();

			//if (rsCheckPermissionByEmployeeID == null || rsCheckPermissionByEmployeeID.Permission == null || rsCheckPermissionByEmployeeID.Permission <= 0)
			//{
			//    return false;
			//}

			return true;
		}
	}
}
