using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    partial class AccountsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("�������� USD");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("�������� EURO");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("�������� BYR");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxInitialBalance = new System.Windows.Forms.TextBox();
            this.labelInitialBalance = new System.Windows.Forms.Label();
            this.buttonChooseCurrency = new System.Windows.Forms.Button();
            this.comboBoxCurrencies = new System.Windows.Forms.ComboBox();
            this.labelCurrency = new System.Windows.Forms.Label();
            this.comboBoxAccountType = new System.Windows.Forms.ComboBox();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.buttonChooseHolder = new System.Windows.Forms.Button();
            this.comboBoxHolder = new System.Windows.Forms.ComboBox();
            this.labelAccountHolder = new System.Windows.Forms.Label();
            this.textBoxAccountNumber = new System.Windows.Forms.TextBox();
            this.labelAccountNumber = new System.Windows.Forms.Label();
            this.textBoxAccountName = new System.Windows.Forms.TextBox();
            this.labelAccountName = new System.Windows.Forms.Label();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.checkBoxActive);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel2.Controls.Add(this.buttonNew);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxInitialBalance);
            this.splitContainer1.Panel2.Controls.Add(this.labelInitialBalance);
            this.splitContainer1.Panel2.Controls.Add(this.buttonChooseCurrency);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxCurrencies);
            this.splitContainer1.Panel2.Controls.Add(this.labelCurrency);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxAccountType);
            this.splitContainer1.Panel2.Controls.Add(this.labelAccountType);
            this.splitContainer1.Panel2.Controls.Add(this.buttonChooseHolder);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxHolder);
            this.splitContainer1.Panel2.Controls.Add(this.labelAccountHolder);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAccountNumber);
            this.splitContainer1.Panel2.Controls.Add(this.labelAccountNumber);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxAccountName);
            this.splitContainer1.Panel2.Controls.Add(this.labelAccountName);
            this.splitContainer1.Size = new System.Drawing.Size(594, 278);
            this.splitContainer1.SplitterDistance = 157;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node0";
            treeNode1.Tag = "1";
            treeNode1.Text = "�������� USD";
            treeNode2.Name = "Node1";
            treeNode2.Tag = "2";
            treeNode2.Text = "�������� EURO";
            treeNode3.Name = "Node2";
            treeNode3.Tag = "3";
            treeNode3.Text = "�������� BYR";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(157, 278);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.Checked = true;
            this.checkBoxActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxActive.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.checkBoxActive.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.checkBoxActive.Location = new System.Drawing.Point(136, 174);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(56, 17);
            this.checkBoxActive.TabIndex = 18;
            this.checkBoxActive.Text = "Active";
            this.checkBoxActive.UseVisualStyleBackColor = true;
            this.checkBoxActive.CheckedChanged += new System.EventHandler(this.checkBoxActive_CheckedChanged);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Enabled = false;
            this.buttonDelete.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Image = Resources.remove;
            this.buttonDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDelete.Location = new System.Drawing.Point(84, 225);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 50);
            this.buttonDelete.TabIndex = 17;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Image = Resources.save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(274, 225);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 16;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNew.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNew.Image = Resources._new;
            this.buttonNew.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonNew.Location = new System.Drawing.Point(3, 225);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 50);
            this.buttonNew.TabIndex = 15;
            this.buttonNew.Text = "New";
            this.buttonNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Image = Resources.close;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(355, 225);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 50);
            this.buttonClose.TabIndex = 14;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxInitialBalance
            // 
            this.textBoxInitialBalance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxInitialBalance.Location = new System.Drawing.Point(136, 148);
            this.textBoxInitialBalance.Name = "textBoxInitialBalance";
            this.textBoxInitialBalance.Size = new System.Drawing.Size(92, 20);
            this.textBoxInitialBalance.TabIndex = 13;
            this.textBoxInitialBalance.Text = "0";
            this.textBoxInitialBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxInitialBalance.TextChanged += new System.EventHandler(this.textBoxInitialBalance_TextChanged);
            // 
            // labelInitialBalance
            // 
            this.labelInitialBalance.AutoSize = true;
            this.labelInitialBalance.Location = new System.Drawing.Point(16, 151);
            this.labelInitialBalance.Name = "labelInitialBalance";
            this.labelInitialBalance.Size = new System.Drawing.Size(75, 13);
            this.labelInitialBalance.TabIndex = 12;
            this.labelInitialBalance.Text = "Initial balance:";
            // 
            // buttonChooseCurrency
            // 
            this.buttonChooseCurrency.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonChooseCurrency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChooseCurrency.Location = new System.Drawing.Point(330, 119);
            this.buttonChooseCurrency.Name = "buttonChooseCurrency";
            this.buttonChooseCurrency.Size = new System.Drawing.Size(30, 23);
            this.buttonChooseCurrency.TabIndex = 11;
            this.buttonChooseCurrency.Text = "...";
            this.buttonChooseCurrency.UseVisualStyleBackColor = true;
            this.buttonChooseCurrency.Click += new System.EventHandler(this.buttonChooseCurrency_Click);
            // 
            // comboBoxCurrencies
            // 
            this.comboBoxCurrencies.FormattingEnabled = true;
            this.comboBoxCurrencies.Location = new System.Drawing.Point(136, 121);
            this.comboBoxCurrencies.Name = "comboBoxCurrencies";
            this.comboBoxCurrencies.Size = new System.Drawing.Size(188, 21);
            this.comboBoxCurrencies.TabIndex = 10;
            this.comboBoxCurrencies.SelectedIndexChanged += new System.EventHandler(this.comboBoxCurrencies_SelectedIndexChanged);
            // 
            // labelCurrency
            // 
            this.labelCurrency.AutoSize = true;
            this.labelCurrency.Location = new System.Drawing.Point(16, 124);
            this.labelCurrency.Name = "labelCurrency";
            this.labelCurrency.Size = new System.Drawing.Size(52, 13);
            this.labelCurrency.TabIndex = 9;
            this.labelCurrency.Text = "Currency:";
            // 
            // comboBoxAccountType
            // 
            this.comboBoxAccountType.FormattingEnabled = true;
            this.comboBoxAccountType.Location = new System.Drawing.Point(136, 94);
            this.comboBoxAccountType.Name = "comboBoxAccountType";
            this.comboBoxAccountType.Size = new System.Drawing.Size(224, 21);
            this.comboBoxAccountType.TabIndex = 8;
            this.comboBoxAccountType.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccountType_SelectedIndexChanged);
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Location = new System.Drawing.Point(16, 97);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(73, 13);
            this.labelAccountType.TabIndex = 7;
            this.labelAccountType.Text = "Account type:";
            // 
            // buttonChooseHolder
            // 
            this.buttonChooseHolder.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonChooseHolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonChooseHolder.Location = new System.Drawing.Point(330, 65);
            this.buttonChooseHolder.Name = "buttonChooseHolder";
            this.buttonChooseHolder.Size = new System.Drawing.Size(30, 23);
            this.buttonChooseHolder.TabIndex = 6;
            this.buttonChooseHolder.Text = "...";
            this.buttonChooseHolder.UseVisualStyleBackColor = true;
            // 
            // comboBoxHolder
            // 
            this.comboBoxHolder.FormattingEnabled = true;
            this.comboBoxHolder.Location = new System.Drawing.Point(136, 67);
            this.comboBoxHolder.Name = "comboBoxHolder";
            this.comboBoxHolder.Size = new System.Drawing.Size(188, 21);
            this.comboBoxHolder.TabIndex = 5;
            this.comboBoxHolder.SelectedIndexChanged += new System.EventHandler(this.comboBoxHolder_SelectedIndexChanged);
            // 
            // labelAccountHolder
            // 
            this.labelAccountHolder.AutoSize = true;
            this.labelAccountHolder.Location = new System.Drawing.Point(16, 70);
            this.labelAccountHolder.Name = "labelAccountHolder";
            this.labelAccountHolder.Size = new System.Drawing.Size(82, 13);
            this.labelAccountHolder.TabIndex = 4;
            this.labelAccountHolder.Text = "Account holder:";
            // 
            // textBoxAccountNumber
            // 
            this.textBoxAccountNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAccountNumber.Location = new System.Drawing.Point(136, 41);
            this.textBoxAccountNumber.Name = "textBoxAccountNumber";
            this.textBoxAccountNumber.Size = new System.Drawing.Size(224, 20);
            this.textBoxAccountNumber.TabIndex = 3;
            this.textBoxAccountNumber.TextChanged += new System.EventHandler(this.textBoxAccountNumber_TextChanged);
            // 
            // labelAccountNumber
            // 
            this.labelAccountNumber.AutoSize = true;
            this.labelAccountNumber.Location = new System.Drawing.Point(16, 43);
            this.labelAccountNumber.Name = "labelAccountNumber";
            this.labelAccountNumber.Size = new System.Drawing.Size(88, 13);
            this.labelAccountNumber.TabIndex = 2;
            this.labelAccountNumber.Text = "Account number:";
            // 
            // textBoxAccountName
            // 
            this.textBoxAccountName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAccountName.Location = new System.Drawing.Point(136, 15);
            this.textBoxAccountName.Name = "textBoxAccountName";
            this.textBoxAccountName.Size = new System.Drawing.Size(224, 20);
            this.textBoxAccountName.TabIndex = 1;
            this.textBoxAccountName.TextChanged += new System.EventHandler(this.textBoxAccountName_TextChanged);
            // 
            // labelAccountName
            // 
            this.labelAccountName.AutoSize = true;
            this.labelAccountName.Location = new System.Drawing.Point(16, 18);
            this.labelAccountName.Name = "labelAccountName";
            this.labelAccountName.Size = new System.Drawing.Size(79, 13);
            this.labelAccountName.TabIndex = 0;
            this.labelAccountName.Text = "Account name:";
            // 
            // AccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(594, 278);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AccountsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Accounts";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label labelAccountNumber;
        private System.Windows.Forms.TextBox textBoxAccountName;
        private System.Windows.Forms.Label labelAccountName;
        private System.Windows.Forms.Button buttonChooseHolder;
        private System.Windows.Forms.ComboBox comboBoxHolder;
        private System.Windows.Forms.Label labelAccountHolder;
        private System.Windows.Forms.TextBox textBoxAccountNumber;
        private System.Windows.Forms.Button buttonChooseCurrency;
        private System.Windows.Forms.ComboBox comboBoxCurrencies;
        private System.Windows.Forms.Label labelCurrency;
        private System.Windows.Forms.ComboBox comboBoxAccountType;
        private System.Windows.Forms.Label labelAccountType;
        private System.Windows.Forms.TextBox textBoxInitialBalance;
        private System.Windows.Forms.Label labelInitialBalance;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.CheckBox checkBoxActive;
    }
}