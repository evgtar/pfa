using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFA.Core
{
    public class LogHelper
    {
        public static string DumpProp(PropertyCollection prop)
        {
            string retValue;
            StringBuilder sb = new();
            sb.AppendFormat("{0}\tDump of property collection\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (object key in prop.Keys)
            {
                sb.AppendFormat($"{key}\t{prop[key]}{Environment.NewLine}");
            }
            sb.AppendLine($"{Environment.NewLine}-----------------------------------");
            retValue = sb.ToString();
            return retValue;
        }

        public static string DumpTable(DataTable table)
        {
            string retValue;

            StringBuilder sb = new();
            sb.AppendFormat("{0}\tDump of table {1}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), table.TableName);
            for (int c = 0; c < table.Columns.Count; c++)
                sb.AppendFormat("{0}\t", table.Columns[c].ColumnName);
            sb.AppendLine();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int c = 0; c < table.Columns.Count; c++)
                    sb.AppendFormat("{0}\t", table.Rows[i][c].ToString());
                sb.AppendLine();
            }
            sb.AppendLine("\n-----------------------------------");
            retValue = sb.ToString();

            return retValue;
        }
    }
}
