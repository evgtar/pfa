using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using PFA.Core;

namespace EvgTar.PFA.Core
{
    public class MSSQLServerHelper: IDBHelper
    {

        public string LastError { get; set; } = "";
        public string LastSQL { get; private set; } = "";
        public string ConnectionString { get; set; } = "";
        public string SQL { get; set; } = "";
        public int TimeOut { get; set; } = 30;
        public bool Debug = false;
        public bool Connected { get; private set; } = false;
        public string LastMessage { get; private set; } = "";
        public Serilog.Core.Logger Logger { get; set; }

        public MSSQLServerHelper(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                Connected = true;
            }
            else
            {
                ConnectionString = connectionString;
                try
                {
                    SqlConnection connect = new SqlConnection(ConnectionString);
                    connect.Open();
                    Connected = (connect.State == ConnectionState.Open);
                    connect.Close();
                    connect.Dispose();
                }
                catch (Exception ex)
                {
                    LastMessage = ex.Message;
                    LastError = ex.ToString();
                    //LastException = ex;
                    Connected = false;
                }
            }
        }
        public MSSQLServerHelper(bool CheckConnection = false)
        {
            if (CheckConnection) { }
            else
                Connected = true;
            //ConnectionString = connectionString;
            //try
            //{
            //    SqlConnection connect = new SqlConnection(ConnectionString);
            //    connect.Open();
            //    Connected = (connect.State == ConnectionState.Open);
            //    connect.Close();
            //    connect.Dispose();
            //}
            //catch(Exception ex)
            //{
            //    LastMessage = ex.Message;
            //    LastError = ex.ToString();
            //    LastException = ex;
            //    Connected = false;
            //}
        }

