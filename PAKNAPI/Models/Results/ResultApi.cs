using System;

namespace PAKNAPI.Models.Results
{
	/// <example>
	/// { "Message": "", "Success" = "OK", "Result"=1 }
	/// </example>
	public class ResultApi
	{
		#region Properties
		public string Message { get; set; }
		public string Success { get; set; }
		public object Result { get; set; }
		#endregion Properties    
	}
}