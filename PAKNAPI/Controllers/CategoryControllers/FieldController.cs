using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.Models;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
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
using Microsoft.AspNetCore.Http;
using PAKNAPI.Models.ModelBase;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using NSwag.Annotations;

namespace PAKNAPI.Controllers.ControllerBase
{
    [Route("api/field")]
    [ApiController]
	[ValidateModel]
	[OpenApiTag("Lĩnh vực", Description = "Danh mục lĩnh vực")]
	public class FieldController : BaseApiController
	{
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
		private readonly IWebHostEnvironment _hostEnvironment;

		public FieldController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
			_hostEnvironment = hostEnvironment;

		}
		/// <summary>
		/// xóa lĩnh vực
		/// </summary>
		/// <param name="_cAFieldDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> CAFieldDeleteBase(CAFieldDeleteIN _cAFieldDeleteIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldDelete(_appSetting).CAFieldDeleteDAO(_cAFieldDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// dánh sách lĩnh vực
		/// </summary>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-field-on-page")]
		public async Task<ActionResult<object>> CAFieldGetAllOnPageBase()
		{
			try
			{
				List<CAFieldGetAllOnPage> rsCAFieldGetAllOnPage = await new CAFieldGetAllOnPage(_appSetting).CAFieldGetAllOnPageDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetAllOnPage", rsCAFieldGetAllOnPage},
						{"TotalCount", rsCAFieldGetAllOnPage != null && rsCAFieldGetAllOnPage.Count > 0 ? rsCAFieldGetAllOnPage[0].RowNumber : 0}
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Route("get-all-list-field-icon-on-page")]
		public async Task<ActionResult<object>> CAFieldGetAllIconOnPage()
		{
			try
			{
				List<CAFieldGetAllIconPath> rsCAFieldGetAllOnPage = await new CAFieldGetAllIconPath(_appSetting).CAFieldGetAllIconPathDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetAllIconPath", rsCAFieldGetAllOnPage}
						
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// chi tiết lĩnh vực
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> CAFieldGetByIDBase(int? Id)
		{
			try
			{
				List<CAFieldGetByID> rsCAFieldGetByID = await new CAFieldGetByID(_appSetting).CAFieldGetByIDDAO(Id);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetByID", rsCAFieldGetByID},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// :D
		/// </summary>
		/// <returns></returns>

		[HttpGet]
		[Route("get-drop-down")]
		public async Task<ActionResult<object>> CAFieldGetDropdownBase()
		{
			try
			{
				List<CAFieldGetDropdown> rsCAFieldGetDropdown = await new CAFieldGetDropdown(_appSetting).CAFieldGetDropdownDAO();
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CAFieldGetDropdown", rsCAFieldGetDropdown},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới lĩnh vực
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<ActionResult<object>> CAFieldInsertBase()
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				CAFieldInsertIN _cAFieldInsertIN = JsonConvert.DeserializeObject<CAFieldInsertIN>(Request.Form["data"].ToString(), jss);

				var fileAvatar = Request.Form.Files.Where(x => x.Name == "image").ToList();
				string folder = "Upload\\Category\\Feild";
				var folderPath = Path.Combine(_hostEnvironment.ContentRootPath, folder);
				string filePath = string.Empty;

				// avatar
				if (fileAvatar.Count() > 0)
				{

					deletefile(_cAFieldInsertIN.FilePath);
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
					filePath = Path.Combine(folder, Path.GetFileName(fileAvatar[0].FileName.Replace("+", "")));
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						fileAvatar[0].CopyTo(stream);
					}
					_cAFieldInsertIN.FilePath = filePath;
				}


				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldInsert(_appSetting).CAFieldInsertDAO(_cAFieldInsertIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cap nhap linh vuc
		/// </summary>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<ActionResult<object>> CAFieldUpdateBase()
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				var jss = new JsonSerializerSettings
				{
					DateFormatHandling = DateFormatHandling.IsoDateFormat,
					DateTimeZoneHandling = DateTimeZoneHandling.Local,
					DateParseHandling = DateParseHandling.DateTimeOffset,
				};
				CAFieldUpdateIN _cAFieldUpdateIN = JsonConvert.DeserializeObject<CAFieldUpdateIN>(Request.Form["data"].ToString(), jss);

				var fileAvatar = Request.Form.Files.Where(x => x.Name == "image").ToList();
				string folder = "Upload\\Category\\Feild";
				var folderPath = Path.Combine(_hostEnvironment.ContentRootPath, folder);
				string filePath = string.Empty;

				// avatar
				if (fileAvatar.Count() > 0)
				{

					deletefile(_cAFieldUpdateIN.FilePath);
					if (!Directory.Exists(folderPath))
					{
						Directory.CreateDirectory(folderPath);
					}
					filePath = Path.Combine(folder, Path.GetFileName(fileAvatar[0].FileName.Replace("+", "")));
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						fileAvatar[0].CopyTo(stream);
					}
					_cAFieldUpdateIN.FilePath = filePath;
				}
				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldUpdate(_appSetting).CAFieldUpdateDAO(_cAFieldUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update-status")]
		public async Task<ActionResult<object>> CAFieldUpdateStatusBase(CAFieldUpdateIN cAFieldUpdateIN)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new CAFieldUpdate(_appSetting).CAFieldUpdateStatusDAO(cAFieldUpdateIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// dropdown lĩnh vực knct
		/// </summary>
		/// <returns></returns>

		[HttpGet]
        [Authorize("ThePolicy")]
        [Route("field-knct-get-dropdown")]
        public async Task<ActionResult<object>> CAFieldKNCTGetDropdownBase()
        {
            try
            {
                List<CAFieldKNCTGetDropdown> rsCAFieldKNCTGetDropdown = await new CAFieldKNCTGetDropdown(_appSetting).CAFieldKNCTGetDropdownDAO();
                IDictionary<string, object> json = new Dictionary<string, object>
                    {
                        {"CAFieldKNCTGetDropdown", rsCAFieldKNCTGetDropdown},
                    };
                return new ResultApi { Success = ResultCode.OK, Result = json };
            }
            catch (Exception ex)
            {
                _bugsnag.Notify(ex);
                new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

                return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
            }
        }

		private bool deletefile(string fname)
		{
			try
			{
				string _imageToBeDeleted = Path.Combine(_hostEnvironment.WebRootPath, fname);
				if ((System.IO.File.Exists(_imageToBeDeleted)))
				{
					System.IO.File.Delete(_imageToBeDeleted);
				}
				return true;
			}
			catch (Exception ex) { return false; }
		}
	}
}
