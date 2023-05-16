using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Security.AccessControl;


namespace EvgTar.PFA
{
    public partial class Options : Form
    {
        public Core pfa_core;
        public DataSet ds = new DataSet();
        public Settings settings;
        public Languages NLS = null;

        public Options()
        {
            InitializeComponent();
        }
        public void Init()
        {
            this.Text = NLS.LNGetString("TitleOptions", "Options");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            tabPageGeneral.Text = NLS.LNGetString("TextGeneral", "General");
            tabPageColors.Text = NLS.LNGetString("TextColors", "Colors");
            checkBoxShowToday.Text = NLS.LNGetString("TextShowTodayAtStartup", "Show Today at startup");
            groupBoxHeader.Text = NLS.LNGetString("TextHeaders", "Headers");
            groupBoxRows.Text = NLS.LNGetString("TextRows", "Rows");
            labelRowNormal.Text = NLS.LNGetString("TextRowNormal", "Normal:");
            labelRowHL.Text = NLS.LNGetString("TextRowHighlighted", "Highlighted:");
            labelRowFont.Text = NLS.LNGetString("TextFont", "Rows") + ":";
            buttonSetDef.Text = NLS.LNGetString("TextSetDefaultSettings", "Set default settings");
            labelLanguage.Text = NLS.LNGetString("TextLanguage", "Language") + ":";
            buttonAssociatePFA.Text = NLS.LNGetString("TextAssociatePFA", "Associate with pfa extension");
            checkBoxHideInTaskbar.Text = NLS.LNGetString("TextHideInTaskbar", "Hide in Taskbar");
            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.Add("English");

            DirectoryInfo dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Languages");
            FileInfo[] files = dir.GetFiles("*.dll");
            string l;
            for (int i = 0; i < files.Length; i++)
            {
                l = files[i].Name.Substring(0, files[i].Name.Length - files[i].Extension.Length);
                if (!l.Equals("English"))
                    comboBoxLanguage.Items.Add(l);                    
            }
            for (int i = 0; i < comboBoxLanguage.Items.Count; i++)
            {
                if (comboBoxLanguage.Items[i].ToString().Equals(settings.Language))
                    comboBoxLanguage.SelectedIndex = i;
            }
            checkBoxShowToday.Checked = settings.ShowToday;
            checkBoxHideInTaskbar.Checked = settings.HideInTaskbar;
            buttonColorCells.BackColor = Color.FromArgb(settings.CellColor[0], settings.CellColor[1], settings.CellColor[2]);

            comboBoxDefaultAccount.BeginUpdate();
            comboBoxDefaultAccount.Items.Clear();
            int ind_account = 0;
            if (pfa_core.AccountsGet(ref ds))
            {
                comboBoxDefaultAccount.Items.Clear();
                for (int i = 0; i < ds.Tables["Accounts"].Rows.Count; i++)
                {
                    comboBoxDefaultAccount.Items.Add(ds.Tables["Accounts"].Rows[i]["account_name"].ToString());
                    if (ds.Tables["Accounts"].Rows[i]["account_status"].ToString().Equals("2"))
                        ind_account = i;
                }
            }
            if (comboBoxDefaultAccount.Items.Count > 0)
                comboBoxDefaultAccount.SelectedIndex = ind_account;
            comboBoxDefaultAccount.EndUpdate();

        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void buttonColorCells_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = buttonColorCells.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                    buttonColorCells.BackColor = cd.Color;                    
            }
        }
        private void buttonHLRows_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = buttonHLRows.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                    buttonHLRows.BackColor = cd.Color;
            }
        }
        private void buttonSetDef_Click(object sender, EventArgs e)
        {
            buttonColorCells.BackColor = Color.LightYellow;
            buttonColorCells.BackColor = Color.LightYellow;
        }
        private void buttonColorCells_BackColorChanged(object sender, EventArgs e)
        {
            buttonColorCells.Text = "#" + buttonColorCells.BackColor.R.ToString("X2") + buttonColorCells.BackColor.G.ToString("X2") + buttonColorCells.BackColor.B.ToString("X2");
        }
        private void buttonHLRows_BackColorChanged(object sender, EventArgs e)
        {
            buttonHLRows.Text = "#" + buttonHLRows.BackColor.R.ToString("X2") + buttonHLRows.BackColor.G.ToString("X2") + buttonHLRows.BackColor.B.ToString("X2");
        }
        private void buttonRFont_Click(object sender, EventArgs e)
        {
            using (FontDialog fd = new FontDialog())
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    buttonRFont.Text = fd.Font.Name + ", " + fd.Font.Size.ToString();
                    if (fd.Font.Bold)
                        buttonRFont.Text += ", Bold";
                    if (fd.Font.Italic)
                        buttonRFont.Text += ", Italic";                    
                }
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            string acc_id = "";
            if (ds.Tables.Contains("Accounts"))
            {
                try
                {
                    acc_id = ds.Tables["Accounts"].Rows[comboBoxDefaultAccount.SelectedIndex]["account_id"].ToString();
                }
                catch { }
                if (!acc_id.Equals(""))
                {
                    //string sql = "update Accounts set account_status = 2 where account_id = " + acc_id;
                    //sqlite.ExecuteSQL(sql);
                }
            }            
            settings.Save();
        }
        private void checkBoxShowToday_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShowToday = checkBoxShowToday.Checked;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            settings.Language = comboBoxLanguage.Items[comboBoxLanguage.SelectedIndex].ToString();
        }
        private void buttonAssociatePFA_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(".pfa", true);
                if (rkey == null)
                    rkey = Registry.ClassesRoot.CreateSubKey(".pfa", RegistryKeyPermissionCheck.Default);
                if (rkey != null)
                    rkey.SetValue("", "Shwaps.PFA");
                rkey = Registry.ClassesRoot.OpenSubKey("Shwaps.PFA", true);
                if (rkey == null)
                    rkey = Registry.ClassesRoot.CreateSubKey("Shwaps.PFA", RegistryKeyPermissionCheck.Default);
                if (rkey != null)
                {
                    rkey.SetValue("", "Personal Finance Assistant Database");
                    RegistryKey rkey1 = rkey.CreateSubKey("DefaultIcon");
                    rkey1.SetValue("", Application.ExecutablePath + ",0");
                    rkey1.Flush();
                    rkey1 = rkey.CreateSubKey("Shell");
                    RegistryKey rkey2 = rkey1.CreateSubKey("Open");
                    rkey2.SetValue("", NLS.LNGetString("TextOpen", "Open"));
                    RegistryKey rkey3 = rkey2.CreateSubKey("Command");
                    rkey3.SetValue("", Application.ExecutablePath + " \"%1\"");
                    rkey1.Flush();
                    rkey2.Flush();
                    rkey3.Flush();
                }
            }
            catch { };
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            settings.HideInTaskbar = checkBoxHideInTaskbar.Checked;
        }
    }
}