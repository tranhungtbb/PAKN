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
	public class CADepartmentOnPage
	{
		public int Id;
		public int? DepartmentGroupId;
		public string Name;
		public string Code;
		public string Email;
		public string Phone;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CADepartment
	{
		private SQLCon _sQLCon;

		public CADepartment(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartment()
		{
		}

		public int Id;
		public int? DepartmentGroupId;
		public string Name;
		public string Code;
		public string Email;
		public string Phone;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CADepartment> CADepartmentGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CADepartment>("CA_DepartmentGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CADepartment>> CADepartmentGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CADepartment>("CA_DepartmentGetAll", DP)).ToList();
		}

		public async Task<List<CADepartmentOnPage>> CADepartmentGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentOnPage>("CA_DepartmentGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CADepartmentInsert(CADepartment _cADepartment)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cADepartment.Name);
			DP.Add("Code", _cADepartment.Code);
			DP.Add("Phone", _cADepartment.Phone);
			DP.Add("IsActived", _cADepartment.IsActived);
			DP.Add("IsDeleted", _cADepartment.IsDeleted);
			DP.Add("Description", _cADepartment.Description);
			DP.Add("Email", _cADepartment.Email);
			DP.Add("DepartmentGroupId", _cADepartment.DepartmentGroupId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentInsert", DP));
		}

		public async Task<int> CADepartmentUpdate(CADepartment _cADepartment)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartment.Id);
			DP.Add("Name", _cADepartment.Name);
			DP.Add("Code", _cADepartment.Code);
			DP.Add("Phone", _cADepartment.Phone);
			DP.Add("IsActived", _cADepartment.IsActived);
			DP.Add("IsDeleted", _cADepartment.IsDeleted);
			DP.Add("Description", _cADepartment.Description);
			DP.Add("Email", _cADepartment.Email);
			DP.Add("DepartmentGroupId", _cADepartment.DepartmentGroupId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentUpdate", DP));
		}

		public async Task<int> CADepartmentDelete(CADepartment _cADepartment)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartment.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentDelete", DP));
		}

		public async Task<int> CADepartmentDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentDeleteAll", DP));
		}

		public async Task<int> CADepartmentCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_DepartmentCount", DP));
		}
	}

	public class CADepartmentGroupOnPage
	{
		public int Id;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CADepartmentGroup
	{
		private SQLCon _sQLCon;

		public CADepartmentGroup(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGroup()
		{
		}

		public int Id;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CADepartmentGroup> CADepartmentGroupGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGroup>("CA_DepartmentGroupGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CADepartmentGroup>> CADepartmentGroupGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGroup>("CA_DepartmentGroupGetAll", DP)).ToList();
		}

		public async Task<List<CADepartmentGroupOnPage>> CADepartmentGroupGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGroupOnPage>("CA_DepartmentGroupGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CADepartmentGroupInsert(CADepartmentGroup _cADepartmentGroup)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cADepartmentGroup.Name);
			DP.Add("Code", _cADepartmentGroup.Code);
			DP.Add("IsActived", _cADepartmentGroup.IsActived);
			DP.Add("IsDeleted", _cADepartmentGroup.IsDeleted);
			DP.Add("Description", _cADepartmentGroup.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentGroupInsert", DP));
		}

		public async Task<int> CADepartmentGroupUpdate(CADepartmentGroup _cADepartmentGroup)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartmentGroup.Id);
			DP.Add("Name", _cADepartmentGroup.Name);
			DP.Add("Code", _cADepartmentGroup.Code);
			DP.Add("IsActived", _cADepartmentGroup.IsActived);
			DP.Add("IsDeleted", _cADepartmentGroup.IsDeleted);
			DP.Add("Description", _cADepartmentGroup.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentGroupUpdate", DP));
		}

		public async Task<int> CADepartmentGroupDelete(CADepartmentGroup _cADepartmentGroup)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartmentGroup.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentGroupDelete", DP));
		}

		public async Task<int> CADepartmentGroupDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentGroupDeleteAll", DP));
		}

		public async Task<int> CADepartmentGroupCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_DepartmentGroupCount", DP));
		}
	}

	public class CADistrictOnPage
	{
		public int Id;
		public int ProvinceId;
		public string Name;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CADistrict
	{
		private SQLCon _sQLCon;

		public CADistrict(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADistrict()
		{
		}

		public int Id;
		public int ProvinceId;
		public string Name;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CADistrict> CADistrictGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CADistrict>("CA_DistrictGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CADistrict>> CADistrictGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CADistrict>("CA_DistrictGetAll", DP)).ToList();
		}

		public async Task<List<CADistrictOnPage>> CADistrictGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CADistrictOnPage>("CA_DistrictGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CADistrictInsert(CADistrict _cADistrict)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ProvinceId", _cADistrict.ProvinceId);
			DP.Add("Name", _cADistrict.Name);
			DP.Add("IsActived", _cADistrict.IsActived);
			DP.Add("IsDeleted", _cADistrict.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DistrictInsert", DP));
		}

		public async Task<int> CADistrictUpdate(CADistrict _cADistrict)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADistrict.Id);
			DP.Add("ProvinceId", _cADistrict.ProvinceId);
			DP.Add("Name", _cADistrict.Name);
			DP.Add("IsActived", _cADistrict.IsActived);
			DP.Add("IsDeleted", _cADistrict.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DistrictUpdate", DP));
		}

		public async Task<int> CADistrictDelete(CADistrict _cADistrict)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADistrict.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DistrictDelete", DP));
		}

		public async Task<int> CADistrictDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DistrictDeleteAll", DP));
		}

		public async Task<int> CADistrictCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_DistrictCount", DP));
		}
	}

	public class CAFieldOnPage
	{
		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAField
	{
		private SQLCon _sQLCon;

		public CAField(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAField()
		{
		}

		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAField> CAFieldGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAField>("CA_FieldGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAField>> CAFieldGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAField>("CA_FieldGetAll", DP)).ToList();
		}

		public async Task<List<CAFieldOnPage>> CAFieldGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAFieldOnPage>("CA_FieldGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAFieldInsert(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAField.Name);
			DP.Add("Code", _cAField.Code);
			DP.Add("IsActived", _cAField.IsActived);
			DP.Add("IsDeleted", _cAField.IsDeleted);
			DP.Add("Description", _cAField.Description);
			DP.Add("OrderNumber", _cAField.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldInsert", DP));
		}

		public async Task<int> CAFieldUpdate(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAField.Id);
			DP.Add("Name", _cAField.Name);
			DP.Add("Code", _cAField.Code);
			DP.Add("IsActived", _cAField.IsActived);
			DP.Add("IsDeleted", _cAField.IsDeleted);
			DP.Add("Description", _cAField.Description);
			DP.Add("OrderNumber", _cAField.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldUpdate", DP));
		}

		public async Task<int> CAFieldDelete(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAField.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDelete", DP));
		}

		public async Task<int> CAFieldDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDeleteAll", DP));
		}

		public async Task<int> CAFieldCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_FieldCount", DP));
		}
	}

	public class CAHashtagOnPage
	{
		public int Id;
		public string Name;
		public string Code;
		public int? Quantity;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAHashtag
	{
		private SQLCon _sQLCon;

		public CAHashtag(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtag()
		{
		}

		public int Id;
		public string Name;
		public string Code;
		public int? Quantity;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAHashtag> CAHashtagGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtag>("CA_HashtagGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAHashtag>> CAHashtagGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtag>("CA_HashtagGetAll", DP)).ToList();
		}

		public async Task<List<CAHashtagOnPage>> CAHashtagGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagOnPage>("CA_HashtagGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAHashtagInsert(CAHashtag _cAHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAHashtag.Name);
			DP.Add("Code", _cAHashtag.Code);
			DP.Add("IsActived", _cAHashtag.IsActived);
			DP.Add("IsDeleted", _cAHashtag.IsDeleted);
			DP.Add("Quantity", _cAHashtag.Quantity);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagInsert", DP));
		}

		public async Task<int> CAHashtagUpdate(CAHashtag _cAHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtag.Id);
			DP.Add("Name", _cAHashtag.Name);
			DP.Add("Code", _cAHashtag.Code);
			DP.Add("IsActived", _cAHashtag.IsActived);
			DP.Add("IsDeleted", _cAHashtag.IsDeleted);
			DP.Add("Quantity", _cAHashtag.Quantity);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagUpdate", DP));
		}

		public async Task<int> CAHashtagDelete(CAHashtag _cAHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtag.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagDelete", DP));
		}

		public async Task<int> CAHashtagDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagDeleteAll", DP));
		}

		public async Task<int> CAHashtagCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_HashtagCount", DP));
		}
	}

	public class CANewsTypeOnPage
	{
		public int Id;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CANewsType
	{
		private SQLCon _sQLCon;

		public CANewsType(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CANewsType()
		{
		}

		public int Id;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CANewsType> CANewsTypeGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CANewsType>("CA_NewsTypeGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CANewsType>> CANewsTypeGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CANewsType>("CA_NewsTypeGetAll", DP)).ToList();
		}

		public async Task<List<CANewsTypeOnPage>> CANewsTypeGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CANewsTypeOnPage>("CA_NewsTypeGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CANewsTypeInsert(CANewsType _cANewsType)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cANewsType.Name);
			DP.Add("Code", _cANewsType.Code);
			DP.Add("IsActived", _cANewsType.IsActived);
			DP.Add("IsDeleted", _cANewsType.IsDeleted);
			DP.Add("Description", _cANewsType.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_NewsTypeInsert", DP));
		}

		public async Task<int> CANewsTypeUpdate(CANewsType _cANewsType)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cANewsType.Id);
			DP.Add("Name", _cANewsType.Name);
			DP.Add("Code", _cANewsType.Code);
			DP.Add("IsActived", _cANewsType.IsActived);
			DP.Add("IsDeleted", _cANewsType.IsDeleted);
			DP.Add("Description", _cANewsType.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_NewsTypeUpdate", DP));
		}

		public async Task<int> CANewsTypeDelete(CANewsType _cANewsType)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cANewsType.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_NewsTypeDelete", DP));
		}

		public async Task<int> CANewsTypeDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_NewsTypeDeleteAll", DP));
		}

		public async Task<int> CANewsTypeCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_NewsTypeCount", DP));
		}
	}

	public class CAPositionOnPage
	{
		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAPosition
	{
		private SQLCon _sQLCon;

		public CAPosition(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPosition()
		{
		}

		public int Id;
		public int? OrderNumber;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAPosition> CAPositionGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAPosition>("CA_PositionGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAPosition>> CAPositionGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAPosition>("CA_PositionGetAll", DP)).ToList();
		}

		public async Task<List<CAPositionOnPage>> CAPositionGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAPositionOnPage>("CA_PositionGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAPositionInsert(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAPosition.Name);
			DP.Add("Code", _cAPosition.Code);
			DP.Add("IsActived", _cAPosition.IsActived);
			DP.Add("IsDeleted", _cAPosition.IsDeleted);
			DP.Add("Description", _cAPosition.Description);
			DP.Add("OrderNumber", _cAPosition.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionInsert", DP));
		}

		public async Task<int> CAPositionUpdate(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPosition.Id);
			DP.Add("Name", _cAPosition.Name);
			DP.Add("Code", _cAPosition.Code);
			DP.Add("IsActived", _cAPosition.IsActived);
			DP.Add("IsDeleted", _cAPosition.IsDeleted);
			DP.Add("Description", _cAPosition.Description);
			DP.Add("OrderNumber", _cAPosition.OrderNumber);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionUpdate", DP));
		}

		public async Task<int> CAPositionDelete(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPosition.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionDelete", DP));
		}

		public async Task<int> CAPositionDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionDeleteAll", DP));
		}

		public async Task<int> CAPositionCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_PositionCount", DP));
		}
	}

	public class CAProvinceOnPage
	{
		public int Id;
		public string Name;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAProvince
	{
		private SQLCon _sQLCon;

		public CAProvince(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAProvince()
		{
		}

		public int Id;
		public string Name;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAProvince> CAProvinceGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAProvince>("CA_ProvinceGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAProvince>> CAProvinceGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAProvince>("CA_ProvinceGetAll", DP)).ToList();
		}

		public async Task<List<CAProvinceOnPage>> CAProvinceGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAProvinceOnPage>("CA_ProvinceGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAProvinceInsert(CAProvince _cAProvince)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAProvince.Name);
			DP.Add("IsActived", _cAProvince.IsActived);
			DP.Add("IsDeleted", _cAProvince.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ProvinceInsert", DP));
		}

		public async Task<int> CAProvinceUpdate(CAProvince _cAProvince)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAProvince.Id);
			DP.Add("Name", _cAProvince.Name);
			DP.Add("IsActived", _cAProvince.IsActived);
			DP.Add("IsDeleted", _cAProvince.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ProvinceUpdate", DP));
		}

		public async Task<int> CAProvinceDelete(CAProvince _cAProvince)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAProvince.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ProvinceDelete", DP));
		}

		public async Task<int> CAProvinceDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ProvinceDeleteAll", DP));
		}

		public async Task<int> CAProvinceCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_ProvinceCount", DP));
		}
	}

	public class CAWardsOnPage
	{
		public int Id;
		public int DistrictId;
		public string Name;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAWards
	{
		private SQLCon _sQLCon;

		public CAWards(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWards()
		{
		}

		public int Id;
		public int DistrictId;
		public string Name;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAWards> CAWardsGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAWards>("CA_WardsGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAWards>> CAWardsGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAWards>("CA_WardsGetAll", DP)).ToList();
		}

		public async Task<List<CAWardsOnPage>> CAWardsGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAWardsOnPage>("CA_WardsGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAWardsInsert(CAWards _cAWards)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("DistrictId", _cAWards.DistrictId);
			DP.Add("Name", _cAWards.Name);
			DP.Add("IsActived", _cAWards.IsActived);
			DP.Add("IsDeleted", _cAWards.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WardsInsert", DP));
		}

		public async Task<int> CAWardsUpdate(CAWards _cAWards)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWards.Id);
			DP.Add("DistrictId", _cAWards.DistrictId);
			DP.Add("Name", _cAWards.Name);
			DP.Add("IsActived", _cAWards.IsActived);
			DP.Add("IsDeleted", _cAWards.IsDeleted);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WardsUpdate", DP));
		}

		public async Task<int> CAWardsDelete(CAWards _cAWards)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWards.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WardsDelete", DP));
		}

		public async Task<int> CAWardsDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WardsDeleteAll", DP));
		}

		public async Task<int> CAWardsCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_WardsCount", DP));
		}
	}

	public class CAWordOnPage
	{
		public int Id;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class CAWord
	{
		private SQLCon _sQLCon;

		public CAWord(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWord()
		{
		}

		public int Id;
		public string Name;
		public string Code;
		public string Description;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<CAWord> CAWordGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAWord>("CA_WordGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAWord>> CAWordGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAWord>("CA_WordGetAll", DP)).ToList();
		}

		public async Task<List<CAWordOnPage>> CAWordGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAWordOnPage>("CA_WordGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAWordInsert(CAWord _cAWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAWord.Name);
			DP.Add("Code", _cAWord.Code);
			DP.Add("IsActived", _cAWord.IsActived);
			DP.Add("IsDeleted", _cAWord.IsDeleted);
			DP.Add("Description", _cAWord.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WordInsert", DP));
		}

		public async Task<int> CAWordUpdate(CAWord _cAWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWord.Id);
			DP.Add("Name", _cAWord.Name);
			DP.Add("Code", _cAWord.Code);
			DP.Add("IsActived", _cAWord.IsActived);
			DP.Add("IsDeleted", _cAWord.IsDeleted);
			DP.Add("Description", _cAWord.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WordUpdate", DP));
		}

		public async Task<int> CAWordDelete(CAWord _cAWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWord.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WordDelete", DP));
		}

		public async Task<int> CAWordDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WordDeleteAll", DP));
		}

		public async Task<int> CAWordCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_WordCount", DP));
		}
	}

	public class SYBusinessOnPage
	{
		public long Id;
		public int? ProvinceId;
		public int WardsId;
		public int DistrictId;
		public string Name;
		public string Code;
		public string Address;
		public string Email;
		public string Phone;
		public string Representative;
		public string IDCard;
		public string Place;
		public string NativePlace;
		public string PermanentPlace;
		public string Nation;
		public DateTime? BirthDay;
		public bool? Gender;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class SYBusiness
	{
		private SQLCon _sQLCon;

		public SYBusiness(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYBusiness()
		{
		}

		public long Id;
		public int? ProvinceId;
		public int WardsId;
		public int DistrictId;
		public string Name;
		public string Code;
		public string Address;
		public string Email;
		public string Phone;
		public string Representative;
		public string IDCard;
		public string Place;
		public string NativePlace;
		public string PermanentPlace;
		public string Nation;
		public DateTime? BirthDay;
		public bool? Gender;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<SYBusiness> SYBusinessGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYBusiness>("SY_BusinessGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYBusiness>> SYBusinessGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYBusiness>("SY_BusinessGetAll", DP)).ToList();
		}

		public async Task<List<SYBusinessOnPage>> SYBusinessGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYBusinessOnPage>("SY_BusinessGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYBusinessInsert(SYBusiness _sYBusiness)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("WardsId", _sYBusiness.WardsId);
			DP.Add("DistrictId", _sYBusiness.DistrictId);
			DP.Add("Name", _sYBusiness.Name);
			DP.Add("Code", _sYBusiness.Code);
			DP.Add("IsActived", _sYBusiness.IsActived);
			DP.Add("IsDeleted", _sYBusiness.IsDeleted);
			DP.Add("ProvinceId", _sYBusiness.ProvinceId);
			DP.Add("Address", _sYBusiness.Address);
			DP.Add("Email", _sYBusiness.Email);
			DP.Add("Phone", _sYBusiness.Phone);
			DP.Add("Representative", _sYBusiness.Representative);
			DP.Add("IDCard", _sYBusiness.IDCard);
			DP.Add("Place", _sYBusiness.Place);
			DP.Add("NativePlace", _sYBusiness.NativePlace);
			DP.Add("PermanentPlace", _sYBusiness.PermanentPlace);
			DP.Add("Nation", _sYBusiness.Nation);
			DP.Add("BirthDay", _sYBusiness.BirthDay);
			DP.Add("Gender", _sYBusiness.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_BusinessInsert", DP));
		}

		public async Task<int> SYBusinessUpdate(SYBusiness _sYBusiness)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("WardsId", _sYBusiness.WardsId);
			DP.Add("DistrictId", _sYBusiness.DistrictId);
			DP.Add("Name", _sYBusiness.Name);
			DP.Add("Code", _sYBusiness.Code);
			DP.Add("Id", _sYBusiness.Id);
			DP.Add("IsActived", _sYBusiness.IsActived);
			DP.Add("IsDeleted", _sYBusiness.IsDeleted);
			DP.Add("ProvinceId", _sYBusiness.ProvinceId);
			DP.Add("Address", _sYBusiness.Address);
			DP.Add("Email", _sYBusiness.Email);
			DP.Add("Phone", _sYBusiness.Phone);
			DP.Add("Representative", _sYBusiness.Representative);
			DP.Add("IDCard", _sYBusiness.IDCard);
			DP.Add("Place", _sYBusiness.Place);
			DP.Add("NativePlace", _sYBusiness.NativePlace);
			DP.Add("PermanentPlace", _sYBusiness.PermanentPlace);
			DP.Add("Nation", _sYBusiness.Nation);
			DP.Add("BirthDay", _sYBusiness.BirthDay);
			DP.Add("Gender", _sYBusiness.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_BusinessUpdate", DP));
		}

		public async Task<int> SYBusinessDelete(SYBusiness _sYBusiness)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYBusiness.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_BusinessDelete", DP));
		}

		public async Task<int> SYBusinessDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_BusinessDeleteAll", DP));
		}

		public async Task<int> SYBusinessCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("SY_BusinessCount", DP));
		}
	}

	public class SYIndividualOnPage
	{
		public long Id;
		public int WardsId;
		public int DistrictId;
		public string FullName;
		public string Code;
		public string Address;
		public string Email;
		public string Phone;
		public string IDCard;
		public string Place;
		public string NativePlace;
		public string PermanentPlace;
		public string Nation;
		public DateTime? BirthDay;
		public bool? Gender;
		public bool IsActived;
		public bool IsDeleted;
		public int? RowNumber; // int, null
	}

	public class SYIndividual
	{
		private SQLCon _sQLCon;

		public SYIndividual(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SYIndividual()
		{
		}

		public long Id;
		public int WardsId;
		public int DistrictId;
		public string FullName;
		public string Code;
		public string Address;
		public string Email;
		public string Phone;
		public string IDCard;
		public string Place;
		public string NativePlace;
		public string PermanentPlace;
		public string Nation;
		public DateTime? BirthDay;
		public bool? Gender;
		public bool IsActived;
		public bool IsDeleted;

		public async Task<SYIndividual> SYIndividualGetByID(long? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SYIndividual>("SY_IndividualGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<SYIndividual>> SYIndividualGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<SYIndividual>("SY_IndividualGetAll", DP)).ToList();
		}

		public async Task<List<SYIndividualOnPage>> SYIndividualGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<SYIndividualOnPage>("SY_IndividualGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> SYIndividualInsert(SYIndividual _sYIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("WardsId", _sYIndividual.WardsId);
			DP.Add("DistrictId", _sYIndividual.DistrictId);
			DP.Add("FullName", _sYIndividual.FullName);
			DP.Add("Code", _sYIndividual.Code);
			DP.Add("IsActived", _sYIndividual.IsActived);
			DP.Add("IsDeleted", _sYIndividual.IsDeleted);
			DP.Add("Address", _sYIndividual.Address);
			DP.Add("Email", _sYIndividual.Email);
			DP.Add("Phone", _sYIndividual.Phone);
			DP.Add("IDCard", _sYIndividual.IDCard);
			DP.Add("Place", _sYIndividual.Place);
			DP.Add("NativePlace", _sYIndividual.NativePlace);
			DP.Add("PermanentPlace", _sYIndividual.PermanentPlace);
			DP.Add("Nation", _sYIndividual.Nation);
			DP.Add("BirthDay", _sYIndividual.BirthDay);
			DP.Add("Gender", _sYIndividual.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_IndividualInsert", DP));
		}

		public async Task<int> SYIndividualUpdate(SYIndividual _sYIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYIndividual.Id);
			DP.Add("WardsId", _sYIndividual.WardsId);
			DP.Add("DistrictId", _sYIndividual.DistrictId);
			DP.Add("FullName", _sYIndividual.FullName);
			DP.Add("Code", _sYIndividual.Code);
			DP.Add("IsActived", _sYIndividual.IsActived);
			DP.Add("IsDeleted", _sYIndividual.IsDeleted);
			DP.Add("Address", _sYIndividual.Address);
			DP.Add("Email", _sYIndividual.Email);
			DP.Add("Phone", _sYIndividual.Phone);
			DP.Add("IDCard", _sYIndividual.IDCard);
			DP.Add("Place", _sYIndividual.Place);
			DP.Add("NativePlace", _sYIndividual.NativePlace);
			DP.Add("PermanentPlace", _sYIndividual.PermanentPlace);
			DP.Add("Nation", _sYIndividual.Nation);
			DP.Add("BirthDay", _sYIndividual.BirthDay);
			DP.Add("Gender", _sYIndividual.Gender);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_IndividualUpdate", DP));
		}

		public async Task<int> SYIndividualDelete(SYIndividual _sYIndividual)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sYIndividual.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_IndividualDelete", DP));
		}

		public async Task<int> SYIndividualDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SY_IndividualDeleteAll", DP));
		}

		public async Task<int> SYIndividualCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("SY_IndividualCount", DP));
		}
	}
}
