export class CONSTANTS {
	public static FILEACCEPT = '.pdf, .png, .jpg, .jpeg, .doc, .docx, .xls, .xlsx, .zip, .rar, .mp4, .mov'
	public static FILEACCEPT_FORM_ADMINISTRATION = '.pdf, .doc, .docx, .xls, .xlsx'
	public static FILEACCEPTAVATAR = '.png, .jpg, .jpeg'
	public static PAGE_SIZE = 20
	public static PAGE_INDEX = 1
}

export const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8'
export const EXCEL_EXTENSION = '.xlsx'

export class RESPONSE_STATUS {
	public static success = 'OK'
	public static orror = 'ORROR'
	public static incorrect = 'INCORRECT'
}

export class LOG_ACTION {
	public static login = 'Đăng nhập'
	public static logOut = 'Đăng xuất'
	public static GETLIST = 'Trả về danh sách'
	public static GETDATACREATE = 'Trả về dữ liệu danh mục'
	public static GETINFO = 'Trả về thông tin'
	public static INSERT = 'Thêm mới'
	public static UPDATE = 'Cập nhật'
	public static FORWARD = 'Chuyển tiếp'
	public static PROCESSED = 'Xử lý'
	public static UPDATESTATUS = 'Cập nhật trạng thái'
	public static DELETE = 'Xóa'
	public static EXPORT = 'Xuất danh sách'
	public static CHANGEPASSWORD = 'Đổi mật khẩu'
	public static PUBLIC = 'Công bố'
	public static WITHDRAW = 'Hủy công bố'
	public static SEND = 'Gửi'
}

export class LOG_OBJECT {
	public static NO_CONTENT = ''
	public static login = 'Đăng nhập'

	public static CA_FIELD = 'Danh mục lĩnh vực'
	public static CA_WORD = 'Thư viện từ ngữ'
	public static CA_GROUPWORD = 'Nhóm thư viện từ ngữ'
	public static CA_DEPARTMENT = 'Sở ngành'
	public static CA_DEPARTMENT_GROUP = 'Nhóm sở ngành'
	public static CA_HASHTAG = 'Hashtag'
	public static CA_POSITION = 'Chức vụ'
	public static CA_NEWS_TYPE = 'Loại tin tức'

	public static SMS_EMAIL = 'Tin nhắn sms'
	public static HIS_EMAIL = 'Lịch sử sms'
	public static SY_INVITATION = 'Thư mời'
	public static HIS_INVITATION = 'Lịch sử thư mời'

	public static NE_NEWS = 'tin tức'

	public static SY_ROLE = 'Vai trò hệ thống'

	public static SY_USER = 'Người dùng hệ thống'
	public static SY_USER_GET_BY_ROLE = 'Người dùng theo vai trò'

	public static HIS_USER = 'Lịch sử người dùng hệ thống'
	public static MR_RECOMMENDATION = 'Phản ánh / Kiến nghị'
	public static MR_HISTORIES = 'Lịch sử phản ánh / Kiến nghị'
	public static RM_REMIND = 'Danh sách nhắc nhở'
	public static MR_BY_HASHTAG = 'Danh sách PAKN theo hashtag'
	public static SY_INTRODUCE = 'Cấu hình trang giới thiệu'
	public static SY_INDEXSETTING = 'Cấu hình trang chủ'
	public static MR_COMMENT = 'Bình luận Phản ánh kiến nghị'
	public static BI_INDIVIDUAL = 'Cá nhân'
	public static BI_BUSINESS = 'Doanh nghiệp'
	public static SY_TIME = 'Cấu hình thời gian làm việc'
	public static SY_EMAIL = 'Cấu hình email hệ thống'
	public static SY_SMS = 'Cấu hình SMS thương hiệu'
	public static SY_UNIT = 'Đơn vị'
	public static SY_CHATBOX = 'Chat box'
	public static SY_SUPPORT = 'Tài liệu hướng dẫn'
	public static SY_SYSTEM = 'Lịch sử hệ thống'
}

export class MESSAGE_COMMON {
	public static ADD_SUCCESS = 'Thêm mới thành công'
	public static DELETE_SUCCESS = 'Xóa thành công'
	public static UPDATE_SUCCESS = 'Cập nhật thành công'
	public static UNLOCK_SUCCESS = 'Mở khóa thành công'
	public static LOCK_SUCCESS = 'Khóa thành công'
	public static ADD_FAILED = 'Thêm mới thất bại'
	public static DELETE_FAILED = 'Xóa thất bại'
	public static UPDATE_FAILED = 'Cập nhật thất bại'
	public static PUBLISH_SUCCESS = 'Công bố thành công'
	public static EXISTED_CODE = 'Mã đã tồn tại'
	public static EXISTED_NAME = 'Tên đã tồn tại'
	public static WITHDRAW_SUCCESS = 'Thu hồi thành công'
	public static SEND_SUCCESS = 'Gửi thành công'
}