        /// <summary>
        /// Returns quantity of records in defined table
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="where"> where case</param>
        /// <returns></returns>
        public int GetCount(string tableName, string where)
        {
            int count = -1;
            LastError = "";
            LastSQL = "";
            if (!Connected)
                return count;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    string sql = "select count(1) from " + tableName + " " + where;
                    LastSQL = sql;
                    using (SqlCommand cmd = new SqlCommand(sql, connect))
                    {
                        try
                        {
                            cmd.CommandTimeout = TimeOut;
                            count = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        catch (SqlException esql)
                        {
                            LastError = "DB Error: " + esql.Message + "\n" + sql;
                            Logger?.Error(LastError);
                        }
                        catch (Exception ex)
                        {
                            LastError = ex.Message;
                            Logger?.Error(LastError);
                        }
                    }
                    connect.Close();
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    Logger?.Error(LastError);
                }
            }
            return count;
        }
        /// <summary>
        /// Execute SQL statement and returns affected lines. Function returns -1 if it's failed
        /// </summary>
        /// <param name="sql">SQL statement</param>
        /// <returns></returns>
        public int ExecuteSQL(string sql)
        {
            int ret = -1;
            LastError = "";
            LastSQL = sql;
            if (!Connected)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(sql, connect)
                    {
                        CommandTimeout = TimeOut
                    };
                    ret = cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    LastError = $"DB Error: {e.Message}{Environment.NewLine}{sql}";
                    Logger?.Error(LastError);
                    ret = -1;
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public int ExecuteSQL(string ProcedureName, PropertyCollection parameters)
        {
            int ret = 1;
            LastSQL = ProcedureName;
            LastError = "";
            if (!Connected)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(ProcedureName, connect)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    string key_s = "";
                    foreach (object key in parameters.Keys)
                    {
                        key_s = key.ToString();
                        if (key_s.StartsWith("#OUT#"))
                        {
                            key_s = key_s.Substring(5);
                            cmd.Parameters.Add(new SqlParameter(key_s, parameters[key].ToString()));
                            cmd.Parameters[key_s].Direction = ParameterDirection.Output;
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter(key_s, parameters[key].ToString()));
                        }
                    }
                    if (!parameters.ContainsKey("RETURN_VALUE"))
                    {
                        cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                        cmd.Parameters["RETURN_VALUE"].Direction = ParameterDirection.ReturnValue;
                    }
                    cmd.ExecuteNonQuery();
                    ret = Convert.ToInt32(cmd.Parameters["RETURN_VALUE"].Value);
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + ": " + ProcedureName + "\r\n";
                    foreach (object key in parameters.Keys)
                        LastError += key.ToString() + "\t" + parameters[key].ToString() + "\r\n";
                    Logger?.Error(LastError);
                    ret = -1;
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                    ret = -1;
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public object GetValueSQL(string sql)
        {
            object ret = null;
            LastError = "";
            LastSQL = sql;
            if (!Connected)
                return ret;
            using (SqlConnection connect = new(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new(sql, connect)
                    {
                        CommandTimeout = TimeOut
                    };
                    ret = cmd.ExecuteScalar();
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + "\n" + sql;
                    Logger?.Error(LastError);
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public string GetValueSQL(string sql, string DefVal)
        {
            string ret = DefVal;
            LastError = "";
            LastSQL = sql;
            if (!Connected)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(sql, connect)
                    {
                        CommandTimeout = TimeOut
                    };
                    object v = cmd.ExecuteScalar();
                    if (v != DBNull.Value)
                        ret = v.ToString();
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + "\n" + sql;
                    Logger?.Error(LastError);
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public Int32 GetValueSQL(string sql, Int32 defVal)
        {
            int ret = defVal;
            LastError = "";
            LastSQL = sql;
            if (!Connected)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(sql, connect)
                    {
                        CommandTimeout = TimeOut
                    };
                    ret = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + "\n" + sql;
                    Logger?.Error(LastError);
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public Int64 GetValueSQL(string sql, Int64 defVal)
        {
            Int64 ret = defVal;
            LastError = "";
            LastSQL = sql;
            if (!Connected)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlCommand cmd = new SqlCommand(sql, connect) { CommandTimeout = TimeOut };
                    ret = Convert.ToInt64(cmd.ExecuteScalar());
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + "\n" + sql;
                    Logger?.Error(LastError);
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public long GetValueSQL(string ProcedureName, PropertyCollection parameters, long defVal)
        {
            long ret = defVal;
            DataSet ds = new DataSet();
            if (GetDataSet(ref ds, "GetValueSQL_RESULTS", ProcedureName, parameters))
            {
                if ((ds.Tables["GetValueSQL_RESULTS"].Rows.Count > 0) && (ds.Tables["GetValueSQL_RESULTS"].Columns.Count > 0))
                {
                    long.TryParse(ds.Tables["GetValueSQL_RESULTS"].Rows[0][0].ToString(), out ret);
                }
            }
            return ret;
        }
        /// <summary>
        /// Returns last error message
        /// </summary>
        /// <param name="error">Reference to string</param>
        public void GetLastError(ref string error)
        {
            error = LastError;
        }
        /// <summary>
        /// Returns dataset with results of execution of SQL statement
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool GetDataSet(ref DataSet results, string TableName, string sql)
        {
            bool ret = true;
            LastSQL = sql;
            if (!Connected)
                return false;

            if (results == null)
                results = new DataSet();
            TableName = TableName.Trim();
            if (!TableName.Equals(""))
            {
                if (results.Tables.Contains(TableName))
                {
                    try
                    {
                        results.Tables.Remove(TableName);
                    }
                    catch (Exception e)
                    {
                        ret = false;
                        LastError = e.Message;
                        Logger?.Error(LastError);
                    }
                }
                if (ret)
                    results.Tables.Add(new DataTable(TableName));
            }
            if (!ret)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(sql, connect);
                    if (TableName.Equals(""))
                        sda.Fill(results);
                    else
                    {
                        results.Tables[TableName].BeginLoadData();
                        sda.Fill(results, TableName);
                        results.Tables[TableName].EndLoadData();
                    }
                    sda.Dispose();
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + "\n" + sql;
                    Logger?.Error(LastError);
                    ret = false;
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                    ret = false;
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public bool GetDataSet(ref DataSet results, string TableName, string ProcedureName, PropertyCollection parameters)
        {
            bool ret = true;
            LastSQL = ProcedureName;
            if (!Connected)
                return false;
            if (results == null)
                results = new DataSet();
            TableName = TableName.Trim();
            if (!TableName.Equals(""))
            {
                if (results.Tables.Contains(TableName))
                {
                    try
                    {
                        results.Tables.Remove(TableName);
                    }
                    catch (Exception e)
                    {
                        ret = false;
                        LastError = e.Message;
                        Logger?.Error(LastError);
                    }
                }
                if (ret)
                    results.Tables.Add(new DataTable(TableName));
            }
            if (!ret)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    DataReaderAdapter da = new DataReaderAdapter();
                    SqlCommand cmd = new SqlCommand(ProcedureName, connect)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = TimeOut
                    };
                    string key_s = "";
                    foreach (object key in parameters.Keys)
                    {
                        key_s = key.ToString();
                        if (key_s.StartsWith("#OUT#"))
                        {
                            key_s = key_s.Substring(0, 5);
                            cmd.Parameters.Add(new SqlParameter(key_s, parameters[key].ToString()));
                            cmd.Parameters[key_s].Direction = ParameterDirection.Output;
                        }
                        else
                            cmd.Parameters.Add(new SqlParameter(key_s, parameters[key].ToString()));
                    }
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (TableName.Equals(""))
                    {
                        DataTable dt = new DataTable();
                        dt.BeginLoadData();
                        da.FillFromReader(dt, sdr);
                        dt.EndLoadData();
                    }
                    else
                    {
                        results.Tables[TableName].BeginLoadData();
                        da.FillFromReader(results.Tables[TableName], sdr);
                        results.Tables[TableName].EndLoadData();
                    }
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + ": " + ProcedureName + System.Environment.NewLine;
                    foreach (object key in parameters.Keys)
                        LastError += key.ToString() + "= " + parameters[key].ToString() + System.Environment.NewLine;
                    Logger?.Error(LastError);
                    ret = false;
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                    ret = false;
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public bool GetDataSet(ref DataSet results, string TableName, string ProcedureName, PropertyCollection parameters, ref PropertyCollection returns)
        {
            bool ret = true;
            LastSQL = ProcedureName;
            if (!Connected)
                return false;
            if (results == null)
                results = new DataSet();
            TableName = TableName.Trim();
            if (!TableName.Equals(""))
            {
                if (results.Tables.Contains(TableName))
                {
                    try
                    {
                        results.Tables.Remove(TableName);
                    }
                    catch (Exception e)
                    {
                        ret = false;
                        LastError = e.Message;
                        Logger?.Error(LastError);
                    }
                }
                if (ret)
                    results.Tables.Add(new DataTable(TableName));
            }
            if (!ret)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    DataReaderAdapter da = new DataReaderAdapter();
                    SqlCommand cmd = new SqlCommand(ProcedureName, connect)
                    {
                        CommandType = CommandType.StoredProcedure,
                        CommandTimeout = TimeOut
                    };
                    string key_s = "";
                    foreach (object key in parameters.Keys)
                    {
                        key_s = key.ToString();
                        if (key_s.StartsWith("#OUT#"))
                        {
                            key_s = key_s.Substring(0, 5);
                            cmd.Parameters.Add(new SqlParameter(key_s, parameters[key].ToString()));
                            cmd.Parameters[key_s].Direction = ParameterDirection.Output;
                        }
                        else
                            cmd.Parameters.Add(new SqlParameter(key_s, parameters[key].ToString()));
                    }
                    SqlDataReader sdr = cmd.ExecuteReader();
                    if (TableName.Equals(""))
                    {
                        DataTable dt = new DataTable();
                        dt.BeginLoadData();
                        da.FillFromReader(dt, sdr);
                        dt.EndLoadData();
                    }
                    else
                    {
                        results.Tables[TableName].BeginLoadData();
                        da.FillFromReader(results.Tables[TableName], sdr);
                        results.Tables[TableName].EndLoadData();
                    }
                    returns.Clear();
                    for (int i = 0; i < cmd.Parameters.Count; i++)
                    {
                        if (cmd.Parameters[i].Direction == ParameterDirection.Output)
                            returns.Add(cmd.Parameters[i].ParameterName, cmd.Parameters[i].Value);
                    }
                }
                catch (SqlException esql)
                {
                    LastError = "DB Error: " + esql.Message + ": " + ProcedureName + "\r\n";
                    foreach (object key in parameters.Keys)
                        LastError += key.ToString() + "\t" + parameters[key].ToString() + "\r\n";
                    Logger?.Error(LastError);
                    ret = false;
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                    ret = false;
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        /// <summary>
        /// Upload data into DB Server
        /// </summary>
        /// <param name="dataSet">DataSet that contains table with data to upload</param>
        /// <param name="TableName">Name of table will be updated</param>
        /// <returns></returns>
        public bool BulkUpload(DataSet dataSet, string TableName)
        {
            return BulkUpload(dataSet, TableName, "");
        }
        /// <summary>
        /// Upload data into DB Server
        /// </summary>
        /// <param name="dataSet">DataSet that contains table with data to upload</param>
        /// <param name="TableName">Name of table in DataSet</param>
        /// <param name="destinationTableName">Name of table will be updated</param>
        /// <returns></returns>
        public bool BulkUpload(DataSet dataSet, string TableName, string destinationTableName)
        {
            bool ret = false;
            LastError = "";
            LastSQL = "";
            if (!Connected)
                return ret;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlBulkCopy sbc = new SqlBulkCopy(connect)
                    {
                        DestinationTableName = destinationTableName.Equals("") ? TableName : destinationTableName,
                        BulkCopyTimeout = TimeOut
                    };
                    sbc.WriteToServer(dataSet.Tables[TableName]);
                    ret = true;
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.Message;
                    Logger?.Error(LastError);
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
        public string NewID()
        {
            return this.GetValueSQL("select NewID()").ToString();
        }
        public string GetDatabaseVersion()
        {
            string version = "";
            if (!Connected)
                return version;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    version = connect.ServerVersion;
                    connect.Close();
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    Logger?.Error(LastError);
                }
            }
            return version;
        }
        public string GetDatabaseName()
        {
            string name = "";
            if (!Connected)
                return name;
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    name = connect.Database;
                    connect.Close();
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    Logger?.Error(LastError);
                }
            }
            return name;
        }
        public int BulkInsert(string[] sql)
        {
            int ret = -1;
            LastError = "";
            using (SqlConnection connect = new SqlConnection(ConnectionString))
            {
                try
                {
                    connect.Open();
                    SqlTransaction transaction = connect.BeginTransaction();
                    SqlCommand cmd = new SqlCommand("", connect, transaction);
                    ++ret;
                    for (int i = 0; i < sql.Length; i++)
                    {
                        if ((sql[i] != null) && !sql[i].Trim().Equals(""))
                        {
                            LastSQL = sql[i];
                            cmd.CommandText = sql[i];
                            ret += cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    LastError = "DB Error: " + e.ToString();
                    Logger?.Error(LastError);
                    ret = -1;
                }
                finally
                {
                    connect.Close();
                }
            }
            return ret;
        }
    }
}