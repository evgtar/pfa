using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    partial class ProjectProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectProperties));
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelBaseCurrency = new System.Windows.Forms.Label();
            this.comboBoxCurrencies = new System.Windows.Forms.ComboBox();
            this.labelDBFile = new System.Windows.Forms.Label();
            this.textBoxDBFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Image = Resources.close;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(205, 129);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 50);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Image = Resources.save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(124, 129);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelBaseCurrency
            // 
            this.labelBaseCurrency.AutoSize = true;
            this.labelBaseCurrency.Location = new System.Drawing.Point(12, 9);
            this.labelBaseCurrency.Name = "labelBaseCurrency";
            this.labelBaseCurrency.Size = new System.Drawing.Size(78, 13);
            this.labelBaseCurrency.TabIndex = 2;
            this.labelBaseCurrency.Text = "Base currency:";
            // 
            // comboBoxCurrencies
            // 
            this.comboBoxCurrencies.FormattingEnabled = true;
            this.comboBoxCurrencies.Location = new System.Drawing.Point(27, 25);
            this.comboBoxCurrencies.Name = "comboBoxCurrencies";
            this.comboBoxCurrencies.Size = new System.Drawing.Size(253, 21);
            this.comboBoxCurrencies.TabIndex = 3;
            // 
            // labelDBFile
            // 
            this.labelDBFile.AutoSize = true;
            this.labelDBFile.Location = new System.Drawing.Point(12, 58);
            this.labelDBFile.Name = "labelDBFile";
            this.labelDBFile.Size = new System.Drawing.Size(72, 13);
            this.labelDBFile.TabIndex = 4;
            this.labelDBFile.Text = "Database file:";
            // 
            // textBoxDBFile
            // 
            this.textBoxDBFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.textBoxDBFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDBFile.Location = new System.Drawing.Point(27, 74);
            this.textBoxDBFile.Multiline = true;
            this.textBoxDBFile.Name = "textBoxDBFile";
            this.textBoxDBFile.ReadOnly = true;
            this.textBoxDBFile.Size = new System.Drawing.Size(253, 49);
            this.textBoxDBFile.TabIndex = 5;
            // 
            // ProjectProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 191);
            this.Controls.Add(this.textBoxDBFile);
            this.Controls.Add(this.labelDBFile);
            this.Controls.Add(this.comboBoxCurrencies);
            this.Controls.Add(this.labelBaseCurrency);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProjectProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Properties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelBaseCurrency;
        private System.Windows.Forms.ComboBox comboBoxCurrencies;
        private System.Windows.Forms.Label labelDBFile;
        private System.Windows.Forms.TextBox textBoxDBFile;
    }
}