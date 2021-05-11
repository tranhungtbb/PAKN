using Dapper;
using PAKNAPI.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.BusinessIndividual
{
    #region IndividualGetAllOnPage
    public class BusinessGetAllOnPage
	{
		private SQLCon _sQLCon;

		public BusinessGetAllOnPage(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public BusinessGetAllOnPage()
		{
		}

		public int? RowNumber { get; set; }
		public int Id { get; set; }
		public string RepresentativeName { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public string Email { get; set; }
		public bool IsActived { get; set; }
		public bool IsDeleted { get; set; }

		public async Task<List<BusinessGetAllOnPage>> BusinessGetAllOnPageDAO(int? PageSize, int? PageIndex, string RepresentativeName, string Address, string Phone, string Email, bool? IsActived, string SortDir, string SortField)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("PageSize", PageSize);
			DP.Add("PageIndex", PageIndex);
			DP.Add("RepresentativeName", RepresentativeName);
			DP.Add("Address", Address);
			DP.Add("Phone", Phone);
			DP.Add("Email", Email);
			DP.Add("IsActived", IsActived);
			DP.Add("SortDir", SortDir);
			DP.Add("SortField", SortField);

			return (await _sQLCon.ExecuteListDapperAsync<BusinessGetAllOnPage>("BI_BusinessGetAllOnPage", DP)).ToList();
		}
	}
	#endregion
}
