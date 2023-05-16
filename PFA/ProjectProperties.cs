using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace EvgTar.PFA
{
    public partial class ProjectProperties : Form
    {
        public Core pfa_core;
        public DataSet ds = new DataSet();
        public Languages NLS = null;
        public string DBFile = "";

        public ProjectProperties()
        {
            InitializeComponent();
        }
        public void Init()
        {
            this.Text = NLS.LNGetString("TitleProjectProperties", "Project Properties");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            labelBaseCurrency.Text = NLS.LNGetString("TextBaseCurrency", "Base currency") + ":";
            labelDBFile.Text = NLS.LNGetString("TextDatabaseFile", "Database file") + ":";

            textBoxDBFile.Text = DBFile;
            if (File.Exists(DBFile))
            {
                try
                {
                    FileInfo fi = new FileInfo(DBFile);
                    if (fi != null)
                    {
                        textBoxDBFile.Text += "\r\n" + NLS.LNGetString("TextFileSize", "File size") + ": " + (Convert.ToDouble(fi.Length) / 1024.0).ToString("f2") + NLS.LNGetString("TextKByte", "KB");
                    }
                }
                catch { }
            }
            if (pfa_core == null)
                return;
            if (pfa_core.CurrenciesGet(ref ds, false))
            {
                comboBoxCurrencies.Items.Clear();
                for (int i = 0; i < ds.Tables["Currencies"].Rows.Count; i++)
                {
                    comboBoxCurrencies.Items.Add(ds.Tables["Currencies"].Rows[i]["currency_code"].ToString() + " - " + ds.Tables["Currencies"].Rows[i]["currency_name"].ToString());
                    if (ds.Tables["Currencies"].Rows[i]["currency_code"].ToString().Equals(pfa_core.base_currency))
                        comboBoxCurrencies.SelectedIndex = i;
                }
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
            /*SQLite sqlite = new SQLite(ConnectionString);
            string sql = "update Settings set setting_value='" + ds.Tables["Currencies"].Rows[comboBoxCurrencies.SelectedIndex]["currency_code"].ToString() + "' where setting_name = 'base_currency'";
            if (sqlite.ExecuteSQL(sql) != 1)
                MessageBox.Show(sqlite.LastError);*/
        }
    }
}