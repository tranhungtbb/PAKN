using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Common
{
	public class ConnectString
	{
		public string Default;
	}

	public interface IAppSetting
	{
		public string GetConnectstring();
		public UrlFileSupport GetUrlFileSupports();
	}

	public class AppSetting : IAppSetting
	{
		private readonly IConfiguration Configuration;

        public AppSetting()
        {
        }

        public AppSetting(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string GetConnectstring()
		{
			return Configuration["ConnectionStrings:Default"];
		}

		public UrlFileSupport GetUrlFileSupports()
		{
			return Configuration.GetSection("UrlFileSupport").Get<UrlFileSupport>();
		}
	}
}
