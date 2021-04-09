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
	public static RecommendationGetDataForCreate = 'Recommendation/RecommendationGetDataForCreate'
	public static RecommendationGetDataForForward = 'Recommendation/RecommendationGetDataForForward'
	public static RecommendationGetDataForProcess = 'Recommendation/RecommendationGetDataForProcess'
	public static RecommendationGetList = 'MRSPBase/MRRecommendationGetAllOnPageBase'
	public static RecommendationGetListProcess = 'MRSPBase/MRRecommendationGetAllWithProcessBase'
	public static RecommendationGetById = 'Recommendation/RecommendationGetById'
	public static RecommendationGetByIdView = 'Recommendation/RecommendationGetByIdView'
	public static RecommendationGetHistories = 'MRSPBase/HISRecommendationGetByObjectIdBase'
	public static RecommendationInsert = 'Recommendation/RecommendationInsert'
	public static RecommendationUpdate = 'Recommendation/RecommendationUpdate'
	public static RecommendationForward = 'Recommendation/RecommendationForward'
	public static RecommendationProcess = 'Recommendation/RecommendationOnProcess'
	public static RecommendationOnProcessConclusion = 'Recommendation/RecommendationOnProcessConclusion'
	public static RecommendationDelete = 'MRSPBase/MRRecommendationDeleteBase'
	public static RecommendationExport = 'MRSPBase/MRRecommendationExportBase'

	public static PuRecommendationGetAllOnPage = 'PURecommendation/PURecommendationAllOnPage'
	public static PuRecommendationGetById = 'PURecommendation/PURecommendationGetById'
	public static PuChangeSatisfaction = 'PURecommendation/ChangeSatisfaction'

	//Field
	public static FieldGetList = 'CACategorySPBase/CAFieldGetAllOnPageBase'
	public static FieldGetById = 'CACategorySPBase/CAFieldGetByIDBase'
	public static FieldInsert = 'CACategorySPBase/CAFieldInsertBase'
	public static FieldUpdate = 'CACategorySPBase/CAFieldUpdateBase'
	public static FieldUpdateStatus = 'CACategorySPBase/CAFieldUpdateBase'
	public static FieldDelete = 'CACategorySPBase/CAFieldDeleteBase'

	//Field
	public static HashtagGetAll = 'CATableBase/CAHashtagGetAll'
	public static HashtagGetList = 'CATableBase/CAHashtagGetAllOnPage'
	public static HashtagGetById = 'CACategorySPBase/CAHashtagGetByID'
	public static HashtagInsert = 'CATableBase/CAHashtagInsert'
	public static HashtagUpdate = 'CATableBase/CAHashtagUpdate'
	public static HashtagUpdateStatus = 'CATableBase/'
	public static HashtagDelete = 'CATableBase/CAHashtagDelete'

	//Unit
	public static UnitGetPagedList = 'CACategorySPBase/CAUnitGetAllOnPageBase'
	public static UnitGetAll = 'CACategorySPBase/CAUnitGetAllBase'
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

	// user
	public static UserGetPagedList = 'SYUserSPBase/SYUserGetAllOnPageBase'
	public static UserGetById = 'SYUserSPBase/SYUserGetByIDBase'
	public static UserInsert = 'User/Create' //'SYUserSPBase/SYUserInsertBase'
	public static UserUpdate = 'User/Update' //'SYUserSPBase/SYUserUpdateBase'
	public static UserDelete = 'User/Delete' //'SYUserSPBase/SYUserDeleteBase'GetAvatar
	public static UserGetAvatar = 'User/GetAvatar' //'SYUserSPBase/SYUserDeleteBase'

	//SY_Role
	public static RoleGetAll = 'SYSPBase/SYRoleGetAllBase'

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
	//Word
	public static WordGetList = 'CACategorySPBase/CAWordGetAllOnPageBase'
	public static WordGetById = 'CACategorySPBase/CAWordGetByIDBase'
	public static WordInsert = 'CACategorySPBase/CAWordInsertBase'
	public static WordUpdate = 'CACategorySPBase/CAWordUpdateBase'
	public static WordUpdateStatus = 'CACategorySPBase/CAWordUpdateBase'
	public static WordDelete = 'CACategorySPBase/CAWordDeleteBase'

	//tin tuc
	public static NewsGetAllOnPage = 'NESPBase/NENewsGetAllOnPageBase'
	public static NewsGetById = 'NESPBase/NENewsGetByIDBase'
	public static NewsInsert = 'NESPBase/NENewsInsertBase'
	public static NewsUpdate = 'NESPBase/NENewsUpdateBase'
	public static NewsDelete = 'NESPBase/NENewsDeleteBase'
	public static NewsUploadFile = 'files/upload?folder=News'
	public static NewsRelatesGetAll = 'NESPBase/NERelateGetAllBase'
	public static NewsGetAvatar = 'files/get-news-avatar'
	public static NewsGetAvatars = 'files/get-news-avatar'

	//Department
	public static DepartmentGetList = 'CACategorySPBase/CADepartmentGetAllOnPageBase'
	public static DepartmentGetById = 'CACategorySPBase/CADepartmentGetByIDBase'
	public static DepartmentInsert = 'CACategorySPBase/CADepartmentInsertBase'
	public static DepartmentUpdate = 'CACategorySPBase/CADepartmentUpdateBase'
	public static DepartmentUpdateStatus = 'CACategorySPBase/CADepartmentUpdateBase'
	public static DepartmentDelete = 'CACategorySPBase/CADepartmentDeleteBase'

	public static RemindInsert = 'RMRemind/RemindInsert'
	public static RemindGetList = 'RMRemind/RemindGetList'

	//RequestRecommendation
	public static RecommendationRequestGetListProcess = 'MRSPBase/MRRecommendationKNCTGetAllWithProcessBase'
	public static RecommendationRequestGetDataForCreate = 'CACategorySPBase/CAFieldKNCTGetDropdownBase'
}
