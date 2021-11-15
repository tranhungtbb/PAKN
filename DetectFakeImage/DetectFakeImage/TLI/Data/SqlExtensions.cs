using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace TLI.Data
{
    public class ColumnAttribute : Attribute
    {
		public string Name { get; }
    }

	public class Dictionary : Dictionary<string, object> { }
    public class ListDictionary : List<Dictionary>
    {
		public ListDictionary():base() { }
		public ListDictionary(IEnumerable<Dictionary> collection) : base(collection) { }
		public ListDictionary(int capacity) : base(capacity) { }
	}

	public static class SqlExtentsion
	{
		#region SqlParameterCollection
		public delegate bool SetParamCallback(string name);

		public static SqlParameterCollection Add(this SqlParameterCollection sqlParams, string name, object value)
		{
			sqlParams.AddWithValue(name, value);
			return sqlParams;
		}
		public static void AddArray(this SqlCommand cmd, string name, params object[] values)
		{
			var parameterNames = new List<string>();
			for (int i = 0; i < values.Count(); i++)
			{
				var paramName = $"@{name}__{i}";
				cmd.Parameters.AddWithValue(paramName, values.ElementAt(i));
				parameterNames.Add(paramName);
			}

			cmd.CommandText = cmd.CommandText.Replace(name, string.Join(",", parameterNames));
		}
		///<inheritdoc cref = "SQLConn" />
		public static SqlParameterCollection AddSerializable(this SqlParameterCollection sqlParams, object objectParam, SetParamCallback callback = null)
		{
			if (objectParam == null)
				return sqlParams;

			Type tType = objectParam.GetType();
			var ps = tType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach (var p in ps)
			{
				string name = p.Name;
				if (callback == null || (callback != null && callback.Invoke(name)))
					sqlParams.AddWithValue(name, p.GetValue(objectParam, null));
			}

			var fs = tType.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
			foreach (var p in fs)
			{
				string name = p.Name;
				if (callback == null || (callback != null && callback.Invoke(name)))
					sqlParams.AddWithValue(name, p.GetValue(objectParam));
			}
			return sqlParams;
		}
		#endregion

		#region SqlCommand
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetCommandText(this SqlConnection conn, string sqlText, object objParam = null, params SqlParameter[] param)
		{
			SqlCommand cmd = new SqlCommand(sqlText, conn);
			cmd.CommandType = CommandType.Text;
			if(param!= null)
			cmd.Parameters.AddRange(param);
			if(objParam!= null)
			cmd.Parameters.Add(objParam);
			return cmd;
		}

		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetCommandProc(this SqlConnection conn, string sqlText, object objectParam = null, params SqlParameter[] param)
		{
			SqlCommand cmd = new SqlCommand(sqlText, conn);
			cmd.CommandType = CommandType.StoredProcedure;
			if(param!= null)
			cmd.Parameters.AddRange(param);
			if(objectParam!= null)
			cmd.Parameters.Add(objectParam);
			return cmd;
		}


        public static T ExecuteScalar<T>(this SqlCommand cmd)
        {
            return (T)cmd.ExecuteScalar();
        }

        public static int ExecuteNonQuery<T>(this SqlCommand cmd)
		{
			return cmd.ExecuteNonQuery();
		}
		public static Task<int> ExecuteNonQueryAsync(this SqlCommand cmd)
		{
			return cmd.ExecuteNonQueryAsync();
		}
		///<inheritdoc cref = "SQLConn" />
		public static T GetRow<T>(SqlDataReader dr)
		{
			T obj = default(T);
			obj = Activator.CreateInstance<T>();
			foreach (var p in obj.GetType().GetProperties())
			{
				try
				{
					string name = p.Name;
					if (!object.Equals(dr[name], DBNull.Value))
					{
						p.SetValue(obj, dr[name], null);
					}
				}
				catch(Exception)
                {

                }
			}
			foreach (var p in obj.GetType().GetFields())
			{
				try
				{
					string name = p.Name;
					if (!object.Equals(dr[name], DBNull.Value))
					{
						p.SetValue(obj, dr[name]);
					}
				}
                catch (Exception)
                {

                }
			}
			return obj;
		}
		public static Dictionary GetRow(SqlDataReader dr)
		{
			var dic = new Dictionary();
			if (dr.Read())
			{
				for (int i = 0; i < dr.FieldCount; i++)
				{
					var v = dr.GetValue(i);
					if (!object.Equals(v, DBNull.Value))
						dic.Add(dr.GetName(i), v);
				}
			}
			dr.Close();
			return dic;
		}

		///<inheritdoc cref = "SQLConn" />
		public static T ExecuteRow<T>(this SqlCommand cmd)
		{
			return GetRow<T>(cmd.ExecuteReader());
		}

		///<inheritdoc cref = "SQLConn" />
		public static List<T> ExecuteList<T>(this SqlCommand cmd)
		{
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				var list = new List<T>();
				while (dr.Read())
				{

					list.Add(GetRow<T>(dr));
				}
				return list;
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static ListDictionary ExecuteList(this SqlCommand cmd)
		{
			using (SqlDataReader dr = cmd.ExecuteReader())
			{
				var list = new ListDictionary();
				while (dr.Read())
				{
					var dic = new Dictionary();
					for (int i = 0; i < dr.FieldCount; i++)
					{
						dic.Add(dr.GetName(i), dr.GetValue(i));
					}
					list.Add(dic);
				}
				return list;
			}
		}

		///<inheritdoc cref = "SQLConn" />
		public static DataTable ExecuteDataTable(this SqlCommand cmd)
		{
			DataTable dt = new DataTable();

			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				da.Fill(dt);
				return dt;
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static DataTable ExecuteDataTableAsync(this SqlCommand cmd)
		{
			DataTable dt = new DataTable();

			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				da.Fill(dt);
				return dt;
			}
		}

		#endregion


		#region Excute command text
		///<inheritdoc cref = "SQLConn" />
		public static  T ExecuteScalar<T>(this SqlConnection conn, string sqlText, object objectParam)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, objectParam))
			{
				//conn.Open();
				return (T)cmd.ExecuteScalar();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  T ExecuteScalar<T>(this SqlConnection conn, string sqlText, params SqlParameter[] param)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, null, param))
			{
				//conn.Open();
				return (T)cmd.ExecuteScalar();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  int ExecuteNonQuery(this SqlConnection conn, string sqlText, object objectParam= null)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, objectParam))
			{
				//conn.Open();
				return cmd.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  int ExecuteNonQuery(this SqlConnection conn, string sqlText, params SqlParameter[] param)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, null, param))
			{
				//conn.Open();
				return cmd.ExecuteNonQuery();
			}
		}

		///<inheritdoc cref = "SQLConn" />
		public static  T ExecuteRow<T>(this SqlConnection conn, string sqlText, object objectParam)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, objectParam))
			{
				//conn.Open();
				return GetRow<T>(cmd.ExecuteReader());
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  T ExecuteRow<T>(this SqlConnection conn, string sqlText, params SqlParameter[] param)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, null, param))
			{
				//conn.Open();
				return GetRow<T>(cmd.ExecuteReader());
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  ListDictionary ExecuteList(this SqlConnection conn, string sqlText, object objectParam)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, objectParam))
			{
				//conn.Open();
				return ExecuteList(cmd);
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  IEnumerable<Dictionary> ExecuteList(this SqlConnection conn, string sqlText, params SqlParameter[] param)
		{
			using (SqlCommand cmd = GetCommandText(conn, sqlText, null, param))
			{
				return ExecuteList(conn, sqlText, param);
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  IEnumerable<T> ExecuteList<T>(this SqlConnection conn, string sqlText, object objectParam)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, objectParam))
			{
				//conn.Open();
				return ExecuteList<T>(cmd);
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  List<T> ExecuteList<T>(this SqlConnection conn, string sqlText, params SqlParameter[] param)
		{
			
			using (SqlCommand cmd = GetCommandText(conn, sqlText, null, param))
			{
				//conn.Open();
				return ExecuteList<T>(cmd);
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  DataTable ExecuteDataTable(this SqlConnection conn, string query, object objectParam)
		{
			DataTable dt = new DataTable();

			
			using (SqlCommand cmd = GetCommandText(conn, query, objectParam))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				//conn.Open();
				da.Fill(dt);
				return dt;
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  DataTable ExecuteDataTable(this SqlConnection conn, string query, params SqlParameter[] param)
		{
			DataTable dt = new DataTable();

			
			using (SqlCommand cmd = GetCommandText(conn, query, null, param))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				//conn.Open();
				da.Fill(dt);
				return dt;
			}
		}

		#endregion

		#region Excute Procedure
		///<inheritdoc cref = "SQLConn" />
		public static  IEnumerable<Dictionary> ExecuteListProc(this SqlConnection conn, string ProcedureName, object objectParam)
		{
			
			using (SqlCommand cmd = GetCommandProc(conn, ProcedureName, objectParam))
			{
				return ExecuteList(cmd);
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  IEnumerable<Dictionary> ExecuteListProc(this SqlConnection conn, string ProcedureName, params SqlParameter[] objectParam)
		{
			
			using (SqlCommand cmd = GetCommandProc(conn, ProcedureName, objectParam))
			{
				//conn.Open();
				return ExecuteList(cmd);
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  void ExecuteNonQueryProc(this SqlConnection conn, string ProcedureName, object objectParam)
		{
			
			using (SqlCommand cmd = GetCommandProc(conn, ProcedureName, objectParam))
			{
				//conn.Open();
				cmd.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static int ExecuteNonQueryProc(this SqlConnection conn, string ProcedureName, params SqlParameter[] param)
		{
			
			using (SqlCommand cmd = GetCommandProc(conn, ProcedureName, null, param))
			{
                return cmd.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  DataTable ExecuteDataTableProc(this SqlConnection conn, string ProcedureName, object objectParam)
		{
			DataTable dt = new DataTable();

			
			using (SqlCommand cmd = GetCommandProc(conn, ProcedureName, objectParam))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				//conn.Open();
				da.Fill(dt);
				return dt;
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static  DataTable ExecuteDataTableProc(this SqlConnection conn, string ProcedureName, params SqlParameter[] param)
		{
			DataTable dt = new DataTable();

			
			using (SqlCommand cmd = GetCommandProc(conn, ProcedureName, null, param))
			using (SqlDataAdapter da = new SqlDataAdapter(cmd))
			{
				//conn.Open();
				da.Fill(dt);
				return dt;
			}
		}

		#endregion

		#region Select
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetSelectCommand(this SqlConnection conn, string tableName, bool distinct, string cols = "*", string wheres = "", object whereArgs = null, SqlParameter[] whereParams = null, int limit = 0, int offset = 0)
		{
			string sql = "SELECT " + (distinct ? "DISTINCT " : "") + cols + " FROM " + tableName
				+ (string.IsNullOrEmpty(wheres) ? "" : (" WHERE " + wheres))
				+ (limit > 0 ? (" OFFSET @offset ROWS FETCH NEXT @limit ROWS ONLY") : "");

			var cmd = GetCommandText(conn, sql, whereArgs, whereParams);
			if (limit > 0)
			{
				cmd.Parameters.AddWithValue("limit", limit);
				cmd.Parameters.AddWithValue("offset", offset);
			}
			return cmd;
		}


		///<inheritdoc cref = "SQLConn" />
		public static IEnumerable<T> Select<T>(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, int limit = 0, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectCommand(conn, tableName, false, cols, wheres, whereArgs, null, limit, offset))
			{
				//conn.Open();
				return ExecuteList<T>(cmd);
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static ListDictionary Select(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, int limit = 0, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectCommand(conn, tableName, false, cols, wheres, whereArgs, null, limit, offset))
			{
				//conn.Open();
				return ExecuteList(cmd);
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static IEnumerable<T> Select<T>(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", SqlParameter[] whereArgs = null, int limit = 0, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectCommand(conn, tableName, false, cols, wheres, null, whereArgs, limit, offset))
			{
				//conn.Open();
				return ExecuteList<T>(cmd);
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static ListDictionary Select(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", SqlParameter[] whereArgs = null, int limit = 0, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectCommand(conn, tableName, false, cols, wheres, null, whereArgs, limit, offset))
			{
				//conn.Open();
				return ExecuteList(cmd);
			}
		}
		#endregion

		#region SelectDistinct
		///<inheritdoc cref = "SQLConn" />
		public static IEnumerable<T> SelectDistinct<T>(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, int limit = 0, int offset = 0)
		{

			using (SqlCommand cmd = GetSelectCommand(conn, tableName, true, cols, wheres, whereArgs, null, limit, offset))
			{
				//conn.Open();
				return ExecuteList<T>(cmd);
			}
		}
		public static ListDictionary SelectDistinct(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, int limit = 0, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectCommand(conn, tableName, true, cols, wheres, whereArgs, null, limit, offset))
			{
				//conn.Open();
				return ExecuteList(cmd);
			}
		}


		// use SqlParameters
		///<inheritdoc cref = "SQLConn" />
		public static IEnumerable<T> SelectDistinct<T>(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", SqlParameter[] whereArgs = null, int limit = 0, int offset = 0)
		{

			using (SqlCommand cmd = GetSelectCommand(conn, tableName, true, cols, wheres, whereArgs, null, limit, offset))
			{
				//conn.Open();
				return ExecuteList<T>(cmd);
			}
		}
		public static ListDictionary SelectDistinct(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", SqlParameter[] whereArgs = null, int limit = 0, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectCommand(conn, tableName, true, cols, wheres, whereArgs, null, limit, offset))
			{
				//conn.Open();
				return ExecuteList(cmd);
			}
		}
		#endregion

		#region SelectRow
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetSelectRowCommand(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, SqlParameter[] whereParams = null, int offset = 0)
		{
			string sql = "SELECT " + cols + " FROM " + tableName
				+ (string.IsNullOrEmpty(wheres) ? "" : (" WHERE " + wheres));

			var cmd = GetCommandText(conn, sql, whereArgs, whereParams);
			if (offset > 0)
			{
				cmd.CommandText += " OFFSET @offset ROWS FETCH NEXT 1 ROWS ONLY";
				cmd.Parameters.AddWithValue("offset", offset);
			}
			return cmd;
		}


		///<inheritdoc cref = "SQLConn" />
		public static T SelectRow<T>(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, int offset = 0)
		{

			using (SqlCommand cmd = GetSelectRowCommand(conn, tableName, cols, wheres, whereArgs, null, offset))
			{
				//conn.Open();
				return GetRow<T>(cmd.ExecuteReader());
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static Dictionary SelectRow(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", object whereArgs = null, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectRowCommand(conn, tableName, cols, wheres, whereArgs, null, offset))
			{
				//conn.Open();
				return GetRow(cmd.ExecuteReader());
			}
		}

		// user SqlParameter[]
		///<inheritdoc cref = "SQLConn" />
		public static T SelectRow<T>(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", SqlParameter[] whereArgs = null, int offset = 0)
		{

			using (SqlCommand cmd = GetSelectRowCommand(conn, tableName, cols, wheres, whereArgs, null, offset))
			{
				//conn.Open();
				return GetRow<T>(cmd.ExecuteReader());
			}
		}

		///<inheritdoc cref = "SQLConn" />
		public static Dictionary SelectRow(this SqlConnection conn, string tableName, string cols = "*", string wheres = "", SqlParameter[] whereArgs = null, int offset = 0)
		{
			using (SqlCommand cmd = GetSelectRowCommand(conn, tableName, cols, wheres, whereArgs, null, offset))
			{
				//conn.Open();
				return GetRow(cmd.ExecuteReader());
			}
		}
		#endregion

		#region Select scalar
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetSelectScalarCommand(this SqlConnection conn, string tableName, string col, string wheres = "", object whereArgs = null, params SqlParameter[] whereParams)
		{
			string sql = "SELECT top 1 " + col + " FROM " + tableName
				+ (string.IsNullOrEmpty(wheres) ? "" : (" WHERE " + wheres));

			return GetCommandText(conn, sql, whereArgs, whereParams);
		}


		///<inheritdoc cref = "SQLConn" />
		public static object SelectScalar(this SqlConnection conn, string tableName, string col, string wheres = "", object whereArgs = null)
		{

			using (SqlCommand cmd = GetSelectScalarCommand(conn, tableName, col, wheres, whereArgs))
			{
				//conn.Open();
				return cmd.ExecuteScalar();
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static T SelectScalar<T>(this SqlConnection conn, string tableName, string col, string wheres = "", object whereArgs = null)
		{
			return (T)SelectScalar(conn, tableName, col, wheres, whereArgs);
		}

		// SqlParameter[]
		///<inheritdoc cref = "SQLConn" />
		public static object SelectScalar(this SqlConnection conn, string tableName, string col, string wheres = "", params SqlParameter[] whereArgs)
		{
			using (SqlCommand cmd = GetSelectScalarCommand(conn, tableName, col, wheres,null, whereArgs))
			{
				//conn.Open();
				return cmd.ExecuteScalar();
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static T SelectScalar<T>(this SqlConnection conn, string tableName, string col, string wheres = "", params SqlParameter[] whereArgs) where T: IConvertible
		{
			var rs= SelectScalar(conn, tableName, col, wheres, whereArgs);
			return rs == null ? default(T) : (T)rs;
		}
		#endregion

		#region Select count
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetSelectCountCommand(this SqlConnection conn, string tableName, string wheres = "", object whereArgs = null, params SqlParameter[] param)
		{
			string sql = "SELECT count(1) FROM " + tableName
			+ (string.IsNullOrEmpty(wheres) ? "" : (" WHERE " + wheres));
			return GetCommandText(conn, sql, whereArgs, param);
		}

		///<inheritdoc cref = "SQLConn" />
		public static int SelectCount(this SqlConnection conn, string tableName, string wheres = "", object whereArgs = null)
		{
			using (var c = GetSelectCountCommand(conn, tableName, wheres, whereArgs))
			{
				//conn.Open();
				return (int)c.ExecuteScalar();
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static int SelectCount(this SqlConnection conn, string tableName, string wheres = "", params SqlParameter[] whereParams)
		{
			using (var c = GetSelectCountCommand(conn, tableName, wheres, null, whereParams))
			{
				//conn.Open();
				return (int)c.ExecuteScalar();
			}
		}
		#endregion

		#region Insert
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetInsertCommandText(this SqlConnection conn, string tableName, object objParam = null, SqlParameter[] param = null, params string[] skipParams)
		{
			SqlCommand sc = new SqlCommand();
			sc.Connection = conn;
			sc.CommandType = CommandType.Text;

			var _colNames = new List<string>();
			var _paramNames = new List< string >();
			AddSerializable(sc.Parameters, objParam, name =>
			{
				if (!skipParams.Contains(name))
				{
					_colNames.Add(name);
					_paramNames.Add("@" + name);
					return true;
				}
				return false;
			});

			if (param != null)
			{
				foreach (var p in param)
				{
					string name = p.ParameterName.StartsWith("@") ? p.ParameterName.Substring(1) : p.ParameterName;
					_colNames.Add(name);
					_paramNames.Add("@" + name);
					sc.Parameters.AddWithValue("@" + name, p.Value);
				}
			}

			sc.CommandText = "INSERT INTO " + tableName + "(" + string.Join(", ", _colNames) + ") VALUES (" + string.Join(", ", _paramNames) + ");"
				+ " SELECT SCOPE_IDENTITY();";

			return sc;
		}

		///<inheritdoc cref = "SQLConn" />
		public static long Insert(this SqlConnection conn, string tableName, object objParam, params string[] skipParams)
		{
			using (var c = GetInsertCommandText(conn, tableName, objParam, null, skipParams))
			{
				//conn.Open();
				object rs = c.ExecuteScalar();
				return rs == null ? 0 : long.Parse(rs.ToString());
			}
		}


		///<inheritdoc cref = "SQLConn" />
		public static long Insert(this SqlConnection conn, string tableName, params SqlParameter[] param)
		{

			using (var c = GetInsertCommandText(conn, tableName, null, param))
			{
				//conn.Open();
				object rs = c.ExecuteScalar();
				return rs == null ? 0 : long.Parse(rs.ToString());
			}
		}
		#endregion

		#region Update
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetUpdateCommandText(this SqlConnection conn, string tableName, object objParam = null, SqlParameter[] dataParam = null, string wheres = "", object whereArgs = null, SqlParameter[] whereParams = null, params string[] skipParams)
		{
			SqlCommand sc = new SqlCommand();
			sc.Connection = conn;
			sc.CommandType = CommandType.Text;

			var _sets = new List<string>();
			sc.Parameters.AddSerializable(objParam, name =>
			{
				if (!skipParams.Contains(name))
				{
					_sets.Add(name + "= @" + name);
					return true;
				}
				return false;
			});

			if (dataParam != null)
			{
				foreach (var p in dataParam)
				{
					string name = p.ParameterName.StartsWith("@") ? p.ParameterName.Substring(1) : p.ParameterName;
					_sets.Add(name + "= @" + name);
					sc.Parameters.AddWithValue("@" + name, p.Value);
				}
			}

			sc.CommandText = "UPDATE " + tableName + " SET " + string.Join(", ", _sets);

			if (!string.IsNullOrEmpty(wheres))
			{
				sc.CommandText += " WHERE " + wheres;
				sc.Parameters.AddSerializable(whereArgs);
				if (whereParams != null)
				{
					foreach (var p in dataParam)
					{
						string name = p.ParameterName.StartsWith("@") ? p.ParameterName.Substring(1) : p.ParameterName;
						sc.Parameters.AddWithValue("@" + name, p.Value);
					}
				}
			}
			return sc;
		}

		///<inheritdoc cref = "SQLConn" />
		public static int Update(this SqlConnection conn, string tableName, object objParam, string wheres = "", object whereArgs = null, params string[] skipParams)
		{

			using (var c = GetUpdateCommandText(conn, tableName, objParam, null, wheres, whereArgs, null, skipParams))
			{
				//conn.Open();
				return c.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static Task<int> UpdateAsync(this SqlConnection conn, string tableName, object objParam, string wheres = "", object whereArgs = null, params string[] skipParams)
		{

			using (var c = GetUpdateCommandText(conn, tableName, objParam, null, wheres, whereArgs, null, skipParams))
			{
				//conn.Open();
				return c.ExecuteNonQueryAsync();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static int Update(this SqlConnection conn, string tableName, object objParam, string wheres = "", params SqlParameter[] whereParams)
		{

			using (var c = GetUpdateCommandText(conn, tableName, objParam, null, wheres, null, whereParams))
			{
				//conn.Open();
				return c.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static Task<int> UpdateAsync(this SqlConnection conn, string tableName, object objParam, string wheres = "", params SqlParameter[] whereParams)
		{

			using (var c = GetUpdateCommandText(conn, tableName, objParam, null, wheres, null, whereParams))
			{
				//conn.Open();
				return c.ExecuteNonQueryAsync();
			}
		}
		#endregion

		#region Delete
		///<inheritdoc cref = "SQLConn" />
		public static SqlCommand GetDeleteCommandText(this SqlConnection conn, string tableName, string wheres = "", object whereArgs = null, params SqlParameter[] whereParams)
		{
			string sql = "DELETE FROM " + tableName + (string.IsNullOrEmpty(wheres) ? "" : (" WHERE " + wheres));
			return GetCommandText(conn, sql, whereArgs);
		}
		///<inheritdoc cref = "SQLConn" />
		public static int Delete(this SqlConnection conn, string tableName, string wheres = "", object whereArgs = null)
		{

			using (var c = GetDeleteCommandText(conn, tableName, wheres, whereArgs))
			{
				//conn.Open();
				return c.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static Task<int> DeleteAsync(this SqlConnection conn, string tableName, string wheres = "", object whereArgs = null)
		{

			using (var c = GetDeleteCommandText(conn, tableName, wheres, whereArgs))
			{
				//conn.Open();
				return c.ExecuteNonQueryAsync();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static int Delete(this SqlConnection conn, string tableName, string wheres = "", params SqlParameter[] whereParams)
		{

			using (var c = GetDeleteCommandText(conn, tableName, wheres, null, whereParams))
			{
				//conn.Open();
				return c.ExecuteNonQuery();
			}
		}
		///<inheritdoc cref = "SQLConn" />
		public static Task<int> DeleteAsync(this SqlConnection conn, string tableName, string wheres = "", params SqlParameter[] whereParams)
		{

			using (var c = GetDeleteCommandText(conn, tableName, wheres, null, whereParams))
			{
				//conn.Open();
				return c.ExecuteNonQueryAsync();
			}
		}
		#endregion

		#region Exist
		public static bool Exist(this SqlConnection conn, string tableName, string wheres, object whereArgs = null)
		{
			return SelectScalar<int>(conn, tableName, "1", wheres, whereArgs) == 1;
		}
		public static bool Exist(this SqlConnection conn, string tableName, string wheres, params SqlParameter[] whereParams)
		{
			return SelectScalar<int>(conn, tableName, "1", wheres, whereParams) == 1;
		}
		#endregion

		
	}
}
