using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EvgTar.PFA
{
    public partial class CategoriesForm : Form
    {
        private DataSet ds = new DataSet();
        string nodeText = "Category";
        public Languages NLS = null;
        public bool retVal = false;
        public string category_id = "";
        public Core.Core pfa_core;

        public CategoriesForm()
        {
            InitializeComponent();
            labelCGroupV.Text = "";
        }
        public bool Init()
        {
            this.Text = NLS.LNGetString("TitleCategories", "Categories");
            buttonClose.Text = NLS.LNGetString("TextClose", "Close");
            buttonSave.Text = NLS.LNGetString("TextSave", "Save");
            buttonNew.Text = NLS.LNGetString("TextAdd", "Add");
            buttonDelete.Text = NLS.LNGetString("TextDelete", "Delete");
            labelCGroup.Text = NLS.LNGetString("TextCategoryGroup", "Category group:");
            labelCName.Text = NLS.LNGetString("TextCategoryName", "Category name:");
            checkBoxActive.Text = NLS.LNGetString("TextActive", "Active");
            nodeText = NLS.LNGetString("TextCategory", "Category");

            if (pfa_core == null)
                return false;

            int sel_node = 0;
            if (treeView1.SelectedNode != null)
                sel_node = (treeView1.SelectedNode.Level == 0) ? treeView1.SelectedNode.Index : treeView1.SelectedNode.Parent.Index;
            if (pfa_core.CategoriesTreeGet(ref ds))
            {
                treeView1.Nodes.Clear();
                DataRow[] pcat = ds.Tables["FCCategories"].Select("pcategory_id = 0", "category_name");
                for (int i = 0; i < pcat.Length; i++)
                {
                    TreeNode node = new TreeNode(pcat[i]["category_name"].ToString());
                    node.Tag = pcat[i]["category_id"];
                    DataRow[] cats = ds.Tables["FCCategories"].Select("pcategory_id = " + pcat[i]["category_id"].ToString(), "category_name");
                    for (int c = 0; c < cats.Length; c++)
                    {
                        TreeNode cnode = new TreeNode(cats[c]["category_name"].ToString());
                        cnode.Tag = cats[c]["category_id"];
                        node.Nodes.Add(cnode);
                    }
                    treeView1.Nodes.Add(node);
                }
            }
            else
                return false;
            
            try
            {
                treeView1.SelectedNode = treeView1.Nodes[sel_node];
            }
            catch { }
            return true;
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (textBoxCName.Text.Trim().Equals(""))
            {
                textBoxCName.BackColor = Color.Red;
                textBoxCName.Select();
                return;
            }

            if (treeView1.SelectedNode == null)
            {
                string tmp = textBoxCName.Text.Trim();
                treeView1.SelectedNode = treeView1.Nodes.Add(tmp);
                textBoxCName.Text = tmp;
            }


            if (pfa_core.CategorySave(
                (treeView1.SelectedNode.Tag == null) ? -1 : (long)treeView1.SelectedNode.Tag,
                (treeView1.SelectedNode.Level == 0) ? 0 : (long)treeView1.SelectedNode.Parent.Tag,
                textBoxCName.Text.Trim(),
                (checkBoxActive.Checked ? 1 : 0)
                )
            )
            {
                buttonSave.Enabled = false;
                treeView1.SelectedNode.Text = textBoxCName.Text.Trim();
                Init();
            }
            treeView1.Select();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            labelCGroupV.Text = "";
            textBoxCName.Text = "";            
            if (treeView1.SelectedNode.Tag != null)
            {
                treeView1.SelectedNode.Expand();
                if (treeView1.SelectedNode.Level == 1)
                    labelCGroupV.Text = treeView1.SelectedNode.Parent.Text;
                textBoxCName.Text = treeView1.SelectedNode.Text;
                DataRow[] dr = ds.Tables["FCCategories"].Select("category_id = " + treeView1.SelectedNode.Tag.ToString());
                if (dr.Length > 0)
                    checkBoxActive.Checked = dr[0]["category_status"].ToString().Equals("1");
            }
            buttonSave.Enabled = false;
            buttonDelete.Enabled = true;
        }        
        private void buttonNew_Click(object sender, EventArgs e)
        {
            if (buttonSave.Enabled)
            {
                if (MessageBox.Show(NLS.LNGetString("TextCategory", "Category") + " \"" + treeView1.SelectedNode.Text + "\" " + NLS.LNGetString("QuestionNotSaved1", "not saved. Save?"), NLS.LNGetString("TitleCategorySave", "Category save"), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    buttonSave_Click(sender, e);
                else
                {
                    if (treeView1.SelectedNode.Tag == null)
                        treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }            
            if (treeView1.SelectedNode != null)
            {
                if (treeView1.SelectedNode.Level == 1)
                {
                    labelCGroupV.Text = treeView1.SelectedNode.Parent.Text;
                    treeView1.SelectedNode = treeView1.SelectedNode.Parent.Nodes.Add(nodeText);
                }
                else
                {
                    if (treeView1.SelectedNode.Nodes.Count == 0)
                        treeView1.SelectedNode = treeView1.SelectedNode.Nodes.Add(nodeText);
                    else
                        treeView1.SelectedNode = treeView1.Nodes.Add(nodeText);
                }
            }
            else
                treeView1.SelectedNode = treeView1.Nodes.Add(nodeText);
            textBoxCName.Text = nodeText;
            textBoxCName.Enabled = true;
            textBoxCName.Select();
        }
        private void textBoxCName_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void checkBoxActive_CheckedChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (pfa_core == null)
                return;
            if (treeView1.SelectedNode.Tag != null)
            {
                
                if (pfa_core.GetCount("Transactions", "where category_id = " + treeView1.SelectedNode.Tag.ToString()) > 0)
                    MessageBox.Show(NLS.LNGetString("TextAccount", "There's at least one transaction for") + " " + treeView1.SelectedNode.Text + NLS.LNGetString("TitleAccountDelete2", " category. First remove transactions."));
                else
                {
                    if (pfa_core.CategoryDelete((long)treeView1.SelectedNode.Tag))
                        Init();
                }
            }
        }
        private void textBoxCName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar == 13)
                buttonSave_Click(sender, null);
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if (!retVal)
                return;
            if ((treeView1.SelectedNode == null) || (treeView1.SelectedNode.Level == 0))
                return;
            category_id = treeView1.SelectedNode.Tag.ToString();
            this.DialogResult = DialogResult.OK;
        }        
    }
}