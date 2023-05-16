using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Xml;
using System.Net;
using System.Threading;

using EvgTar.PFA.Properties;
using EvgTar.PFA.Core;

namespace EvgTar.PFA
{
    public partial class MainForm : Form
    {
        Settings settings = new Settings(AppDomain.CurrentDomain.BaseDirectory + @"\pfa.xml");
        //public CFileLog log = null;
        private DataSet ds = new DataSet();
        Core.Core pfa_core;
        private Languages NLS;
        NotifyIcon ni = new NotifyIcon();
        ContextMenuStrip ni_CMS = new ContextMenuStrip();
        ContextMenuStrip contextMenuStripReports = new ContextMenuStrip();
        bool init = true;
        int ReportID = 1;
        private string expand = "";
        Thread get_currency_rates_thread;
        public event EventHandler Executed;
        string LastError = "";

        public MainForm()
        {
            InitializeComponent();
            NLS = new Languages(settings.Language);
            ni_CMS.Items.Add(NLS.LNGetString("TextOpen", "Open"), null, new EventHandler(ni_DoubleClick));
            ni_CMS.Items.Add(NLS.LNGetString("MainMenuExit", "Exit"), Resources.close, new EventHandler(exitToolStripMenuItem_Click));
            InitNLS();
            if (settings.LogFileName.Equals(""))
                settings.LogFileName = AppDomain.CurrentDomain.BaseDirectory + "pfa.log";
            //log = new CFileLog(settings.LogFileName);
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                settings.DB = args[1];
            pfa_core = new(settings.DB)
            {
                DBType = settings.DBType
            };
            switch (settings.DBType)
            {
                case DBTypes.SQLite:
                    if ((!settings.DB.Equals("")) && (File.Exists(settings.DB)))
                    {
                        pfa_core.ConnectionString = "Data Source = " + settings.DB;
                        if (pfa_core.LoadDB(ref ds, settings.DB))
                            Init();
                    }
                    break;
                case DBTypes.MSSQL:
                    pfa_core.Init();
                    Init();
                    break;
            }
            init = true;
            dateTimePickerFrom.Format = dateTimePickerTill.Format = DateTimePickerFormat.Custom;
            dateTimePickerFrom.CustomFormat = dateTimePickerTill.CustomFormat = "dd MMMM yyyy";
            dateTimePickerFrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0);
            dateTimePickerTill.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
            checkBoxStartupShow.Checked = settings.ShowToday;
            init = false;
            if (settings.WindMaximized)
                this.WindowState = FormWindowState.Maximized;
            else
                this.Size = new Size(settings.WindWidth, settings.WindHeight);

            ni.Icon = this.Icon;
            ni.DoubleClick += new EventHandler(ni_DoubleClick);
            ni.ContextMenuStrip = ni_CMS;

