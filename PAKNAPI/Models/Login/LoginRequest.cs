using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.Models.Results
{
	/// <example>
	/// { "UserName": "tt_chuyenvien@gmail.com", "Password" : "123456a"}
	/// </example>
	public class LoginIN
	{
		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên đăng nhập không được để trống")]
		public string UserName { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Mật khẩu không được để trống")]
		public string Password { get; set; }

	
	}
	public class EditUserRequest : BaseRequest
	{

	}

	
	public class Forgetpassword {
		/// <example>tt_chuyenvien @gmail.com</example>
		[Required(AllowEmptyStrings = false, ErrorMessage = "E-mail không được để trống")]
		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng định dạng")]

		public string Email { get; set; }
	}

	public class GetTokenByEmail
	{
		/// <example>tt_chuyenvien @gmail.com</example>
		[Required(AllowEmptyStrings = false, ErrorMessage = "E-mail không được để trống")]
		[DataType(DataType.EmailAddress, ErrorMessage = "E-mail không đúng định dạng")]
		
		public string Email { get; set; }

		/// <summary>
		/// Type = 1 : Đăng ký tài khoản <br />
		/// Type = 2 : Quên mật khẩu
		/// </summary>
		/// <example>1</example>
		public int? Type { get; set; }
	}
}