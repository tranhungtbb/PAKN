using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Statistic
{
	public class StatisticRecommendationByUnitGetAllOnPage
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByUnitGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByUnitGetAllOnPage()
		{
		}
		public int UnitId { get; set; }
		public string UnitName { get; set; }

		public int Total { get; set; }
		public int Created { get; set; } //1
		public int ReceiveWait { get; set; } // 2
		public int ReceiveApproved { get; set; } // 3
		public int ReceiveDeny { get; set; } // 4
		public int ProcessWait { get; set; } // 5
		public int ProcessDeny { get; set; } // 6
		public int Processing { get; set; } // 7
		public int ApproveWait { get; set; } // 8
		public int ApproveDeny { get; set; } // 9

		public int Finised { get; set; }

		public async Task<List<StatisticRecommendationByUnitGetAllOnPage>> StatisticRecommendationByUnitGetAllOnPageDAO(string LtsUnitId, int UnitProcessId, long UserProcessId, DateTime? FromDate, DateTime? ToDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByUnitGetAllOnPage>("[TK_ListRecommendationByUnit]", DP)).ToList();
		}
	}


	public class StatisticRecommendationByUnitDetailGetAllOnPage
	{
		private SQLCon _sQLCon;
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string FieldName { get; set; }
		public int Status { get; set; }
		public int? RowNumber { get; set; }

		public StatisticRecommendationByUnitDetailGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByUnitDetailGetAllOnPage()
		{
		}

		public async Task<List<StatisticRecommendationByUnitDetailGetAllOnPage>> StatisticRecommendationByUnitDetailGetAllOnPageDAO(int UnitId, int UnitProcessId, long UserProcessId, string Code, string CreateName, string Title, int? Field, int? Status, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitId", UnitId);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("Code", Code);
			DP.Add("CreateName", CreateName);
			DP.Add("Title", Title);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByUnitDetailGetAllOnPage>("TK_ListRecommendationByUnit_Detail", DP)).ToList();
		}
	}

	public class StatisticRecommendationByFieldGetAllOnPage
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByFieldGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByFieldGetAllOnPage()
		{
		}

		public int FieldId { get; set; }
		public string FieldName { get; set; }

		public int Total { get; set; }
		public int Created { get; set; } //1
		public int ReceiveWait { get; set; } // 2
		public int ReceiveApproved { get; set; } // 3
		public int ReceiveDeny { get; set; } // 4
		public int ProcessWait { get; set; } // 5
		public int ProcessDeny { get; set; } // 6
		public int Processing { get; set; } // 7
		public int ApproveWait { get; set; } // 8
		public int ApproveDeny { get; set; } // 9

		public int Finised { get; set; }

		public async Task<List<StatisticRecommendationByFieldGetAllOnPage>> StatisticRecommendationByFieldGetAllOnPageDAO(string LtsUnitId, int UnitProcessId, long UserProcessId, DateTime? FromDate, DateTime? ToDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByFieldGetAllOnPage>("[TK_ListRecommendationByField]", DP)).ToList();
		}
	}

	public class StatisticRecommendationByFiledDetailGetAllOnPage
	{
		private SQLCon _sQLCon;
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string UnitName { get; set; }
		public int Status { get; set; }
		public int? RowNumber { get; set; }

		public StatisticRecommendationByFiledDetailGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByFiledDetailGetAllOnPage()
		{
		}

		public async Task<List<StatisticRecommendationByFiledDetailGetAllOnPage>> StatisticRecommendationByFieldDetailGetAllOnPageDAO(int FiledId, int UnitProcessId, long UserProcessId, string Code, string CreateName, string Title, string? LstUnitId, int? Status, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LstUnitId", LstUnitId);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("Code", Code);
			DP.Add("CreateName", CreateName);
			DP.Add("Title", Title);
			DP.Add("Field", FiledId);
			DP.Add("Status", Status);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByFiledDetailGetAllOnPage>("TK_ListRecommendationByField_Detail", DP)).ToList();
		}
	}

	public class StatisticRecommendationByGroupWordGetAllOnPage
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByGroupWordGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByGroupWordGetAllOnPage()
		{
		}
		public int GroupWordId { get; set; }
		public string GroupWordName { get; set; }
		public int? UnitId { get; set; }
		public double? Total { get; set; }

		public async Task<List<StatisticRecommendationByGroupWordGetAllOnPage>> StatisticRecommendationByGroupWordGetAllOnPageDAO(string LtsUnitId, DateTime? FromDate, DateTime? ToDate)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("LtsUnitId", LtsUnitId);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByGroupWordGetAllOnPage>("[TK_ListRecommendationByGroupWord]", DP)).ToList();
		}
	}

	public class StatisticRecommendationByGroupWordDetail
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationByGroupWordDetail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByGroupWordDetail()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }

		public async Task<List<StatisticRecommendationByGroupWordDetail>> StatisticRecommendationByGroupWordDetailDAO(string Code, string SendName, string Title, string Content, int? UnitId, int? GroupWordId, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("SendName", SendName);
			DP.Add("Title", Title);
			DP.Add("Content", Content);
			DP.Add("UnitId", UnitId);
			DP.Add("GroupWordId", GroupWordId);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByGroupWordDetail>("TK_ListRecommendationByGroupWord_Detail", DP)).ToList();
		}
	}

	public class PU_Statistic
	{
		private SQLCon _sQLCon;

		public string Title { get; set; }
		public int? Value { get; set; }
		public PU_Statistic(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public PU_Statistic()
		{
		}

		public async Task<List<PU_Statistic>> StatisticByProvinceDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<PU_Statistic>("TK_RecommendationByProvince", DP)).ToList();
		}
		public async Task<List<PU_Statistic>> StatisticSatisfactionDAO()
		{
			DynamicParameters DP = new DynamicParameters();

			return (await _sQLCon.ExecuteListDapperAsync<PU_Statistic>("[TK_RecommendationBySatisfaction]", DP)).ToList();
		}
	}

	public class StatisticByByUnitParent
	{
		private SQLCon _sQLCon;
		public int Index { get; set; }
		public int Id { get; set; }
		public int UnitLevel { get; set; }
		public bool ExistChild { get; set; }
		public string UnitName { get; set; }
		public int? TotalResult { get; set; }
		public int? ReceiveApproved { get; set; }
		public int? Finised { get; set; }
		public int? Processing { get; set; }
		public int? Expired { get; set; }
		public int? Satisfaction { get; set; }
		public int? Accept { get; set; }
		public int? UnSatisfaction { get; set; }
		public StatisticByByUnitParent(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticByByUnitParent()
		{
		}

		public async Task<List<StatisticByByUnitParent>> StatisticByUnitParentDAO(int? ParentId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("ParentId", ParentId);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticByByUnitParent>("TK_RecommendationPublicByUnit", DP)).ToList();
		}
	}

	public class RecommendationStatisticForChart
	{
		public string label { get; set; }
		public List<int?> data { get; set; }

		public RecommendationStatisticForChart(string label, List<int?> data)
		{
			this.label = label;
			this.data = data;
		}
		public RecommendationStatisticForChart() { }
	}

	public class StatisticRecommendationProcessStatus
	{
		private SQLCon _sQLCon;
		public long Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public int? UnitId { get; set; }
		public string Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime ReceivedDate { get; set; }
		public int? Status { get; set; }
		public long? RowNumber { get; set; }
		public string FieldName { get; set; }
		public string UnitName { get; set; }
		public DateTime? ConclusionDate { get; set; }
		public string UserProcessName { get; set; }
		public string UnitProcessName { get; set; }
		public string Phone { get; set; }
		public StatisticRecommendationProcessStatus(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationProcessStatus()
		{
		}

		public async Task<List<StatisticRecommendationProcessStatus>> StatisticRecommendationProcessStatusDAO(DateTime? FromDate, DateTime? ToDate, int? UnitId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("UnitProcessId", UnitId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationProcessStatus>("[TK_RecommendationProcessStatus]", DP)).ToList();
		}
	}

	public class StatisticRecommendationProcessResults
	{
		private SQLCon _sQLCon;
		public int Id { get; set; }
		public string UnitName { get; set; }
		public int RecommendationDVC { get; set; }
		public int RecommendationKTXH { get; set; }
		public int? Total { get; set; }
		public int? ProcessOnTime { get; set; }
		public int? ProcessExpire { get; set; }
		public int? Process { get; set; }
		public int? FinisedOnTime { get; set; }
		public int? FinisedExpire { get; set; }
		public int? Finised { get; set; }
		public int? Satisfaction { get; set; }
		public int? Accept { get; set; }
		public int? UnSatisfaction { get; set; }
		public int? SatisfactionTotal { get; set; }
		public StatisticRecommendationProcessResults(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationProcessResults()
		{
		}

		public async Task<List<StatisticRecommendationProcessResults>> StatisticRecommendationProcessResultsDAO(DateTime? FromDate, DateTime? ToDate, int? UnitId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("UnitId", UnitId);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationProcessResults>("[TK_RecommendationProcessResults]", DP)).ToList();
		}
	}

	public class StatisticRecommendationProcessStatusByReceptionType
	{
		private SQLCon _sQLCon;
		public int? STT { get; set; }
		public int? Id { get; set; }
		public string Name { get; set; }
		public int? Received { get; set; }
		public int? RecommendationMobile { get; set; }
		public int? RecommendationEmail { get; set; }
		public int? RecommendationWeb { get; set; }
		public int? RecommendationApp { get; set; }
		public int? ProcessedTotal { get; set; }
		public int? ProcessedOnTime { get; set; }
		public int? ProcessedOutOfDate { get; set; }
		public int? ProcessTotal { get; set; }
		public int? ProcessOnTime { get; set; }
		public int? ProcessOutOfDate { get; set; }
		public int? Evaluate { get; set; }
		public int? Like { get; set; }
		public int? Dislike { get; set; }
		public int? Accept { get; set; }
		public int? PublicQuantity { get; set; }
		public long? RowNumber { get; set; }
		
		public StatisticRecommendationProcessStatusByReceptionType(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationProcessStatusByReceptionType()
		{
		}

		public async Task<List<StatisticRecommendationProcessStatusByReceptionType>> StatisticRecommendationProcessStatusByFeildAndReceptionDAO(DateTime? FromDate, DateTime? ToDate, int? UnitId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("UnitProcessId", UnitId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationProcessStatusByReceptionType>("[TK_RecommendationProcessByFeildAndReception]", DP)).ToList();
		}
		public async Task<List<StatisticRecommendationProcessStatusByReceptionType>> StatisticRecommendationProcessStatusByUnitAndReceptionDAO(DateTime? FromDate, DateTime? ToDate, int? UnitId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("UnitProcessId", UnitId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationProcessStatusByReceptionType>("[TK_RecommendationProcessByUnitAndReception]", DP)).ToList();
		}
	}

	public class StatisticRecommendationProcessStatusByFeildOrUnit
	{
		private SQLCon _sQLCon;
		public int? STT { get; set; }
		public string Name { get; set; }
		public int Id { get; set; }
		public int? Received { get; set; }
		public int? RecommendationPublicService { get; set; }
		public int? RecommendationSocioeconomic { get; set; }
		public int? ProcessedTotal { get; set; }
		public int? ProcessedOnTime { get; set; }
		public int? ProcessedOutOfDate { get; set; }
		public int? ProcessTotal { get; set; }
		public int? ProcessOnTime { get; set; }
		public int? ProcessOutOfDate { get; set; }
		public int? Evaluate { get; set; }
		public int? Like { get; set; }
		public int? Dislike { get; set; }
		public int? Accept { get; set; }
		public int? PublicQuantity { get; set; }
		public long? RowNumber { get; set; }

		public StatisticRecommendationProcessStatusByFeildOrUnit(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationProcessStatusByFeildOrUnit()
		{
		}

		public async Task<List<StatisticRecommendationProcessStatusByFeildOrUnit>> StatisticRecommendationProcessStatusByFeildDAO(DateTime? FromDate, DateTime? ToDate, int? UnitId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("UnitProcessId", UnitId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationProcessStatusByFeildOrUnit>("[TK_RecommendationProcessByFeild]", DP)).ToList();
		}
		public async Task<List<StatisticRecommendationProcessStatusByFeildOrUnit>> StatisticRecommendationProcessStatusByUnitDAO(DateTime? FromDate, DateTime? ToDate, int? UnitId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("UnitProcessId", UnitId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationProcessStatusByFeildOrUnit>("[TK_RecommendationProcessByUnit]", DP)).ToList();
		}
	}

	public class StatisticRecommendationForMenu
	{
		private SQLCon _sQLCon;

		public StatisticRecommendationForMenu(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationForMenu()
		{
		}
		public int Total { get; set; }
		public int Created { get; set; } //1
		public int ReceiveWait { get; set; } // 2
		public int ReceiveApproved { get; set; } // 3
		public int ReceiveDeny { get; set; } // 4
		public int ProcessWait { get; set; } // 5
		public int ProcessDeny { get; set; } // 6
		public int Processing { get; set; } // 7
		public int ApproveWait { get; set; } // 8
		public int ApproveDeny { get; set; } // 9

		public int Finised { get; set; }

		public async Task<StatisticRecommendationForMenu> StatisticRecommendationForMenuDAO(int UnitProcessId, long UserProcessId)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);

			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationForMenu>("[TK_RecommendationByUnitForMenu]", DP)).FirstOrDefault();
		}
	}

	public class StatisticRecommendationByRecommendationTypeDetail
	{
		private SQLCon _sQLCon;

		public int? RowNumber { get; set; }
		public int RecommendationId { get; set; }
		public string Code { get; set; }
		public string UserSendName { get; set; }
		public string Title { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public int? Type { get; set; }
		public int? UnitId { get; set; }
		public int? FieldId { get; set; }


		public StatisticRecommendationByRecommendationTypeDetail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByRecommendationTypeDetail()
		{
		}

		public async Task<List<StatisticRecommendationByRecommendationTypeDetail>> StatisticRecommendationByRecommendationTypeAndFieldDetailDAO(int? FieldId, int? UnitId, int? RecommendationType, string Code, string Name, string Title, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FieldId", FieldId);
			DP.Add("UnitId", UnitId);
			DP.Add("RecommendationType", RecommendationType);
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Title", Title);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByRecommendationTypeDetail>("[TK_ListRecommendationByRecommendationTypeAndFieldDetail]", DP)).ToList();
		}

		public async Task<List<StatisticRecommendationByRecommendationTypeDetail>> StatisticRecommendationByRecommendationTypeAndUnitProcessDetailDAO(int? FieldId, int? UnitProcess, int? RecommendationType, string Code, string Name, string Title, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitProcess", UnitProcess);
			DP.Add("FieldId", FieldId);
			DP.Add("RecommendationType", RecommendationType);
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Title", Title);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByRecommendationTypeDetail>("[TK_ListRecommendationByRecommendationTypeAndUnitProcessDetail]", DP)).ToList();
		}
	}


	public class StatisticRecommendationByReceptionTypeDetail
	{
		private SQLCon _sQLCon;

		public int? RowNumber { get; set; }
		public int RecommendationId { get; set; }
		public string Code { get; set; }
		public string UserSendName { get; set; }
		public string Title { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public int? Type { get; set; }
		public int? ReceptionType { get; set; }
		public int? UnitId { get; set; }
		public int? FieldId { get; set; }


		public StatisticRecommendationByReceptionTypeDetail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByReceptionTypeDetail()
		{
		}

		public async Task<List<StatisticRecommendationByReceptionTypeDetail>> StatisticRecommendationByReceptionTypeAndFieldDetailDAO(int? FieldId, int? UnitId, int? ReceptionType, string Code, string Name, string Title, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FieldId", FieldId);
			DP.Add("UnitId", UnitId);
			DP.Add("ReceptionType", ReceptionType);
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Title", Title);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByReceptionTypeDetail>("[TK_ListRecommendationByReceptionTypeAndFieldDetail]", DP)).ToList();
		}

		public async Task<List<StatisticRecommendationByReceptionTypeDetail>> StatisticRecommendationByReceptionTypeAndUnitProcessDetailDAO(int? FieldId, int? UnitProcess, int? ReceptionType, string Code, string Name, string Title, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitProcess", UnitProcess);
			DP.Add("FieldId", FieldId);
			DP.Add("ReceptionType", ReceptionType);
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Title", Title);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByReceptionTypeDetail>("[TK_ListRecommendationByReceptionTypeAndUnitProcessDetail]", DP)).ToList();
		}
	}

	public class StatisticRecommendationByResultDetail
	{
		private SQLCon _sQLCon;

		public int? RowNumber { get; set; }
		public int RecommendationId { get; set; }
		public string Code { get; set; }
		public string UserSendName { get; set; }
		public string Title { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public int? Type { get; set; }
		public int? ReceptionType { get; set; }
		public int? UnitId { get; set; }
		public int? FieldId { get; set; }


		public StatisticRecommendationByResultDetail(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public StatisticRecommendationByResultDetail()
		{
		}

		public async Task<List<StatisticRecommendationByResultDetail>> StatisticRecommendationByResultAndFieldDetailDAO(int? FieldId, int? UnitId, int? Status, bool? IsOnTime, string Code, string Name, string Title, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("FieldId", FieldId);
			DP.Add("UnitId", UnitId);
			DP.Add("Status", Status);
			DP.Add("IsOnTime", IsOnTime);
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Title", Title);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByResultDetail>("[TK_ListRecommendationByResultAndFieldDetail]", DP)).ToList();
		}

		public async Task<List<StatisticRecommendationByResultDetail>> StatisticRecommendationByResultAndUnitProcessDetailDAO(int? FieldId, int? UnitProcess, int? Status, bool? IsOnTime, string Code, string Name, string Title, DateTime? FromDate, DateTime? ToDate, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("UnitProcess", UnitProcess);
			DP.Add("FieldId", FieldId);
			DP.Add("Status", Status);
			DP.Add("IsOnTime", IsOnTime);
			DP.Add("Code", Code);
			DP.Add("Name", Name);
			DP.Add("Title", Title);
			DP.Add("FromDate", FromDate);
			DP.Add("ToDate", ToDate);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			return (await _sQLCon.ExecuteListDapperAsync<StatisticRecommendationByResultDetail>("[TK_ListRecommendationByResultAndUnitProcessDetail]", DP)).ToList();
		}
	}
}
