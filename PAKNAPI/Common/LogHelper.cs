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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LogHelper(IAppSetting appSetting)
        {
            _appSetting = appSetting;
        }
        //public LogHelper( IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}
        //Custome code here
        public async void InsertSystemLogging(Logs logHelper)
        {
            //Get User from Claims
            //string userId = logHelper.claim.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.GivenName).Value;

            string userId = logHelper.claim.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

            long _userId;
            long.TryParse(userId, out _userId);
            SYUser sYUser = await new SYUser(_appSetting).SYUserGetByID(_userId);

            SYLOGInsertIN sYSystemLogInsertIN = new SYLOGInsertIN
            {
                UserId = sYUser?.Id??0,
                FullName = sYUser?.FullName??" ",
                Action = logHelper.logAction,
                IPAddress = logHelper.ipAddress,
                MACAddress = logHelper.macAddress,
                Status = logHelper.status,
                Description = logHelper.logAction + " " + logHelper.logObject,
                CreatedDate = DateTime.Now,
                MessageError = logHelper.messageError,
                Exception = logHelper.e != null ? logHelper.e.Message + " - " + logHelper.e.InnerException : null
            };
            await new SYLOGInsert(_appSetting).SYLOGInsertDAO(sYSystemLogInsertIN);
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
                logTitle = httpRequest.Headers["logTitle"]
            };
        }

        public void ProcessInsertLogAsync(HttpContext httpContext, string messageError, Exception ex)
        {
            LogHelper logHelper = new LogHelper(_appSetting);

            BaseRequest baseRequest = logHelper.ReadHeaderFromRequest(httpContext.Request);

            if (baseRequest == null) return;
            if (string.IsNullOrEmpty(baseRequest.logAction)) {
                messageError = "LogAction is Empty";
            }
            //if (string.IsNullOrEmpty(baseRequest.logObject)) {
            //    messageError = "LogObject is Empty";
            //}
            //if (string.IsNullOrEmpty(baseRequest.logTitle))
            //{
            //    messageError = "LogTitle is Empty";
            //}

            logHelper.InsertSystemLogging(new Logs
            {
                logAction = 
                    System.Uri.UnescapeDataString(string.IsNullOrEmpty(baseRequest.logAction) == true ? "" : baseRequest.logAction),
                logObject = 
                    System.Uri.UnescapeDataString(baseRequest.logObject == null ? "" : baseRequest.logObject) +
                    System.Uri.UnescapeDataString(string.IsNullOrEmpty(baseRequest.logTitle) == true ? "" : ": " + baseRequest.logTitle),
                ipAddress = baseRequest.ipAddress,
                macAddress = baseRequest.macAddress,
                messageError = messageError,
                status = (byte?)(messageError != null || ex != null ? 0 : 1),
                e = ex,
                location = baseRequest.location,
                claim = httpContext.User
            });;
        }

        public string GetFullNameFromRequest2(HttpContext httpContext)
        {
            var s = httpContext.User.Identities.FirstOrDefault().Claims;
            return httpContext.User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
        }
        public string GetFullNameFromRequest(HttpContext httpContext)
        {
            return httpContext.User.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
        }

        public long GetUserIdFromRequest(HttpContext httpContext)
        {
            return long.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);
        }

        public int GetUnitIdFromRequest(HttpContext httpContext)
        {
            Int32.TryParse(httpContext.User.Claims.FirstOrDefault(c => c.Type == "UnitId")?.Value, out int i);
            return i;
        }

        public bool GetIsMainFromRequest(HttpContext httpContext)
        {
            return bool.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == "IsMain")?.Value);
        }

        public int GetTypeFromRequest(HttpContext httpContext)
        {
            return int.Parse(httpContext.User.Claims.FirstOrDefault(c => c.Type == "Type")?.Value);
        }
    }

    public class Logs
    {
        public string logAction { get; set; }
        public string logObject { get; set; }
        public string ipAddress { get; set; }
        public string macAddress { get; set; }
        public string messageError { get; set; }
        public byte? status { get; set; }
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
