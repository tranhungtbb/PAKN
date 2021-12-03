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
	public static INSERT_AND_SEND = 'Thêm mới và Gửi'
	public static UPDATE = 'Cập nhật'
	public static UPDATE_AND_SEND = 'Cập nhập và Gửi'
	public static FORWARD = 'Chuyển tiếp'
	public static PROCESSED = 'Xử lý'
	public static UPDATESTATUS = 'Cập nhật trạng thái'
	public static DELETE = 'Xóa'
	public static EXPORT = 'Xuất danh sách'
	public static CHANGEPASSWORD = 'Đổi mật khẩu'
	public static PUBLIC = 'Công bố'
	public static WITHDRAW = 'Hủy công bố'
	public static RECALL = 'Thu hồi'
	public static SEND = 'Gửi'
	public static ASYNC = 'Đồng bộ'

	public static FORWARD_TT = 'Gửi lại trung tâm'
	public static RECEIVE_DENY = 'Từ chối xử lý' //3 Từ chối xử lý
	public static RECEIVE_APPROVED = 'Tiếp nhận xử lý'
	public static PROCESS_SEND = 'Gửi giải quyết'
	public static PROCESS_DENY = 'Từ chối giải quyết' //6 Từ chối giải quyết
	public static PROCESSING = 'Tiếp nhận giải quyết' //7  giải quyết
	public static APPROVE_SEND = 'Gửi phê duyệt'
	public static APPROVE = 'Phê duyệt'
	public static APPROVE_DENY = 'Từ chối phê duyệt' //9 Từ chối phê duyệt
}

export class LOG_OBJECT {
	public static NO_CONTENT = ''
	public static login = 'Đăng nhập'

	public static CA_FIELD = 'Lĩnh vực'
	public static CA_WORD = 'Thư viện từ ngữ'
	public static CA_GROUPWORD = 'Nhóm thư viện từ ngữ'
	public static CA_DEPARTMENT = 'Sở ngành'
	public static CA_DEPARTMENT_GROUP = 'Nhóm sở ngành'
	public static CA_HASHTAG = 'Hashtag'
	public static CA_POSITION = 'Chức vụ'
	public static CA_NEWS_TYPE = 'Loại tin tức'
	public static DAM_ADMINISTRATOR = 'Thủ tục hành chính'

	public static EMAIL = 'Email'
	public static SMS_EMAIL = 'Tin nhắn sms'
	public static HIS_EMAIL = 'Lịch sử sms'
	public static SY_INVITATION = 'Thư mời'
	public static HIS_INVITATION = 'Lịch sử thư mời'

	public static NE_NEWS = 'Tin tức'

	public static SY_ROLE = 'Vai trò hệ thống'

	public static SY_USER = 'Người dùng hệ thống'
	public static SY_USER_GET_BY_ROLE = 'Người dùng theo vai trò'

	public static HIS_USER = 'Lịch sử người dùng hệ thống'
	public static MR_RECOMMENDATION = 'Phản ánh / Kiến nghị'
	public static MR_RECOMMENDATION_HOPTHUGOPY = 'Hành chính công Khánh Hòa'
	public static MR_RECOMMENDATION_PAKN_CHINHPHU = 'Hệ thống PAKN Chính phủ'
	public static MR_RECOMMENDATION_GOPY = 'Cổng thông tin điện tử tỉnh Khánh Hòa'
	public static MR_HISTORIES = 'Lịch sử phản ánh / Kiến nghị'
	public static RM_REMIND = 'Danh sách nhắc nhở'
	public static MR_BY_HASHTAG = 'Danh sách PAKN theo hashtag'
	public static SY_INTRODUCE = 'Cấu hình trang giới thiệu'
	public static SY_INDEXSETTING = 'Cấu hình trang chủ'
	public static MR_COMMENT = 'Bình luận Phản ánh kiến nghị'
	public static MR_INFOMATIONCHANGE = 'Bình luận Phản ánh kiến nghị'
	public static BI_INDIVIDUAL = 'Cá nhân'
	public static BI_BUSINESS = 'Doanh nghiệp'
	public static SY_TIME = 'Cấu hình thời gian làm việc'
	public static SY_CONFIG = 'Cấu hình hệ thống'
	public static SY_SMS = 'Cấu hình SMS thương hiệu'
	public static SY_UNIT = 'Đơn vị'
	public static SY_CHATBOX = 'Chat box'
	public static SY_SUPPORT = 'Tài liệu hướng dẫn sử dụng'
	public static SY_SUPPORT_VIDEO = 'Video hướng dẫn sử dụng'
	public static SY_SYSTEM = 'Lịch sử hệ thống'
	public static CALL_HISTORY = 'Cuộc gọi'
	public static SY_NOTIFICATION = 'Thông báo'
	public static SY_GALLERY = 'Thư viện ảnh'
	public static SY_PublishNotification = 'Thông báo trang công bố'
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

	public static UPDATED = 11 //đã chuyển
}

export class STEP_RECOMMENDATION {
	public static RECEIVE = 1 //Tiếp nhận
	public static PROCESS = 2 //2 Giải quyết
	public static APPROVE = 3 //3 Phê duyệt
	// riêng biệt
	public static FORWARD_MAIN = 4 // 4 chuyển trung tâm
}

export class TYPE_RECOMMENDATION {
	public static PublicService = 0 // dịch vụ công
	public static Socioeconomic = 1 // kinh tế xã hội
}

export class RECEPTION_TYPE {
	public static Web = 1 //
	public static App = 2 //
	public static Phone = 3 //
	public static Email = 4 //
}

export class TYPE_NOTIFICATION {
	public static NEWS = 1 // thông báo tin tức
	public static RECOMMENDATION = 2 // thông báo PAKN
	public static INVITATION = 3 // thông báo có thư mời
	public static ADMINISTRATIVE = 4 // administrative-formalities
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

export class USER_TYPE {
	public static SYSTEM = 1 // hệ thống
	public static INDIVIDUAL = 2 // Người dân
	public static BUSSINESS = 3 // Doanh nghiệp
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

export const TYPE_CONFIG = [
	{ text: 'Cấu hình email', value: 1 }, //Email hệ thống
	{ text: 'Cấu hình sms', value: 2 }, //SMS hệ thống
	{ text: 'Cấu hình tổng đài', value: 3 }, //Config tổng đài
	{ text: 'Cấu hình chung', value: 4 }, // #
	{ text: 'Cấu hình trang chủ', value: 5 }, // #
	{ text: 'Cấu hình đồng bộ', value: 6 }, // #
]

export class TYPECONFIG {
	public static CONFIG_EMAIL = 1
	public static CONFIG_SMS = 2
	public static CONFIG_SWITCHBOARD = 3
	public static CONFIG_NUMBER_WARNING = 4
	public static TYPE_INDEX = 5
	public static APPLICATION = 7
	public static SYNC_CONFIG = 6
}

export class REGEX {
	public static PHONE_VN = '^(84|0[3|5|7|8|9])+([0-9]{8})$'
}

export class PathSampleFiles {
	public static PathSampleFilesIndividual = '/Upload/BusinessIndividual/Danh-sach-nguoi-dan.xlsx'
	public static PathSampleFilesBusiness = '/Upload/BusinessIndividual/Danh-sach-doanh-nghiep.xlsx'
}
export const LocationURL = 'https://www.googleapis.com/geolocation/v1/geolocate'
export const GoogleApiKey = 'AIzaSyBriVbWgmHEE8CGaEJM6V47Bem3VoYCi0Q'
