using Dapper;
using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Models.Results
{
    public class PermissionDAO
    {
        public static SQLCon _sQLCon ;
        const string Letters = "12346789ABCDEFGHJKLMNPRTUVWXYZ";

        public PermissionDAO(IAppSetting appSetting)
        {
            _sQLCon = new SQLCon(appSetting.GetConnectstring());
        }
        public async Task<List<PermissionCategoryObject>> GetListPermission()
        {
            try
            {
                DynamicParameters DP = new DynamicParameters();
                List<PermissionCategoryObject> lstData = new List<PermissionCategoryObject>();
                lstData = (await _sQLCon.ExecuteListDapperAsync<PermissionCategoryObject>("SY_PermissionCategory_Get", DP)).ToList();
                foreach (var cat in lstData)
                {
                    DP = new DynamicParameters();
                    DP.Add("Id", cat.Id);
                    cat.Function = (await _sQLCon.ExecuteListDapperAsync<FunctionObject>("SY_PermissionFunction_GetByCategory", DP)).ToList();
                    foreach (var per in cat.Function)
                    {
                        DP = new DynamicParameters();
                        DP.Add("Id", per.Id);
                        per.Permission = (await _sQLCon.ExecuteListDapperAsync<PermissionObject>("SY_Permission_GetByFunction", DP)).ToList();
                    }
                }
                return lstData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    public class PermissionCategoryObject
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Selected { get; set; }

        public ICollection<FunctionObject> Function { get; set; }
    }
    public class FunctionObject
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public short CategoryId { get; set; }

        public bool Selected { get; set; }

        public ICollection<PermissionObject> Permission { get; set; }
    }
    public class PermissionObject
    {
        public PermissionObject()
        {
            CheckedKeys = new List<int>();
        }
        public short Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public short CategoryId { get; set; }
        public short? ParentId { get; set; }

        public bool Selected { get; set; }
        public List<int> CheckedKeys { get; set; }
    }
}
