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
using System.ComponentModel.DataAnnotations;

namespace PAKNAPI.ModelBase
{
	public class CAClassifyKNCTOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? RowNumber; // int, null
	}

	public class CAClassifyKNCT
	{
		private SQLCon _sQLCon;

		public CAClassifyKNCT(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAClassifyKNCT()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }

		public async Task<CAClassifyKNCT> CAClassifyKNCTGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAClassifyKNCT>("CA_ClassifyKNCTGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAClassifyKNCT>> CAClassifyKNCTGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAClassifyKNCT>("CA_ClassifyKNCTGetAll", DP)).ToList();
		}

		public async Task<List<CAClassifyKNCTOnPage>> CAClassifyKNCTGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAClassifyKNCTOnPage>("CA_ClassifyKNCTGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAClassifyKNCTInsert(CAClassifyKNCT _cAClassifyKNCT)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAClassifyKNCT.Name);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ClassifyKNCTInsert", DP));
		}

		public async Task<int> CAClassifyKNCTUpdate(CAClassifyKNCT _cAClassifyKNCT)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAClassifyKNCT.Id);
			DP.Add("Name", _cAClassifyKNCT.Name);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ClassifyKNCTUpdate", DP));
		}

		public async Task<int> CAClassifyKNCTDelete(CAClassifyKNCT _cAClassifyKNCT)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAClassifyKNCT.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ClassifyKNCTDelete", DP));
		}

		public async Task<int> CAClassifyKNCTDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_ClassifyKNCTDeleteAll", DP));
		}

		public async Task<int> CAClassifyKNCTCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_ClassifyKNCTCount", DP));
		}
	}

	public class CADepartmentOnPage
	{
		public int Id { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Address { get; set; }
		public string Fax { get; set; }
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

		public int Id { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Address { get; set; }
		public string Fax { get; set; }

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
			DP.Add("IsActived", _cADepartment.IsActived);
			DP.Add("IsDeleted", _cADepartment.IsDeleted);
			DP.Add("Phone", _cADepartment.Phone);
			DP.Add("Description", _cADepartment.Description);
			DP.Add("Address", _cADepartment.Address);
			DP.Add("Fax", _cADepartment.Fax);
			DP.Add("Email", _cADepartment.Email);
			DP.Add("DepartmentGroupId", _cADepartment.DepartmentGroupId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentInsert", DP));
		}

		public async Task<int> CADepartmentUpdate(CADepartment _cADepartment)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartment.Id);
			DP.Add("Name", _cADepartment.Name);
			DP.Add("IsActived", _cADepartment.IsActived);
			DP.Add("IsDeleted", _cADepartment.IsDeleted);
			DP.Add("Phone", _cADepartment.Phone);
			DP.Add("Description", _cADepartment.Description);
			DP.Add("Address", _cADepartment.Address);
			DP.Add("Fax", _cADepartment.Fax);
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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public int ProvinceId { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public int ProvinceId { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
			DP.Add("IsActived", _cAField.IsActived);
			DP.Add("IsDeleted", _cAField.IsDeleted);
			DP.Add("Description", _cAField.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldInsert", DP));
		}

		public async Task<int> CAFieldUpdate(CAField _cAField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAField.Id);
			DP.Add("Name", _cAField.Name);
			DP.Add("IsActived", _cAField.IsActived);
			DP.Add("IsDeleted", _cAField.IsDeleted);
			DP.Add("Description", _cAField.Description);

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

	public class CAFieldKNCTOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int? RowNumber; // int, null
	}

	public class CAFieldKNCT
	{
		private SQLCon _sQLCon;

		public CAFieldKNCT(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldKNCT()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }

		public async Task<CAFieldKNCT> CAFieldKNCTGetByID(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldKNCT>("CA_FieldKNCTGetByID", DP)).ToList().FirstOrDefault();
		}

		public async Task<List<CAFieldKNCT>> CAFieldKNCTGetAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldKNCT>("CA_FieldKNCTGetAll", DP)).ToList();
		}

		public async Task<List<CAFieldKNCTOnPage>> CAFieldKNCTGetAllOnPage(int PageSize, int PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();

			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<CAFieldKNCTOnPage>("CA_FieldKNCTGetAllOnPage", DP)).ToList();
		}

		public async Task<int?> CAFieldKNCTInsert(CAFieldKNCT _cAFieldKNCT)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAFieldKNCT.Name);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldKNCTInsert", DP));
		}

		public async Task<int> CAFieldKNCTUpdate(CAFieldKNCT _cAFieldKNCT)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAFieldKNCT.Id);
			DP.Add("Name", _cAFieldKNCT.Name);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldKNCTUpdate", DP));
		}

		public async Task<int> CAFieldKNCTDelete(CAFieldKNCT _cAFieldKNCT)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAFieldKNCT.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldKNCTDelete", DP));
		}

		public async Task<int> CAFieldKNCTDeleteAll()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldKNCTDeleteAll", DP));
		}

		public async Task<int> CAFieldKNCTCount()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteDapperAsync<int>("CA_FieldKNCTCount", DP));
		}
	}

	public class CAHashtagOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
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

		public int Id { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Tên hashtag không được để trống")]
		public string Name { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Trạng thái không được để trống")]
		[Range(typeof(bool), "true", "true", ErrorMessage = "Trạng thái không đúng định dạng")]

		public bool IsActived { get; set; }

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
			DP.Add("IsActived", _cAHashtag.IsActived);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagInsert", DP));
		}

		public async Task<int> CAHashtagUpdate(CAHashtag _cAHashtag)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtag.Id);
			DP.Add("Name", _cAHashtag.Name);
			DP.Add("IsActived", _cAHashtag.IsActived);

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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
			DP.Add("IsActived", _cAPosition.IsActived);
			DP.Add("IsDeleted", _cAPosition.IsDeleted);
			DP.Add("Description", _cAPosition.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionInsert", DP));
		}

		public async Task<int> CAPositionUpdate(CAPosition _cAPosition)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPosition.Id);
			DP.Add("Name", _cAPosition.Name);
			DP.Add("IsActived", _cAPosition.IsActived);
			DP.Add("IsDeleted", _cAPosition.IsDeleted);
			DP.Add("Description", _cAPosition.Description);

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
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public int DistrictId { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public int DistrictId { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public int GroupId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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

		public int Id { get; set; }
		public int GroupId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
			DP.Add("GroupId", _cAWord.GroupId);
			DP.Add("Name", _cAWord.Name);
			DP.Add("IsActived", _cAWord.IsActived);
			DP.Add("IsDeleted", _cAWord.IsDeleted);
			DP.Add("Description", _cAWord.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WordInsert", DP));
		}

		public async Task<int> CAWordUpdate(CAWord _cAWord)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWord.Id);
			DP.Add("GroupId", _cAWord.GroupId);
			DP.Add("Name", _cAWord.Name);
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
}
