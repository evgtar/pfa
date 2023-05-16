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
    public partial class TransferForm : Form
    {
        public Core pfa_core;
        public Languages NLS = null;
        private DataSet ds = new DataSet();
        private string decimal_sign = ",";
        private string decimal_sign_change = ".";
        private bool init = false;
        private bool change_exchange = true;

        public TransferForm()
        {
            InitializeComponent();
            decimal_sign = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            decimal_sign_change = decimal_sign.Equals(",") ? "." : ",";
            dateTimePickerDate.CustomFormat = "dd MMMM yyyy HH:mm";
            dateTimePickerDate.Value = DateTime.Now;
        }
        public bool Init()
        {
            this.Text = labelInfo.Text = NLS.LNGetString("TextTransfer", "Transfer");
            labelAccountFrom.Text = NLS.LNGetString("TextFromAccount", "From account:");
            labelAccountTo.Text = NLS.LNGetString("TextToAccount", "To account:");
            labelDate.Text = NLS.LNGetString("TextDate", "Date") + ":";
            labelAmountFrom.Text = NLS.LNGetString("TextAmount", "Amount") + ":";
            labelCRate.Text = NLS.LNGetString("TextCurrencyRate", "Currency rate:");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            if (pfa_core == null)
                return false;            
            if (pfa_core.AccountsGet(ref ds, false, "TransAccounts"))
            {
                if (ds.Tables["TransAccounts"].Rows.Count > 2)
                {
                    comboBoxAccountFrom.Items.Clear();
                    comboBoxAccountTo.Items.Clear();
                    for (int i = 0; i < ds.Tables["TransAccounts"].Rows.Count; i++)
                    {
                        comboBoxAccountFrom.Items.Add(ds.Tables["TransAccounts"].Rows[i]["account_name"].ToString());
                        comboBoxAccountTo.Items.Add(ds.Tables["TransAccounts"].Rows[i]["account_name"].ToString());
                    }
                    comboBoxAccountFrom.SelectedIndex = 0;
                    comboBoxAccountTo.SelectedIndex = 1;
                }
                else
                {
                    comboBoxAccountFrom.Enabled = false;
                    comboBoxAccountTo.Enabled = false;
                }
            }
            pfa_core.ExchangeRateGetAll(ref ds, 1);
            labelBaseCur1.Text = labelBaseCur2.Text = pfa_core.base_currency;
            init = true;
            AccountChanged();
            return true;
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void AccountChanged()
        {
            if (!init)
                return;
            string cc_from = ds.Tables["TransAccounts"].Rows[comboBoxAccountFrom.SelectedIndex]["currency_code"].ToString();
            string cc_to = ds.Tables["TransAccounts"].Rows[comboBoxAccountTo.SelectedIndex]["currency_code"].ToString();
            if (comboBoxAccountFrom.SelectedIndex == comboBoxAccountTo.SelectedIndex)
            {
                MessageBox.Show(NLS.LNGetString("AlertChooseDifAccount", "From and to accounts are equal. Please choose different accounts"), NLS.LNGetString("TextWarning", "Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                buttonSave.Enabled = false;
            }
            else
                buttonSave.Enabled = true;
            try
            {
                DataRow[] r = ds.Tables["ExchangeRate"].Select("cc_from = '" + cc_from + "' and cc_to = '" + cc_to + "'");
                if (r.Length > 0)
                {
                    textBoxExchangeRate.Text = r[0]["cc_rate"].ToString();
                }
                else
                    textBoxExchangeRate.Text = "1";
            }
            catch (Exception ex)
            {
                //if (pfa_core.Debug)
                //    new CFileLog(pfa_core.LogFileName).WriteLine(ex.ToString());
            }
            labelAmountFrom.Text = NLS.LNGetString("TextAmount", "Amount") + " (" + cc_from + "):";
            labelAmountTo.Text = NLS.LNGetString("TextAmount", "Amount") + " (" + cc_to + "):";
            AmountChanged(true);
        }
        private void AmountChanged(bool direct_exchange)
        {
            change_exchange = false;
            if (checkBoxCalcExchRate.Checked)
            {
                try
                {
                    textBoxExchangeRate.Text = (Convert.ToDouble(textBoxAmountTo.Text.Trim().Replace(decimal_sign_change, decimal_sign)) / Convert.ToDouble(textBoxAmountFrom.Text.Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
                }
                catch { }
            }
            else
            {
                if (textBoxExchangeRate.Text.Trim().Equals(""))
                    textBoxExchangeRate.Text = "1";
                try
                {
                    if (direct_exchange)
                        textBoxAmountTo.Text = (Convert.ToDouble(textBoxAmountFrom.Text.Trim().Replace(decimal_sign_change, decimal_sign)) * Convert.ToDouble(textBoxExchangeRate.Text.Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
                    else
                        textBoxAmountFrom.Text = (Convert.ToDouble(textBoxAmountTo.Text.Trim().Replace(decimal_sign_change, decimal_sign)) / Convert.ToDouble(textBoxExchangeRate.Text.Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
                }
                catch { }
            }
            try
            {
                textBoxBaseFrom.Text = (Convert.ToDouble(textBoxAmountFrom.Text.Trim().Replace(decimal_sign_change, decimal_sign)) / Convert.ToDouble(ds.Tables["TransAccounts"].Rows[comboBoxAccountFrom.SelectedIndex]["currency_rate"].ToString().Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
            }
            catch { }
            try
            {
                textBoxBaseTo.Text = (Convert.ToDouble(textBoxAmountTo.Text.Trim().Replace(decimal_sign_change, decimal_sign)) / Convert.ToDouble(ds.Tables["TransAccounts"].Rows[comboBoxAccountTo.SelectedIndex]["currency_rate"].ToString().Trim().Replace(decimal_sign_change, decimal_sign))).ToString();
            }
            catch { }
            change_exchange = true;
        }
        private void comboBoxAccountFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountChanged();
        }
        private void comboBoxAccountTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            AccountChanged();
        }
        private void textBoxAmountFrom_TextChanged(object sender, EventArgs e)
        {
            if (change_exchange)
                AmountChanged(true);
        }
        private void textBoxExchangeRate_TextChanged(object sender, EventArgs e)
        {
            if (change_exchange)
                AmountChanged(true);
        }
        private void textBoxAmountTo_TextChanged(object sender, EventArgs e)
        {
            if (change_exchange)
                AmountChanged(false);
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Int64 f_account = 0;
            float f_amount = 0;
            float f_amount_b = 0;
            float f_currency_rate = 1;
            Int64 t_account = 0;
            float t_amount = 0;
            float t_amount_b = 0;
            float t_currency_rate = 1;
            float currency_rate = 1;
            try
            {
                f_account = Convert.ToInt64(ds.Tables["TransAccounts"].Rows[comboBoxAccountFrom.SelectedIndex]["account_id"]);
            }
            catch { }
            try
            {
                f_amount = Convert.ToSingle(textBoxAmountFrom.Text.Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            try
            {
                f_currency_rate = Convert.ToSingle(ds.Tables["TransAccounts"].Rows[comboBoxAccountFrom.SelectedIndex]["currency_rate"].ToString().Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            try
            {
                f_amount_b = Convert.ToSingle(textBoxBaseFrom.Text.Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            try
            {
                t_account = Convert.ToInt64(ds.Tables["TransAccounts"].Rows[comboBoxAccountTo.SelectedIndex]["account_id"]);
            }
            catch { }
            try
            {
                t_amount = Convert.ToSingle(textBoxAmountTo.Text.Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            try
            {
                t_currency_rate = Convert.ToSingle(ds.Tables["TransAccounts"].Rows[comboBoxAccountTo.SelectedIndex]["currency_rate"].ToString().Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            try
            {
                t_amount_b = Convert.ToSingle(textBoxBaseTo.Text.Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            try
            {
                currency_rate = Convert.ToSingle(textBoxExchangeRate.Text.Trim().Replace(decimal_sign_change, decimal_sign));
            }
            catch { }
            if (pfa_core.Transfer(f_account, f_amount, f_currency_rate, f_amount_b, t_account, t_amount, t_currency_rate, t_amount_b, dateTimePickerDate.Value))
            {
                this.DialogResult = DialogResult.OK;
                // ��������� ����
                pfa_core.AddCrossRate(dateTimePickerDate.Value, ds.Tables["TransAccounts"].Rows[comboBoxAccountFrom.SelectedIndex]["currency_code"].ToString(), ds.Tables["TransAccounts"].Rows[comboBoxAccountTo.SelectedIndex]["currency_code"].ToString(), currency_rate);
            }
            else
                this.DialogResult = DialogResult.Abort;
        }
        private void checkBoxCalcExchRate_CheckedChanged(object sender, EventArgs e)
        {
            textBoxExchangeRate.Enabled = !checkBoxCalcExchRate.Checked;
        }
    }
}