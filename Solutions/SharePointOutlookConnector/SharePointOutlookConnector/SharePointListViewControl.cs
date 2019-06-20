using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL;
using System.IO;
using EmailUploader.BLL;
using System.Diagnostics;
using System.Collections;
using System.Xml;
using Outlook = Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Outlook;
using System.Collections.Specialized;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities.FileSystem;
using Sobiens.Office.SharePointOutlookConnector.Forms;
using EmailUploader.BLL.Entities;

namespace Sobiens.Office.SharePointOutlookConnector
{
    public delegate void UpdateContentDataGridViewHandler(ISPCView selectedView, List<ISPCItem> items, string listItemCollectionPositionNext, int itemCount);
    public delegate void UpdateViewsHandler(ISPCFolder folder);
    public delegate void UploadEmailHandler(SharePointListViewControl listviewControl, ISPCFolder selectedFolder, DragEventArgs e, List<EUEmailUploadFile> emailItems, bool isListItemAndAttachmentMode);
    public delegate void HideLoadingPictureBoxHandler();
    public delegate void NotifyUploadItemHandler(Guid guid, EUListItem listItem);
    public delegate void DeleteUploadItemHandler(Guid guid);


    public partial class SharePointListViewControl : UserControl
    {
        public SharePointListViewControl()
        {
            InitializeComponent();
        }
        private DataObject myData = new DataObject();
        public event System.EventHandler SelectionChanged;
        public event System.EventHandler EditListItemSelected;
        private ISPCItem CopiedListItem = null;
        public ISPCFolder SelectedFolder = null;
        private ISPCView SelectedView = null;
        private string SortedFieldName = String.Empty;
        private bool IsAsc = true;
        private int CurrentPageIndex = 0;
        private int CurrentPageItemCount = 0;
        private NameValueCollection ListItemCollectionPositionNexts = new NameValueCollection();
        public Outlook.Application Application = null;
        private string BackgroundWorkerAction = String.Empty;
        public EUCamlFilters CustomFilters = new EUCamlFilters();

        public void Initialize(Outlook.Application _Application)
        {
            Application = _Application;
        }

        public void UpdateContentDataGridView(ISPCView selectedView, List<ISPCItem> items, string listItemCollectionPositionNext, int itemCount)
        {
            NextButton.Enabled = false;
            PreviousButton.Enabled = false;

            LoadItems(selectedView, items, listItemCollectionPositionNext, itemCount);
            LoadingPictureBox.Visible = false;
        }

        public void LoadViews(ISPCFolder folder)
        {
            IOutlookConnector connector = OutlookConnector.GetConnector(folder.SiteSetting);
            List<ISPCView> views = connector.GetViews(folder);
            ViewsComboBox.Items.Clear();
            foreach (ISPCView view in views)
            {
                ViewsComboBox.Items.Add(view);
            }
            ViewsComboBox.SelectedIndex = 0;
        }
        public void LoadViewsExt(string webUrl, string listName)
        {
            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "LoadViews";
            ChangeViewBackgroundWorker.RunWorkerAsync(new string[2]{webUrl, listName});
        }

        public void SelectFolder(ISPCFolder folder)
        {
            if (folder == null || folder.ContainsItems == false)
                return;
            SelectedFolder = folder;
            CustomFilters = new EUCamlFilters();
            LoadViews(folder);
        }
        public List<ISPCItem> GetViewItems(ISPCView view, string sortField, bool isAsc, int pageIndex, EUCamlFilters filters, out string listItemCollectionPositionNext, out int itemCount)
        {
            string currentListItemCollectionPositionNext = ListItemCollectionPositionNexts[(pageIndex).ToString()];
            IOutlookConnector connector = OutlookConnector.GetConnector(view.SiteSetting);
            return connector.GetListItems(SelectedFolder, view, sortField, isAsc, CurrentPageIndex, currentListItemCollectionPositionNext, filters, out listItemCollectionPositionNext, out itemCount);
        }


