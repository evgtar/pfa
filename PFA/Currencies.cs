using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EvgTar.PFA
{
    public partial class CurrenciesForm : Form
    {
        public Core pfa_core;
        public DataSet ds = new DataSet();
        public Languages NLS = null;

        public CurrenciesForm()
        {
            InitializeComponent();
        }
        public bool Init()
        {
            this.Text = NLS.LNGetString("TitleCurrencies", "Currencies");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            buttonNew.Text = NLS.LNGetString("TextAdd", "Add");
            buttonDelete.Text = NLS.LNGetString("TextDelete", "Delete");
            labelCCode.Text = NLS.LNGetString("TextCurrencyCode", "Currency code:");
            labelCName.Text = NLS.LNGetString("TextCurrencyName", "Currency name:");
            labelCSymbol.Text = NLS.LNGetString("TextCurrencySymbol", "Currency symbol:");
            labelCRate.Text = NLS.LNGetString("TextCurrencyRate", "Current rate for base currency:");

            if (pfa_core == null)
                return false; 
            
            label1BC.Text = "1 " + pfa_core.base_currency + " = ";
            if (pfa_core.CurrenciesGet(ref ds, true))
            {
                treeView1.Nodes.Clear();
                TreeNode node;
                for (int i = 0; i < ds.Tables["Currencies"].Rows.Count; i++)
                {
                    node = new TreeNode(ds.Tables["Currencies"].Rows[i]["currency_name"].ToString());
                    node.Tag = ds.Tables["Currencies"].Rows[i]["currency_code"];
                    node.ToolTipText = ds.Tables["Currencies"].Rows[i]["currency_code"].ToString() + " - " + ds.Tables["Currencies"].Rows[i]["currency_name"].ToString();
                    if (!ds.Tables["Currencies"].Rows[i]["currency_status"].ToString().Equals("1"))
                        node.ToolTipText += "\n" + NLS.LNGetString("TextDisabled", "Disabled");
                    treeView1.Nodes.Add(node);
                }
            }
            return true;
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {            
            if (treeView1.SelectedNode.Tag != null)
            {
                DataRow[] dr = ds.Tables["Currencies"].Select("currency_code = '" + treeView1.SelectedNode.Tag.ToString() + "'");
                if (dr.Length > 0)
                {
                    textBoxCCode.Text = dr[0]["currency_code"].ToString();
                    textBoxCName.Text = dr[0]["currency_name"].ToString();
                    textBoxCSymbol.Text = dr[0]["currency_symbol"].ToString();
                    labelCC.Text = dr[0]["currency_code"].ToString();
                    textBoxCRate.Text = dr[0]["currency_rate"].ToString();
                }
                textBoxCCode.Enabled = false;
                if (treeView1.SelectedNode.Tag.Equals(pfa_core.base_currency))
                {
                    buttonDelete.Enabled = false;
                    textBoxCRate.Text = "1";
                    textBoxCRate.Enabled = false;
                }
                else
                {
                    buttonDelete.Enabled = true;
                    textBoxCRate.Enabled = true;
                }
                buttonSave.Enabled = false;
            }
        }
        private void textBoxCCode_TextChanged(object sender, EventArgs e)
        {            
            textBoxCCode.BackColor = Color.White;
            labelCC.Text = textBoxCCode.Text.Trim();
            buttonSave.Enabled = true;
        }
        private void textBoxCName_TextChanged(object sender, EventArgs e)
        {
            textBoxCName.BackColor = Color.White;
            buttonSave.Enabled = true;
        }
        private void textBoxCSymbol_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void textBoxCRate_TextChanged(object sender, EventArgs e)
        {
            textBoxCRate.BackColor = Color.White;
            buttonSave.Enabled = true;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            bool next = true;
            bool exists = false;
            if (textBoxCName.Text.Trim().Equals(""))
            {
                textBoxCName.BackColor = Color.Red;
                textBoxCName.Select();
                next = false;
            }
            if (textBoxCCode.Text.Trim().Equals(""))
            {
                textBoxCCode.BackColor = Color.Red;
                textBoxCCode.Select();
                next = false;
            }
            if (textBoxCRate.Text.Trim().Equals(""))
            {
                textBoxCRate.BackColor = Color.Red;
                textBoxCRate.Select();
                next = false;
            }
            if (next)
            {
                next = false;
                if (treeView1.SelectedNode.Tag == null)
                {
                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                    {
                        if ((treeView1.Nodes[i].Tag != null) && (treeView1.Nodes[i].Tag.ToString().Equals(textBoxCCode.Text)))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (exists)
                    {
                        ToolTip tt = new ToolTip();
                        tt.SetToolTip(textBoxCCode, NLS.LNGetString("ErrorCurrencyCodeExists", "Currency code already exists!"));
                        textBoxCCode.BackColor = Color.Red;
                    }
                    else
                    {                        
                        next = pfa_core.CurrencyAdd(textBoxCCode.Text.Trim(), textBoxCName.Text.Trim(), textBoxCSymbol.Text.Trim(), textBoxCRate.Text.Trim().Replace(",", "."));
                    }
                }
                else
                {
                    next = pfa_core.CurrencyUpdate(textBoxCCode.Text.Trim(), textBoxCName.Text.Trim(), textBoxCSymbol.Text.Trim(), textBoxCRate.Text.Trim().Replace(",", "."));
                }
            }
            if (next)
            {
                Init();
                textBoxCCode.Enabled = false;
                buttonSave.Enabled = false;
            }
        }
        private void textBoxCCode_Leave(object sender, EventArgs e)
        {
            textBoxCCode.Text = textBoxCCode.Text.Trim().ToUpper();
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (buttonSave.Enabled)
            {
                if (MessageBox.Show(NLS.LNGetString("TextCurrency", "Currency") + " \"" + treeView1.SelectedNode.Text + "\" " + NLS.LNGetString("QuestionNotSaved1", " not saved. Save?"), NLS.LNGetString("TitleCurrencySave", "Currency save"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    buttonSave_Click(sender, e);
                else
                {
                    if (treeView1.SelectedNode.Tag == null)
                        treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
            if (!buttonSave.Enabled)
            {
                string nodeText = NLS.LNGetString("TextCurrency", "Currency");
                int num = 0;
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
                    nodeText = NLS.LNGetString("TextCurrency", "Currency") + " #" + num.ToString();
                    nodeExists = false;
                    for (int i = 0; i < treeView1.Nodes.Count; i++)
                        if (treeView1.Nodes[i].Text.Equals(nodeText))
                            nodeExists = true;
                }
                treeView1.SelectedNode = treeView1.Nodes.Add(nodeText);
                textBoxCCode.Enabled = true;
                textBoxCCode.Text = "";
                textBoxCName.Text = nodeText;
                textBoxCSymbol.Text = "";
                textBoxCRate.Text = "1";
                textBoxCRate.Enabled = true;
            }
            buttonSave.Enabled = true;
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
                return;
            if (treeView1.SelectedNode.Tag != null)
            {
                if (pfa_core.GetCount("Accounts", "where currency_code = '" + treeView1.SelectedNode.Tag.ToString() + "'") > 0)
                    MessageBox.Show(NLS.LNGetString("TextCurrency", "Currency")+" \"" + treeView1.SelectedNode.Tag.ToString() + "\" (" + treeView1.SelectedNode.Text + ") "+NLS.LNGetString("ErrorCurrencyUsedForAccount", "used for account and can't be deleted"));
                else
                {
                    if (pfa_core.CurrencyDelete(treeView1.SelectedNode.Tag.ToString()))
                    {
                        Init();
                        if (treeView1.Nodes.Count > 0)
                            treeView1.SelectedNode = treeView1.Nodes[0];
                        else
                        {
                            textBoxCCode.Text = "";
                            textBoxCName.Text = "";
                            textBoxCSymbol.Text = "";
                        }
                    }
                }
            }
        }
    }
}