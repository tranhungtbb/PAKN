using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Services
{
    public class ClientRateLimit
    {
        private readonly IpRateLimitOptions _options;
        private readonly IIpPolicyStore _ipPolicyStore;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IConfiguration _config;

        public ClientRateLimit(IOptions<IpRateLimitOptions> optionsAccessor, IIpPolicyStore ipPolicyStore, IHttpContextAccessor contextAccessor, IConfiguration configuration) {
            _options = optionsAccessor.Value;
            _ipPolicyStore = ipPolicyStore;
            _contextAccessor = contextAccessor;
            _config = configuration;
        }

        public async Task AddClientRateLimitAsync() {
            var IpCurrent = _contextAccessor.HttpContext.Connection.RemoteIpAddress;
            var pol = await _ipPolicyStore.GetAsync(_options.IpPolicyPrefix);

            if (!pol.IpRules.Exists(x => x.Ip == IpCurrent.ToString()))
            {
                pol.IpRules.Add(new IpRateLimitPolicy
                {
                    Ip = IpCurrent.ToString(),
                    Rules = new List<RateLimitRule>(new RateLimitRule[] {
                        new RateLimitRule {
                            Endpoint = "*",
                            Limit = Convert.ToInt64(_config["LimitRequest"]),
                            Period = _config["Period"]
                        }
                    })
                });
                await _ipPolicyStore.SetAsync(_options.IpPolicyPrefix, pol);
            }
        }
    }
}
