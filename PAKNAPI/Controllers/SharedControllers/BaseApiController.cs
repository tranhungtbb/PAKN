namespace PAKNAPI.Controllers
{
    public class BaseApiController : Microsoft.AspNetCore.Mvc.ControllerBase
	{
		//public ISqlCon _connection;
		//IConfiguration _configuration;
		public BaseApiController()
		{
			//this._connection = new ESConnection(_configuration.GetConnectionString("Default"));
		}
	}
}
