using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResCodeController : BaseApiController
    {
        [Route("Get")]
        [HttpGet]
        public ActionResult Get(int code)
        {
            return StatusCode(code);
        }
    }
}
