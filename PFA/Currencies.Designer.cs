using EvgTar.PFA.Properties;

namespace EvgTar.PFA
{
    partial class CurrenciesForm
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("US Dollars");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("EURO");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("����������� �����");
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.labelCC = new System.Windows.Forms.Label();
            this.textBoxCRate = new System.Windows.Forms.TextBox();
            this.label1BC = new System.Windows.Forms.Label();
            this.labelCRate = new System.Windows.Forms.Label();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonNew = new System.Windows.Forms.Button();
            this.textBoxCSymbol = new System.Windows.Forms.TextBox();
            this.labelCSymbol = new System.Windows.Forms.Label();
            this.textBoxCName = new System.Windows.Forms.TextBox();
            this.labelCName = new System.Windows.Forms.Label();
            this.textBoxCCode = new System.Windows.Forms.TextBox();
            this.labelCCode = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
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
            this.splitContainer1.Panel2.Controls.Add(this.labelCC);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxCRate);
            this.splitContainer1.Panel2.Controls.Add(this.label1BC);
            this.splitContainer1.Panel2.Controls.Add(this.labelCRate);
            this.splitContainer1.Panel2.Controls.Add(this.buttonDelete);
            this.splitContainer1.Panel2.Controls.Add(this.buttonSave);
            this.splitContainer1.Panel2.Controls.Add(this.buttonNew);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxCSymbol);
            this.splitContainer1.Panel2.Controls.Add(this.labelCSymbol);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxCName);
            this.splitContainer1.Panel2.Controls.Add(this.labelCName);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxCCode);
            this.splitContainer1.Panel2.Controls.Add(this.labelCCode);
            this.splitContainer1.Panel2.Controls.Add(this.buttonClose);
            this.splitContainer1.Size = new System.Drawing.Size(494, 192);
            this.splitContainer1.SplitterDistance = 139;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node0";
            treeNode1.Tag = "USD";
            treeNode1.Text = "US Dollars";
            treeNode2.Name = "Node1";
            treeNode2.Tag = "EUR";
            treeNode2.Text = "EURO";
            treeNode3.Name = "Node2";
            treeNode3.Tag = "BYR";
            treeNode3.Text = "����������� �����";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(139, 192);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // labelCC
            // 
            this.labelCC.AutoSize = true;
            this.labelCC.Location = new System.Drawing.Point(150, 114);
            this.labelCC.Name = "labelCC";
            this.labelCC.Size = new System.Drawing.Size(30, 13);
            this.labelCC.TabIndex = 14;
            this.labelCC.Text = "EUR";
            // 
            // textBoxCRate
            // 
            this.textBoxCRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCRate.Location = new System.Drawing.Point(94, 111);
            this.textBoxCRate.Name = "textBoxCRate";
            this.textBoxCRate.Size = new System.Drawing.Size(50, 20);
            this.textBoxCRate.TabIndex = 13;
            this.textBoxCRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxCRate.TextChanged += new System.EventHandler(this.textBoxCRate_TextChanged);
            // 
            // label1BC
            // 
            this.label1BC.AutoSize = true;
            this.label1BC.Location = new System.Drawing.Point(37, 114);
            this.label1BC.Name = "label1BC";
            this.label1BC.Size = new System.Drawing.Size(51, 13);
            this.label1BC.TabIndex = 12;
            this.label1BC.Text = "1 EUR = ";
            // 
            // labelCRate
            // 
            this.labelCRate.AutoSize = true;
            this.labelCRate.Location = new System.Drawing.Point(12, 92);
            this.labelCRate.Name = "labelCRate";
            this.labelCRate.Size = new System.Drawing.Size(150, 13);
            this.labelCRate.TabIndex = 11;
            this.labelCRate.Text = "Current rate for base currency:";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDelete.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDelete.Image = Resources.remove;
            this.buttonDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDelete.Location = new System.Drawing.Point(88, 139);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 50);
            this.buttonDelete.TabIndex = 10;
            this.buttonDelete.TabStop = false;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Enabled = false;
            this.buttonSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Image = Resources.save;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(192, 139);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 50);
            this.buttonSave.TabIndex = 9;
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
            this.buttonNew.Location = new System.Drawing.Point(7, 139);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(75, 50);
            this.buttonNew.TabIndex = 8;
            this.buttonNew.Text = "New";
            this.buttonNew.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // textBoxCSymbol
            // 
            this.textBoxCSymbol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCSymbol.Location = new System.Drawing.Point(105, 62);
            this.textBoxCSymbol.MaxLength = 3;
            this.textBoxCSymbol.Name = "textBoxCSymbol";
            this.textBoxCSymbol.Size = new System.Drawing.Size(50, 20);
            this.textBoxCSymbol.TabIndex = 6;
            this.textBoxCSymbol.TextChanged += new System.EventHandler(this.textBoxCSymbol_TextChanged);
            // 
            // labelCSymbol
            // 
            this.labelCSymbol.AutoSize = true;
            this.labelCSymbol.Location = new System.Drawing.Point(12, 65);
            this.labelCSymbol.Name = "labelCSymbol";
            this.labelCSymbol.Size = new System.Drawing.Size(87, 13);
            this.labelCSymbol.TabIndex = 5;
            this.labelCSymbol.Text = "Currency symbol:";
            // 
            // textBoxCName
            // 
            this.textBoxCName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCName.Location = new System.Drawing.Point(105, 36);
            this.textBoxCName.Name = "textBoxCName";
            this.textBoxCName.Size = new System.Drawing.Size(237, 20);
            this.textBoxCName.TabIndex = 4;
            this.textBoxCName.TextChanged += new System.EventHandler(this.textBoxCName_TextChanged);
            // 
            // labelCName
            // 
            this.labelCName.AutoSize = true;
            this.labelCName.Location = new System.Drawing.Point(12, 39);
            this.labelCName.Name = "labelCName";
            this.labelCName.Size = new System.Drawing.Size(81, 13);
            this.labelCName.TabIndex = 3;
            this.labelCName.Text = "Currency name:";
            // 
            // textBoxCCode
            // 
            this.textBoxCCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCCode.Location = new System.Drawing.Point(105, 10);
            this.textBoxCCode.MaxLength = 3;
            this.textBoxCCode.Name = "textBoxCCode";
            this.textBoxCCode.Size = new System.Drawing.Size(50, 20);
            this.textBoxCCode.TabIndex = 2;
            this.textBoxCCode.Leave += new System.EventHandler(this.textBoxCCode_Leave);
            this.textBoxCCode.TextChanged += new System.EventHandler(this.textBoxCCode_TextChanged);
            // 
            // labelCCode
            // 
            this.labelCCode.AutoSize = true;
            this.labelCCode.Location = new System.Drawing.Point(12, 13);
            this.labelCCode.Name = "labelCCode";
            this.labelCCode.Size = new System.Drawing.Size(79, 13);
            this.labelCCode.TabIndex = 1;
            this.labelCCode.Text = "Currency code:";
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Image = Resources.close;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(273, 139);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 50);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // CurrenciesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(494, 192);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CurrenciesForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Currencies";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxCSymbol;
        private System.Windows.Forms.Label labelCSymbol;
        private System.Windows.Forms.TextBox textBoxCName;
        private System.Windows.Forms.Label labelCName;
        private System.Windows.Forms.TextBox textBoxCCode;
        private System.Windows.Forms.Label labelCCode;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.TextBox textBoxCRate;
        private System.Windows.Forms.Label label1BC;
        private System.Windows.Forms.Label labelCRate;
        private System.Windows.Forms.Label labelCC;
    }
}