using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    partial class TransferForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferForm));
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelInfo = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelAccountFrom = new System.Windows.Forms.Label();
            this.labelAccountTo = new System.Windows.Forms.Label();
            this.comboBoxAccountFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxAccountTo = new System.Windows.Forms.ComboBox();
            this.labelAmountFrom = new System.Windows.Forms.Label();
            this.textBoxAmountFrom = new System.Windows.Forms.TextBox();
            this.labelCRate = new System.Windows.Forms.Label();
            this.labelAmountTo = new System.Windows.Forms.Label();
            this.textBoxExchangeRate = new System.Windows.Forms.TextBox();
            this.textBoxAmountTo = new System.Windows.Forms.TextBox();
            this.textBoxBaseFrom = new System.Windows.Forms.TextBox();
            this.textBoxBaseTo = new System.Windows.Forms.TextBox();
            this.labelDate = new System.Windows.Forms.Label();
            this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
            this.labelBaseCur1 = new System.Windows.Forms.Label();
            this.labelBaseCur2 = new System.Windows.Forms.Label();
            this.checkBoxCalcExchRate = new System.Windows.Forms.CheckBox();
            this.panelInfo.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelInfo
            // 
            this.panelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.panelInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInfo.Controls.Add(this.labelInfo);
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInfo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.panelInfo.ForeColor = System.Drawing.Color.Maroon;
            this.panelInfo.Location = new System.Drawing.Point(0, 0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(494, 25);
            this.panelInfo.TabIndex = 0;
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelInfo.Location = new System.Drawing.Point(10, 5);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(57, 14);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = "Transfer";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonSave);
            this.panelBottom.Controls.Add(this.buttonClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 223);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(494, 55);
            this.panelBottom.TabIndex = 1;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Image = Resources.save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(335, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Image = Resources.close;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(416, 3);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 50);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelAccountFrom
            // 
            this.labelAccountFrom.AutoSize = true;
            this.labelAccountFrom.Location = new System.Drawing.Point(12, 43);
            this.labelAccountFrom.Name = "labelAccountFrom";
            this.labelAccountFrom.Size = new System.Drawing.Size(75, 13);
            this.labelAccountFrom.TabIndex = 2;
            this.labelAccountFrom.Text = "From account:";
            // 
            // labelAccountTo
            // 
            this.labelAccountTo.AutoSize = true;
            this.labelAccountTo.Location = new System.Drawing.Point(279, 43);
            this.labelAccountTo.Name = "labelAccountTo";
            this.labelAccountTo.Size = new System.Drawing.Size(65, 13);
            this.labelAccountTo.TabIndex = 3;
            this.labelAccountTo.Text = "To account:";
            // 
            // comboBoxAccountFrom
            // 
            this.comboBoxAccountFrom.FormattingEnabled = true;
            this.comboBoxAccountFrom.Location = new System.Drawing.Point(15, 59);
            this.comboBoxAccountFrom.Name = "comboBoxAccountFrom";
            this.comboBoxAccountFrom.Size = new System.Drawing.Size(200, 21);
            this.comboBoxAccountFrom.TabIndex = 4;
            this.comboBoxAccountFrom.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccountFrom_SelectedIndexChanged);
            // 
            // comboBoxAccountTo
            // 
            this.comboBoxAccountTo.FormattingEnabled = true;
            this.comboBoxAccountTo.Location = new System.Drawing.Point(282, 59);
            this.comboBoxAccountTo.Name = "comboBoxAccountTo";
            this.comboBoxAccountTo.Size = new System.Drawing.Size(200, 21);
            this.comboBoxAccountTo.TabIndex = 5;
            this.comboBoxAccountTo.SelectedIndexChanged += new System.EventHandler(this.comboBoxAccountTo_SelectedIndexChanged);
            // 
            // labelAmountFrom
            // 
            this.labelAmountFrom.AutoSize = true;
            this.labelAmountFrom.Location = new System.Drawing.Point(52, 99);
            this.labelAmountFrom.Name = "labelAmountFrom";
            this.labelAmountFrom.Size = new System.Drawing.Size(46, 13);
            this.labelAmountFrom.TabIndex = 6;
            this.labelAmountFrom.Text = "Amount:";
            // 
            // textBoxAmountFrom
            // 
            this.textBoxAmountFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAmountFrom.Location = new System.Drawing.Point(55, 115);
            this.textBoxAmountFrom.Name = "textBoxAmountFrom";
            this.textBoxAmountFrom.Size = new System.Drawing.Size(100, 20);
            this.textBoxAmountFrom.TabIndex = 7;
            this.textBoxAmountFrom.Text = "0";
            this.textBoxAmountFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxAmountFrom.TextChanged += new System.EventHandler(this.textBoxAmountFrom_TextChanged);
            // 
            // labelCRate
            // 
            this.labelCRate.AutoSize = true;
            this.labelCRate.Location = new System.Drawing.Point(197, 99);
            this.labelCRate.Name = "labelCRate";
            this.labelCRate.Size = new System.Drawing.Size(79, 13);
            this.labelCRate.TabIndex = 8;
            this.labelCRate.Text = "Exchange rate:";
            // 
            // labelAmountTo
            // 
            this.labelAmountTo.AutoSize = true;
            this.labelAmountTo.Location = new System.Drawing.Point(336, 99);
            this.labelAmountTo.Name = "labelAmountTo";
            this.labelAmountTo.Size = new System.Drawing.Size(46, 13);
            this.labelAmountTo.TabIndex = 9;
            this.labelAmountTo.Text = "Amount:";
            // 
            // textBoxExchangeRate
            // 
            this.textBoxExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxExchangeRate.Location = new System.Drawing.Point(200, 115);
            this.textBoxExchangeRate.Name = "textBoxExchangeRate";
            this.textBoxExchangeRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxExchangeRate.TabIndex = 10;
            this.textBoxExchangeRate.Text = "1";
            this.textBoxExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxExchangeRate.TextChanged += new System.EventHandler(this.textBoxExchangeRate_TextChanged);
            // 
            // textBoxAmountTo
            // 
            this.textBoxAmountTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAmountTo.Location = new System.Drawing.Point(339, 115);
            this.textBoxAmountTo.Name = "textBoxAmountTo";
            this.textBoxAmountTo.Size = new System.Drawing.Size(100, 20);
            this.textBoxAmountTo.TabIndex = 11;
            this.textBoxAmountTo.Text = "0";
            this.textBoxAmountTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxAmountTo.TextChanged += new System.EventHandler(this.textBoxAmountTo_TextChanged);
            // 
            // textBoxBaseFrom
            // 
            this.textBoxBaseFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBaseFrom.Location = new System.Drawing.Point(55, 141);
            this.textBoxBaseFrom.Name = "textBoxBaseFrom";
            this.textBoxBaseFrom.ReadOnly = true;
            this.textBoxBaseFrom.Size = new System.Drawing.Size(100, 20);
            this.textBoxBaseFrom.TabIndex = 12;
            this.textBoxBaseFrom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBoxBaseTo
            // 
            this.textBoxBaseTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBaseTo.Location = new System.Drawing.Point(339, 141);
            this.textBoxBaseTo.Name = "textBoxBaseTo";
            this.textBoxBaseTo.ReadOnly = true;
            this.textBoxBaseTo.Size = new System.Drawing.Size(100, 20);
            this.textBoxBaseTo.TabIndex = 13;
            this.textBoxBaseTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(11, 183);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(33, 13);
            this.labelDate.TabIndex = 14;
            this.labelDate.Text = "Date:";
            // 
            // dateTimePickerDate
            // 
            this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerDate.Location = new System.Drawing.Point(55, 179);
            this.dateTimePickerDate.Name = "dateTimePickerDate";
            this.dateTimePickerDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerDate.TabIndex = 15;
            // 
            // labelBaseCur1
            // 
            this.labelBaseCur1.AutoSize = true;
            this.labelBaseCur1.Enabled = false;
            this.labelBaseCur1.Location = new System.Drawing.Point(161, 145);
            this.labelBaseCur1.Name = "labelBaseCur1";
            this.labelBaseCur1.Size = new System.Drawing.Size(30, 13);
            this.labelBaseCur1.TabIndex = 16;
            this.labelBaseCur1.Text = "EUR";
            // 
            // labelBaseCur2
            // 
            this.labelBaseCur2.AutoSize = true;
            this.labelBaseCur2.Enabled = false;
            this.labelBaseCur2.Location = new System.Drawing.Point(445, 145);
            this.labelBaseCur2.Name = "labelBaseCur2";
            this.labelBaseCur2.Size = new System.Drawing.Size(30, 13);
            this.labelBaseCur2.TabIndex = 17;
            this.labelBaseCur2.Text = "EUR";
            // 
            // checkBoxCalcExchRate
            // 
            this.checkBoxCalcExchRate.AutoSize = true;
            this.checkBoxCalcExchRate.Location = new System.Drawing.Point(220, 142);
            this.checkBoxCalcExchRate.Name = "checkBoxCalcExchRate";
            this.checkBoxCalcExchRate.Size = new System.Drawing.Size(70, 17);
            this.checkBoxCalcExchRate.TabIndex = 18;
            this.checkBoxCalcExchRate.Text = "Calculate";
            this.checkBoxCalcExchRate.UseVisualStyleBackColor = true;
            this.checkBoxCalcExchRate.CheckedChanged += new System.EventHandler(this.checkBoxCalcExchRate_CheckedChanged);
            // 
            // TransferForm
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(494, 278);
            this.Controls.Add(this.checkBoxCalcExchRate);
            this.Controls.Add(this.labelBaseCur2);
            this.Controls.Add(this.labelBaseCur1);
            this.Controls.Add(this.dateTimePickerDate);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.textBoxBaseTo);
            this.Controls.Add(this.textBoxBaseFrom);
            this.Controls.Add(this.textBoxAmountTo);
            this.Controls.Add(this.textBoxExchangeRate);
            this.Controls.Add(this.labelAmountTo);
            this.Controls.Add(this.labelCRate);
            this.Controls.Add(this.textBoxAmountFrom);
            this.Controls.Add(this.labelAmountFrom);
            this.Controls.Add(this.comboBoxAccountTo);
            this.Controls.Add(this.comboBoxAccountFrom);
            this.Controls.Add(this.labelAccountTo);
            this.Controls.Add(this.labelAccountFrom);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TransferForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Transfer";
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelAccountFrom;
        private System.Windows.Forms.Label labelAccountTo;
        private System.Windows.Forms.ComboBox comboBoxAccountFrom;
        private System.Windows.Forms.ComboBox comboBoxAccountTo;
        private System.Windows.Forms.Label labelAmountFrom;
        private System.Windows.Forms.TextBox textBoxAmountFrom;
        private System.Windows.Forms.Label labelCRate;
        private System.Windows.Forms.Label labelAmountTo;
        private System.Windows.Forms.TextBox textBoxExchangeRate;
        private System.Windows.Forms.TextBox textBoxAmountTo;
        private System.Windows.Forms.TextBox textBoxBaseFrom;
        private System.Windows.Forms.TextBox textBoxBaseTo;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerDate;
        private System.Windows.Forms.Label labelBaseCur1;
        private System.Windows.Forms.Label labelBaseCur2;
        private System.Windows.Forms.CheckBox checkBoxCalcExchRate;
    }
}