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
using Bugsnag;
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.ModelBase;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/NENews")]
    [ApiController]
    public class NENewsController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;

        public NENewsController(IAppSetting appSetting, IClient bugsnag)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
        }



		[HttpGet]
		[Route("NENewsGetListHomePage")]
		public async Task<ActionResult<object>> NENewsGetListHome()
		{
			try
			{
				List<NENewsGetListHomePage> rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage(true);
				if (rsPUNewsGetListHomePage.Count < 4)
				{
					rsPUNewsGetListHomePage = await new NENewsGetListHomePage(_appSetting).PU_NewsGetListHomePage(false);
				}
				return new ResultApi { Success = ResultCode.OK, Result = rsPUNewsGetListHomePage };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}
}
