using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.Models.Results
{
	public class LoginIN
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]

		public string Password { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]

		public string UserName { get; set; }
	}
	public class EditUserRequest : BaseRequest
	{

	}

	public class Forgetpassword {
		[Required(AllowEmptyStrings = false, ErrorMessage = "E-mail không được để trống")]
		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng định dạng")]
		public string Email { get; set; }
	}

	public class GetTokenByEmail
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "E-mail không được để trống")]
		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng định dạng")]
		public string Email { get; set; }
		public int? Type { get; set; }
	}
}