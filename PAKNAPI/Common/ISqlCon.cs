using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace PAKNAPI.Commons.SQL
{
	public interface ISqlCon
	{
		SqlConnection Connection { get; set; }

		SqlCommand Command { get; set; }
		SqlTransaction Transaction { get; set; }

		SqlConnection GetConnection();

		SqlTransaction BeginTran(string transactionname);
		void Commit();
		void Rollback();

		#region SqlCommand methods
		T ExecuteScalar<T>(string query, CommandType cmdType = CommandType.StoredProcedure, params SqlParameter[] param);

		int ExecuteNonQuery(string query, CommandType cmdType = CommandType.StoredProcedure, params SqlParameter[] param);

		#endregion SqlCommand methods

		#region Dapper methods
		DynamicParameters DapperParams(params KeyValuePair<string, object>[] parameters);
		T ExecuteScalarDapper<T>(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null);
		T ExecuteScalarTextDapper<T>(string query, DynamicParameters dp = null);
		T ExecuteScalarProcDapper<T>(string query, DynamicParameters dp = null);

		IEnumerable<T> ExecuteListDapper<T>(string sqlText, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null);
		IEnumerable<T> ExecuteTextListDapper<T>(string sqlText, DynamicParameters dp = null);
		IEnumerable<T> ExecuteProcListDapper<T>(string sqlText, DynamicParameters dp = null);

		int Execute(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null);

		int ExecuteText(string query, DynamicParameters dp = null);
		int ExecuteProc(string query, DynamicParameters dp = null);
		/// <summary>
		/// ExecuteNonQueryAsync -- Dapper
		/// </summary>
		/// <param name="query"></param>
		/// <param name="cmdType"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		Task<int> ExecuteAsync(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null);

		Task<int> ExecuteTextAsync(string query, DynamicParameters dp = null);
		Task<int> ExecuteProcAsync(string query, DynamicParameters dp = null);

		/// <summary>
		/// Dapper
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <param name="cmdType"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		Task<T> ExecuteScalarAsync<T>(string query, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null);
		Task<T> ExecuteScalarTextAsync<T>(string query, DynamicParameters dp = null);
		Task<T> ExecuteScalarProcAsync<T>(string proc, DynamicParameters dp = null);
		/// <summary>
		/// Dapper
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sqlText"></param>
		/// <param name="cmdType"></param>
		/// <param name="dp"></param>
		/// <returns></returns>
		Task<IEnumerable<T>> ExecuteListAsync<T>(string sqlText, CommandType cmdType = CommandType.StoredProcedure, DynamicParameters dp = null);
		Task<IEnumerable<T>> ExecuteTextListAsync<T>(string sqlText, DynamicParameters dp = null);
		Task<IEnumerable<T>> ExecuteProcListAsync<T>(string sqlText, DynamicParameters dp = null);

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
		Task<IEnumerable<TReturn>> QueryAsyncTwoTypes<TFirst, TSecond, TReturn>(string sqlText, CommandType? cmdType, Func<TFirst, TSecond, TReturn> map, string splitOn, DynamicParameters dp = null);

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
		Task<IEnumerable<TReturn>> QueryTextAsyncTwoTypes<TFirst, TSecond, TReturn>(string sqlText, Func<TFirst, TSecond, TReturn> map, string splitOn, DynamicParameters dp = null);

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
		Task<IEnumerable<TReturn>> QueryProcAsyncTwoTypes<TFirst, TSecond, TReturn>(string sqlText, Func<TFirst, TSecond, TReturn> map, string splitOn, DynamicParameters dp = null);

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
		Task<IEnumerable<TReturn>> QueryAsyncThreeTypes<TFirst, TSecond, TThird, TReturn>(string sqlText, CommandType? cmdType, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters dp = null);

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
		Task<IEnumerable<TReturn>> QueryTextAsyncThreeTypes<TFirst, TSecond, TThird, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters dp = null);

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
		Task<IEnumerable<TReturn>> QueryProcAsyncThreeTypes<TFirst, TSecond, TThird, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn, DynamicParameters dp = null);

		Task<IEnumerable<TReturn>> QueryAsyncFourTypes<TFirst, TSecond, TThird, TFourth, TReturn>(string sqlText, CommandType? cmdType, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters dp = null);
		Task<IEnumerable<TReturn>> QueryTextAsyncFourTypes<TFirst, TSecond, TThird, TFourth, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters dp = null);
		Task<IEnumerable<TReturn>> QueryProcAsyncFourTypes<TFirst, TSecond, TThird, TFourth, TReturn>(string sqlText, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn, DynamicParameters dp = null);
		#endregion Dapper methods
		int ExecuteNonQueryDapper(string ProcedureName, DynamicParameters DP);
		Task<int> ExecuteNonQueryDapperAsync(string ProcedureName, DynamicParameters DP);
		T ExecuteScalarDapper<T>(string ProcedureName, DynamicParameters DP);
		Task<T> ExecuteScalarDapperAsync<T>(string ProcedureName, DynamicParameters DP);
		IEnumerable<T> ExecuteListDapper<T>(string ProcedureName, DynamicParameters DP);
		Task<IEnumerable<T>> ExecuteListDapperAsync<T>(string ProcedureName, DynamicParameters DP);
		T GetFirstOrDefaultEntityDapper<T>(string ProcedureName, DynamicParameters DP);
		Task<T> GetFirstOrDefaultEntityDapperAsync<T>(string ProcedureName, DynamicParameters DP);
		Dapper.SqlMapper.GridReader GetQueryMultipleAsync(string ProcedureName, DynamicParameters DP);
	}
}
