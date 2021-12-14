using Dapper;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Data.SqlClient;

namespace PAKNAPI.Commons.SQL
{
	public class ESConnection : ISqlCon
	{
		public string _connectionString;
		public SqlConnection Connection { get; set; }

		public SqlCommand Command { get; set; }

		public SqlTransaction Transaction { get; set; }

		public ESConnection(string connectionString)
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Generate a Dapper DynamicParameters object from a list of KeyValuePair object
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public DynamicParameters DapperParams(params KeyValuePair<string, object>[] parameters)
		{
			var result = new DynamicParameters();
			foreach (var item in parameters)
			{
				result.Add(item.Key, item.Value);
			}
			return result;
		}
		public SqlConnection GetConnection()
		{
			if (Connection == null)
			{
				Connection = new SqlConnection(_connectionString);
			}
			if (Connection.State != ConnectionState.Open)
			{
				Connection.Close();
				Connection.Open();
			}
			return Connection;
		}
		public SqlTransaction BeginTran(string transactionName)
		{
			Connection = GetConnection();
			this.Transaction = Connection.BeginTransaction(transactionName);
			return this.Transaction;
		}
		public void Commit()
		{
			Connection = GetConnection();
			Transaction.Commit();
			Transaction.Dispose();
			Transaction = null;
			Connection.Close();
		}
		public void Rollback()
		{
			Connection = GetConnection();
			Transaction.Rollback();
			Transaction.Dispose();
			Transaction = null;
			Connection.Close();
		}

		#region SqlCommand methods
		public T ExecuteScalar<T>(string query, CommandType cmdType = CommandType.StoredProcedure, params SqlParameter[] param)
		{
			Command = new SqlCommand
			{
				Connection = GetConnection(),
				CommandText = query,
				CommandType = CommandType.Text,
				Transaction = this.Transaction
			};
			Command.Parameters.AddRange(param);
			object result = Command.ExecuteScalar();
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return (T)result;
		}

		public int ExecuteNonQuery(string query, CommandType cmdType = CommandType.StoredProcedure, params SqlParameter[] param)
		{
			Command = new SqlCommand
			{
				Connection = GetConnection(),
				CommandText = query,
				CommandType = cmdType,
				Transaction = this.Transaction
			};
			Command.Parameters.AddRange(param);
			int i = Command.ExecuteNonQuery();
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return i;
		}
		#endregion SqlCommand methods