        public void LoadItems(ISPCView view, List<ISPCItem> items, string listItemCollectionPositionNext, int itemCount)
        {
            if (listItemCollectionPositionNext != String.Empty)
            {
                NextButton.Enabled = true;
                if (ListItemCollectionPositionNexts[(CurrentPageIndex + 1).ToString()] == null)
                    ListItemCollectionPositionNexts.Add((CurrentPageIndex + 1).ToString(), listItemCollectionPositionNext);
            }
            else
            {
                NextButton.Enabled = false;
            }
            if (CurrentPageIndex > 0)
                PreviousButton.Enabled = true;
            int startIndex = CurrentPageIndex * view.RowLimit + 1;
            int endIndex = startIndex + itemCount-1;
            if (endIndex == 0)
                startIndex = 0;
            PagingLabel.Text = startIndex.ToString() + " - " + endIndex.ToString();

            OutlookConnector.GetConnector(view.SiteSetting).BindItemsToListViewControl(SelectedFolder, view, items, LibraryContentDataGridView);
        }
        public void DeleteUploadItemInvoke(Guid guid)
        {
            object[] args = new object[] { guid};
            this.Invoke(new DeleteUploadItemHandler(DeleteUploadItem), args);
        }

        public void DeleteUploadItem(Guid guid)
        {
            for (int i = LibraryContentDataGridView.Rows.Count - 1; i > -1;i-- )
            {
                if (LibraryContentDataGridView.Rows[i].Tag is string && LibraryContentDataGridView.Rows[i].Tag.ToString() == guid.ToString())
                {
                    LibraryContentDataGridView.Rows.RemoveAt(i);
                }
            }
        }

        public void NotifyUploadItemInvoke(Guid guid, EUListItem listItem)
        {
            object[] args = new object[]{guid, listItem} ;
            this.Invoke(new NotifyUploadItemHandler(NotifyUploadItem), args);
        }

        public void NotifyUploadItem(Guid guid, EUListItem listItem)
        {
            bool isDocumentLibrary = false;
            if (SelectedFolder is EUFolder)
                isDocumentLibrary = ((EUFolder)SelectedFolder).IsDocumentLibrary;
            else if (SelectedFolder is EUList)
                isDocumentLibrary = ((EUList)SelectedFolder).IsDocumentLibrary;
            foreach (DataGridViewRow row in LibraryContentDataGridView.Rows)
            {
                if (row.Tag is string && row.Tag .ToString() == guid.ToString())
                {
                    BLL.SharePoint.SharePointOutlookConnector.BindListItemToRow(listItem, row, isDocumentLibrary, SelectedView);    
                }
            }

            bool exist = false;
            for (int i=0;i<LibraryContentDataGridView.Rows.Count;i++)
            {
                if (LibraryContentDataGridView.Rows[i].Cells["TitleColumn"].Value.ToString() == listItem.Title)
                {
                    if (exist == true)
                    {
                        LibraryContentDataGridView.Rows.RemoveAt(i);
                        break;
                    }
                    exist = true;
                }
            }
        }

