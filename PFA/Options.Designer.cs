using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    partial class Options
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
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.checkBoxHideInTaskbar = new System.Windows.Forms.CheckBox();
            this.buttonAssociatePFA = new System.Windows.Forms.Button();
            this.comboBoxLanguage = new System.Windows.Forms.ComboBox();
            this.labelLanguage = new System.Windows.Forms.Label();
            this.checkBoxShowToday = new System.Windows.Forms.CheckBox();
            this.tabPageColors = new System.Windows.Forms.TabPage();
            this.groupBoxHeader = new System.Windows.Forms.GroupBox();
            this.groupBoxRows = new System.Windows.Forms.GroupBox();
            this.buttonRFont = new System.Windows.Forms.Button();
            this.labelRowFont = new System.Windows.Forms.Label();
            this.buttonHLRows = new System.Windows.Forms.Button();
            this.labelRowHL = new System.Windows.Forms.Label();
            this.labelRowNormal = new System.Windows.Forms.Label();
            this.buttonColorCells = new System.Windows.Forms.Button();
            this.buttonSetDef = new System.Windows.Forms.Button();
            this.labelDefaultAccount = new System.Windows.Forms.Label();
            this.comboBoxDefaultAccount = new System.Windows.Forms.ComboBox();
            this.panelBottom.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageColors.SuspendLayout();
            this.groupBoxRows.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonSave);
            this.panelBottom.Controls.Add(this.buttonClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 269);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(292, 55);
            this.panelBottom.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Image = Resources.save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(133, 2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Image = Resources.close;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(214, 2);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 50);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageColors);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(292, 269);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.comboBoxDefaultAccount);
            this.tabPageGeneral.Controls.Add(this.labelDefaultAccount);
            this.tabPageGeneral.Controls.Add(this.checkBoxHideInTaskbar);
            this.tabPageGeneral.Controls.Add(this.buttonAssociatePFA);
            this.tabPageGeneral.Controls.Add(this.comboBoxLanguage);
            this.tabPageGeneral.Controls.Add(this.labelLanguage);
            this.tabPageGeneral.Controls.Add(this.checkBoxShowToday);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(284, 243);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // checkBoxHideInTaskbar
            // 
            this.checkBoxHideInTaskbar.AutoSize = true;
            this.checkBoxHideInTaskbar.Location = new System.Drawing.Point(8, 191);
            this.checkBoxHideInTaskbar.Name = "checkBoxHideInTaskbar";
            this.checkBoxHideInTaskbar.Size = new System.Drawing.Size(102, 17);
            this.checkBoxHideInTaskbar.TabIndex = 4;
            this.checkBoxHideInTaskbar.Text = "Hide in TaskBar";
            this.checkBoxHideInTaskbar.UseVisualStyleBackColor = true;
            this.checkBoxHideInTaskbar.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // buttonAssociatePFA
            // 
            this.buttonAssociatePFA.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonAssociatePFA.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAssociatePFA.Location = new System.Drawing.Point(8, 214);
            this.buttonAssociatePFA.Name = "buttonAssociatePFA";
            this.buttonAssociatePFA.Size = new System.Drawing.Size(268, 23);
            this.buttonAssociatePFA.TabIndex = 3;
            this.buttonAssociatePFA.Text = "Associate with pfa extension";
            this.buttonAssociatePFA.UseVisualStyleBackColor = true;
            this.buttonAssociatePFA.Click += new System.EventHandler(this.buttonAssociatePFA_Click);
            // 
            // comboBoxLanguage
            // 
            this.comboBoxLanguage.FormattingEnabled = true;
            this.comboBoxLanguage.Location = new System.Drawing.Point(20, 53);
            this.comboBoxLanguage.Name = "comboBoxLanguage";
            this.comboBoxLanguage.Size = new System.Drawing.Size(256, 21);
            this.comboBoxLanguage.TabIndex = 2;
            this.comboBoxLanguage.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // labelLanguage
            // 
            this.labelLanguage.AutoSize = true;
            this.labelLanguage.Location = new System.Drawing.Point(8, 37);
            this.labelLanguage.Name = "labelLanguage";
            this.labelLanguage.Size = new System.Drawing.Size(58, 13);
            this.labelLanguage.TabIndex = 1;
            this.labelLanguage.Text = "Language:";
            // 
            // checkBoxShowToday
            // 
            this.checkBoxShowToday.AutoSize = true;
            this.checkBoxShowToday.Location = new System.Drawing.Point(11, 6);
            this.checkBoxShowToday.Name = "checkBoxShowToday";
            this.checkBoxShowToday.Size = new System.Drawing.Size(133, 17);
            this.checkBoxShowToday.TabIndex = 0;
            this.checkBoxShowToday.Text = "Show Today at startup";
            this.checkBoxShowToday.UseVisualStyleBackColor = true;
            this.checkBoxShowToday.CheckedChanged += new System.EventHandler(this.checkBoxShowToday_CheckedChanged);
            // 
            // tabPageColors
            // 
            this.tabPageColors.Controls.Add(this.groupBoxHeader);
            this.tabPageColors.Controls.Add(this.groupBoxRows);
            this.tabPageColors.Controls.Add(this.buttonSetDef);
            this.tabPageColors.Location = new System.Drawing.Point(4, 22);
            this.tabPageColors.Name = "tabPageColors";
            this.tabPageColors.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageColors.Size = new System.Drawing.Size(284, 243);
            this.tabPageColors.TabIndex = 1;
            this.tabPageColors.Text = "Colors";
            this.tabPageColors.UseVisualStyleBackColor = true;
            // 
            // groupBoxHeader
            // 
            this.groupBoxHeader.Location = new System.Drawing.Point(6, 6);
            this.groupBoxHeader.Name = "groupBoxHeader";
            this.groupBoxHeader.Size = new System.Drawing.Size(268, 99);
            this.groupBoxHeader.TabIndex = 10;
            this.groupBoxHeader.TabStop = false;
            this.groupBoxHeader.Text = "Header";
            // 
            // groupBoxRows
            // 
            this.groupBoxRows.Controls.Add(this.buttonRFont);
            this.groupBoxRows.Controls.Add(this.labelRowFont);
            this.groupBoxRows.Controls.Add(this.buttonHLRows);
            this.groupBoxRows.Controls.Add(this.labelRowHL);
            this.groupBoxRows.Controls.Add(this.labelRowNormal);
            this.groupBoxRows.Controls.Add(this.buttonColorCells);
            this.groupBoxRows.Location = new System.Drawing.Point(6, 111);
            this.groupBoxRows.Name = "groupBoxRows";
            this.groupBoxRows.Size = new System.Drawing.Size(268, 97);
            this.groupBoxRows.TabIndex = 9;
            this.groupBoxRows.TabStop = false;
            this.groupBoxRows.Text = "Rows";
            // 
            // buttonRFont
            // 
            this.buttonRFont.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonRFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRFont.Location = new System.Drawing.Point(123, 69);
            this.buttonRFont.Name = "buttonRFont";
            this.buttonRFont.Size = new System.Drawing.Size(139, 23);
            this.buttonRFont.TabIndex = 14;
            this.buttonRFont.UseVisualStyleBackColor = true;
            this.buttonRFont.Click += new System.EventHandler(this.buttonRFont_Click);
            // 
            // labelRowFont
            // 
            this.labelRowFont.AutoSize = true;
            this.labelRowFont.Location = new System.Drawing.Point(16, 74);
            this.labelRowFont.Name = "labelRowFont";
            this.labelRowFont.Size = new System.Drawing.Size(31, 13);
            this.labelRowFont.TabIndex = 13;
            this.labelRowFont.Text = "Font:";
            // 
            // buttonHLRows
            // 
            this.buttonHLRows.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonHLRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHLRows.Location = new System.Drawing.Point(123, 40);
            this.buttonHLRows.Name = "buttonHLRows";
            this.buttonHLRows.Size = new System.Drawing.Size(139, 23);
            this.buttonHLRows.TabIndex = 11;
            this.buttonHLRows.UseVisualStyleBackColor = true;
            this.buttonHLRows.BackColorChanged += new System.EventHandler(this.buttonHLRows_BackColorChanged);
            this.buttonHLRows.Click += new System.EventHandler(this.buttonHLRows_Click);
            // 
            // labelRowHL
            // 
            this.labelRowHL.AutoSize = true;
            this.labelRowHL.Location = new System.Drawing.Point(16, 45);
            this.labelRowHL.Name = "labelRowHL";
            this.labelRowHL.Size = new System.Drawing.Size(63, 13);
            this.labelRowHL.TabIndex = 10;
            this.labelRowHL.Text = "Highlighted:";
            // 
            // labelRowNormal
            // 
            this.labelRowNormal.AutoSize = true;
            this.labelRowNormal.Location = new System.Drawing.Point(16, 16);
            this.labelRowNormal.Name = "labelRowNormal";
            this.labelRowNormal.Size = new System.Drawing.Size(43, 13);
            this.labelRowNormal.TabIndex = 9;
            this.labelRowNormal.Text = "Normal:";
            // 
            // buttonColorCells
            // 
            this.buttonColorCells.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.buttonColorCells.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonColorCells.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonColorCells.Location = new System.Drawing.Point(123, 11);
            this.buttonColorCells.Name = "buttonColorCells";
            this.buttonColorCells.Size = new System.Drawing.Size(139, 23);
            this.buttonColorCells.TabIndex = 12;
            this.buttonColorCells.UseVisualStyleBackColor = false;
            this.buttonColorCells.BackColorChanged += new System.EventHandler(this.buttonColorCells_BackColorChanged);
            this.buttonColorCells.Click += new System.EventHandler(this.buttonColorCells_Click);
            // 
            // buttonSetDef
            // 
            this.buttonSetDef.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSetDef.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetDef.Location = new System.Drawing.Point(3, 214);
            this.buttonSetDef.Name = "buttonSetDef";
            this.buttonSetDef.Size = new System.Drawing.Size(275, 23);
            this.buttonSetDef.TabIndex = 2;
            this.buttonSetDef.Text = "Set default settings";
            this.buttonSetDef.UseVisualStyleBackColor = true;
            this.buttonSetDef.Click += new System.EventHandler(this.buttonSetDef_Click);
            // 
            // labelDefaultAccount
            // 
            this.labelDefaultAccount.AutoSize = true;
            this.labelDefaultAccount.Location = new System.Drawing.Point(8, 77);
            this.labelDefaultAccount.Name = "labelDefaultAccount";
            this.labelDefaultAccount.Size = new System.Drawing.Size(86, 13);
            this.labelDefaultAccount.TabIndex = 5;
            this.labelDefaultAccount.Text = "Default account:";
            // 
            // comboBoxDefaultAccount
            // 
            this.comboBoxDefaultAccount.FormattingEnabled = true;
            this.comboBoxDefaultAccount.Location = new System.Drawing.Point(20, 93);
            this.comboBoxDefaultAccount.Name = "comboBoxDefaultAccount";
            this.comboBoxDefaultAccount.Size = new System.Drawing.Size(256, 21);
            this.comboBoxDefaultAccount.TabIndex = 6;
            // 
            // Options
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(292, 324);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.panelBottom.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageColors.ResumeLayout(false);
            this.groupBoxRows.ResumeLayout(false);
            this.groupBoxRows.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.CheckBox checkBoxShowToday;
        private System.Windows.Forms.TabPage tabPageColors;
        private System.Windows.Forms.Button buttonSetDef;
        private System.Windows.Forms.GroupBox groupBoxHeader;
        private System.Windows.Forms.GroupBox groupBoxRows;
        private System.Windows.Forms.Label labelRowFont;
        private System.Windows.Forms.Button buttonHLRows;
        private System.Windows.Forms.Label labelRowHL;
        private System.Windows.Forms.Label labelRowNormal;
        private System.Windows.Forms.Button buttonColorCells;
        private System.Windows.Forms.Button buttonRFont;
        private System.Windows.Forms.ComboBox comboBoxLanguage;
        private System.Windows.Forms.Label labelLanguage;
        private System.Windows.Forms.Button buttonAssociatePFA;
        private System.Windows.Forms.CheckBox checkBoxHideInTaskbar;
        private System.Windows.Forms.ComboBox comboBoxDefaultAccount;
        private System.Windows.Forms.Label labelDefaultAccount;
    }
}