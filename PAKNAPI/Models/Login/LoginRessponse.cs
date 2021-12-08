using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.Results
{
    public class LoginResponse : ResultApi
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? UnitId { get; set; }
        public bool? IsActive { get; set; }
        public int? TypeObject { get; set; }
        public string UnitName { get; set; }
        public bool? IsMain { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? IsUnitMain { get; set; }
        public bool? IsApprove { get; set; }

        public bool? IsHaveToken { get; set; }
        public int Role { get; set; }
        public string Permissions { get; set; }
        public string PermissionFunctions { get; set; }
        public string PermissionCategories { get; set; }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public LoginResponse(SYUSRLogin user, string accessToken, string refreshToken) {
            Success = ResultCode.OK;
            UserId = user.Id;
            UserName = user.UserName;
            FullName = user.FullName;
            Email = user.Email;
            IsActive = user.IsActived;
            Phone = user.UserName;
            UnitId = user.UnitId;
            UnitName = user.UnitName;
            IsMain = user.IsMain;
            IsUnitMain = user.IsUnitMain;
            IsAdmin = user.IsAdmin;
            TypeObject = user.TypeObject;
            AccessToken = accessToken;
            IsHaveToken = true;
            RefreshToken = refreshToken;
            IsApprove = user.IsApprove;
        }
    }
}