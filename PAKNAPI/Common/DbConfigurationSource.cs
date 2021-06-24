using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAKNAPI.Common
{
    public class DbConfigModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class DbConfigurationSource : ConfigurationProvider
    {
        private readonly string _connectionString;
        private SQLCon _sQLCon;

        public DbConfigurationSource(string connectionString)
        {
            _connectionString = connectionString;
            _sQLCon = new SQLCon(_connectionString);
        }

        public override void Load()
        {
            DynamicParameters DP = new DynamicParameters();
            var list = _sQLCon.ExecuteListDapper<DbConfigModel>("[Sy_ConfigurationKeyVal]", DP);
            Data = list.ToDictionary(c => c.Key, c => c.Value);
            //Data = list.Any()
            //    ? list
            //    : CreateAndSaveDefaultValues(dbContext);
        }

    }
}
