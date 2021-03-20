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
	[Route("api/SYLogSPBase")]
	[ApiController]
	public class SYLogSPBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public SYLogSPBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}
	}
}
