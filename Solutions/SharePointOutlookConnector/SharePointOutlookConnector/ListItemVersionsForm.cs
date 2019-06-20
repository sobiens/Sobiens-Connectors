using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Diagnostics;
using Sobiens.Office.SharePointOutlookConnector.BLL;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public partial class ListItemVersionsForm : Form
    {
        public ListItemVersionsForm()
        {
            InitializeComponent();
        }
        private void LoadVersions(List<EUListItemVersion> versions)
        {
            ListItemVersionsDataGridView.Rows.Clear();
            foreach (EUListItemVersion version in versions)
            {
                int rowIndex = ListItemVersionsDataGridView.Rows.Add();
                DataGridViewRow row = ListItemVersionsDataGridView.Rows[rowIndex];
                row.Cells["URLColumn"].Value = version.URL;
                row.Cells["VersionColumn"].Value = version.Version;
                row.Cells["SizeColumn"].Value = version.Size;
                row.Cells["CreatedColumn"].Value = version.Created;
                row.Cells["CreatedByColumn"].Value = version.CreatedBy;
                row.Cells["CommentsColumn"].Value = version.Comments;
                row.Tag = version;
            }
        }
        public void Initialize(EUSiteSetting siteSetting, string webURL, string fileURL)
        {
            List<EUListItemVersion> versions = SharePointManager.GetListItemVersions(siteSetting, webURL, fileURL);
            LoadVersions(versions);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItemVersion version = (EUListItemVersion)ListItemVersionsDataGridView.SelectedRows[0].Tag;
            Process.Start("IExplore.exe", version.URL);
        }

        private void rollbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItemVersion version = (EUListItemVersion)ListItemVersionsDataGridView.SelectedRows[0].Tag;
            List<EUListItemVersion> versions = SharePointManager.RestoreVersion(version.SiteSetting, version.WebURL, version.URL, version.Version);
            LoadVersions(versions);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (ListItemVersionsDataGridView.SelectedRows.Count == 0)
                contextMenuStrip1.Enabled = false;
            else
            {
                contextMenuStrip1.Enabled = true;
                EUListItemVersion version = (EUListItemVersion)ListItemVersionsDataGridView.SelectedRows[0].Tag;
                if (version.Version.IndexOf("@") > -1)
                {
                    rollbackToolStripMenuItem.Enabled = false;
                }
                else
                    rollbackToolStripMenuItem.Enabled = true;
            }
        }
    }
}
