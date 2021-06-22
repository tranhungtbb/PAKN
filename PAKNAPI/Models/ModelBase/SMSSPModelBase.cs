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
	public class SMSDoanhNghiepDeleteBySMSId
	{
		private SQLCon _sQLCon;

		public SMSDoanhNghiepDeleteBySMSId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSDoanhNghiepDeleteBySMSId()
		{
		}

		public async Task<int> SMSDoanhNghiepDeleteBySMSIdDAO(SMSDoanhNghiepDeleteBySMSIdIN _sMSDoanhNghiepDeleteBySMSIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _sMSDoanhNghiepDeleteBySMSIdIN.SMSId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_DoanhNghiepDeleteBySMSId", DP));
		}
	}

	public class SMSDoanhNghiepDeleteBySMSIdIN
	{
		public int? SMSId { get; set; }
	}

	public class SMSDoanhNghiepInsert
	{
		private SQLCon _sQLCon;

		public SMSDoanhNghiepInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSDoanhNghiepInsert()
		{
		}

		public async Task<int> SMSDoanhNghiepInsertDAO(SMSDoanhNghiepInsertIN _sMSDoanhNghiepInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _sMSDoanhNghiepInsertIN.SMSId);
			DP.Add("BusinessId", _sMSDoanhNghiepInsertIN.BusinessId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_DoanhNghiepInsert", DP));
		}
	}

	public class SMSDoanhNghiepInsertIN
	{
		public int? SMSId { get; set; }
		public int? BusinessId { get; set; }
	}

	public class SMSGetListIndividualBusinessBySMSId
	{
		private SQLCon _sQLCon;

		public SMSGetListIndividualBusinessBySMSId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSGetListIndividualBusinessBySMSId()
		{
		}

		public long Id { get; set; }
		public int Category { get; set; }
		public string Name { get; set; }
		public string AdministrativeUnitName { get; set; }
		public short? AdministrativeUnitId { get; set; }

		public async Task<List<SMSGetListIndividualBusinessBySMSId>> SMSGetListIndividualBusinessBySMSIdDAO(int? SMSId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", SMSId);

			return (await _sQLCon.ExecuteListDapperAsync<SMSGetListIndividualBusinessBySMSId>("SMS_GetListIndividualBusinessBySMSId", DP)).ToList();
		}
	}

	public class SMSNguoiNhanDeleteBySMSId
	{
		private SQLCon _sQLCon;

		public SMSNguoiNhanDeleteBySMSId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSNguoiNhanDeleteBySMSId()
		{
		}

		public async Task<int> SMSNguoiNhanDeleteBySMSIdDAO(SMSNguoiNhanDeleteBySMSIdIN _sMSNguoiNhanDeleteBySMSIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _sMSNguoiNhanDeleteBySMSIdIN.SMSId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_NguoiNhanDeleteBySMSId", DP));
		}
	}

	public class SMSNguoiNhanDeleteBySMSIdIN
	{
		public int? SMSId { get; set; }
	}

	public class SMSNguoiNhanInsert
	{
		private SQLCon _sQLCon;

		public SMSNguoiNhanInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSNguoiNhanInsert()
		{
		}

		public async Task<int> SMSNguoiNhanInsertDAO(SMSNguoiNhanInsertIN _sMSNguoiNhanInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _sMSNguoiNhanInsertIN.SMSId);
			DP.Add("Individual", _sMSNguoiNhanInsertIN.Individual);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_NguoiNhanInsert", DP));
		}
	}

	public class SMSNguoiNhanInsertIN
	{
		public int? SMSId { get; set; }
		public int? Individual { get; set; }
	}

	public class SMSQuanLyTinNhanDelete
	{
		private SQLCon _sQLCon;

		public SMSQuanLyTinNhanDelete(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSQuanLyTinNhanDelete()
		{
		}

		public async Task<int> SMSQuanLyTinNhanDeleteDAO(SMSQuanLyTinNhanDeleteIN _sMSQuanLyTinNhanDeleteIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sMSQuanLyTinNhanDeleteIN.Id);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_QuanLyTinNhanDelete", DP));
		}
	}

	public class SMSQuanLyTinNhanDeleteIN
	{
		public int? Id { get; set; }
	}

	public class SMSQuanLyTinNhanGetAllOnPage
	{
		private SQLCon _sQLCon;

		public SMSQuanLyTinNhanGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSQuanLyTinNhanGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string UnitName { get; set; }
		public byte Status { get; set; }
		public string Type { get; set; }

		public int SenderNumber { get; set; }

		public async Task<List<SMSQuanLyTinNhanGetAllOnPage>> SMSQuanLyTinNhanGetAllOnPageDAO(int? PageSize, int? PageIndex, string Title, int? UnitId, string Type, byte? Status)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("Title", Title);
			DP.Add("UnitId", UnitId);
			DP.Add("Type", Type);
			DP.Add("Status", Status);

			return (await _sQLCon.ExecuteListDapperAsync<SMSQuanLyTinNhanGetAllOnPage>("SMS_QuanLyTinNhanGetAllOnPage", DP)).ToList();
		}
	}

	public class SMSQuanLyTinNhanGetById
	{
		private SQLCon _sQLCon;

		public SMSQuanLyTinNhanGetById(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSQuanLyTinNhanGetById()
		{
		}

		public int Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Signature { get; set; }
		public DateTime CreateDate { get; set; }
		public int UserCreateId { get; set; }
		public DateTime? SendDate { get; set; }
		public int? UserSend { get; set; }
		public byte Status { get; set; }
		public string Type { get; set; }

		public async Task<List<SMSQuanLyTinNhanGetById>> SMSQuanLyTinNhanGetByIdDAO(int? Id)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);

			return (await _sQLCon.ExecuteListDapperAsync<SMSQuanLyTinNhanGetById>("SMS_QuanLyTinNhanGetById", DP)).ToList();
		}
	}

	public class SMSQuanLyTinNhanInsert
	{
		private SQLCon _sQLCon;

		public SMSQuanLyTinNhanInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSQuanLyTinNhanInsert()
		{
		}

		public async Task<decimal?> SMSQuanLyTinNhanInsertDAO(SMSQuanLyTinNhanInsertIN _sMSQuanLyTinNhanInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Title", _sMSQuanLyTinNhanInsertIN.Title);
			DP.Add("Content", _sMSQuanLyTinNhanInsertIN.Content);
			DP.Add("Signature", _sMSQuanLyTinNhanInsertIN.Signature);
			DP.Add("CreateDate", _sMSQuanLyTinNhanInsertIN.CreateDate);
			DP.Add("UserCreateId", _sMSQuanLyTinNhanInsertIN.UserCreateId);
			DP.Add("SendDate", _sMSQuanLyTinNhanInsertIN.SendDate);
			DP.Add("UserSend", _sMSQuanLyTinNhanInsertIN.UserSend);
			DP.Add("Status", _sMSQuanLyTinNhanInsertIN.Status);
			DP.Add("Type", _sMSQuanLyTinNhanInsertIN.Type);

			return await _sQLCon.ExecuteScalarDapperAsync<decimal?>("SMS_QuanLyTinNhanInsert", DP);
		}
	}

	public class SMSQuanLyTinNhanInsertIN
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public string Signature { get; set; }
		public DateTime? CreateDate { get; set; }
		public int? UserCreateId { get; set; }
		public DateTime? SendDate { get; set; }
		public int? UserSend { get; set; }
		public int? Status { get; set; }
		public string Type { get; set; }
	}

	public class SMSQuanLyTinNhanUpdate
	{
		private SQLCon _sQLCon;

		public SMSQuanLyTinNhanUpdate(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSQuanLyTinNhanUpdate()
		{
		}

		public async Task<int?> SMSQuanLyTinNhanUpdateDAO(SMSQuanLyTinNhanUpdateIN _sMSQuanLyTinNhanUpdateIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", _sMSQuanLyTinNhanUpdateIN.Id);
			DP.Add("Title", _sMSQuanLyTinNhanUpdateIN.Title);
			DP.Add("Content", _sMSQuanLyTinNhanUpdateIN.Content);
			DP.Add("Signature", _sMSQuanLyTinNhanUpdateIN.Signature);
			DP.Add("UpdateDate", _sMSQuanLyTinNhanUpdateIN.UpdateDate);
			DP.Add("UserUpdateId", _sMSQuanLyTinNhanUpdateIN.UserUpdateId);
			DP.Add("SendDate", _sMSQuanLyTinNhanUpdateIN.SendDate);
			DP.Add("UserSend", _sMSQuanLyTinNhanUpdateIN.UserSend);
			DP.Add("Status", _sMSQuanLyTinNhanUpdateIN.Status);
			DP.Add("Type", _sMSQuanLyTinNhanUpdateIN.Type);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SMS_QuanLyTinNhanUpdate", DP);
		}
	}

	public class SMSQuanLyTinNhanUpdateIN
	{
		public int? Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Signature { get; set; }
		public DateTime? UpdateDate { get; set; }
		public int? UserUpdateId { get; set; }
		public DateTime? SendDate { get; set; }
		public int? UserSend { get; set; }
		public int? Status { get; set; }
		public string Type { get; set; }
	}

	public class SMSTinNhanAdministrativeUnitMapDeleteBySMSId
	{
		private SQLCon _sQLCon;

		public SMSTinNhanAdministrativeUnitMapDeleteBySMSId(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSTinNhanAdministrativeUnitMapDeleteBySMSId()
		{
		}

		public async Task<int> SMSTinNhanAdministrativeUnitMapDeleteBySMSIdDAO(SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN _sMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _sMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN.SMSId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_TinNhan_AdministrativeUnit_MapDeleteBySMSId", DP));
		}
	}

	public class SMSTinNhanAdministrativeUnitMapDeleteBySMSIdIN
	{
		public int? SMSId { get; set; }
	}

	public class SMSTinNhanAdministrativeUnitMapInsert
	{
		private SQLCon _sQLCon;

		public SMSTinNhanAdministrativeUnitMapInsert(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public SMSTinNhanAdministrativeUnitMapInsert()
		{
		}

		public async Task<int> SMSTinNhanAdministrativeUnitMapInsertDAO(SMSTinNhanAdministrativeUnitMapInsertIN _sMSTinNhanAdministrativeUnitMapInsertIN)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("SMSId", _sMSTinNhanAdministrativeUnitMapInsertIN.SMSId);
			DP.Add("AdministrativeUnitId", _sMSTinNhanAdministrativeUnitMapInsertIN.AdministrativeUnitId);

			return (await _sQLCon.ExecuteNonQueryDapperAsync("SMS_TinNhan_AdministrativeUnit_MapInsert", DP));
		}
	}

	public class SMSTinNhanAdministrativeUnitMapInsertIN
	{
		public int? SMSId { get; set; }
		public int? AdministrativeUnitId { get; set; }
	}
}
