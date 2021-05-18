using PAKNAPI.Common;
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
using PAKNAPI.Models.ModelBase;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace PAKNAPI.Controllers
{
    [Route("api/Files")]
    [ApiController]
   
    public class FilesController : BaseApiController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;

        public FilesController(IWebHostEnvironment hostingEnvironment, IClient bugsnag)
        {
            _hostingEnvironment = hostingEnvironment;
            _bugsnag = bugsnag;
        }

        [HttpGet]
        [Route("DownloadFile")]
        [Authorize]
        public ActionResult DownloadFile(string Path, string Name)
        {
            string strFileUrl = "";
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                string PathDecrypt = decrypt.DecryptData(Path.Replace(" ", "+"));
                //string PathDecrypt = Path.Replace(" ", "+");
                strFileUrl = _hostingEnvironment.ContentRootPath + "\\" + PathDecrypt; 
                strFileUrl = HttpUtility.UrlDecode(strFileUrl);
                byte[] bytes = System.IO.File.ReadAllBytes(strFileUrl);
                return File(bytes, FileContentType.GetContentType(strFileUrl), Name);
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                return NotFound();
            }
        }
        [HttpGet]
        [Route("GetFile")]
        [Authorize]
        public ActionResult GetFile(string Path, string Name)
        {
            try
            {
                Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
                string PathDecrypt = decrypt.DecryptData(Path.Replace(" ", "+"));
                string strFileUrl = _hostingEnvironment.ContentRootPath + "\\" + PathDecrypt; 
                strFileUrl = HttpUtility.UrlDecode(strFileUrl);
                byte[] bytes = System.IO.File.ReadAllBytes(strFileUrl);
                return File(bytes, FileContentType.GetContentType(strFileUrl), Name);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
