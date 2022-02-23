//using AspNetCoreRateLimit;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;

//namespace PAKNAPI.Middle
//{
//    public class CustomRateLimitConfiguration : RateLimitConfiguration
//    {
//        public override void RegisterResolvers()
//        {
//            base.RegisterResolvers();

//            ClientResolvers.Add(new ClientQueryStringResolveContributor(HttpContextAccessor, ClientRateLimitOptions.ClientIdHeader));
//        }
//    }
//}
