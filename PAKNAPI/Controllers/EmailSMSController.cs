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

namespace PAKNAPI.Controllers
{
    [Route("api/EmailSMS")]
    [ApiController]
   
    public class EmailSMSController : BaseApiController
    {
        private readonly IAppSetting _appSetting;
        private readonly IClient _bugsnag;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmailSMSController(IAppSetting appSetting, IClient bugsnag, IWebHostEnvironment hostEnvironment)
        {
            _appSetting = appSetting;
            _bugsnag = bugsnag;
            _hostingEnvironment = hostEnvironment;
        }

		[HttpPost]
		[Authorize]
		[Route("SMSDelete")]
		public async Task<ActionResult<object>> SMSQuanLyTinNhanDeleteBase(SMSQuanLyTinNhanDeleteIN _sMSQuanLyTinNhanDeleteIN)
		{
			try
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				SMSDoanhNghiepDeleteBySMSIdIN dn = new SMSDoanhNghiepDeleteBySMSIdIN();
				dn.SMSId = _sMSQuanLyTinNhanDeleteIN.Id;
				await new SMSDoanhNghiepDeleteBySMSId(_appSetting).SMSDoanhNghiepDeleteBySMSIdDAO(dn);

				SMSNguoiNhanDeleteBySMSIdIN nn = new SMSNguoiNhanDeleteBySMSIdIN();
				nn.SMSId = _sMSQuanLyTinNhanDeleteIN.Id;
				await new SMSNguoiNhanDeleteBySMSId(_appSetting).SMSNguoiNhanDeleteBySMSIdDAO(nn);

				SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN map = new SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN();
				map.SMSId = _sMSQuanLyTinNhanDeleteIN.Id;
				await new SMSTinNhanAdministrativeUnitMapDeleteBySMSId(_appSetting).SMSTinNhanAdministrativeUnitMapDeleteBySMSIdDAO(map);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK, Result = await new SMSQuanLyTinNhanDelete(_appSetting).SMSQuanLyTinNhanDeleteDAO(_sMSQuanLyTinNhanDeleteIN) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpPost]
		[Authorize]
		[Route("SMSInsert")]
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
						}
						else if(item.Category == 1){
							SMSNguoiNhanInsertIN cn = new SMSNguoiNhanInsertIN();
							cn.SMSId = id;
							cn.Individual = item.Id;
							await new SMSNguoiNhanInsert(_appSetting).SMSNguoiNhanInsertDAO(cn);
						}
					}
					// insert map
					


					foreach(var item in AdmintrativeUnitIds) {
						SMSTinNhanAdministrativeUnitMapInsertIN map = new SMSTinNhanAdministrativeUnitMapInsertIN();
						map.SMSId = id;
						map.AdministrativeUnitId = item;
						await new SMSTinNhanAdministrativeUnitMapInsert(_appSetting).SMSTinNhanAdministrativeUnitMapInsertDAO(map);
					}
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK , Result = id};

				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, new Exception());
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "title already exists" };
				}
			}
			catch (Exception ex) {
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize]
		[Route("SMSUpdate")]
		public async Task<object> SMSUpdate(int id)
		{
			try
			{
				SMSUpdateModel sms = new SMSUpdateModel();
				sms.model = (await new SMSQuanLyTinNhanGetById(_appSetting).SMSQuanLyTinNhanGetByIdDAO(id)).FirstOrDefault();
				sms.IndividualBusinessInfo = await new SMSGetListIndividualBusinessBySMSId(_appSetting).SMSGetListIndividualBusinessBySMSIdDAO(id);

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
				return new ResultApi { Success = ResultCode.OK , Result = sms };
			}
			catch (Exception ex)
			{
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("SMSUpdate")]
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
						}
						else if (item.Category == 1)
						{
							SMSNguoiNhanInsertIN cn = new SMSNguoiNhanInsertIN();
							cn.SMSId = id;
							cn.Individual = item.Id;
							await new SMSNguoiNhanInsert(_appSetting).SMSNguoiNhanInsertDAO(cn);
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
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK, Result = id };

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Result = id, Message = "title already exists" };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}


		[HttpGet]
		[Authorize]
		[Route("SMSUpdateStatusTypeSend")]
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
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);
					return new ResultApi { Success = ResultCode.OK };

				}
				else
				{
					return new ResultApi { Success = ResultCode.ORROR, Message = "Error" };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("HISSMSInsert")]
		public async Task<ActionResult<object>> HISNewsInsert(HISInsertIN _hISSMS)
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

				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new HISSMSInsert(_appSetting).HISSMSInsertDAO(_hISSMS) };
			}
			catch (Exception ex)
			{
				_bugsnag.Notify(ex);
				//new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

	}
}
