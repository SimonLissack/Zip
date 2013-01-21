/* Directory zipper
 * Simon Lissack 2012
 * 
 * Loops through a directory and adds selected sub directories to into corresponding zip files
 * 
 * Uses https://github.com/icsharpcode/SharpZipLib
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;


namespace Zip
{
    // This delegate allows for communication between FrmMain and FrmFilter forms
    public delegate void AddItemDelegate(string filter);

    public partial class FrmMain : Form
    {
        string path;
        // Stores the filter options of which file types get zipped
        // If filter == "" then all files will be zipped
        string filter = "";
        FrmFilter frmFilter = new FrmFilter();

        public FrmMain()
        {
            InitializeComponent();
            // Sets up UpdateFilter to listen for the AddFilterOption delegate
            frmFilter.AddFilterOption = new AddItemDelegate(this.UpdateFilter);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
        }

        /*
         * Browse to target directory
         */
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.ShowDialog();

            path = fbd.SelectedPath;
            lblDirectoryName.Text = path;

            if (path != "")
                GetFolders();
        }

        private void GetFolders()
        {
            string[] folders = Directory.GetDirectories(@path);

            lstFolders.Items.Clear();

            for (int i = 0; i < folders.Length; i++)
            {
                lstFolders.Items.Add(folders[i]);
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            //int n = lstFolders.SelectedIndices.Count;
            //MessageBox.Show(n.ToString());

            //List<String> selectedFolders = GetSelectedFolders();
            //for (int i = 0; i < selectedFolders.Count; i++)
            //    MessageBox.Show(@selectedFolders[i] + ".zip");

            //System.Media.SystemSounds.Asterisk.Play();

            //FrmFilter frmFilter = new FrmFilter();
            //// Subscribe
            //frmFilter.AddFilterOption = new AddItemDelagate(this.UpdateFilter);
            //frmFilter.Show();         
        }

        private void UpdateFilter(string filter)
        {
            this.filter = filter;
        }

        private void btnZip_Click(object sender, EventArgs e)
        {
            List<String> selectedFolders = GetSelectedFolders();

            // Include all files in the directories
            bool recurse = true; 
            FastZip fz = new FastZip();

            pgbProgress.Value = 0;
            pgbProgress.Maximum = selectedFolders.Count;
            try
            {
                for (int i = 0; i < selectedFolders.Count; i++)
                {
                    // Creates a zip file as <folderName>.zip
                    fz.CreateZip(@selectedFolders[i] + ".zip", @selectedFolders[i], recurse, filter);
                    pgbProgress.Value++;
                }
                System.Media.SystemSounds.Asterisk.Play();
            }
            catch (Exception exception)
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("Error Occurred: " + exception);
            }
        }

        /*
         * Adds all the sub-folders that have been selected via lstFolders to a list.
         */
        private List<String> GetSelectedFolders()
        {
            List<String> selectedFolders = new List<string>();

            for (int i = 0; i < lstFolders.Items.Count; i++)
            {
                for (int j = 0; j < lstFolders.SelectedIndices.Count; j++)
                {
                    if (lstFolders.Items[i] == lstFolders.SelectedItems[j])
                    {
                        selectedFolders.Add(lstFolders.SelectedItems[j].ToString());
                    }
                }
            }

            return selectedFolders;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            frmFilter.CheckSelectedItems();
            frmFilter.Show();    
        }

    }
}