        private void LibraryContentDataGridView_DragDrop(object sender, DragEventArgs e)
        {
            if (SelectedFolder == null)
                return;
            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "UploadEmail";
            List<Outlook.MailItem> emailItems = new List<MailItem>();

            bool isListItemAndAttachmentMode = false;
            if (SelectedFolder as EUFolder != null && ((EUFolder)SelectedFolder).IsDocumentLibrary == false)
            {
                if (((EUFolder)SelectedFolder).EnableAttachments == false)
                {
                    MessageBox.Show("In order to upload email, you need to enable attachment feature in this list.");
                    return;
                }
                isListItemAndAttachmentMode = true;
            }

            // JON SILVER JUNE 2011
            // This is a possible drag/drop site

            if (e.Data.GetDataPresent("RenPrivateSourceFolder") == false)
            {
                emailItems.Add(Application.ActiveExplorer().Selection[1] as Outlook.MailItem);
            }
            else
            {
                for (int i = 0; i < Application.ActiveExplorer().Selection.Count; i++)
                {
                    Outlook.MailItem item = Application.ActiveExplorer().Selection[i + 1] as Outlook.MailItem;
                    emailItems.Add(item);
                }
            }
            List<EUEmailUploadFile> emailUploadFiles = CommonManager.GetEmailUploadFiles(emailItems, e, isListItemAndAttachmentMode, SaveFormatOverride.None); // JOEL JEFFERY 20110708 added SaveFormatOverride.None
            ChangeViewBackgroundWorker.RunWorkerAsync(new object[] { this, SelectedFolder, e, emailUploadFiles, isListItemAndAttachmentMode });
            //            EUEmailManager.UploadEmail(SelectedFolder, e, Application.ActiveExplorer().Selection);
        }

        private void LibraryContentDataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;

        }

        private void LibraryContentDataGridView_Resize(object sender, EventArgs e)
        {
//            NameColumn.Width = LibraryContentDataGridView.Width - 50;
        }

        private void LibraryContentDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (LibraryContentDataGridView.SelectedRows.Count != 1)
                return;
            if (LibraryContentDataGridView.SelectedRows[0].Tag == null)
                return;
            ISPCItem listItem = LibraryContentDataGridView.SelectedRows[0].Tag as ISPCItem;
            if (listItem == null)
                return;
            SelectionChanged(listItem, null);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISPCItem listItem = (ISPCItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            Process.Start("IExplore.exe", listItem.URL);
        }

