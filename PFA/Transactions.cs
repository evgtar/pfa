using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace EvgTar.PFA
{
    public partial class TransactionForm : Form
    {
        public DataSet ds = new DataSet();
        private DataRow[] toAccount;
        private string decimal_sign = ",";
        private string decimal_sign_change = ".";
        public long trans_id = -1;
        private int ind_account = 0;
        private int ind_category = 0;
        private int ind_asset = 0;
        private string AmountLocal = "0";
        private string AmountBase = "0";
        public Languages NLS = null;
        public string LastError = "";
        public string LogFileName = "";
        public bool Debug = false;
        public Core pfa_core;

        public TransactionForm()
        {
            InitializeComponent();
            decimal_sign = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            decimal_sign_change = decimal_sign.Equals(",") ? "." : ",";
            dateTimePickerTransDate.CustomFormat = "dd MMMM yyyy HH:mm";
            dateTimePickerTransDate.Value = DateTime.Now;
            
        }
        public bool Init()
        {
            this.Text = NLS.LNGetString("TitleTransaction", "Transaction");
            labelAccount.Text = NLS.LNGetString("TextFromAccount", "From account:");
            labelPaymentType.Text = NLS.LNGetString("TextTransactionType", "Transaction type:");
            labelPayer.Text = NLS.LNGetString("TextPayer", "Payer") + ":";
            labelCategory.Text = NLS.LNGetString("TextCategory", "Category") + ":";
            labelDate.Text = NLS.LNGetString("TextDate", "Date") + ":";
            labelAmount.Text = NLS.LNGetString("TextAmount", "Amount") + ":";
            labelCRate.Text = NLS.LNGetString("TextCurrencyRate", "Currency rate:");
            labelNotes.Text = NLS.LNGetString("TextNotes", "Notes") + ":";
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            int indPaymentType = 1;
            if (pfa_core == null)
                return false;

            if (!pfa_core.Settings.ContainsKey("last_account"))
                pfa_core.Settings.Add("last_account", "");
            if (!pfa_core.Settings.ContainsKey("last_category"))
                pfa_core.Settings.Add("last_category", "");
            if (!pfa_core.Settings.ContainsKey("last_payer"))
                pfa_core.Settings.Add("last_payer", "");
            if (!pfa_core.Settings.ContainsKey("last_asset"))
                pfa_core.Settings.Add("last_asset", "");

            bool nextStep;
            if (trans_id > 0)
            {
                nextStep = pfa_core.TransactionGet(ref ds, trans_id);
                if (nextStep)
                {
                    if (ds.Tables["TransactionInfo"].Rows.Count > 0)
                    {
                        pfa_core.Settings["last_account"] = ds.Tables["TransactionInfo"].Rows[0]["account_id"].ToString();
                        pfa_core.Settings["last_category"] = ds.Tables["TransactionInfo"].Rows[0]["category_id"].ToString();
                        pfa_core.Settings["last_payer"] = ds.Tables["TransactionInfo"].Rows[0]["payer_id"].ToString();
                        pfa_core.Settings["last_asset"] = ds.Tables["TransactionInfo"].Rows[0]["asset_id"].ToString();
                        indPaymentType = ds.Tables["TransactionInfo"].Rows[0]["trans_type"].ToString().Equals("0") ? 0 : 1;
                        AmountLocal = ds.Tables["TransactionInfo"].Rows[0]["amount_local"].ToString();
                        textBoxAmount.Text = ds.Tables["TransactionInfo"].Rows[0]["amount_local"].ToString();
                        textBoxCRate.Text = ds.Tables["TransactionInfo"].Rows[0]["currency_rate"].ToString();
                        AmountBase = ds.Tables["TransactionInfo"].Rows[0]["amount_base"].ToString();
                        textBoxAmountBase.Text = ds.Tables["TransactionInfo"].Rows[0]["amount_base"].ToString();
                        textBoxNotes.Text = ds.Tables["TransactionInfo"].Rows[0]["trans_notes"].ToString();
                        try
                        {
                            dateTimePickerTransDate.Value = Convert.ToDateTime(ds.Tables["TransactionInfo"].Rows[0]["trans_date"]);
                        }
                        catch { }
                        comboBoxAccount.Enabled = false;
                        comboBoxPaymentType.Enabled = false;
                    }
                }
                //else
                //    new Utils.CFileLog(LogFileName).WriteLine(pfa_core.LastError);
            }

            nextStep = pfa_core.AccountsGet(ref ds, false, "TransAccounts");
            if (nextStep)
            {
                comboBoxAccount.Items.Clear();
                ind_account = 0;
                for (int i = 0; i < ds.Tables["TransAccounts"].Rows.Count; i++)
                {
                    comboBoxAccount.Items.Add(ds.Tables["TransAccounts"].Rows[i]["account_name"].ToString());
                    if (!string.IsNullOrWhiteSpace(pfa_core.Settings["last_account"]))
                    {
                        if (ds.Tables["TransAccounts"].Rows[i]["account_id"].ToString().Equals(pfa_core.Settings["last_account"]))
                            ind_account = i;
                    }
                    else
                    {
                        //MessageBox.Show(ds.Tables["TransAccounts"].Rows[i]["account_status"].ToString());
                        if (ds.Tables["TransAccounts"].Rows[i]["account_status"].ToString().Equals("2"))
                            ind_account = i;
                    }
                }
                if (comboBoxAccount.Items.Count >= ind_account)
                    comboBoxAccount.SelectedIndex = ind_account;
            }
            //else
            //    new Utils.CFileLog(LogFileName).WriteLine(pfa_core.LastError);

            nextStep = pfa_core.AssetsGet(ref ds);
            if (nextStep)
            {
                comboBoxAsset.Items.Clear();
                ind_asset = -1;
                for (int i = 0; i < ds.Tables["Assets"].Rows.Count; i++)
                {
                    comboBoxAsset.Items.Add(ds.Tables["Assets"].Rows[i]["AssetName"].ToString());
                    if (!string.IsNullOrWhiteSpace(pfa_core.Settings["last_asset"]))
                    {
                        if (ds.Tables["Assets"].Rows[i]["AssetId"].ToString().Equals(pfa_core.Settings["last_asset"]))
                            ind_asset = i;
                    }
                }
                if (comboBoxAsset.Items.Count >= ind_asset)
                    comboBoxAsset.SelectedIndex = ind_asset;
            }
            //else
            //    new Utils.CFileLog(LogFileName).WriteLine(pfa_core.LastError);

            comboBoxPaymentType.Items.Clear();
            comboBoxPaymentType.Items.Add(NLS.LNGetString("TextDeposit", "Deposit"));
            comboBoxPaymentType.Items.Add(NLS.LNGetString("TextWithdrawal", "Withdrawal"));
            comboBoxPaymentType.SelectedIndex = indPaymentType;
            LoadPayers();
            LoadCategories();
            labelAmountBaseC.Text = NLS.LNGetString("TextAmount", "Amount:") + " (" + pfa_core.base_currency + "):";

            if (comboBoxAccount.Items.Count < 1)
            {
                MessageBox.Show(NLS.LNGetString("ErrorNoAccount", "There's no any account. Please create at least one account first") + "\n" + NLS.LNGetString("TextAccountsPath", "Menu: Tools -> Accounts"));
                return false;
            }
            if (comboBoxCategory.Items.Count < 1)
            {
                MessageBox.Show(NLS.LNGetString("ErrorNoCategory", "There's no any category. Please create at least one category first") + "\n" + NLS.LNGetString("TextCategoriesPath", "Menu: Tools -> Categories"));
                return false;
            }
            return true;
        }
        private void LoadPayers()
        {
            if (pfa_core == null)
                return;

            if (pfa_core.PayersGet(ref ds, "TransPayers", false))
            {
                comboBoxPayer.Items.Clear();
                for (int i = 0; i < ds.Tables["TransPayers"].Rows.Count; i++)
                {
                    comboBoxPayer.Items.Add(ds.Tables["TransPayers"].Rows[i]["payer_name"]);
                    if (ds.Tables["TransPayers"].Rows[i]["payer_id"].ToString().Equals(pfa_core.Settings["last_payer"]))
                        comboBoxPayer.SelectedIndex = i;
                }
            }
        }
        private void LoadCategories()
        {
            if (pfa_core == null)
                return;
            string sql = "";
            /*bool nextStep = false;
            switch (pfa_core.DBType)
            {
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(pfa_core.ConnectionString);
                    sql = "select cat.category_id CategoryID, pcat.category_name||' - '||cat.category_name CName ";
                    sql += " from Categories cat, Categories pcat ";
                    sql += " where cat.pcategory_id = pcat.category_id ";
                    sql += " order by CName";
                    nextStep = sqlite.GetDataSet(ref ds, "TransCategories", sql);
                    if (!nextStep)
                        LastError = sqlite.LastError;
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.CategoriesGet";
                    MSSQLUtils dbtools = new MSSQLUtils(pfa_core.ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "TransCategories", sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }*/
            if (pfa_core.CategoriesGet(ref ds, false, "TransCategories"))
            {
                comboBoxCategory.Items.Clear();
                for (int i = 0; i < ds.Tables["TransCategories"].Rows.Count; i++)
                {
                    comboBoxCategory.Items.Add(ds.Tables["TransCategories"].Rows[i]["CName"].ToString());
                    if (!string.IsNullOrWhiteSpace(pfa_core.Settings["last_category"]))
                    {
                        if (ds.Tables["TransCategories"].Rows[i]["CategoryID"].ToString().Equals(pfa_core.Settings["last_category"]))                        
                            ind_category = i;
                    }
                }
                comboBoxCategory.SelectedIndex = ind_category;
            }
            //else
            //    new Utils.CFileLog(LogFileName).WriteLine(LastError + Environment.NewLine + sql);
        }
        private void LoadTags()
        {
            /*if (pfa_core == null)
                return;
            string sql = "";
            bool nextStep = false;
            switch (pfa_core.DBType)
            {
                case DBTypes.SQLite:
                    SQLite sqlite = new SQLite(pfa_core.ConnectionString);
                    sql = "select tag_id, tag_code tags order by tag_code";
                    nextStep = sqlite.GetDataSet(ref ds, "Tags", sql);
                    if (!nextStep)
                        LastError = sqlite.LastError;
                    break;
                case DBTypes.MSSQL:
                    sql = "dbo.TagsGet";
                    MSSQLUtils dbtools = new MSSQLUtils(pfa_core.ConnectionString);
                    nextStep = dbtools.GetDataSet(ref ds, "Tags", sql);
                    if (!nextStep)
                        LastError = dbtools.LastError;
                    break;
            }
            if (nextStep)
            {
                comboBoxCategory.Items.Clear();
                for (int i = 0; i < ds.Tables["Tags"].Rows.Count; i++)
                {
                    comboBoxCategory.Items.Add(ds.Tables["Tags"].Rows[i]["tag_code"].ToString());
                }
                comboBoxCategory.SelectedIndex = -1;
            }*/
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void comboBoxAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
            if (trans_id < 0)
            {
                labelAmount.Text = NLS.LNGetString("TextAmount", "Amount") + " (" + ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["currency_code"].ToString() + "):";
                textBoxCRate.Text = ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["currency_rate"].ToString();
                textBoxCRate.Enabled = !ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["currency_code"].ToString().Equals(pfa_core.base_currency);
                if (comboBoxPaymentType.SelectedIndex == 2)
                {
                    labelAccount.Text = NLS.LNGetString("TextFromAccount", "From account:");
                    labelPayer.Text = NLS.LNGetString("TextToAccount", "To account:");
                    toAccount = ds.Tables["TransAccounts"].Select("account_id <> " + ds.Tables["Accounts"].Rows[comboBoxAccount.SelectedIndex]["account_id"].ToString());
                    comboBoxPayer.Items.Clear();
                    for (int i = 0; i < toAccount.Length; i++)
                        comboBoxPayer.Items.Add(toAccount[i]["account_name"].ToString());
                    if (toAccount.Length > 0)
                        comboBoxPayer.SelectedIndex = 0;
                }
            }
        }
        private void comboBoxPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
            switch (comboBoxPaymentType.SelectedIndex)
            {
                case 0: // deposit
                    labelAccount.Text = NLS.LNGetString("TextToAccount", "To account:");
                    labelPayer.Text = NLS.LNGetString("TextPayer", "Payer") + ":";
                    LoadPayers();
                    break;
                case 1: // withdraw
                    labelAccount.Text = NLS.LNGetString("TextFromAccount", "From account:");
                    labelPayer.Text = NLS.LNGetString("TextPayee", "Payee") + ":";
                    LoadPayers();
                    break;
                case 2: // transfer
                    labelAccount.Text = NLS.LNGetString("TextFromAccount", "From account:");
                    labelPayer.Text = NLS.LNGetString("TextToAccount", "To account:");
                    toAccount = ds.Tables["TransAccounts"].Select("account_id <> " + ds.Tables["Accounts"].Rows[comboBoxAccount.SelectedIndex]["account_id"].ToString());
                    LoadPayers();
                    for (int i = 0; i < toAccount.Length; i++)
                        comboBoxPayer.Items.Add(toAccount[i]["account_name"].ToString());
                    if (toAccount.Length > 0)
                        comboBoxPayer.SelectedIndex = 0;
                    break;
            }
        }
        private void comboBoxPayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
            if ((comboBoxPaymentType.SelectedIndex == 2) && (toAccount != null))
            {
                textBoxCRate.Text = toAccount[comboBoxPayer.SelectedIndex]["currency_rate"].ToString();
                labelAmountBaseC.Text = " (" + toAccount[comboBoxPayer.SelectedIndex]["currency_code"].ToString() + "):";
                textBoxCRate.Enabled = !ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["currency_code"].ToString().Equals(pfa_core.base_currency);
            }
        }
        private void comboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void textBoxAmount_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;

            if (textBoxCRate.Text.Trim().Equals(""))
                textBoxCRate.Text = "1";            
            try
            {
                textBoxAmountBase.Text = (Convert.ToDouble(textBoxAmount.Text.Trim().Replace(decimal_sign_change, decimal_sign)) / Convert.ToDouble(textBoxCRate.Text.Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
            }
            catch { }
        }
        private void textBoxCRate_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
            if (!textBoxCRate.Text.Trim().Equals(""))
            {
                try
                {
                    textBoxAmountBase.Text = (Convert.ToDouble(textBoxAmount.Text.Trim().Replace(decimal_sign_change, decimal_sign)) / Convert.ToDouble(textBoxCRate.Text.Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
                }
                catch { }
            }
        }
        private void textBoxNotes_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
                return;
            if (textBoxAmount.Text.Trim().Equals(""))
                textBoxAmount.Text = "0";
            string s_crate = textBoxCRate.Text.Trim().Replace(",", ".");
            float crate = 1, amount_l = 0F, amount_b = 0F;
            float.TryParse(s_crate, out crate);
            float.TryParse(textBoxAmount.Text.Trim().Replace(",", ".").Replace("�", "."), out amount_l);
            float.TryParse(textBoxAmountBase.Text.Trim().Replace(",", ".").Replace("�", "."), out amount_b);
            if (trans_id < 0){
                //trans_id = (DateTime.Now.Ticks - (new DateTime(2000, 1, 1).Ticks));
				trans_id = -1;
			}
            long payer_id = -1;
            if (comboBoxPayer.SelectedIndex > -1)
                payer_id = long.Parse(ds.Tables["TransPayers"].Rows[comboBoxPayer.SelectedIndex]["payer_id"].ToString());
            float.TryParse(ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["currency_rate"].ToString().Replace(",", "."), out float currate);
            if (!currate.Equals(crate))
            {
                if (MessageBox.Show(NLS.LNGetString("QuestionCurrencyRate1", "Entered currency rate ") + "(" + crate + ")" + NLS.LNGetString("QuestionCurrencyRate2", " is different for this currency ") + "(" + 1F + ")." + NLS.LNGetString("QuestionCurrencyRate3", "Save new currency rate?"), NLS.LNGetString("TitleCurrencyRate", "Currency rate"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    pfa_core.CurrencyRateUpdate(ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["currency_code"].ToString(), crate.ToString());
                }
            }            
            if ((comboBoxAccount.SelectedIndex > -1) && (comboBoxCategory.SelectedIndex > -1))
            {
                pfa_core.TransactionAdd(trans_id,
                    dateTimePickerTransDate.Value,
                    comboBoxPaymentType.SelectedIndex,
                    (long)ds.Tables["TransCategories"].Rows[comboBoxCategory.SelectedIndex]["CategoryID"],
                    (long)ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["account_id"],
                    -1,
                    payer_id,
                    comboBoxAsset.SelectedIndex > -1 ? (long)ds.Tables["Assets"].Rows[comboBoxAsset.SelectedIndex]["AssetId"] : -1,
                    amount_l,
                    crate,
                    amount_b,
                    textBoxNotes.Text.Trim(),
                    1
                );
                /*switch (pfa_core.DBType)
                {
                    case DBTypes.SQLite:
                        trans_id = new CoreSQLite(pfa_core.ConnectionString).TransactionAdd(trans_id,
                            dateTimePickerTransDate.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                            comboBoxPaymentType.SelectedIndex,
                            (int)ds.Tables["TransCategories"].Rows[comboBoxCategory.SelectedIndex]["CategoryID"],
                            (int)ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["account_id"],
                            -1,
                            payer_id,
                            -1,
                            textBoxAmount.Text.Trim().Replace(",", "."),
                            crate,
                            textBoxAmountBase.Text.Trim().Replace(",", "."),
                            textBoxNotes.Text.Trim()
                            ).ToString();
                        break;
                    case DBTypes.MSSQL:
                        MSSQLUtils dbtools = new MSSQLUtils(pfa_core.ConnectionString);
                        PropertyCollection sqlProp = new PropertyCollection();
                        sqlProp.Add("@TransactionId", trans_id);
                        sqlProp.Add("@TransactionDate", dateTimePickerTransDate.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                        sqlProp.Add("@PaymentType", comboBoxPaymentType.SelectedIndex);
                        sqlProp.Add("@CategoryId", ds.Tables["TransCategories"].Rows[comboBoxCategory.SelectedIndex]["CategoryID"]);
                        sqlProp.Add("@AccountId", ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["account_id"]);
                        sqlProp.Add("@AccountIdRef", -1);
                        sqlProp.Add("@PayerId", payer_id);
                        sqlProp.Add("@Amount", textBoxAmount.Text.Trim().Replace(",", "."));
                        sqlProp.Add("@CurrencyRate", crate);
                        sqlProp.Add("@AmountBase", textBoxAmountBase.Text.Trim().Replace(",", "."));
                        sqlProp.Add("@Notes", textBoxNotes.Text.Trim());
                        sqlProp.Add("@AssetId", "");
                        if (dbtools.GetDataSet(ref ds, "TransactionId", "dbo.TransactionAdd", sqlProp))
                        {
                            if (ds.Tables["TransactionId"].Rows.Count > 0)
                            {
                                trans_id = ds.Tables["TransactionId"].Rows[0]["TransactionId"].ToString();
                            }
                        }
                        break;
                }*/
                pfa_core.Settings["last_account"] = ds.Tables["TransAccounts"].Rows[comboBoxAccount.SelectedIndex]["account_id"].ToString();
                pfa_core.Settings["last_category"] = ds.Tables["TransCategories"].Rows[comboBoxCategory.SelectedIndex]["CategoryID"].ToString();
                pfa_core.Settings["last_payer"] = payer_id.ToString();
                pfa_core.Settings["last_asset"] = comboBoxAsset.SelectedIndex > 0 ? ds.Tables["Assets"].Rows[comboBoxAsset.SelectedIndex]["AssetId"].ToString() : "";
                pfa_core.SettingsSave();
            }
        }
        private void buttonGetPayer_Click(object sender, EventArgs e)
        {
            using (Payers payers = new Payers())
            {
                payers.pfa_core = pfa_core;
                payers.NLS = NLS;
                payers.retVal = true;
                payers.Init();
                if (payers.ShowDialog() == DialogResult.OK)
                {
                    pfa_core.Settings["last_payer"] = payers.payer_id;
                    LoadPayers();
                }
            }
        }
        private void buttonGetCategory_Click(object sender, EventArgs e)
        {
            using (CategoriesForm categories = new CategoriesForm())
            {
                categories.pfa_core = pfa_core;
                categories.NLS = NLS;
                categories.retVal = true;
                categories.Init();
                if (categories.ShowDialog() == DialogResult.OK)
                {
                    pfa_core.Settings["last_category"] = categories.category_id;
                    LoadCategories();
                }
            }
        }

        private void comboBoxTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*LinkLabel ll = new LinkLabel();
            ll.Text = comboBoxTags.Text;
            try
            {
                using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
                {
                    ll.Left = labelTags.Left + Convert.ToInt32(graphics.MeasureString(labelTags.Text, labelTags.Font).Width) + 5;
                    comboBoxTags.Left = ll.Left + 5 + Convert.ToInt32(graphics.MeasureString(ll.Text, ll.Font).Width);
                }
            }
            catch { }
            ll.Top = labelTags.Top;
            ll.Size = new Size(ll.PreferredWidth, ll.PreferredHeight);
            ll.Click += new EventHandler(ll_Click);

            this.Controls.Add(ll);*/
        }

        void ll_Click(object sender, EventArgs e)
        {
            this.Controls.Remove((LinkLabel)sender);
        }

        private void comboBoxTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                comboBoxTags_SelectedIndexChanged(sender, null);
        }
    }
}