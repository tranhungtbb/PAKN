export class Api {
	//DashBoard
	public static GET_DASH_BOARD = 'DashBoard/GetDashBoard'
	//Account
	public static LOGIN = 'Contact/Login'
	public static REGISTER = 'Contact/Register'
	public static FORGETPASSWORD = 'Contact/Forgetpassword'
	public static CHANGEPASSWORD = 'Contact/ChangePassword'
	public static RESTORE_ACCOUNT = 'Contact/UnlockAccount'
	public static logOut = 'Contact/LogOut'

	// Captcha Controller
	public static ValidateCaptcha = 'Captcha/ValidatorCaptcha'
	public static getImageCaptcha = 'Captcha/GetCaptchaImage'

	// FileUpload/Download
	public static DownloadApp = 'UploadFiles/DownloadApp'
	public static GetEncryptedPath = 'UploadFiles/GetEncryptedPath'
	public static download = 'UploadFiles/DownloadFile'
	public static DownloadFilebyId = 'UploadFiles/DownloadFilebyId'
	public static downloadFileSupport = 'UploadFiles/DownloadFileSupport'
	public static GetFile = 'UploadFiles/GetFile'
	public static uploadfiles = 'UploadFiles/Uploadfiles'
	public static getFileImage = 'UploadFiles/GetFile'

	//Recommendation
	public static RecommendationGetList = 'MRSPBase/MRRecommendationGetAllOnPageBase'
	public static RecommendationGetById = 'MRSPBase/MRRecommendationGetByIDBase'
	public static RecommendationInsert = 'MRSPBase/MRRecommendationInsertBase'
	public static RecommendationUpdate = 'MRSPBase/MRRecommendationUpdateBase'
	public static RecommendationDelete = 'MRSPBase/MRRecommendationDeleteBase'
	public static RecommendationExport = 'MRSPBase/MRRecommendationExportBase'

	//Field
	public static FieldGetList = 'CACategorySPBase/CAFieldGetAllOnPageBase'
	public static FieldGetById = 'CACategorySPBase/CAFieldGetByIDBase'
	public static FieldInsert = 'CACategorySPBase/CAFieldInsertBase'
	public static FieldUpdate = 'CACategorySPBase/CAFieldUpdateBase'
	public static FieldUpdateStatus = 'CACategorySPBase/CAFieldUpdateBase'
	public static FieldDelete = 'CACategorySPBase/CAFieldDeleteBase'

	//Unit
	public static UnitGetPagedList = 'CACategorySPBase/CAUnitGetAllOnPageBase'
	public static UnitGetById = 'CACategorySPBase/CAUnitGetByIDBase'
	public static UnitInsert = 'CACategorySPBase/CAUnitInsertBase'
	public static UnitUpdate = 'CACategorySPBase/CAUnitUpdateBase'
	public static UnitDeleteList = 'CACategorySPBase/CAUnitDeleteListBase'
	public static UnitDelete = 'CACategorySPBase/CAUnitDeleteBase'

	public static FieldExport = 'CACategorySPBase/CAFieldExportBase'

	//Position
	public static PositionInsert = 'CACategorySPBase/CAPositionInsertBase'
	public static PositionGetList = 'CACategorySPBase/CAPositionGetAllOnPageBase'
	public static PositionDelete = 'CACategorySPBase/CAPositionDeleteBase'
	public static PositionGetById = 'CACategorySPBase/CAPositionGetByIDBase'
	public static PositionUpdate = 'CACategorySPBase/CAPositionUpdateBase'

	// SY_Unit
	public static UserGetPagedList = 'SYUserSPBase/SYUserGetAllOnPageBase'
	public static UserGetById = 'SYUserSPBase/SYUserGetAllOnPageBase'
	public static UserInsert = 'SYUserSPBase/SYUserInsertBase'
	public static UserUpdate = 'SYUserSPBase/SYUserUpdateBase'
	public static UserDelete = 'SYUserSPBase/SYUserDeleteBase'

	//NewsType
	public static NewsTypeGetList = 'CACategorySPBase/CANewsTypeGetAllOnPageBase'
	public static NewsTypeGetById = 'CACategorySPBase/CANewsTypeGetByIDBase'
	public static NewsTypeInsert = 'CACategorySPBase/CANewsTypeInsertBase'
	public static NewsTypeUpdate = 'CACategorySPBase/CANewsTypeUpdateBase'
	public static NewsTypeUpdateStatus = 'CACategorySPBase/CANewsTypeUpdateBase'
	public static NewsTypeDelete = 'CACategorySPBase/CANewsTypeDeleteBase'
	//DepartmentGroup
	public static DepartmentGroupGetList = 'CACategorySPBase/CADepartmentGroupGetAllOnPageBase'
	public static DepartmentGroupGetById = 'CACategorySPBase/CADepartmentGroupGetByIDBase'
	public static DepartmentGroupInsert = 'CACategorySPBase/CADepartmentGroupInsertBase'
	public static DepartmentGroupUpdate = 'CACategorySPBase/CADepartmentGroupUpdateBase'
	public static DepartmentGroupUpdateStatus = 'CACategorySPBase/CADepartmentGroupUpdateBase'
	public static DepartmentGroupDelete = 'CACategorySPBase/CADepartmentGroupDeleteBase'
}
