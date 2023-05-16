using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFA.Core
{
    public class SQLiteHelper: IDBHelper
    {
        public string LastError { get; set; }
        public SQLiteHelper(string ConnectionString)
        {
            throw new NotImplementedException();
        }

        public static bool CreateDB(string DBFileName)
        {
            throw new NotImplementedException();
        }

        public void Execute(string sql)
        {
            throw new NotImplementedException();
        }
        public bool GetDataSet(ref DataSet results, string TableName, string sql)
        {
            throw new NotImplementedException();
        }
        public bool GetDataSet(ref DataSet results, string TableName, string ProcedureName, PropertyCollection parameters)
        {
            throw new NotImplementedException();
        }

        public string GetValueSQL(string sql)
        {
            throw new NotImplementedException();
        }

        public int ExecuteSQL(string sql)
        {
            throw new NotImplementedException();
        }
        public int GetCount(string tableName, string where)
        {
            throw new NotImplementedException();
        }
        public int BulkInsert(string[] sql)
        {
            throw new NotImplementedException();
        }
    }
}
