namespace PAKNAPI.Common
{
	public static class STATUS_RECOMMENDATION
	{
		public const int CREATED = 1;
		public const int RECEIVE_WAIT = 2;//2 Chờ xử lý
		public const int RECEIVE_DENY = 3;//3 Từ chối xử lý
		public const int RECEIVE_APPROVED = 4;//4 Tiếp nhận xử lý
		public const int PROCESS_WAIT = 5;//5 Chờ giải quyết
		public const int PROCESS_DENY = 6;//6 Từ chối giải quyết
		public const int PROCESSING = 7;//7 Đang giải quyết
		public const int APPROVE_WAIT = 8;//8 Chờ phê duyệt
		public const int APPROVE_DENY = 9;//9 Từ chối phê duyệt
		public const int FINISED = 10;//10 Đã giải quyết
	}
}