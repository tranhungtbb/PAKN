namespace PAKNAPI.Controllers
{
    public class BaseApiController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		//public ISqlCon _connection;
		//IConfiguration _configuration;

		private readonly Bugsnag.IClient _bugsnag;
		public BaseApiController()
		{
			//this._connection = new ESConnection(_configuration.GetConnectionString("Default"));
		}
		public BaseApiController(Bugsnag.IClient client)
		{
			_bugsnag = client;
		}
	}
}
