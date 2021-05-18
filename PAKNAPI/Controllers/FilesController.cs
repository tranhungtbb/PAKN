﻿using PAKNAPI.Common;
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
        private readonly IAppSetting _appSetting;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IClient _bugsnag;

        public FilesController(IWebHostEnvironment hostingEnvironment, IClient bugsnag, IAppSetting appSetting)
        {
            _hostingEnvironment = hostingEnvironment;
            _bugsnag = bugsnag;
            _appSetting = appSetting;
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

        [HttpGet]
        [Route("GetFileSupport")]
        [Authorize]
        public List<string> GetFileSupport(int UserId)
        {
            //string strFileUrl = _hostingEnvironment.WebRootPath + "\\";
            var _urlFileSupport = _appSetting.GetUrlFileSupports();
            List<string> list = new List<string>();
            var fileHdsdCv = _urlFileSupport.UrlHdsdCV;
            var fileHdsdLdcb = _urlFileSupport.UrlHdsdLdcb;
            var fileHdsdQtht = _urlFileSupport.UrlHdsdQtht;
            var fileUrlHdsdDb = _urlFileSupport.UrlHdsdDb;
            list.Add(fileHdsdCv);
            list.Add(fileHdsdLdcb);
            list.Add(fileHdsdQtht);
            list.Add(fileUrlHdsdDb);
            return list;
        }

        [HttpGet]
        [Route("DownloadFileSupport")]
        public ActionResult LoadImage(string filePath, string Name)
        {
            try
            {
                string strFileUrl = System.IO.Path.Combine(_hostingEnvironment.WebRootPath, filePath);
                byte[] bytes = System.IO.File.ReadAllBytes(strFileUrl);
                return File(bytes, FileContentType.GetContentType(strFileUrl), Name);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
