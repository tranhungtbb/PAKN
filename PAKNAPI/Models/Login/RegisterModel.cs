using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
        [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu không đúng định dạng")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu nhập lại không đúng định dạng")]
        public string RePassword { get; set; }
    }

    public class BusinessRegisterModel : BIBusinessInsertIN
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
        [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu không đúng định dạng")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu nhập lại không được để trống")]
        [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu nhập lại không đúng định dạng")]
        public string RePassword { get; set; }


        public class ChangePwdModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu cũ không được để trống")]
            [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu cũ không đúng định dạng")]
            public string OldPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu mới không được để trống")]
            [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu mới không đúng định dạng")]
            public string NewPassword { get; set; }

            [Required(AllowEmptyStrings = false, ErrorMessage = "Xác nhận mật khẩu mới không được để trống")]
            [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Xác nhận mật khẩu mới không đúng định dạng")]
            public string RePassword { get; set; }
        }
        public class ChangePwdInManage
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "Mã người dùng không được để trống")]
            public int UserId { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu mới không được để trống")]
            [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu mới không đúng định dạng")]
            public string NewPassword { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu cũ không được để trống")]
            [RegularExpression(ConstantRegex.PASSWORD, ErrorMessage = "Mật khẩu cũ không đúng định dạng")]
            public string RePassword { get; set; }
        }
    }
}
