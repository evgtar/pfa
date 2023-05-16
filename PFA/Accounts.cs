using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EvgTar.PFA
{
    public partial class AccountsForm : Form
    {
        public DataSet ds = new DataSet();
        public Languages NLS { get; set; } = null;
        public Core pfa_core = null;
        public AccountsForm()
        {
            InitializeComponent();
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        public bool Init()
        {
            this.Text = NLS.LNGetString("TitleAccounts", "Accounts");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            buttonNew.Text = NLS.LNGetString("TextAdd", "Add");
            buttonDelete.Text = NLS.LNGetString("TextDelete", "Delete");
            labelAccountName.Text = NLS.LNGetString("TextAccountName", "Account name:");
            labelAccountNumber.Text = NLS.LNGetString("TextAccountNumber", "Account number:");
            labelAccountHolder.Text = NLS.LNGetString("TextAccountHolder", "Account holder:");
            labelAccountType.Text = NLS.LNGetString("TextAccountType", "Account type:");
            labelInitialBalance.Text = NLS.LNGetString("TextInitialBalance", "Initial balance:");
            labelCurrency.Text = NLS.LNGetString("TextCurrency", "Currency") + ":";
            checkBoxActive.Text = NLS.LNGetString("TextActive", "Active");

            if (pfa_core == null)
                return false;
            
            comboBoxAccountType.BeginUpdate();
            comboBoxAccountType.Items.Clear();
            
            if (pfa_core.AccountTypesGet(ref ds))
            {
                for (int i = 0; i < ds.Tables["AccountTypes"].Rows.Count; i++)
                {
                    comboBoxAccountType.Items.Add(NLS.LNGetString(ds.Tables["AccountTypes"].Rows[i]["acctype_code"].ToString().Trim(), ds.Tables["AccountTypes"].Rows[i]["acctype_name"].ToString()));
                }
            }
            if (comboBoxAccountType.Items.Count > 0)
                comboBoxAccountType.SelectedIndex = 0;
            comboBoxAccountType.EndUpdate();

            int sel_node = 0;
            if (treeView1.SelectedNode != null)
                sel_node = treeView1.SelectedNode.Index;

            if (pfa_core.AccountsGet(ref ds, true))
            {
                treeView1.Nodes.Clear();
                TreeNode node;
                for (int i = 0; i < ds.Tables["Accounts"].Rows.Count; i++)
                {
                    node = new TreeNode(ds.Tables["Accounts"].Rows[i]["account_name"].ToString())
                    {
                        Tag = ds.Tables["Accounts"].Rows[i]["account_id"],
                        ToolTipText = NLS.LNGetString("TextAccountNumber", "Account number:") + " " + ds.Tables["Accounts"].Rows[i]["account_number"].ToString() + "\n"
                    };
                    node.ToolTipText += NLS.LNGetString("TextCurrency", "Currency") + ": " + ds.Tables["Accounts"].Rows[i]["currency_code"].ToString() + "\n";
                    node.ToolTipText += NLS.LNGetString("TextInitialBalance", "Initial balance:") + " " + ds.Tables["Accounts"].Rows[i]["initial_balance"].ToString() + "\n";
                    treeView1.Nodes.Add(node);
                }
            }
            else
            {
                MessageBox.Show(pfa_core.LastError);
                return false;
            }
            LoadCurrencies();
            buttonSave.Enabled = false;
            try
            {
                treeView1.SelectedNode = treeView1.Nodes[sel_node];
            }
            catch { }
            return true;
        }
        private void LoadCurrencies()
        {
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
        private void buttonNew_Click(object sender, EventArgs e)
        {
            int num = 0;
            if ((buttonSave.Enabled) && (treeView1.SelectedNode != null))
            {
                DialogResult dr = MessageBox.Show(NLS.LNGetString("TextAccount", "Account") + " \"" + treeView1.SelectedNode.Text + "\" " + NLS.LNGetString("QuestionNotSaved", "not saved. Save?"), NLS.LNGetString("TitleAccountSave", "Account save"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                switch (dr)
                {
                    case DialogResult.Yes:
                        buttonSave_Click(sender, e);
                        break;
                    case DialogResult.No:
                        if (treeView1.SelectedNode.Tag == null)
                            treeView1.Nodes.Remove(treeView1.SelectedNode);
                        break;
                    case DialogResult.Cancel:
                        return;
                }                
            }
            string nodeText = NLS.LNGetString("TextAccount", "Account");
            bool nodeExists = false;
            for (int i = 0; i < treeView1.Nodes.Count; i++)
                if (treeView1.Nodes[i].Text.Equals(nodeText))
                {
                    nodeExists = true;
                    break;
                }
            while (nodeExists)
            {
                num++;
                nodeText = NLS.LNGetString("TextAccount", "Account") + " #" + num.ToString();
                nodeExists = false;
                for (int i = 0; i < treeView1.Nodes.Count; i++)
                    if (treeView1.Nodes[i].Text.Equals(nodeText))
                        nodeExists = true;
            }
            treeView1.SelectedNode = treeView1.Nodes.Add(nodeText);
            textBoxAccountName.Text = nodeText;
            for (int i = 0; i < ds.Tables["Currencies"].Rows.Count; i++)
            {                
                if (ds.Tables["Currencies"].Rows[i]["currency_code"].ToString().Equals(pfa_core.base_currency))
                    comboBoxCurrencies.SelectedIndex = i;
            }
            comboBoxAccountType.SelectedIndex = 0;
            textBoxInitialBalance.Text = "0";
            buttonSave.Enabled = true;
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode.Tag != null)
            {
                DataRow[] dr = ds.Tables["Accounts"].Select("account_id = " + treeView1.SelectedNode.Tag.ToString());
                if (dr.Length > 0)
                {
                    textBoxAccountName.Text = dr[0]["account_name"].ToString();
                    textBoxAccountNumber.Text = dr[0]["account_number"].ToString();
                    textBoxInitialBalance.Text = dr[0]["initial_balance"].ToString();
                    for (int i = 0; i < ds.Tables["Currencies"].Rows.Count; i++)
                    {
                        if (ds.Tables["Currencies"].Rows[i]["currency_code"].ToString().Equals(dr[0]["currency_code"].ToString()))
                        {
                            comboBoxCurrencies.SelectedIndex = i;
                            break;
                        }
                    }
                    for (int i = 0; i < ds.Tables["AccountTypes"].Rows.Count; i++)
                    {
                        if (i < comboBoxAccountType.Items.Count)
                        {
                            if (ds.Tables["AccountTypes"].Rows[i]["account_type"].ToString().Equals(dr[0]["account_type"].ToString()))
                                comboBoxAccountType.SelectedIndex = i;
                        }
                    }
                    
                    checkBoxActive.Checked = dr[0]["account_status"].ToString().Equals("1");
                }
            }
            buttonDelete.Enabled = true;
            buttonSave.Enabled = false;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxAccountName.Text.Trim().Equals(""))
            {
                textBoxAccountName.BackColor = Color.Red;
                textBoxAccountName.Select();
                return;
            }
            if (textBoxInitialBalance.Text.Trim().Equals(""))
                textBoxInitialBalance.Text = "0";
            if (treeView1.SelectedNode == null)
                treeView1.SelectedNode = treeView1.Nodes.Add(textBoxAccountName.Text);
            if (pfa_core.AccountSave(
                    treeView1.SelectedNode.Tag == null ? -1 : (long)treeView1.SelectedNode.Tag,
                    textBoxAccountName.Text.Trim(),
                    textBoxAccountNumber.Text.Trim(),
                    int.Parse(ds.Tables["AccountTypes"].Rows[comboBoxAccountType.SelectedIndex]["account_type"].ToString()),
                    ds.Tables["Currencies"].Rows[comboBoxCurrencies.SelectedIndex]["currency_code"].ToString(),
                    textBoxInitialBalance.Text.Trim().Replace(",", "."),
                    checkBoxActive.Checked
                ))
                buttonSave.Enabled = false;
            else
                MessageBox.Show(pfa_core.LastError);
            Init();
        }
        private void textBoxAccountName_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void textBoxAccountNumber_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void comboBoxHolder_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void comboBoxAccountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void comboBoxCurrencies_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void textBoxInitialBalance_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void buttonChooseCurrency_Click(object sender, EventArgs e)
        {
            using (CurrenciesForm currencies = new CurrenciesForm())
            {
                currencies.pfa_core = pfa_core;
                currencies.ds = ds;
                if (currencies.Init())
                    currencies.ShowDialog();
            }
            LoadCurrencies();
            if (treeView1.SelectedNode.Tag != null)
            {
                DataRow[] dr = ds.Tables["Accounts"].Select("account_id = " + treeView1.SelectedNode.Tag.ToString());
                if (dr.Length > 0)
                {
                    for (int i = 0; i < ds.Tables["Currencies"].Rows.Count; i++)
                    {
                        if (ds.Tables["Currencies"].Rows[i]["currency_code"].ToString().Equals(dr[0]["currency_code"].ToString()))
                        {
                            comboBoxCurrencies.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
                return;
            if (treeView1.SelectedNode.Tag != null)
            {
                if (pfa_core.TransactionsGetCount((long)treeView1.SelectedNode.Tag, -1) != 0)
                    MessageBox.Show(NLS.LNGetString("TitleAccountDelete1", "There's at least one transaction for ") + treeView1.SelectedNode.Text + NLS.LNGetString("TitleAccountDelete2", " account. First remove all transactions"));
                else
                {
                    if (pfa_core.AccountDelete((long)treeView1.SelectedNode.Tag))
                    {
                        Init();
                        if (treeView1.Nodes.Count > 0)
                            treeView1.SelectedNode = treeView1.Nodes[0];
                    }
                    else
                        MessageBox.Show(pfa_core.LastError);
                }
            }
        }

        /*
            string url = "http://export.rbc.ru/free/cb.0/free.fcgi?period=DAILY&tickers=USD&lastdays=0&separator=TAB&data_format=BROWSER";
            using (WebClient web = new WebClient())
            {
                string txt = Encoding.GetEncoding("windows-1251").GetString(web.DownloadData(url));
                MessageBox.Show(txt);
            } 
        */
    }
}