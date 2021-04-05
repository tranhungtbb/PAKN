using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Web.Http.Controllers;
using System.Web.Http.Results;

namespace PAKNAPI.AuthorizePermission
{
	public class AuthorizePermissionAttribute : Attribute
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			//Custom code ...
			var ab = context.HttpContext.Items["User"];

			// get api name từ context
			//check userid có dược call api name này không




			//string apiName = context.ActionDescriptor.Request.RequestUri.Segments[3];
			//Resolving a custom Services from the container
			//var service = context.HttpContext.RequestServices.GetRequiredService<ISample>();
			//string name = service.GetName(); // returns "anish"

			////Return based on logic
			//context.Result = new UnauthorizedResult();
		}
		//public void OnAuthorization(AuthorizationFilterContext context)
		//{
		//    var user = (User)context.HttpContext.Items["User"];
		//    if (user == null)
		//    {
		//        // not logged in
		//        context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
		//    }
		//}
		//protected override bool IsAuthorized(HttpActionContext actionContext)
		//{
		//    bool isAuthroized = base.IsAuthorized(actionContext);
		//    string apiName = actionContext.Request.RequestUri.Segments[3];

		//    ////Run only for dev. But production then remove
		//    //new DevInsertAPICategory().DevInsertAutoAPICategory(apiName);

		//    //bool check = ValidateKey(apiName);
		//    bool check = true;

		//    return (isAuthroized && check);
		//}

		//private bool ValidateKey(string apiName)
		//{
		//    Guid? employeeID = ContactModel.GetCurrentUserEmployerID();
		//    CheckPermissionByEmployeeID rsCheckPermissionByEmployeeID = (new CheckPermissionByEmployeeID().CheckPermissionByUserIDDAO(apiName, employeeID)).FirstOrDefault();

		//    if (rsCheckPermissionByEmployeeID == null || rsCheckPermissionByEmployeeID.Permission == null || rsCheckPermissionByEmployeeID.Permission <= 0)
		//    {
		//        return false;
		//    }

		//    return true;
		//}
	}
}