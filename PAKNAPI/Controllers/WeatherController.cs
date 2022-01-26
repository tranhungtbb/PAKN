﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;

namespace PAKNAPI.Controllers
{
    [Route("api/weather")]
    [ApiController]
    [OpenApiTag("Thời tiết", Description = "Thời tiết")]
    public class WeatherController : BaseApiController
    {
        static readonly HttpClient client = new HttpClient();

        private readonly IAppSetting _appSetting;
        private readonly IConfiguration _config;

        public WeatherController(IAppSetting appSetting, IConfiguration config)
        {
            _appSetting = appSetting;
            _config = config;
        }
        /// <summary>
        /// thông tin thời tiết
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>

        [Route("get")]
        public async Task<object> getWeatherAsync(string lat, string lon)
        {
            var config = new WeatherConfigModel { 
                api = _config["ConfigWeather:api"],
                appid = _config["ConfigWeather:appid"],
            };



            var data = await this.getWeatherData(lat,lon, config);

            IDictionary<string, object> json = new Dictionary<string, object>
            {
                {"Data", data},
            };

            return new ResultApi { Success = ResultCode.OK, Result = json };
        }
        /// <summary>
        /// thông tin thời tiết theo thành phố
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>

        [Route("get-by-q")]
        public async Task<object> getByQ(string q= "Nha Trang, VN")
        {
            var config = new WeatherConfigModel
            {
                api = _config["ConfigWeather:api"],
                appid = _config["ConfigWeather:appid"],
            };

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
