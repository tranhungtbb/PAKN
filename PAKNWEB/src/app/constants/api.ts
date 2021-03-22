export class Api {
  //DashBoard
  public static GET_DASH_BOARD = "DashBoard/GetDashBoard";
  public static GetListHocVien = "DashBoard/GetListHocVien";
  public static GET_NOTIFICATIONS = "DashBoard/GetNotifications";
  public static UpdateIsViewRemind = "DashBoard/UpdateIsViewRemind";
  public static DashBoardThongKeKienNghi = "DashBoard/DashBoardThongKeKienNghi";
  public static DashBoardGetList = "DashBoard/DashBoardGetList";
  public static DashBoardGetYear = "DashBoard/DashBoardGetYear";
  public static DashBoardCount = "DashBoard/DashBoardCount";
  //Account
  public static LOGIN = 'Contact/Login';
  public static REGISTER = 'Contact/Register';
  public static FORGETPASSWORD = 'Contact/Forgetpassword';
  public static CHANGEPASSWORD = 'Contact/ChangePassword';
  public static RESTORE_ACCOUNT = 'Contact/UnlockAccount';

  // Captcha Controller
  public static ValidateCaptcha = "Captcha/ValidatorCaptcha";
  public static getImageCaptcha = "Captcha/GetCaptchaImage";

  //Department
  public static GET_DEPARTMENT_TREE = 'Department/Index';
  public static CHECK_EXISTED_CODE = 'Department/CheckExistedCode';
  public static CREATEDERPARTMENT = 'Department/Create';
  public static UPDATEDERPARTMENT = 'Department/Update';
  public static RemoveUserFromDepartment = 'Department/RemoveUserFromDepartment';
  public static DELETEDEPARTMENT = 'Department/Delete';
  public static DEPARTMENTGETBYID = 'Department/GetbyId';
  public static GETDEPARTMENTABYUNITID = 'Department/GetDepartmentaByUnitId';
  public static GetDepartmentByUnit = 'Department/GetDepartmentByUnit';
  public static GetDepartmentForProcess = 'Department/GetDepartmentForProcess';
  public static GETUSERBYUNITID = 'Department/GetListUser';
  public static GETUSERTOADD = 'Department/GetUserToAdd';
  public static GET_UNIT_TREE = 'Department/GetUnitTree';
  public static GET_TREE_ADD_USER = 'Department/GetTreeAddUser';
  public static ADDDEPARTMENTUSER = 'Department/AddDepartmentUser';
  public static DELETEDEPARTMENTUSER = 'Department/DeleteDepartmentUser';
  public static GETUSERBYDEPID = 'Department/GetUserByDepId';
  public static UPDATEREPRESENTATIVE = 'Department/UpdateRepresentative';
  public static GETDROPDOWNDEPARTMENT = 'Department/GetDropDown';
  public static GET_UNIT_NAME = 'Department/Get_Unit_Name';
  public static GetHeadDepartment = 'Department/GetHeadDepartment';
  public static GetListUserOverDepartmentId = 'Department/GetListUserOverDepartmentId';
  public static DepartmentAddUser = 'Department/DepartmentAddUser';
  public static GetDepartments = 'Department/GetDepartments';

  //GroupUser
  public static GET_GROUPUSER_LIST = 'GroupUser/Index';
  public static CHECK_GROUPUSER_EXISTED_CODE = 'GroupUser/CheckExistedCode';
  public static CREATEGROUPUSER = 'GroupUser/Create';
  public static UPDATEGROUPUSER = 'GroupUser/Update';
  public static DELETEGROUPUSER = 'GroupUser/Delete';
  public static GROUPUSERBYID = 'GroupUser/GetbyId';
  public static ADDUSERTOGROUP = 'GroupUser/AddUserToGroup';
  public static ADDLISTUSERTOGROUP = 'GroupUser/AddListUserToGroup';
  public static GETCREATEGROUPUSERDATAS = 'GroupUser/GetCreateGroupUserDatas';
  public static GETLISTUSERBYUNITIDANDFILTER = 'GroupUser/GetListUserByUnitIdAndFilter';
  public static DELUSERFROMGROUPUSER = "GroupUser/DeleteUserFromGroupUser";
  public static GETLISTGROUPUSERBYUNITIDANDPAGE = "GroupUser/GetListGroupUserByUnitIdAndPage";

  //User
  public static GET_USER_BY_DEPARTMENT = 'User/GetUserByDepartment';
  public static CREATE_USER = 'User/CreateUser';
  public static GET_CREATE_USER_DATAS = 'User/GetCreateUserDatas';
  public static GETUSERBYGROUPID = 'User/GetListUserByGroupId';
  public static Edit_User = 'User/EditUser';
  public static UPDATE_INFO = 'User/UpdateInfo';
  public static UPDATE_USER_LOGIN = 'User/UpdateUserLogin';
  public static LOCK_USER = 'User/LockUser';
  public static DELETE_USER = 'User/DeleteUser';
  public static logOut = 'User/LogOut';
  public static GET_USER_BY_REGION = 'User/GetUserByRegion';
  public static GET_USER_BY_UNIT = 'User/GetUserByUnit';
  public static GetAllUserByUnitForForward = 'User/GetAllUserByUnitForForward';
  public static GET_USER_BY_UNIT_AND_QUERY = 'User/GetUserByUnitAndQuery';
  public static GetUserById = 'User/GetUserById';
  public static IsNguoiDaiDien = 'User/IsNguoiDaiDien';
  public static UserUpdateStatus = 'User/UpdateStatus';
  public static GetListChucVuAndListPhongBan = 'User/GetListChucVuAndListPhongBan';

  //lay danh sach co phan trang
  public static GET_USER_BY_REGION_UNIT = 'User/GetUserByRegionOrUnit';

  //Catalog
  public static GETCATALOGBYUNITID = 'SystemConfig/GetCatalogValueByDepartment';
  public static GET_CATALOG_VALUE_BY_ID = 'SystemConfig/GetCatalogValueById';
  public static DELETE_CATALOG_VALUE = 'SystemConfig/DeleteCatalogValue';
  public static UPDATE_CATALOG_VALUE = 'SystemConfig/UpdateCatalogValue';
  public static CREATE_CATALOG_VALUE = 'SystemConfig/CreateCatalogValue';
  public static GET_DATA_CREATE_DOCUMENT = 'SystemConfig/GetDataCreateDocument';
  public static GET_DATA_UPDATE_DOCUMENT = 'SystemConfig/GetDataUpdateDocument';
  public static getFileSupport = 'SystemConfig/GetFileSupport';
  public static CREATE_FORM_CATALOG = 'SystemConfig/CreateFormCatalog';
  public static UPDATE_FORM_CATALOG = 'SystemConfig/UpdateFormCatalog';
  public static DELETE_FORM_CATALOG = 'SystemConfig/DeleteFormCatalog';
  public static GET_DORMITORY_TREE = 'SystemConfig/GetDormitoryTree';
  public static GET_DDL_PROFESSION = 'SystemConfig/GetDropdownProfession';
  public static GET_DDL_CATALOG_VALUE = 'SystemConfig/GetDropdownCatalogValue';
  public static GetUserForCreateByDepartment = 'SystemConfig/GetUserForCreateByDepartment';
  public static GetNhomNguoiDungNguoiDung = 'SystemConfig/GetNhomNguoiDungNguoiDung';
  public static CreateNguoiDungNhomNguoiDung = 'SystemConfig/CreateNguoiDungNhomNguoiDung';
  public static DeleteNguoiDungNhomNguoiDung = 'SystemConfig/DeleteNguoiDungNhomNguoiDung';
  public static GetCodeConfigByCode = 'SystemConfig/GetCodeConfigByCode';

  //System Config
  public static GET_SYSTEM_CONFIG = 'SystemConfig/GetSystemConfigByDepartment';
  public static GET_SYSTEM_CONFIG_BY_ID = 'SystemConfig/GetBySystemConfigId';
  public static UPDATE_SYSTEM_CONFIG = 'SystemConfig/UpdateSystemConfig';
  public static GETDOCUMENTTYPEUNITID = 'SystemConfig/GetDocumentTypeByUnitId';
  public static EmailTemplateGetList = 'SystemConfig/EmailTemplateGetList';
  public static EmailTemplateUpdateTrangThai = 'SystemConfig/EmailTemplateUpdateTrangThai';
  public static EmailTemplateDelete = 'SystemConfig/EmailTemplateDelete';
  public static EmailTemplateGetById = 'SystemConfig/EmailTemplateGetById';
  public static EmailTemplateCreate = 'SystemConfig/EmailTemplateCreate';
  public static EmailTemplateGetListEmailConfiguration = 'SystemConfig/EmailTemplateGetListByEmailConfiguration';
  public static CauHinhEmailDropdown = 'SystemConfig/CauHinhEmailDropdown';

  //Systemlog
  public static SYSTEMLOGGETDATAGRID = 'SystemLog/Index';
  public static SYSTEMLOGDELETEDATAGRID = 'SystemLog/Delete';
  public static SYSTEMLOGGETTIMELINE = 'SystemLog/GetTimeLine';
  public static SYSTEMLOGGETUSERTIMELINESEARCH = 'SystemLog/UserTimeLineSearch';

  public static getListSuggestUsersNew = 'Workflow/GetListSuggestUsersNew';

  // FileUpload/Download
  public static DownloadApp = "UploadFiles/DownloadApp";
  public static GetEncryptedPath = "UploadFiles/GetEncryptedPath";
  public static download = 'UploadFiles/DownloadFile';
  public static DownloadFilebyId = 'UploadFiles/DownloadFilebyId';
  public static downloadFileSupport = 'UploadFiles/DownloadFileSupport';
  public static GetFile = 'UploadFiles/GetFile';
  public static uploadfiles = 'UploadFiles/Uploadfiles';
  public static getFileImage = 'UploadFiles/GetFile';
  public static FileGetFile = "File/Download";
  public static DownloadFileNoAuthor = "UploadFiles/DownloadFileNoAuthor";
  public static deleteFileAttInspection = 'UploadFiles/DeleteFileAttInspection';

  //History
  public static HistoryGetTimeLine = 'History/TimeLine';
  public static DepartmentGroupGetList = 'DepartmentGroup/GetList';
  public static DepartmentGroupDelete = 'DepartmentGroup/Delete';
  public static MajorGetList = "Catalog/MajorGetList";
  public static StageGetList = "Catalog/StageGetList";
  public static RecommendationsGetList = "Catalog/RecommendationsGetList";
  public static RecommendationsTypeGetList = "Catalog/RecommendationsTypeGetList";
  public static ResolutionTypeGetList = "Catalog/ResolutionTypeGetList";
  public static RecomendationFieldTreeGetList = "Catalog/RecomendationFieldTreeGetList";
  public static GetlistcapChaId = "Catalog/GetlistcapChaId";

  public static RecommendationsFieldGetList = "Catalog/RecommendationsFieldGetList";
  public static ComplaintLetterGetList = "Catalog/ComplaintLetterGetList";
  public static ComplaintLetterUpdateStatus = "Catalog/ComplaintLetterUpdateStatus";
  public static PositionGetList = "Catalog/PositionGetList";
  public static PositionGroupGetList = "Catalog/PositionGroupGetList";
  public static NationGetList = "Catalog/NationGetList";
  public static NationGetById = "Catalog/NationGetById";
  public static UpdateStatus = "Catalog/UpdateStatus";
  public static DeleteCatalog = "Catalog/DeleteCatalog";
  public static KhoaHdndGetList = "Catalog/KhoaHdndGetList";
  public static CatalogCreate = "Catalog/Create";
  public static CatalogUpdate = "Catalog/Update";
  public static CatalogGetById = "Catalog/GetById";
  public static UnitGetList = "Catalog/UnitGetList";
  public static ExportCatalog = "Catalog/ExportCatalog";
  public static GetDoiTuongByNoiDung = "Catalog/GetDoiTuongByNoiDung";

  // Download File
  public static DownloadFile = "File/Download";

  //Histories
  public static GetHistories = "Histories/GetHistories";

  //Region
  public static GETREGIONFIRSTNODE = 'Region/FirstNode';
  public static GETREGIONCHILDNODE = 'Region/ChildNode';

  public static REGIONCREATE = 'Region/Create';
  public static REGIONUPDATE = 'Region/Update';
  public static REGIONDELETE = 'Region/Delete';
  public static REGIONGETBYID = 'Region/GetbyId';
  public static REGIONGETDropDown = 'Region/GetDropDown';
  public static RegionGetDropDownQuanHuyen = 'Region/GetDropQuanHuyen';
  public static RegionGetDropDownPhuongXa = 'Region/GetDropDownPhuongXa';
  public static RegionGetDropDownQuanHuyen_Group = 'Region/GetDropDownQuanHuyen_Group';
  public static RegionGetDropDownQuanHuyen_All = 'Region/GetDropDownQuanHuyenAll';
  public static RegionGetDropDownTinh_Search = 'Region/GetDropDownTinhSearch';

}
