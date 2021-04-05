using PAKNAPI.Common;
using System;
using System.Collections.Generic;

namespace PAKNAPI.Models.Results
{
	public class LoginIN
	{
		public string Password { get; set; }
		public string UserName { get; set; }
	}
	public class EditUserRequest : BaseRequest
    {

    }
}