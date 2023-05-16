using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SKTIT.PFA
{
    public partial class CreateDatabase : Form
    {
        public CreateDatabase()
        {
            InitializeComponent();
        }
        public int ProgressBarPosition
        {
            get { return progressBar1.Value; }
            set { progressBar1.Value = value; }
        }
        public int ProgressBarMax
        {
            get { return progressBar1.Maximum; }
            set { progressBar1.Maximum = value; }
        }
        public string Info
        {
            get { return labelInfo.Text; }
            set { labelInfo.Text = value.Trim(); }
        }
        public string Title
        {
            get { return this.Text; }
            set { this.Text = value.Trim(); }
        }
    }
}