            dataGridViewTransactions.Columns.Clear();
        }
        private void InitNLS()
        {
            this.Text = NLS.LNGetString("Title", "Personal Finance Assistant");
            ni.Text = NLS.LNGetString("Title", "Personal Finance Assistant");
            ni_CMS.Items[0].Text = NLS.LNGetString("TextOpen", "Open");
            ni_CMS.Items[1].Text = NLS.LNGetString("MainMenuExit", "Exit");
            ToolStripMenuItemFile.Text = NLS.LNGetString("MainMenuFile", "File");
            ToolStripMenuItemNewProject.Text = NLS.LNGetString("MainMenuProjectNew", "New Project ...");
            ToolStripMenuItemOpenProject.Text = NLS.LNGetString("MainMenuProjectOpen", "Open Project ...");
            ToolStripMenuItemProjectProperties.Text = NLS.LNGetString("MainMenuProjectProperties", "Project Properties");
            ToolStripMenuItemExit.Text = NLS.LNGetString("MainMenuExit", "Exit");
            ToolStripMenuItemTools.Text = NLS.LNGetString("MainMenuTools", "Tools");
            ToolStripMenuItemNewTransaction.Text = NLS.LNGetString("MainMenuNewTransaction", "New Transaction ...");
            ToolStripMenuItemAccounts.Text = NLS.LNGetString("MainMenuAccounts", "Accounts ...");
            ToolStripMenuItemCategories.Text = NLS.LNGetString("MainMenuCategories", "Categories ...");
            ToolStripMenuItemCurrencies.Text = NLS.LNGetString("MainMenuCurrencies", "Currencies ...");
            ToolStripMenuItemPayers.Text = NLS.LNGetString("MainMenuPayers", "Payers ...");
            ToolStripMenuItemOptions.Text = NLS.LNGetString("MainMenuOptions", "Options ...");
            ToolStripMenuItemHelp.Text = NLS.LNGetString("MainMenuHelp", "Help");
            ToolStripMenuItemAbout.Text = NLS.LNGetString("MainMenuAbout", "About");
            labelAccount.Text = NLS.LNGetString("TextAccount", "Account");
            labelDateFrom.Text = NLS.LNGetString("TextDateFrom", "Date from:");
            labelDateTill.Text = NLS.LNGetString("TextTill", "till:");
            buttonAddTrans.Text = NLS.LNGetString("TextAdd", "Add");
            buttonTransDelete.Text = NLS.LNGetString("TextDelete", "Delete");
            buttonTransfer.Text = NLS.LNGetString("TextTransfer", "Transfer");
            transferToolStripMenuItem.Text = NLS.LNGetString("TextTransfer", "Transfer") + "...";
            tabPageToday.Text = NLS.LNGetString("TitleToday", "Today");
            toolStripButtonAccounts.Text = NLS.LNGetString("MainMenuAccounts", "Accounts");
            toolStripButtonCategories.Text = NLS.LNGetString("MainMenuCategories", "Categories ...");
            toolStripButtonCurrencies.Text = NLS.LNGetString("MainMenuCurrencies", "Currencies ...");
            toolStripButtonPayers.Text = NLS.LNGetString("MainMenuPayers", "Payers ...");
            labelCategory.Text = NLS.LNGetString("TextCategory", "Category") + ":";
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (settings.ShowToday)
            {
                tabControl1.SelectedIndex = 0;
                ShowToday();
            }
            else
                tabControl1.SelectedIndex = 1;
        }
        private void ni_DoubleClick(object sender, EventArgs e)
        {
            Show();
            ni.Visible = false;
            WindowState = settings.WindMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ni.Visible = false;
            Close();
        }
        private void Init()
        {
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString))
                return;
            init = true;
            toolStripStatusLabelBaseCurrency.Text = NLS.LNGetString("TextBaseCurrency", "Base currency") + ": " + pfa_core.base_currency;
            #region Transactions tab
            bool nextStep = pfa_core.AccountsGet(ref ds);
            if (nextStep)
            {
                comboBoxAccount.BeginUpdate();
                comboBoxAccount.Items.Clear();
                comboBoxAccount.Items.Add(NLS.LNGetString("TextAllAccounts", "All accounts"));
                for (int i = 0; i < ds.Tables["Accounts"].Rows.Count; i++)
                    comboBoxAccount.Items.Add(ds.Tables["Accounts"].Rows[i]["account_name"].ToString());
                comboBoxAccount.EndUpdate();
                comboBoxAccount.SelectedIndex = 0;
            }
            nextStep = pfa_core.CategoriesGet(ref ds);
            if (nextStep)
            {
                comboBoxCategory.BeginUpdate();
                comboBoxCategory.Items.Clear();
                comboBoxCategory.Items.Add(NLS.LNGetString("TextAllCategories", "All categories"));
                for (int i = 0; i < ds.Tables["Categories"].Rows.Count; i++)
                    comboBoxCategory.Items.Add(ds.Tables["Categories"].Rows[i]["CName"].ToString());
                comboBoxCategory.EndUpdate();
                comboBoxCategory.SelectedIndex = 0;
            }
            else
            {
                //if (settings.Debug)
                //    log.WriteLine(LastError);
            }
            nextStep = pfa_core.PayersGet(ref ds);
            if (nextStep)
            {
                comboBoxPayers.BeginUpdate();
                comboBoxPayers.Items.Clear();
                comboBoxPayers.Items.Add(NLS.LNGetString("TextAllPayers", "All payers"));
                for (int i = 0; i < ds.Tables["TransPayers"].Rows.Count; i++)
                    comboBoxPayers.Items.Add(ds.Tables["TransPayers"].Rows[i]["payer_name"].ToString());
                comboBoxPayers.EndUpdate();
                comboBoxPayers.SelectedIndex = 0;
            }
            else
            {
                //if (settings.Debug)
                //    log.WriteLine(LastError);
            }
            nextStep = pfa_core.AssetsGet(ref ds);
            if (nextStep)
            {
                comboBoxAssets.BeginUpdate();
                comboBoxAssets.Items.Clear();
                comboBoxAssets.Items.Add(NLS.LNGetString("TextAllAssets", "All assets"));
                for (int i = 0; i < ds.Tables["Assets"].Rows.Count; i++)
                    comboBoxAssets.Items.Add(ds.Tables["Assets"].Rows[i]["AssetName"].ToString());
                comboBoxAssets.EndUpdate();
                comboBoxAssets.SelectedIndex = 0;
            }
            else
            {
                //if (settings.Debug)
                //    log.WriteLine(LastError);
            }
            #endregion

            ToolStripMenuItemAccounts.Enabled = true;
            ToolStripMenuItemCategories.Enabled = true;
            ToolStripMenuItemCurrencies.Enabled = true;
            ToolStripMenuItemProjectProperties.Enabled = true;
            ToolStripMenuItemNewTransaction.Enabled = true;
            ToolStripMenuItemPayers.Enabled = true;
            buttonAddTrans.Enabled = true;
            toolStripButtonAccounts.Enabled = true;
            toolStripButtonCategories.Enabled = true;
            toolStripButtonCurrencies.Enabled = true;
            toolStripButtonPayers.Enabled = true;
            budgetToolStripMenuItem.Enabled = true;
            transferToolStripMenuItem.Enabled = buttonTransfer.Enabled = true;

            #region Report tab
            labelYear.Text = NLS.LNGetString("TextYear", "Year") + ":";
            checkBoxDetailed.Text = NLS.LNGetString("TextDetailed", "Detailed");
            buttonRemoveCategoryFromReport.Text = NLS.LNGetString("CMenuRemoveCategoryFromReport", "Remove category from report");
            buttonClearFilters.Text = NLS.LNGetString("CMenuClearFilters", "Clear Filters");
            nextStep = pfa_core.TransactionsYearsGet(ref ds);

            comboBoxYear.Items.Clear();
            if (nextStep && (ds.Tables["Years"].Rows.Count > 0))
            {
                for (int i = 0; i < ds.Tables["Years"].Rows.Count; i++)
                {
                    int ind = comboBoxYear.Items.Add(ds.Tables["Years"].Rows[i]["YR"]);
                    if (comboBoxYear.Items[ind].ToString().Equals(DateTime.Now.Year.ToString()))
                        comboBoxYear.SelectedIndex = ind;
                }
            }
            else
                comboBoxYear.Items.Add(DateTime.Now.Year);

            comboBoxReports.Items.Clear();
            comboBoxReports.Items.Add(NLS.LNGetString("TitleCostSharing", "Cost Sharing"));
            comboBoxReports.Items.Add(NLS.LNGetString("TitleIncomingsAndExpenses", "Incomings and Expenses"));
            comboBoxReports.Items.Add(NLS.LNGetString("TitleReportRevenue", "Revenue"));
            comboBoxReports.Items.Add(NLS.LNGetString("TitleReportPayers", "Payers"));
            comboBoxReports.Items.Add(NLS.LNGetString("TitleReportPayees", "Payees"));
            comboBoxReports.SelectedIndex = (ReportID == 0) ? 0 : ReportID - 1;
            #endregion

            get_currency_rates_thread = new Thread(new ThreadStart(GetCurrencyRateThreadExecute))
            {
                IsBackground = true
            };
            buttonUpdCurrencyRates.Enabled = false;
            try
            {
                get_currency_rates_thread.Start();
            }
            catch { };
            Executed += new EventHandler(GetCurrencyRateThreadExecuted);

            init = false;
        }
        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AccountsForm accounts = new AccountsForm())
            {
                accounts.pfa_core = pfa_core;
                accounts.ds = ds;
                accounts.NLS = NLS;
                if (accounts.Init())
                    accounts.ShowDialog();
            }
            Init();
        }
        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CategoriesForm categories = new CategoriesForm())
            {
                categories.pfa_core = pfa_core;
                categories.NLS = NLS;
                if (categories.Init())
                    categories.ShowDialog();
            }
            //Init();
        }
        private void currenciesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CurrenciesForm currencies = new CurrenciesForm())
            {
                currencies.pfa_core = pfa_core;
                currencies.ds = ds;
                currencies.NLS = NLS;
                if (currencies.Init())
                    currencies.ShowDialog();
            }
        }
        private void ToolStripMenuItemNewProject_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = "pfa";
                sfd.AddExtension = true;
                sfd.Title = NLS.LNGetString("TitleCreateProject", "Create new project ...");
                sfd.Filter = "Personal Finance Assistant database (*.pfa)|*.pfa|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    settings.DB = sfd.FileName;
                    sfd.Dispose();
                    this.Refresh();
                    if (pfa_core.CreateDB(settings.DB))
                        settings.Save();
                }
            }
        }
        private void ToolStripMenuItemOpenProject_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.DefaultExt = "pfa";
                ofd.Title = NLS.LNGetString("TitleOpenProject", "Open project ...");
                ofd.Filter = "Personal Finance Assistant database (*.pfa)|*.pfa|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    settings.DB = ofd.FileName;
                    pfa_core.ConnectionString = "Data Source = " + ofd.FileName;
                    settings.Save();
                    if (pfa_core.LoadDB(ref ds, settings.DB))
                        Init();
                }
            }
        }
        private void buttonAddTrans_Click(object sender, EventArgs e)
        {
            using (TransactionForm transaction = new TransactionForm()
            {
                pfa_core = pfa_core,
                NLS = NLS,
                LogFileName = settings.LogFileName,
                Debug = settings.Debug
            })
            {
                if (transaction.Init())
                {
                    if (transaction.ShowDialog() == DialogResult.OK)
                    {
                        ShowTransactions();
                    }
                }
            }
        }
        private void newTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buttonAddTrans_Click(sender, e);
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Options options = new Options())
            {
                options.pfa_core = pfa_core;
                options.ds = ds;
                options.settings = settings;
                options.NLS = NLS;
                options.Init();
                if (options.ShowDialog() == DialogResult.OK)
                {
                    NLS = new Languages(settings.Language);
                    InitNLS();
                }
            }
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if ((this.WindowState == FormWindowState.Minimized) && settings.HideInTaskbar)
            {
                ni.Visible = true;
                Hide();
            }
            else
            {
                settings.WindHeight = this.Size.Height;
                settings.WindWidth = this.Size.Width;
                settings.WindMaximized = (this.WindowState == FormWindowState.Maximized);
                settings.Save();
            }
        }
        private void projectPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ProjectProperties projectproperties = new ProjectProperties())
            {
                projectproperties.pfa_core = pfa_core;
                projectproperties.ds = ds;
                projectproperties.NLS = NLS;
                projectproperties.DBFile = settings.DB;
                projectproperties.Init();
                if (projectproperties.ShowDialog() == DialogResult.OK)
                {
                    pfa_core.GetDataSet(ref ds, "Settings", "select * from Settings");
                    DataRow[] dr = ds.Tables["Settings"].Select("setting_name = 'base_currency'");
                    if (dr.Length > 0)
                        pfa_core.base_currency = dr[0]["setting_value"].ToString();
                }
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.RowIndex >= dataGridViewTransactions.Rows.Count))
            {
                return;
            }
            using (TransactionForm transaction = new TransactionForm())
            {
                transaction.pfa_core = pfa_core;
                transaction.trans_id = (long)dataGridViewTransactions.Rows[e.RowIndex].Cells["trans_id"].Value;
                transaction.NLS = NLS;
                if (transaction.Init())
                {
                    if (transaction.ShowDialog() == DialogResult.OK)
                    {
                        //Init();
                        ShowTransactions();
                    }
                }
            }
        }
        private void buttonTransDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewTransactions.SelectedRows.Count > 0)
            {
                if (MessageBox.Show(NLS.LNGetString("QuestionDeleteTransaction", "Do you want to delete this transaction?"), NLS.LNGetString("TitleDeleteTransaction", "Delete transaction"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    bool nextStep = pfa_core.TransactionDelete((long)dataGridViewTransactions.Rows[dataGridViewTransactions.SelectedCells[0].RowIndex].Cells["trans_id"].Value);
                    if (nextStep)
                    {
                        if (pfa_core.RecalcAccount(dataGridViewTransactions.Rows[dataGridViewTransactions.SelectedCells[0].RowIndex].Cells["account_id"].Value.ToString(), dataGridViewTransactions.Rows[dataGridViewTransactions.SelectedCells[0].RowIndex].Cells["currency_rate"].Value.ToString()))
                        {
                            ShowTransactions();
                        }
                        else
                            MessageBox.Show(pfa_core.LastError);
                    }
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex < 0) || (e.RowIndex >= dataGridViewTransactions.Rows.Count))
                return;
            buttonTransDelete.Enabled = true;
        }
        private void comboBoxAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init)
                ShowTransactions();
        }
        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            if (!init)
                ShowTransactions();
        }
        private void dateTimePickerTill_ValueChanged(object sender, EventArgs e)
        {
            if (!init)
                ShowTransactions();
        }
        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init)
                ShowTransactions();
        }

        private void comboBoxPayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init)
                ShowTransactions();
        }
        private void ShowToday()
        {
            bool nextStep = pfa_core.AccountsInfoGet(ref ds, checkBoxAllAccounts.Checked ? -1 : 1);
            if (nextStep)
            {
                dataGridViewToday.Columns.Clear();
                ds.Tables["AccountsInfo"].Rows.Add(new object[] { null, NLS.LNGetString("TextTotal", "Total") + ":", pfa_core.base_currency });
                BindingSource bs = new BindingSource(ds, "AccountsInfo");
                dataGridViewToday.DataSource = bs;
                dataGridViewToday.Columns["account_id"].Visible = false;
                dataGridViewToday.Columns["account_name"].HeaderText = NLS.LNGetString("TextAccount", "Account");
                dataGridViewToday.Columns["account_name"].Width = 200;
                dataGridViewToday.Columns["currency_code"].HeaderText = NLS.LNGetString("TextCurrency", "Currency");
                dataGridViewToday.Columns["currency_code"].Width = 75;
                dataGridViewToday.Columns["currency_rate"].HeaderText = NLS.LNGetString("TextCurrencyRate", "Currency rate");
                dataGridViewToday.Columns["currency_rate"].Width = 100;
                dataGridViewToday.Columns["local_balance"].HeaderText = NLS.LNGetString("TextSummary", "Summary");
                dataGridViewToday.Columns["local_balance"].Width = 75;
                dataGridViewToday.Columns["local_balance"].DefaultCellStyle.Format = "f2";
                dataGridViewToday.Columns["base_balance"].Visible = false;
                //dataGridViewToday.Columns["base_balance"].HeaderText = NLS.LNGetString("TextSummary", "Summary") + " (" + base_currency + ")";
                //dataGridViewToday.Columns["base_balance"].Width = 120;
                //dataGridViewToday.Columns["base_balance"].DefaultCellStyle.Format = "f2";
                dataGridViewToday.Columns["base_calc"].HeaderText = NLS.LNGetString("TextSummary", "Summary") + " (" + pfa_core.base_currency + ")";
                dataGridViewToday.Columns["base_calc"].Width = 120;
                dataGridViewToday.Columns["base_calc"].DefaultCellStyle.Format = "f2";
                dataGridViewToday.Columns["account_checkdt"].HeaderText = NLS.LNGetString("TextChecked", "Checked");
                dataGridViewToday.Columns["account_checkdt"].Width = 120;
                #region Total Row
                int TotalRowIndex = 0;
                if (dataGridViewToday.Rows.Count > 1)
                    TotalRowIndex = dataGridViewToday.Rows.Count - 1;
                dataGridViewToday.Rows[TotalRowIndex].Cells["base_balance"].Value = ds.Tables["AccountsInfo"].Compute("Sum([base_balance])", "");
                dataGridViewToday.Rows[TotalRowIndex].Cells["base_calc"].Value = ds.Tables["AccountsInfo"].Compute("Sum([base_calc])", "");
                #endregion
            }
            else
            {
                //if (settings.Debug)
                //{
                //    new CFileLog(settings.LogFileName).WriteLine(LastError);
                //}
            }

            DataGridViewCellStyle hst = new DataGridViewCellStyle(dataGridViewToday.ColumnHeadersDefaultCellStyle)
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font(dataGridViewToday.ColumnHeadersDefaultCellStyle.Font, FontStyle.Bold),
                BackColor = Color.FromArgb(192, 192, 255)
            };
            dataGridViewToday.ColumnHeadersDefaultCellStyle = hst;
            #region Average block
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            nextStep = pfa_core.AccountsInfoAvgGet(ref ds, dt);
            if (nextStep)
            {
                try
                {
                    labelAVG.Text = NLS.LNGetString("TextAVGPerDay", "Average payments per day") + ": " + (Convert.ToDouble(ds.Tables["AverageInfo"].Compute("Avg([Total])", "")) / DateTime.Now.Day).ToString("f2") + " " + pfa_core.base_currency;
                    labelCMExpenses.Text = NLS.LNGetString("TextCMExpenses", "Current month expenses") + ": " + Convert.ToDouble(ds.Tables["AverageInfo"].Rows[0]["Total"]).ToString("f2") + " " + pfa_core.base_currency;
                }
                catch { }
            }
            else
            {
                //if (settings.Debug)
                //    log.WriteLine(pfa_core.LastError);
            }
            #endregion

            DataGridViewCellStyle cs_highlight = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(settings.CellHColor[0], settings.CellHColor[1], settings.CellHColor[2])
            };
            for (int i = 0; i < dataGridViewToday.Rows.Count; i++)
            {
                if (i % 2 == 0)
                    dataGridViewToday.Rows[i].DefaultCellStyle = cs_highlight;
            }
            int TotalRowInd = 0;
            if (dataGridViewToday.Rows.Count > 1)
                TotalRowInd = dataGridViewToday.Rows.Count - 1;
            try
            {
                DataGridViewCellStyle cell_total = new DataGridViewCellStyle(dataGridViewToday.DefaultCellStyle);
                cell_total.Font = new Font(cell_total.Font, FontStyle.Bold);
                dataGridViewToday.Rows[TotalRowInd].DefaultCellStyle = cell_total;
                cell_total.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewToday.Rows[TotalRowInd].Cells["account_name"].Style = cell_total;
                dataGridViewToday.Rows[TotalRowInd].Cells["currency_code"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewToday.Columns["currency_code"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewToday.Columns["local_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewToday.Columns["base_balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewToday.Columns["base_calc"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            catch { }

            buttonUpdCurrencyRates.Enabled = true;
        }
        private void ShowTransactions()
        {
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString) || init)
                return;
            List<string> visibleColumns = new()
            {
                "DT",
                "account_name",
                "currency_code",
                "CName",
                "deposit",
                "withdraw",
                "payer_name",
                "asset_name",
                "trans_notes"
            };

            bool nextStep = pfa_core.TransactionsGet(
                ref ds,
                dateTimePickerFrom.Value,
                dateTimePickerTill.Value,
                textBoxTag.Text.Trim(),
                comboBoxAccount.SelectedIndex > 0 ? (long)ds.Tables["Accounts"].Rows[comboBoxAccount.SelectedIndex - 1]["account_id"] : -1,
                comboBoxCategory.SelectedIndex > 0 ? (long)ds.Tables["Categories"].Rows[comboBoxCategory.SelectedIndex - 1]["CategoryID"] : -1,
                comboBoxPayers.SelectedIndex > 0 ? (long)ds.Tables["TransPayers"].Rows[comboBoxPayers.SelectedIndex - 1]["payer_id"] : -1,
                comboBoxAssets.SelectedIndex > 0 ? (long)ds.Tables["Assets"].Rows[comboBoxAssets.SelectedIndex - 1]["AssetId"] : -1
               );
            if (nextStep)
            {
                BindingSource bs = new BindingSource(ds, "Transactions");
                dataGridViewTransactions.DataSource = bs;

                for (int c = 0; c < dataGridViewTransactions.Columns.Count; c++)
                {
                    if (!visibleColumns.Contains(dataGridViewTransactions.Columns[c].Name))
                        dataGridViewTransactions.Columns[c].Visible = false;
                }

                dataGridViewTransactions.Columns["DT"].HeaderText = NLS.LNGetString("TextDate", "Date");
                dataGridViewTransactions.Columns["DT"].Width = 70;
                dataGridViewTransactions.Columns["account_name"].HeaderText = NLS.LNGetString("TextAccount", "Account");
                dataGridViewTransactions.Columns["currency_code"].HeaderText = NLS.LNGetString("TextCurrency", "Currency");
                dataGridViewTransactions.Columns["currency_code"].Width = 75;
                dataGridViewTransactions.Columns["CName"].HeaderText = NLS.LNGetString("TextCategory", "Category");
                dataGridViewTransactions.Columns["CName"].Width = 250;
                dataGridViewTransactions.Columns["deposit"].HeaderText = NLS.LNGetString("TextDeposit", "Deposit");
                dataGridViewTransactions.Columns["deposit"].Width = 90;
                dataGridViewTransactions.Columns["deposit"].DefaultCellStyle.Format = "f2";
                dataGridViewTransactions.Columns["deposit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewTransactions.Columns["withdraw"].HeaderText = NLS.LNGetString("TextWithdrawal", "Withdrawal");
                dataGridViewTransactions.Columns["withdraw"].Width = 90;
                dataGridViewTransactions.Columns["withdraw"].DefaultCellStyle.Format = "f2";
                dataGridViewTransactions.Columns["withdraw"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewTransactions.Columns["payer_name"].HeaderText = NLS.LNGetString("TextPayer", "Payer");
                dataGridViewTransactions.Columns["asset_name"].HeaderText = NLS.LNGetString("TextAssetName", "Asset");
                dataGridViewTransactions.Columns["asset_name"].Width = 150;
                dataGridViewTransactions.Columns["trans_notes"].HeaderText = NLS.LNGetString("TextNotes", "Notes");
                dataGridViewTransactions.Columns["trans_notes"].Width = this.Size.Width - 980;
            }
            else
            {
                //if (settings.Debug)
                //    log.WriteLine(pfa_core.LastError);
            }
            DataGridViewCellStyle hst = new DataGridViewCellStyle(dataGridViewTransactions.ColumnHeadersDefaultCellStyle)
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font(dataGridViewTransactions.ColumnHeadersDefaultCellStyle.Font, FontStyle.Bold),
                BackColor = Color.FromArgb(settings.HeaderBackColor[0], settings.HeaderBackColor[1], settings.HeaderBackColor[2])
            };
            dataGridViewTransactions.ColumnHeadersDefaultCellStyle = hst;
            ShowAccountsTotal();

            DataGridViewCellStyle cs_highlight = new DataGridViewCellStyle();
            DataGridViewCellStyle cs = new DataGridViewCellStyle();
            cs_highlight.BackColor = Color.FromArgb(settings.CellHColor[0], settings.CellHColor[1], settings.CellHColor[2]);
            cs.BackColor = Color.FromArgb(settings.CellColor[0], settings.CellColor[1], settings.CellColor[2]);
            for (int i = 0; i < dataGridViewTransactions.Rows.Count; i++)
            {
                dataGridViewTransactions.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                if (dataGridViewTransactions.Rows[i].Cells["withdraw"].Value != null)
                    if (dataGridViewTransactions.Rows[i].Cells["withdraw"].Value.ToString().Equals("0"))
                        dataGridViewTransactions.Rows[i].Cells["withdraw"].Value = DBNull.Value;
                if (dataGridViewTransactions.Rows[i].Cells["deposit"].Value != null)
                    if (dataGridViewTransactions.Rows[i].Cells["deposit"].Value.ToString().Equals("0"))
                        dataGridViewTransactions.Rows[i].Cells["deposit"].Value = DBNull.Value;
            }

            if (dataGridViewTransactions.Rows.Count > 1)
                dataGridViewTransactions.CurrentCell = dataGridViewTransactions.Rows[dataGridViewTransactions.Rows.Count - 1].Cells["DT"];
            toolStripStatusLabelLinesCount.Text = NLS.LNGetString("TextLines", "Lines:") + " " + dataGridViewTransactions.Rows.Count.ToString();
        }
        private void ShowAccountsTotal()
        {
            toolStripStatusLabelTotal.Text = NLS.LNGetString("TextTotal", "Total") + ": " + pfa_core.AccountsInfoTotalGet().ToString("f2");
        }
        private void ShowReport()
        {
            buttonRemoveCategoryFromReport.Visible = false;
            buttonClearFilters.Visible = false;
            checkBoxDetailed.Visible = false;
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString))
                return;
            dataGridViewReports.ContextMenuStrip = null;
            switch (ReportID)
            {
                case 1:
                    buttonRemoveCategoryFromReport.Visible = true;
                    buttonClearFilters.Visible = true;
                    checkBoxDetailed.Visible = true;
                    ReportCostSharing();
                    break;
                case 2:
                    ReportIncomingsAndExpenses();
                    break;
                case 3:
                    buttonRemoveCategoryFromReport.Visible = true;
                    buttonClearFilters.Visible = true;
                    ReportRevenue();
                    break;
                case 4:
                    ReportPayers();
                    break;
                case 5:
                    ReportPayees();
                    break;
            }

            DataGridViewCellStyle hst = new DataGridViewCellStyle(dataGridViewReports.ColumnHeadersDefaultCellStyle)
            {
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = new Font(dataGridViewReports.ColumnHeadersDefaultCellStyle.Font, FontStyle.Bold),
                BackColor = Color.FromArgb(settings.HeaderBackColor[0], settings.HeaderBackColor[1], settings.HeaderBackColor[2])
            };
            dataGridViewReports.ColumnHeadersDefaultCellStyle = hst;
            //loaded = true;
            ShowReportLoadComplete();
            contextMenuStripReports.Items.Clear();
            if (buttonRemoveCategoryFromReport.Visible)
            {
                contextMenuStripReports.Items.Add(NLS.LNGetString("CMenuRemoveCategoryFromReport", "Remove category from report"), null, new EventHandler(RemoveCategoryFromReport));
                contextMenuStripReports.Items.Add(NLS.LNGetString("ShowFilters", "Show filters"), null, new EventHandler(ShowFilters));
            }
            dataGridViewReports.ContextMenuStrip = contextMenuStripReports;
        }
        private void ShowReportLoadComplete()
        {
            DataGridViewCellStyle cs = new DataGridViewCellStyle();
            DataGridViewCellStyle cs2 = new DataGridViewCellStyle();
            DataGridViewCellStyle cs_highlight = new DataGridViewCellStyle();
            DataGridViewCellStyle cs_highlight2 = new DataGridViewCellStyle();
            DataGridViewCellStyle cell_total = new DataGridViewCellStyle(dataGridViewReports.DefaultCellStyle);
            DataGridViewCellStyle cell_total_hl = new DataGridViewCellStyle(dataGridViewReports.DefaultCellStyle);
            cs.BackColor = Color.FromArgb(settings.CellColor[0], settings.CellColor[1], settings.CellColor[2]);
            cs2.BackColor = Color.FromArgb(245, 245, 245);
            cs_highlight.BackColor = Color.FromArgb(settings.CellHColor[0], settings.CellHColor[1], settings.CellHColor[2]);
            cs_highlight2.BackColor = Color.FromArgb(240, 240, 240);
            cell_total.Font = new Font(cell_total.Font, FontStyle.Bold);
            cell_total_hl.BackColor = Color.FromArgb(settings.CellHColor[0], settings.CellHColor[1], settings.CellHColor[2]);
            cell_total_hl.Font = new Font(cell_total.Font, FontStyle.Bold);
            int TotalRowIndex = 0;
            if (dataGridViewReports.Rows.Count > 1)
                TotalRowIndex = dataGridViewReports.Rows.Count - 1;
            switch (ReportID)
            {
                case 1:
                    for (int i = 0; i < dataGridViewReports.Rows.Count; i++)
                    {
                        dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                        try
                        {
                            for (int j = 1; j < 13; j++)
                            {
                                if ((dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value != null) && (dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value.ToString().Equals("0")))
                                    dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value = DBNull.Value;
                            }
                        }
                        catch { }
                    }
                    #region Total Row
                    if (TotalRowIndex > 0)
                    {
                        dataGridViewReports.Rows[TotalRowIndex].DefaultCellStyle = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        cell_total.Alignment = DataGridViewContentAlignment.MiddleRight;
                        cell_total_hl.Alignment = DataGridViewContentAlignment.MiddleRight;
                        try
                        {
                            dataGridViewReports.Rows[TotalRowIndex].Cells["CName"].Style = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        }
                        catch { }
                    }
                    #endregion
                    break;
                case 2:
                    for (int i = 0; i < dataGridViewReports.Rows.Count; i++)
                    {
                        try
                        {
                            if (dataGridViewReports.Rows[i].Cells[0].Value.ToString().StartsWith("  "))
                                dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight2 : cs2;
                            else
                                dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                            if ((dataGridViewReports.Rows[i].Cells["Incomings"].Value != null) && (dataGridViewReports.Rows[i].Cells["Incomings"].Value.ToString().Equals("0")) && (dataGridViewReports.Rows[i].Cells["Expenses"].Value != null) && (dataGridViewReports.Rows[i].Cells["Expenses"].Value.ToString().Equals("0")))
                            {
                                dataGridViewReports.Rows[i].Cells["Incomings"].Value = DBNull.Value;
                                dataGridViewReports.Rows[i].Cells["Expenses"].Value = DBNull.Value;
                            }
                        }
                        catch { }
                    }
                    #region Total Row
                    if (TotalRowIndex > 0)
                    {
                        dataGridViewReports.Rows[TotalRowIndex].DefaultCellStyle = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        cell_total.Alignment = DataGridViewContentAlignment.MiddleRight;
                        cell_total_hl.Alignment = DataGridViewContentAlignment.MiddleRight;
                        try
                        {
                            dataGridViewReports.Rows[TotalRowIndex].Cells["Month"].Style = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        }
                        catch { }
                    }
                    #endregion
                    break;
                case 3:
                    for (int i = 0; i < dataGridViewReports.Rows.Count; i++)
                    {
                        dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                        try
                        {
                            for (int j = 1; j < 13; j++)
                            {
                                if ((dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value != null) && (dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value.ToString().Equals("0")))
                                    dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value = DBNull.Value;
                            }
                        }
                        catch { }
                    }
                    #region Total Row
                    if (TotalRowIndex > 0)
                    {
                        dataGridViewReports.Rows[TotalRowIndex].DefaultCellStyle = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        cell_total.Alignment = DataGridViewContentAlignment.MiddleRight;
                        cell_total_hl.Alignment = DataGridViewContentAlignment.MiddleRight;
                        try
                        {
                            dataGridViewReports.Rows[TotalRowIndex].Cells["CName"].Style = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        }
                        catch { }
                    }
                    #endregion
                    break;
                case 4:
                    for (int i = 0; i < dataGridViewReports.Rows.Count; i++)
                    {
                        dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                        try
                        {
                            for (int j = 1; j < 13; j++)
                            {
                                if ((dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value != null) && (dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value.ToString().Equals("0")))
                                    dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value = DBNull.Value;
                            }
                        }
                        catch { }
                    }
                    #region Total Row
                    if (TotalRowIndex > 0)
                    {
                        dataGridViewReports.Rows[TotalRowIndex].DefaultCellStyle = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        cell_total.Alignment = DataGridViewContentAlignment.MiddleRight;
                        cell_total_hl.Alignment = DataGridViewContentAlignment.MiddleRight;
                        try
                        {
                            dataGridViewReports.Rows[TotalRowIndex].Cells["CName"].Style = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        }
                        catch { }
                    }
                    #endregion
                    break;
                case 5:
                    for (int i = 0; i < dataGridViewReports.Rows.Count; i++)
                    {
                        dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                        try
                        {
                            for (int j = 1; j < 13; j++)
                            {
                                if ((dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value != null) && (dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value.ToString().Equals("0")))
                                    dataGridViewReports.Rows[i].Cells["M" + ((j < 10) ? "0" : "") + j.ToString()].Value = DBNull.Value;
                            }
                        }
                        catch { }
                    }
                    #region Total Row
                    if (TotalRowIndex > 0)
                    {
                        dataGridViewReports.Rows[TotalRowIndex].DefaultCellStyle = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        cell_total.Alignment = DataGridViewContentAlignment.MiddleRight;
                        cell_total_hl.Alignment = DataGridViewContentAlignment.MiddleRight;
                        try
                        {
                            dataGridViewReports.Rows[TotalRowIndex].Cells["CName"].Style = (TotalRowIndex % 2 == 0) ? cell_total_hl : cell_total;
                        }
                        catch { }
                    }
                    #endregion
                    break;
                default:
                    for (int i = 0; i < dataGridViewReports.Rows.Count; i++)
                        dataGridViewReports.Rows[i].DefaultCellStyle = (i % 2 == 0) ? cs_highlight : cs;
                    break;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using AboutForm about = new()
            {
                NLS = NLS
            };
            about.Init();
            about.ShowDialog();
        }
        private void ToolStripMenuItemPayers_Click(object sender, EventArgs e)
        {
            using Payers payers = new();
            payers.pfa_core = pfa_core;
            payers.NLS = NLS;
            payers.Init();
            payers.ShowDialog();
        }
        private void toolStripButtonAccounts_Click(object sender, EventArgs e)
        {
            accountToolStripMenuItem_Click(sender, e);
        }
        private void toolStripButtonCategories_Click(object sender, EventArgs e)
        {
            categoriesToolStripMenuItem_Click(sender, e);
        }
        private void toolStripButtonCurrencies_Click(object sender, EventArgs e)
        {
            currenciesToolStripMenuItem_Click(sender, e);
        }
        private void toolStripButtonPayers_Click(object sender, EventArgs e)
        {
            ToolStripMenuItemPayers_Click(sender, e);
        }        
        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            using (TransferForm transfer = new TransferForm())
            {
                transfer.pfa_core = pfa_core;
                transfer.NLS = NLS;
                if (transfer.Init())
                {
                    if (transfer.ShowDialog() == DialogResult.OK)
                        ShowTransactions();
                }
            }
        }
        private void checkBoxStartupShow_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShowToday = checkBoxStartupShow.Checked;
            settings.Save();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    ShowToday();
                    break;
                case 1:
                    ShowTransactions();
                    break;
                case 2:
                    ShowReport();
                    break;
            }
        }
        private void comboBoxReports_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init)
            {
                ReportID = comboBoxReports.SelectedIndex + 1;
                expand = "";
                ShowReport();
            }
        }
        private void ShowFilters(object sender, EventArgs e)
        {
            string sql = "select category_name from Categories, ReportExcludes where report_id = " + ReportID.ToString() + " and exclude_id = category_id";
            if (pfa_core.GetDataSet(ref ds, "Filters", sql))
            {
                string s = "";
                for (int i = 0; i < ds.Tables["Filters"].Rows.Count; i++)
                    s += ds.Tables["Filters"].Rows[i]["category_name"].ToString() + "\n";
                MessageBox.Show(s, NLS.LNGetString("Filters", "Filters"), MessageBoxButtons.OK);
            }
        }
        private void ReportCostSharing()
        {
            int year = DateTime.Now.Year;
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString))
                return;
            if (pfa_core.GetCount("ReportExcludes", "where report_id=" + ReportID.ToString()) > 0)
            {
                contextMenuStripReports.Items.Add(NLS.LNGetString("CMenuClearFilters", "Clear Filters"), null, new EventHandler(ClearFilters));
                buttonClearFilters.Enabled = true;
            }
            else
                buttonClearFilters.Enabled = false;
            if (comboBoxYear.SelectedIndex >= 0)
                year = (int)comboBoxYear.SelectedItem;

            if (pfa_core.ReportCostSharing(ref ds, ReportID, year, checkBoxDetailed.Checked))
            {
                if (ds.Tables["ReportData"].Rows.Count > 0)
                    ds.Tables["ReportData"].Rows.Add(new object[] { null, NLS.LNGetString("TextTotal", "Total") + ":" });
                BindingSource bs = new BindingSource(ds, "ReportData");
                dataGridViewReports.DataSource = bs;
                dataGridViewReports.DefaultCellStyle.Format = "f2";
                dataGridViewReports.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewReports.Columns["CName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewReports.Columns["CName"].HeaderText = NLS.LNGetString("TextCategory", "Category");
                dataGridViewReports.Columns["M01"].HeaderText = NLS.LNGetString("TextJanuary", "January");
                dataGridViewReports.Columns["M02"].HeaderText = NLS.LNGetString("TextFebruary", "February");
                dataGridViewReports.Columns["M03"].HeaderText = NLS.LNGetString("TextMarch", "March");
                dataGridViewReports.Columns["M04"].HeaderText = NLS.LNGetString("TextApril", "April");
                dataGridViewReports.Columns["M05"].HeaderText = NLS.LNGetString("TextMay", "May");
                dataGridViewReports.Columns["M06"].HeaderText = NLS.LNGetString("TextJune", "June");
                dataGridViewReports.Columns["M07"].HeaderText = NLS.LNGetString("TextJuly", "July");
                dataGridViewReports.Columns["M08"].HeaderText = NLS.LNGetString("TextAugust", "August");
                dataGridViewReports.Columns["M09"].HeaderText = NLS.LNGetString("TextSeptember", "September");
                dataGridViewReports.Columns["M10"].HeaderText = NLS.LNGetString("TextOctober", "October");
                dataGridViewReports.Columns["M11"].HeaderText = NLS.LNGetString("TextNovember", "November");
                dataGridViewReports.Columns["M12"].HeaderText = NLS.LNGetString("TextDecember", "December");
                dataGridViewReports.Columns["CName"].Width = 180;
                dataGridViewReports.Columns["M01"].Width = 70;
                dataGridViewReports.Columns["M02"].Width = 70;
                dataGridViewReports.Columns["M03"].Width = 70;
                dataGridViewReports.Columns["M04"].Width = 70;
                dataGridViewReports.Columns["M05"].Width = 70;
                dataGridViewReports.Columns["M06"].Width = 70;
                dataGridViewReports.Columns["M07"].Width = 70;
                dataGridViewReports.Columns["M08"].Width = 70;
                dataGridViewReports.Columns["M09"].Width = 70;
                dataGridViewReports.Columns["M10"].Width = 70;
                dataGridViewReports.Columns["M11"].Width = 70;
                dataGridViewReports.Columns["M12"].Width = 70;

                #region Total Row
                int TotalRowIndex = 0;
                if (dataGridViewReports.Rows.Count > 1)
                {
                    TotalRowIndex = dataGridViewReports.Rows.Count - 1;
                    for (int m = 1; m < 13; m++)
                        dataGridViewReports.Rows[TotalRowIndex].Cells["M" + ((m < 10) ? "0" : "") + m.ToString()].Value = ds.Tables["ReportData"].Compute("Sum([" + ("M" + ((m < 10) ? "0" : "") + m.ToString()) + "])", "");
                }
                #endregion
                dataGridViewReports.Columns["CategoryID"].Visible = false;
            }
            else
            {
                //if (settings.Debug)
                //{
                //    log.WriteLine(pfa_core.LastError);
                //}
            };
        }
        private void ReportIncomingsAndExpenses()
        {
            contextMenuStripReports.Items.Clear();
            int year = (comboBoxYear.SelectedIndex >= 0) ? (int)comboBoxYear.SelectedItem : DateTime.Now.Year;
            int detailedMonth = -1;
            if (!string.IsNullOrWhiteSpace(expand))
            {
                int.TryParse(expand.Split("-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault(), out detailedMonth);
            }
            if (pfa_core.ReportIncommingsAndExpenses(ref ds, ReportID, year, detailedMonth))
            {
                if (ds.Tables["ReportData"].Rows.Count > 0)
                    ds.Tables["ReportData"].Rows.Add(new object[] { NLS.LNGetString("TextTotal", "Total") + ":" });
                BindingSource bs = new BindingSource(ds, "ReportData");
                dataGridViewReports.DataSource = bs;
                dataGridViewReports.DefaultCellStyle.Format = "f2";
                dataGridViewReports.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewReports.Columns["Month"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewReports.Columns["Month"].HeaderText = NLS.LNGetString("TextMonth", "Month");
                dataGridViewReports.Columns["Month"].Width = expand.Equals("") ? 100 : 250;
                dataGridViewReports.Columns["Incomings"].HeaderText = NLS.LNGetString("TextIncomings", "Incomings");
                dataGridViewReports.Columns["Expenses"].HeaderText = NLS.LNGetString("TextExpenses", "Expenses");
                dataGridViewReports.Columns["Diff"].HeaderText = NLS.LNGetString("TextDifference", "Difference");
                #region Total Row
                int TotalRowIndex = 1;
                if (dataGridViewReports.Rows.Count > 1)
                    TotalRowIndex = dataGridViewReports.Rows.Count - 1;
                dataGridViewReports.Rows[TotalRowIndex].Cells["Incomings"].Value = ds.Tables["ReportData"].Compute("Sum([Incomings])", "SUBSTRING([Month], 1, 1)<>' '");
                dataGridViewReports.Rows[TotalRowIndex].Cells["Expenses"].Value = ds.Tables["ReportData"].Compute("Sum([Expenses])", "SUBSTRING([Month], 1, 1)<>' '");
                dataGridViewReports.Rows[TotalRowIndex].Cells["Diff"].Value = ds.Tables["ReportData"].Compute("Sum([Diff])", "SUBSTRING([Month], 1, 1)<>' '");

                #endregion
            }
        }
        private void ReportRevenue()
        {
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString))
                return;
            int year = (comboBoxYear.SelectedIndex >= 0) ? (int)comboBoxYear.SelectedItem : DateTime.Now.Year;
            if (pfa_core.GetCount("ReportExcludes", "where report_id=" + ReportID.ToString()) > 0)
            {
                contextMenuStripReports.Items.Add(NLS.LNGetString("CMenuClearFilters", "Clear Filters"), null, new EventHandler(ClearFilters));
                buttonClearFilters.Enabled = true;
            }
            else
                buttonClearFilters.Enabled = false;

            if (pfa_core.ReportRevenue(ref ds, ReportID, year))
            {
                if (ds.Tables["ReportData"].Rows.Count > 0)
                    ds.Tables["ReportData"].Rows.Add(new object[] { null, NLS.LNGetString("TextTotal", "Total") + ":" });
                BindingSource bs = new BindingSource(ds, "ReportData");
                dataGridViewReports.DataSource = bs;
                dataGridViewReports.DefaultCellStyle.Format = "f2";
                dataGridViewReports.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewReports.Columns["CName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewReports.Columns["CName"].HeaderText = NLS.LNGetString("TextCategory", "Category");
                dataGridViewReports.Columns["M01"].HeaderText = NLS.LNGetString("TextJanuary", "January");
                dataGridViewReports.Columns["M02"].HeaderText = NLS.LNGetString("TextFebruary", "February");
                dataGridViewReports.Columns["M03"].HeaderText = NLS.LNGetString("TextMarch", "March");
                dataGridViewReports.Columns["M04"].HeaderText = NLS.LNGetString("TextApril", "April");
                dataGridViewReports.Columns["M05"].HeaderText = NLS.LNGetString("TextMay", "May");
                dataGridViewReports.Columns["M06"].HeaderText = NLS.LNGetString("TextJune", "June");
                dataGridViewReports.Columns["M07"].HeaderText = NLS.LNGetString("TextJuly", "July");
                dataGridViewReports.Columns["M08"].HeaderText = NLS.LNGetString("TextAugust", "August");
                dataGridViewReports.Columns["M09"].HeaderText = NLS.LNGetString("TextSeptember", "September");
                dataGridViewReports.Columns["M10"].HeaderText = NLS.LNGetString("TextOctober", "October");
                dataGridViewReports.Columns["M11"].HeaderText = NLS.LNGetString("TextNovember", "November");
                dataGridViewReports.Columns["M12"].HeaderText = NLS.LNGetString("TextDecember", "December");
                dataGridViewReports.Columns["CName"].Width = 180;
                dataGridViewReports.Columns["M01"].Width = 70;
                dataGridViewReports.Columns["M02"].Width = 70;
                dataGridViewReports.Columns["M03"].Width = 70;
                dataGridViewReports.Columns["M04"].Width = 70;
                dataGridViewReports.Columns["M05"].Width = 70;
                dataGridViewReports.Columns["M06"].Width = 70;
                dataGridViewReports.Columns["M07"].Width = 70;
                dataGridViewReports.Columns["M08"].Width = 70;
                dataGridViewReports.Columns["M09"].Width = 70;
                dataGridViewReports.Columns["M10"].Width = 70;
                dataGridViewReports.Columns["M11"].Width = 70;
                dataGridViewReports.Columns["M12"].Width = 70;
                #region Total Row
                int TotalRowIndex = 0;
                if (dataGridViewReports.Rows.Count > 1)
                    TotalRowIndex = dataGridViewReports.Rows.Count - 1;
                for (int m = 1; m < 13; m++)
                    dataGridViewReports.Rows[TotalRowIndex].Cells["M" + ((m < 10) ? "0" : "") + m.ToString()].Value = ds.Tables["ReportData"].Compute("Sum([" + ("M" + ((m < 10) ? "0" : "") + m.ToString()) + "])", "");
                #endregion
                dataGridViewReports.Columns["CategoryID"].Visible = false;
            }
            else
            {
                MessageBox.Show(pfa_core.LastError);
            }
        }
        private void ReportPayers()
        {
            contextMenuStripReports.Items.Clear();
            int year = (comboBoxYear.SelectedIndex >= 0) ? (int)comboBoxYear.SelectedItem : DateTime.Now.Year;
            if (pfa_core.ReportPayers(ref ds, ReportID, year))
            {
                if (ds.Tables["ReportData"].Rows.Count > 0)
                    ds.Tables["ReportData"].Rows.Add(new object[] { NLS.LNGetString("TextTotal", "Total") + ":" });
                BindingSource bs = new BindingSource(ds, "ReportData");
                dataGridViewReports.DataSource = bs;
                dataGridViewReports.DefaultCellStyle.Format = "f2";
                dataGridViewReports.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewReports.Columns["CName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewReports.Columns["CName"].HeaderText = NLS.LNGetString("TextPayer", "Payer");
                dataGridViewReports.Columns["M01"].HeaderText = NLS.LNGetString("TextJanuary", "January");
                dataGridViewReports.Columns["M02"].HeaderText = NLS.LNGetString("TextFebruary", "February");
                dataGridViewReports.Columns["M03"].HeaderText = NLS.LNGetString("TextMarch", "March");
                dataGridViewReports.Columns["M04"].HeaderText = NLS.LNGetString("TextApril", "April");
                dataGridViewReports.Columns["M05"].HeaderText = NLS.LNGetString("TextMay", "May");
                dataGridViewReports.Columns["M06"].HeaderText = NLS.LNGetString("TextJune", "June");
                dataGridViewReports.Columns["M07"].HeaderText = NLS.LNGetString("TextJuly", "July");
                dataGridViewReports.Columns["M08"].HeaderText = NLS.LNGetString("TextAugust", "August");
                dataGridViewReports.Columns["M09"].HeaderText = NLS.LNGetString("TextSeptember", "September");
                dataGridViewReports.Columns["M10"].HeaderText = NLS.LNGetString("TextOctober", "October");
                dataGridViewReports.Columns["M11"].HeaderText = NLS.LNGetString("TextNovember", "November");
                dataGridViewReports.Columns["M12"].HeaderText = NLS.LNGetString("TextDecember", "December");
                dataGridViewReports.Columns["CName"].Width = 180;
                dataGridViewReports.Columns["M01"].Width = 70;
                dataGridViewReports.Columns["M02"].Width = 70;
                dataGridViewReports.Columns["M03"].Width = 70;
                dataGridViewReports.Columns["M04"].Width = 70;
                dataGridViewReports.Columns["M05"].Width = 70;
                dataGridViewReports.Columns["M06"].Width = 70;
                dataGridViewReports.Columns["M07"].Width = 70;
                dataGridViewReports.Columns["M08"].Width = 70;
                dataGridViewReports.Columns["M09"].Width = 70;
                dataGridViewReports.Columns["M10"].Width = 70;
                dataGridViewReports.Columns["M11"].Width = 70;
                dataGridViewReports.Columns["M12"].Width = 70;
                #region Total Row
                int TotalRowIndex = 0;
                if (dataGridViewReports.Rows.Count > 1)
                    TotalRowIndex = dataGridViewReports.Rows.Count - 1;
                for (int m = 1; m < 13; m++)
                    dataGridViewReports.Rows[TotalRowIndex].Cells["M" + ((m < 10) ? "0" : "") + m.ToString()].Value = ds.Tables["ReportData"].Compute("Sum([" + ("M" + ((m < 10) ? "0" : "") + m.ToString()) + "])", "");
                #endregion
            }
            //else
            //    log.WriteLine(LastError);
        }
        private void ReportPayees()
        {
            contextMenuStripReports.Items.Clear();
            int year = (comboBoxYear.SelectedIndex >= 0) ? (int)comboBoxYear.SelectedItem : DateTime.Now.Year;
            if (pfa_core.ReportPayees(ref ds, ReportID, year))
            {
                if (ds.Tables["ReportData"].Rows.Count > 0)
                    ds.Tables["ReportData"].Rows.Add(new object[] { NLS.LNGetString("TextTotal", "Total") + ":" });
                BindingSource bs = new BindingSource(ds, "ReportData");
                dataGridViewReports.DataSource = bs;
                dataGridViewReports.DefaultCellStyle.Format = "f2";
                dataGridViewReports.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewReports.Columns["CName"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewReports.Columns["CName"].HeaderText = NLS.LNGetString("TextPayer", "Payer");
                dataGridViewReports.Columns["M01"].HeaderText = NLS.LNGetString("TextJanuary", "January");
                dataGridViewReports.Columns["M02"].HeaderText = NLS.LNGetString("TextFebruary", "February");
                dataGridViewReports.Columns["M03"].HeaderText = NLS.LNGetString("TextMarch", "March");
                dataGridViewReports.Columns["M04"].HeaderText = NLS.LNGetString("TextApril", "April");
                dataGridViewReports.Columns["M05"].HeaderText = NLS.LNGetString("TextMay", "May");
                dataGridViewReports.Columns["M06"].HeaderText = NLS.LNGetString("TextJune", "June");
                dataGridViewReports.Columns["M07"].HeaderText = NLS.LNGetString("TextJuly", "July");
                dataGridViewReports.Columns["M08"].HeaderText = NLS.LNGetString("TextAugust", "August");
                dataGridViewReports.Columns["M09"].HeaderText = NLS.LNGetString("TextSeptember", "September");
                dataGridViewReports.Columns["M10"].HeaderText = NLS.LNGetString("TextOctober", "October");
                dataGridViewReports.Columns["M11"].HeaderText = NLS.LNGetString("TextNovember", "November");
                dataGridViewReports.Columns["M12"].HeaderText = NLS.LNGetString("TextDecember", "December");
                dataGridViewReports.Columns["CName"].Width = 180;
                dataGridViewReports.Columns["M01"].Width = 70;
                dataGridViewReports.Columns["M02"].Width = 70;
                dataGridViewReports.Columns["M03"].Width = 70;
                dataGridViewReports.Columns["M04"].Width = 70;
                dataGridViewReports.Columns["M05"].Width = 70;
                dataGridViewReports.Columns["M06"].Width = 70;
                dataGridViewReports.Columns["M07"].Width = 70;
                dataGridViewReports.Columns["M08"].Width = 70;
                dataGridViewReports.Columns["M09"].Width = 70;
                dataGridViewReports.Columns["M10"].Width = 70;
                dataGridViewReports.Columns["M11"].Width = 70;
                dataGridViewReports.Columns["M12"].Width = 70;
                #region Total Row
                int TotalRowIndex = 0;
                if (dataGridViewReports.Rows.Count > 1)
                    TotalRowIndex = dataGridViewReports.Rows.Count - 1;
                for (int m = 1; m < 13; m++)
                    dataGridViewReports.Rows[TotalRowIndex].Cells["M" + ((m < 10) ? "0" : "") + m.ToString()].Value = ds.Tables["ReportData"].Compute("Sum([" + ("M" + ((m < 10) ? "0" : "") + m.ToString()) + "])", "");
                #endregion
            }
            //else
            //    log.WriteLine(LastError);
        }
        private double GetTotal()
        {
            double sum = 0;
            for (int i = 0; i < dataGridViewReports.SelectedRows.Count; i++)
            {
                for (int c = 1; c < 13; c++)
                {
                    if ((dataGridViewReports.SelectedRows[i].Cells["M" + ((c < 10) ? "0" : "") + c.ToString()].Value != null) && (!dataGridViewReports.SelectedRows[i].Cells["M" + ((c < 10) ? "0" : "") + c.ToString()].Value.ToString().Equals("")))
                    {
                        try
                        {
                            sum += Convert.ToDouble(dataGridViewReports.SelectedRows[i].Cells["M" + ((c < 10) ? "0" : "") + c.ToString()].Value);
                        }
                        catch { }
                    }
                }
            }
            return sum;
        }
        private void RemoveCategoryFromReport(object sender, EventArgs e)
        {
            string sql;
            bool refresh = false;
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString))
                return;
            for (int i = 0; i < dataGridViewReports.SelectedRows.Count; i++)
            {
                if (dataGridViewReports.SelectedRows[i].Cells["CategoryID"].Value != DBNull.Value)
                {
                    sql = "insert into ReportExcludes (report_id, exclude_id) values (" + ReportID.ToString() + ", " + dataGridViewReports.SelectedRows[i].Cells["CategoryID"].Value.ToString() + ")";
                    if (pfa_core.ExecuteSQL(sql) == 1)
                        refresh = true;
                }
            }
            if (refresh)
                ShowReport();
        }
        private void ClearFilters(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(pfa_core.ConnectionString))
                return;
            string sql = "delete from ReportExcludes where report_id=" + ReportID.ToString();
            if (pfa_core.ExecuteSQL(sql) > 0)
                ShowReport();
        }
        private void dataGridViewReports_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (ReportID != 2)
                return;
            string s = dataGridViewReports.Rows[e.RowIndex].Cells[0].Value.ToString();
            if (s.StartsWith(" "))
                return;
            expand = s.Equals(expand) ? "" : s;
            ShowReport();
        }
        private void dataGridViewReports_Click(object sender, DataGridViewCellEventArgs e)
        {
            //double sum = GetTotal();
            //toolStripStatusLabelAmountSum.Text = (sum > 0) ? NLS.LNGetString("TextTotal", "Total") + ": " + sum.ToString("f2") : "";

        }
        private void dataGridViewToday_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Recalc", null, RecalcAccount_Click);
            if (pfa_core.DBType == DBTypes.MSSQL)
                cms.Items.Add("Update account check date", null, AccountChecked_Click);
            dataGridViewToday.ContextMenuStrip = cms;
        }
        private void RecalcAccount_Click(object sender, EventArgs e)
        {
            if (dataGridViewToday.SelectedCells.Count > 0)
            {
                int row_ind = dataGridViewToday.SelectedCells[0].RowIndex;
                pfa_core.RecalcAccount(dataGridViewToday.Rows[row_ind].Cells["account_id"].Value.ToString(), dataGridViewToday.Rows[row_ind].Cells["currency_rate"].Value.ToString());
                ShowToday();
            }
        }
        private void AccountChecked_Click(object sender, EventArgs e)
        {
            if (dataGridViewToday.SelectedCells.Count > 0)
            {
                int row_ind = dataGridViewToday.SelectedCells[0].RowIndex;
                pfa_core.AccountChecked(dataGridViewToday.Rows[row_ind].Cells["account_id"].Value.ToString());
                ShowToday();
            }
        }
        private void buttonUpdCurrencyRates_Click(object sender, EventArgs e)
        {
            buttonUpdCurrencyRates.Enabled = false;
            try
            {
                get_currency_rates_thread.Start();
            }
            catch { };
        }
        private void GetCurrencyRateThreadExecute()
        {
            GetCurrencyRateBYR();
            GetCurrencyRateRUB();
            GetCurrencyRatePLN();
            GetCurrencyRateUSD_EUR();

            Executed?.Invoke(this, null);
        }
        private void GetCurrencyRateThreadExecuted(object sender, EventArgs e)
        {
            //if (tabControl1.SelectedIndex == 1)
            {
                dataGridViewToday.Invoke(new MethodInvoker(ShowToday));
            }
        }
        /// <summary>�������� ���� ������������ �����</summary>
        private void GetCurrencyRateBYR()
        {
            string crate_v = "";
            String xml_s = "";
            string url = "http://www.nbrb.by/Services/XmlExRates.aspx?ondate=" + DateTime.Now.ToString("MM.dd.yyyy");
            try
            {
                HttpWebRequest web_req = (HttpWebRequest)HttpWebRequest.Create(url);
                web_req.Timeout = 30000;
                web_req.Method = "GET";
                web_req.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse web_res = (HttpWebResponse)web_req.GetResponse();
                using (StreamReader sr = new StreamReader(web_res.GetResponseStream()))
                {
                    xml_s = sr.ReadToEnd();
                    sr.Close();
                }
                web_res.Close();
            }
            catch { }
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xml_s);

                XmlNodeList nl = xml.GetElementsByTagName("Currency");
                bool get_v = false;
                for (int i = 0; i < nl.Count; i++)
                {
                    if (nl[i].HasChildNodes)
                    {
                        for (int c = 0; c < nl[i].ChildNodes.Count; c++)
                        {
                            if (nl[i].ChildNodes[c].LocalName.ToUpper().Equals("CHARCODE"))
                                get_v = nl[i].ChildNodes[c].InnerText.ToUpper().Equals("USD");

                            if (get_v && nl[i].ChildNodes[c].LocalName.ToUpper().Equals("RATE"))
                            {
                                crate_v = nl[i].ChildNodes[c].InnerText;
                                break;
                            }
                        }
                        if (get_v)
                            break;
                    }
                }
                if (!crate_v.Trim().Equals(""))
                {
                    pfa_core.CurrencyRateUpdate("BYN", crate_v);
                }
            }
            catch (Exception xmlEx)
            {
                //log.WriteLine(xmlEx.ToString());
            }
        }
        /// <summary>�������� ���� ����������� �����</summary>
        private void GetCurrencyRateRUB()
        {
            /* 
             * ������ � �������, ����������� ;
             * TICKER;DATE;OPEN;HIGH;LOW;CLOSE;VOL;WAPRICE
             * USD;2008-09-09;;;;25.2626;;
             */
            string xml_s = "";
            string url = "http://export.rbc.ru/free/cb.0/free.fcgi?period=DAILY&tickers=USD&d1=" + DateTime.Now.ToString("dd") + "&m1=" + DateTime.Now.ToString("MM") + "&y1=" + DateTime.Now.ToString("yyyy") + "&d2=" + DateTime.Now.ToString("dd") + "&m2=" + DateTime.Now.ToString("MM") + "&y2=" + DateTime.Now.ToString("yyyy") + "&lastdays=0&separator=%3B&data_format=BROWSER";
            try
            {
                HttpWebRequest web_req = (HttpWebRequest)HttpWebRequest.Create(url);
                web_req.Timeout = 30000; // 30 ������
                web_req.Method = "GET";
                web_req.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse web_res = (HttpWebResponse)web_req.GetResponse();
                using (StreamReader sr = new StreamReader(web_res.GetResponseStream()))
                {
                    xml_s = sr.ReadToEnd(); // ������ � ������ � ���������
                    sr.Close();
                }
                web_res.Close();
            }
            catch { }
            string[] res_v = xml_s.Split(';');
            if (res_v.Length > 6)
            {
                if (!string.IsNullOrEmpty(res_v[5].Trim()))
                {
                    pfa_core.CurrencyRateUpdate("RUB", res_v[5]);
                }
            }
        }
        /// <summary>�������� ���� ��������� �������</summary>
        private void GetCurrencyRatePLN()
        {
            string crate_v_usd = "";
            string crate_v_eur = "";
            string xml_s = "";
            //string url = "http://www.nbp.pl/kursy/xml/a182z080917.xml";
            string url = "http://rss.nbp.pl/kursy/TabelaA.xml";
            try
            {
                HttpWebRequest web_req = (HttpWebRequest)HttpWebRequest.Create(url);
                web_req.Timeout = 30000; // 30 ������
                web_req.Method = "GET";
                web_req.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse web_res = (HttpWebResponse)web_req.GetResponse();
                using (StreamReader sr = new StreamReader(web_res.GetResponseStream()))
                {
                    xml_s = sr.ReadToEnd(); // ������ � ������ � ���������
                    sr.Close();
                }
                web_res.Close();

                try
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(xml_s);
                    XmlNodeList nl = xml.GetElementsByTagName("enclosure");
                    if (nl.Count > 0)
                    {
                        url = nl[0].Attributes["url"].Value;
                    }
                    if (!url.Trim().Equals(""))
                    {
                        web_req = (HttpWebRequest)HttpWebRequest.Create(url);
                        web_res = (HttpWebResponse)web_req.GetResponse();
                        using (StreamReader sr = new StreamReader(web_res.GetResponseStream()))
                        {
                            xml_s = sr.ReadToEnd(); // ������ � ������ � ���������
                            sr.Close();
                        }
                        web_res.Close();
                    }
                    if (xml_s.Trim().Equals(""))
                        return;

                    xml.LoadXml(xml_s);
                    nl = xml.GetElementsByTagName("pozycja");
                    bool get_v_usd = false;
                    bool get_v_eur = false;
                    for (int i = 0; i < nl.Count; i++)
                    {
                        if (nl[i].HasChildNodes)
                        {
                            for (int c = 0; c < nl[i].ChildNodes.Count; c++)
                            {
                                if (nl[i].ChildNodes[c].LocalName.ToUpper().Equals("KOD_WALUTY") && nl[i].ChildNodes[c].InnerText.ToUpper().Equals("USD"))
                                    get_v_usd = true;
                                if (nl[i].ChildNodes[c].LocalName.ToUpper().Equals("KOD_WALUTY") && nl[i].ChildNodes[c].InnerText.ToUpper().Equals("EUR"))
                                    get_v_eur = true;

                                if (get_v_usd && nl[i].ChildNodes[c].LocalName.ToUpper().Equals("KURS_SREDNI"))
                                {
                                    crate_v_usd = nl[i].ChildNodes[c].InnerText.Replace(",", ".");
                                    get_v_usd = false;
                                    //break;
                                }

                                if (get_v_eur && nl[i].ChildNodes[c].LocalName.ToUpper().Equals("KURS_SREDNI"))
                                {
                                    crate_v_eur = nl[i].ChildNodes[c].InnerText.Replace(",", ".");
                                    get_v_eur = false;
                                    //break;
                                }
                            }
                            if (!crate_v_eur.Equals("") && !crate_v_eur.Equals(""))
                                break;
                        }
                    }
                    Core.Core core = new(pfa_core.ConnectionString) { DBType = settings.DBType };
                    if (!crate_v_usd.Trim().Equals(""))
                    {
                        core.CurrencyRateUpdate("PLN", crate_v_usd);
                        if (!core.AddCrossRate(DateTime.Now, "PLN", "USD", (float)Convert.ToDouble(crate_v_usd, System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            //log.WriteLine(core.LastError);
                        };
                    }

                    if (!crate_v_eur.Trim().Equals(""))
                    {
                        if (!core.AddCrossRate(DateTime.Now, "PLN", "EUR", (float)Convert.ToDouble(crate_v_eur, System.Globalization.CultureInfo.InvariantCulture)))
                        {
                            //log.WriteLine(core.LastError);
                        };
                    }
                }

                catch (Exception ex)
                {
                    //log.WriteLine(ex.ToString());
                }
            }
            catch (Exception xmlEx)
            {
                //log.WriteLine(xmlEx.ToString());
            }
        }
        /// <summary>�������� ���� ������� � ����</summary>
        private void GetCurrencyRateEUR_USD()
        {
            /* 
             * ������ � �������, ����������� ;
             * TICKER;DATE;OPEN;HIGH;LOW;CLOSE;WAPRICE
             * EUR_USD;2008-10-01;1.4078;1.4175;1.3976;1.4012;1.409
             */
            DateTime dt = DateTime.Now.AddDays(-1);
            string crate_v;
            string xml_s = "";
            string url = "http://export.rbc.ru/free/forex.0/free.fcgi?period=DAILY&tickers=EUR_USD&d1=" + dt.ToString("dd") + "&m1=" + dt.ToString("MM") + "&y1=" + dt.ToString("yyyy") + "&d2=" + DateTime.Now.ToString("dd") + "&m2=" + DateTime.Now.ToString("MM") + "&y2=" + DateTime.Now.ToString("yyyy") + "&lastdays=1&separator=%3B&data_format=BROWSER";
            try
            {
                HttpWebRequest web_req = (HttpWebRequest)HttpWebRequest.Create(url);
                web_req.Timeout = 30000; // 30 ������
                web_req.Method = "GET";
                web_req.ContentType = "application/x-www-form-urlencoded";
                HttpWebResponse web_res = (HttpWebResponse)web_req.GetResponse();

                using (StreamReader sr = new(web_res.GetResponseStream()))
                {
                    xml_s = sr.ReadToEnd(); // ������ � ������ � ���������
                    sr.Close();
                }
                web_res.Close();
            }
            catch { }
            string[] res_v = xml_s.Split(';');
            if (res_v.Length > 6)
            {
                crate_v = res_v[5].Trim();
                if (!crate_v.Equals(""))
                {
                    Core.Core core = new(pfa_core.ConnectionString) { DBType = settings.DBType };
                    core.CurrencyRateUpdate("EUR", crate_v);
                }
            }
        }
        /// <summary>�������� ���� ���� � �������</summary>
        private void GetCurrencyRateUSD_EUR()
        {
            /* 
             * ������ � �������, ����������� ;
             * TICKER;DATE;OPEN;HIGH;LOW;CLOSE;WAPRICE
             * EUR_USD;2008-10-01;1.4078;1.4175;1.3976;1.4012;1.409
             */
            DateTime dt = DateTime.Now.AddDays(-1);
            string crate_v;
            string xml_s = "";
            string url = "http://export.rbc.ru/free/forex.0/free.fcgi?period=DAILY&tickers=EUR_USD&d1=" + dt.ToString("dd") + "&m1=" + dt.ToString("MM") + "&y1=" + dt.ToString("yyyy") + "&d2=" + DateTime.Now.ToString("dd") + "&m2=" + DateTime.Now.ToString("MM") + "&y2=" + DateTime.Now.ToString("yyyy") + "&lastdays=1&separator=%3B&data_format=BROWSER";
            try
            {
                HttpWebRequest web_req = (HttpWebRequest)HttpWebRequest.Create(url);
                web_req.Timeout = 30000; // 30 ������
                web_req.Method = "GET";
                web_req.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse web_res = (HttpWebResponse)web_req.GetResponse();
                using (StreamReader sr = new StreamReader(web_res.GetResponseStream()))
                {
                    xml_s = sr.ReadToEnd();
                    sr.Close();
                }
                web_res.Close();
            }
            catch { };
            string[] res_v = xml_s.Split(';');
            if (res_v.Length > 6)
            {
                crate_v = res_v[5];
                if (!string.IsNullOrEmpty(crate_v.Trim()))
                {
                    if (float.TryParse(crate_v, out float r))
                        pfa_core.CurrencyRateUpdate("EUR", (1 / r).ToString().Replace(",", "."));
                }
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "MySQL Database dump|*.sql"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FilterIndex == 1)
                {
                    pfa_core.Export(sfd.FileName);
                }
            }
        }

        private void checkBoxAllAccounts_CheckedChanged(object sender, EventArgs e)
        {
            ShowToday();
        }

        private void textBoxTag_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!init)
                    ShowTransactions();
            }
        }

        private void ComboBoxAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init)
                ShowTransactions();
        }

        private void buttonEventAdd_Click(object sender, EventArgs e)
        {

        }
    }
}