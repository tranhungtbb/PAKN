export class Api {
	//DashBoard
	public static GET_DASH_BOARD = 'DashBoard/GetDashBoard'
	//Account
	public static LOGIN = 'contact/login'
	public static FORGETPASSWORD = 'contact/forget-password'
	public static logOut = 'contact/log-out'

	// kha nang ko dùng
	public static REGISTER = 'contact/register'
	public static CHANGEPASSWORD = 'contact/ChangePassword'
	public static RESTORE_ACCOUNT = 'contact/UnlockAccount'

	// Captcha Controller
	public static ValidateCaptcha = 'captcha/validator-captcha'
	public static getImageCaptcha = 'captcha/get-captcha-image'

	// FileUpload/Download
	public static DownloadApp = 'files/DownloadApp'
	public static GetEncryptedPath = 'files/GetEncryptedPath'
	public static download = 'files/download-file'
	public static DownloadFilebyId = 'files/DownloadFilebyId'
	public static downloadFileSupport = 'files/download-file-support'
	public static GetFile = 'files/get-file'
	public static UploadImageNews = 'upload-files/upload-image-news'
	public static getFileImage = 'files/get-file'
	public static getFileSupport = 'files/GetFileSupport'

	//Recommendation
	public static RecommendationGetDataForCreate = 'recommendation/get-data-for-create'
	public static RecommendationGetDataForSearch = 'recommendation/get-data-for-search'
	public static RecommendationGetDataForForward = 'recommendation/get-data-for-forward'
	public static RecommendationGetDataForProcess = 'recommendation/get-data-for-process'
	public static RecommendationGetList = 'recommendation/get-list-recommentdation-on-page'
	public static RecommendationGetListProcess = 'recommendation/get-list-recommentdation-process-on-page'
	public static RecommendationCombination = 'recommendation/get-list-recommentdation-combination-on-page'

	public static RecommendationGetListReactionaryWord = 'recommendation/get-list-recommentdation-reactionary-word'
	public static RecommendationGetListFakeImage = 'recommendation/get-list-recommentdation-fake-image'
	public static RecommendationGetById = 'recommendation/get-by-id'
	public static RecommendationGetByIdView = 'recommendation/get-detail-by-id'
	public static RecommendationCombineGetByIdView = 'recommendation/get-detail-mr-combine-by-id'

	public static RecommendationGetByIdViewPublic = 'recommendation/get-detail-public-by-id'

	public static RecommendationGetHistories = 'recommendation/get-his-by-recommentdation'
	public static RecommendationInsert = 'recommendation/insert'
	public static RecommendationUpdate = 'recommendation/update'
	public static RecommendationForward = 'recommendation/recommendation-forward'
	public static RecommendationCombine = 'recommendation/recommendation-combine-insert'
	public static RecommendationProcess = 'recommendation/recommendation-on-process'
	public static RecommendationUpdateStatus = 'recommendation/recommendation-update-status'
	public static RecommendationOnProcessConclusion = 'recommendation/recommendation-on-process-conclusion'
	public static RecommendationOnProcessConclusionCombine = 'recommendation/recommendation-on-process-conclusion-combine'
	public static RecommendationDelete = 'recommendation/delete'
	public static RecommendationExport = 'recommendation/MRRecommendationExportBase' // cái này bỏ
	public static RecommendationGetSuggestCreate = 'recommendation/recommendation-get-suggest-create'
	public static RecommendationGetSuggestReply = 'recommendation/recommendation-get-suggest-reply'
	public static RecommendationGetDataGraph = 'recommendation/recommendation-get-data-graph'
	public static RecommendationGetSendUserDataGraph = 'recommendation/recommendation-get-send-user-data-graph'
	public static RecommendationGetByHashtagAllOnPage = 'recommendation/get-list-recommendation-by-hashtag-on-page'
	public static RecommendationGetDenyContents = 'recommendation/recommendation-get-deny-contents'
	public static InsertHashtagForRecommentdation = 'recommendation/insert-hashtag-for-recommentdation'
	public static DeleteHashtagForRecommentdation = 'recommendation/delete-hashtag-for-recommentdation'
	public static MRRecommendationCommentInsert = 'recommendation/insert-comment'
	public static MRRecommendationCommentUpdateStatus = 'recommendation/update-status-comment'
	public static MRRecommendationCommentDelete = 'recommendation/delete-comment'
	public static MRRecommendationCommentGetOnPage = 'recommendation/get-all-comment'
	public static MRRecommendationCommentGetOnPageBase = 'recommendation/	get-all-comment-on-page'
	public static MRRecommendationCommentGetAllByParentId = 'recommendation/get-all-comment-by-parent'
	public static MRRecommendationCommentGetPageByParent = 'recommendation/get-comment-by-parent-on-page'
	public static MR_Recommendation7dayGraph = 'recommendation/recommendation7daygraph'

	// infomationChange
	public static MRInfomationExchangeGetOnPage = 'recommendation/get-all-infomation-exchange'
	public static MRInfomationExchangeInsert = 'recommendation/insert-infomation-exchange'

	public static PuRecommendationGetAllOnPage = 'pu-recommendation/get-list-recommentdation-on-page'
	public static PuRecommendationHomePage = 'pu-recommendation/get-list-recommentdation-home-page'
	public static RecommendationReceiveDeny = 'pu-recommendation/get-list-recommentdation-receive-deny'
	public static PuRecomentdationHightLight = 'pu-recommendation/recommendations-hight-light'
	public static PuRecomentdationProcessing = 'pu-recommendation/get-list-recommentdation-processing'
	public static PuNotificationGetDashboard = 'pu-recommendation/notification-getdashboard'
	public static MyRecommendationGetAllOnPage = 'pu-recommendation/get-list-my-recommentdation-on-page'
	public static PuRecommendationGetById = 'pu-recommendation/get-by-id'
	public static PuChangeSatisfaction = 'pu-recommendation/change-satisfaction'
	public static PuRecommendationCountClick = 'pu-recommendation/recommendation-count-click'
	public static PURecommendationStatisticsGetByUserId = 'pu-recommendation/recommendation-statistics-get-by-user-id'
	public static PURecommendationStatisticsProvince = 'pu-recommendation/recommendation-statistic-by-province'
	public static PURecommendationStatisticsSatisfaction = 'pu-recommendation/statistics-satisfaction-recommentdation'
	public static PUUnitdissatisfactionRate = 'pu-recommendation/unit-dissatisfaction-rate-on-page'
	public static PULateProcessingUnit = 'pu-recommendation/late-processing-unit-on-page'

	//Field
	public static FieldGetList = 'field/get-list-field-on-page'
	public static FieldGetDropDown = 'field/get-drop-down'
	public static FieldGetById = 'field/get-by-id'
	public static FieldInsert = 'field/insert'
	public static FieldUpdate = 'field/update'
	public static FieldUpdateStatus = 'field/update-status'
	public static FieldDelete = 'field/delete'
	public static RecommendationRequestGetDataForCreate = 'field/field-knct-get-dropdown'
	// thằng này ko tồn tại
	public static FieldExport = 'CACategorySPBase/CAFieldExportBase'

	//Hashtag
	public static HashtagGetAll = 'hashtag/get-all'
	public static HashtagGetList = 'hashtag/get-list-hashtag-on-page'
	public static HashtagGetById = 'hashtag/get-by-id'
	public static HashtagInsert = 'hashtag/insert'
	public static HashtagUpdate = 'hashtag/update'
	public static HashtagDelete = 'hashtag/delete'

	//Unit
	public static UnitGetPagedList = 'unit/get-list-unit-on-page'
	public static UnitGetAll = 'unit/get-all'
	public static UnitGetDataForCreate = 'unit/get-data-for-create'
	public static UnitGetById = 'unit/get-by-id'
	public static UnitInsert = 'unit/insert'
	public static UnitUpdate = 'unit/update'
	public static UnitDelete = 'unit/delete'
	public static UnitChangeStatus = 'unit/change-status'
	public static UnitCheckExists = 'unit/check-exists'
	public static UnitGetChildrenDropdown = 'unit/get-children-dropdown'
	public static UnitGetChildrenDropdownByField = 'unit/get-drop-unit-by-field'

	public static UnitPermissionSMSGetPageList = 'unit/get-list-unit-permission-on-page'
	public static UnitPermissionSMSDelete = 'unit/unit-permission-delete'
	public static UnitPermissionSMSInsert = 'unit/unit-permission-insert'
	public static UnitNotPermissionSMSGetDrop = 'unit/get-unit-dropdown-not-permission'
	public static UnitHasPermissionSMS = 'unit/unit-has-permission'
	public static UnitGetByGroup = 'unit/unit-get-by-group'
	public static UnitGetByParentId = 'unit/get-children-dropdown-by-parent'



	//Position
	public static PositionInsert = 'position/insert'
	public static PositionGetList = 'position/get-list-position-on-page'
	public static PositionDelete = 'position/delete'
	public static PositionGetById = 'position/get-by-id'
	public static PositionUpdate = 'position/update'

	// user
	public static UserGetPagedList = 'user/get-list-user-on-page-by-unit-id'
	public static UserGetAllOnPagedList = 'user/get-list-user-on-page'
	public static UsersGetDataForCreate = 'user/get-data-for-create'
	public static UserGetById = 'user/get-by-id'
	public static UserGetByRoleIdOnPage = 'user/get-list-user-on-page-by-role-id'
	public static UserGetByRoleId = 'user/get-list-user-on-page-base-by-role-id'
	public static UserInsert = 'user/create'
	public static UserUpdate = 'user/update'
	public static UserDelete = 'user/delete'
	public static UserGetByIdUpdate = 'user/get-info'
	public static UserChangeStatus = 'user/change-status'
	public static UserChangePwdInManage = 'user/user-change-password-in-manage'
	public static UserCheckExists = 'user/check-exists'
	public static SystemGetUserDropDown = 'user/get-drop-down'
	public static getIsSystemOrderByUnit = 'user/get-user-order-by-unit'
	public static UserGetIsSystem = 'user/get-user-is-system'
	public static UserGetIsNotRole = 'user/user-not-role'
	public static DeleteUserRole = 'user/user-role-map-delete'
	public static UserUpdateProfile = 'user/update-profile'
	public static UserUpdateQBId = 'user/update-qb'
	public static UserGetPagedListForChat = 'user/get-list-user-for-chat'
	public static UserGetAllByIdQb = 'user/get-list-user-by-lst-id-qb'

	//tai khoan ca nhan, doanh nghiep
	public static AccountGetInfo = 'user/user-get-info'
	public static AccountChangePassword = 'user/user-change-password'
	public static updateInfoIndividualCurrent = 'user/update-current-info-individual'
	public static updateInfoBusinessCurrent = 'user/update-current-info-business'

	// user system

	public static UserSystemGetAllOnPagedList = 'user/get-list-user-system-on-page'
	public static UserSystemUpdate = 'user/user-system-update'
	public static UserSystemCreate = 'user/user-system-create'

	public static SystemLogin = 'system-log/get-list-system-log-on-page'
	public static SystemLoginAdmin = 'system-log/get-list-system-log-admin-on-page'
	public static SystemLogDelete = 'system-log/delete'
	public static ExportExcel = 'system-log/export-excel'

	//SY_Role
	public static RoleGetAll = 'role/get-list-role-base'
	public static RoleGetAllOnPage = 'role/get-list-role-on-page'
	public static RoleInsert = 'role/insert'
	public static RoleUpdate = 'role/update'
	public static RoleDelete = 'role/delete'
	public static RoleGetById = 'role/get-by-id'
	public static RoleInsertPermission = 'role/permission-group-user-insert-by-list'
	public static RoleGetDataForCreate = 'role/get-data-for-create'
	public static InsertMultiUserRole = 'role/user-role-map-insert-list'

	//NewsType
	public static NewsTypeGetList = 'news-type/get-list-news-type-on-page'
	public static NewsTypeGetById = 'news-type/get-by-id'
	public static NewsTypeInsert = 'news-type/insert'
	public static NewsTypeUpdate = 'news-type/update'
	public static NewsTypeUpdateStatus = 'news-type/update'
	public static NewsTypeDelete = 'news-type/delete'
	public static NewsTypeGetDrop = 'news-type/get-drop-down'
	//DepartmentGroup
	public static DepartmentGroupGetList = 'department-group/get-list-department-group-on-page'
	public static DepartmentGroupGetById = 'department-group/get-by-id'
	public static DepartmentGroupInsert = 'department-group/insert'
	public static DepartmentGroupUpdate = 'department-group/update'
	public static DepartmentGroupUpdateStatus = 'department-group/update'
	public static DepartmentGroupDelete = 'department-group/delete'
	//Word
	public static WordGetList = 'word/get-list-word-on-page'
	public static WordGetListByGroupId = 'word/get-list-word-on-page-by-group-id'
	public static WordGetById = 'word/get-by-id'
	public static WordInsert = 'word/insert'
	public static WordUpdate = 'word/update'
	public static WordUpdateStatus = 'word/update'
	public static WordDelete = 'word/delete'
	public static WordGetListSuggest = 'word/get-list-suggest'
	//GroupWord
	public static GroupWordGetList = 'group-word/get-list-group-word-on-page'
	public static GroupWordGetById = 'group-word/get-by-id'
	public static GroupWordInsert = 'group-word/insert'
	public static GroupWordUpdate = 'group-word/update'
	public static GroupWordUpdateStatus = 'group-word/update'
	public static GroupWordDelete = 'group-word/delete'
	public static GroupWordGetListSuggest = 'group-word/get-list-suggest'

	//tin tuc
	public static NewsGetAllOnPage = 'news/get-list-news-on-page'
	public static NewsGetListHomePage = 'news/get-list-news-on-home-page'
	public static NewsGetById = 'news/get-by-id'
	public static NewsInsert = 'news/insert'
	public static NewsUpdate = 'news/update'
	public static NewsDelete = 'news/delete'
	public static NewsChangeStatus = 'news/change-status-news'
	public static NewsRelatesGetAll = 'news/get-list-relates'
	public static NewsRelatesGetAllForCreate = 'news/get-list-news-relates-forcreate-by-id'
	public static HisNewsInsert = 'news/insert-his'
	public static HisNewsGetListByNewsId = 'news/get-list-his-on-page'
	public static NewsGetViewDetail = 'news/get-detail'
	public static NewsGetViewDetailPublic = 'news/get-detail-public'
	public static NewsGetAllRelates = 'news/get-list-relates-by-id'

	//Department
	public static DepartmentGetList = 'department/get-list-department-on-page'
	public static DepartmentGetById = 'department/get-by-id'
	public static DepartmentInsert = 'department/insert'
	public static DepartmentUpdate = 'department/update'
	public static DepartmentUpdateStatus = 'department/update'
	public static DepartmentDelete = 'department/delete'

	//remind
	public static RemindInsert = 'remind/insert'
	public static RemindGetList = 'remind/get-remind-list'

	//invitation

	public static InnvitationInsert = 'invitation/insert'
	public static InnvitationUpdate = 'invitation/update'
	public static InvitationDelete = 'invitation/delete'
	public static InvitationGetList = 'invitation/get-list-invitation-on-page'
	public static InnvitationDetail = 'invitation/get-detail'
	public static UserReadedInvitationGetList = 'invitation/get-list-user-readed-invitation-on-page'
	public static InnvitationGetListHisOnPage = 'invitation/get-list-his'
	public static InnvitationGetDataForCreate = 'invitation/get-data-for-create'

	//sms
	public static SMSManagementGetOnPage = 'sms/get-list-sms-on-page'
	public static SMSManagementGetHisOnPage = 'sms/list-his'
	public static SMSManagementInsert = 'sms/insert'
	public static SMSManagementUpdate = 'sms/update'
	public static SMSManagementUpdateStatusSend = 'sms/update-status'
	public static SMSManagementDelete = 'sms/delete'
	public static SMSManagementGetById = 'sms/get-by-id'
	public static HISSMSInsert = 'sms/insert-his'

	//dự thảo email
	public static EmailManagementUpdate = 'email-management/update'
	public static EmailManagementGetById = 'email-management/get-by-id'
	public static EmailManagementGetPagedList = 'email-management/get-list-email-on-page'
	public static EmailManagementDelete = 'email-management/delete'
	public static EmailManagementSendEmail = 'email-management/send-email'
	public static EmailManagementHisPagedList = 'email-management/list-his'

	// SYIntroduce

	public static SYIntroduceGetInfo = 'introduce/get-by-id'
	public static SYIntroduceUnitGetInfo = 'introduce/introduce-unit-get-by-id'
	public static SYIntroduceUpdate = 'introduce/update'
	public static SYIntroduceUnitGetOnPage = 'introduce/get-list-introduce-unit-on-page'
	public static SYIntroduceUnitInsert = 'introduce/introduce-unit-insert'
	public static SYIntroduceUnitUpdate = 'introduce/introduce-unit-update'
	public static SYIntroduceUnitDetete = 'introduce/introduce-unit-delete'

	// support

	public static SYSupportGetAllByCategory = 'support/get-by-category'
	public static SYSupportInsert = 'support/insert'
	public static SYSupportUpdate = 'support/update'
	public static SYSupportDelete = 'support/delete'
	public static PU_Support = 'support/support-public'
	public static SYGalleryInsert = 'support/gallery-insert'
	public static SYGalleryGetList = 'support/gallery-get-all'
	public static SYGalleryDelete = 'support/gallery-delete'

	// SYIndexSetting

	public static SYIndexSettingGetInfo = 'index-setting/get-info'
	public static SYIndexSettingUpdate = 'index-setting/update'
	public static SYIndexWebsiteInsert = 'index-setting/insert'
	public static SYIndexWebsiteGetAll = 'index-setting/get-list-index-website'

	//api dia danh
	public static ProvinceGetAll = 'administrative/get-list-province'
	public static DistrictGetAll = 'administrative/get-list-district'
	public static VillageGetAll = 'administrative/get-list-village'
	public static GetAllByProvinceId = 'administrative/get-list-province-by-province-id'
	public static AdministrativeUnits = 'administrative/get-drop-down'

	//PublishNotification
	public static PublishNotificationGetList = 'notification/publish-get-all'
	public static PublishNotificationGetById = 'notification/publish-get-by-id'
	public static PublishNotificationInsert = 'notification/publish-insert'
	public static PublishNotificationUpdate = 'notification/publish-update'
	public static PublishNotificationDelete = 'notification/publish-delete'

	//register
	public static RegisterIndividual = 'user/individual-register'
	public static RegisterOrganization = 'user/organization-register'

	//Administrative Formalities
	public static AdministrativeFormalitiesGetDropdown = 'administration-formalities/get-drop-down'
	public static AdministrativeFormalitiesGetList = 'administration-formalities/get-list-administration-formalities-on-page'
	public static AdministrativeFormalitiesGetListForward = 'administration-formalities/get-list-administration-formalities-forward-on-page'
	public static AdministrativeFormalitiesGetListHomePage = 'administration-formalities/get-list-administration-formalities-home-page'
	public static AdministrativeFormalitiesGetById = 'administration-formalities/get-by-id'
	public static AdministrativeFormalitiesGetByIdView = 'Administradministration-formalitiesativeFormalities/RecommendationGetByIdView'
	public static AdministrativeFormalitiesInsert = 'administration-formalities/insert'
	public static AdministrativeFormalitiesUpdate = 'administration-formalities/update'
	public static AdministrativeFormalitiesDelete = 'administration-formalities/delete'
	public static AdministrativeFormalitiesUpdateShow = 'administration-formalities/update-status'
	public static AdministrativeFormalitiesCAFieldDAM = 'administration-formalities/get-drop-down'
	public static AdministrativeFormalitiesCAUnitDAM = 'administration-formalities/get-drop-down-unit'
	public static AdministrativeForward = 'administration-formalities/administration-forward'

	// Notification
	public static NotificationInsertTypeNews = 'notification/insert-type-new'
	public static NotificationInsertTypeRecommendation = 'notification/insert-type-recommendation'
	public static NotificationGetList = 'notification/get-list-notification-on-page'
	public static NotificationGetById = 'notification/get-by-id'
	public static NotificationDelete = 'notification/delete'
	public static NotificationUpdateIsViewed = 'notification/update-is-viewed'
	public static NotificationUpdateIsReaded = 'notification/update-is-readed'

	//Chatbot
	public static ChatbotGetList = 'chat-bot/get-list-chat-bot-on-page'
	public static ChatbotDelete = 'chat-bot/delete'
	public static ChatbotGetById = 'chat-bot/get-by-id'
	public static ChatbotLibGetById = 'chat-bot/answer-get-by-libid'
	public static ChatbotUpdate = 'chat-bot/update'
	public static ChatbotInsertQuestion = 'chat-bot/insert-question'
	public static ChatbotInsertData = 'chat-bot/insert-data'
	public static ChatbotGetListHistory = 'chat-bot/list-his'
	public static ChatbotGetAllActive = 'chat-bot/get-all-active'

	//systemConfig
	public static SYConfigSystemGetAllOnPage = 'system-config/get-list-system-config-on-page'
	public static SYConfigSystemGetById = 'system-config/get-by-id'
	public static SYConfigSystemGetByType = 'system-config/get-by-type'
	public static SYConfigSystemUpdate = 'system-config/update'
	public static TimeConfigInsert = 'system-config/sys-time-insert'
	public static TimeConfigUpdate = 'system-config/sys-time-update'
	public static TimeConfigGetAllOnPage = 'system-config/get-list-sys-time-on-page'
	public static TimeConfigDelete = 'system-config/sys-time-delete'
	public static TimeConfigGetById = 'system-config/sys-time-get-by-id'
	public static TimeConfigGetDateActive = 'system-config/sys-time-data-active'

	//Individual
	public static IndividualGetAllOnPage = 'individual/get-list-individual-on-page'
	public static IndivialChangeStatus = 'individual/change-status'
	public static IndivialDelete = 'individual/delete'
	public static InvididualRegister = 'individual/insert'
	public static InvididualGetByID = 'individual/get-by-id'
	public static InvididualUpdate = 'individual/update'
	public static ImportDataInvididual = 'individual/import-data-individual?folder=BusinessIndividual'
	// public static InvididualCheckExists = 'Individual/IndividualCheckExistsBase'
	public static IndividualCheckExists = 'individual/check-exists'

	//Business
	public static BusinessGetAllOnPage = 'business/get-list-business-on-page'
	public static BusinessChangeStatus = 'business/change-status'
	public static BusinessDelete = 'business/delete'
	public static BusinessRegister = 'business/insert'
	public static BusinessGetByID = 'business/get-by-id'
	public static BusinessUpdate = 'business/update'
	public static ImportDataBusiness = 'business/import-data-business?folder=BusinessIndividual'
	public static GetListIndividualAndBusinessByAdmintrativeUnitId = 'business/get-list-individual-business-by-provice-id'
	public static OrganizationCheckExists = 'business/check-exists'
	public static BusinessImportFile = 'business/import-data-business?folder=BusinessIndividual'
	public static GetListIndividualBusinessDrop = 'business/get-drop-list-individual-business'

	//Statistic admin

	public static StatisticRecommendationByUnit = 'statistic/recommendation-by-unit'
	public static StatisticRecommendationByUnitDetail = 'statistic/recommendation-by-unit-detail'
	public static StatisticRecommendationByField = 'statistic/recommendation-by-field'
	public static StatisticRecommendationByFieldDetail = 'statistic/recommendation-by-field-detail'
	public static StatisticRecommendationByGroupWord = 'statistic/recommendation-by-group-word'
	public static StatisticRecommendationByGroupWordDetail = 'statistic/recommendation-by-group-word-detail'
	public static StatisticRecommendationProcessingStatus = 'statistic/recommendation-processing-status'
	public static StatisticRecommendationProcessingResults = 'statistic/recommendation-processing-results'
	public static StatisticRecommendationProcessingResultsByReceptionType = 'statistic/recommendation-processing-results-by-reception-type'
	public static StatisticRecommendationProcessingResultsByType = 'statistic/recommendation-processing-results-by-type'
	public static StatisticRecommendationByTypeDetail = 'statistic/recommendation-by-type-detail-on-page'
	public static StatisticRecommendationByRecptionTypeDetail = 'statistic/recommendation-by-reception-type-detail-on-page'
	public static StatisticRecommendationResultDetail = 'statistic/recommendation-by-result-detail-on-page'

	public static StatisticRecommendationProcessingResultsByUnit = 'statistic/recommendation-processing-results-by-unit'
	public static StatisticRecommendationProcessingResultsByUnitAndReception = 'statistic/recommendation-processing-results-by-unit-and-reception'
	public static StatisticRecommendationForMenu = 'statistic/recommendation-for-menu'
	//statstic public
	public static StatisticsByUnitParentId = 'statistic/recommendation-statistic-by-unit-parent'
	public static StatisticsForChart = 'statistic/recommendation-statistic-for-chart'

	//call history
	public static CallHistoryGetPagedList = 'call-history/get-list-call-history-on-page'
	public static CallHisDelete = 'call-history/delete'

	// đồng bộ
	public static MrSyncCongThongTinDienTuTinhPagedList = 'sync-data/get-list-cong-thong-tin-dien-tu-tinh-on-page'
	public static AsyncCongThongTinDienTu = 'sync-data/sync-gop-y-kien-nghi'
	public static MrSyncCongThongTinDienTuTinhGetById = 'sync-data/cong-thong-tin-dien-tu-tinh-get-by-id'

	public static MrSyncCongThongTinDichVuHCCPagedList = 'sync-data/get-list-cong-thong-tin-dv-hcc-on-page'
	public static AsyncCongThongTinDichVuHCC = 'sync-data/sync-hop-thu-gop-y-khanh-hoa'
	public static MrSyncCongThongTinDichVuHCCGetById = 'sync-data/cong-thong-tin-dv-hcc-get-by-id'

	public static AsyncRecommendationKNCT = 'sync-data/sync-quan-ly-kien-nghi-cu-tri'
	public static RecommendationRequestGetByIdView = 'sync-data/cu-tri-khanh-hoa-get-by-id'

	public static MrSyncHeThongPANKChinhPhuPagedList = 'sync-data/get-list-pakn-chinh-phu-on-page'
	public static MrSyncHeThongPANKChinhPhuGetByObjectId = 'sync-data/pakn-chinh-phu-get-by-id'
	public static AsyncMrSyncHeThongPANKChinhPhu = 'sync-data/sync-cong-dich-vu-cong-quoc-gia'

	public static MrSyncHeThongQuanLyKienNghiCuTriPagedList = 'sync-data/get-list-quan-ly-kien-nghi-cu-tri-on-page'
	public static PU_RecommandationSyncPagedList = 'sync-data/PUGetPagedList'
	public static PU_RecommandationSyncGetDetail = 'sync-data/PUGetGetById'

	// Weather
	public static Weather = 'weather/get'
	public static WeatherByQ = 'weather/get-by-q'

	//chatbot
	public static BotRooms = 'bot/rooms'
	public static GetMessages = 'bot/get-message'
	public static CreateRoom = 'bot/bot-create-room'
}
