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
		public int Id { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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
		public string Code { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Description { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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
		public string Code { get; set; }
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

	public class CAFieldOnPage
	{
		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public int? Quantity { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
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
		public string Name { get; set; }
		public string Code { get; set; }
		public int? Quantity { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

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
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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
		public string Code { get; set; }
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
		public int Id { get; set; }
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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
		public int? OrderNumber { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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

	public class CAWordOnPage
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
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
		public string Name { get; set; }
		public string Code { get; set; }
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
}
