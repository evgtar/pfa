using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.IO;
using System.Data;
using System.Linq;

using SKTIT.Utils;
using SKTIT.DBUtils;
using System.Data.SqlTypes;

namespace EvgTar.PFA
{    
    public class Core
    {
        public string ConnectionString = "";
        private string decimal_sign = ",";
        private string decimal_sign_change;
        public string LastError = "";
        public string LastSQL = "";
        public string LogFileName = "pfa.log";
        public string DBName = "";
        public DBTypes DBType = DBTypes.SQLite;
        public string base_currency = "EUR";
        public bool Debug = false;
        public Dictionary<string, string> Settings = new Dictionary<string, string>();

        public Core(string connectionString)
        {
            ConnectionString = connectionString;
            decimal_sign = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            decimal_sign_change = decimal_sign.Equals(",") ? "." : ",";
        }
        /// <summary>
        /// Get settings from db
        /// </summary>
        public void Init()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return;
            SettingsGet();
        }
        public bool SettingsGet()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return false;
            bool nextStep = false;
            string sql = "";
            DataSet ds = new DataSet();
            switch (DBType)
            {
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(ConnectionString);
                    sql = "select  setting_name, setting_value from dbo.Settings order by setting_name";
                    nextStep = sqlite.GetDataSet(ref ds, "Settings", sql);
                    if (!nextStep)
                        LastError = sqlite.LastError;
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.SettingsGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "Settings", sql, new PropertyCollection());
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            if (nextStep)
            {
                for (int i = 0; i < ds.Tables["Settings"].Rows.Count; i++)
                {
                    Settings.Add(ds.Tables["Settings"].Rows[i]["setting_name"].ToString(), ds.Tables["Settings"].Rows[i]["setting_value"].ToString());
                }
                base_currency = Settings.Where(d => d.Key.Equals("base_currency")).Select(d => d.Value).FirstOrDefault();
            }
            else
            {
                new CFileLog(LogFileName).WriteLine(LastError + Environment.NewLine + sql);
            }
            return nextStep;
        }
        public bool SettingsSave()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return false;
            switch (DBType)
            {
                case DBTypes.SQLite:
                    foreach (var key in Settings.Keys)
                    {
                        ExecuteSQL($"insert or replace into Settings (setting_name, setting_value) values ('{key}', '{Settings[key]}')");
                    }
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@SettingName", "" },
                        { "@SettingValue", "" }
                    };
                    foreach (var key in Settings.Keys)
                    {
                        sqlProp["@SettingName"] = key;
                        sqlProp["@SettingValue"] = Settings[key];
                        dbtools.ExecuteSQL("dbo.SettingUpd", sqlProp);
                    }
                    break;
            }
            return true;
        }
        public bool Synchronize(Int64 AccountID)
        {
            return true;
        }
        public bool Transfer(Int64 FromAccountID, float FromAmount, float FromCurrencyRate, float FromAmountBase, Int64 ToAccountID, float ToAmount, float ToCurrencyRate, float ToAmountBase, DateTime TransferDate)
        {
            bool nextStep = false;
            //	2308978158281250
            if (ConnectionString.Equals(""))
                return nextStep;

            Int64 TransferCategoryID = 0;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    try
                    {
                        TransferCategoryID = Convert.ToInt64(mysql.GetValueSQL("select setting_value from Settings where setting_name = 'TransferCategoryID'"));
                    }
                    catch { }
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    try
                    {
                        TransferCategoryID = Convert.ToInt64(mssql.GetValueSQL("select setting_value from Settings where setting_name = 'TransferCategoryID'"));
                    }
                    catch { }
                    break;
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(ConnectionString);
                    try
                    {
                        TransferCategoryID = Convert.ToInt64(sqlite.GetValueSQL("select setting_value from Settings where setting_name = 'TransferCategoryID'"));
                    }
                    catch { }
                    break;
            }
            //Int64 trans_id = (DateTime.Now.Ticks - (new DateTime(2000, 1, 1).Ticks));
            long trans_id = TransactionAdd(-1, TransferDate, 1, TransferCategoryID, FromAccountID, ToAccountID, 0, -1, FromAmount, FromCurrencyRate, FromAmountBase, "transfer", 1);
            if (trans_id > 0)
                TransactionAdd(-1, TransferDate, 0, TransferCategoryID, ToAccountID, FromAccountID, 0, -1, ToAmount, ToCurrencyRate, ToAmountBase, "transfer", 1);
            else
                return false;
            return true;
        }
        public bool AddCrossRate(DateTime date, string CurrencyFrom, string CurrencyTo, float rate)
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                return false;
            string sql;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    sql = "insert into ExchangeRates (cc_date, cc_from, cc_to, cc_rate, cc_status) values ('" + date.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + CurrencyFrom + "', '" + CurrencyTo + "', " + rate.ToString().Replace(",", ".") + ", 0)";
                    if (mysql.ExecuteSQL(sql) == 1)
                    {
                        sql = "update ExchangeRates set cc_status = 99 where cc_status = 1 and cc_from = '" + CurrencyFrom + "' and cc_to = '" + CurrencyTo + "'";
                        mysql.ExecuteSQL(sql);
                        sql = "update ExchangeRates set cc_status = 1 where cc_status = 0 and cc_from = '" + CurrencyFrom + "' and cc_to = '" + CurrencyTo + "'";
                        mysql.ExecuteSQL(sql);
                    }
                    else
                        return false;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CurrencyCodeFrom", CurrencyFrom },
                        { "@CurrencyCodeTo", CurrencyTo },
                        { "@ExchangeRateDate", date.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "@ExchangeRate", rate.ToString().Replace(",", ".") }
                    };
                    sql = "dbo.ExchangeRateAdd";
                    mssql.ExecuteSQL(sql, sqlProp);
                    break;
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(ConnectionString);
                    sql = "insert into ExchangeRates (cc_date, cc_from, cc_to, cc_rate, cc_status) values ('" + date.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + CurrencyFrom + "', '" + CurrencyTo + "', " + rate.ToString().Replace(",", ".") + ", 0)";
                    if (sqlite.ExecuteSQL(sql) == 1)
                    {
                        sql = "update ExchangeRates set cc_status = 99 where cc_status = 1 and cc_from = '" + CurrencyFrom + "' and cc_to = '" + CurrencyTo + "'";
                        sqlite.ExecuteSQL(sql);
                        sql = "update ExchangeRates set cc_status = 1 where cc_status = 0 and cc_from = '" + CurrencyFrom + "' and cc_to = '" + CurrencyTo + "'";
                        sqlite.ExecuteSQL(sql);
                    }
                    else
                        return false;
                    break;
            }

            return true;
        }
        public bool SetCreditPayment(Int64 CreditPaymentID, Int64 CreditID, Int64 TransactionID, DateTime CreditPaymentDate, Single CreditPaymentAmount, Single CreditPaymentPercentsAmount, Int16 CreditPaymentStatus)
        {
            LastError = "";
            if (ConnectionString.Equals(""))
            {
                LastError = "Connection string is empty";
                return false;
            }
            SQLite sqlite = new SQLite(ConnectionString);
            string sql;
            if (CreditPaymentID == 0)
            {
                CreditPaymentID = DateTime.Now.Ticks - (new DateTime(2000, 1, 1).Ticks);
                sql = "insert into CreditPayments (crpaym_id, credit_id, trans_id, crpaym_date, crpaym_amount, crpaym_perc_amount, crpaym_status) values (" + CreditPaymentID.ToString() + ", " + CreditID.ToString() + ", " + TransactionID.ToString() + ", '" + CreditPaymentDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " + CreditPaymentAmount.ToString().Replace(",", ".") + ", " + CreditPaymentPercentsAmount.ToString().Replace(",", ".") + ", " + CreditPaymentStatus.ToString() + ")";
            }
            else
                sql = "update CreditPayments set trans_id = " + TransactionID.ToString() + ", crpaym_date = '" + CreditPaymentDate.ToString("yyyy-MM-dd HH:mm:ss") + "', crpaym_amount = " + CreditPaymentAmount.ToString().Replace(",", ".") + ", crpaym_perc_amount = " + CreditPaymentPercentsAmount.ToString().Replace(",", ".") + ", crpaym_status = " + CreditPaymentStatus.ToString() + " where crpaym_id = " + CreditPaymentID.ToString();

            if (sqlite.ExecuteSQL(sql) != 1)
            {
                LastError = sqlite.LastError + "\n" + sql;
                return false;
            }
            return true;
        }
        public bool CreateDB(string DBFileName)
        {
            bool ret_val = false;
            string[] sql ={
                "create table Currencies (currency_code char(3) not null, currency_name varchar(255) not null, currency_symbol varchar(10), currency_rate double not null default 1.0, currency_status smallint not null default 0, primary key (currency_code))",
                "insert into Currencies (currency_code, currency_name, currency_symbol, currency_status) values ('EUR', 'Euro', '�', 1)",
                "insert into Currencies (currency_code, currency_name, currency_symbol, currency_status) values ('USD', 'US Dollars', '$', 1)",
                "insert into Currencies (currency_code, currency_name, currency_symbol, currency_status) values ('RUR', '���������� �����', '', 1)",
                "insert into Currencies (currency_code, currency_name, currency_symbol, currency_status) values ('BYR', '��������i ������', '', 1)",
                "insert into Currencies (currency_code, currency_name, currency_symbol, currency_status) values ('UAH', '���������� ������', '', 1)",
                "create table Settings (setting_name varchar(100) not null, setting_value text, primary key (setting_name))",
                "insert into Settings (setting_name, setting_value) values ('base_currency', 'EUR')",
                "create table Holders (holder_id bigint not null, holder_name varchar(100) not null, holder_descr text, holder_status smallint not null default 0, primary key (holder_id))",
                "create table Categories (category_id bigint not null, pcategory_id bigint not null default 0, category_name varchar(255) not null, category_status smallint not null default 0, primary key (category_id))",
                "create table Accounts(account_id bigint not null, holder_id bigint, currency_code char(3) not null, account_name varchar(255) not null, account_number varchar(255), initial_balance double not null default 0.0, local_balance double not null default 0.0, base_balance double not null default 0.0, account_status smallint not null default 0, primary key (account_id))",
                "create table Payers (payer_id bigint not null, payer_name varchar(255) not null, primary key (payer_id))",
                "create table Transactions (trans_id bigint not null, trans_date datetime not null default current_timestamp, trans_type smallint not null default 0, category_id bigint not null, account_id bigint not null, account_id_ref bigint, payer_id bigint, amount_local double not null default 0.0, currency_rate double not null default 1.0, amount_base double default 0.0, trans_notes text, trans_status smallint not null default 0, primary key (trans_id))",
                "create table ReportExcludes (report_id bigint not null, exclude_id bigint, primary key (report_id, exclude_id))"
                          };
            bool next = true;
            if (File.Exists(DBFileName))
            {

                try
                {
                    File.Delete(DBFileName);
                }
                catch
                {
                    next = false;
                }
            }
            if (next)
            {
                switch (DBType)
                {
                    default:
                        SQLite sqlite = new SQLite("");
                        if (sqlite.CreateDB(DBName))
                        {
                            ret_val = true;
                            for (int i = 0; i < sql.Length; i++)
                            {
                                ExecuteSQL(sql[i]);
                            }
                            /*
                            using (CreateDatabase createDB = new CreateDatabase())
                            {
                                createDB.Parent = null;
                                createDB.ProgressBarMax = sql.Length + 1;
                                createDB.ProgressBarPosition = 0;
                                createDB.Text = NLS.LNGetString("TitleCreateProject", "Create new project ...");
                                createDB.Show();
                                ConnectionString = "Data Source = " + settings.DB;
                                sqlite = new SQLite(ConnectionString);
                                createDB.Info = NLS.LNGetString("TextCreatingDatabase", "Creating database ...");
                                for (int i = 0; i < sql.Length; i++)
                                {
                                    sqlite.ExecuteSQL(sql[i]);
                                    createDB.ProgressBarPosition = i;
                                    createDB.Refresh();
                                }
                                createDB.Info = NLS.LNGetString("TextLoadingDatabase", "Loading database ...");
                                createDB.Refresh();
                                LoadDB(DBFileName);
                            }
                            */
                        }
                        else
                            LastError = sqlite.LastError;
                        break;
                }
            }
            return ret_val;
        }
        public bool LoadDB(ref DataSet ds, string DBName)
        {
            bool ret_val = false;
            this.DBName = DBName;
            switch (DBType) // create backup
            {
                default: //SQLite
                    if (!DBName.Equals(""))
                    {
                        try
                        {
                            FileInfo fi = new FileInfo(DBName);
                            //this.Text += " - " + fi.Name;
                            try
                            {
                                File.Copy(DBName, fi.DirectoryName + "\\" + fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + ".pfb", true);
                            }
                            catch { }
                        }
                        catch { }
                    }
                    break;
            }
            if (!DBName.Equals(""))
            {
                base_currency = "";
                if (GetDataSet(ref ds, "Settings", "select * from Settings"))
                {
                    DataRow[] dr = ds.Tables["Settings"].Select("setting_name = 'base_currency'");
                    if (dr.Length > 0)
                        base_currency = dr[0]["setting_value"].ToString();
                    ret_val = true;
                }
            }

            return ret_val;
        }
        public bool GetDataSet(ref DataSet ds, String TableName, String sql)
        {
            bool retval;
            LastError = "";
            LastSQL = sql;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    retval = mysql.GetDataSet(ref ds, TableName, sql);
                    if (!retval)
                        LastError = mysql.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    retval = mssql.GetDataSet(ref ds, TableName, sql);
                    if (!retval)
                        LastError = mssql.LastError;
                    break;
                default:
                    SQLite sqlite = new SQLite(ConnectionString);
                    retval = sqlite.GetDataSet(ref ds, TableName, sql);
                    if (!retval)
                        LastError = sqlite.LastError;
                    break;
            }
            return retval;
        }
        public int ExecuteSQL(string sql)
        {
            int retval;
            LastError = "";
            LastSQL = sql;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    retval = mysql.ExecuteSQL(sql);
                    if (!mysql.LastError.Equals(""))
                        LastError = mysql.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    retval = mssql.ExecuteSQL(sql);
                    if (!mssql.LastError.Equals(""))
                        LastError = mssql.LastError;
                    break;
                default:
                    SQLite sqlite = new SQLite(ConnectionString);
                    retval = sqlite.ExecuteSQL(sql);
                    if (!sqlite.LastError.Equals(""))
                        LastError = sqlite.LastError;
                    break;
            }
            return retval;
        }
        public object GetValueSQL(String sql)
        {
            object retval;
            LastError = "";
            LastSQL = sql;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    retval = mysql.GetValueSQL(sql);
                    if (!mysql.LastError.Equals(""))
                        LastError = mysql.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    retval = mssql.GetValueSQL(sql);
                    if (!mssql.LastError.Equals(""))
                        LastError = mssql.LastError;
                    break;
                default:
                    SQLite sqlite = new SQLite(ConnectionString);
                    retval = sqlite.GetValueSQL(sql);
                    if (!sqlite.LastError.Equals(""))
                        LastError = sqlite.LastError;
                    break;
            }
            return retval;
        }
        public int GetCount(String tableName, String where)
        {
            int retval;
            LastError = "";
            LastSQL = tableName + " " + where;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    retval = mysql.GetCount(tableName, where);
                    if (!mysql.LastError.Equals(""))
                        LastError = mysql.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    retval = mssql.GetCount(tableName, where);
                    if (!mssql.LastError.Equals(""))
                        LastError = mssql.LastError;
                    break;
                default:
                    SQLite sqlite = new SQLite(ConnectionString);
                    retval = sqlite.GetCount(tableName, where);
                    if (!sqlite.LastError.Equals(""))
                        LastError = sqlite.LastError;
                    break;
            }
            return retval;
        }
        public int BulkInsert(String[] sql)
        {
            int retval;
            LastError = "";
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:
                    MySQL mysql = new MySQL(ConnectionString);
                    retval = mysql.BulkInsert(sql);
                    if (!mysql.LastError.Equals(""))
                        LastError = mysql.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils mssql = new MSSQLUtils(ConnectionString);
                    retval = mssql.BulkInsert(sql);
                    if (!mssql.LastError.Equals(""))
                        LastError = mssql.LastError;
                    break;
                default:
                    SQLite sqlite = new SQLite(ConnectionString);
                    retval = sqlite.BulkInsert(sql);
                    if (!sqlite.LastError.Equals(""))
                        LastError = sqlite.LastError;
                    break;
            }
            return retval;
        }
        public bool Export(string FileName)
        {
            bool ret_val = false;
            switch (DBType) // create backup
            {
                case DBTypes.MySQL:

                    break;
                case DBTypes.MSSQL:

                    break;
                default:
                    SQLite sqlite = new SQLite(ConnectionString);
                    String retval = sqlite.ExportToMySQL(true);
                    if (!sqlite.LastError.Equals(""))
                        LastError = sqlite.LastError;
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(FileName, false, Encoding.UTF8))
                        {
                            sw.Write(retval);
                            sw.Close();
                        }
                        ret_val = true;
                    }
                    break;
            }
            return ret_val;
        }
        public bool RecalcAccount(string account_id, string crate)
        {
            string sql = "";
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    CoreSQLite sqliteCore = new CoreSQLite(ConnectionString);
                    nextStep = sqliteCore.RecalcAccount(Int64.Parse(account_id), crate);
                    if (!nextStep)
                        LastError = sqliteCore.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@AccountId", account_id },
                        { "@CurrencyRate", crate.Replace(",", ".") }
                    };
                    sql = "dbo.RecalcAccount";
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            if (!nextStep)
            {
                new CFileLog(LogFileName).WriteLine(LastError + Environment.NewLine + sql);
            }
            return nextStep;
        }
        public bool TransactionsGet(ref DataSet ds, DateTime TransactionFrom, DateTime TransactionTill, string Tags = "", long AccountId = -1, long CategoryId = -1, long PayerId = -1, long AssetId = -1, string TableName = "Transactions")
        {
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql = "select trans_id, trans.account_id, trans.payer_id, currency_rate, date(trans_date) DT, account_name, pcat.category_name||' - '||cat.category_name CName, currency_code, (case when trans_type = 0 then amount_local else 0.00 end) deposit, (case when trans_type = 1 then amount_local else 0.00 end) withdraw, p.payer_name, trans_notes ";
                    sql += " from Transactions trans ";
                    sql += " left join Accounts acc on trans.account_id = acc.account_id ";
                    sql += " left join Categories cat on trans.category_id = cat.category_id ";
                    sql += " left join Categories pcat on cat.pcategory_id = pcat.category_id ";
                    sql += " left join Payers p on p.payer_id=trans.payer_id ";
                    sql += " where trans_date>='" + TransactionFrom.ToString("yyyy-MM-dd 00:00:00") + "' and trans_date<='" + TransactionTill.ToString("yyyy-MM-dd 23:59:59") + "' ";
                    if (AccountId > 0)
                        sql += " and acc.account_id = " + AccountId.ToString("G19");
                    if (CategoryId > 0)
                        sql += " and trans.category_id = " + CategoryId.ToString("G19");
                    if (PayerId > 0)
                        sql += " and trans.payer_id = " + PayerId.ToString("G19");
                    if (AssetId > 0)
                        sql += " and trans.asset_id = " + AssetId.ToString("G19");
                    sql += " order by trans_date";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@TransFrom", TransactionFrom.ToString("yyyy-MM-dd 00:00:00") },
                        { "@TransTill", TransactionTill.ToString("yyyy-MM-dd 23:59:59") },
                        { "@AccountId", AccountId },
                        { "@CategoryId", CategoryId },
                        { "@PayerId", PayerId },
                        { "@AssetId", AssetId },
                        { "@Tags", Tags }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.TransactionsGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool TransactionGet(ref DataSet ds, long TransactionId, string TableName = "TransactionInfo")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(ConnectionString);
                    sql = "select trans_id, trans_date, account_id, category_id, payer_id, trans_type, amount_local, currency_rate, amount_base, trans_notes from Transactions where trans_id=" + TransactionId.ToString("G19");
                    nextStep = sqlite.GetDataSet(ref ds, "TransactionInfo", sql);
                    if (!nextStep)
                        LastError = sqlite.LastError;
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.TransactionInfoGet";
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@TransactionId", TransactionId }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public long TransactionAdd(long TransactionID, DateTime TransactionDate, int TransactionType, long CategoryID, long AccountID, long AccountRefID, long PayerID, long AssetId, float AmountLocal, float CurrencyRate, float AmountBase, string Notes, int TransactionStatus)
        {
            /*if (ConnectionString.Equals(""))
                return false;
            string m = (TransactionType == 0) ? "+" : "-";            
            SQLite sqlite = new SQLite(ConnectionString);
            string sql = "insert into Transactions (trans_id, trans_date, trans_type, category_id, account_id, account_id_ref, payer_id, amount_local, currency_rate, amount_base, trans_notes, trans_status) values (";
            sql += TransactionID.ToString("G19") + ", ";
            sql += "'" + TransactionDate.ToString("yyyy-MM-dd HH:mm:ss") + "', ";
            sql += TransactionType.ToString() + ",";
            sql += CategoryID.ToString("G19") + ", ";
            sql += AccountID.ToString("G19") + ", ";
            sql += AccountRefID.ToString("G19") + ", ";
            sql += PayerID.ToString("G19") + ", ";
            sql += AmountLocal.ToString().Replace(",", ".") + ", ";
            sql += CurrencyRate.ToString().Replace(",", ".") + ", ";
            sql += AmountBase.ToString().Replace(",", ".") + ", ";
            sql += "'" + Notes.Replace("'", "`") + "', ";
            sql += TransactionStatus.ToString() + ")";
            if (sqlite.ExecuteSQL(sql) == 1)
            {                
                sql = "update Accounts set ";
                sql += " local_balance = local_balance " + m + " " + AmountLocal.ToString().Replace(",", ".") + ", ";
                sql += " base_balance = base_balance " + m + " " + AmountBase.ToString().Replace(",", ".") + " ";
                sql += " where account_id = " + AccountID.ToString("G19");
                sqlite.ExecuteSQL(sql);
                        //if ((sqlite.ExecuteSQL(sql) != 1)
            }
            else
                return false;
            return true;*/
            long trans_id = -1;
            DataSet ds = new DataSet();
            switch (DBType)
            {
                case DBTypes.SQLite:
                    trans_id = new CoreSQLite(ConnectionString).TransactionAdd(TransactionID.ToString("G19"),
                        TransactionDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        TransactionType,
                        CategoryID,
                        AccountID,
                        AccountRefID,
                        PayerID,
                        AssetId,
                        AmountLocal.ToString().Replace(",", "."),
                        CurrencyRate.ToString().Replace(",", "."),
                        AmountBase.ToString().Replace(",", "."),
                        Notes.Trim()
                        );
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@TransactionId", TransactionID },
                        { "@TransactionDate", TransactionDate.ToString("yyyy-MM-dd HH:mm:ss") },
                        { "@PaymentType", TransactionType },
                        { "@CategoryId", CategoryID },
                        { "@AccountId", AccountID },
                        { "@AccountIdRef", AccountRefID },
                        { "@PayerId", PayerID },
                        { "@Amount", AmountLocal.ToString().Replace(",", ".") },
                        { "@CurrencyRate", CurrencyRate.ToString().Replace(",", ".") },
                        { "@AmountBase", AmountBase.ToString("G19").Replace(",", ".") },
                        { "@Notes", Notes.Trim() },
                        { "@AssetId", AssetId }
                    };
                    if (dbtools.GetDataSet(ref ds, "TransactionId", "dbo.TransactionAdd", sqlProp))
                    {
                        if (ds.Tables["TransactionId"].Rows.Count > 0)
                        {
                            trans_id = (long)ds.Tables["TransactionId"].Rows[0]["TransactionId"];
                        }
                        else
                        {
                            new CFileLog(LogFileName).WriteLine("No transaction ID returned");
                            new CFileLog(LogFileName).DumpProp(sqlProp);
                        }
                    }
                    else
                    {
                        new CFileLog(LogFileName).WriteLine("Error add transaction: " + dbtools.LastError);
                        new CFileLog(LogFileName).DumpProp(sqlProp);
                    }

                    break;
                default:
                    new CFileLog(LogFileName).WriteLine("Database type " + DBType.ToString() + " is not implemented yet");
                    break;
            }
            return trans_id;
        }
        public void TransactionLogAdd(long TransactionID, string TransactionText, int TransactionSourceId)
        {
            switch (DBType)
            {
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@TransactionId", TransactionID },
                        { "@TransactionText", TransactionText.Trim() },
                        { "@TransactionSourceId", TransactionSourceId }
                    };
                    new CFileLog(LogFileName).DumpProp(sqlProp);
                    dbtools.ExecuteSQL("dbo.TransactionLogAdd", sqlProp);
                    break;
            }
        }
        public bool TransactionDelete(long TransactionId)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "delete from Transactions where trans_id = " + TransactionId.ToString();
                    nextStep = (ExecuteSQL(sql) == 1);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.TransactionDel";
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@TransactionId", TransactionId }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            return nextStep;
        }
        public int TransactionsGetCount(long AccountId, long PayerId)
        {
            LastError = "";
            int ret_val = -1;
            string where = "where " + ((AccountId > 0) ? " account_id = " + AccountId.ToString() : "") + ((PayerId > 0) ? " payer_id = " + PayerId.ToString() : "");
            switch (DBType)
            {
                case DBTypes.SQLite:
                    ret_val = GetCount("Transactions", where);
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    ret_val = dbtools.GetCount("Transactions", where);
                    break;
            }
            return ret_val;
        }
        public bool TransactionsYearsGet(ref DataSet ds, string TableName = "Years")
        {
            bool nextStep = false;
            string sql;
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select distinct strftime('%Y', trans_date) YR from Transactions order by 1";
                    nextStep = GetDataSet(ref ds, "Years", sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "select distinct year(trans_date) YR from Transactions order by 1";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "Years", sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool AccountTypesGet(ref DataSet ds)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select * from AccountTypes where acctype_status > 0";
                    nextStep = GetDataSet(ref ds, "AccountTypes", sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.AccountTypesGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "AccountTypes", sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool AccountsGet(ref DataSet ds, bool AllStatuses = false, string TableName = "Accounts")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select account_id, a.currency_code, holder_id, account_name, account_number, account_type, initial_balance, account_status, c.currency_rate from Accounts a join Currencies c on c.currency_code = a.currency_code order by account_name";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.AccountsGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@AccountStatus", AllStatuses ? -2 : -1 }
                    };
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool AccountSave(long AccountId, string AccountName, string AccountNumber, int AccountType, string CurrencyCode, string initBalace, bool IsActive)
        {
            string sql = "select currency_rate from Currencies where currency_code = '" + CurrencyCode + "'";
            string crate = GetValueSQL(sql).ToString().Replace(",", ".");
            DataSet ds = new DataSet();
            if (AccountId < 0)
            {
                sql = "select coalesce(max(account_id)+1, 1) from Accounts";
                string id = GetValueSQL(sql).ToString();
                sql = "insert into Accounts (account_id, currency_code, holder_id, account_name, account_number, account_type, initial_balance, local_balance, base_balance, account_status) values (";
                sql += id + ", '" + CurrencyCode + "', null, '" + AccountName + "', '" + AccountNumber + "', " + AccountType.ToString() + "," + initBalace + ", " + initBalace + ", " + initBalace + " / " + crate + "," + (IsActive ? "1" : "0") + ")";
            }
            else
            {
                sql = "select coalesce(sum(amount_local), 0) local, coalesce(sum(amount_base), 0) base from Transactions where account_id = " + AccountId.ToString() + " and trans_type = 0";
                string dep_local = "0";
                //string dep_base = "0";
                if (GetDataSet(ref ds, "DepositAmounts", sql))
                {
                    if (ds.Tables["DepositAmounts"].Rows.Count > 0)
                    {
                        dep_local = ds.Tables["DepositAmounts"].Rows[0]["local"].ToString().Replace(",", ".");
                        //dep_base = ds.Tables["DepositAmounts"].Rows[0]["base"].ToString().Replace(",", ".");
                    }
                }
                sql = "select coalesce(sum(amount_local), 0) local, coalesce(sum(amount_base), 0) base from Transactions where account_id = " + AccountId.ToString() + " and trans_type = 1";
                string wd_local = "0";
                //string wd_base = "0";
                if (GetDataSet(ref ds, "WithdrawalAmounts", sql))
                {
                    if (ds.Tables["WithdrawalAmounts"].Rows.Count > 0)
                    {
                        wd_local = ds.Tables["WithdrawalAmounts"].Rows[0]["local"].ToString().Replace(",", ".");
                        //wd_base = ds.Tables["WithdrawalAmounts"].Rows[0]["base"].ToString().Replace(",", ".");
                    }
                }
                sql = "update Accounts set currency_code='" + CurrencyCode + "', ";
                sql += " account_name = '" + AccountName + "', account_number='" + AccountNumber + "', ";
                sql += " account_type = " + AccountType.ToString() + ", ";
                sql += " initial_balance = " + initBalace + ", ";
                sql += " local_balance = " + initBalace + " + " + dep_local + " - " + wd_local + ", ";
                //sql += " base_balance = " + init_balance + " / " + crate + " + " + dep_base + " - " + wd_base + ", ";
                sql += " base_balance = (" + initBalace + " + " + dep_local + " - " + wd_local + ") / " + crate + ", ";
                sql += " account_status = " + (IsActive ? "1" : "0") + " where account_id = " + AccountId.ToString();
            }
            return (ExecuteSQL(sql) == 1);
        }
        public bool AccountDelete(long AccountId)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "delete from Accounts where account_id = " + AccountId.ToString();
                    nextStep = (ExecuteSQL(sql) == 1);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.AccountDel";
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@AccountId", AccountId }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            return nextStep;
        }
        public bool AccountChecked(string account_id)
        {
            string sql = "";
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    //CoreSQLite sqliteCore = new CoreSQLite(ConnectionString);
                    //nextStep = sqliteCore.RecalcAccount(Int64.Parse(account_id));
                    //if (!nextStep)
                    //    LastError = sqliteCore.LastError;
                    LastError = "������ ������ � ������ ������ �� ��������";
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new DBUtils.MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@AccountId", account_id }
                    };
                    sql = "dbo.AccountSetCheck";
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            if (!nextStep)
            {
                new CFileLog(LogFileName).WriteLine(LastError + Environment.NewLine + sql);
            }
            return nextStep;
        }
        public bool AccountsInfoGet(ref DataSet ds, int AccountType = -1)
        {
            bool nextStep = false;
            LastError = "";
            if (ds.Tables.Contains("AccountsInfo"))
                ds.Tables.Remove("AccountsInfo");
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql = "select a.account_id, a.account_name, a.currency_code, round(c.currency_rate, 2) as currency_rate, round(a.local_balance, 2) as local_balance, round(a.base_balance, 2) as base_balance, round(a.local_balance / c.currency_rate, 2) as base_calc, a.account_checkdt from Accounts a, Currencies c where a.account_status > 0 and abs(a.local_balance / c.currency_rate) > a.min_balance and a.currency_code = c.currency_code ";
                    if (AccountType > 0)
                        sql += " and a.account_type = " + AccountType.ToString();
                    sql += " order by account_name";
                    nextStep = GetDataSet(ref ds, "AccountsInfo", sql);
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@AccountType", AccountType }
                    };
                    nextStep = dbtools.GetDataSet(ref ds, "AccountsInfo", "dbo.AccountsInfoGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public double AccountsInfoTotalGet()
        {
            double amount = 0.0;
            LastError = "";

            try
            {
                switch (DBType)
                {
                    case DBTypes.SQLite:
                        amount = (double)GetValueSQL("select sum(base_balance) from Accounts where account_status > 0 and base_balance > 0");
                        break;
                    case DBTypes.MSSQL:
                        MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                        amount = (double)dbtools.GetValueSQL("select sum(base_balance) from Accounts where account_status > 0 and base_balance > 0");
                        break;
                }
            }
            catch (Exception ex)
            {
                LastError = "Core::AccountsInfoTotalGet::Exception" + Environment.NewLine + ex.ToString();
            }

            return amount;
        }
        public bool AccountsInfoAvgGet(ref DataSet ds, DateTime OnDate)
        {
            bool nextStep = false;
            string sql;
            LastError = "";

            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select sum(amount_base) Total from transactions t1 where trans_type = 1 and category_id not in (select exclude_id from ReportExcludes where report_id=1) and strftime('%Y-%m-%d', trans_date)>='" + OnDate.ToString("yyyy-MM-dd") + "'";
                    nextStep = GetDataSet(ref ds, "AverageInfo", sql);
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    sql = "select sum(amount_base) Total from transactions t1 where trans_type = 1 and category_id not in (select exclude_id from ReportExcludes where report_id=1) and trans_date>=convert(datetime, '" + OnDate.ToString("yyyy-MM-dd") + " 00:00:00', 120)";
                    nextStep = dbtools.GetDataSet(ref ds, "AverageInfo", sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        /// <summary>
        /// Returns data in refered Dataset. Default TableName is Assets
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="AssetId">Int64</param>
        /// <param name="AssetGroupId">Int64</param>
        /// <param name="AssetStatus">Int32</param>
        /// <param name="TableName">String</param>
        /// <returns></returns>
        public bool AssetsGet(ref DataSet ds, long AssetId = -1, long AssetGroupId = -1, int AssetStatus = 0, string TableName = "Assets")
        {
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql = "select a.asset_id as AssetId, a.ag_id as AssetGroupId, a.asset_name as AssetName, ag.ag_name as AssetGroupName, coalesce(a.asset_note, '') as AssetNote, a.asset_status as AssetStatus from Assets a join AssetGroups ag on a.ag_id = ag.ag_id where a.asset_status > @AssetStatus and (nullif(@AssetId, -1) is null or a.asset_id = @AssetId) and (nullif(@AssetGroupId, -1) is null or a.ag_id = @AssetGroupId) order by a.ag_id, a.asset_id";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@AssetId", AssetId },
                        { "@AssetGroupId", AssetGroupId },
                        { "@AssetStatus", AssetStatus }
                    };
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.AssetsGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool CategoriesGet(ref DataSet ds, bool AllStatuses = false, string TableName = "Categories")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select cat.category_id CategoryID, pcat.category_name||' - '||cat.category_name CName ";
                    sql += " from Categories cat, Categories pcat ";
                    sql += " where cat.pcategory_id = pcat.category_id ";
                    sql += " order by CName";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.CategoriesGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool CategorySave(long CategoryId, long ParentCategoryId, string CategoryName, int CategoryStatus = 1)
        {
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql;
                    if (CategoryId < 0)
                    {
                        sql = "insert into Categories (category_id, pcategory_id, category_name, category_status) values (";
                        sql += (DateTime.Now.Ticks - (new DateTime(2000, 1, 1)).Ticks).ToString("G17") + ", ";
                        sql += ParentCategoryId.ToString("G19") + ", ";
                        sql += "'" + CategoryName + "', ";
                        sql += CategoryStatus.ToString() + ")";
                    }
                    else
                        sql = "update Categories set category_name = '" + CategoryName + "', category_status=" + CategoryStatus.ToString() + " where category_id = " + CategoryId.ToString("G19");

                    SQLite sqlite = new SQLite(ConnectionString);
                    nextStep = (sqlite.ExecuteSQL(sql) == 1);
                    break;
                case DBTypes.MSSQL:
                    DataSet ds = new DataSet();
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CategoryId", CategoryId },
                        { "@ParentCategoryId", ParentCategoryId },
                        { "@CategoryName", CategoryName },
                        { "@CategoryStatus", CategoryStatus }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "CategoryId", "dbo.CategoryUpd", sqlProp);
                    if (nextStep && (ds.Tables["CategoryId"].Rows.Count > 0))
                    {
                        CategoryId = (long)ds.Tables["CategoryId"].Rows[0]["CategoryId"];
                        nextStep = (CategoryId > 0);
                        if (!nextStep && Debug)
                        {
                            new CFileLog(LogFileName).DumpProp(sqlProp);
                            new CFileLog(LogFileName).DumpTable(ds.Tables["CategoryId"]);

                        }
                        else
                        {
                            if (Debug)
                            {
                                new CFileLog(LogFileName).WriteLine(dbtools.LastError);
                                new CFileLog(LogFileName).DumpProp(sqlProp);
                            }
                        }
                    }
                    break;
            }
            return nextStep;
        }
        public bool CategoryDelete(long CategoryId)
        {
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(ConnectionString);
                    string sql = "delete from Categories where pcategory_id = " + CategoryId.ToString("G19");
                    sqlite.ExecuteSQL(sql);
                    sql = "delete from Categories where category_id = " + CategoryId.ToString("G19");
                    nextStep = (sqlite.ExecuteSQL(sql) == 1);
                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CategoryId", CategoryId }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    dbtools.ExecuteSQL("dbo.CategoryDel", sqlProp);
                    nextStep = true;
                    break;
            }
            return nextStep;
        }
        public bool CategoriesTreeGet(ref DataSet ds, bool AllStatuses = false)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    //sqlite.GetDataSet(ref ds, "FCCategories", "select * from Categories order by category_name")
                    sql = "select * from Categories order by category_name";
                    nextStep = GetDataSet(ref ds, "FCCategories", sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.CategoriesTreeGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "FCCategories", sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool CreditsGet(ref DataSet ds, int CreditStatus = -1, string TableName = "Credits")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select c.*, cat2.category_name || ' - ' || cat1.category_name as category from Credits c left join Categories cat1 on cat1.category_id = c.credit_cat_id left join Categories cat2 on cat2.category_id = cat1.pcategory_id where credit_status > 0 order by credit_status, credit_name";
                    nextStep = GetDataSet(ref ds, "Credits", sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "select c.*, concat(cat2.category_name, ' - ', cat1.category_name) as category from Credits c left join Categories cat1 on cat1.category_id = c.credit_cat_id left join Categories cat2 on cat2.category_id = cat1.pcategory_id where credit_status > 0 order by credit_status, credit_name";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        /// <summary>
        /// Returns Currencies set in DataSet table with name Currencies
        /// </summary>
        /// <param name="ds">ref DataSet</param>
        /// <param name="AllStatuses">true to get all currencies and false to get only active</param>
        /// <returns>Currencies table in DataSet</returns>
        public bool CurrenciesGet(ref DataSet ds, bool AllStatuses = false, string TableName = "Currencies")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select * from Currencies where currency_status=1";
                    nextStep = GetDataSet(ref ds, "Currencies", sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.CurrenciesGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CurrencyStatus", AllStatuses ? -2 : -1 }
                    };
                    nextStep = dbtools.GetDataSet(ref ds, "Currencies", sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool CurrencyAdd(string CurrencyCode, string CurrencyName, string CurrencySymbol, string CurrencyRate, bool CurrencyStatus = true)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "insert into Currencies (currency_code, currency_name, currency_symbol, currency_rate, currency_status) values (";
                    sql += "'" + CurrencyCode + "', '" + CurrencyName + "', '" + CurrencySymbol + "', " + CurrencyRate.Replace(",", ".") + ", " + (CurrencyStatus ? "1" : "0") + ")";
                    nextStep = (ExecuteSQL(sql) > 0);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.CurrencyAdd";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CurrencyCode", CurrencyCode },
                        { "@CurrencyName", CurrencyName },
                        { "@CurrencySymbol", CurrencySymbol },
                        { "@CurrencyRate", CurrencyRate.Replace(",", ".") },
                        { "@CurrencyStatus", (CurrencyStatus ? "1" : "0") }
                    };
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            return nextStep;
        }
        public bool CurrencyUpdate(string CurrencyCode, string CurrencyName, string CurrencySymbol, string CurrencyRate, bool CurrencyStatus = true)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "update Currencies set ";
                    sql += " currency_name = '" + CurrencyName + "',";
                    sql += " currency_symbol = '" + CurrencySymbol + "',";
                    sql += " currency_rate = " + CurrencyRate.Replace(",", ".") + ", ";
                    sql += " currency_status = " + (CurrencyStatus ? "1" : "0");
                    sql += " where currency_code = '" + CurrencyCode + "'";
                    nextStep = (ExecuteSQL(sql) > 0);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.CurrencyUpd";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CurrencyCode", CurrencyCode },
                        { "@CurrencyName", CurrencyName },
                        { "@CurrencySymbol", CurrencySymbol },
                        { "@CurrencyRate", CurrencyRate.Replace(",", ".") },
                        { "@CurrencyStatus", (CurrencyStatus ? "1" : "0") }
                    };
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            return nextStep;
        }
        public bool CurrencyDelete(string CurrencyCode)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = $"delete from Currencies where currency_code = '{CurrencyCode}'";
                    nextStep = (ExecuteSQL(sql) == 1);
                    break;
                case DBTypes.MSSQL:
                    new MSSQLUtils(ConnectionString).ExecuteSQL("dbo.CurrencyDel", new PropertyCollection { { "@CurrencyCode", CurrencyCode } });
                    nextStep = true;
                    break;
            }
            return nextStep;
        }
        public bool CurrencyRateUpdate(string CurrencyCode, string rate)
        {
            if (ConnectionString.Equals(""))
                return false;
            string sql = "update Currencies set currency_rate = '" + rate.ToString().Replace(",", ".") + "' where currency_code = '" + CurrencyCode + "'";
            bool nextStep = false;
            switch (DBType)
            {
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(ConnectionString);
                    nextStep = (sqlite.ExecuteSQL(sql) != 1);
                    if (!nextStep)
                        LastError = sqlite.LastError;
                    break;
                case DBTypes.MSSQL:
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = (dbtools.ExecuteSQL(sql) != 1);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            if (!nextStep)
            {
                if (Debug)
                    new CFileLog(LogFileName).WriteLine(LastError + Environment.NewLine + sql);
            }
            return nextStep;
        }
        public bool ExchangeRateGet(ref DataSet ds, string CurrencyCodeFrom, string CurrencyCodeTo, string TableName = "ExchangeRate")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select cc_from, cc_to, cc_rate from ExchangeRates where cc_status = 1  order by cc_date desc";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.ExchangeRateGet";
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@CurrencyCodeFrom", CurrencyCodeFrom },
                        { "@CurrencyCodeTo", CurrencyCodeTo }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool ExchangeRateGetAll(ref DataSet ds, int ExchangeRateStatus = 1, string TableName = "ExchangeRate")
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select cc_from, cc_to, cc_rate from ExchangeRates where cc_status = 1  order by cc_date desc";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.ExchangeRateGetAll";
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@ExchangeRateStatus", ExchangeRateStatus }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        /// <summary>
        /// Returns data in refered Dataset. Default TableName TransPayers
        /// </summary>
        /// <param name="ds">DataSet</param>
        /// <param name="TableName">string</param>
        /// <param name="AllStatuses">boolean</param>
        /// <returns></returns>
        public bool PayersGet(ref DataSet ds, string TableName = "TransPayers", bool AllStatuses = false)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "select payer_id, payer_name from Payers order by payer_name";
                    nextStep = GetDataSet(ref ds, TableName, sql);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.PayersGet";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@PayerStatus", AllStatuses ? -2 : -1 }
                    };
                    nextStep = dbtools.GetDataSet(ref ds, TableName, sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool PayerUpdate(long PayerId, string PayerName)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    if (PayerId < 0)
                        sql = "insert into Payers (payer_id, payer_name) values (" + (DateTime.Now.Ticks - (new DateTime(2000, 1, 1)).Ticks).ToString("G19") + ", '" + PayerName + "')";
                    else
                        sql = "update Payers set payer_name='" + PayerName + "' where payer_id=" + PayerId.ToString();
                    nextStep = (ExecuteSQL(sql) > 0);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.PayerUpd";
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@PayerId", PayerId },
                        { "@PayerName", PayerName }
                    };
                    DataSet ds = new DataSet();
                    nextStep = dbtools.GetDataSet(ref ds, "NewPayer", sql, sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    else
                    {
                        if (ds.Tables["NewPayer"].Rows.Count > 0)
                        {
                            long ret_PayerId = (long)ds.Tables["NewPayer"].Rows[0]["PayerId"];
                            nextStep = (ret_PayerId > 0);
                        }
                        else
                            nextStep = false;
                    }
                    break;
            }
            return nextStep;
        }
        public bool PayerDelete(long PayerId)
        {
            string sql;
            bool nextStep = false;
            LastError = "";
            switch (DBType)
            {
                case DBTypes.SQLite:
                    sql = "delete from Payers where payer_id=" + PayerId.ToString();
                    nextStep = (ExecuteSQL(sql) == 1);
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.PayerDel";
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@PayerId", PayerId }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    dbtools.ExecuteSQL(sql, sqlProp);
                    nextStep = true;
                    break;
            }
            return nextStep;
        }

        #region Reports
        public bool ReportCostSharing(ref DataSet ds, long ReportId, int Year, bool IsDetaled, string TableName = "ReportData")
        {
            bool nextStep = false;
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql;
                    if (IsDetaled)
                    {
                        sql = "select cat.category_id CategoryID, pcat.category_name||' - '||cat.category_name CName, ";
                        sql += " (select coalesce(sum(tr01.amount_base), 0.00) from Transactions tr01 where tr01.category_id=cat.category_id and tr01.trans_type=1 and strftime('%Y-%m', tr01.trans_date)='" + Year + "-01') M01, ";
                        sql += " (select coalesce(sum(tr02.amount_base), 0.00) from Transactions tr02 where tr02.category_id=cat.category_id and tr02.trans_type=1 and strftime('%Y-%m', tr02.trans_date)='" + Year + "-02') M02, ";
                        sql += " (select coalesce(sum(tr03.amount_base), 0.00) from Transactions tr03 where tr03.category_id=cat.category_id and tr03.trans_type=1 and strftime('%Y-%m', tr03.trans_date)='" + Year + "-03') M03, ";
                        sql += " (select coalesce(sum(tr04.amount_base), 0.00) from Transactions tr04 where tr04.category_id=cat.category_id and tr04.trans_type=1 and strftime('%Y-%m', tr04.trans_date)='" + Year + "-04') M04, ";
                        sql += " (select coalesce(sum(tr05.amount_base), 0.00) from Transactions tr05 where tr05.category_id=cat.category_id and tr05.trans_type=1 and strftime('%Y-%m', tr05.trans_date)='" + Year + "-05') M05, ";
                        sql += " (select coalesce(sum(tr06.amount_base), 0.00) from Transactions tr06 where tr06.category_id=cat.category_id and tr06.trans_type=1 and strftime('%Y-%m', tr06.trans_date)='" + Year + "-06') M06, ";
                        sql += " (select coalesce(sum(tr07.amount_base), 0.00) from Transactions tr07 where tr07.category_id=cat.category_id and tr07.trans_type=1 and strftime('%Y-%m', tr07.trans_date)='" + Year + "-07') M07, ";
                        sql += " (select coalesce(sum(tr08.amount_base), 0.00) from Transactions tr08 where tr08.category_id=cat.category_id and tr08.trans_type=1 and strftime('%Y-%m', tr08.trans_date)='" + Year + "-08') M08, ";
                        sql += " (select coalesce(sum(tr09.amount_base), 0.00) from Transactions tr09 where tr09.category_id=cat.category_id and tr09.trans_type=1 and strftime('%Y-%m', tr09.trans_date)='" + Year + "-09') M09, ";
                        sql += " (select coalesce(sum(tr10.amount_base), 0.00) from Transactions tr10 where tr10.category_id=cat.category_id and tr10.trans_type=1 and strftime('%Y-%m', tr10.trans_date)='" + Year + "-10') M10, ";
                        sql += " (select coalesce(sum(tr11.amount_base), 0.00) from Transactions tr11 where tr11.category_id=cat.category_id and tr11.trans_type=1 and strftime('%Y-%m', tr11.trans_date)='" + Year + "-11') M11, ";
                        sql += " (select coalesce(sum(tr12.amount_base), 0.00) from Transactions tr12 where tr12.category_id=cat.category_id and tr12.trans_type=1 and strftime('%Y-%m', tr12.trans_date)='" + Year + "-12') M12 ";
                        sql += " from Categories cat, Categories pcat ";
                        sql += " where cat.pcategory_id = pcat.category_id ";
                        sql += " and cat.category_id not in (select exclude_id from ReportExcludes where report_id = " + ReportId.ToString("G19") + ")";
                        sql += " and (M01>0 or M02>0 or M03>0 or M04>0 or M05>0 or M06>0 or M07>0 or M08>0 or M09>0 or M10>0 or M11>0 or M12>0) ";
                        sql += " order by CName";
                    }
                    else
                    {
                        sql = "select cat.category_id CategoryID, cat.category_name CName, ";
                        sql += " (select coalesce(sum(tr01.amount_base), 0.00) from Transactions tr01, Categories c01 where tr01.category_id=c01.category_id and c01.pcategory_id=cat.category_id and tr01.trans_type=1 and tr01.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr01.trans_date, '%Y-%m')='" + Year + "-01') M01, ";
                        sql += " (select coalesce(sum(tr02.amount_base), 0.00) from Transactions tr02, Categories c02 where tr02.category_id=c02.category_id and c02.pcategory_id=cat.category_id and tr02.trans_type=1 and tr02.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr02.trans_date, '%Y-%m')='" + Year + "-02') M02, ";
                        sql += " (select coalesce(sum(tr03.amount_base), 0.00) from Transactions tr03, Categories c03 where tr03.category_id=c03.category_id and c03.pcategory_id=cat.category_id and tr03.trans_type=1 and tr03.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr03.trans_date, '%Y-%m')='" + Year + "-03') M03, ";
                        sql += " (select coalesce(sum(tr04.amount_base), 0.00) from Transactions tr04, Categories c04 where tr04.category_id=c04.category_id and c04.pcategory_id=cat.category_id and tr04.trans_type=1 and tr04.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr04.trans_date, '%Y-%m')='" + Year + "-04') M04, ";
                        sql += " (select coalesce(sum(tr05.amount_base), 0.00) from Transactions tr05, Categories c05 where tr05.category_id=c05.category_id and c05.pcategory_id=cat.category_id and tr05.trans_type=1 and tr05.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr05.trans_date, '%Y-%m')='" + Year + "-05') M05, ";
                        sql += " (select coalesce(sum(tr06.amount_base), 0.00) from Transactions tr06, Categories c06 where tr06.category_id=c06.category_id and c06.pcategory_id=cat.category_id and tr06.trans_type=1 and tr06.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr06.trans_date, '%Y-%m')='" + Year + "-06') M06, ";
                        sql += " (select coalesce(sum(tr07.amount_base), 0.00) from Transactions tr07, Categories c07 where tr07.category_id=c07.category_id and c07.pcategory_id=cat.category_id and tr07.trans_type=1 and tr07.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr07.trans_date, '%Y-%m')='" + Year + "-07') M07, ";
                        sql += " (select coalesce(sum(tr08.amount_base), 0.00) from Transactions tr08, Categories c08 where tr08.category_id=c08.category_id and c08.pcategory_id=cat.category_id and tr08.trans_type=1 and tr08.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr08.trans_date, '%Y-%m')='" + Year + "-08') M08, ";
                        sql += " (select coalesce(sum(tr09.amount_base), 0.00) from Transactions tr09, Categories c09 where tr09.category_id=c09.category_id and c09.pcategory_id=cat.category_id and tr09.trans_type=1 and tr09.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr09.trans_date, '%Y-%m')='" + Year + "-09') M09, ";
                        sql += " (select coalesce(sum(tr10.amount_base), 0.00) from Transactions tr10, Categories c10 where tr10.category_id=c10.category_id and c10.pcategory_id=cat.category_id and tr10.trans_type=1 and tr10.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr10.trans_date, '%Y-%m')='" + Year + "-10') M10, ";
                        sql += " (select coalesce(sum(tr11.amount_base), 0.00) from Transactions tr11, Categories c11 where tr11.category_id=c11.category_id and c11.pcategory_id=cat.category_id and tr11.trans_type=1 and tr11.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr11.trans_date, '%Y-%m')='" + Year + "-11') M11, ";
                        sql += " (select coalesce(sum(tr12.amount_base), 0.00) from Transactions tr12, Categories c12 where tr12.category_id=c12.category_id and c12.pcategory_id=cat.category_id and tr12.trans_type=1 and tr12.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") and date_format(tr12.trans_date, '%Y-%m')='" + Year + "-12') M12 ";
                        sql += " from Categories cat ";
                        sql += " where cat.pcategory_id = 0 ";
                        sql += " and (M01>0 or M02>0 or M03>0 or M04>0 or M05>0 or M06>0 or M07>0 or M08>0 or M09>0 or M10>0 or M11>0 or M12>0) ";
                        sql += " order by CName";
                    }
                    nextStep = GetDataSet(ref ds, "ReportData", sql);

                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@Year", Year },
                        { "@IsDetailed", IsDetaled ? 1 : 0 }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.ReportCostSharingGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool ReportIncommingsAndExpenses(ref DataSet ds, long ReportId, int Year, int DetailedMonth, string TableName = "ReportData")
        {
            bool nextStep = false;
            if (ds.Tables.Contains("ReportData"))
                ds.Tables.Remove("ReportData");
            ds.Tables.Add("ReportData");
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string ym, sql;

                    ds.Tables["ReportData"].Columns.Add("Month", Type.GetType("System.String"));
                    ds.Tables["ReportData"].Columns.Add("Incomings", Type.GetType("System.Double"));
                    ds.Tables["ReportData"].Columns.Add("Expenses", Type.GetType("System.Double"));
                    ds.Tables["ReportData"].Columns.Add("Diff", Type.GetType("System.Double"), "[Incomings] - [Expenses]");
                    for (int i = 1; i < 13; i++)
                    {
                        ym = Year.ToString() + "-" + ((i < 10) ? "0" : "") + i.ToString();
                        sql = $"select '{ym}' as Month, coalesce((select sum(amount_base) from Transactions where trans_type=0 and strftime('%Y-%m', trans_date)='{ym}' and category_id not in (select exclude_id from ReportExcludes where report_id={ReportId})), 0.00) as Incomings, coalesce((select sum(amount_base) from Transactions where trans_type=1 and strftime('%Y-%m', trans_date)='{ym}' and category_id not in (select exclude_id from ReportExcludes where report_id={ReportId})), 0.00) as Expenses ";
                        if (GetDataSet(ref ds, "ReportDataMonth", sql))
                        {
                            DataRow[] r = ds.Tables["ReportDataMonth"].Select();
                            for (int j = 0; j < r.Length; j++)
                                ds.Tables["ReportData"].ImportRow(r[j]);
                            if (DetailedMonth == i)
                            {
                                sql = "select '   '||cat.category_name Month, ";
                                sql += " coalesce((select sum(tr01.amount_base) from Transactions tr01, Categories c01 where strftime('%Y-%m', tr01.trans_date)='" + ym + "' and tr01.category_id=c01.category_id and c01.pcategory_id=cat.category_id and tr01.trans_type=0), 0.00) as Incomings, ";
                                sql += " coalesce((select sum(tr02.amount_base) from Transactions tr02, Categories c02 where strftime('%Y-%m', tr02.trans_date)='" + ym + "' and tr02.category_id=c02.category_id and c02.pcategory_id=cat.category_id and tr02.trans_type=1), 0.00) as Expenses ";
                                sql += " from Categories cat ";
                                sql += " where cat.pcategory_id=0 ";
                                sql += " and cat.category_id not in (select exclude_id from ReportExcludes where report_id=" + ReportId + ") ";
                                sql += " and (Incomings>0 or Expenses>0) ";
                                sql += " order by 1 ";

                                if (GetDataSet(ref ds, "ReportDataMonthExpanded", sql))
                                {
                                    DataRow[] r1 = ds.Tables["ReportDataMonthExpanded"].Select();
                                    for (int j = 0; j < r1.Length; j++)
                                        ds.Tables["ReportData"].ImportRow(r1[j]);
                                }
                            }
                        }
                    }
                    nextStep = true;
                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@Year", Year }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.ReportIncomingsAndExpensesGet", sqlProp);
                    if (nextStep)
                    {
                        if (DetailedMonth > 0)
                        {
                            sqlProp.Add("@DetailedMonth", DetailedMonth);
                            if (dbtools.GetDataSet(ref ds, "ReportDataMonthExpanded", "dbo.ReportIncomingsAndExpensesDetailedGet", sqlProp))
                            {
                                DataRow[] r1 = ds.Tables["ReportDataMonthExpanded"].Select();
                                for (int j = 0; j < r1.Length; j++)
                                    ds.Tables["ReportData"].ImportRow(r1[j]);
                            }
                        }
                    }
                    else
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool ReportRevenue(ref DataSet ds, long ReportId, int Year, string TableName = "ReportData")
        {
            bool nextStep = false;
            switch (DBType)
            {
                case DBTypes.SQLite:

                    string sql = "select cat.category_id CategoryID, pcat.category_name||' - '||cat.category_name CName, ";
                    sql += " (select coalesce(sum(tr01.amount_base), 0.00) from Transactions tr01 where tr01.category_id=cat.category_id and tr01.trans_type=0 and strftime('%Y-%m', tr01.trans_date)='" + Year + "-01') M01, ";
                    sql += " (select coalesce(sum(tr02.amount_base), 0.00) from Transactions tr02 where tr02.category_id=cat.category_id and tr02.trans_type=0 and strftime('%Y-%m', tr02.trans_date)='" + Year + "-02') M02, ";
                    sql += " (select coalesce(sum(tr03.amount_base), 0.00) from Transactions tr03 where tr03.category_id=cat.category_id and tr03.trans_type=0 and strftime('%Y-%m', tr03.trans_date)='" + Year + "-03') M03, ";
                    sql += " (select coalesce(sum(tr04.amount_base), 0.00) from Transactions tr04 where tr04.category_id=cat.category_id and tr04.trans_type=0 and strftime('%Y-%m', tr04.trans_date)='" + Year + "-04') M04, ";
                    sql += " (select coalesce(sum(tr05.amount_base), 0.00) from Transactions tr05 where tr05.category_id=cat.category_id and tr05.trans_type=0 and strftime('%Y-%m', tr05.trans_date)='" + Year + "-05') M05, ";
                    sql += " (select coalesce(sum(tr06.amount_base), 0.00) from Transactions tr06 where tr06.category_id=cat.category_id and tr06.trans_type=0 and strftime('%Y-%m', tr06.trans_date)='" + Year + "-06') M06, ";
                    sql += " (select coalesce(sum(tr07.amount_base), 0.00) from Transactions tr07 where tr07.category_id=cat.category_id and tr07.trans_type=0 and strftime('%Y-%m', tr07.trans_date)='" + Year + "-07') M07, ";
                    sql += " (select coalesce(sum(tr08.amount_base), 0.00) from Transactions tr08 where tr08.category_id=cat.category_id and tr08.trans_type=0 and strftime('%Y-%m', tr08.trans_date)='" + Year + "-08') M08, ";
                    sql += " (select coalesce(sum(tr09.amount_base), 0.00) from Transactions tr09 where tr09.category_id=cat.category_id and tr09.trans_type=0 and strftime('%Y-%m', tr09.trans_date)='" + Year + "-09') M09, ";
                    sql += " (select coalesce(sum(tr10.amount_base), 0.00) from Transactions tr10 where tr10.category_id=cat.category_id and tr10.trans_type=0 and strftime('%Y-%m', tr10.trans_date)='" + Year + "-10') M10, ";
                    sql += " (select coalesce(sum(tr11.amount_base), 0.00) from Transactions tr11 where tr11.category_id=cat.category_id and tr11.trans_type=0 and strftime('%Y-%m', tr11.trans_date)='" + Year + "-11') M11, ";
                    sql += " (select coalesce(sum(tr12.amount_base), 0.00) from Transactions tr12 where tr12.category_id=cat.category_id and tr12.trans_type=0 and strftime('%Y-%m', tr12.trans_date)='" + Year + "-12') M12 ";
                    sql += " from Categories cat, Categories pcat ";
                    sql += " where cat.pcategory_id=pcat.category_id ";
                    sql += " and cat.category_id not in (select exclude_id from ReportExcludes where report_id = 3) ";
                    sql += " and (M01>0 or M02>0 or M03>0 or M04>0 or M05>0 or M06>0 or M07>0 or M08>0 or M09>0 or M10>0 or M11>0 or M12>0) ";
                    sql += " order by CName";
                    nextStep = GetDataSet(ref ds, "ReportData", sql);
                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@Year", Year }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.ReportRevenueGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool ReportPayers(ref DataSet ds, long ReportId, int Year, string TableName = "ReportData")
        {
            bool nextStep = false;
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql = "select payer_name CName, ";
                    sql += " (select coalesce(sum(tr01.amount_base), 0.00) from Transactions tr01 where tr01.payer_id=p.payer_id and tr01.trans_type=0 and strftime('%Y-%m', tr01.trans_date)='" + Year + "-01') M01, ";
                    sql += " (select coalesce(sum(tr02.amount_base), 0.00) from Transactions tr02 where tr02.payer_id=p.payer_id and tr02.trans_type=0 and strftime('%Y-%m', tr02.trans_date)='" + Year + "-02') M02, ";
                    sql += " (select coalesce(sum(tr03.amount_base), 0.00) from Transactions tr03 where tr03.payer_id=p.payer_id and tr03.trans_type=0 and strftime('%Y-%m', tr03.trans_date)='" + Year + "-03') M03, ";
                    sql += " (select coalesce(sum(tr04.amount_base), 0.00) from Transactions tr04 where tr04.payer_id=p.payer_id and tr04.trans_type=0 and strftime('%Y-%m', tr04.trans_date)='" + Year + "-04') M04, ";
                    sql += " (select coalesce(sum(tr05.amount_base), 0.00) from Transactions tr05 where tr05.payer_id=p.payer_id and tr05.trans_type=0 and strftime('%Y-%m', tr05.trans_date)='" + Year + "-05') M05, ";
                    sql += " (select coalesce(sum(tr06.amount_base), 0.00) from Transactions tr06 where tr06.payer_id=p.payer_id and tr06.trans_type=0 and strftime('%Y-%m', tr06.trans_date)='" + Year + "-06') M06, ";
                    sql += " (select coalesce(sum(tr07.amount_base), 0.00) from Transactions tr07 where tr07.payer_id=p.payer_id and tr07.trans_type=0 and strftime('%Y-%m', tr07.trans_date)='" + Year + "-07') M07, ";
                    sql += " (select coalesce(sum(tr08.amount_base), 0.00) from Transactions tr08 where tr08.payer_id=p.payer_id and tr08.trans_type=0 and strftime('%Y-%m', tr08.trans_date)='" + Year + "-08') M08, ";
                    sql += " (select coalesce(sum(tr09.amount_base), 0.00) from Transactions tr09 where tr09.payer_id=p.payer_id and tr09.trans_type=0 and strftime('%Y-%m', tr09.trans_date)='" + Year + "-09') M09, ";
                    sql += " (select coalesce(sum(tr10.amount_base), 0.00) from Transactions tr10 where tr10.payer_id=p.payer_id and tr10.trans_type=0 and strftime('%Y-%m', tr10.trans_date)='" + Year + "-10') M10, ";
                    sql += " (select coalesce(sum(tr11.amount_base), 0.00) from Transactions tr11 where tr11.payer_id=p.payer_id and tr11.trans_type=0 and strftime('%Y-%m', tr11.trans_date)='" + Year + "-11') M11, ";
                    sql += " (select coalesce(sum(tr12.amount_base), 0.00) from Transactions tr12 where tr12.payer_id=p.payer_id and tr12.trans_type=0 and strftime('%Y-%m', tr12.trans_date)='" + Year + "-12') M12 ";
                    sql += " from Payers p ";
                    sql += " where (M01>0 or M02>0 or M03>0 or M04>0 or M05>0 or M06>0 or M07>0 or M08>0 or M09>0 or M10>0 or M11>0 or M12>0) ";
                    sql += " order by 1 ";
                    nextStep = GetDataSet(ref ds, "ReportData", sql);
                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@Year", Year }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.ReportPayersGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        public bool ReportPayees(ref DataSet ds, long ReportId, int Year, string TableName = "ReportData")
        {
            bool nextStep = false;
            switch (DBType)
            {
                case DBTypes.SQLite:
                    string sql = "select payer_name CName, ";
                    sql += " (select coalesce(sum(tr01.amount_base), 0.00) from Transactions tr01 where tr01.payer_id=p.payer_id and tr01.trans_type=1 and strftime('%Y-%m', tr01.trans_date)='" + Year + "-01') M01, ";
                    sql += " (select coalesce(sum(tr02.amount_base), 0.00) from Transactions tr02 where tr02.payer_id=p.payer_id and tr02.trans_type=1 and strftime('%Y-%m', tr02.trans_date)='" + Year + "-02') M02, ";
                    sql += " (select coalesce(sum(tr03.amount_base), 0.00) from Transactions tr03 where tr03.payer_id=p.payer_id and tr03.trans_type=1 and strftime('%Y-%m', tr03.trans_date)='" + Year + "-03') M03, ";
                    sql += " (select coalesce(sum(tr04.amount_base), 0.00) from Transactions tr04 where tr04.payer_id=p.payer_id and tr04.trans_type=1 and strftime('%Y-%m', tr04.trans_date)='" + Year + "-04') M04, ";
                    sql += " (select coalesce(sum(tr05.amount_base), 0.00) from Transactions tr05 where tr05.payer_id=p.payer_id and tr05.trans_type=1 and strftime('%Y-%m', tr05.trans_date)='" + Year + "-05') M05, ";
                    sql += " (select coalesce(sum(tr06.amount_base), 0.00) from Transactions tr06 where tr06.payer_id=p.payer_id and tr06.trans_type=1 and strftime('%Y-%m', tr06.trans_date)='" + Year + "-06') M06, ";
                    sql += " (select coalesce(sum(tr07.amount_base), 0.00) from Transactions tr07 where tr07.payer_id=p.payer_id and tr07.trans_type=1 and strftime('%Y-%m', tr07.trans_date)='" + Year + "-07') M07, ";
                    sql += " (select coalesce(sum(tr08.amount_base), 0.00) from Transactions tr08 where tr08.payer_id=p.payer_id and tr08.trans_type=1 and strftime('%Y-%m', tr08.trans_date)='" + Year + "-08') M08, ";
                    sql += " (select coalesce(sum(tr09.amount_base), 0.00) from Transactions tr09 where tr09.payer_id=p.payer_id and tr09.trans_type=1 and strftime('%Y-%m', tr09.trans_date)='" + Year + "-09') M09, ";
                    sql += " (select coalesce(sum(tr10.amount_base), 0.00) from Transactions tr10 where tr10.payer_id=p.payer_id and tr10.trans_type=1 and strftime('%Y-%m', tr10.trans_date)='" + Year + "-10') M10, ";
                    sql += " (select coalesce(sum(tr11.amount_base), 0.00) from Transactions tr11 where tr11.payer_id=p.payer_id and tr11.trans_type=1 and strftime('%Y-%m', tr11.trans_date)='" + Year + "-11') M11, ";
                    sql += " (select coalesce(sum(tr12.amount_base), 0.00) from Transactions tr12 where tr12.payer_id=p.payer_id and tr12.trans_type=1 and strftime('%Y-%m', tr12.trans_date)='" + Year + "-12') M12 ";
                    sql += " from Payers p ";
                    sql += " where (M01>0 or M02>0 or M03>0 or M04>0 or M05>0 or M06>0 or M07>0 or M08>0 or M09>0 or M10>0 or M11>0 or M12>0) ";
                    sql += " order by 1 ";
                    nextStep = GetDataSet(ref ds, "ReportData", sql);
                    break;
                case DBTypes.MSSQL:
                    PropertyCollection sqlProp = new PropertyCollection
                    {
                        { "@Year", Year }
                    };
                    MSSQLUtils dbtools = new MSSQLUtils(ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, TableName, "dbo.ReportPayeesGet", sqlProp);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            return nextStep;
        }
        #endregion
    }
}
