using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EvgTar.PFA
{
    public partial class Payers : Form
    {
        public Core pfa_core = null;
        public Languages NLS = null;
        private DataSet ds = new DataSet();
        public string payer_id = "";
        public bool retVal = false;

        public Payers()
        {
            InitializeComponent();
        }
        public void Init()
        {
            this.Text = NLS.LNGetString("TitlePayers", "Payers");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            buttonDelete.Text = NLS.LNGetString("TextDelete", "Delete");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            buttonNew.Text = NLS.LNGetString("TextAdd", "Add");
            LoadPayers();
        }
        private void LoadPayers()
        {
            if (pfa_core == null)
                return;
            if (pfa_core.PayersGet(ref ds, "Payers", false))
            {
                listBoxPayers.Items.Clear();
                for (int i = 0; i < ds.Tables["Payers"].Rows.Count; i++)
                    listBoxPayers.Items.Add(ds.Tables["Payers"].Rows[i]["payer_name"]);                    
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void buttonNew_Click(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
            buttonDelete.Enabled = false;
            if ((listBoxPayers.SelectedIndex == -1) && !textBoxPayerName.Text.Trim().Equals(""))
                buttonSave_Click(sender, e);
            else
            {
                listBoxPayers.SelectedIndex = -1;
                textBoxPayerName.Text = "";
                textBoxPayerName.Select();
                textBoxPayerName.BackColor = Color.Red;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
            textBoxPayerName.BackColor = Color.White;
            if (listBoxPayers.SelectedIndex == -1)
                buttonSave.Enabled = true;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
                return;
            if (textBoxPayerName.Text.Trim().Equals(""))
            {
                textBoxPayerName.BackColor = Color.Red;
                textBoxPayerName.Select();
                return;
            }
            if (pfa_core.PayerUpdate((listBoxPayers.SelectedIndex >= 0)?(long)ds.Tables["Payers"].Rows[listBoxPayers.SelectedIndex]["payer_id"]:-1, textBoxPayerName.Text.Trim()))
            {
                buttonSave.Enabled = false;
                textBoxPayerName.Text = "";
                LoadPayers();
            }
            else
                MessageBox.Show(pfa_core.LastError);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonDelete.Enabled = true;
            textBoxPayerName.Text = listBoxPayers.SelectedItem.ToString();
            buttonSave.Enabled = false;
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
                return;
            if (listBoxPayers.SelectedIndex < 0)
                return;
            if (pfa_core.TransactionsGetCount(-1, (long)ds.Tables["Payers"].Rows[listBoxPayers.SelectedIndex]["payer_id"]) > 0)
                MessageBox.Show(NLS.LNGetString("TextPayer", "Payer") + " \"" + listBoxPayers.SelectedItem.ToString() + "\" " + NLS.LNGetString("TitleAccountDelete2", "has at least one transaction. First remove all transactions"));
            else
            {
                if (pfa_core.PayerDelete((long)ds.Tables["Payers"].Rows[listBoxPayers.SelectedIndex]["payer_id"]))
                {
                    textBoxPayerName.Text = "";
                    buttonDelete.Enabled = false;
                    buttonSave.Enabled = false;
                    LoadPayers();
                }
                else
                    MessageBox.Show(pfa_core.LastError);
            }
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (!retVal)
                return;
            if (listBoxPayers.SelectedIndex < 0)
                return;
            payer_id = ds.Tables["Payers"].Rows[listBoxPayers.SelectedIndex]["payer_id"].ToString();
            this.DialogResult = DialogResult.OK;
        }
        private void textBoxPayerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
                buttonSave_Click(sender, null);
        }
    }
}