        private void editPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItem listItem = (EUListItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            EUFolder folder = SelectedFolder as EUFolder;
            List<EUContentType> contentTypes = SharePointManager.GetContentTypes(folder.SiteSetting, folder.WebUrl, folder.ListName);
            EUFieldCollection fields = SharePointManager.GetFields(folder.SiteSetting, folder.WebUrl, folder.ListName);
            ListItemEditForm listItemEditForm = new ListItemEditForm();
            listItemEditForm.InitializeForm(folder, listItem);
            listItemEditForm.ShowDialog();
            if (listItemEditForm.DialogResult != DialogResult.OK)
            {
                return;
            }
            

            Hashtable changedProperties = new Hashtable();
            if(listItemEditForm.FieldInformations.ContentType != null)
                changedProperties.Add("ContentType", listItemEditForm.FieldInformations.ContentType.Name);
            for (int i = 0; i < listItemEditForm.FieldInformations.Count(); i++)
            {
                changedProperties.Add(listItemEditForm.FieldInformations[i].InternalName, listItemEditForm.FieldInformations[i].Value);
            }
            SharePointManager.UpdateListItem(listItem.SiteSetting, listItem.WebURL, listItem.ListName, listItem.ID, changedProperties);
            RefreshViewExt();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopiedListItem = (ISPCItem)LibraryContentDataGridView.SelectedRows[0].Tag;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileCopyNameForm fileCopyNameForm = new FileCopyNameForm();
            fileCopyNameForm.Initialize(CopiedListItem, SelectedFolder);
            fileCopyNameForm.ShowDialog();
            RefreshViewExt();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISPCItem listItem = (ISPCItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            DialogResult result = MessageBox.Show("Are you sure you would like to delete " + listItem.Title + "?","", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;
            OutlookConnector.GetConnector(listItem.SiteSetting).DeleteListItem(listItem);
            RefreshViewExt();
        }

        private void approveRejectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not Implemented!");
        }

        private void checkInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItem listItem = (EUListItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            CheckInForm checkInForm = new CheckInForm();
            checkInForm.SelectedItem = listItem;
            if (checkInForm.ShowDialog() == DialogResult.OK)
            {
                this.RefreshViewExt();
            }
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItem listItem = (EUListItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            SharePointManager.CheckOutFile(listItem.SiteSetting, listItem.WebURL, listItem.URL);
            this.RefreshViewExt();
        }

        private void versionHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItem listItem = (EUListItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            ListItemVersionsForm listItemVersionsForm = new ListItemVersionsForm();
            listItemVersionsForm.Text = "Version history for " + listItem.Title;
            listItemVersionsForm.Initialize(listItem.SiteSetting, listItem.WebURL, listItem.URL);
            listItemVersionsForm.ShowDialog();
        }

        private void ListItemContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            if (LibraryContentDataGridView.SelectedRows.Count != 1 || LibraryContentDataGridView.SelectedRows[0].Tag == null)
            {
                foreach (ToolStripItem toolStripItem in ListItemContextMenuStrip.Items)
                {
                    toolStripItem.Enabled = false;
                }

                pasteToolStripMenuItem.Enabled = (CopiedListItem == null ? false : true);
                return;
            }
            ISPCItem listItem = LibraryContentDataGridView.SelectedRows[0].Tag as ISPCItem;
            foreach (ToolStripItem toolStripItem in ListItemContextMenuStrip.Items)
            {
                toolStripItem.Enabled = true;
            }
            pasteToolStripMenuItem.Enabled = (CopiedListItem == null ? false : true);
            if (listItem == null)
            {
                attachToolStripMenuItem.Visible = false;
                versionHistoryToolStripMenuItem.Visible = false;
                checkOutToolStripMenuItem.Visible = false;
                checkInToolStripMenuItem.Visible = false;
                undoCheckOutToolStripMenuItem.Visible = false;
                approveRejectToolStripMenuItem.Visible = false;
                editPropertiesToolStripMenuItem.Visible = false;
            }
            else
            {
                if (GetCurrentInspactor() == null)
                {
                    attachToolStripMenuItem.Visible = false;
                }
                else
                {
                    attachToolStripMenuItem.Visible = true;
                    Outlook.MailItem emailItem = GetCurrentInspactor().CurrentItem as Outlook.MailItem;
                    attachToolStripMenuItem.Enabled = false;
                    if (emailItem != null)
                    {
                        attachToolStripMenuItem.Enabled = true;
                    }
                }
                EUFolder folder = SelectedFolder as EUFolder;
                if (folder != null)
                {
                    versionHistoryToolStripMenuItem.Visible = folder.EnableVersioning;
                }
                if (folder == null || folder.IsDocumentLibrary == false)
                {
                    checkOutToolStripMenuItem.Visible = false;
                    checkInToolStripMenuItem.Visible = false;
                    undoCheckOutToolStripMenuItem.Visible = false;
                }
                else
                {
                    if ((listItem as EUListItem).CheckoutUser == String.Empty)
                    {
                        checkOutToolStripMenuItem.Visible = true;
                        checkInToolStripMenuItem.Visible = false;
                        undoCheckOutToolStripMenuItem.Visible = false;
                    }
                    else
                    {
                        checkOutToolStripMenuItem.Visible = false;
                        checkInToolStripMenuItem.Visible = true;
                        undoCheckOutToolStripMenuItem.Visible = true;
                    }
                }
            }
        }

        private Inspector GetCurrentInspactor()
        {
            SharePointExplorerPane sharePointExplorerPane = this.Parent.Parent.Parent as SharePointExplorerPane;
            return sharePointExplorerPane.Inspector;
        }

        private void LibraryContentDataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (e.Button == MouseButtons.Left && LibraryContentDataGridView.SelectedRows.Count>0)
            {
                ISPCItem listItem = (ISPCItem)LibraryContentDataGridView.SelectedRows[0].Tag;
                FSItem fsItem = listItem as FSItem;
                object draggedObject = null;
                if (fsItem != null)
                {
                    myData.SetData(DataFormats.FileDrop, true, fsItem.URL);
//                    myData.SetData(DataFormats.Text, true, fsItem.URL);
                    draggedObject = myData;
                }
                else
                    draggedObject = (listItem as EUListItem).URL;
                
                this.DoDragDrop(myData, DragDropEffects.All);
            }
            */
        }

