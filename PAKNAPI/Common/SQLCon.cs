using System;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PAKNAPI.Common
{
	public class SQLCon
	{
		private static string _connectString;

		public SQLCon(string connectString)
		{
			_connectString = connectString;
		}

		public SQLCon()
		{
		}

		public static SqlConnection GetConnection()
		{
			return new SqlConnection(_connectString);
		}

		#region Static functions

		public static int ExecuteNonQueryStringStatic(string sqlText, SqlParameter[] param = null)
		{
			if (param == null)
			{
				param = new SqlParameter[] { };
			}

			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(sqlText, conn))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddRange(param);

				conn.Open();
				return cmd.ExecuteNonQuery();
			}
		}

		public static DataTable ExecuteDataTableStatic(string commandText, SqlParameter[] param, CommandType cmdType = CommandType.StoredProcedure)
		{
			DataTable dt = new DataTable();

			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(commandText, conn))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				cmd.CommandType = cmdType;
				cmd.Parameters.AddRange(param);

				conn.Open();
				da.Fill(dt);
				return dt;
			}
		}

		public static IEnumerable<T> ExecuteListDapperStatic<T>(string commandText, DynamicParameters DP, CommandType cmdType = CommandType.StoredProcedure)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				IEnumerable<T> result = conn.Query<T>(commandText, DP, commandType: cmdType);
				return result;
			}
		}

		public static IEnumerable<T> ExecuteListDapperStaticText<T>(string commandText, DynamicParameters DP, CommandType cmdType = CommandType.Text)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return conn.Query<T>(commandText, DP, commandType: cmdType);
			}
		}

		#endregion Static Functions

		public T ExecuteScalar<T>(string sqlText, params SqlParameter[] param)
		{
			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(sqlText, conn))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddRange(param);

				conn.Open();
				return (T)cmd.ExecuteScalar();
			}
		}

		public int ExecuteNonQueryString(string sqlText, params SqlParameter[] param)
		{
			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(sqlText, conn))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddRange(param);

				conn.Open();
				return cmd.ExecuteNonQuery();
			}
		}

		public void ExecuteNonQueryProc(string ProcedureName, SqlParameter[] param)
		{
			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(ProcedureName, conn))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddRange(param);

				conn.Open();
				cmd.ExecuteNonQuery();
			}
		}

		public DataTable ExecuteDataTable(string ProcedureName, SqlParameter[] param)
		{
			DataTable dt = new DataTable();

			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(ProcedureName, conn))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddRange(param);

				conn.Open();
				da.Fill(dt);
				return dt;
			}
		}

		public DataTable ExecuteDataTableText(string query, SqlParameter[] param)
		{
			DataTable dt = new DataTable();

			using (SqlConnection conn = GetConnection())
			using (SqlCommand cmd = new SqlCommand(query, conn))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddRange(param);

				conn.Open();
				da.Fill(dt);
				return dt;
			}
		}

		public int ExecuteNonQueryDapper(string ProcedureName, DynamicParameters DP)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return conn.Execute(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			}
		}

		public async Task<int> ExecuteNonQueryDapperAsync(string ProcedureName, DynamicParameters DP)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return await conn.ExecuteAsync(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			}
		}

		public T ExecuteScalarDapper<T>(string ProcedureName, DynamicParameters DP)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return (T)conn.ExecuteScalar(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			}
		}

		public async Task<T> ExecuteScalarDapperAsync<T>(string ProcedureName, DynamicParameters DP)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return (T)await conn.ExecuteScalarAsync(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			}
		}

		public T ExecuteScalarDapper<T>(string query)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return (T)conn.ExecuteScalar(query, commandType: CommandType.Text);
			}
		}

		public IEnumerable<T> ExecuteListDapper<T>(string ProcedureName, DynamicParameters DP)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return conn.Query<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			}
		}

		public async Task<IEnumerable<T>> ExecuteListDapperAsync<T>(string ProcedureName, DynamicParameters DP)
		{
			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return await conn.QueryAsync<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			}
		}

		public IEnumerable<T> ExecuteListDapperText<T>(string query, DynamicParameters DP = null)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return conn.Query<T>(query, DP, commandType: CommandType.Text);
			}
		}

		public async Task<IEnumerable<T>> ExecuteListDapperTextAsync<T>(string query, DynamicParameters DP = null)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();
				return await conn.QueryAsync<T>(query, DP, commandType: CommandType.Text);
			}
		}

		public T ExecuteDapper<T>(string query, DynamicParameters DP = null)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				List<T> list = conn.Query<T>(query, DP, commandType: CommandType.Text).ToList();

				return list.Count > 0 ? list[0] : default(T);
			}
		}

		public async Task<T> ExecuteDapperAsync<T>(string query, DynamicParameters DP = null)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				List<T> list = (await conn.QueryAsync<T>(query, DP, commandType: CommandType.Text)).ToList();

				return list.Count > 0 ? list[0] : default(T);
			}
		}

		public T GetFirstOrDefaultEntityDapper<T>(string ProcedureName, DynamicParameters DP)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				List<T> list = conn.Query<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure).ToList();

				return list.Count > 0 ? list[0] : default(T);
			}
		}

		public async Task<T> GetFirstOrDefaultEntityDapperAsync<T>(string ProcedureName, DynamicParameters DP)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				List<T> list = (await conn.QueryAsync<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure)).ToList();

				return list.Count > 0 ? list[0] : default(T);
			}
		}

		public async Task<T> GetFirstOrDefaultEntityDapperTextAsync<T>(string query, DynamicParameters DP) where T : new()
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			using (SqlConnection conn = GetConnection())
			{
				conn.Open();

				List<T> list = (await conn.QueryAsync<T>(query, DP, commandType: CommandType.Text)).ToList();

				return list.Count > 0 ? list[0] : new T();
			}
		}


		//public async Task<T> GetListQueryGroupDapperAsync<T>(string query, DynamicParameters DP, List<T> list, T object2) where T : new()
		//{
		//	//List<object1> sYFunction1s = new List<object1>();
		//	using (SqlConnection conn = GetConnection())
		//	{
		//		conn.Open();

		//		(await conn.QueryAsync<T, T, T>(query, (sYFunction1, sYFunctionAction1) =>
		//		{
		//			var _sYFunction1 = list.Find(item => item.Id == sYFunction1.Id);
		//			if (_sYFunction1 == null) { list.Add(sYFunction1); _sYFunction1 = sYFunction1; }
		//			_sYFunction1.sYFunctionAction1s = _sYFunction1.sYFunctionAction1s ?? new List<T>();
		//			if (sYFunctionAction1 != null)
		//				_sYFunction1.sYFunctionAction1s.Add(sYFunctionAction1);
		//			return _sYFunction1;
		//		})).ToList();
		//	}
		//}
	}
}