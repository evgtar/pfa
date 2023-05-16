using System.Data;

namespace PFA.Core
{
    interface IDBHelper
    {
        public string LastError { get; set; }

        public void Execute(string sql)
        {

        }
        public bool GetDataSet(ref DataSet results, string TableName, string sql)
        {
            return false;
        }
        public bool GetDataSet(ref DataSet results, string TableName, string ProcedureName, PropertyCollection parameters)
        {
            return false;
        }

        public string GetValueSQL(string sql)
        {
            return "";
        }

        public int ExecuteSQL(string sql)
        {
            return 1;
        }

        public int GetCount(string tableName, string where)
        {
            return 0;
        }
        public int BulkInsert(string[] sql)
        {
            return 0;
        }
    }
}