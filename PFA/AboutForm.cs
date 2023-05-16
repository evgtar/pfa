using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    public partial class AboutForm : Form
    {
        public Languages NLS = null;
        
        public AboutForm()
        {
            InitializeComponent();
            labelVersion.Text = string.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            labelProductName.Text = AssemblyProduct;
        }
        public void Init()
        {
            this.Text = NLS.LNGetString("TitleAbout", "About");
            switch (NLS.Language)
            {
                case "Russian":
                    textBoxVersionHistory.Text = Resources.VersionHistoryRU;
                    break;
            }
        }
        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void linkLabelURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/evgtar/pfa");
        }
    }
}