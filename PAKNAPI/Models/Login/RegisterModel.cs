using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Login
{
    public class RegisterModel
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    public class IndivialRegisterModel : BIIndividualInsertIN
    {
        //public string Phone { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    public class BusinessRegisterModel : BIBusinessInsertIN
    {
        public string Password { get; set; }
        public string RePassword { get; set; }


        public class ChangePwdModel
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
            public string RePassword { get; set; }
        }
        public class ChangePwdInManage
        {
            public int UserId { get; set; }
            public string NewPassword { get; set; }
            public string RePassword { get; set; }
        }
    }
}
