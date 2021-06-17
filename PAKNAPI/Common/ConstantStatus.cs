namespace PAKNAPI.Common
{
	public static class STATUS_RECOMMENDATION
	{
		public const int CREATED = 1;
		public const int RECEIVE_WAIT = 2;//2 Chờ xử lý
		public const int RECEIVE_DENY = 3;//3 Từ chối xử lý
		public const int RECEIVE_APPROVED = 4;//4 Đã tiếp nhận
		public const int PROCESS_WAIT = 5;//5 Chờ giải quyết
		public const int PROCESS_DENY = 6;//6 Từ chối giải quyết
		public const int PROCESSING = 7;//7 Đang giải quyết
		public const int APPROVE_WAIT = 8;//8 Chờ phê duyệt
		public const int APPROVE_DENY = 9;//9 Từ chối phê duyệt
		public const int FINISED = 10;//10 Đã giải quyết


		//Action
		public const int UPDATED = 11;
	}
	public static class STEP_RECOMMENDATION
	{
		public const int RECEIVE = 1;//Tiếp nhận
		public const int PROCESS = 2;//2 Giải quyết
		public const int APPROVE = 3;//3 Phê duyệt
	}
	public static class PROCESS_STATUS_RECOMMENDATION
	{
		public const int WAIT = 1;//Chờ xử lý
		public const int APPROVED = 2;//2 Đồng ý xử lý
		public const int DENY = 3;//3 Từ chối xử lý
		public const int FORWARD = 4;//3 Chuyển xử lý
	}

	public static class TYPENOTIFICATION
	{
		public const int NEWS = 1; // thông báo tin tức
		public const int RECOMMENDATION = 2; // thông báo PAKN
		public const int INVITATION = 3;
	}

	public class STATUS_HISNEWS
	{
		public const int CREATE = 0; //khoi tao
		public const int COMPILE = 1; // soan thao
		public const int UPDATE = 2; //2 cap nhap
		public const int PUBLIC = 3; //3 cong bo
		public const int CANCEL = 4; //3 huy cong bo
	}

	public class STATUS_SMS
	{
		public const int CREATE = 0; //khoi tao
		public const int COMPILE = 1; // soan thao
		public const int SEND = 2; //2 gửi
	}

	public class STATUS_HIS_SMS
	{
		public const int CREATE = 0; //khoi tao
		public const int UPDATE = 1; // soan thao
		public const int SEND = 2; //2 gửi
	}
}