using System;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using PAKNAPI.Common;
using PAKNAPI.Models.Results;

namespace PAKNAPI.ModelBase
{
	public class SYAPIInsert
	{
		private SQLCon _sQLCon;

		public SYAPIInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYAPIInsert()
		{
		}

		public async Task<int> SYAPIInsertDAO(SYAPIInsertIN _sYAPIInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYAPIInsertIN.Name);
			DP.Add("Authorize", _sYAPIInsertIN.Authorize);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_APIInsert", DP));
		}
	}


	public class SYAPIInsertIN
	{
		public string Name { get; set; }
		public bool? Authorize { get; set; }
	}

	public class SYUnitChageStatus
	{
		private SQLCon _sQLCon;

		public SYUnitChageStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitChageStatus()
		{
		}

		public async Task<int> SYUnitChageStatusDAO(SYUnitChageStatusIN _sYUnitChageStatusIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYUnitChageStatusIN.Id);
			DP.Add("IsActived", _sYUnitChageStatusIN.IsActived);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_UnitChageStatus", DP));
		}
	}

	public class SYUnitChageStatusIN
	{
		public long? Id { get; set; }
		public bool? IsActived { get; set; }
	}
	public class SYEmailGetFirst
	{
		private SQLCon _sQLCon;

		public SYEmailGetFirst(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYEmailGetFirst()
		{
		}

		public int Id { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string Port { get; set; }
		public string Server { get; set; }

		public async Task<List<SYEmailGetFirst>> SYEmailGetFirstDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYEmailGetFirst>("SY_EmailGetFirst", DP)).ToList();
		}
	}

	public class SYSupportMenu
	{
		private SQLCon _sQLCon;

		public SYSupportMenu(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSupportMenu()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public int Level { get; set; }
		public int ParentId { get; set; }
		public int Category { get; set; }
		public int Type { get; set; }

		public string Content { get; set; }
		public string? FilePath { get; set; }
		public int? FileType { get; set; }

		public string FileName { get; set; }
		public async Task<List<SYSupportMenu>> SYSupportMenuGetByCategoryDAO(int? Category)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Category", Category);

			return (await _sQLCon.ExecuteListDapperAsync<SYSupportMenu>("[SY_SupportMenuGetAllByCategory]", DP)).ToList();
		}

		public async Task<int?> SYSupportMenuUpdateDAO(SYSupportMenu model)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", model.Id);
			DP.Add("Title", model.Title);
			DP.Add("Level", model.Level);
			DP.Add("ParentId", model.ParentId);
			DP.Add("Category", model.Category);
			DP.Add("Type", model.Type);
			DP.Add("Content", model.Content);
			DP.Add("FilePath", model.FilePath);
			DP.Add("FileType", model.FileType);
			DP.Add("FileName", model.FileName);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("[SY_SupportMenuUpdate]", DP);
		}
		public async Task<int?> SYSupportMenuInsertDAO(SYSupportMenu model)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Title", model.Title);
			DP.Add("Level", model.Level);
			DP.Add("ParentId", model.ParentId);
			DP.Add("Category", model.Category);
			DP.Add("Type", model.Type);
			DP.Add("Content", model.Content);
			DP.Add("FilePath", model.FilePath);
			DP.Add("FileType", model.FileType);
			DP.Add("FileName", model.FileName);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("[SY_SupportMenuInsert]", DP);
		}

		public async Task<int?> SYSupportMenuDeleteDAO(SYSupportMenuDelete model)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", model.Id);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("[SY_SupportMenuDelete]", DP);
		}


	}

	public class SYSupportMenuDelete {
		public int Id { get; set; }
	}


	public class SYIntroduce
	{
		private SQLCon _sQLCon;

		public SYIntroduce(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIntroduce()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public string DescriptionUnit { get; set; }
		public string DescriptionFunction { get; set; }
		public string BannerUrl { get; set; }

		public DateTime? UpdateDate { get; set; }

		public async Task<List<SYIntroduce>> SYIntroduceGetInfoDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYIntroduce>("[SY_IntroduceGetInfo]", DP)).ToList();
		}

		public async Task<int?> SYIntroduceUpdateDAO(SYIntroduce syIntroduce)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", syIntroduce.Id);
			DP.Add("Title", syIntroduce.Title);
			DP.Add("Summary", syIntroduce.Summary);
			DP.Add("DescriptionUnit", syIntroduce.DescriptionUnit);
			DP.Add("DescriptionFunction", syIntroduce.DescriptionFunction);
			DP.Add("BannerUrl", syIntroduce.BannerUrl);
			DP.Add("UpdateDate", syIntroduce.UpdateDate);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IntroduceUpdate", DP);
		}


	}

	public class SYConfig
	{
		private SQLCon _sQLCon;

		public SYConfig(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYConfig()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Content { get; set; }
		public int? Type { get; set; }

		public int RowNumber { get; set; }

		public async Task<List<SYConfig>> SYConfigGetByIdDAO(int? IntroduceId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", IntroduceId);

			return (await _sQLCon.ExecuteListDapperAsync<SYConfig>("SY_ConfigGetById", DP)).ToList();
		}

		public async Task<List<SYConfig>> SYConfigGetByTypeDAO(int? type)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Type", type);

			return (await _sQLCon.ExecuteListDapperAsync<SYConfig>("SY_ConfigGetByType", DP)).ToList();
		}

		public async Task<List<SYConfig>> SYConfigGetAllOnPageDAO(string Title,string Description, int? Type, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Title", Title);
			DP.Add("Description", Description);
			DP.Add("Type", Type);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<SYConfig>("[SY_ConfigGetAllOnPage]", DP)).ToList();
		}


		public async Task<int?> SYConfigUpdateDAO(SYConfig syConfig)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", syConfig.Id);
			DP.Add("Title", syConfig.Title);
			DP.Add("Description", syConfig.Description);
			DP.Add("Type", syConfig.Type);
			DP.Add("Content", syConfig.Content);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_ConfigUpdate", DP);
		}
	}


	public class SYIntroduceUnit
	{
		private SQLCon _sQLCon;

		public SYIntroduceUnit(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIntroduceUnit()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string Infomation { get; set; }
		public int? Index { get; set; }

		public int RowNumber { get; set; }

		public int IntroduceId { get; set; }

		public async Task<List<SYIntroduceUnit>> SYIntroduceUnitGetByIntroduceId(int? IntroduceId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("IntroduceId", IntroduceId);

			return (await _sQLCon.ExecuteListDapperAsync<SYIntroduceUnit>("[SY_IntroduceUnitGetByIntroduceId]", DP)).ToList();
		}

		public async Task<List<SYIntroduceUnit>> SYIntroduceUnitGetOnPageByIntroduceId(int? IntroduceId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("IntroduceId", IntroduceId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<SYIntroduceUnit>("[SY_IntroduceUnitGetOnPageByIntroduceId]", DP)).ToList();
		}

		public async Task<int?> SYIntroduceUnitGetById(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);
			return await _sQLCon.ExecuteScalarDapperAsync<int?>("[SY_IntroduceUnitGetById]", DP);
		}

		public async Task<int?> SYIntroduceUnitInsertDAO(SYIntroduceUnit syIntroduceUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Title", syIntroduceUnit.Title);
			DP.Add("Description", syIntroduceUnit.Description);
			DP.Add("Infomation", syIntroduceUnit.Infomation);
			DP.Add("Index", syIntroduceUnit.Index);
			DP.Add("IntroduceId", syIntroduceUnit.IntroduceId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IntroduceUnitInsert", DP);
		}

		public async Task<int?> SYIntroduceUnitUpdateDAO(SYIntroduceUnit syIntroduceUnit)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", syIntroduceUnit.Id);
			DP.Add("Title", syIntroduceUnit.Title);
			DP.Add("Description", syIntroduceUnit.Description);
			DP.Add("Infomation", syIntroduceUnit.Infomation);
			DP.Add("Index", syIntroduceUnit.Index);
			DP.Add("IntroduceId", syIntroduceUnit.IntroduceId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IntroduceUnitUpdate", DP);
		}

		public async Task<int> SYIntroduceUnitDeleteDAO(SYIntroduceUnitDelete syIntroduceUnitDelete)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", syIntroduceUnitDelete.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_IntroduceUnitDelete", DP));
		}

	}

	public class SYIntroduceUnitDelete {
		public int Id { get; set; }
	}


	public class SYIntroduceFunction
	{
		private SQLCon _sQLCon;

		public SYIntroduceFunction(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIntroduceFunction()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Icon { get; set; }
		public string IconNew { get; set; }
		public string IntroduceId { get; set; }

		public async Task<List<SYIntroduceFunction>> SYIntroduceFunctionGetByIntroductId(int? syIntroduceId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("IntroduceId", syIntroduceId);
			

			return (await _sQLCon.ExecuteListDapperAsync<SYIntroduceFunction>("SY_IntroduceFunctionGetByIntroduceId", DP)).ToList();
		}

		public async Task<int?> SYIntroduceFunctionUpdateDAO(SYIntroduceFunction syIntroduceF)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", syIntroduceF.Id);
			DP.Add("Title", syIntroduceF.Title);
			DP.Add("Content", syIntroduceF.Content);
			DP.Add("Icon", syIntroduceF.Icon);
			DP.Add("IntroduceId", syIntroduceF.IntroduceId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IntroduceFunctionUpdate", DP);
		}


	}

	public class SYIndexSetting
	{
		private SQLCon _sQLCon;

		public SYIndexSetting(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIndexSetting()
		{
		}

		public int Id { get; set; }
		public string BannerUrl { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }

		public string License { get; set; }

		public async Task<List<SYIndexSetting>> SYIndexSettingGetInfoDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYIndexSetting>("[SY_IndexSettingGetInfo]", DP)).ToList();
		}

		public async Task<int?> SYIndexSettingUpdateDAO(SYIndexSetting sy)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", sy.Id);
			DP.Add("BannerUrl", sy.BannerUrl);
			DP.Add("Phone", sy.Phone);
			DP.Add("Email", sy.Email);
			DP.Add("Address", sy.Address);
			DP.Add("Description", sy.Description);
			DP.Add("License", sy.License);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IndexSettingUpdate", DP);
		}


	}



	public class SYIndexSettingBanner
	{
		private SQLCon _sQLCon;

		public SYIndexSettingBanner(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIndexSettingBanner()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string FileAttach { get; set; }
		public int FileType { get; set; }
		public int IndexSystemId { get; set; }

		public async Task<List<SYIndexSettingBanner>> SYIndexSettingBannerGetByIndexSettingId(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("IndexSettingId", Id);
			return (await _sQLCon.ExecuteListDapperAsync<SYIndexSettingBanner>("[SY_IndexBannerGetByIndexSettingId]", DP)).ToList();
		}

		public async Task<int?> SYIndexBannerInsertDAO(SYIndexSettingBanner sy)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", sy.Name);
			DP.Add("FileAttach", sy.FileAttach);
			DP.Add("FileType", sy.FileType);
			DP.Add("IndexSystemId", sy.IndexSystemId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IndexBannerInsert", DP);
		}

		

		public async Task<int> SYIndexBannerDeleteDAO(SYIntroduceUnitDelete sy)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", sy.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_IndexBannerDelete", DP));
		}

	}


	public class SYIndexWebsite
	{
		private SQLCon _sQLCon;

		public SYIndexWebsite(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIndexWebsite()
		{
		}

		public int Id { get; set; }
		public string NameWebsite { get; set; }
		public string UrlWebsite { get; set; }
		public int IndexSystemId { get; set; }

		public async Task<List<SYIndexWebsite>> SY_IndexWebsiteGetByIndexSettingId(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("IndexSettingId", Id);
			return (await _sQLCon.ExecuteListDapperAsync<SYIndexWebsite>("SY_IndexWebsiteGetByIndexSettingId", DP)).ToList();
		}

		public async Task<int?> SY_IndexWebsiteInsertDAO(SYIndexWebsite sy)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("NameWebsite", sy.NameWebsite);
			DP.Add("UrlWebsite", sy.UrlWebsite);
			DP.Add("IndexSystemId", sy.IndexSystemId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_IndexWebsiteInsert", DP);
		}



		public async Task<int> SY_IndexWebsiteDeleteAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();
			return (await _sQLCon.ExecuteNonQueryDapperAsync("[SY_IndexWebsiteDelete]", DP));
		}

	}

	public class SY_IndexWebsiteDelete {
		public int? Id { get; set; }
	}


	public class SYEmailInsert
	{
		private SQLCon _sQLCon;

		public SYEmailInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYEmailInsert()
		{
		}

		public async Task<int?> SYEmailInsertDAO(SYEmailInsertIN _sYEmailInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Email", _sYEmailInsertIN.Email);
			DP.Add("Password", _sYEmailInsertIN.Password);
			DP.Add("Server", _sYEmailInsertIN.Server);
			DP.Add("Port", _sYEmailInsertIN.Port);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_EmailInsert", DP);
		}
	}

	public class SYUserUserAgent
	{
		private SQLCon _sQLCon;

		public SYUserUserAgent(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserUserAgent()
		{
		}

		public SYUserUserAgent(long userId, string userAgent, string ipAddress, bool status)
		{
			this.UserId = userId;
			this.UserAgent = userAgent;
			this.IpAddress = ipAddress;
			this.Status = status;
		}
		public SYUserUserAgent(long userId, string userAgent, string ipAddress)
		{
			this.UserId = userId;
			this.UserAgent = userAgent;
			this.IpAddress = ipAddress;
		}

		public long UserId { get; set; }
		public string UserAgent { get; set; }
		public string IpAddress { get; set; }
		public bool Status { get; set; }

		public async Task<int?> SYUserUserAgentInsertDAO(SYUserUserAgent sYUserUserAgentInsert)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", sYUserUserAgentInsert.UserId);
			DP.Add("UserAgent", sYUserUserAgentInsert.UserAgent);
			DP.Add("IpAddress", sYUserUserAgentInsert.IpAddress);
			DP.Add("Status", sYUserUserAgentInsert.Status);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_UserUserAgentInsert", DP);
		}

		public async Task<int?> SYUserUserAgentUpdateStatusDAO(long UserId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_UserUserAgentUpdateStatus", DP);
		}

		public async Task<List<SYUserUserAgent>> SYUserUserAgentGetDAO(SYUserUserAgent sYUserUserAgentInsert)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", sYUserUserAgentInsert.UserId);
			DP.Add("UserAgent", sYUserUserAgentInsert.UserAgent);
			DP.Add("IpAddress", sYUserUserAgentInsert.IpAddress);
			return (await _sQLCon.ExecuteListDapperAsync<SYUserUserAgent>("SY_UserUserAgentGetByUserId_UserAgent_IpAddress", DP)).ToList();
		}

		
	}

	public class SYEmailInsertIN
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string Server { get; set; }
		public string Port { get; set; }
	}

	public class SYPermissionGetByFunction
	{
		private SQLCon _sQLCon;

		public SYPermissionGetByFunction(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionGetByFunction()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short FunctionId { get; set; }
		public short? ParentId { get; set; }

		public async Task<List<SYPermissionGetByFunction>> SYPermissionGetByFunctionDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionGetByFunction>("SY_Permission_GetByFunction", DP)).ToList();
		}
	}

	public class SYPermissionCategoryGet
	{
		private SQLCon _sQLCon;

		public SYPermissionCategoryGet(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionCategoryGet()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }

		public async Task<List<SYPermissionCategoryGet>> SYPermissionCategoryGetDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCategoryGet>("SY_PermissionCategory_Get", DP)).ToList();
		}
	}

	public class SYPermissionCheckByUserId
	{
		private SQLCon _sQLCon;

		public SYPermissionCheckByUserId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionCheckByUserId()
		{
		}

		public int Permission { get; set; }

		public async Task<List<SYPermissionCheckByUserId>> SYPermissionCheckByUserIdDAO(int? UserId, string APIName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			DP.Add("APIName", APIName);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionCheckByUserId>("SY_PermissionCheckByUserId", DP)).ToList();
		}
	}

	public class SYPermissionFunctionGetByCategory
	{
		private SQLCon _sQLCon;

		public SYPermissionFunctionGetByCategory(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionFunctionGetByCategory()
		{
		}

		public short Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public short CategoryId { get; set; }

		public async Task<List<SYPermissionFunctionGetByCategory>> SYPermissionFunctionGetByCategoryDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYPermissionFunctionGetByCategory>("SY_PermissionFunction_GetByCategory", DP)).ToList();
		}
	}

	public class SYPermissionGroupUserInsertByList
	{
		private SQLCon _sQLCon;

		public SYPermissionGroupUserInsertByList(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYPermissionGroupUserInsertByList()
		{
		}

		public async Task<int> SYPermissionGroupUserInsertByListDAO(SYPermissionGroupUserInsertByListIN _sYPermissionGroupUserInsertByListIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("lstid", _sYPermissionGroupUserInsertByListIN.lstid);
			DP.Add("GroupUserId", _sYPermissionGroupUserInsertByListIN.GroupUserId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_PermissionGroupUser_InsertByList", DP));
		}
	}

	public class SYPermissionGroupUserInsertByListIN
	{
		public string lstid { get; set; }
		public int? GroupUserId { get; set; }
	}

	public class SYRoleDelete
	{
		private SQLCon _sQLCon;

		public SYRoleDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleDelete()
		{
		}

		public async Task<int> SYRoleDeleteDAO(SYRoleDeleteIN _sYRoleDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYRoleDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_RoleDelete", DP));
		}
	}

	public class SYRoleDeleteIN
	{
		public int? Id { get; set; }
	}

	public class SYRoleGetAll
	{
		private SQLCon _sQLCon;

		public SYRoleGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleGetAll()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }

		public async Task<List<SYRoleGetAll>> SYRoleGetAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetAll>("SY_RoleGetAll", DP)).ToList();
		}
	}

	public class SYRoleGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYRoleGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public int? UserCount { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<SYRoleGetAllOnPage>> SYRoleGetAllOnPageDAO(int? PageSize, int? PageIndex, int? UserCount, string Name, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("UserCount", UserCount);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetAllOnPage>("SY_RoleGetAllOnPage", DP)).ToList();
		}
	}

	public class SYRoleGetByID
	{
		private SQLCon _sQLCon;

		public SYRoleGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleGetByID()
		{
		}

		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<SYRoleGetByID>> SYRoleGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYRoleGetByID>("SY_RoleGetByID", DP)).ToList();
		}
	}

	public class SYRoleInsert
	{
		private SQLCon _sQLCon;

		public SYRoleInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleInsert()
		{
		}

		public async Task<int?> SYRoleInsertDAO(SYRoleInsertIN _sYRoleInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("OrderNumber", _sYRoleInsertIN.OrderNumber);
			DP.Add("Name", _sYRoleInsertIN.Name);
			DP.Add("IsActived", _sYRoleInsertIN.IsActived);
			DP.Add("IsDeleted", _sYRoleInsertIN.IsDeleted);
			DP.Add("Description", _sYRoleInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_RoleInsert", DP);
		}
	}

	public class SYRoleInsertIN
	{
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class SYRoleUpdate
	{
		private SQLCon _sQLCon;

		public SYRoleUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYRoleUpdate()
		{
		}

		public async Task<int?> SYRoleUpdateDAO(SYRoleUpdateIN _sYRoleUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYRoleUpdateIN.Id);
			DP.Add("OrderNumber", _sYRoleUpdateIN.OrderNumber);
			DP.Add("Name", _sYRoleUpdateIN.Name);
			DP.Add("IsActived", _sYRoleUpdateIN.IsActived);
			DP.Add("IsDeleted", _sYRoleUpdateIN.IsDeleted);
			DP.Add("Description", _sYRoleUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_RoleUpdate", DP);
		}
	}

	public class SYRoleUpdateIN
	{
		public int? Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class SYSMSGetFirst
	{
		private SQLCon _sQLCon;

		public SYSMSGetFirst(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSMSGetFirst()
		{
		}

		public int Id { get; set; }
		public string Linkwebservice { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Code { get; set; }
		public string ServiceID { get; set; }
		public string CommandCode { get; set; }
		public bool? ContenType { get; set; }

		public async Task<List<SYSMSGetFirst>> SYSMSGetFirstDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYSMSGetFirst>("SY_SMSGetFirst", DP)).ToList();
		}
	}

	public class SYSMSInsert
	{
		private SQLCon _sQLCon;

		public SYSMSInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSMSInsert()
		{
		}

		public async Task<int?> SYSMSInsertDAO(SYSMSInsertIN _sYSMSInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Linkwebservice", _sYSMSInsertIN.Linkwebservice);
			DP.Add("User", _sYSMSInsertIN.User);
			DP.Add("Password", _sYSMSInsertIN.Password);
			DP.Add("Code", _sYSMSInsertIN.Code);
			DP.Add("ServiceID", _sYSMSInsertIN.ServiceID);
			DP.Add("CommandCode", _sYSMSInsertIN.CommandCode);
			DP.Add("ContenType", _sYSMSInsertIN.ContenType);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_SMSInsert", DP);
		}
	}

	public class SYSMSInsertIN
	{
		public string Linkwebservice { get; set; }
		public string User { get; set; }
		public string Password { get; set; }
		public string Code { get; set; }
		public string ServiceID { get; set; }
		public string CommandCode { get; set; }
		public bool? ContenType { get; set; }
	}

	public class SYSystemLogDelete
	{
		private SQLCon _sQLCon;

		public SYSystemLogDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLogDelete()
		{
		}

		public async Task<int> SYSystemLogDeleteDAO(SYSystemLogDeleteIN _sYSystemLogDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYSystemLogDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_SystemLogDelete", DP));
		}
	}

	public class SYSystemLogDeleteIN
	{
		public int? Id { get; set; }
	}

	public class SYSystemLogGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYSystemLogGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLogGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public long UserId { get; set; }
		public string FullName { get; set; }
		public string IPAddress { get; set; }
		public string MACAddress { get; set; }
		public string Description { get; set; }
		public DateTime? CreatedDate { get; set; }
		public byte Status { get; set; }
		public string Action { get; set; }
		public string Exception { get; set; }

		public async Task<List<SYSystemLogGetAllOnPage>> SYSystemLogGetAllOnPageDAO(int? UserId, int? PageSize, int? PageIndex, DateTime? FromDate, DateTime? ToDate, string Content, int? Status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("Content", Content);
			DP.Add("Status", Status);

			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLogGetAllOnPage>("SY_SystemLogGetAllOnPage", DP)).ToList();
		}
	}

	public class SYSystemLogGetAllOnPageAdmin
	{
		private SQLCon _sQLCon;

		public SYSystemLogGetAllOnPageAdmin(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYSystemLogGetAllOnPageAdmin()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public long UserId { get; set; }
		public string FullName { get; set; }
		public string IPAddress { get; set; }
		public string MACAddress { get; set; }
		public string Description { get; set; }
		public DateTime? CreatedDate { get; set; }
		public byte Status { get; set; }
		public string Action { get; set; }
		public string Exception { get; set; }

		public async Task<List<SYSystemLogGetAllOnPageAdmin>> SYSystemLogGetAllOnPageAdminDAO(int? UserId, int? PageSize, int? PageIndex, DateTime? CreateDate, byte? Status, string Description)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserId", UserId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("CreateDate", CreateDate);
			DP.Add("Status", Status);
			DP.Add("Description", Description);

			return (await _sQLCon.ExecuteListDapperAsync<SYSystemLogGetAllOnPageAdmin>("SY_SystemLogGetAllOnPageAdmin", DP)).ToList();
		}
	}

	public class SYTimeDelete
	{
		private SQLCon _sQLCon;

		public SYTimeDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYTimeDelete()
		{
		}

		public async Task<int> SYTimeDeleteDAO(SYTimeDeleteIN _sYTimeDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYTimeDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_TimeDelete", DP));
		}
	}

	public class SYTimeDeleteIN
	{
		public int? Id { get; set; }
	}

	public class SYTimeGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SYTimeGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYTimeGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public DateTime? Time { get; set; }
		public string Description { get; set; }

		public async Task<List<SYTimeGetAllOnPage>> SYTimeGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Code, DateTime? Time, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Code", Code);
			DP.Add("Time", Time);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<SYTimeGetAllOnPage>("SY_TimeGetAllOnPage", DP)).ToList();
		}
	}

	public class SYTimeGetByID
	{
		private SQLCon _sQLCon;

		public SYTimeGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYTimeGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public DateTime? Time { get; set; }
		public string Description { get; set; }

		public async Task<List<SYTimeGetByID>> SYTimeGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYTimeGetByID>("SY_TimeGetByID", DP)).ToList();
		}
	}

	public class SYTimeGetDateActive
	{
		private SQLCon _sQLCon;

		public SYTimeGetDateActive(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYTimeGetDateActive()
		{
		}

		public DateTime? Time { get; set; }

		public async Task<List<SYTimeGetDateActive>> SYTimeGetDateActiveDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYTimeGetDateActive>("SY_TimeGetDateActive", DP)).ToList();
		}
	}

	public class SYTimeInsert
	{
		private SQLCon _sQLCon;

		public SYTimeInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYTimeInsert()
		{
		}

		public async Task<int?> SYTimeInsertDAO(SYTimeInsertIN _sYTimeInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _sYTimeInsertIN.Name);
			DP.Add("IsActived", _sYTimeInsertIN.IsActived);
			DP.Add("Time", _sYTimeInsertIN.Time);
			DP.Add("Description", _sYTimeInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_TimeInsert", DP);
		}
	}

	public class SYTimeInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public DateTime? Time { get; set; }
		public string Description { get; set; }
	}

	public class SYTimeUpdate
	{
		private SQLCon _sQLCon;

		public SYTimeUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYTimeUpdate()
		{
		}

		public async Task<int?> SYTimeUpdateDAO(SYTimeUpdateIN _sYTimeUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYTimeUpdateIN.Id);
			DP.Add("Name", _sYTimeUpdateIN.Name);
			DP.Add("IsActived", _sYTimeUpdateIN.IsActived);
			DP.Add("Time", _sYTimeUpdateIN.Time);
			DP.Add("Description", _sYTimeUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_TimeUpdate", DP);
		}
	}

	public class SYTimeUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public DateTime? Time { get; set; }
		public string Description { get; set; }
	}

	public class SYUnitCheckExists
	{
		private SQLCon _sQLCon;

		public SYUnitCheckExists(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitCheckExists()
		{
		}

		public bool? Exists { get; set; }
		public string Value { get; set; }

		public async Task<List<SYUnitCheckExists>> SYUnitCheckExistsDAO(string Field, string Value, long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Field", Field);
			DP.Add("Value", Value);
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitCheckExists>("SY_Unit_CheckExists", DP)).ToList();
		}
	}

	public class SYUnitGetDropdown
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }
		public int? ParentId { get; set; }
		public byte UnitLevel { get; set; }

		public async Task<List<SYUnitGetDropdown>> SYUnitGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdown>("SY_UnitGetDropdown", DP)).ToList();
		}
	}

	public class SYUnitGetDropdownLevel
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdownLevel(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdownLevel()
		{
		}

		public int? Value { get; set; }
		public string Text { get; set; }

		public async Task<List<SYUnitGetDropdownLevel>> SYUnitGetDropdownLevelDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdownLevel>("SY_UnitGetDropdownLevel", DP)).ToList();
		}
	}

	public class SYUnitGetDropdownByListId
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdownByListId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdownByListId()
		{
		}

		public int? Value { get; set; }
		public string Text { get; set; }

		public async Task<List<SYUnitGetDropdownByListId>> SYUnitGetDropdownByListIdDAO(string LtsUnitId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdownByListId>("SY_UnitGetDropdownByListId", DP)).ToList();
		}
	}

	public class SYUnitGetDropdownNotMain
	{
		private SQLCon _sQLCon;

		public SYUnitGetDropdownNotMain(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetDropdownNotMain()
		{
		}

		public int? Value { get; set; }
		public string Text { get; set; }

		public async Task<List<SYUnitGetDropdownNotMain>> SYUnitGetDropdownNotMainDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetDropdownNotMain>("SY_UnitGetDropdownNotMain", DP)).ToList();
		}
	}

	public class SYUnitGetMainId
	{
		private SQLCon _sQLCon;

		public SYUnitGetMainId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetMainId()
		{
		}

		public int Id { get; set; }

		public async Task<List<SYUnitGetMainId>> SYUnitGetMainIdDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetMainId>("SY_UnitGetMainId", DP)).ToList();
		}
	}

	public class SYUnitGetNameById
	{
		private SQLCon _sQLCon;

		public SYUnitGetNameById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetNameById()
		{
		}

		public string Name { get; set; }

		public async Task<List<SYUnitGetNameById>> SYUnitGetNameByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetNameById>("SY_UnitGetNameById", DP)).ToList();
		}
	}

	public class SYUnitGetByField
	{
		private SQLCon _sQLCon;

		public SYUnitGetByField(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUnitGetByField()
		{
		}
		public int Id { get; set; }
		public string Name { get; set; }

		public async Task<List<SYUnitGetByField>> SYUnitGetByFieldDAO(int? fieldId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FieldId", fieldId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUnitGetByField>("SY_UnitGetByField", DP)).ToList();
		}
	}

	public class SYUserGetByUnitId
	{
		private SQLCon _sQLCon;

		public SYUserGetByUnitId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetByUnitId()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public int TypeId { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }

		public async Task<List<SYUserGetByUnitId>> SYUserGetByUnitIdDAO(int? UnitId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitId", UnitId);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByUnitId>("SY_UserGetByUnitId", DP)).ToList();
		}
	}

	public class SYUserGetByUserName
	{
		private SQLCon _sQLCon;

		public SYUserGetByUserName(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetByUserName()
		{
		}

		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }
		public int TypeId { get; set; }

		public async Task<List<SYUserGetByUserName>> SYUserGetByUserNameDAO(string UserName)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UserName", UserName);

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetByUserName>("SY_UserGetByUserName", DP)).ToList();
		}
	}

	public class SYUserGetNonSystem
	{
		private SQLCon _sQLCon;

		public SYUserGetNonSystem(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUserGetNonSystem()
		{
		}

		public int? RowNumber { get; set; }
		public long Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public bool Gender { get; set; }
		public byte Type { get; set; }
		public bool IsSuperAdmin { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int? UnitId { get; set; }
		public byte? CountLock { get; set; }
		public DateTime? LockEndOut { get; set; }
		public string Avatar { get; set; }
		public string Address { get; set; }
		public int? PositionId { get; set; }

		public async Task<List<SYUserGetNonSystem>> SYUserGetNonSystemDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUserGetNonSystem>("SY_UserGetNonSystem", DP)).ToList();
		}
	}

	public class SYUsersGetDropdown
	{
		private SQLCon _sQLCon;

		public SYUsersGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYUsersGetDropdown()
		{
		}

		public long Value { get; set; }
		public string Text { get; set; }

		public async Task<List<SYUsersGetDropdown>> SYUsersGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYUsersGetDropdown>("SY_UsersGetDropdown", DP)).ToList();
		}
	}

	public class SYNotificationDelete
	{
		private SQLCon _sQLCon;

		public SYNotificationDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYNotificationDelete()
		{
		}

		public async Task<int> SYNotificationDeleteDAO(SYNotificationDeleteIN _sYNotificationDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYNotificationDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SYNotificationDelete", DP));
		}
	}

	public class SYNotificationDeleteIN
	{
		public int? Id { get; set; }
	}
}
