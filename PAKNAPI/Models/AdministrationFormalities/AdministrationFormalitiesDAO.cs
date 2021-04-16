using Dapper;
using PAKNAPI.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using PAKNAPI.ModelBase;

namespace PAKNAPI.Models.AdministrationFormalities
{

	public class AdministrationFormalitiesDAO
    {
		private SQLCon _sQLCon;
		public AdministrationFormalitiesDAO(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public async Task<AdministrationGetById> AdministrationFormalitiesGetByID(int? Id)
		{
			AdministrationGetById administration = new AdministrationGetById();
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", Id);
			administration.Data = (await _sQLCon.ExecuteListDapperAsync<DAMAdministrationGetById>("DAM_Administration_GetById", DP)).FirstOrDefault();
			administration.Files = (await _sQLCon.ExecuteListDapperAsync<DAMFileObj>("[DAM_Administration_FilesGetByAdministrationId]", DP)).ToList();
			administration.LstCompositionProfile = (await _sQLCon.ExecuteListDapperAsync<DAMCompositionProfileCreateObj>("[DAM_CompositionProfile_GetByAdministration]", DP)).ToList();
			administration.LstCharges = (await _sQLCon.ExecuteListDapperAsync<DAMChargesGetById>("[DAM_Charges_GetByAdministration]", DP)).ToList();
			administration.LstImplementationProcess = (await _sQLCon.ExecuteListDapperAsync<DAMImplementationProcessUpdateIN>("[DAM_ImplementationProcess_GetByAdministration]", DP)).ToList();

            if (administration.LstCompositionProfile != null && administration.LstCompositionProfile.Count > 0)
            {
                for (int i = 0; i < administration.LstCompositionProfile.Count(); i++)
                {
					var item = administration.LstCompositionProfile[i];
					DP = new DynamicParameters();
					DP.Add("Id", item.Id);
					item.Files = (await _sQLCon.ExecuteListDapperAsync<DAMCompositionProfileObj>("DAM_CompositionProfile_File_FilesGetByCompositionProfileId", DP)).ToList();

				}
            }
			Base64EncryptDecryptFile decrypt = new Base64EncryptDecryptFile();
			foreach (var item in administration.Files)
			{
				item.FileAttach = decrypt.EncryptData(item.FileAttach);
			}
			return administration;
		}
	}
}
