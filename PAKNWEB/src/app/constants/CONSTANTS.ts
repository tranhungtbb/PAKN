export class CONSTANTS {
	public static FILEACCEPT = '.pdf, .png, .jpg, .jpeg, .doc, .docx, .xls, .xlsx, .txt'
	public static PAGE_SIZE = 20
	public static PAGE_INDEX = 1
}

export class RESPONSE_STATUS {
	public static success = 'OK'
	public static orror = 'ORROR'
	public static incorrect = 'INCORRECT'
}

export class LOG_ACTION {
	public static login = 'Đăng nhập'
	public static GETLIST = 'Trả về danh sách'
	public static GETINFO = 'Trả về thông tin'
	public static INSERT = 'Thêm mới'
	public static UPDATE = 'Cập nhật'
	public static UPDATESTATUS = 'Cập nhật trạng thái'
	public static DELETE = 'Xóa'
	public static EXPORT = 'Xuất danh sách'
}

export class LOG_OBJECT {
	public static login = 'Đăng nhập'
	public static CA_FIELD = 'Danh mục lĩnh vực'
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
]
