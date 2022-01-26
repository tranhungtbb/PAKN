using PAKNAPI.ModelBase;
using System.Collections.Generic;

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
		public const int UPDATED = 11; // chuyeenr tiep
		public const int PROCESS_DENY_MAIN = 12; // tu choi giai quyet trung tam
		public const int COMBINE = 13; // tu ueu cau phoi hop

		public const int RECEIVE_FINISED = 14;

	}
	public static class STEP_RECOMMENDATION
	{
		public const int RECEIVE = 1;//Tiếp nhận
		public const int PROCESS = 2;//2 Giải quyết
		public const int APPROVE = 3;//3 Phê duyệt

		public const int FORWARD = 4;//4 chuyển tiếp cho đơn vị cấp dưới hoặc trung tâm
	}
	public static class PROCESS_STATUS_RECOMMENDATION
	{
		public const int WAIT = 1;//Chờ xử lý
		public const int APPROVED = 2;//2 Đồng ý xử lý
		public const int DENY = 3;//3 Từ chối xử lý
		public const int FORWARD = 4;//3 Chuyển xử lý
	}

	// Phuong thuc tiep nhan
	public static class ReceptionType
	{
		public const int Web = 1; // web
		public const int App = 2;// app
		public const int Phone = 3;// dien thoai
		public const int Email = 4;// email
	}

	public static class StatisticType
	{
		public const int Field = 1; // lv
		public const int Unit = 2;// don vi
	}

	public static class TYPENOTIFICATION
	{
		public const int NEWS = 1; // thông báo tin tức
		public const int RECOMMENDATION = 2; // thông báo PAKN
		public const int INVITATION = 3;
		public const int ADMINISTRATIVE = 4;
	}

	public static class TYPECONFIG
	{
		public const int CONFIG_EMAIL = 1;
		public const int CONFIG_SMS = 2;
		public const int CONFIG_SWITCHBOARD = 3;
		public const int GENERAL = 4;
		public const int VIEWHOME = 5;
		public const int SAMELOCATION = 8;
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
		
	public class STATUS_HIS_INVITATION
	{
		public const int CREATE = 0; //khoi tao
		public const int UPDATE = 1; // soan thao
		public const int SEND = 2; //2 gửi
	}

	public class GroupUnit
	{
		public static List<SYUnitDropdown> ListGroupUnit()
		{
			var list = new List<SYUnitDropdown>
									   {
										   new SYUnitDropdown
											   {
												   Id = 1,
												   Name = "Cơ quan chuyên môn thuộc tỉnh"
											   },
										   new SYUnitDropdown
											   {
												   Id = 2,
												   Name = "Ban ngành thuộc tỉnh Khánh Hòa"
											   },
										   new SYUnitDropdown
											  {
												   Id = 3,
												   Name = "Hạ tầng kỹ thuật"
											   },
										   new SYUnitDropdown
											  {
												   Id = 4,
												   Name = "Ủy ban nhân dân các huyện, thành phố",
												   IsAdministrative = true
											   },
									   };

			return list;
		}
	}
}