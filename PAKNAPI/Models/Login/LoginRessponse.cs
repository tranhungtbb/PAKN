﻿using PAKNAPI.ModelBase;
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
        public string AccessToken { get; set; }
        public int? UnitId { get; set; }

        public bool? IsActive { get; set; }
        public int? TypeObject { get; set; }
        public string UnitName { get; set; }
        public bool? IsMain { get; set; }
        public bool? IsHaveToken { get; set; }
        public int Role { get; set; }
        public List<SYUSRGetPermissionByUserId> Permissions { get; set; }
        public List<string> Functions { get; set; }
        public List<string> PermissionCategories { get; set; }
    }
}