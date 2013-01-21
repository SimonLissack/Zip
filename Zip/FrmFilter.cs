using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Zip
{
    public partial class FrmFilter : Form
    {
        public AddItemDelegate AddFilterOption;

        RegistryKey rk = Registry.ClassesRoot;
        List<string> allExtensions = new List<string>();
        List<string> lastSelectedKeys = new List<string>();
        List<string> selectedExtensions = new List<string>();

        public FrmFilter()
        {
            InitializeComponent();
        }

        private void frmFilter_Load(object sender, EventArgs e)
        {
            string[] keys = rk.GetSubKeyNames();

            clbExtenstions.Items.Clear();

            // Gets all registry keys starting with ".", which will be file extension names
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].IndexOf('.') == 0)
                    allExtensions.Add(keys[i]);
            }
            SetList();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Passes the filtered string back to FrmMain
            AddFilterOption(@ParseFilter());
            this.Hide();
            lastSelectedKeys = selectedExtensions;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            selectedExtensions = lastSelectedKeys;
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

        }

        /*
         * Creates a filter that is read by the FastZip.CreateZip method
         */
        private string ParseFilter()
        {
            string str = "";

            for (int i = 0; i < selectedExtensions.Count; i++)
            {
                str += @"\" + selectedExtensions[i] + @"$;";
            }
            return str;
        }

        private void SetList()
        {
            clbExtenstions.Items.Clear();
            for (int i = 0; i < allExtensions.Count; i++)
            {
                clbExtenstions.Items.Add(allExtensions[i]);
            }
            CheckSelectedItems();
        }

        /*
         * Searches while the user types 
         */
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            List<String> tempList = new List<string>();
            if (txtSearch.Text == "")
                SetList();
            else
            {
                clbExtenstions.Items.Clear();
                SetList();

                for (int i = 0; i < clbExtenstions.Items.Count; i++)
                {
                    tempList.Add(clbExtenstions.Items[i].ToString());
                    if (tempList[tempList.Count - 1].Contains(txtSearch.Text) == false)
                        tempList.RemoveAt(tempList.Count - 1);
                }
                clbExtenstions.Items.Clear();
                for (int i = 0; i < tempList.Count; i++)
                    clbExtenstions.Items.Add(tempList[i]);
            }
            CheckSelectedItems();
        }

        private void clbExtenstions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (clbExtenstions.GetItemChecked(clbExtenstions.SelectedIndex))
            {
                selectedExtensions.Add(clbExtenstions.SelectedItem.ToString());
            }
        }

        public void CheckSelectedItems()
        {
            for (int i = 0; i < clbExtenstions.Items.Count; i++)
                for (int j = 0; j < selectedExtensions.Count; j++)
                    if (clbExtenstions.Items[i].ToString() == selectedExtensions[j])
                        clbExtenstions.SetItemCheckState(i, CheckState.Checked);
        }
        
    }
}
