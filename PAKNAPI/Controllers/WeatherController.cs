using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PAKNAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : BaseApiController
    {
        static readonly HttpClient client = new HttpClient();

        [Route("get")]
        public async Task<object> getWeatherAsync(string lat, string lon)
        {
            var data = await this.getWeatherData(lat,lon);

            IDictionary<string, object> json = new Dictionary<string, object>
            {
                {"Data", data},
            };

            return new ResultApi { Success = ResultCode.OK, Result = json };
        }


        private async Task<object> getWeatherData(string lat, string lon)
        {
            try
            {

                //var _config = await new SYConfig().SYConfigGetByTypeDAO(4);
                //var config = JsonConvert.DeserializeObject<WeatherConfigModel>(_config.FirstOrDefault().Content);

                var config = new WeatherConfigModel
                {
                    api = "http://api.openweathermap.org/data/2.5/weather",
                    appid = "3203582b0e1e98b17f97b639bcef2350"
                };


                var url = $"{config.api}?appid={config.appid}&lat={lat}&lon={lon}";
                HttpResponseMessage response = await client.GetAsync(url);


                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                return responseBody;

            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }
    }
}
