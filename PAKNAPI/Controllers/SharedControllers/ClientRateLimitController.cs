using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers.SharedControllers
{
    [ApiController]
    [Route("api/client-rate-limit")]
    public class IpRateLimitController : BaseApiController
    {
        private readonly IpRateLimitOptions _options;
        private readonly IIpPolicyStore _ipPolicyStore;
        private readonly IRateLimitConfiguration _config;



        public IpRateLimitController(IOptions<IpRateLimitOptions> optionsAccessor, IIpPolicyStore ipPolicyStore, IRateLimitConfiguration config)
        {
            _options = optionsAccessor.Value;
            _ipPolicyStore = ipPolicyStore;
            _config = config;
        }

        [HttpGet]
        [Route("get")]
        public async Task<IpRateLimitPolicies> Get()
        {
            var s = await _ipPolicyStore.GetAsync(_options.IpPolicyPrefix);
            return s;
        }
    }
}
