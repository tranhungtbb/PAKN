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

        private readonly IAppSetting _appSetting;

        public WeatherController(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }

        [Route("get")]
        public async Task<object> getWeatherAsync(string lat, string lon)
        {
            var config = new WeatherConfigModel
            {
                api = "http://api.openweathermap.org/data/2.5/weather",
                appid = "3203582b0e1e98b17f97b639bcef2350"
            };

            try
            {
                var _config = await new SYConfig(_appSetting).SYConfigGetByTypeDAO(4);
                config = JsonConvert.DeserializeObject<WeatherConfigModel>(_config.FirstOrDefault().Content);
            }
            catch
            {

            }


            var data = await this.getWeatherData(lat,lon, config);

            IDictionary<string, object> json = new Dictionary<string, object>
            {
                {"Data", data},
            };

            return new ResultApi { Success = ResultCode.OK, Result = json };
        }

        [Route("getByQ")]
        public async Task<object> getByQ(string q= "Nha Trang, VN")
        {
            var config = new WeatherConfigModel
            {
                api = "http://api.openweathermap.org/data/2.5/weather",
                appid = "3203582b0e1e98b17f97b639bcef2350"
            };

            try
            {
                var _config = await new SYConfig(_appSetting).SYConfigGetByTypeDAO(4);
                config = JsonConvert.DeserializeObject<WeatherConfigModel>(_config.FirstOrDefault().Content);
            }catch{}


            var data = await this.getWeatherDataByQ(q, config);

            IDictionary<string, object> json = new Dictionary<string, object>
            {
                {"Data", data},
            };

            return new ResultApi { Success = ResultCode.OK, Result = json };
        }

        private async Task<object> getWeatherDataByQ(string q,WeatherConfigModel config)
        {
            try
            {

                var url = $"{config.api}?appid={config.appid}&q={q}";
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

        private async Task<object> getWeatherData(string lat, string lon, WeatherConfigModel config)
        {
            try
            {

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
