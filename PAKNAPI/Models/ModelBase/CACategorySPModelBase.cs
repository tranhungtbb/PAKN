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
	public class CADepartmentDelete
	{
		private SQLCon _sQLCon;

		public CADepartmentDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentDelete()
		{
		}

		public async Task<int> CADepartmentDeleteDAO(CADepartmentDeleteIN _cADepartmentDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartmentDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentDelete", DP));
		}
	}

	public class CADepartmentDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CADepartmentGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CADepartmentGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Address { get; set; }
		public string Fax { get; set; }
		public string GroupName { get; set; }

		public async Task<List<CADepartmentGetAllOnPage>> CADepartmentGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived, int? DepartmentGroupId, string Phone, string Email, string Address, string Fax)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);
			DP.Add("DepartmentGroupId", DepartmentGroupId);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("Address", Address);
			DP.Add("Fax", Fax);

			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGetAllOnPage>("CA_DepartmentGetAllOnPage", DP)).ToList();
		}
	}

	public class CADepartmentGetByID
	{
		private SQLCon _sQLCon;

		public CADepartmentGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Address { get; set; }
		public string Fax { get; set; }

		public async Task<List<CADepartmentGetByID>> CADepartmentGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGetByID>("CA_DepartmentGetByID", DP)).ToList();
		}
	}

	public class CADepartmentGroupDelete
	{
		private SQLCon _sQLCon;

		public CADepartmentGroupDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGroupDelete()
		{
		}

		public async Task<int> CADepartmentGroupDeleteDAO(CADepartmentGroupDeleteIN _cADepartmentGroupDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartmentGroupDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_DepartmentGroupDelete", DP));
		}
	}

	public class CADepartmentGroupDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CADepartmentGroupGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CADepartmentGroupGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGroupGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CADepartmentGroupGetAllOnPage>> CADepartmentGroupGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGroupGetAllOnPage>("CA_DepartmentGroupGetAllOnPage", DP)).ToList();
		}
	}

	public class CADepartmentGroupGetByID
	{
		private SQLCon _sQLCon;

		public CADepartmentGroupGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGroupGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CADepartmentGroupGetByID>> CADepartmentGroupGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CADepartmentGroupGetByID>("CA_DepartmentGroupGetByID", DP)).ToList();
		}
	}

	public class CADepartmentGroupInsert
	{
		private SQLCon _sQLCon;

		public CADepartmentGroupInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGroupInsert()
		{
		}

		public async Task<int?> CADepartmentGroupInsertDAO(CADepartmentGroupInsertIN _cADepartmentGroupInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cADepartmentGroupInsertIN.Name);
			DP.Add("IsActived", _cADepartmentGroupInsertIN.IsActived);
			DP.Add("IsDeleted", _cADepartmentGroupInsertIN.IsDeleted);
			DP.Add("Description", _cADepartmentGroupInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_DepartmentGroupInsert", DP);
		}
	}

	public class CADepartmentGroupInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CADepartmentGroupUpdate
	{
		private SQLCon _sQLCon;

		public CADepartmentGroupUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentGroupUpdate()
		{
		}

		public async Task<int?> CADepartmentGroupUpdateDAO(CADepartmentGroupUpdateIN _cADepartmentGroupUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartmentGroupUpdateIN.Id);
			DP.Add("Name", _cADepartmentGroupUpdateIN.Name);
			DP.Add("IsActived", _cADepartmentGroupUpdateIN.IsActived);
			DP.Add("IsDeleted", _cADepartmentGroupUpdateIN.IsDeleted);
			DP.Add("Description", _cADepartmentGroupUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_DepartmentGroupUpdate", DP);
		}
	}

	public class CADepartmentGroupUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CADepartmentInsert
	{
		private SQLCon _sQLCon;

		public CADepartmentInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentInsert()
		{
		}

		public async Task<int?> CADepartmentInsertDAO(CADepartmentInsertIN _cADepartmentInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cADepartmentInsertIN.Name);
			DP.Add("DepartmentGroupId", _cADepartmentInsertIN.DepartmentGroupId);
			DP.Add("Phone", _cADepartmentInsertIN.Phone);
			DP.Add("Email", _cADepartmentInsertIN.Email);
			DP.Add("Fax", _cADepartmentInsertIN.Fax);
			DP.Add("Address", _cADepartmentInsertIN.Address);
			DP.Add("IsActived", _cADepartmentInsertIN.IsActived);
			DP.Add("IsDeleted", _cADepartmentInsertIN.IsDeleted);
			DP.Add("Description", _cADepartmentInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_DepartmentInsert", DP);
		}
	}

	public class CADepartmentInsertIN
	{
		public string Name { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public string Address { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CADepartmentUpdate
	{
		private SQLCon _sQLCon;

		public CADepartmentUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CADepartmentUpdate()
		{
		}

		public async Task<int?> CADepartmentUpdateDAO(CADepartmentUpdateIN _cADepartmentUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cADepartmentUpdateIN.Id);
			DP.Add("Name", _cADepartmentUpdateIN.Name);
			DP.Add("Phone", _cADepartmentUpdateIN.Phone);
			DP.Add("IsActived", _cADepartmentUpdateIN.IsActived);
			DP.Add("IsDeleted", _cADepartmentUpdateIN.IsDeleted);
			DP.Add("Description", _cADepartmentUpdateIN.Description);
			DP.Add("Email", _cADepartmentUpdateIN.Email);
			DP.Add("DepartmentGroupId", _cADepartmentUpdateIN.DepartmentGroupId);
			DP.Add("Address", _cADepartmentUpdateIN.Address);
			DP.Add("Fax", _cADepartmentUpdateIN.Fax);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_DepartmentUpdate", DP);
		}
	}

	public class CADepartmentUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public int? DepartmentGroupId { get; set; }
		public string Address { get; set; }
		public string Fax { get; set; }
	}

	public class CAFieldDelete
	{
		private SQLCon _sQLCon;

		public CAFieldDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldDelete()
		{
		}

		public async Task<int> CAFieldDeleteDAO(CAFieldDeleteIN _cAFieldDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAFieldDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_FieldDelete", DP));
		}
	}

	public class CAFieldDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAFieldGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAFieldGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CAFieldGetAllOnPage>> CAFieldGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetAllOnPage>("CA_FieldGetAllOnPage", DP)).ToList();
		}
	}

	public class CAFieldGetByID
	{
		private SQLCon _sQLCon;

		public CAFieldGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CAFieldGetByID>> CAFieldGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAFieldGetByID>("CA_FieldGetByID", DP)).ToList();
		}
	}

	public class CAFieldInsert
	{
		private SQLCon _sQLCon;

		public CAFieldInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldInsert()
		{
		}

		public async Task<int?> CAFieldInsertDAO(CAFieldInsertIN _cAFieldInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAFieldInsertIN.Name);
			DP.Add("IsActived", _cAFieldInsertIN.IsActived);
			DP.Add("IsDeleted", _cAFieldInsertIN.IsDeleted);
			DP.Add("Description", _cAFieldInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_FieldInsert", DP);
		}
	}

	public class CAFieldInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CAFieldUpdate
	{
		private SQLCon _sQLCon;

		public CAFieldUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAFieldUpdate()
		{
		}

		public async Task<int?> CAFieldUpdateDAO(CAFieldUpdateIN _cAFieldUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAFieldUpdateIN.Id);
			DP.Add("Name", _cAFieldUpdateIN.Name);
			DP.Add("IsActived", _cAFieldUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAFieldUpdateIN.IsDeleted);
			DP.Add("Description", _cAFieldUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_FieldUpdate", DP);
		}
	}

	public class CAFieldUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CAHashtagDelete
	{
		private SQLCon _sQLCon;

		public CAHashtagDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagDelete()
		{
		}

		public async Task<int> CAHashtagDeleteDAO(CAHashtagDeleteIN _cAHashtagDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtagDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_HashtagDelete", DP));
		}
	}

	public class CAHashtagDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAHashtagGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAHashtagGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public int? QuantityUser { get; set; }

		public async Task<List<CAHashtagGetAllOnPage>> CAHashtagGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, int? QuantityUser, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("QuantityUser", QuantityUser);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagGetAllOnPage>("CA_HashtagGetAllOnPage", DP)).ToList();
		}
	}

	public class CAHashtagGetByID
	{
		private SQLCon _sQLCon;

		public CAHashtagGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }

		public async Task<List<CAHashtagGetByID>> CAHashtagGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAHashtagGetByID>("CA_HashtagGetByID", DP)).ToList();
		}
	}

	public class CAHashtagInsert
	{
		private SQLCon _sQLCon;

		public CAHashtagInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagInsert()
		{
		}

		public async Task<int?> CAHashtagInsertDAO(CAHashtagInsertIN _cAHashtagInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAHashtagInsertIN.Name);
			DP.Add("IsActived", _cAHashtagInsertIN.IsActived);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_HashtagInsert", DP);
		}
	}

	public class CAHashtagInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
	}

	public class CAHashtagUpdate
	{
		private SQLCon _sQLCon;

		public CAHashtagUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAHashtagUpdate()
		{
		}

		public async Task<int?> CAHashtagUpdateDAO(CAHashtagUpdateIN _cAHashtagUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAHashtagUpdateIN.Id);
			DP.Add("Name", _cAHashtagUpdateIN.Name);
			DP.Add("IsActived", _cAHashtagUpdateIN.IsActived);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_HashtagUpdate", DP);
		}
	}

	public class CAHashtagUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
	}

	public class CANewsTypeDelete
	{
		private SQLCon _sQLCon;

		public CANewsTypeDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CANewsTypeDelete()
		{
		}

		public async Task<int> CANewsTypeDeleteDAO(CANewsTypeDeleteIN _cANewsTypeDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cANewsTypeDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_NewsTypeDelete", DP));
		}
	}

	public class CANewsTypeDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CANewsTypeGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CANewsTypeGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CANewsTypeGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CANewsTypeGetAllOnPage>> CANewsTypeGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CANewsTypeGetAllOnPage>("CA_NewsTypeGetAllOnPage", DP)).ToList();
		}
	}

	public class CANewsTypeGetByID
	{
		private SQLCon _sQLCon;

		public CANewsTypeGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CANewsTypeGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CANewsTypeGetByID>> CANewsTypeGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CANewsTypeGetByID>("CA_NewsTypeGetByID", DP)).ToList();
		}
	}

	public class CANewsTypeInsert
	{
		private SQLCon _sQLCon;

		public CANewsTypeInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CANewsTypeInsert()
		{
		}

		public async Task<int?> CANewsTypeInsertDAO(CANewsTypeInsertIN _cANewsTypeInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cANewsTypeInsertIN.Name);
			DP.Add("IsActived", _cANewsTypeInsertIN.IsActived);
			DP.Add("IsDeleted", _cANewsTypeInsertIN.IsDeleted);
			DP.Add("Description", _cANewsTypeInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_NewsTypeInsert", DP);
		}
	}

	public class CANewsTypeInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CANewsTypeUpdate
	{
		private SQLCon _sQLCon;

		public CANewsTypeUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CANewsTypeUpdate()
		{
		}

		public async Task<int?> CANewsTypeUpdateDAO(CANewsTypeUpdateIN _cANewsTypeUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cANewsTypeUpdateIN.Id);
			DP.Add("Name", _cANewsTypeUpdateIN.Name);
			DP.Add("IsActived", _cANewsTypeUpdateIN.IsActived);
			DP.Add("IsDeleted", _cANewsTypeUpdateIN.IsDeleted);
			DP.Add("Description", _cANewsTypeUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_NewsTypeUpdate", DP);
		}
	}

	public class CANewsTypeUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CAPositionDelete
	{
		private SQLCon _sQLCon;

		public CAPositionDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionDelete()
		{
		}

		public async Task<int> CAPositionDeleteDAO(CAPositionDeleteIN _cAPositionDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPositionDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_PositionDelete", DP));
		}
	}

	public class CAPositionDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAPositionGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAPositionGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CAPositionGetAllOnPage>> CAPositionGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Code, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Code", Code);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAPositionGetAllOnPage>("CA_PositionGetAllOnPage", DP)).ToList();
		}
	}

	public class CAPositionGetByID
	{
		private SQLCon _sQLCon;

		public CAPositionGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CAPositionGetByID>> CAPositionGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAPositionGetByID>("CA_PositionGetByID", DP)).ToList();
		}
	}

	public class CAPositionGetDropdown
	{
		private SQLCon _sQLCon;

		public CAPositionGetDropdown(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionGetDropdown()
		{
		}

		public int Value { get; set; }
		public string Text { get; set; }

		public async Task<List<CAPositionGetDropdown>> CAPositionGetDropdownDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAPositionGetDropdown>("CA_PositionGetDropdown", DP)).ToList();
		}
	}

	public class CAPositionInsert
	{
		private SQLCon _sQLCon;

		public CAPositionInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionInsert()
		{
		}

		public async Task<int?> CAPositionInsertDAO(CAPositionInsertIN _cAPositionInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAPositionInsertIN.Name);
			DP.Add("IsActived", _cAPositionInsertIN.IsActived);
			DP.Add("IsDeleted", _cAPositionInsertIN.IsDeleted);
			DP.Add("Description", _cAPositionInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_PositionInsert", DP);
		}
	}

	public class CAPositionInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CAPositionUpdate
	{
		private SQLCon _sQLCon;

		public CAPositionUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAPositionUpdate()
		{
		}

		public async Task<int?> CAPositionUpdateDAO(CAPositionUpdateIN _cAPositionUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAPositionUpdateIN.Id);
			DP.Add("Name", _cAPositionUpdateIN.Name);
			DP.Add("IsActived", _cAPositionUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAPositionUpdateIN.IsDeleted);
			DP.Add("Description", _cAPositionUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_PositionUpdate", DP);
		}
	}

	public class CAPositionUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CAUnitDelete
	{
		private SQLCon _sQLCon;

		public CAUnitDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitDelete()
		{
		}

		public async Task<int> CAUnitDeleteDAO(CAUnitDeleteIN _cAUnitDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAUnitDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitDelete", DP));
		}
	}

	public class CAUnitDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAUnitGetAll
	{
		private SQLCon _sQLCon;

		public CAUnitGetAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitGetAll()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public int? ParentId { get; set; }
		public byte UnitLevel { get; set; }

		public async Task<List<CAUnitGetAll>> CAUnitGetAllDAO(int? ParentId, byte? UnitLevel)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ParentId", ParentId);
			DP.Add("UnitLevel", UnitLevel);

			return (await _sQLCon.ExecuteListDapperAsync<CAUnitGetAll>("CA_UnitGetAll", DP)).ToList();
		}
	}

	public class CAUnitGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAUnitGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public byte UnitLevel { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool IsMain { get; set; }

		public async Task<List<CAUnitGetAllOnPage>> CAUnitGetAllOnPageDAO(int? PageSize, int? PageIndex, int? ParentId, byte? UnitLevel, string Name, string Phone, string Email, string Address, bool? IsActived, bool? IsMain)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("ParentId", ParentId);
			DP.Add("UnitLevel", UnitLevel);
			DP.Add("Name", Name);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("Address", Address);
			DP.Add("IsActived", IsActived);
			DP.Add("IsMain", IsMain);

			return (await _sQLCon.ExecuteListDapperAsync<CAUnitGetAllOnPage>("CA_UnitGetAllOnPage", DP)).ToList();
		}
	}

	public class CAUnitGetByID
	{
		private SQLCon _sQLCon;

		public CAUnitGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public byte UnitLevel { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool IsMain { get; set; }

		public async Task<List<CAUnitGetByID>> CAUnitGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAUnitGetByID>("CA_UnitGetByID", DP)).ToList();
		}
	}

	public class CAUnitGetTree
	{
		private SQLCon _sQLCon;

		public CAUnitGetTree(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitGetTree()
		{
		}

		public int? RowNumber { get; set; }
		public int ProvindeId { get; set; }
		public string ProvindeName { get; set; }
		public int? DistrictId { get; set; }
		public string DistrictName { get; set; }
		public int? WardsId { get; set; }
		public string WardsName { get; set; }

		public async Task<List<CAUnitGetTree>> CAUnitGetTreeDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<CAUnitGetTree>("CA_UnitGetTree", DP)).ToList();
		}
	}

	public class CAUnitInsert
	{
		private SQLCon _sQLCon;

		public CAUnitInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitInsert()
		{
		}

		public async Task<int> CAUnitInsertDAO(CAUnitInsertIN _cAUnitInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAUnitInsertIN.Name);
			DP.Add("UnitLevel", _cAUnitInsertIN.UnitLevel);
			DP.Add("ParentId", _cAUnitInsertIN.ParentId);
			DP.Add("Description", _cAUnitInsertIN.Description);
			DP.Add("Email", _cAUnitInsertIN.Email);
			DP.Add("Phone", _cAUnitInsertIN.Phone);
			DP.Add("Address", _cAUnitInsertIN.Address);
			DP.Add("IsActived", _cAUnitInsertIN.IsActived);
			DP.Add("IsDeleted", _cAUnitInsertIN.IsDeleted);
			DP.Add("IsMain", _cAUnitInsertIN.IsMain);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitInsert", DP));
		}
	}

	public class CAUnitInsertIN
	{
		public string Name { get; set; }
		public byte? UnitLevel { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsMain { get; set; }
	}

	public class CAUnitUpdate
	{
		private SQLCon _sQLCon;

		public CAUnitUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUnitUpdate()
		{
		}

		public async Task<int> CAUnitUpdateDAO(CAUnitUpdateIN _cAUnitUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAUnitUpdateIN.Id);
			DP.Add("Name", _cAUnitUpdateIN.Name);
			DP.Add("UnitLevel", _cAUnitUpdateIN.UnitLevel);
			DP.Add("ParentId", _cAUnitUpdateIN.ParentId);
			DP.Add("Description", _cAUnitUpdateIN.Description);
			DP.Add("Email", _cAUnitUpdateIN.Email);
			DP.Add("Phone", _cAUnitUpdateIN.Phone);
			DP.Add("Address", _cAUnitUpdateIN.Address);
			DP.Add("IsActived", _cAUnitUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAUnitUpdateIN.IsDeleted);
			DP.Add("IsMain", _cAUnitUpdateIN.IsMain);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_UnitUpdate", DP));
		}
	}

	public class CAUnitUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public byte? UnitLevel { get; set; }
		public int? ParentId { get; set; }
		public string Description { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public bool? IsMain { get; set; }
	}

	public class CAUserGetByUnitId
	{
		private SQLCon _sQLCon;

		public CAUserGetByUnitId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAUserGetByUnitId()
		{
		}

		public long Id { get; set; }
		public string FullName { get; set; }
		public string Phone { get; set; }
		public bool IsActived { get; set; }
		public short? UnitId { get; set; }

		public async Task<List<CAUserGetByUnitId>> CAUserGetByUnitIdDAO(int? PageIndex, int? PageSize, string UserName, string FullName, string Phone, bool? IsActive, int? UnitId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageIndex", PageIndex);
			DP.Add("PageSize", PageSize);
			DP.Add("UserName", UserName);
			DP.Add("FullName", FullName);
			DP.Add("Phone", Phone);
			DP.Add("IsActive", IsActive);
			DP.Add("UnitId", UnitId);

			return (await _sQLCon.ExecuteListDapperAsync<CAUserGetByUnitId>("CA_UserGetByUnitId", DP)).ToList();
		}
	}

	public class CAWordDelete
	{
		private SQLCon _sQLCon;

		public CAWordDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWordDelete()
		{
		}

		public async Task<int> CAWordDeleteDAO(CAWordDeleteIN _cAWordDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWordDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("CA_WordDelete", DP));
		}
	}

	public class CAWordDeleteIN
	{
		public int? Id { get; set; }
	}

	public class CAWordGetAllOnPage
	{
		private SQLCon _sQLCon;

		public CAWordGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWordGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CAWordGetAllOnPage>> CAWordGetAllOnPageDAO(int? PageSize, int? PageIndex, string Name, string Description, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Name", Name);
			DP.Add("Description", Description);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<CAWordGetAllOnPage>("CA_WordGetAllOnPage", DP)).ToList();
		}
	}

	public class CAWordGetByID
	{
		private SQLCon _sQLCon;

		public CAWordGetByID(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWordGetByID()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }
		public string Description { get; set; }

		public async Task<List<CAWordGetByID>> CAWordGetByIDDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<CAWordGetByID>("CA_WordGetByID", DP)).ToList();
		}
	}

	public class CAWordInsert
	{
		private SQLCon _sQLCon;

		public CAWordInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWordInsert()
		{
		}

		public async Task<int?> CAWordInsertDAO(CAWordInsertIN _cAWordInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _cAWordInsertIN.Name);
			DP.Add("IsActived", _cAWordInsertIN.IsActived);
			DP.Add("IsDeleted", _cAWordInsertIN.IsDeleted);
			DP.Add("Description", _cAWordInsertIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_WordInsert", DP);
		}
	}

	public class CAWordInsertIN
	{
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}

	public class CAWordUpdate
	{
		private SQLCon _sQLCon;

		public CAWordUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public CAWordUpdate()
		{
		}

		public async Task<int?> CAWordUpdateDAO(CAWordUpdateIN _cAWordUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _cAWordUpdateIN.Id);
			DP.Add("Name", _cAWordUpdateIN.Name);
			DP.Add("IsActived", _cAWordUpdateIN.IsActived);
			DP.Add("IsDeleted", _cAWordUpdateIN.IsDeleted);
			DP.Add("Description", _cAWordUpdateIN.Description);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("CA_WordUpdate", DP);
		}
	}

	public class CAWordUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public bool? IsActived { get; set; }
		public bool? IsDeleted { get; set; }
		public string Description { get; set; }
	}
}