        private void asHyperlinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISPCItem listItem = LibraryContentDataGridView.SelectedRows[0].Tag as ISPCItem;
            Outlook.MailItem emailItem = GetCurrentInspactor().CurrentItem as Outlook.MailItem;
            emailItem.Body = listItem.URL + Environment.NewLine + emailItem.Body;
        }

        private void asAnAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ISPCItem listItem = (ISPCItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            Outlook.MailItem emailItem = GetCurrentInspactor().CurrentItem as Outlook.MailItem;
            emailItem.Attachments.Add(listItem.URL, OlAttachmentType.olByValue, 1, listItem.Title);
        }

        private void undoCheckOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EUListItem listItem = (EUListItem)LibraryContentDataGridView.SelectedRows[0].Tag;
            DialogResult result = MessageBox.Show("Are you sure you would like to undo " + listItem.Title + "?", "", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;
            SharePointManager.UndoCheckOutFile(listItem.SiteSetting, listItem.WebURL, listItem.URL);
            RefreshViewExt();
        }

        private void ChangeView(ISPCView selectedView)
        {
            SortedFieldName = String.Empty;
            IsAsc = true;
            CurrentPageIndex = 0;
            ListItemCollectionPositionNexts = new NameValueCollection();
            ListItemCollectionPositionNexts.Add("0", String.Empty);
            string listItemCollectionPositionNext = String.Empty;
            int itemCount;
            string currentListItemCollectionPositionNext = ListItemCollectionPositionNexts[(CurrentPageIndex).ToString()];

            List<ISPCItem> items = OutlookConnector.GetConnector(selectedView.SiteSetting).GetListItems(SelectedFolder, selectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, CustomFilters, out listItemCollectionPositionNext, out itemCount);

            EUView view = selectedView as EUView;
            if (view != null)
                SelectedView = SharePointManager.GetView(view.WebURL, view.ListName, view.Name, view.SiteSetting);
            else
                SelectedView = selectedView;

            object[] args = new object[4] { SelectedView, items, listItemCollectionPositionNext, itemCount };
            this.Invoke(new UpdateContentDataGridViewHandler(UpdateContentDataGridView),args);
        }
        private void ViewsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ISPCView selectedView = ViewsComboBox.SelectedItem as ISPCView;
            if (selectedView == null)
                return;
            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "ChangeView";
            if (ChangeViewBackgroundWorker.IsBusy == true)
            {
                MessageBox.Show("Background engine is busy right now, Please try again later.");
                return;
            }
            ChangeViewBackgroundWorker.RunWorkerAsync(selectedView);
        }

        private void SortContentGridView()
        {
            string listItemCollectionPositionNext = String.Empty;
            int itemCount;

            List<ISPCItem> items = GetViewItems(SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, CustomFilters, out listItemCollectionPositionNext, out itemCount);
            object[] args = new object[4] { SelectedView, items, listItemCollectionPositionNext, itemCount };
            this.Invoke(new UpdateContentDataGridViewHandler(UpdateContentDataGridView), args);
        }

        private void LibraryContentDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn selectedColumn = LibraryContentDataGridView.Columns[e.ColumnIndex];
            string fieldName = String.Empty;
            if (selectedColumn.Tag != null)
                fieldName = selectedColumn.Tag.ToString();
            if (SortedFieldName == fieldName)
            {
                if (IsAsc == false)
                    IsAsc = true;
                else
                    IsAsc = false;
            }
            else
            {
                IsAsc = true;
            }
            CurrentPageIndex = 0;
            SortedFieldName = fieldName;

            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "SortContentGridView";
            ChangeViewBackgroundWorker.RunWorkerAsync();
        }

        private void GoToNextPage()
        {
            CurrentPageIndex++;

            string listItemCollectionPositionNext = String.Empty;
            int itemCount;

            List<ISPCItem> items = GetViewItems(SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, CustomFilters, out listItemCollectionPositionNext, out itemCount);
            object[] args = new object[4] { SelectedView, items, listItemCollectionPositionNext, itemCount };
            this.Invoke(new UpdateContentDataGridViewHandler(UpdateContentDataGridView), args);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "GoToNextPage";
            ChangeViewBackgroundWorker.RunWorkerAsync();
        }

        private void GoToPreviousPage()
        {
            CurrentPageIndex--;
            string listItemCollectionPositionNext = String.Empty;
            int itemCount;

            List<ISPCItem> items = GetViewItems(SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, CustomFilters, out listItemCollectionPositionNext, out itemCount);
            object[] args = new object[4] { SelectedView, items, listItemCollectionPositionNext, itemCount };
            this.Invoke(new UpdateContentDataGridViewHandler(UpdateContentDataGridView), args);
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "GoToPreviousPage";
            ChangeViewBackgroundWorker.RunWorkerAsync();
        }

        public void RefreshView()
        {
            LoadingPictureBox.Visible = true;
            BackgroundWorkerAction = "RefreshView";
            ChangeViewBackgroundWorker.RunWorkerAsync();
        }

        public void RefreshViewExt()
        {
            string listItemCollectionPositionNext = String.Empty;
            int itemCount;

            List<ISPCItem> items = GetViewItems(SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, CustomFilters, out listItemCollectionPositionNext, out itemCount);
            object[] args = new object[4] { SelectedView, items, listItemCollectionPositionNext, itemCount };
            this.Invoke(new UpdateContentDataGridViewHandler(UpdateContentDataGridView), args);
        }

        private void ChangeViewBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BackgroundWorkerAction == "ChangeView")
            {
                ChangeView((ISPCView)e.Argument);
            }
            else if (BackgroundWorkerAction == "GoToPreviousPage")
            {
                GoToPreviousPage();
            }
            else if (BackgroundWorkerAction == "GoToNextPage")
            {
                GoToNextPage();
            }
            else if (BackgroundWorkerAction == "RefreshView")
            {
                RefreshView();
            }
            else if (BackgroundWorkerAction == "SortContentGridView")
            {
                SortContentGridView();
            }
            else if (BackgroundWorkerAction == "LoadViews")
            {
                string[] args = (string[])e.Argument;
//                            object[] args = new object[4] { SelectedView, items, listItemCollectionPositionNext, itemCount };
                this.Invoke(new UpdateViewsHandler(LoadViews), args);
//                LoadViews(args[0], args[1]);
            }
            else if (BackgroundWorkerAction == "UploadEmail")
            {
                object[] args = (object[])e.Argument;
                this.Invoke(new UploadEmailHandler(EUEmailManager.UploadEmail), args);
                //RefreshViewExt();

                //EUFolder selectedFolder = (EUFolder)args[0];
                //DragEventArgs e1 = (DragEventArgs)args[1];
                //Selection selection = (Selection)args[2];
                //EUEmailManager.UploadEmail(selectedFolder, e1, selection);
            }
        }
        private void HideLoadingPictureBox()
        {
            LoadingPictureBox.Visible = false;
        }
        private void ChangeViewBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new HideLoadingPictureBoxHandler(HideLoadingPictureBox), null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            EUFolder folder = SelectedFolder as EUFolder;
            if (folder == null)
                return;
            EUFieldCollection fields = SharePointManager.GetFields(folder.SiteSetting, folder.WebUrl, folder.ListName);
            ListFiltersForm listFiltersForm = new ListFiltersForm();
            listFiltersForm.InitializeForm(fields, CustomFilters);
            listFiltersForm.ShowDialog();

            if (listFiltersForm.DialogResult == DialogResult.OK)
            {
                CustomFilters = listFiltersForm.CustomFilters;
                RefreshViewExt();
            }
        }
    }
}
