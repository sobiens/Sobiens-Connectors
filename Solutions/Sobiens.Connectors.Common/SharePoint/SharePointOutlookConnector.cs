#if General
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Services.SharePoint;
using Sobiens.Connectors.Entities.Settings;
using System.Windows.Threading;
using System.Threading;
using System.Data;
using System.Windows.Data;
using Sobiens.Connectors.Entities.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sobiens.Connectors.Common.SharePoint
{
    public class SharePointOutlookConnector:IOutlookConnector
    {
        /// <summary>
        /// Occurs when [upload failed].
        /// </summary>
        public event EventHandler UploadFailed;

        /// <summary>
        /// Occurs when [upload succeeded].
        /// </summary>
        public event EventHandler UploadSucceeded;

        private bool addedEventHandler = false;
        EventHandler eventUploadSucceededHandler = null;
        EventHandler eventUploadFailedHandler = null;

        private ISiteSetting SiteSetting = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SharePointOutlookConnector"/> class.
        /// </summary>
        public SharePointOutlookConnector(ISiteSetting siteSetting)
        {
            this.SiteSetting = siteSetting;
            if (!addedEventHandler)
            {
                eventUploadFailedHandler = new EventHandler(SharePointOutlookConnector_UploadFailed);
                eventUploadSucceededHandler = new EventHandler(SharePointOutlookConnector_UploadSucceeded);
                //BackgroundThreadManager.GetInstance().UploadSucceeded += eventUploadSucceededHandler;
                addedEventHandler = true;
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SharePointOutlookConnector"/> is reclaimed by garbage collection.
        /// </summary>
        ~SharePointOutlookConnector()
        {
            if (eventUploadSucceededHandler != null)
            {
                //BackgroundThreadManager.GetInstance().UploadFailed -= eventUploadFailedHandler;
                //BackgroundThreadManager.GetInstance().UploadSucceeded -= eventUploadSucceededHandler;
            }
        }

        /// <summary>
        /// Handles the UploadFailed event of the SharePointOutlookConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SharePointOutlookConnector_UploadFailed(object sender, EventArgs e)
        {
            if (UploadFailed != null)
                UploadFailed(sender, e);

        }

        /// <summary>
        /// Handles the UploadSucceeded event of the SharePointOutlookConnector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void SharePointOutlookConnector_UploadSucceeded(object sender, EventArgs e)
        {
            if (UploadSucceeded != null)
                UploadSucceeded(sender, e);

        }

        public Folder GetRootFolder()
        {
            return new SPWeb(this.SiteSetting.Url, this.SiteSetting.Url, this.SiteSetting.ID, this.SiteSetting.Url, this.SiteSetting.Url, this.SiteSetting.Url, this.SiteSetting.Url);
        }
        public List<Folder> GetSubFolders(Folder folder)
        {
            ISharePointService spService = new SharePointService();
            List<Folder> subFolders = new List<Folder>();
            /*
            if (folder as SPWeb != null)
            {
                SPWeb web = (SPWeb)folder;
                List<SPWeb> webs = spService.GetWebs(web.Url, (SiteSetting)this.SiteSetting);
                foreach (SPWeb _web in webs)
                {
                    _web.ParentFolder = web;
                    subFolders.Add(_web);
                }
                List<SPList> lists = spService.GetLists(web.Url, (SiteSetting)this.SiteSetting);
                foreach (SPList list in lists)
                {
                    if (
                        (list.ServerTemplate == 101 || list.ServerTemplate == 100 || list.BaseType == 1) //or BaseType == 1 
                        && list.Hidden == false
                        )
                    {
                        list.ParentFolder = web;
                        subFolders.Add(list);
                    }
                }
            }
            else if (folder as SPFolder != null)
            {
                SPFolder _folder = (SPFolder)folder;
                List<Folder> folders = spService.GetFolders(_folder, (SiteSetting)this.SiteSetting);
                foreach (SPFolder __folder in folders)
                {
                    __folder.ParentFolder = _folder;
                    subFolders.Add(__folder);
                }
            }
             */ 
            return subFolders;
        }

        public List<IView> GetViews(Folder folder)
        {
            return null;
            /*
            SPFolder _folder = folder as SPFolder;
            ISharePointService spService = new SharePointService();
            return spService.GetViews(_folder.WebUrl, _folder.ListName, (SiteSetting)this.SiteSetting);
             */ 
        }

        public IView GetView(string webUrl, string listName, string viewName)
        {
            return null;
        }

        public List<IItem> GetListItems(Folder folder, IView view, string sortField, bool isAsc, int currentPageIndex, string currentListItemCollectionPositionNext, CamlFilters filters, bool isRecursive, out string listItemCollectionPositionNext, out int itemCount)
        {
            SPFolder _folder = folder as SPFolder;
            string folderPath = string.Empty;
            if (folder as SPList == null)
            {
                folderPath = _folder.GetPath();
            }

            ISharePointService spService = new SharePointService();
            return spService.GetListItems((SiteSetting)this.SiteSetting, (SPView)view, sortField, isAsc, _folder.IsDocumentLibrary, _folder.WebUrl, _folder.ListName, folderPath, currentListItemCollectionPositionNext, filters, isRecursive, out listItemCollectionPositionNext, out itemCount);
        }

        public void BindFoldersToListViewControl(Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataGridView)
        {
            DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataSet ds = libraryContentDataGridView.Tag as DataSet;
                if (ds == null)
                {
                    ds = this.GetBaseDataSet();
                }
                else
                {
                    RemoveAdditionalColumns(ds);
                }

                if (folders != null)
                {
                    RemoveFolders(ds);
                    AddFolders((SPBaseFolder)parentFolder, ds, folders);
                }

                this.SetBaseGridColumns(libraryContentDataGridView);
                libraryContentDataGridView.ItemsSource = ds.Tables[0].AsDataView();
            }));
        }

        private DataSet GetBaseDataSet()
        {
            DataSet ds = new DataSet();
            OC_DataTable dt = new OC_DataTable();
            ds.Tables.Add(dt);
            DataColumn dtTypeColumn = dt.Columns.Add("Type");
            DataColumn dtPictureColumn = dt.Columns.Add("Picture");
            DataColumn dtIDColumn = dt.Columns.Add("ID");
            DataColumn dtTitleColumn = dt.Columns.Add("Title");
            dtTitleColumn.Caption = Languages.Translate("Name");
            DataColumn dtModifiedColumn = dt.Columns.Add("Modified");
            DataColumn dtModifiedByColumn = dt.Columns.Add("ModifiedBy");
            dtModifiedByColumn.Caption = Languages.Translate("Modified By");
            DataColumn dtURLColumn = dt.Columns.Add("URL");
            return ds;
        }

        private void SetBaseGridColumns(DataGrid libraryContentDataGridView)
        {
            libraryContentDataGridView.Columns.Clear();

            DataGridTemplateColumn col1 = new DataGridTemplateColumn();
            col1.Header = "Icon";
            FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
            Binding b1 = new Binding("Picture");
            b1.Mode = BindingMode.TwoWay;
            factory1.SetValue(Image.SourceProperty, b1);
            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factory1;
            col1.CellTemplate = cellTemplate1;
            libraryContentDataGridView.Columns.Add(col1);

            DataGridTextColumn titleColumn = new DataGridTextColumn();
            titleColumn.Header =Languages.Translate("Title");
            titleColumn.Binding = new Binding("Title");
            //titleColumn.AllowAutoFilter = true;
            libraryContentDataGridView.Columns.Add(titleColumn);

            DataGridTextColumn urlColumn = new DataGridTextColumn();
            urlColumn.Header = Languages.Translate("URL");
            urlColumn.Binding = new Binding("URL");
            //urlColumn.AllowAutoFilter = true;
            urlColumn.Visibility = Visibility.Hidden;
            libraryContentDataGridView.Columns.Add(urlColumn);
        }

        private void RemoveAdditionalColumns(DataSet dataset)
        {
            OC_DataTable table = dataset.Tables[0] as OC_DataTable;
            for (int i = table.Columns.Count - 1; i > -1; i--)
            {
                string cname = table.Columns[i].ColumnName;
                if (cname.Equals("ID") == false && cname.Equals("Type") == false
                    && cname.Equals("Title") == false && cname.Equals("Modified") == false
                    && cname.Equals("ModifiedBy") == false && cname.Equals("URL") == false)
                {
                    table.Columns.RemoveAt(i);
                }
            }
        }

        private void AddFolders(SPBaseFolder parentFolder, DataSet dataset, List<Folder> folders)
        {
            OC_DataTable table = dataset.Tables[0] as OC_DataTable;
            if (parentFolder != null)
            {
                OC_Datarow newRow = (OC_Datarow)table.NewRow();
                newRow.Tag = parentFolder;
                newRow["ID"] = parentFolder.UniqueIdentifier;
                newRow["Title"] = "..";
                newRow["URL"] = parentFolder.UniqueIdentifier;
                newRow["Picture"] = ImageManager.GetInstance().GetUpFolderImage();
                table.Rows.Add(newRow);
            }
            foreach (Folder subfolder in folders)
            {
                OC_Datarow newRow = (OC_Datarow)table.NewRow();
                newRow.Tag = subfolder;
                newRow["ID"] = subfolder.UniqueIdentifier;
                newRow["Title"] = subfolder.Title;
                newRow["URL"] = subfolder.UniqueIdentifier;
                string picturePath = string.Empty;
                if (subfolder as SPWeb != null)
                {
                    picturePath = ImageManager.GetInstance().GetWebImage();
                }
                else if (subfolder as SPList != null)
                {
                    picturePath = ImageManager.GetInstance().GetListImage();
                }
                else
                {
                    picturePath = ImageManager.GetInstance().GetFolderImage();
                }

                newRow["Picture"] = picturePath;
                table.Rows.Add(newRow);
            }
        }

        private void RemoveFolders(DataSet dataset)
        {
            OC_DataTable table = dataset.Tables[0] as OC_DataTable;
            for (int i = table.Rows.Count - 1; i > -1; i--)
            {
                OC_Datarow row = table.Rows[i] as OC_Datarow;
                if (row.Tag as Folder != null)
                {
                    table.Rows.RemoveAt(i);
                }
            }
        }

        private void AddItems(DataSet dataset, List<IItem> items)
        {
            OC_DataTable table = dataset.Tables[0] as OC_DataTable;
            foreach (IItem item in items)
            {
                OC_Datarow newRow = (OC_Datarow)table.NewRow();
                newRow.Tag = item;
                string extensionName = item.URL.Substring(item.URL.LastIndexOf('.')+1);
                newRow["Picture"] = ImageManager.GetInstance().GetExtensionImageFromResource(extensionName,item.isExtracted());
                newRow["ID"] = item.UniqueIdentifier;
                newRow["Title"] = item.Title;
                newRow["URL"] = item.URL;
                table.Rows.Add(newRow);
            }
        }

        private void RemoveItems(DataSet dataset)
        {
            OC_DataTable table = dataset.Tables[0] as OC_DataTable;
            for (int i = table.Rows.Count - 1; i > -1; i--)
            {
                OC_Datarow row = table.Rows[i] as OC_Datarow;
                if (row.Tag as IItem != null)
                {
                    table.Rows.RemoveAt(i);
                }
            }
        }

        public void BindSearchResultsToListViewControl(List<IItem> items, object LibraryContentDataGridView) 
        {
            DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                //libraryContentDataGridView.Columns.Clear();
                //libraryContentDataGridView.Items.Clear();
                /*
                foreach (CamlFieldRef viewField in view.ViewFields)
                {
                    DataGridTextColumn textColumn = new DataGridTextColumn(); 
                    textColumn.Header = viewField.Name; 
                    textColumn.Binding = new Binding(viewField.Name); 
                    libraryContentDataGridView.Columns.Add(textColumn); 
                }
                 */
                DataSet ds = new DataSet();
                DataTable dt = ds.Tables.Add();
                DataColumn dtTypeColumn = dt.Columns.Add("Type");
                DataColumn dtIDColumn = dt.Columns.Add("ID");
                DataColumn dtTitleColumn = dt.Columns.Add("Title");
                dtTitleColumn.Caption = Languages.Translate("Name");
                DataColumn dtModifiedColumn = dt.Columns.Add("Modified");
                DataColumn dtModifiedByColumn = dt.Columns.Add("ModifiedBy");
                dtModifiedByColumn.Caption = Languages.Translate("Modified By");
                DataColumn dtURLColumn = dt.Columns.Add("URL");
                foreach (IItem item in items)
                {
                    DataRow newRow = dt.NewRow();
                    newRow["ID"] = item.UniqueIdentifier;
                    newRow["Title"] = item.Title;
                    newRow["URL"] = item.URL;
                    dt.Rows.Add(newRow);
                }

                DataGridTextColumn titleColumn = new DataGridTextColumn();
                titleColumn.Header = Languages.Translate("Title");
                titleColumn.Binding = new Binding("Title");
                //titleColumn.AllowAutoFilter = true;
                libraryContentDataGridView.Columns.Add(titleColumn);

                DataGridTextColumn urlColumn = new DataGridTextColumn();
                urlColumn.Header = Languages.Translate("URL");
                urlColumn.Binding = new Binding("URL");
                //urlColumn.AllowAutoFilter = true;
                urlColumn.Visibility = Visibility.Hidden;
                libraryContentDataGridView.Columns.Add(urlColumn);

                libraryContentDataGridView.Tag = ds;
                libraryContentDataGridView.ItemsSource = dt.AsDataView();
            }));
        }

        public void BindItemsToListViewControl(Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataGridView)
        {
            DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataSet ds = libraryContentDataGridView.Tag as DataSet;
                if (ds == null)
                {
                    ds = this.GetBaseDataSet();
                }
                else
                {
                    RemoveAdditionalColumns(ds);
                }

                if (folders != null)
                {
                    RemoveFolders(ds);
                    AddFolders((SPBaseFolder)parentFolder, ds, folders);
                }

                if (items != null)
                {
                    RemoveItems(ds);
                    AddItems(ds, items);
                }

                this.SetBaseGridColumns(libraryContentDataGridView);
                /*
                foreach (CamlFieldRef viewField in view.ViewFields)
                {
                    DataGridTextColumn textColumn = new DataGridTextColumn(); 
                    textColumn.Header = viewField.Name; 
                    textColumn.Binding = new Binding(viewField.Name); 
                    libraryContentDataGridView.Columns.Add(textColumn); 
                }
                 */
                libraryContentDataGridView.ItemsSource = ds.Tables[0].AsDataView();
            }));
        }

        /*
        public static void BindListItemToRow(EUListItem listItem, DataGridViewRow row, bool isDocumentLibrary, IView view)
        {
            string title = String.Empty;
            if (isDocumentLibrary == true)
            {
                string fileName = listItem.Title;
                string extensionName = String.Empty;
                if (fileName.LastIndexOf(".") > 0)
                {
                    extensionName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();
                }
                title = fileName;
                row.Cells["ExtensionImageColumn"].Value = ImageManager.GetInstance().GetExtensionImageFromResource(extensionName);
            }
            else
            {
                title = listItem.Title;
                row.Cells["ExtensionImageColumn"].Value = ImageManager.GetInstance().GetExtensionImageFromResource("list");
            }
            if (listItem.CheckoutUser != String.Empty)
            {
                row.Cells["ExtensionImageColumn"].Style.BackColor = Color.MediumSeaGreen;
            }

            row.Cells["TitleColumn"].Value = title;
            row.Cells["FilePathColumn"].Value = listItem.URL;
            for (int i = 0; i < view.ViewFields.Count; i++)
            {
                string fieldName = view.ViewFields[i].Name;
                if (listItem.Properties["ows_" + fieldName] != null)
                    row.Cells[fieldName].Value = listItem.Properties["ows_" + fieldName].Value;
            }
            row.Tag = listItem;
        }

        public void UploadFiles(Folder folder, List<EUEmailUploadFile> uploadFiles, List<Field> fields, EUFieldInformations fieldInformations, SharePointListViewControl sharePointListViewControl)
        {
            SPFolder _folder = folder as SPFolder;
            string destinationFolderUrl = _folder.WebUrl.TrimEnd(new char[] { '/' }) + "/" + _folder.FolderPath.TrimStart(new char[] { '/' });

            BackgroundThreadManager.GetInstance().QueueUploadItems(_folder, destinationFolderUrl, uploadFiles, fields, fieldInformations, sharePointListViewControl);

        }
        */
        public void DeleteListItem(IItem item)
        {
            /*
            EUListItem listItem = (EUListItem)item;
            SharePointManager.DeleteListItem(listItem.SiteSetting, listItem.WebURL, listItem.ListName, listItem.URL, listItem.ID);
             */ 
        }
        
        /// <summary>
        /// Checks the folder exists.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public bool CheckFolderExists(Folder folder, string newFolderName)
        {
            return false;
            /*
            SPFolder listFolder = (SPFolder)folder;
            return SharePointManager.CheckFolderExists(listFolder.SiteSetting, listFolder.WebUrl, listFolder.ListName, listFolder.FolderPath, newFolderName);
             */
        }

        public bool CheckFileExistency(Folder folder, IItem item, string newFileName)
        {
            return false;
            /*
            SPFolder listFolder = (SPFolder)folder;
            return SharePointManager.CheckFileExistency(listFolder.SiteSetting, listFolder.WebUrl, listFolder.ListName, listFolder.FolderPath, null, newFileName);
             */
        }

        public void CopyFile(Folder folder, IItem item, string newFileName)
        {
            return;
            /*
            EUListItem listItem = (EUListItem)item;
            SPFolder listFolder = (SPFolder)folder;

            string folderPath = listFolder.WebUrl + "/" + listFolder.FolderPath;
            string webUrl = listFolder.WebUrl;
            string listName = listFolder.ListName;

            SharePointCopyWS.CopyResult[] myCopyResultArray = null;
            SharePointManager.CopyFile(listFolder.SiteSetting, webUrl, listItem.URL, folderPath + "/" + newFileName, out myCopyResultArray);
             */ 
        }
        public void MoveFile(IItem item, Folder folder, string newFileName)
        {
            //ISharePointService spService = new SharePointService();
            
            return;
        }
    }
}
#endif
