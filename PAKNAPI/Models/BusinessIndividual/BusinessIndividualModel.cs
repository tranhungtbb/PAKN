using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.BusinessIndividual
{
	#region IndividualGetAllOnPage
	public class IndividualGetAllOnPage
	{
		private SQLCon _sQLCon;

		public IndividualGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public IndividualGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string FullName { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<IndividualGetAllOnPage>> IndividualGetAllOnPageDAO2(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, bool? IsActived)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FullName", FullName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("IsActived", IsActived);

			return (await _sQLCon.ExecuteListDapperAsync<IndividualGetAllOnPage>("BI_IndividualGetAllOnPage", DP)).ToList();
		}

		public async Task<List<IndividualGetAllOnPage>> IndividualGetAllOnPageDAO(int? PageSize, int? PageIndex, string FullName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("FullName", FullName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("IsActived", IsActived);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

			return (await _sQLCon.ExecuteListDapperAsync<IndividualGetAllOnPage>("BI_IndividualGetAllOnPage", DP)).ToList();
		}
	}
	#endregion


	public class BusinessIndividualGetDataForCreateResponse
    {
        public string Code { get; set; }
        public List<DropdownTree> lstUnit { get; set; }
        public List<DropdownObject> lstField { get; set; }
        public List<DropdownObject> lstIndividual { get; set; }
        public List<DropdownObject> lstBusiness { get; set; }
        public List<DropdownObject> lstHashTag { get; set; }
    }

    public class BusinessIndividualGetDataForForwardResponse
    {
        public List<DropdownObject> lstUnitNotMain { get; set; }
    }

    public class BusinessIndividualGetDataForProcessResponse
    {
        public List<DropdownObject> lstHashtag { get; set; }
        public List<DropdownObject> lstUsers { get; set; }
        public List<DropdownObject> lstGroupWord { get; set; }
    }
    public class BusinessIndividualInsertRequest
    {
        public long? UserId { get; set; }
        public int? UserType { get; set; }
        public string UserFullName { get; set; }
        //public MRRecommendationInsertIN Data { get; set; }
        public List<DropdownObject> ListHashTag { get; set; }
        //public List<MRRecommendationFiles> LstXoaFile { get; set; }
        //public IFormFileCollection Files { get; set; }
    }

	public class BusinessIndividualGetAllWithProcess
	{
		private SQLCon _sQLCon;

		public BusinessIndividualGetAllWithProcess(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessIndividualGetAllWithProcess()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int? Field { get; set; }
		public string FieldName { get; set; }
		public int? UnitId { get; set; }
		public string UnitName { get; set; }
		public short? TypeObject { get; set; }
		public long? SendId { get; set; }
		public string Name { get; set; }
		public byte? Status { get; set; }
		public DateTime? SendDate { get; set; }
		public long? CreatedBy { get; set; }
		public DateTime? CreatedDate { get; set; }
		public long? UpdatedBy { get; set; }
		public DateTime? UpdatedDate { get; set; }
		public int? ProcessId { get; set; }
		public int? UnitSendId { get; set; }
		public long? UserSendId { get; set; }
		public long? ReceiveId { get; set; }
		public int? UnitReceiveId { get; set; }

		public async Task<List<BusinessIndividualGetAllWithProcess>> BusinessIndividualGetAllWithProcessDAO(string Code, string SendName, string Content, int? UnitId, int? Field, int? Status, int? UnitProcessId, long? UserProcessId, int? PageSize, int? PageIndex)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Code", Code);
			DP.Add("SendName", SendName);
			DP.Add("Content", Content);
			DP.Add("UnitId", UnitId);
			DP.Add("Field", Field);
			DP.Add("Status", Status);
			DP.Add("UnitProcessId", UnitProcessId);
			DP.Add("UserProcessId", UserProcessId);
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);

			return (await _sQLCon.ExecuteListDapperAsync<BusinessIndividualGetAllWithProcess>("MR_RecommendationGetAllWithProcess", DP)).ToList();
		}
	}


}
