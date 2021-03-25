using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PAKNAPI.Common
{
	public class LogHelper
	{
		private SQLCon _sQLCon = new SQLCon();
		public List<string> ActionNames = new List<string>
		{
			"Thêm",//1
            "Cập nhật",//2
            "Xóa",//3
            "Đăng nhập",//4
            "Đăng xuất",//5
            "Phê duyệt",//6
            "Yêu cầu",//7
            "Xuất file",//8
            "Phát hành", //9
            "Thay đổi mật khẩu", //10
            "Khóa tài khoản", //11
            "Mở khóa tài khoản", //12
            "Khôi phục tài khoản", //13
            "Từ chối", //14
            "Thu hồi", //15
            "Gửi", //16
            "Xin cấp số",//17
            "Ủy quyền",//18
            "Cấp số",//19
            "Loại bỏ",//20
            "Đồng ý",//21
            "Chuyển",//22
            "Ký duyệt",//23
            "Vào sổ",//24
            "Kết thúc",//25
            "Phối hợp",//26
            "Giữ",//27
            "Đưa",//28
            "Trả lời",//29
            "Hủy giữ",//30
        };
		private readonly IAppSetting _appSetting;
		public LogHelper(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}
		//Custome code here
		public async void InsertSystemLogging(Logs logHelper)
		{
			//Get User from Claims
			//string userId = logHelper.claim.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName).Value;

			string userId = logHelper.claim.Claims.FirstOrDefault(c => c.Type == "Id").Value;
			SYUser sYUser = await new SYUser(_appSetting).SYUserGetByID((long?)long.Parse(userId));

			SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
			{
				UserId = sYUser.Id,
				FullName = sYUser.FullName,
				Action = logHelper.logAction,
				IPAddress = logHelper.ipAddress,
				MACAddress = logHelper.macAddress,
				Description = logHelper.logAction + " " + logHelper.logObject,
				CreatedDate = DateTime.Now,
				Exception = logHelper.e != null ? logHelper.e.Message + " - " + logHelper.e.InnerException : null
			};
			await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
			//DynamicParameters DP = new DynamicParameters();
			//DP.Add("TenNguoiDung", sYUser.FullName);
			//DP.Add("NgayHanhDong", DateTime.Now);
			//DP.Add("TenHanhDong", logHelper.logAction);//Ten action (add, edit..), truyen tu client
			//DP.Add("DiaChiIp", logHelper.ipAddress);
			//DP.Add("DiaChiMac", logHelper.macAddress);
			//DP.Add("NgoaiLe", logHelper.e != null ? logHelper.e.Message + " - " + logHelper.e.InnerException : null);
			//DP.Add("TacNhan", logHelper.logObject);//Ten tac nhan, truyen tu client
			//DP.Add("NoiXayRaLoi", logHelper.location);//Ten vi tri gay loi
			//DP.Add("MaDonVi", sYUser.OrganizationName);
			//DP.Add("NoiDung", logHelper.logAction + " " + logHelper.logObject);

			//await _sQLCon.ExecuteNonQueryDapperAsync("TenStoredInsertLog", DP);
		}

		public BaseRequest ReadBodyFromRequest(HttpRequest httpRequest)
		{
			httpRequest.EnableBuffering();
			string str = (string)httpRequest.HttpContext.Items["body"];
			//using (StreamReader reader = new StreamReader(httpRequest.Body))
			//{
			//	str = await reader.ReadToEndAsync();
			//}

			return JsonConvert.DeserializeObject<BaseRequest>(str);
		}

		public BaseRequest ReadHeaderFromRequest(HttpRequest httpRequest)
		{
			return new BaseRequest
			{
				logAction = httpRequest.Headers["logAction"],
				logObject = httpRequest.Headers["logObject"],
				ipAddress = httpRequest.Headers["ipAddress"],
				macAddress = httpRequest.Headers["macAddress"],
			};
		}

		public void ProcessInsertLogAsync(HttpContext httpContext, Exception ex)
		{
			LogHelper logHelper = new LogHelper(_appSetting);

			BaseRequest baseRequest = logHelper.ReadHeaderFromRequest(httpContext.Request);

			if (baseRequest == null) return;

			logHelper.InsertSystemLogging(new Logs
			{
				logAction = System.Uri.UnescapeDataString(baseRequest.logAction),
				logObject = System.Uri.UnescapeDataString(baseRequest.logObject),
				ipAddress = baseRequest.ipAddress,
				macAddress = baseRequest.macAddress,
				e = ex,
				location = baseRequest.location,
				claim = httpContext.User
			});
		}
	}

	public class Logs
	{
		public string logAction { get; set; }
		public string logObject { get; set; }
		public string ipAddress { get; set; }
		public string macAddress { get; set; }
		public Exception e { get; set; }
		public string location { get; set; }
		public ClaimsPrincipal claim;
	}

	public enum LogAction
	{
		Add = 1,
		Edit = 2,
		Delete = 3,
		Login = 4,
		LogOut = 5,
		Approved = 6,
		Request = 7,
		Export = 8,
		Publish = 9,
		ChangePassWord = 10,
		Lock = 11,
		Unlock = 12,
		Restored = 13,
		Deny = 14,
		Recover = 15,
		Send = 16,
		Wait = 17,
		Delegacy = 18,
		ProvidedNumber = 19,
		Remove = 20,
		Accept = 21,
		Forward = 22,
		Signed = 23,
		VaoSo = 24,
		EndProcess = 25,
		Support = 26,
		Keep = 27,
		Move = 28,
		Reply = 29,
		CanceKeep = 30,
		Confirm = 31,
		Finish = 32
	}
}
