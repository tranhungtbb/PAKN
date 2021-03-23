using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.Models;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using System;
using Dapper;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace PAKNAPI.ControllerBase
{
	[Route("api/SYCapchaSPBase")]
	[ApiController]
	public class SYCapchaSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public SYCapchaSPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}
	}
}
