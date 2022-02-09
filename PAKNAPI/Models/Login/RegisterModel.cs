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

    /// <example>
    /// {
    ///     "phone": "0988234234",
    ///     "password": "123456a",
    ///     "rePassword": "123456a",
    ///     "gender": true,
    ///     "email": "tran@gmail.com",
    ///     "fullName": "HungTĐ",
    ///     "iDCard": "123123123",
    ///     "nation": "Việt Nam",
    ///     "isActived": true,
    ///     "address": "Hoàng Quốc Việt",
    ///}
    /// </example>
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

    /// <example>
    ///{
    ///     "phone": "0983242342",
    ///     "password": "123456a",
    ///     "rePassword": "123456a",
    ///     "email": "",
    ///     "representativeGender": true,
    ///     "representativeName": "Hùng TĐ",
    ///     "status": 1,
    ///     "isActived": true,
    ///     "address": "",
    ///     "business": "Công ty b",
    ///     "businessRegistration": "1230128123",
    ///     "orgEmail": "trand@gmail.com",
    ///     "orgAddress": "",
    ///     "orgPhone": ""
    ///}
    /// </example>
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

    /// <example>
    /// {
    ///     "UserId": 140969,
    ///     "NewPassword": "123456a",
    ///     "RePassword": "123456a"
    ///}
    /// </example>

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
