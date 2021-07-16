//using Dapper;
//using PAKNAPI.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace PAKNAPI.Models.ModelBase
//{
//	public class DAMAdministrationGetListHomePage
//	{
//		private SQLCon _sQLCon;

//		public DAMAdministrationGetListHomePage(IAppSetting appSetting)
//		{
//			_sQLCon = new SQLCon(appSetting.GetConnectstring());
//		}

//		public DAMAdministrationGetListHomePage()
//		{
//		}

//		public int? RowNumber { get; set; }
//		public int Id { get; set; }
//		public string Code { get; set; }
//		public string Name { get; set; }
//		public string Object { get; set; }
//		public string Organization { get; set; }
//		public byte? Status { get; set; }
//		public string FiledName { get; set; }

//		public async Task<List<DAMAdministrationGetListHomePage>> DAMAdministrationGetListHomePageDAO()
//		{
//			DynamicParameters DP = new DynamicParameters();

//			return (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationGetListHomePage>("DAM_AdministrationGetListHomePage", DP)).ToList();
//		}
//	}
//}
