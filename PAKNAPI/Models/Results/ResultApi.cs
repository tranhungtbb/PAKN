using System;

namespace PAKNAPI.Models.Results
{
	public class ResultApi
	{
		#region Properties
		public string Message { get; set; }
		public string Success { get; set; }
		public object Result { get; set; }
		#endregion Properties    
	}
}