		#region Dapper methods
		public T ExecuteScalarDapper<T>(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null)
		{
			if (dp == null)
			{
				dp = new DynamicParameters();
			}
			GetConnection();
			object data = Connection.ExecuteScalar(query, dp, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return (T)data;
		}
		public T ExecuteScalarTextDapper<T>(string query, DynamicParameters dp = null)
		{
			return ExecuteScalarDapper<T>(query, CommandType.Text, dp);
		}
		public T ExecuteScalarProcDapper<T>(string query, DynamicParameters dp = null)
		{
			return ExecuteScalarDapper<T>(query, CommandType.StoredProcedure, dp);
		}

		public IEnumerable<T> ExecuteListDapper<T>(string sqlText, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null)
		{
			if (dp == null)
			{
				dp = new DynamicParameters();
			}
			GetConnection();
			try
			{
				IEnumerable<T> result = Connection.Query<T>(sqlText, dp, commandType: cmdType, transaction: this.Transaction);

				if (this.Transaction == null)
				{
					Connection.Close();
				}

				return result;
			}
			catch
			{
				return new List<T>();
			}
		}
		public IEnumerable<T> ExecuteTextListDapper<T>(string sqlText, DynamicParameters dp = null)
		{
			return ExecuteListDapper<T>(sqlText, CommandType.Text, dp);
		}
		public IEnumerable<T> ExecuteProcListDapper<T>(string sqlText, DynamicParameters dp = null)
		{
			return ExecuteListDapper<T>(sqlText, CommandType.StoredProcedure, dp);
		}

		/// <summary>
		/// ExecuteNonQueryAsync
		/// </summary>
		/// <param name="query"></param>
		/// <param name="cmdType"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		public async Task<int> ExecuteAsync(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null)
		{
			Connection = GetConnection();
			int i = await Connection.ExecuteAsync(query, dp, commandType: cmdType, transaction: Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return i;
		}

		/// <summary>
		/// /// Overload of ExecuteAsync
		/// </summary>
		/// <param name="query"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		public async Task<int> ExecuteTextAsync(string query, DynamicParameters dp = null)
		{
			return await this.ExecuteAsync(query, CommandType.Text, dp);
		}

		/// <summary>
		/// Overload of ExecuteAsync
		/// </summary>
		/// <param name="query"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		public async Task<int> ExecuteProcAsync(string query, DynamicParameters dp = null)
		{
			return await this.ExecuteAsync(query, CommandType.StoredProcedure, dp);
		}

		public async Task<T> ExecuteScalarAsync<T>(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null)
		{
			GetConnection();
			T data = await Connection.ExecuteScalarAsync<T>(query, dp, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return data;
		}
		public async Task<T> ExecuteScalarTextAsync<T>(string query, DynamicParameters dp = null)
		{
			return await ExecuteScalarAsync<T>(query, CommandType.Text, dp);
		}
		public async Task<T> ExecuteScalarProcAsync<T>(string query, DynamicParameters dp = null)
		{
			return await ExecuteScalarAsync<T>(query, CommandType.StoredProcedure, dp);
		}

		public async Task<IEnumerable<T>> ExecuteListAsync<T>(string sqlText, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null)
		{
			GetConnection();
			IEnumerable<T> result = await Connection.QueryAsync<T>(sqlText, dp, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return result;
		}
		/// <summary>
		/// Overload of ExecuteListAsync
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sqlText"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		public async Task<IEnumerable<T>> ExecuteTextListAsync<T>(string sqlText, DynamicParameters dp = null)
		{
			return await this.ExecuteListAsync<T>(sqlText, CommandType.Text, dp);
		}
		/// <summary>
		/// Overload of ExecuteListAsync
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sqlText"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		public async Task<IEnumerable<T>> ExecuteProcListAsync<T>(string sqlText, DynamicParameters dp = null)
		{
			return await this.ExecuteListAsync<T>(sqlText, CommandType.StoredProcedure, dp);
		}

		public async Task<IEnumerable<T>> ExecuteProcListIsolationAsync<T>(string ProcedureName, DynamicParameters DP, ESConnection con)
		{
			SqlConnection sql = new SqlConnection(con._connectionString);
			IEnumerable<T> List = await sql.QueryAsync<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			if (this.Transaction == null)
			{
				sql.Close();
			}
			return List;
		}

		public async Task<IEnumerable<TReturn>> QueryAsyncTwoTypes<TFirst, TSecond, TReturn>(string sqlText, CommandType? cmdType, Func<TFirst, TSecond, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			GetConnection();
			IEnumerable<TReturn> result = await Connection.QueryAsync<TFirst, TSecond, TReturn>(sqlText, map, dp, splitOn: splitOn, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return result;
		}

		public async Task<IEnumerable<TReturn>> QueryTextAsyncTwoTypes<TFirst, TSecond, TReturn>(string sqlText, Func<TFirst, TSecond, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			return await QueryAsyncTwoTypes(sqlText, CommandType.Text, map, splitOn, dp);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TFirst"></typeparam>
		/// <typeparam name="TSecond"></typeparam>
		/// <typeparam name="TReturn">The return type</typeparam>
		/// <param name="sqlText"></param>
		/// <param name="map">mapping method, use to map the selected data to the correct object</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id")</param>
		/// <param name="dp"></param>
		/// <returns></returns>
		public async Task<IEnumerable<TReturn>> QueryProcAsyncTwoTypes<TFirst, TSecond, TReturn>(string sqlText, Func<TFirst, TSecond, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			return await QueryAsyncTwoTypes(sqlText, CommandType.StoredProcedure, map, splitOn, dp);
		}

		public async Task<IEnumerable<TReturn>> QueryAsyncThreeTypes<TFirst, TSecond, TThird, TReturn>(string sqlText, CommandType? cmdType, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			GetConnection();
			IEnumerable<TReturn> result = await Connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(sqlText, map, dp, splitOn: splitOn, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return result;
		}

		public async Task<IEnumerable<TReturn>> QueryTextAsyncThreeTypes<TFirst, TSecond, TThird, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			return await QueryAsyncThreeTypes(sqlText, CommandType.Text, map, splitOn, dp);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TFirst"></typeparam>
		/// <typeparam name="TSecond"></typeparam>
		/// <typeparam name="TReturn">The return type</typeparam>
		/// <param name="sqlText"></param>
		/// <param name="map">mapping method, use to map the selected data to the correct object</param>
		/// <param name="splitOn">The field we should split and read the second object from (default: "Id")</param>
		/// <param name="dp"></param>
		/// <returns></returns>
		public async Task<IEnumerable<TReturn>> QueryProcAsyncThreeTypes<TFirst, TSecond, TThird, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			return await QueryAsyncThreeTypes(sqlText, CommandType.StoredProcedure, map, splitOn, dp);
		}

		public int Execute(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null)
		{
			Connection = GetConnection();
			int i = Connection.Execute(query, dp, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return i;
		}

		public int ExecuteText(string query, DynamicParameters dp = null)
		{
			return this.Execute(query, CommandType.Text, dp);
		}

		public int ExecuteProc(string query, DynamicParameters dp = null)
		{
			return this.Execute(query, CommandType.StoredProcedure, dp);
		}

		public async Task<IEnumerable<TReturn>> QueryAsyncFourTypes<TFirst, TSecond, TThird, TFourth, TReturn>(string sqlText, CommandType? cmdType, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			GetConnection();
			IEnumerable<TReturn> result = await Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(sqlText, map, dp, splitOn: splitOn, commandType: cmdType, transaction: this.Transaction);
			if (this.Transaction == null)
			{
				Connection.Close();
			}
			return result;
		}

		public async Task<IEnumerable<TReturn>> QueryTextAsyncFourTypes<TFirst, TSecond, TThird, TFourth, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			return await QueryAsyncFourTypes(sqlText, CommandType.Text, map, splitOn, dp);
		}

		public async Task<IEnumerable<TReturn>> QueryProcAsyncFourTypes<TFirst, TSecond, TThird, TFourth, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters dp = null)
		{
			return await QueryAsyncFourTypes(sqlText, CommandType.StoredProcedure, map, splitOn, dp);
		}
		#endregion Dapper methods

		public int ExecuteNonQueryDapper(string ProcedureName, DynamicParameters DP)
		{
			GetConnection();
			int recUpdated = Connection.Execute(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			Connection.Close();
			return recUpdated;
		}


		public async Task<int> ExecuteNonQueryDapperAsync(string ProcedureName, DynamicParameters DP)
		{
			GetConnection();
			int recUpdated = await Connection.ExecuteAsync(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			Connection.Close();
			return recUpdated;
		}

		public T ExecuteScalarDapper<T>(string ProcedureName, DynamicParameters DP)
		{
			GetConnection();
			object data = Connection.ExecuteScalar(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			Connection.Close();
			return (T)data;
		}

		public async Task<T> ExecuteScalarDapperAsync<T>(string ProcedureName, DynamicParameters DP)
		{
			GetConnection();
			object data = await Connection.ExecuteScalarAsync(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			Connection.Close();
			return (T)data;
		}

		public IEnumerable<T> ExecuteListDapper<T>(string ProcedureName, DynamicParameters DP)
		{
			GetConnection();
			IEnumerable<T> List = Connection.Query<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			Connection.Close();
			return List;
		}

		public IEnumerable<T> ExecuteListIsolationDapper<T>(string ProcedureName, DynamicParameters DP, ESConnection con)
		{
			SqlConnection sql = new SqlConnection(con._connectionString);
			IEnumerable<T> List = sql.Query<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			if (this.Transaction == null)
			{
				sql.Close();
			}
			return List;
		}

		public async Task<IEnumerable<T>> ExecuteListDapperAsync<T>(string ProcedureName, DynamicParameters DP)
		{
			GetConnection();
			IEnumerable<T> List = await Connection.QueryAsync<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure);
			Connection.Close();
			return List;
		}

		public T GetFirstOrDefaultEntityDapper<T>(string ProcedureName, DynamicParameters DP)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			GetConnection();
			List<T> list = Connection.Query<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure).ToList();
			Connection.Close();

			return list.Count > 0 ? list[0] : default(T);
		}

		public async Task<T> GetFirstOrDefaultEntityDapperAsync<T>(string ProcedureName, DynamicParameters DP)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			GetConnection();
			List<T> list = (await Connection.QueryAsync<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure)).ToList();
			Connection.Close();

			return list.Count > 0 ? list[0] : default(T);
		}


		public async Task<T> GetFirstOrDefaultEntityIsolationDapperAsync<T>(string ProcedureName, DynamicParameters DP, ESConnection con)
		{
			SqlConnection sql = new SqlConnection(con._connectionString);
			List<T> list = (await sql.QueryAsync<T>(ProcedureName, DP, commandType: CommandType.StoredProcedure)).ToList();
			if (this.Transaction == null)
			{
				sql.Close();
			}
			return list.Count > 0 ? list[0] : default(T);
		}
		public SqlMapper.GridReader GetQueryMultipleAsync(string ProcedureName, DynamicParameters DP)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}
			GetConnection();
			SqlMapper.GridReader grid = (Connection.QueryMultiple(ProcedureName, DP, commandType: CommandType.StoredProcedure));
			//Connection.Close();
			return grid;
		}

		public async Task<IEnumerable<TReturn>> GetListThreeType<TFirst, TSecond, TThird, TReturn>(string proc, System.Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters DP = null)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			GetConnection();

			IEnumerable<TReturn> result = await Connection.QueryAsync<TFirst, TSecond, TThird, TReturn>(proc, map, DP, splitOn: splitOn, commandType: CommandType.StoredProcedure);

			return result;


		}

		public async Task<IEnumerable<TReturn>> GetListFourType<TFirst, TSecond, TThird, TFourth, TReturn>(string proc, System.Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters DP = null)
		{
			if (DP == null)
			{
				DP = new DynamicParameters();
			}

			GetConnection();

			IEnumerable<TReturn> result = await Connection.QueryAsync<TFirst, TSecond, TThird, TFourth, TReturn>(proc, map, DP, splitOn: splitOn, commandType: CommandType.StoredProcedure);

			return result;
		}
	}
}