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
using PAKNAPI.Models.ModelBase;
using PAKNAPI.Models.Remind;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PAKNAPI.Models.Recommendation;
using PAKNAPI.Models.Invitation;
using PAKNAPI.Models.EmailSMSModel;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using static PAKNAPI.Controllers.SendSmsController;

namespace PAKNAPI.Controllers
{
    [Route("api/sms")]
    [ApiController]
	[ValidateModel]
	public class SMSController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

		public SMSController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment, IHttpContextAccessor httpContextAccessor, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
		}
		/// <summary>
		/// xóa SMS
		/// </summary>
		/// <param name="_sMSQuanLyTinNhanDeleteIN"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("delete")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanDeleteBase(SMSQuanLyTinNhanDeleteIN _sMSQuanLyTinNhanDeleteIN)
		{
			try
			{
				var s = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims;
				var identity = (ClaimsIdentity)User.Identity;
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				SMSDoanhNghiepDeleteBySMSIdIN dn = new SMSDoanhNghiepDeleteBySMSIdIN();
				dn.SMSId = _sMSQuanLyTinNhanDeleteIN.Id;
				await new SMSDoanhNghiepDeleteBySMSId(_appSetting).SMSDoanhNghiepDeleteBySMSIdDAO(dn);

				SMSNguoiNhanDeleteBySMSIdIN nn = new SMSNguoiNhanDeleteBySMSIdIN();
				nn.SMSId = _sMSQuanLyTinNhanDeleteIN.Id;
				await new SMSNguoiNhanDeleteBySMSId(_appSetting).SMSNguoiNhanDeleteBySMSIdDAO(nn);

				SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN map = new SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN();
				map.SMSId = _sMSQuanLyTinNhanDeleteIN.Id;
				await new SMSTinNhanAdministrativeUnitMapDeleteBySMSId(_appSetting).SMSTinNhanAdministrativeUnitMapDeleteBySMSIdDAO(map);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = await new SMSQuanLyTinNhanDelete(_appSetting).SMSQuanLyTinNhanDeleteDAO(_sMSQuanLyTinNhanDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thêm mới SMS
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>

		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("insert")]
		public async Task<object> SMSInsert(SMSInsertModel response)
		{
			try
			{
				response.model.CreateDate = DateTime.Now;
				response.model.UserCreateId = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				if (response.model.Status == STATUS_SMS.SEND)
				{
					response.model.SendDate = DateTime.Now;
					response.model.UserSend = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				}
				int id = Int32.Parse((await new SMSQuanLyTinNhanInsert(_appSetting).SMSQuanLyTinNhanInsertDAO(response.model)).ToString());

				if (id > 0)
				{
					List<int> AdmintrativeUnitIds = new List<int>();
					SendSmsController sendSmsController = new SendSmsController(_appSetting, _bugsnag, _configuration);
					SendMessageRequest sendMessageRequest = new SendMessageRequest();
					sendMessageRequest.message = $"{response.model.Title} {Environment.NewLine}{response.model.Content}{Environment.NewLine}{response.model.Signature}";
					foreach (var item in response.IndividualBusinessInfo) {
						if (AdmintrativeUnitIds.Count() == 0)
						{
							AdmintrativeUnitIds.Add(item.AdmintrativeUnitId);
						}
						else {
							int check = AdmintrativeUnitIds.Where(x => x == item.AdmintrativeUnitId).FirstOrDefault();
							if (check == 0) { AdmintrativeUnitIds.Add(item.AdmintrativeUnitId); }
						}
						

						if (item.Category == 2)
						{
							SMSDoanhNghiepInsertIN dn = new SMSDoanhNghiepInsertIN();
							dn.SMSId = id;
							dn.BusinessId = item.Id;
							await new SMSDoanhNghiepInsert(_appSetting).SMSDoanhNghiepInsertDAO(dn);
							if (response.model.Status == 2)
							{
								var userSend = await new BIBusiness(_appSetting).BIBusinessGetByID(item.Id);
								sendMessageRequest.phoneTo = userSend.Phone;
								if (userSend.Phone.StartsWith('0'))
								{
									sendMessageRequest.phoneTo = userSend.Phone.Remove(0, 1);
									sendMessageRequest.phoneTo = "+84" + sendMessageRequest.phoneTo;
								}
								await sendSmsController.OnSendSMS(sendMessageRequest);
							}
						}
						else if(item.Category == 1){
							SMSNguoiNhanInsertIN cn = new SMSNguoiNhanInsertIN();
							cn.SMSId = id;
							cn.Individual = item.Id;
							await new SMSNguoiNhanInsert(_appSetting).SMSNguoiNhanInsertDAO(cn);
							if (response.model.Status == 2)
							{
								var userSend = await new BIIndividual(_appSetting).BIIndividualGetByID(item.Id);
								sendMessageRequest.phoneTo = userSend.Phone;
								if (userSend.Phone.StartsWith('0'))
								{
									sendMessageRequest.phoneTo = userSend.Phone.Remove(0, 1);
									sendMessageRequest.phoneTo = "+84" + sendMessageRequest.phoneTo;
								}
								await sendSmsController.OnSendSMS(sendMessageRequest);
							}
						}
					}
					// insert map
					


					foreach(var item in AdmintrativeUnitIds) {
						SMSTinNhanAdministrativeUnitMapInsertIN map = new SMSTinNhanAdministrativeUnitMapInsertIN();
						map.SMSId = id;
						map.AdministrativeUnitId = item;
						await new SMSTinNhanAdministrativeUnitMapInsert(_appSetting).SMSTinNhanAdministrativeUnitMapInsertDAO(map);
					}

					var his = new HISInsertIN();
					his.ObjectId = id;
					his.Status = STATUS_HIS_SMS.CREATE;
					await HISNewsInsert(his);

					if (response.model.Status == 2) { // đã gửi
						his.Status = STATUS_HIS_SMS.SEND;
						await HISNewsInsert(his);
					}

					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK , Result = id};

				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,"Tiêu đề đã tồn tại", new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "Tiêu đề đã tồn tại" };
				}
			}
			catch (Exception ex) {
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// thông tin SMS - để cập nhập
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<object> SMSUpdate(int id)
		{
			try
			{
				SMSUpdateModel sms = new SMSUpdateModel();
				sms.model = (await new SMSQuanLyTinNhanGetById(_appSetting).SMSQuanLyTinNhanGetByIdDAO(id)).FirstOrDefault();
				sms.IndividualBusinessInfo = await new SMSGetListIndividualBusinessBySMSId(_appSetting).SMSGetListIndividualBusinessBySMSIdDAO(id);

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK , Result = sms };
			}
			catch (Exception ex)
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// cập nhập SMS
		/// </summary>
		/// <param name="response"></param>
		/// <returns></returns>
		[HttpPost]
		[Authorize("ThePolicy")]
		[Route("update")]
		public async Task<object> SMSUpdate(SMSUpdateRequestModel response)
		{
			try
			{
				SMSQuanLyTinNhanUpdateIN update = new SMSQuanLyTinNhanUpdateIN();
				update.Id = response.model.Id;
				update.Title = response.model.Title;
				update.Content = response.model.Content;
				update.Signature = response.model.Signature;
				update.Status = response.model.Status;
				update.UpdateDate = DateTime.Now;
				update.Type = response.model.Type;
				update.UserUpdateId = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				if (response.model.Status == STATUS_SMS.SEND)
				{
					update.SendDate = DateTime.Now;
					update.UserSend = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				}
				int id = Int32.Parse((await new SMSQuanLyTinNhanUpdate(_appSetting).SMSQuanLyTinNhanUpdateDAO(update)).ToString());

				if (id > 0)
				{
					//delete SMS
					SMSDoanhNghiepDeleteBySMSIdIN dn = new SMSDoanhNghiepDeleteBySMSIdIN();
					dn.SMSId = id;
					await new SMSDoanhNghiepDeleteBySMSId(_appSetting).SMSDoanhNghiepDeleteBySMSIdDAO(dn);

					SMSNguoiNhanDeleteBySMSIdIN nn = new SMSNguoiNhanDeleteBySMSIdIN();
					nn.SMSId = id;
					await new SMSNguoiNhanDeleteBySMSId(_appSetting).SMSNguoiNhanDeleteBySMSIdDAO(nn);

					List<int> AdmintrativeUnitIds = new List<int>();
					SendSmsController sendSmsController = new SendSmsController(_appSetting, _bugsnag, _configuration);
					SendMessageRequest sendMessageRequest = new SendMessageRequest();
					sendMessageRequest.message = $"{response.model.Title} {Environment.NewLine}{response.model.Content}{Environment.NewLine}{response.model.Signature}";
					foreach (var item in response.IndividualBusinessInfo)
					{
						if (AdmintrativeUnitIds.Count() == 0)
						{
							AdmintrativeUnitIds.Add(item.AdmintrativeUnitId);
						}
						else
						{
							int check = AdmintrativeUnitIds.Where(x => x == item.AdmintrativeUnitId).FirstOrDefault();
							if (check == 0) { AdmintrativeUnitIds.Add(item.AdmintrativeUnitId); }
						}


						if (item.Category == 2)
						{
							SMSDoanhNghiepInsertIN dn2 = new SMSDoanhNghiepInsertIN();
							dn2.SMSId = id;
							dn2.BusinessId = item.Id;
							await new SMSDoanhNghiepInsert(_appSetting).SMSDoanhNghiepInsertDAO(dn2);
							if (response.model.Status == 2)
							{
								var userSend = await new BIBusiness(_appSetting).BIBusinessGetByID(item.Id);
								sendMessageRequest.phoneTo = userSend.Phone;
								if (userSend.Phone.StartsWith('0'))
								{
									sendMessageRequest.phoneTo = userSend.Phone.Remove(0, 1);
									sendMessageRequest.phoneTo = "+84" + sendMessageRequest.phoneTo;
								}
								await sendSmsController.OnSendSMS(sendMessageRequest);
							}
						}
						else if (item.Category == 1)
						{
							SMSNguoiNhanInsertIN cn = new SMSNguoiNhanInsertIN();
							cn.SMSId = id;
							cn.Individual = item.Id;
							await new SMSNguoiNhanInsert(_appSetting).SMSNguoiNhanInsertDAO(cn);
							if (response.model.Status == 2)
							{
								var userSend = await new BIIndividual(_appSetting).BIIndividualGetByID(item.Id);
								sendMessageRequest.phoneTo = userSend.Phone;
								if (userSend.Phone.StartsWith('0'))
								{
									sendMessageRequest.phoneTo = userSend.Phone.Remove(0, 1);
									sendMessageRequest.phoneTo = "+84" + sendMessageRequest.phoneTo;
								}
								await sendSmsController.OnSendSMS(sendMessageRequest);
							}
						}
					}

					
					// insert map

					SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN mapDelete = new SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN();
					mapDelete.SMSId = id;
					await new SMSTinNhanAdministrativeUnitMapDeleteBySMSId(_appSetting).SMSTinNhanAdministrativeUnitMapDeleteBySMSIdDAO(mapDelete);

					foreach (var item in AdmintrativeUnitIds)
					{
						SMSTinNhanAdministrativeUnitMapInsertIN map = new SMSTinNhanAdministrativeUnitMapInsertIN();
						map.SMSId = id;
						map.AdministrativeUnitId = item;
						await new SMSTinNhanAdministrativeUnitMapInsert(_appSetting).SMSTinNhanAdministrativeUnitMapInsertDAO(map);
					}
					// history
					var his = new HISInsertIN();
					his.ObjectId = id;
					his.Status = STATUS_HIS_SMS.UPDATE;
					await HISNewsInsert(his);

					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK, Result = id , Message = ResultMessage.OK};

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "Tiêu đề SMS đã tồn tại" };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		/// <summary>
		/// cập nhập trạng thái
		/// </summary>
		/// <param name="idMSMS"></param>
		/// <returns></returns>
		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("update-status")]
		public async Task<object> SMSUpdateStatusTypeSend(int idMSMS)
		{
			try
			{
				SMSQuanLyTinNhanGetById model = (await new SMSQuanLyTinNhanGetById(_appSetting).SMSQuanLyTinNhanGetByIdDAO(idMSMS)).FirstOrDefault();

				SMSQuanLyTinNhanUpdateIN update = new SMSQuanLyTinNhanUpdateIN();
				update.Id = model.Id;
				update.Title = model.Title;
				update.Content = model.Content;
				update.Signature = model.Signature;
				update.Status = STATUS_SMS.SEND;
				update.UpdateDate = DateTime.Now;
				update.Type = model.Type;
				update.UserUpdateId = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				update.SendDate = DateTime.Now;
				update.UserSend = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				int id = Int32.Parse((await new SMSQuanLyTinNhanUpdate(_appSetting).SMSQuanLyTinNhanUpdateDAO(update)).ToString());

				if (id > 0)
				{
					var his = new HISInsertIN();
					his.ObjectId = id;
					his.Status = STATUS_HIS_SMS.SEND;
					await HISNewsInsert(his);

					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
					return new ResultApi { Success = ResultCode.OK };

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Error" };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		private async Task<bool> HISNewsInsert(HISInsertIN _hISSMS)
		{
			try
			{
				_hISSMS.CreatedBy = (int)new LogHelper(_appSetting).GetUserIdFromRequest(HttpContext);
				_hISSMS.CreatedName = new LogHelper(_appSetting).GetFullNameFromRequest(HttpContext);
				_hISSMS.CreatedDate = DateTime.Now;

				switch (_hISSMS.Status)
				{
					case STATUS_HIS_SMS.CREATE:
						_hISSMS.Content = _hISSMS.CreatedName + " đã khởi tạo SMS";
						break;
					case STATUS_HIS_SMS.UPDATE:
						_hISSMS.Content = _hISSMS.CreatedName + " đã cập nhập SMS";
						break;
					case STATUS_HIS_SMS.SEND:
						_hISSMS.Content = _hISSMS.CreatedName + " đã gửi SMS";
						break;
				}

				await new HISSMSInsert(_appSetting).HISSMSInsertDAO(_hISSMS);

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		/// <summary>
		/// chi tiếu SMS
		/// </summary>
		/// <param name="Id"></param>
		/// <returns></returns>


		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-by-id")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanGetByIdBase(int? Id)
		{
			try
			{
				//List<SMSQuanLyTinNhanGetById> rsSMSQuanLyTinNhanGetById = await new SMSQuanLyTinNhanGetById(_appSetting).SMSQuanLyTinNhanGetByIdDAO(Id);
				//IDictionary<string, object> json = new Dictionary<string, object>
				//	{
				//		{"SMSQuanLyTinNhanGetById", rsSMSQuanLyTinNhanGetById},
				//	};
				//return new ResultApi { Success = ResultCode.OK, Result = json };
				SMSUpdateModel sms = new SMSUpdateModel();
				sms.model = (await new SMSQuanLyTinNhanGetById(_appSetting).SMSQuanLyTinNhanGetByIdDAO(Id)).FirstOrDefault();
				sms.IndividualBusinessInfo = await new SMSGetListIndividualBusinessBySMSId(_appSetting).SMSGetListIndividualBusinessBySMSIdDAO(Id);

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null,null);
				return new ResultApi { Success = ResultCode.OK, Result = sms };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext,null, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}
		/// <summary>
		/// danh sách SMS
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="Title"></param>
		/// <param name="UnitId"></param>
		/// <param name="Type"></param>
		/// <param name="Status"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("get-list-sms-on-page")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanGetAllOnPageBase(int? PageSize, int? PageIndex, string Title, int? UnitId, string Type, byte? Status)
		{
			try
			{
				List<SMSQuanLyTinNhanGetAllOnPage> rsSMSQuanLyTinNhanGetAllOnPage = await new SMSQuanLyTinNhanGetAllOnPage(_appSetting).SMSQuanLyTinNhanGetAllOnPageDAO(PageSize, PageIndex, Title, UnitId, Type, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"SMSQuanLyTinNhanGetAllOnPage", rsSMSQuanLyTinNhanGetAllOnPage},
						{"TotalCount", rsSMSQuanLyTinNhanGetAllOnPage != null && rsSMSQuanLyTinNhanGetAllOnPage.Count > 0 ? rsSMSQuanLyTinNhanGetAllOnPage[0].RowNumber : 0},
						{"PageIndex", rsSMSQuanLyTinNhanGetAllOnPage != null && rsSMSQuanLyTinNhanGetAllOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsSMSQuanLyTinNhanGetAllOnPage != null && rsSMSQuanLyTinNhanGetAllOnPage.Count > 0 ? PageSize : 0},
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
		/// danh sách lịch sử SMS
		/// </summary>
		/// <param name="PageSize"></param>
		/// <param name="PageIndex"></param>
		/// <param name="SMSId"></param>
		/// <param name="Content"></param>
		/// <param name="UserName"></param>
		/// <param name="CreateDate"></param>
		/// <param name="Status"></param>
		/// <returns></returns>

		[HttpGet]
		[Authorize("ThePolicy")]
		[Route("list-his")]
		public async Task<ActionResult<object>> HISSMSGetBySMSIdOnPageBase(int? PageSize, int? PageIndex, int? SMSId, string Content, string UserName, DateTime? CreateDate, int? Status)
		{
			try
			{
				List<HISSMSGetBySMSIdOnPage> rsHISSMSGetBySMSIdOnPage = await new HISSMSGetBySMSIdOnPage(_appSetting).HISSMSGetBySMSIdOnPageDAO(PageSize, PageIndex, SMSId, Content, UserName, CreateDate, Status);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"HISSMSGetBySMSIdOnPage", rsHISSMSGetBySMSIdOnPage},
						{"TotalCount", rsHISSMSGetBySMSIdOnPage != null && rsHISSMSGetBySMSIdOnPage.Count > 0 ? rsHISSMSGetBySMSIdOnPage[0].RowNumber : 0},
						{"PageIndex", rsHISSMSGetBySMSIdOnPage != null && rsHISSMSGetBySMSIdOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsHISSMSGetBySMSIdOnPage != null && rsHISSMSGetBySMSIdOnPage.Count > 0 ? PageSize : 0},
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

	}
}
