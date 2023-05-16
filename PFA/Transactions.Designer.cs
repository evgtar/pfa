using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    partial class TransactionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransactionForm));
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelAccount = new System.Windows.Forms.Label();
            this.comboBoxAccount = new System.Windows.Forms.ComboBox();
            this.labelPaymentType = new System.Windows.Forms.Label();
            this.comboBoxPaymentType = new System.Windows.Forms.ComboBox();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelAmount = new System.Windows.Forms.Label();
            this.textBoxAmount = new System.Windows.Forms.TextBox();
            this.labelCRate = new System.Windows.Forms.Label();
            this.textBoxCRate = new System.Windows.Forms.TextBox();
            this.labelAmountBaseC = new System.Windows.Forms.Label();
            this.textBoxAmountBase = new System.Windows.Forms.TextBox();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelPayer = new System.Windows.Forms.Label();
            this.comboBoxPayer = new System.Windows.Forms.ComboBox();
            this.comboBoxCategory = new System.Windows.Forms.ComboBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.dateTimePickerTransDate = new System.Windows.Forms.DateTimePicker();
            this.buttonGetPayer = new System.Windows.Forms.Button();
            this.buttonGetCategory = new System.Windows.Forms.Button();
            this.comboBoxAsset = new System.Windows.Forms.ComboBox();
            this.labelAsset = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonClose.Image = Resources.close;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(311, 326);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 50);
            this.buttonClose.TabIndex = 16;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(14, 54);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(75, 13);
            this.labelAccount.TabIndex = 1;
            this.labelAccount.Text = "From account:";
            // 
            // comboBoxAccount
            // 
            this.comboBoxAccount.FormattingEnabled = true;
            this.comboBoxAccount.Location = new System.Drawing.Point(107, 51);
            this.comboBoxAccount.Name = "comboBoxAccount";
            this.comboBoxAccount.Size = new System.Drawing.Size(241, 21);
            this.comboBoxAccount.TabIndex = 2;
            this.comboBoxAccount.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccounts_SelectedIndexChanged);
            // 
            // labelPaymentType
            // 
            this.labelPaymentType.AutoSize = true;
            this.labelPaymentType.Location = new System.Drawing.Point(14, 81);
            this.labelPaymentType.Name = "labelPaymentType";
            this.labelPaymentType.Size = new System.Drawing.Size(89, 13);
            this.labelPaymentType.TabIndex = 3;
            this.labelPaymentType.Text = "Transaction type:";
            // 
            // comboBoxPaymentType
            // 
            this.comboBoxPaymentType.FormattingEnabled = true;
            this.comboBoxPaymentType.Items.AddRange(new object[] {
            "Deposit",
            "Withdraw",
            "Transfer"});
            this.comboBoxPaymentType.Location = new System.Drawing.Point(107, 78);
            this.comboBoxPaymentType.Name = "comboBoxPaymentType";
            this.comboBoxPaymentType.Size = new System.Drawing.Size(241, 21);
            this.comboBoxPaymentType.TabIndex = 3;
            this.comboBoxPaymentType.SelectedIndexChanged += new System.EventHandler(this.comboBoxPaymentType_SelectedIndexChanged);
            // 
            // labelCategory
            // 
            this.labelCategory.AutoSize = true;
            this.labelCategory.Location = new System.Drawing.Point(14, 135);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(52, 13);
            this.labelCategory.TabIndex = 5;
            this.labelCategory.Text = "Category:";
            // 
            // labelAmount
            // 
            this.labelAmount.AutoSize = true;
            this.labelAmount.Location = new System.Drawing.Point(14, 204);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(46, 13);
            this.labelAmount.TabIndex = 7;
            this.labelAmount.Text = "Amount:";
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAmount.Location = new System.Drawing.Point(29, 220);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.Size = new System.Drawing.Size(100, 20);
            this.textBoxAmount.TabIndex = 9;
            this.textBoxAmount.TextChanged += new System.EventHandler(this.textBoxAmount_TextChanged);
            // 
            // labelCRate
            // 
            this.labelCRate.AutoSize = true;
            this.labelCRate.Location = new System.Drawing.Point(134, 204);
            this.labelCRate.Name = "labelCRate";
            this.labelCRate.Size = new System.Drawing.Size(73, 13);
            this.labelCRate.TabIndex = 9;
            this.labelCRate.Text = "Currency rate:";
            // 
            // textBoxCRate
            // 
            this.textBoxCRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCRate.Location = new System.Drawing.Point(149, 220);
            this.textBoxCRate.Name = "textBoxCRate";
            this.textBoxCRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxCRate.TabIndex = 10;
            this.textBoxCRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxCRate.TextChanged += new System.EventHandler(this.textBoxCRate_TextChanged);
            // 
            // labelAmountBaseC
            // 
            this.labelAmountBaseC.AutoSize = true;
            this.labelAmountBaseC.Location = new System.Drawing.Point(250, 204);
            this.labelAmountBaseC.Name = "labelAmountBaseC";
            this.labelAmountBaseC.Size = new System.Drawing.Size(78, 13);
            this.labelAmountBaseC.TabIndex = 11;
            this.labelAmountBaseC.Text = "Amount (EUR):";
            // 
            // textBoxAmountBase
            // 
            this.textBoxAmountBase.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAmountBase.Enabled = false;
            this.textBoxAmountBase.Location = new System.Drawing.Point(270, 220);
            this.textBoxAmountBase.Name = "textBoxAmountBase";
            this.textBoxAmountBase.Size = new System.Drawing.Size(100, 20);
            this.textBoxAmountBase.TabIndex = 11;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Location = new System.Drawing.Point(14, 254);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(38, 13);
            this.labelNotes.TabIndex = 13;
            this.labelNotes.Text = "Notes:";
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxNotes.Location = new System.Drawing.Point(17, 270);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(365, 50);
            this.textBoxNotes.TabIndex = 12;
            this.textBoxNotes.TextChanged += new System.EventHandler(this.textBoxNotes_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Enabled = false;
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSave.Image = Resources.save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(230, 326);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 15;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelPayer
            // 
            this.labelPayer.AutoSize = true;
            this.labelPayer.Location = new System.Drawing.Point(14, 108);
            this.labelPayer.Name = "labelPayer";
            this.labelPayer.Size = new System.Drawing.Size(37, 13);
            this.labelPayer.TabIndex = 16;
            this.labelPayer.Text = "Payer:";
            // 
            // comboBoxPayer
            // 
            this.comboBoxPayer.FormattingEnabled = true;
            this.comboBoxPayer.Location = new System.Drawing.Point(107, 105);
            this.comboBoxPayer.Name = "comboBoxPayer";
            this.comboBoxPayer.Size = new System.Drawing.Size(241, 21);
            this.comboBoxPayer.TabIndex = 4;
            this.comboBoxPayer.SelectedIndexChanged += new System.EventHandler(this.comboBoxPayer_SelectedIndexChanged);
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(107, 132);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(241, 21);
            this.comboBoxCategory.TabIndex = 6;
            this.comboBoxCategory.SelectedIndexChanged += new System.EventHandler(this.comboBoxCategory_SelectedIndexChanged);
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(14, 16);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(33, 13);
            this.labelDate.TabIndex = 19;
            this.labelDate.Text = "Date:";
            // 
            // dateTimePickerTransDate
            // 
            this.dateTimePickerTransDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTransDate.Location = new System.Drawing.Point(107, 12);
            this.dateTimePickerTransDate.Name = "dateTimePickerTransDate";
            this.dateTimePickerTransDate.Size = new System.Drawing.Size(241, 20);
            this.dateTimePickerTransDate.TabIndex = 1;
            // 
            // buttonGetPayer
            // 
            this.buttonGetPayer.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonGetPayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetPayer.Location = new System.Drawing.Point(349, 104);
            this.buttonGetPayer.Name = "buttonGetPayer";
            this.buttonGetPayer.Size = new System.Drawing.Size(33, 23);
            this.buttonGetPayer.TabIndex = 5;
            this.buttonGetPayer.Text = "...";
            this.buttonGetPayer.UseVisualStyleBackColor = true;
            this.buttonGetPayer.Click += new System.EventHandler(this.buttonGetPayer_Click);
            // 
            // buttonGetCategory
            // 
            this.buttonGetCategory.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonGetCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGetCategory.Location = new System.Drawing.Point(349, 130);
            this.buttonGetCategory.Name = "buttonGetCategory";
            this.buttonGetCategory.Size = new System.Drawing.Size(33, 23);
            this.buttonGetCategory.TabIndex = 7;
            this.buttonGetCategory.Text = "...";
            this.buttonGetCategory.UseVisualStyleBackColor = true;
            this.buttonGetCategory.Click += new System.EventHandler(this.buttonGetCategory_Click);
            // 
            // comboBoxAsset
            // 
            this.comboBoxAsset.FormattingEnabled = true;
            this.comboBoxAsset.Location = new System.Drawing.Point(107, 159);
            this.comboBoxAsset.Name = "comboBoxAsset";
            this.comboBoxAsset.Size = new System.Drawing.Size(241, 21);
            this.comboBoxAsset.TabIndex = 8;
            // 
            // labelAsset
            // 
            this.labelAsset.AutoSize = true;
            this.labelAsset.Location = new System.Drawing.Point(14, 162);
            this.labelAsset.Name = "labelAsset";
            this.labelAsset.Size = new System.Drawing.Size(36, 13);
            this.labelAsset.TabIndex = 20;
            this.labelAsset.Text = "Asset:";
            // 
            // TransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(398, 385);
            this.Controls.Add(this.comboBoxAsset);
            this.Controls.Add(this.labelAsset);
            this.Controls.Add(this.buttonGetCategory);
            this.Controls.Add(this.buttonGetPayer);
            this.Controls.Add(this.dateTimePickerTransDate);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.comboBoxCategory);
            this.Controls.Add(this.comboBoxPayer);
            this.Controls.Add(this.labelPayer);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxNotes);
            this.Controls.Add(this.labelNotes);
            this.Controls.Add(this.textBoxAmountBase);
            this.Controls.Add(this.labelAmountBaseC);
            this.Controls.Add(this.textBoxCRate);
            this.Controls.Add(this.labelCRate);
            this.Controls.Add(this.textBoxAmount);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.comboBoxPaymentType);
            this.Controls.Add(this.labelPaymentType);
            this.Controls.Add(this.comboBoxAccount);
            this.Controls.Add(this.labelAccount);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TransactionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transaction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.ComboBox comboBoxAccount;
        private System.Windows.Forms.Label labelPaymentType;
        private System.Windows.Forms.ComboBox comboBoxPaymentType;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelAmount;
        private System.Windows.Forms.TextBox textBoxAmount;
        private System.Windows.Forms.Label labelCRate;
        private System.Windows.Forms.TextBox textBoxCRate;
        private System.Windows.Forms.Label labelAmountBaseC;
        private System.Windows.Forms.TextBox textBoxAmountBase;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelPayer;
        private System.Windows.Forms.ComboBox comboBoxPayer;
        private System.Windows.Forms.ComboBox comboBoxCategory;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerTransDate;
        private System.Windows.Forms.Button buttonGetPayer;
        private System.Windows.Forms.Button buttonGetCategory;
        private System.Windows.Forms.ComboBox comboBoxAsset;
        private System.Windows.Forms.Label labelAsset;
    }
}