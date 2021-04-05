using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PAKNAPI.Common;
using PAKNAPI.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
namespace PAKNAPI.Controllers
{
    [Route("api/[controller]")]
    public class CaptchaController
    {
        private readonly IAppSetting _appSetting;
        public static List<CaptchaObject> captChaCode = new List<CaptchaObject>();

        public CaptchaController(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }
        [Route("GetCaptchaImage")]
        [HttpGet]
        public IActionResult GetCaptchaImage()
        {
            try
            {
                int width = 200;
                int height = 60;
                var captchaCode = new Captcha(_appSetting).GenerateCaptchaCode();
                var result = new Captcha(_appSetting).GenerateCaptchaImage(width, height, captchaCode);
                new Captcha(_appSetting).InsertCaptcha(result.CaptchaCode);
                Stream s = new MemoryStream(result.CaptchaByteData);
                return new FileStreamResult(s, "image/png");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex);
            }

        }

        [Route("ValidatorCaptcha")]
        [HttpGet]
        public ActionResult<object> ValidatorCaptcha(string CaptchaCode)
        {

            if (!new Captcha(_appSetting).ValidateCaptchaCode(CaptchaCode, captChaCode))
            {
                new Captcha(_appSetting).DeleteCaptcha("");
                return new ResultApi { Success = ResultCode.ORROR };
            }
            else
            {
                new Captcha(_appSetting).DeleteCaptcha(CaptchaCode);
                return new ResultApi { Success = ResultCode.OK };
            }
        }

    }
}
