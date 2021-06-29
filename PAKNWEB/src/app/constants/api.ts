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
	public static DownloadApp = 'Files/DownloadApp'
	public static GetEncryptedPath = 'Files/GetEncryptedPath'
	public static download = 'Files/DownloadFile'
	public static DownloadFilebyId = 'Files/DownloadFilebyId'
	public static downloadFileSupport = 'Files/DownloadFileSupport'
	public static GetFile = 'Files/GetFile'
	public static UploadImageNews = 'Files/UploadImageNews'
	public static getFileImage = 'Files/GetFile'
	public static getFileSupport = 'Files/GetFileSupport'

	//Recommendation
	public static RecommendationGetDataForCreate = 'Recommendation/RecommendationGetDataForCreate'
	public static RecommendationGetDataForForward = 'Recommendation/RecommendationGetDataForForward'
	public static RecommendationGetDataForProcess = 'Recommendation/RecommendationGetDataForProcess'
	public static RecommendationGetList = 'MRSPBase/MRRecommendationGetAllOnPageBase'
	public static RecommendationGetListProcess = 'MRSPBase/MRRecommendationGetAllWithProcessBase'
	public static RecommendationGetListReactionaryWord = 'MRSPBase/MRRecommendationGetAllReactionaryWordBase'
	public static RecommendationGetById = 'Recommendation/RecommendationGetById'
	public static RecommendationGetByIdView = 'Recommendation/RecommendationGetByIdView'
	public static RecommendationGetHistories = 'MRSPBase/HISRecommendationGetByObjectIdBase'
	public static RecommendationInsert = 'Recommendation/RecommendationInsert'
	public static RecommendationUpdate = 'Recommendation/RecommendationUpdate'
	public static RecommendationForward = 'Recommendation/RecommendationForward'
	public static RecommendationProcess = 'Recommendation/RecommendationOnProcess'
	public static RecommendationUpdateStatus = 'Recommendation/RecommendationUpdateStatus'
	public static RecommendationOnProcessConclusion = 'Recommendation/RecommendationOnProcessConclusion'
	public static RecommendationDelete = 'MRSPBase/MRRecommendationDeleteBase'
	public static RecommendationExport = 'MRSPBase/MRRecommendationExportBase'
	public static RecommendationGetSuggestCreate = 'MRSPBase/MRRecommendationGetSuggestCreateBase'
	public static RecommendationGetSuggestReply = 'MRSPBase/MRRecommendationGetSuggestReplyBase'
	public static RecommendationGetDataGraph = 'MRSPBase/MRRecommendationGetDataGraphBase'
	public static RecommendationGetSendUserDataGraph = 'MRSPBase/MRRecommendationGetSendUserDataGraphBase'
	public static RecommendationGetByHashtagAllOnPage = 'MRSPBase/MRRecommendationGetByHashtagAllOnPageBase'
	public static RecommendationGetDenyContents = 'MRSPBase/MRRecommendationGetDenyContentsBase'
	public static InsertHashtagForRecommentdation = 'MRSPBase/MRInsertHashtagForRecommentdation'
	public static DeleteHashtagForRecommentdation = 'MRSPBase/MRDeleteHashtagForRecommentdation'

	public static PuRecommendationGetAllOnPage = 'PURecommendation/PURecommendationAllOnPage'
	public static PuRecomentdationGetListOrderByCountClick = 'PURecommendation/PURecommendationGetListOrderByCountClick'
	public static MyRecommendationGetAllOnPage = 'PURecommendation/MyRecommendationAllOnPage'
	public static PuRecommendationGetById = 'PURecommendation/PURecommendationGetById'
	public static PuChangeSatisfaction = 'PURecommendation/ChangeSatisfaction'
	public static PuRecommendationCountClick = 'PURecommendation/PURecommendationCountClick'
	public static PURecommendationStatisticsGetByUserId = 'PURecommendation/PURecommendationStatisticsGetByUserIdBase'

	public static MRRecommendationCommentInsert = 'MRSPBase/MRCommnentInsertBase'
	public static MRRecommendationCommentGetOnPage = 'MRSPBase/MRCommnentGetAllOnPageBase'

	//Field
	public static FieldGetList = 'CACategorySPBase/CAFieldGetAllOnPageBase'
	public static FieldGetDropDown = 'CASPBase/CAFieldGetDropdownBase'
	public static FieldGetById = 'CACategorySPBase/CAFieldGetByIDBase'
	public static FieldInsert = 'CACategorySPBase/CAFieldInsertBase'
	public static FieldUpdate = 'CACategorySPBase/CAFieldUpdateBase'
	public static FieldUpdateStatus = 'CACategorySPBase/CAFieldUpdateBase'
	public static FieldDelete = 'CACategorySPBase/CAFieldDeleteBase'

	//Field
	public static HashtagGetAll = 'CATableBase/CAHashtagGetAll'
	public static HashtagGetList = 'CAHashtag/CAHashtagGetAllOnPage'
	public static HashtagGetById = 'CACategorySPBase/CAHashtagGetByID'
	public static HashtagInsert = 'CATableBase/CAHashtagInsert'
	public static HashtagUpdate = 'CAHashtag/CAHashtagUpdate'
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
	public static UnitChangeStatus = 'SYSPBase/SYUnitChageStatusBase'
	public static UnitCheckExists = 'SYSPBase/SYUnitCheckExistsBase'
	public static UnitGetChildrenDropdown = 'Statistic/SY_UnitGetChildrenDropdown'
	public static FieldExport = 'CACategorySPBase/CAFieldExportBase'

	//Position
	public static PositionInsert = 'CACategorySPBase/CAPositionInsertBase'
	public static PositionGetList = 'CACategorySPBase/CAPositionGetAllOnPageBase'
	public static PositionDelete = 'CACategorySPBase/CAPositionDeleteBase'
	public static PositionGetById = 'CACategorySPBase/CAPositionGetByIDBase'
	public static PositionUpdate = 'CACategorySPBase/CAPositionUpdateBase'

	// user
	public static UserGetPagedList = 'SYUserSPBase/SYUserGetAllOnPageBase'
	public static UserGetAllOnPagedList = 'User/SYUserGetAllOnPageList'
	public static UsersGetDataForCreate = 'User/UsersGetDataForCreate'
	public static UserGetById = 'SYUserSPBase/SYUserGetByIDBase'
	public static UserGetByRoleIdOnPage = 'SYUserSPBase/SYUserGetByRoleIdAllOnPageBase'
	public static UserGetByRoleId = 'SYUserSPBase/SYUserGetAllByRoleIdBase'
	public static UserInsert = 'User/Create' //'SYUserSPBase/SYUserInsertBase'
	public static UserUpdate = 'User/Update' //'SYUserSPBase/SYUserUpdateBase'
	public static UserDelete = 'User/Delete' //'SYUserSPBase/SYUserDeleteBase'GetAvatar
	public static UserGetByIdUpdate = 'User/UserGetByID'
	public static UserGetAvatar = 'User/GetAvatar' //'SYUserSPBase/SYUserDeleteBase'
	public static UserChangeStatus = 'SYUserSPBase/SYUserChangeStatusBase'
	public static UserChangePwdInManage = 'User/UserChangePwdInManage'
	public static UserCheckExists = 'SYUserSPBase/SYUserCheckExistsBase'

	public static SystemLogin = 'SYSPBase/SYSystemLogGetAllOnPageBase'
	public static SystemLoginAdmin = 'SYSPBase/SYSystemLogGetAllOnPageAdminBase'
	public static SystemLogDelete = 'AdministrationFormalities/SYSystemLogDeleteBase'
	public static SystemGetUserDropDown = 'SYSPBase/SYUsersGetDropdownBase'
	public static UserGetIsSystem = 'SYUserSPBase/SYUserGetIsSystemBase'
	public static UserGetIsSystem2 = 'SYUserSPBase/SYUserGetIsSystem2Base'
	public static getIsSystemOrderByUnit = 'User/GetOrderByUnit'
	public static DeleteUserRole = 'SYUserSPBase/SYUserRoleMapDeleteBase'
	public static InsertMultiUserRole = 'UserMapRole/SYUserRoleMapListInsert'

	//SY_Role
	public static RoleGetAll = 'SYSPBase/SYRoleGetAllBase'
	public static RoleGetAllOnPage = 'SYSPBase/SYRoleGetAllOnPageBase'
	public static RoleInsert = 'SYSPBase/SYRoleInsertBase'
	public static RoleUpdate = 'SYSPBase/SYRoleUpdateBase'
	public static RoleDelete = 'SYSPBase/SYRoleDeleteBase'
	public static RoleGetById = 'Role/GetByID'
	public static RoleInsertPermission = 'SYSPBase/SYPermissionGroupUserInsertByListBase'
	public static RoleGetDataForCreate = 'Role/GetDataForCreate'

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
	public static WordGetListByGroupId = 'CACategorySPBase/CAWordGetAllOnPageByGroupIdBase'
	public static WordGetById = 'CACategorySPBase/CAWordGetByIDBase'
	public static WordInsert = 'CACategorySPBase/CAWordInsertBase'
	public static WordUpdate = 'CACategorySPBase/CAWordUpdateBase'
	public static WordUpdateStatus = 'CACategorySPBase/CAWordUpdateBase'
	public static WordDelete = 'CACategorySPBase/CAWordDeleteBase'
	public static WordGetListSuggest = 'CACategorySPBase/CAWordGetListSuggestBase'
	//GroupWord
	public static GroupWordGetList = 'CACategorySPBase/CAGroupWordGetAllOnPageBase'
	public static GroupWordGetById = 'CACategorySPBase/CAGroupWordGetByIDBase'
	public static GroupWordInsert = 'CACategorySPBase/CAGroupWordInsertBase'
	public static GroupWordUpdate = 'CACategorySPBase/CAGroupWordUpdateBase'
	public static GroupWordUpdateStatus = 'CACategorySPBase/CAGroupWordUpdateBase'
	public static GroupWordDelete = 'CACategorySPBase/CAGroupWordDeleteBase'
	public static GroupWordGetListSuggest = 'CACategorySPBase/CAGroupWordGetListSuggestBase'

	//tin tuc
	public static NewsGetAllOnPage = 'NESPBase/NENewsGetAllOnPageBase'
	public static NewsGetListHomePage = 'NENews/NENewsGetListHomePage'
	public static NewsGetById = 'NESPBase/NENewsGetByIDBase'
	public static NewsInsert = 'NESPBase/NENewsInsertBase'
	public static NewsUpdate = 'NESPBase/NENewsUpdateBase'
	public static NewsDelete = 'NESPBase/NENewsDeleteBase'
	public static NewsUploadFile = 'files/upload?folder=News'
	public static NewsRelatesGetAll = 'NESPBase/NERelateGetAllBase'
	public static NewsGetAvatar = 'files/get-news-avatar'
	public static NewsGetAvatars = 'files/get-news-avatar'
	public static HisNewsInsert = 'HISNews/HISNewsInsert'
	public static HisNewsGetListByNewsId = 'HISNews/HISNewsGetByNewsId'
	public static NewsGetViewDetail = 'NESPBase/NENewsViewDetailBase'
	public static NewsGetAllRelates = 'NESPBase/NENewsGetAllRelatesBase'

	//Department
	public static DepartmentGetList = 'CACategorySPBase/CADepartmentGetAllOnPageBase'
	public static DepartmentGetById = 'CACategorySPBase/CADepartmentGetByIDBase'
	public static DepartmentInsert = 'CACategorySPBase/CADepartmentInsertBase'
	public static DepartmentUpdate = 'CACategorySPBase/CADepartmentUpdateBase'
	public static DepartmentUpdateStatus = 'CACategorySPBase/CADepartmentUpdateBase'
	public static DepartmentDelete = 'CACategorySPBase/CADepartmentDeleteBase'

	//remind
	public static RemindInsert = 'RMRemind/RemindInsert'
	public static RemindGetList = 'RMRemind/RemindGetList'

	//invitation

	public static InnvitationInsert = 'INVInvitation/INVInvitationInsert'
	public static InnvitationUpdate = 'INVInvitation/INVInvitationUpdate'
	public static InvitationDelete = 'INVInvitation/INVInvitationDelete'
	public static InvitationGetList = 'INVSPBase/INVInvitationGetAllOnPageBase'
	public static InnvitationDetail = 'INVInvitation/INVInvitationDetail'
	public static UserReadedInvitationGetList = 'INVSPBase/SYUserReadedInvitationGetAllOnPage'

	//sms
	public static SMSManagementGetOnPage = 'SMSSPBase/SMSQuanLyTinNhanGetAllOnPageBase'
	public static SMSManagementGetHisOnPage = 'HISPBase/HISSMSGetBySMSIdOnPageBase'
	public static SMSManagementInsert = 'EmailSMS/SMSInsert'
	public static SMSManagementUpdate = 'EmailSMS/SMSUpdate'
	public static SMSManagementUpdateStatusSend = 'EmailSMS/SMSUpdateStatusTypeSend'
	public static SMSManagementDelete = 'EmailSMS/SMSDelete'
	public static SMSManagementGetById = 'SMSSPBase/SMSQuanLyTinNhanGetByIdBase'
	public static AdministrativeUnits = 'CASPBase/CAAdministrativeUnitsGetDropDownBase'
	public static GetListIndividualAndBusinessByAdmintrativeUnitId = 'BISPBase/BIIndividualOrBusinessGetDropListByProviceIdBase'
	public static HISSMSInsert = 'EmailSMS/HISSMSInsert'

	// SYIntroduce

	public static SYIntroduceGetInfo = 'SYIntroduce/IntroduceGetInfo'
	public static SYIntroduceUnitGetInfo = 'SYIntroduce/IntroduceUnitGetById'
	public static SYIntroduceUpdate = 'SYIntroduce/IntroduceUpdate'
	public static SYIntroduceUnitGetOnPage = 'SYIntroduce/IntroduceUnitGetOnPage'
	public static SYIntroduceUnitInsert = 'SYIntroduce/IntroduceUnitInsert'
	public static SYIntroduceUnitUpdate = 'SYIntroduce/IntroduceUnitUpdate'
	public static SYIntroduceUnitDetete = 'SYIntroduce/IntroduceUnitDetete'

	// support

	public static SYSupportGetAllByCategory = 'SYSupport/SYSupportGetByCategory'
	public static SYSupportInsert = 'SYSupport/SYSupportInsert'
	public static SYSupportUpdate = 'SYSupport/SYSupportUpdate'
	public static SYSupportDelete = 'SYSupport/SYSupportDetete'

	// SYIndexSetting

	public static SYIndexSettingGetInfo = 'SYIndexSetting/IndexSettingGetInfo'
	public static SYIndexSettingUpdate = 'SYIndexSetting/IndexSettingUpdate'
	public static SYIndexWebsiteInsert = 'SYIndexSetting/IndexWebsiteInsert'
	public static SYIndexWebsiteGetAll = 'SYIndexSetting/IndexWebsiteGetAll'

	//RequestRecommendation
	public static RecommendationRequestGetListProcess = 'MRSPBase/MRRecommendationKNCTGetAllWithProcessBase'
	public static RecommendationRequestGetDataForCreate = 'CACategorySPBase/CAFieldKNCTGetDropdownBase'

	//api dia danh
	public static ProvinceGetAll = 'CASPBase/CAProvinceGetAllBase'
	public static DistrictGetAll = 'CASPBase/CADistrictGetAllBase'
	public static VillageGetAll = 'CASPBase/CAVillageGetAllBase'
	public static GetAllByProvinceId = 'caspbase/GetAllByProvinceId'

	//register
	public static RegisterIndividual = 'User/InvididualRegister'
	public static RegisterOrganization = 'User/OrganizationRegister'
	public static IndividualCheckExists = 'BISPBase/BIIndividualCheckExistsBase'
	public static OrganizationCheckExists = 'BISPBase/BIBusinessCheckExistsBase'

	public static RecommendationRequestGetByIdView = 'MRSPBase/MRRecommendationKNCTGetByIdBase'
	public static RecommendationRequestGetFile = 'MRSPBase/MRRecommendationKNCTFilesGetByRecommendationIdBase'

	//tai khoan ca nhan, doanh nghiep
	public static AccountGetInfo = 'user/UserGetInfo'
	public static AccountChangePassword = 'User/UserChagePwd'
	public static AccountUpdateInfo = 'User/UpdateCurrentInfo'

	//Administrative Formalities
	public static AdministrativeFormalitiesGetDropdown = 'AdministrativeFormalities/AdministrativeFormalitiesGetDropdown'
	public static AdministrativeFormalitiesGetList = 'DAMSPBase/DAMAdministrationGetListBase'
	public static AdministrativeFormalitiesGetListHomePage = 'DAMSPBase/DAMAdministrationGetListTopBase'
	public static AdministrativeFormalitiesGetById = 'AdministrationFormalities/AdministrationFormalitiesGetByID'
	public static AdministrativeFormalitiesGetByIdView = 'AdministrativeFormalities/RecommendationGetByIdView'
	public static AdministrativeFormalitiesInsert = 'AdministrationFormalities/AdministrationFormalitiesInsert'
	public static AdministrativeFormalitiesUpdate = 'AdministrationFormalities/AdministrationFormalitiesUpdate'
	public static AdministrativeFormalitiesDelete = 'DAMSPBase/DAMAdministrationDeleteBase'
	public static AdministrativeFormalitiesUpdateShow = 'DAMSPBase/DAMAdministrationUpdateShowBase'
	public static AdministrativeFormalitiesCAFieldDAM = 'CASPBase/CAFieldDAMGetDropdownBase'

	// Notification
	public static NotificationInsertTypeNews = 'SYNotification/SYNotificationInsertTypeNews'
	public static NotificationInsertTypeRecommendation = 'SYNotification/SYNotificationInsertTypeRecommendation'
	public static NotificationGetList = 'SYNotification/SYNotificationGetListOnPage'
	public static NotificationGetById = 'SYNotification/NotificationGetById'
	public static NotificationDelete = 'SYNotification/SYNotificationDelete'
	public static NotificationUpdateIsViewed = 'SYNotification/SYNotificationUpdateIsViewed'
	public static NotificationUpdateIsReaded = 'SYNotification/SYNotificationUpdateIsReaded'

	//Chatbot
	public static ChatbotGetList = 'Chatbot/ChatbotGetAllOnPageBase'
	public static ChatbotDelete = 'Chatbot/ChatbotDeleteBase'
	public static ChatbotGetById = 'Chatbot/ChatbotGetByID'
	public static ChatbotUpdate = 'Chatbot/ChatbotUpdateBase'
	public static ChatbotInsertQuestion = 'Chatbot/ChatbotInsertQuestion'
	public static ChatbotInsertData = 'Chatbot/ChatbotInsertData'
	public static ChatbotGetListHistory = 'Chatbot/HistoryChatbotGetAllOnPage'

	//systemConfig
	public static EmailGetFirstBase = 'SYSPBase/SYEmailGetFirstBase'
	public static EmailConfigSystemUpdate = 'SYSPBase/SYEmailInsertBase'
	public static SMSGetFirstBase = 'SYSPBase/SYSMSGetFirstBase'
	public static SMSConfigSystemUpdate = 'SYSPBase/SYSMSInsertBase'

	public static SYConfigSystemGetAllOnPage = 'SYSPBase/SYConfigGetAllOnPageBase'
	public static SYConfigSystemGetById = 'SYSPBase/SYConfigGetByIDBase'
	public static SYConfigSystemUpdate = 'SYSPBase/SYConfigUpdateBase'

	public static TimeConfigInsert = 'SYSPBase/SYTimeInsertBase'
	public static TimeConfigUpdate = 'SYSPBase/SYTimeUpdateBase'
	public static TimeConfigGetAllOnPage = 'SYSPBase/SYTimeGetAllOnPageBase'
	public static TimeConfigDelete = 'SYSPBase/SYTimeDeleteBase'
	public static TimeConfigGetById = 'SYSPBase/SYTimeGetByIdBase'
	public static TimeConfigGetDateActive = 'SYSPBase/SYTimeGetDateActiveBase'

	//Individual
	public static IndividualGetAllOnPage = 'BusinessIndividual/IndividualGetAllOnPage'
	public static IndivialChangeStatus = 'BusinessIndividual/IndivialChangeStatus'
	public static IndivialDelete = 'BusinessIndividual/IndivialDelete'
	public static InvididualRegister = 'BusinessIndividual/InvididualRegister'
	public static InvididualGetByID = 'BusinessIndividual/InvididualGetByID'
	public static InvididualUpdate = 'BusinessIndividual/InvididualUpdate'
	public static ImportDataInvididual = 'BusinessIndividual/ImportDataInvididual?folder=BusinessIndividual'
	public static InvididualCheckExists = 'BusinessIndividual/BIInvididualCheckExistsBase'

	//Business
	public static BusinessGetAllOnPage = 'BusinessIndividual/BusinessGetAllOnPage'
	public static BusinessChangeStatus = 'BusinessIndividual/BusinessChangeStatus'
	public static BusinessDelete = 'BusinessIndividual/BusinessDelete'
	public static BusinessRegister = 'BusinessIndividual/BusinessRegister'
	public static BusinessGetByID = 'BusinessIndividual/BusinessGetByID'
	public static BusinessUpdate = 'BusinessIndividual/BusinessUpdate'
	public static ImportDataBusiness = 'BusinessIndividual/ImportDataBusiness?folder=BusinessIndividual'

	//Statistic

	public static StatisticRecommendationByUnit = 'Statistic/STT_RecommendationByUnit'
	public static StatisticRecommendationByUnitDetail = 'Statistic/STT_RecommendationsByUnitDetail'
	public static StatisticRecommendationByField = 'Statistic/STT_RecommendationByField'
	public static StatisticRecommendationByFieldDetail = 'Statistic/STT_RecommendationsByFieldDetail'
	public static StatisticRecommendationByGroupWord = 'Statistic/STT_RecommendationByGroupWord'
	public static StatisticRecommendationByGroupWordDetail = 'Statistic/STT_RecommendationByGroupWordDetail'

	// import
	public static BusinessImportFile = 'BusinessIndividual/ImportDataBusiness?folder=BusinessIndividual'
	public static PU_Support = 'PUSPBase/PUSupportDocument'

	//call history
	public static CallHistoryGetPagedList = 'SYCallHistory/SYCallHistoryGetPagedList'

	//dự thảo email
	public static EmailManagementUpdate = 'EmailManagement/Update'
	public static EmailManagementGetById = 'EmailManagement/GetById'
	public static EmailManagementGetPagedList = 'EmailManagement/GetPagedList'
	public static EmailManagementDelete = 'EmailManagement/delete'
	public static EmailManagementSendEmail = 'EmailManagement/SendEmail'
	public static EmailManagementHisPagedList = 'EmailManagement/GetHisPagedList'


	// đồng bộ
	public static MrSyncCongThongTinDienTuTinhPagedList = 'RecommandationSync/CongThongTinDienTuTinhPagedList'
	public static MrSyncCongThongTinDichVuHCCPagedList = 'RecommandationSync/CongThongTinDichVuHCCPagedList'
	public static MrSyncHeThongPANKChinhPhuPagedList = 'RecommandationSync/HeThongPANKChinhPhuPagedList'
	public static MrSyncHeThongQuanLyKienNghiCuTriPagedList = 'RecommandationSync/HeThongQuanLyKienNghiCuTriPagedList'
	public static PU_RecommandationSyncPagedList = 'RecommandationSync/PUGetPagedList'
	public static PU_RecommandationSyncGetDetail = 'RecommandationSync/PUGetGetById'

	
}