export class RECOMMENDATION_STATUS {
	public static CREATED = 1 // đang soạn thảo
	public static RECEIVE_WAIT = 2 //2 Chờ xử lý
	public static RECEIVE_DENY = 3 //3 Từ chối xử lý
	public static RECEIVE_APPROVED = 4 //4 Đã tiếp nhận
	public static PROCESS_WAIT = 5 //5 Chờ giải quyết
	public static PROCESS_DENY = 6 //6 Từ chối giải quyết
	public static PROCESSING = 7 //7 Đang giải quyết
	public static APPROVE_WAIT = 8 //8 Chờ phê duyệt
	public static APPROVE_DENY = 9 //9 Từ chối phê duyệt
	public static FINISED = 10 //10 Đã giải quyết

	public static UPDATED = 11 //11 Chỉ để lưu log
}

export class STEP_RECOMMENDATION {
	public static RECEIVE = 1 //Tiếp nhận
	public static PROCESS = 2 //2 Giải quyết
	public static APPROVE = 3 //3 Phê duyệt
}

export class TYPE_NOTIFICATION {
	public static NEWS = 1 // thông báo tin tức
	public static RECOMMENDATION = 2 // thông báo PAKN
}

export class PROCESS_STATUS_RECOMMENDATION {
	public static WAIT = 1 //Chờ xử lý
	public static APPROVED = 2 //2 Đồng ý xử lý
	public static DENY = 3 //3 Từ chối xử lý
	public static FORWARD = 4 //3 Chuyển xử lý
}

export class STATUS_HISNEWS {
	public static CREATE = 0 //khoi tao
	public static COMPILE = 1 // soan thao
	public static UPDATE = 2 //2 cap nhap
	public static PUBLIC = 3 //3 cong bo
	public static CANCEL = 4 //3 huy cong bo
}

export class STATUS_HIS_SMS {
	public static CREATE = 0 //khoi tao
	public static UPDATE = 1 //2 cap nhap
	public static SEND = 2 //3 cong bo
}
export class CATEGORY_SUPPORT {
	public static DOCUMENT = 0 //khoi tao
	public static VIDEO = 1 //2 cap nhap
}

export const FILETYPE = [
	{ text: 'application/msword', value: 1 },
	{ text: 'text/plain', value: 1 },
	{ text: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document', value: 1 },
	{ text: 'application/vnd.openxmlformats-officedocument.wordprocessingml.template', value: 1 },
	{ text: 'application/vnd.ms-word.document.macroEnabled.12', value: 1 },
	{ text: 'application/vnd.ms-word.template.macroEnabled.12', value: 1 },
	{ text: 'application/vnd.ms-excel', value: 2 },
	{ text: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet', value: 2 },
	{ text: 'application/vnd.openxmlformats-officedocument.spreadsheetml.template', value: 2 },
	{ text: 'application/vnd.ms-excel.sheet.macroEnabled.12', value: 2 },
	{ text: 'application/vnd.ms-excel.template.macroEnabled.12', value: 2 },
	{ text: 'application/vnd.ms-excel.addin.macroEnabled.12', value: 2 },
	{ text: 'application/vnd.ms-excel.sheet.binary.macroEnabled.12', value: 2 },
	{ text: 'application/octet-stream', value: 2 },
	{ text: 'application/pdf', value: 3 },
	{ text: 'image/png', value: 4 },
	{ text: 'image/jpeg', value: 4 },
	{ text: 'image/gif', value: 4 },
	{ text: 'application/x-rar-compressed', value: 5 },
	{ text: 'application/zip', value: 5 },
	{ text: 'application/x-7z-compressed', value: 5 },
	{ text: 'application/vnd.ms-powerpoint', value: 6 },
	{ text: 'application/vnd.openxmlformats-officedocument.presentationml.presentation', value: 6 },
	{ text: 'application/vnd.openxmlformats-officedocument.presentationml.template', value: 6 },
	{ text: 'application/vnd.openxmlformats-officedocument.presentationml.slideshow', value: 6 },
	{ text: 'application/vnd.ms-powerpoint.addin.macroEnabled.12', value: 6 },
	{ text: 'application/vnd.ms-powerpoint.presentation.macroEnabled.12', value: 6 },
	{ text: 'application/vnd.ms-powerpoint.template.macroEnabled.12', value: 6 },
	{ text: 'application/vnd.ms-powerpoint.slideshow.macroEnabled.12', value: 6 },
	{ text: 'video/mp4', value: 7 },
]

export class REGEX {
	public static PHONE_VN = '^(84|0[3|5|7|8|9])+([0-9]{8})$'
}
