using Dapper;
using SampleAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SampleAPI.Common
{
	public class BaseRequest
	{
		public string logAction { get; set; }
		public string logObject { get; set; }
		public string ipAddress { get; set; }
		public string macAddress { get; set; }
		public string location { get; set; }
	}
}
