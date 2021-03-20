namespace PAKNAPI.Common
{
	public static class ResultSuccess
	{
		public const string OK = "OK";
		public const string ORROR = "ORROR";
		public const string ALREADY_EXISTS = "ALREADY_EXISTS";
	}
	public static class ResultMessage
	{
		public const string OK = "Success";
		public const string ORROR = "Some error occured, please try again";
		public const string ALREADY_EXISTS = "Already exists";
		public const string NOTSUPPORTED_FILETYPE = "Not Supported File Type";
		public const string FILESIZE_EXCEED = "Please Upload a file upto 1 mb.";
	}
	public class RestrictFileTypeMIME
	{
		public static readonly string[] acceptedFile = { ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".jpeg", ".png" };
		public static readonly string[] acceptedImage = { ".jpeg", ".png", ".jpg", ".gif", ".bmp" };
	}
}