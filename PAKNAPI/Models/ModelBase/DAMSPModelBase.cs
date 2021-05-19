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
	public class DAMAdministrationCheckExistedId
	{
		private SQLCon _sQLCon;

		public DAMAdministrationCheckExistedId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationCheckExistedId()
		{
		}

		public int? Total { get; set; }

		public async Task<List<DAMAdministrationCheckExistedId>> DAMAdministrationCheckExistedIdDAO(int? AdministrationId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("AdministrationId", AdministrationId);

			return (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationCheckExistedId>("DAM_Administration_CheckExistedId", DP)).ToList();
		}
	}

	public class DAMAdministrationDelete
	{
		private SQLCon _sQLCon;

		public DAMAdministrationDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationDelete()
		{
		}

		public async Task<int> DAMAdministrationDeleteDAO(DAMAdministrationDeleteIN _dAMAdministrationDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMAdministrationDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Administration_Delete", DP));
		}
	}
	public class DAMAdministrationUpdateShow
	{
		private SQLCon _sQLCon;

		public DAMAdministrationUpdateShow(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationUpdateShow()
		{
		}

		public async Task<int> DAMAdministrationUpdateShowDAO(DAMAdministrationUpdateShowIN _dAMAdministrationUpdateShowIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMAdministrationUpdateShowIN.Id);
			DP.Add("Status", _dAMAdministrationUpdateShowIN.Status);
			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Administration_UpdateShow", DP));
		}
	}
	public class DAMAdministrationDeleteIN
	{
		public int? Id { get; set; }
	}
	public class DAMAdministrationUpdateShowIN
    {
		public int? Id { get; set; }
		public int Status { get; set; }
    }

	public class DAMAdministrationDeleteAll
	{
		private SQLCon _sQLCon;

		public DAMAdministrationDeleteAll(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationDeleteAll()
		{
		}

		public async Task<int> DAMAdministrationDeleteAllDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Administration_Delete_All", DP));
		}
	}

	public class DAMAdministrationFilesDelete
	{
		private SQLCon _sQLCon;

		public DAMAdministrationFilesDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationFilesDelete()
		{
		}

		public async Task<int> DAMAdministrationFilesDeleteDAO(DAMAdministrationFilesDeleteIN _dAMAdministrationFilesDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMAdministrationFilesDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Administration_FilesDelete", DP));
		}
	}

	public class DAMAdministrationFilesDeleteIN
	{
		public int? Id { get; set; }
	}

	public class DAMAdministrationFilesGetByAdministrationId
	{
		private SQLCon _sQLCon;

		public DAMAdministrationFilesGetByAdministrationId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationFilesGetByAdministrationId()
		{
		}

		public int Id { get; set; }
		public int? AdministrationId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FileAttach { get; set; }

		public async Task<List<DAMAdministrationFilesGetByAdministrationId>> DAMAdministrationFilesGetByAdministrationIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationFilesGetByAdministrationId>("DAM_Administration_FilesGetByAdministrationId", DP)).ToList();
		}
	}

	public class DAMAdministrationFilesInsert
	{
		private SQLCon _sQLCon;

		public DAMAdministrationFilesInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationFilesInsert()
		{
		}

		public async Task<int> DAMAdministrationFilesInsertDAO(DAMAdministrationFilesInsertIN _dAMAdministrationFilesInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("AdministrationId", _dAMAdministrationFilesInsertIN.AdministrationId);
			DP.Add("Name", _dAMAdministrationFilesInsertIN.Name);
			DP.Add("FileType", _dAMAdministrationFilesInsertIN.FileType);
			DP.Add("FileAttach", _dAMAdministrationFilesInsertIN.FileAttach);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Administration_FilesInsert", DP));
		}
	}

	public class DAMAdministrationFilesInsertIN
	{
		public int? AdministrationId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FileAttach { get; set; }
	}

	public class DAMAdministrationGetById
	{
		private SQLCon _sQLCon;

		public DAMAdministrationGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationGetById()
		{
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string FieldName { get; set; }
		public string Code { get; set; }
		public string CountryCode { get; set; }
		public int UnitReceive { get; set; }
		public int Field { get; set; }
		public string RankReceive { get; set; }
		public bool TypeSend { get; set; }
		public string FileNum { get; set; }
		public string AmountTime { get; set; }
		public string Proceed { get; set; }
		public string Object { get; set; }
		public string Organization { get; set; }
		public string OrganizationDecision { get; set; }
		public string Address { get; set; }
		public string OrganizationAuthor { get; set; }
		public string OrganizationCombine { get; set; }
		public string Result { get; set; }
		public string LegalGrounds { get; set; }
		public string Request { get; set; }
		public string ImpactAssessment { get; set; }
		public string Note { get; set; }
		public byte? Status { get; set; }
		public bool? IsShow { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? PublishedDate { get; set; }
		public long? CreatedBy { get; set; }
		public int? AdministrationId { get; set; }

		public async Task<List<DAMAdministrationGetById>> DAMAdministrationGetByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationGetById>("DAM_Administration_GetById", DP)).ToList();
		}
	}

	public class DAMAdministrationGetList
	{
		private SQLCon _sQLCon;

		public DAMAdministrationGetList(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationGetList()
		{
		}

		public int? RowNumber { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string FieldName { get; set; }
		public string Object { get; set; }
		public byte? Status { get; set; }
		public int Id { get; set; }
		public string Organization { get; set; }

		public async Task<List<DAMAdministrationGetList>> DAMAdministrationGetListDAO(string Code, string Name, string Object, string Organization, int? UnitId, int? Field, int? Status, int? PageSize, int? PageIndex, int? TotalRecords)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Object", Object);
			DP.Add("Organization", Organization);
			DP.Add("UnitId", UnitId);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("TotalRecords", TotalRecords);

			return (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationGetList>("DAM_Administration_GetList", DP)).ToList();
		}
	}

	public class DAMAdministrationInsert
	{
		private SQLCon _sQLCon;

		public DAMAdministrationInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationInsert()
		{
		}

		public async Task<decimal?> DAMAdministrationInsertDAO(DAMAdministrationInsertIN _dAMAdministrationInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Name", _dAMAdministrationInsertIN.Name);
			DP.Add("Code", _dAMAdministrationInsertIN.Code);
			DP.Add("CountryCode", _dAMAdministrationInsertIN.CountryCode);
			DP.Add("UnitReceive", _dAMAdministrationInsertIN.UnitReceive);
			DP.Add("Field", _dAMAdministrationInsertIN.Field);
			DP.Add("RankReceive", _dAMAdministrationInsertIN.RankReceive);
			DP.Add("TypeSend", _dAMAdministrationInsertIN.TypeSend);
			DP.Add("FileNum", _dAMAdministrationInsertIN.FileNum);
			DP.Add("AmountTime", _dAMAdministrationInsertIN.AmountTime);
			DP.Add("Proceed", _dAMAdministrationInsertIN.Proceed);
			DP.Add("Object", _dAMAdministrationInsertIN.Object);
			DP.Add("Organization", _dAMAdministrationInsertIN.Organization);
			DP.Add("OrganizationDecision", _dAMAdministrationInsertIN.OrganizationDecision);
			DP.Add("Address", _dAMAdministrationInsertIN.Address);
			DP.Add("OrganizationAuthor", _dAMAdministrationInsertIN.OrganizationAuthor);
			DP.Add("OrganizationCombine", _dAMAdministrationInsertIN.OrganizationCombine);
			DP.Add("Result", _dAMAdministrationInsertIN.Result);
			DP.Add("LegalGrounds", _dAMAdministrationInsertIN.LegalGrounds);
			DP.Add("Request", _dAMAdministrationInsertIN.Request);
			DP.Add("ImpactAssessment", _dAMAdministrationInsertIN.ImpactAssessment);
			DP.Add("Note", _dAMAdministrationInsertIN.Note);
			DP.Add("Status", _dAMAdministrationInsertIN.Status);
			DP.Add("IsShow", _dAMAdministrationInsertIN.IsShow);
			DP.Add("CreatedBy", _dAMAdministrationInsertIN.CreatedBy);
			DP.Add("AdministrationId", _dAMAdministrationInsertIN.AdministrationId);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("DAM_Administration_Insert", DP);
		}
	}

	public class DAMAdministrationInsertIN
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public string CountryCode { get; set; }
		public int? UnitReceive { get; set; }
		public int? Field { get; set; }
		public string RankReceive { get; set; }
		public bool? TypeSend { get; set; }
		public string FileNum { get; set; }
		public string AmountTime { get; set; }
		public string Proceed { get; set; }
		public string Object { get; set; }
		public string Organization { get; set; }
		public string OrganizationDecision { get; set; }
		public string Address { get; set; }
		public string OrganizationAuthor { get; set; }
		public string OrganizationCombine { get; set; }
		public string Result { get; set; }
		public string LegalGrounds { get; set; }
		public string Request { get; set; }
		public string ImpactAssessment { get; set; }
		public string Note { get; set; }
		public byte? Status { get; set; }
		public bool? IsShow { get; set; }
		public int? CreatedBy { get; set; }
		public int? AdministrationId { get; set; }
	}

	public class DAMAdministrationUpdate
	{
		private SQLCon _sQLCon;

		public DAMAdministrationUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationUpdate()
		{
		}

		public async Task<int> DAMAdministrationUpdateDAO(DAMAdministrationUpdateIN _dAMAdministrationUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMAdministrationUpdateIN.Id);
			DP.Add("Name", _dAMAdministrationUpdateIN.Name);
			DP.Add("Code", _dAMAdministrationUpdateIN.Code);
			DP.Add("CountryCode", _dAMAdministrationUpdateIN.CountryCode);
			DP.Add("UnitReceive", _dAMAdministrationUpdateIN.UnitReceive);
			DP.Add("Field", _dAMAdministrationUpdateIN.Field);
			DP.Add("RankReceive", _dAMAdministrationUpdateIN.RankReceive);
			DP.Add("TypeSend", _dAMAdministrationUpdateIN.TypeSend);
			DP.Add("FileNum", _dAMAdministrationUpdateIN.FileNum);
			DP.Add("AmountTime", _dAMAdministrationUpdateIN.AmountTime);
			DP.Add("Proceed", _dAMAdministrationUpdateIN.Proceed);
			DP.Add("Object", _dAMAdministrationUpdateIN.Object);
			DP.Add("Organization", _dAMAdministrationUpdateIN.Organization);
			DP.Add("OrganizationDecision", _dAMAdministrationUpdateIN.OrganizationDecision);
			DP.Add("Address", _dAMAdministrationUpdateIN.Address);
			DP.Add("OrganizationAuthor", _dAMAdministrationUpdateIN.OrganizationAuthor);
			DP.Add("OrganizationCombine", _dAMAdministrationUpdateIN.OrganizationCombine);
			DP.Add("Result", _dAMAdministrationUpdateIN.Result);
			DP.Add("LegalGrounds", _dAMAdministrationUpdateIN.LegalGrounds);
			DP.Add("Request", _dAMAdministrationUpdateIN.Request);
			DP.Add("ImpactAssessment", _dAMAdministrationUpdateIN.ImpactAssessment);
			DP.Add("Note", _dAMAdministrationUpdateIN.Note);
			DP.Add("Status", _dAMAdministrationUpdateIN.Status);
			DP.Add("IsShow", _dAMAdministrationUpdateIN.IsShow);
			DP.Add("UpdatedBy", _dAMAdministrationUpdateIN.UpdatedBy);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Administration_Update", DP));
		}
	}

	public class DAMAdministrationUpdateIN
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string CountryCode { get; set; }
		public int? UnitReceive { get; set; }
		public int? Field { get; set; }
		public int? RankReceive { get; set; }
		public bool? TypeSend { get; set; }
		public string FileNum { get; set; }
		public string AmountTime { get; set; }
		public string Proceed { get; set; }
		public string Object { get; set; }
		public string Organization { get; set; }
		public string OrganizationDecision { get; set; }
		public string Address { get; set; }
		public string OrganizationAuthor { get; set; }
		public string OrganizationCombine { get; set; }
		public string Result { get; set; }
		public string LegalGrounds { get; set; }
		public string Request { get; set; }
		public string ImpactAssessment { get; set; }
		public string Note { get; set; }
		public byte? Status { get; set; }
		public bool? IsShow { get; set; }
		public int? UpdatedBy { get; set; }
	}

	public class DAMAdministrationGetListHomePage
	{
		private SQLCon _sQLCon;

		public DAMAdministrationGetListHomePage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMAdministrationGetListHomePage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Object { get; set; }
		public string Organization { get; set; }
		public byte? Status { get; set; }
		public string FiledName { get; set; }

		public async Task<List<DAMAdministrationGetListHomePage>> DAMAdministrationGetListHomePageDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationGetListHomePage>("DAM_AdministrationGetListHomePage", DP)).ToList();
		}
	}

	public class DAMChargesCreate
	{
		private SQLCon _sQLCon;

		public DAMChargesCreate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMChargesCreate()
		{
		}

		public async Task<int> DAMChargesCreateDAO(DAMChargesCreateIN _dAMChargesCreateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("AdministrationId", _dAMChargesCreateIN.AdministrationId);
			DP.Add("Charges", _dAMChargesCreateIN.Charges);
			DP.Add("Description", _dAMChargesCreateIN.Description);
			DP.Add("ChargesId", _dAMChargesCreateIN.ChargesId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Charges_Create", DP));
		}
	}

	public class DAMChargesCreateIN
	{
		public int? AdministrationId { get; set; }
		public string Charges { get; set; }
		public string Description { get; set; }
		public int? ChargesId { get; set; }
	}

	public class DAMChargesDeleteById
	{
		private SQLCon _sQLCon;

		public DAMChargesDeleteById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMChargesDeleteById()
		{
		}

		public async Task<int> DAMChargesDeleteByIdDAO(DAMChargesDeleteByIdIN _dAMChargesDeleteByIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMChargesDeleteByIdIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Charges_DeleteById", DP));
		}
	}

	public class DAMChargesDeleteByIdIN
	{
		public int? Id { get; set; }
	}

	public class DAMChargesGetByAdministration
	{
		private SQLCon _sQLCon;

		public DAMChargesGetByAdministration(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMChargesGetByAdministration()
		{
		}

		public int Id { get; set; }
		public int? AdministrationId { get; set; }
		public string Charges { get; set; }
		public string Description { get; set; }
		public int? ChargesId { get; set; }

		public async Task<List<DAMChargesGetByAdministration>> DAMChargesGetByAdministrationDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<DAMChargesGetByAdministration>("DAM_Charges_GetByAdministration", DP)).ToList();
		}
	}

	public class DAMChargesGetById
	{
		private SQLCon _sQLCon;

		public DAMChargesGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMChargesGetById()
		{
		}

		public int Id { get; set; }
		public int? AdministrationId { get; set; }
		public string Charges { get; set; }
		public string Description { get; set; }
		public int? ChargesId { get; set; }

		public async Task<List<DAMChargesGetById>> DAMChargesGetByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<DAMChargesGetById>("DAM_Charges_GetById", DP)).ToList();
		}
	}

	public class DAMChargesUpdate
	{
		private SQLCon _sQLCon;

		public DAMChargesUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMChargesUpdate()
		{
		}

		public async Task<int> DAMChargesUpdateDAO(DAMChargesUpdateIN _dAMChargesUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMChargesUpdateIN.Id);
			DP.Add("AdministrationId", _dAMChargesUpdateIN.AdministrationId);
			DP.Add("Charges", _dAMChargesUpdateIN.Charges);
			DP.Add("Description", _dAMChargesUpdateIN.Description);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_Charges_Update", DP));
		}
	}

	public class DAMChargesUpdateIN
	{
		public int? Id { get; set; }
		public int? AdministrationId { get; set; }
		public string Charges { get; set; }
		public string Description { get; set; }
	}

	public class DAMCompositionProfileCreate
	{
		private SQLCon _sQLCon;

		public DAMCompositionProfileCreate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMCompositionProfileCreate()
		{
		}

		public async Task<decimal?> DAMCompositionProfileCreateDAO(DAMCompositionProfileCreateIN _dAMCompositionProfileCreateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("AdministrationId", _dAMCompositionProfileCreateIN.AdministrationId);
			DP.Add("NameExhibit", _dAMCompositionProfileCreateIN.NameExhibit);
			DP.Add("Form", _dAMCompositionProfileCreateIN.Form);
			DP.Add("FormType", _dAMCompositionProfileCreateIN.FormType);
			DP.Add("OriginalForm", _dAMCompositionProfileCreateIN.OriginalForm);
			DP.Add("CopyForm", _dAMCompositionProfileCreateIN.CopyForm);
			DP.Add("IsBind", _dAMCompositionProfileCreateIN.IsBind);
			DP.Add("CompositionProfileId", _dAMCompositionProfileCreateIN.CompositionProfileId);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("DAM_CompositionProfile_Create", DP);
		}
	}

	public class DAMCompositionProfileCreateIN
	{
		public int? AdministrationId { get; set; }
		public string NameExhibit { get; set; }
		public string Form { get; set; }
		public string FormType { get; set; }
		public string OriginalForm { get; set; }
		public string CopyForm { get; set; }
		public bool? IsBind { get; set; }
		public int? CompositionProfileId { get; set; }
	}

	public class DAMCompositionProfileDeleteById
	{
		private SQLCon _sQLCon;

		public DAMCompositionProfileDeleteById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMCompositionProfileDeleteById()
		{
		}

		public async Task<int> DAMCompositionProfileDeleteByIdDAO(DAMCompositionProfileDeleteByIdIN _dAMCompositionProfileDeleteByIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMCompositionProfileDeleteByIdIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_CompositionProfile_DeleteById", DP));
		}
	}

	public class DAMCompositionProfileDeleteByIdIN
	{
		public int? Id { get; set; }
	}

	public class DAMCompositionProfileFileFilesDelete
	{
		private SQLCon _sQLCon;

		public DAMCompositionProfileFileFilesDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMCompositionProfileFileFilesDelete()
		{
		}

		public async Task<int> DAMCompositionProfileFileFilesDeleteDAO(DAMCompositionProfileFileFilesDeleteIN _dAMCompositionProfileFileFilesDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMCompositionProfileFileFilesDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_CompositionProfile_File_FilesDelete", DP));
		}
	}

	public class DAMCompositionProfileFileFilesDeleteIN
	{
		public int? Id { get; set; }
	}

	public class DAMCompositionProfileFileFilesGetByCompositionProfileId
	{
		private SQLCon _sQLCon;

		public DAMCompositionProfileFileFilesGetByCompositionProfileId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMCompositionProfileFileFilesGetByCompositionProfileId()
		{
		}

		public int Id { get; set; }
		public int? CompositionProfileId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FileAttach { get; set; }

		public async Task<List<DAMCompositionProfileFileFilesGetByCompositionProfileId>> DAMCompositionProfileFileFilesGetByCompositionProfileIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<DAMCompositionProfileFileFilesGetByCompositionProfileId>("DAM_CompositionProfile_File_FilesGetByCompositionProfileId", DP)).ToList();
		}
	}

	public class DAMCompositionProfileFileFilesInsert
	{
		private SQLCon _sQLCon;

		public DAMCompositionProfileFileFilesInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMCompositionProfileFileFilesInsert()
		{
		}

		public async Task<int> DAMCompositionProfileFileFilesInsertDAO(DAMCompositionProfileFileFilesInsertIN _dAMCompositionProfileFileFilesInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("CompositionProfileId", _dAMCompositionProfileFileFilesInsertIN.CompositionProfileId);
			DP.Add("Name", _dAMCompositionProfileFileFilesInsertIN.Name);
			DP.Add("FileType", _dAMCompositionProfileFileFilesInsertIN.FileType);
			DP.Add("FileAttach", _dAMCompositionProfileFileFilesInsertIN.FileAttach);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_CompositionProfile_File_FilesInsert", DP));
		}
	}

	public class DAMCompositionProfileFileFilesInsertIN
	{
		public int? CompositionProfileId { get; set; }
		public string Name { get; set; }
		public short? FileType { get; set; }
		public string FileAttach { get; set; }
	}

	public class DAMCompositionProfileUpdate
	{
		private SQLCon _sQLCon;

		public DAMCompositionProfileUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMCompositionProfileUpdate()
		{
		}

		public async Task<int> DAMCompositionProfileUpdateDAO(DAMCompositionProfileUpdateIN _dAMCompositionProfileUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMCompositionProfileUpdateIN.Id);
			DP.Add("AdministrationId", _dAMCompositionProfileUpdateIN.AdministrationId);
			DP.Add("NameExhibit", _dAMCompositionProfileUpdateIN.NameExhibit);
			DP.Add("Form", _dAMCompositionProfileUpdateIN.Form);
			DP.Add("FormType", _dAMCompositionProfileUpdateIN.FormType);
			DP.Add("OriginalForm", _dAMCompositionProfileUpdateIN.OriginalForm);
			DP.Add("CopyForm", _dAMCompositionProfileUpdateIN.CopyForm);
			DP.Add("IsBind", _dAMCompositionProfileUpdateIN.IsBind);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_CompositionProfile_Update", DP));
		}
	}

	public class DAMCompositionProfileUpdateIN
	{
		public int? Id { get; set; }
		public int? AdministrationId { get; set; }
		public string NameExhibit { get; set; }
		public string Form { get; set; }
		public string FormType { get; set; }
		public string OriginalForm { get; set; }
		public string CopyForm { get; set; }
		public bool? IsBind { get; set; }
	}

	public class DAMImplementationProcessCreate
	{
		private SQLCon _sQLCon;

		public DAMImplementationProcessCreate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMImplementationProcessCreate()
		{
		}

		public async Task<int> DAMImplementationProcessCreateDAO(DAMImplementationProcessCreateIN _dAMImplementationProcessCreateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("AdministrationId", _dAMImplementationProcessCreateIN.AdministrationId);
			DP.Add("Name", _dAMImplementationProcessCreateIN.Name);
			DP.Add("Unit", _dAMImplementationProcessCreateIN.Unit);
			DP.Add("Time", _dAMImplementationProcessCreateIN.Time);
			DP.Add("Result", _dAMImplementationProcessCreateIN.Result);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_ImplementationProcess_Create", DP));
		}
	}

	public class DAMImplementationProcessCreateIN
	{
		public int? AdministrationId { get; set; }
		public string Name { get; set; }
		public string Unit { get; set; }
		public string Time { get; set; }
		public string Result { get; set; }
	}

	public class DAMImplementationProcessDeleteById
	{
		private SQLCon _sQLCon;

		public DAMImplementationProcessDeleteById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMImplementationProcessDeleteById()
		{
		}

		public async Task<int> DAMImplementationProcessDeleteByIdDAO(DAMImplementationProcessDeleteByIdIN _dAMImplementationProcessDeleteByIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMImplementationProcessDeleteByIdIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_ImplementationProcess_DeleteById", DP));
		}
	}

	public class DAMImplementationProcessDeleteByIdIN
	{
		public int? Id { get; set; }
	}

	public class DAMImplementationProcessGetByAdministration
	{
		private SQLCon _sQLCon;

		public DAMImplementationProcessGetByAdministration(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMImplementationProcessGetByAdministration()
		{
		}

		public int Id { get; set; }
		public int? AdministrationId { get; set; }
		public string Name { get; set; }
		public string Unit { get; set; }
		public string Time { get; set; }
		public string Result { get; set; }

		public async Task<List<DAMImplementationProcessGetByAdministration>> DAMImplementationProcessGetByAdministrationDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<DAMImplementationProcessGetByAdministration>("DAM_ImplementationProcess_GetByAdministration", DP)).ToList();
		}
	}

	public class DAMImplementationProcessUpdate
	{
		private SQLCon _sQLCon;

		public DAMImplementationProcessUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public DAMImplementationProcessUpdate()
		{
		}

		public async Task<int> DAMImplementationProcessUpdateDAO(DAMImplementationProcessUpdateIN _dAMImplementationProcessUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _dAMImplementationProcessUpdateIN.Id);
			DP.Add("AdministrationId", _dAMImplementationProcessUpdateIN.AdministrationId);
			DP.Add("Name", _dAMImplementationProcessUpdateIN.Name);
			DP.Add("Unit", _dAMImplementationProcessUpdateIN.Unit);
			DP.Add("Time", _dAMImplementationProcessUpdateIN.Time);
			DP.Add("Result", _dAMImplementationProcessUpdateIN.Result);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("DAM_ImplementationProcess_Update", DP));
		}
	}

	public class DAMImplementationProcessUpdateIN
	{
		public int? Id { get; set; }
		public int? AdministrationId { get; set; }
		public string Name { get; set; }
		public string Unit { get; set; }
		public string Time { get; set; }
		public string Result { get; set; }
	}
}
