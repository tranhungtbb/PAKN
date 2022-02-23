namespace PAKNAPI.Controllers
{
    public class BaseApiController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		private readonly Bugsnag.IClient _bugsnag;
		public BaseApiController()
		{
		}
		public BaseApiController(Bugsnag.IClient client)
		{
			_bugsnag = client;
		}
	}
}
