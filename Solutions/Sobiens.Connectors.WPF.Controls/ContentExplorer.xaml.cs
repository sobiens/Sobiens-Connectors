using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;
using Sobiens.Connectors.Entities;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.WPF.Controls.Outlook;
using Sobiens.Connectors.Entities.Data;
using System.Data;
using System.Collections.Specialized;
using WPF.JoshSmith.ServiceProviders.UI;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class ContentExplorer : HostControl, IContentExplorer
    {
        ListViewDragDropManager<DataRowView> dragMgr;

        public ContentExplorer()
        {
            InitializeComponent();
            dragMgr = new ListViewDragDropManager<DataRowView>(this.LibraryContentDataListView);
            this.LibraryContentDataListView.Drop -= dragMgr.listView_Drop;
            this.LibraryContentDataListView.Drop += dragMgr_ProcessDrop;
            this.LibraryContentDataListView.DragEnter += LibraryContentDataGridView_DragEnter;
        }

        void dragMgr_ProcessDrop(object sender, DragEventArgs e)//, ProcessDropEventArgs<DataRowView> e)
        {
            if (e.Effects == DragDropEffects.None)
                return;

            if (e.Data.GetDataPresent("System.Data.DataRowView", false) == true && this.dragMgr.IndexUnderDragCursor != -1)//JD.
            {
                
                ListView list = (ListView)e.Source;
                DataTable items = ((DataView)list.ItemsSource).Table;

                DataRowView item = e.Data.GetData(typeof(DataRowView)) as DataRowView;
                
                OC_Datarow dropUnder = (OC_Datarow)(items.Rows[this.dragMgr.IndexUnderDragCursor]);

                bool isFolder = (dropUnder.Tag as Folder != null);

                if (isFolder)
                {
                    ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(SelectedFolder.SiteSettingID);

                    Folder folder = dropUnder.Tag as Folder;
                    OC_Datarow dataSourceItem = (OC_Datarow)(item).Row;
                    IItem itemToMove = dataSourceItem.Tag as IItem;

                    if (ItemsManager.moveItem(siteSetting, itemToMove, folder)) reloadItemList();
                }
            }
            else
            {
                if (SelectedFolder == null)
                    return;
                List<IItem> emailItems = new List<IItem>();

                if (SelectedFolder.CanUpload() == false)
                {
                    MessageBox.Show(Languages.Translate("In order to upload email, you need to enable attachment feature in this list."));
                    return;
                }

                ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(SelectedFolder.SiteSettingID);
                List<UploadItem> uploadItems = ApplicationContext.Current.GetUploadItems(SelectedFolder.GetWebUrl(), siteSetting, SelectedFolder, e, getFieldMappings);
                if (uploadItems == null)
                {
                    return;
                }

                WorkItem workItem = new WorkItem(Languages.Translate("Uploading emails"));
                workItem.CallbackFunction = new WorkRequestDelegate(UploadEmails_Callback);
                workItem.CallbackData = new object[] { siteSetting, SelectedFolder, uploadItems, this.ConnectorExplorer, true };
                workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
                BackgroundManager.GetInstance().AddWorkItem(workItem);

            }
        }

        private bool DisableEventFiring = false;
        private IView SelectedView = null;
        private string SortedFieldName = String.Empty;
        private bool IsAsc = true;
        private int CurrentPageIndex = 0;
        private int CurrentPageItemCount = 0;
        private NameValueCollection ListItemCollectionPositionNexts = new NameValueCollection();
        public CamlFilters CustomFilters = new CamlFilters();

        public IConnectorExplorer ConnectorExplorer
        {
            get;
            private set;
        }

        public void SetConnectorExplorer(IConnectorExplorer connectorExplorer)
        {
            this.ConnectorExplorer = connectorExplorer;
        }

        public Folder SelectedParentFolder
        {
            get;
            set;
        }

        public Folder SelectedFolder
        {
            get;
            set;
        }


        protected override void OnLoad()
        {
            base.OnLoad();
            this.LibraryContentDataListView.ContextMenu = new ContextMenu();
            DisabledAllControls();
        }

        private void DisabledAllControls()
        {
            PreviousButton.IsEnabled = false;
            NextButton.IsEnabled = false;
            SelectViewButton.IsEnabled = false;
            NewButton.IsEnabled = false;
            SynchronizeButton.IsEnabled = false;
        }

        public bool HasAnythingToDisplay
        {
            get
            {
                throw new Exception("Not implemented yet");
            }
        }

        public void AddTempItemInGrid(string id, string title)
        {
            ItemsManager.AddTempItemInGrid(id, title, LibraryContentDataListView);
        }

        public void UpdateUploadItemInGrid(string id, IItem item)
        {
            ItemsManager.UpdateUploadItemInGrid(id, item, LibraryContentDataListView);
        }

        public void UpdateUploadItemErrorInGrid(string id, IItem item)
        {
            ItemsManager.UpdateUploadItemErrorInGrid(id, item, LibraryContentDataListView);
        }
        public void UpdateItem(string id, IItem item)
        {
            ItemsManager.UpdateItem(id, item, LibraryContentDataListView);
        }

        private void LibraryContentDataGridView_DragDrop(object sender, DragEventArgs e)
        {
            //for (int i = 0; i < Application.ActiveExplorer().Selection.Count; i++)
            {
                //TODO: Get the dragged item
                //Outlook.MailItem item = Application.ActiveExplorer().Selection[i + 1] as Outlook.MailItem;
                //emailItems.Add(item);
            }

            //List<EUEmailUploadFile> emailUploadFiles = CommonManager.GetEmailUploadFiles(emailItems, e, isListItemAndAttachmentMode, SaveFormatOverride.None); 
            //ChangeViewBackgroundWorker.RunWorkerAsync(new object[] { this, SelectedFolder, e, emailUploadFiles, isListItemAndAttachmentMode });
        }

        private Dictionary<object, object> getFieldMappings(string webURL, List<ApplicationItemProperty> properties, List<ContentType> contentTypes, FolderSettings folderSettings, FolderSetting defaultFolderSetting, ISiteSetting siteSetting, string rootFolder, out ContentType contentType,bool displayFileName)
        {
            Dictionary<object, object> mappings = new Dictionary<object, object>();

            EditItemPropertiesControl editItemPropertiesControl = new EditItemPropertiesControl(webURL, properties, contentTypes, folderSettings, defaultFolderSetting, siteSetting, rootFolder, null, displayFileName);
            bool? dialogResult = editItemPropertiesControl.ShowDialog(this.ParentWindow, Languages.Translate("Mappings..."));

            if (dialogResult.HasValue == false || dialogResult.Value == false)
            {
                contentType = null;
                return null;
            }
            else
            {
                return editItemPropertiesControl.GetValues(out contentType);
            }
        }

        void UploadEmails_Callback(object item, DateTime dateTime)
        {
            object[] objects = (object[])item;
            ISiteSetting siteSetting = ((ISiteSetting)objects[0]);
            Folder folder = ((Folder)objects[1]);
            List<UploadItem> uploadItems = ((List<UploadItem>)objects[2]);
            IConnectorExplorer connectorExplorer = ((IConnectorExplorer)objects[3]);
            bool canUpdateItemInGrid = ((bool)objects[4]);

            foreach (UploadItem uploadItem in uploadItems)
            {
                if (canUpdateItemInGrid == true)
                {
                    connectorExplorer.ContentExplorer.AddTempItemInGrid(uploadItem.UniqueID.ToString(), uploadItem.FilePath);
                }

                ApplicationContext.Current.UploadFile(siteSetting, uploadItem, connectorExplorer, canUpdateItemInGrid, true, Upload_Success, Upload_Failed);
            }
        }


        public void Upload_Success(object sender, UploadEventArgs e)
        {
            if (e.CanUpdateItemInGrid == true)
            {
                e.ConnectorExplorer.ContentExplorer.UpdateUploadItemInGrid(e.UploadItem.UniqueID.ToString(), e.UploadedItem);
            }
            
        }

        public void Upload_Failed(object sender, UploadEventArgs e)
        {
            if (e.CanUpdateItemInGrid == true)
            {
                e.ConnectorExplorer.ContentExplorer.UpdateUploadItemErrorInGrid(e.UploadItem.UniqueID.ToString(), e.UploadedItem);
            }
        }

        private void LibraryContentDataGridView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
        }

        private void LibraryContentDataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            /*
            if (e.Button == MouseButtons.Left && LibraryContentDataGridView.SelectedRows.Count>0)
            {
                IItem listItem = (IItem)LibraryContentDataGridView.SelectedRows[0].Tag;
                FSItem fsItem = listItem as FSItem;
                object draggedObject = null;
                if (fsItem != null)
                {
                    myData.SetData(DataFormats.FileDrop, true, fsItem.URL);
//                    myData.SetData(DataFormats.Text, true, fsItem.URL);
                    draggedObject = myData;
                }
                else
                    draggedObject = (listItem as IItem).URL;
                
                this.DoDragDrop(myData, DragDropEffects.All);
            }
            */
        }

        private void LibraryContentDataGridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LibraryContentDataListView.SelectedItem == null)
                return;

            OC_Datarow row = (OC_Datarow)((DataRowView)LibraryContentDataListView.SelectedItem).Row;

            if (row.Tag as Folder != null)
            {
                Folder folder = row.Tag as Folder;
                SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);
                Folder parentFolder = this.SelectedFolder;
                if (row["Title"].ToString() == "..")
                {
                    parentFolder = ApplicationContext.Current.GetParentFolder(siteSetting, folder);
                }
                SelectFolder(parentFolder, folder);

                /*
                WorkItem workItem = new WorkItem(Languages.Translate("Populating breadcrumb"));
                workItem.CallbackFunction = new WorkRequestDelegate(BreadcrumbNavigatorExplorerSelectFolder_Callback);
                workItem.CallbackData = new object[] { LibraryContentDataListView, this.ConnectorExplorer.BreadcrumbNavigatorExplorer, folder };
                workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
                BackgroundManager.GetInstance().AddWorkItem(workItem);
                */


                //this.ConnectorExplorer.BreadcrumbNavigatorExplorer.SelectFolder(folder);
            }
        }

        void BreadcrumbNavigatorExplorerSelectFolder_Callback(object item, DateTime dateTime)
        {
            object[] folderObjects = (object[])item;
            ListView listView = ((ListView)folderObjects[0]);
            INavigatorExplorer navigatorExplorer = ((INavigatorExplorer)folderObjects[1]);
            Folder folder = ((Folder)folderObjects[2]);

            listView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                navigatorExplorer.SelectFolder(folder);
            }));
        }


        private void LibraryContentDataGridView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ListView grid = sender as ListView;
            if (grid == null)
            {
                return;
            }
            DataRowView rowView = grid.SelectedItem as DataRowView;

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(this.SelectedFolder.SiteSettingID);
            object obj = rowView != null ? ((OC_Datarow)rowView.Row).Tag : this.SelectedFolder;

            IConnectorMainView connectorExplorer = FindControls.FindParent<OutlookConnectorExplorer>(this);
            object inspector = connectorExplorer != null ? connectorExplorer.Inspector : null;
            ContextMenuManager.Instance.FillContextMenuItems(LibraryContentDataListView.ContextMenu, siteSetting, obj, inspector, SelectedFolder);
        }

        public void SelectFolder(Folder parentFolder, Folder folder)
        {
            DisabledAllControls();
            if (folder == null)
                return;

            this.SelectedFolder = folder;
            this.SelectedParentFolder = parentFolder;
            this.CustomFilters = new CamlFilters();
            this.LoadSubFolders(parentFolder, folder);
            if (folder as SPFolder != null)
            {
                SelectViewButton.IsEnabled = true;
                NewButton.IsEnabled = true;
                SynchronizeButton.IsEnabled = true;
            }
        }

        public void LoadItems(IView view, List<Folder> folders, List<IItem> items, string listItemCollectionPositionNext, int itemCount)
        {
            NextButton.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {

                if (listItemCollectionPositionNext != String.Empty)
                {
                    NextButton.IsEnabled = true;
                    if (ListItemCollectionPositionNexts[(CurrentPageIndex + 1).ToString()] == null)
                        ListItemCollectionPositionNexts.Add((CurrentPageIndex + 1).ToString(), listItemCollectionPositionNext);
                }
                else
                {
                    NextButton.IsEnabled = false;
                }
            }));

            if (CurrentPageIndex > 0)
            {
                PreviousButton.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    PreviousButton.IsEnabled = true;
                }));
            }
            int rowLimit = view != null ? view.RowLimit : 50;
            int startIndex = CurrentPageIndex * rowLimit + 1;
            int endIndex = startIndex + itemCount - 1;
            if (endIndex == 0)
                startIndex = 0;
            PagingLabel.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                PagingLabel.Content = startIndex.ToString() + " - " + endIndex.ToString();
            }));
            SiteSetting siteSetting = null;
            Folder selectedParentFolder = null;
            Folder selectedFolder = null;
            this.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(this.SelectedFolder.SiteSettingID);
                selectedParentFolder = this.SelectedParentFolder;
                selectedFolder = this.SelectedFolder;
            }));

            ApplicationContext.Current.BindItemsToListViewControl(siteSetting, selectedParentFolder, selectedFolder, view, folders, items, LibraryContentDataListView, GridViewControl);
        }

        public void LoadViews(Folder folder)
        {
            if (folder.ContainsItems == false)
            {
                return;
            }

            WorkItem workItem = new WorkItem(Languages.Translate("Populating views"));
            workItem.CallbackFunction = new WorkRequestDelegate(LoadViews_Callback);
            workItem.CallbackData = folder;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void LoadSubFolder_Callback(object item, DateTime dateTime)
        {
            CurrentPageIndex = 0;
            string currentListItemCollectionPositionNext = string.Empty;

            object[] folderObjects = (object[])item;
            Folder parentFolder = ((Folder)folderObjects[0]);
            Folder folder = ((Folder)folderObjects[1]);

            WorkItem workItem = new WorkItem(Languages.Translate("Populating list view"));
            workItem.CallbackFunction = new WorkRequestDelegate(BindListView_Callback);
            workItem.CallbackData = new object[] { folder, SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, CustomFilters };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);

            LoadViews(folder);
        }

        void LoadViews_Callback(object item, DateTime dateTime)
        {
            Folder folder = ((Folder)item);
            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);
            List<IView> views = ApplicationContext.Current.GetViews(siteSetting, folder);
            SelectViewButtonContextMenu.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DisableEventFiring = true;
                SelectViewButtonContextMenu.Items.Clear();
                foreach (IView view in views)
                {
                    MenuItem menuItem = new MenuItem();
                    menuItem.Click += menuItem_Click;
                    menuItem.Header = view.ToString();
                    menuItem.Tag = view;
                    //editMnu.Icon = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/Images/edit.png", UriKind.Absolute)) };


                    SelectViewButtonContextMenu.Items.Add(menuItem);
                }
                //SelectViewButtonContextMenu.SelectedIndex = 0;
                SelectViewButtonContextMenu.Visibility = System.Windows.Visibility.Visible;
                DisableEventFiring = false;
            }));

        }

        void menuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DisableEventFiring == true)
                return;

            IView view = ((MenuItem)e.Source).Tag as IView;
            if (view != null)
            {
                ChangeView(view);
            }
        }


        public void DeleteUploadItemInvoke(Guid guid)
        {
            //TODO: Implement this
            /*
            object[] args = new object[] { guid };
            this.Invoke(new DeleteUploadItemHandler(DeleteUploadItem), args);
             */
        }

        public void DeleteUploadItem(Guid guid)
        {
            //TODO: Implement this
            /*
            for (int i = LibraryContentDataGridView.Rows.Count - 1; i > -1; i--)
            {
                if (LibraryContentDataGridView.Rows[i].Tag is string && LibraryContentDataGridView.Rows[i].Tag.ToString() == guid.ToString())
                {
                    LibraryContentDataGridView.Rows.RemoveAt(i);
                }
            }
             */
        }

        private void ChangeView(IView selectedView)
        {
            SortedFieldName = String.Empty;
            IsAsc = true;
            CurrentPageIndex = 0;
            ListItemCollectionPositionNexts = new NameValueCollection();
            ListItemCollectionPositionNexts.Add("0", String.Empty);
            string currentListItemCollectionPositionNext = ListItemCollectionPositionNexts[(CurrentPageIndex).ToString()];

            WorkItem workItem = new WorkItem(Languages.Translate("Populating list view"));
            workItem.CallbackFunction = new WorkRequestDelegate(BindListView_Callback);
            workItem.CallbackData = new object[] { SelectedFolder, selectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, CustomFilters };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);

        }

        private CamlFilters GetCustomFilters()
        {
            CamlFilters customFilters = new CamlFilters();
            if (string.IsNullOrEmpty(searchTextBox1.Text) == false)
            {
                if (this.SelectedFolder as SPFolder != null
                && (this.SelectedFolder as SPFolder).IsDocumentLibrary == true)
                {
                    customFilters.Add(new CamlFilter("FileLeafRef", FieldTypes.Text, CamlFilterTypes.Contains, searchTextBox1.Text));
                }
                else
                {
                    customFilters.Add(new CamlFilter("Title", FieldTypes.Text, CamlFilterTypes.Contains, searchTextBox1.Text));
                }
            }

            return customFilters;
        }

        void BindListView_Callback(object item, DateTime dateTime)
        {
            int itemCount;
            string listItemCollectionPositionNext = String.Empty;
            object[] args = ((object[])item);
            Folder selectedFolder = (Folder)args[0];
            IView selectedView = (IView)args[1];
            string sortedFieldName = (string)args[2];
            bool isAsc = (bool)args[3];
            int currentPageIndex = (int)args[4];
            string currentListItemCollectionPositionNext = (string)args[5];
            CamlFilters customFilters = (CamlFilters)args[6];

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(selectedFolder.SiteSettingID);

            List<IItem> items = ApplicationContext.Current.GetListItems(siteSetting, selectedFolder, selectedView, sortedFieldName, isAsc, currentPageIndex, currentListItemCollectionPositionNext, customFilters, false, out listItemCollectionPositionNext, out itemCount);
            List<Folder> folders = null;
            if (selectedFolder.Folders.Count > 0)
            {
                folders = selectedFolder.Folders;
            }
            else
            {
                folders = ApplicationContext.Current.GetSubFolders(siteSetting, selectedFolder, null);
            }

            LoadItems(selectedView, folders, items, listItemCollectionPositionNext, itemCount);

        }
        //void MoveItem_Callback(object item, DateTime dateTime)
        //{
        //    object[] args = (object[])item;
        //    ISiteSetting siteSetting = (ISiteSetting)args[0];
        //    IItem Item = (IItem)args[1];
        //    Folder folder = (Folder)args[2];
        //    try
        //    {
        //        ItemsManager.moveItem(siteSetting, Item, folder);
        //        //ApplicationContext.Current.MoveFile(Item, folder, Item.Title);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.ToString());
        //    }


        //}


        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex--;
            string currentListItemCollectionPositionNext = ListItemCollectionPositionNexts[(CurrentPageIndex).ToString()];

            WorkItem workItem = new WorkItem(Languages.Translate("Populating list view"));
            workItem.CallbackFunction = new WorkRequestDelegate(BindListView_Callback);
            workItem.CallbackData = new object[] { SelectedFolder, SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, this.GetCustomFilters() };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentPageIndex++;
            string currentListItemCollectionPositionNext = ListItemCollectionPositionNexts[(CurrentPageIndex).ToString()];

            WorkItem workItem = new WorkItem(Languages.Translate("Populating list view"));
            workItem.CallbackFunction = new WorkRequestDelegate(BindListView_Callback);
            workItem.CallbackData = new object[] { SelectedFolder, SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, this.GetCustomFilters() };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }
        private void searchTextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            CurrentPageIndex = 0;
            string currentListItemCollectionPositionNext = string.Empty;

            WorkItem workItem = new WorkItem(Languages.Translate("Populating list view"));
            workItem.CallbackFunction = new WorkRequestDelegate(BindListView_Callback);
            workItem.CallbackData = new object[] { SelectedFolder, SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, this.GetCustomFilters() };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        public void LoadSubFolders(Folder parentFolder, Folder folder)
        {
            CustomFilters = new CamlFilters();

            LoadSubFolder_Callback(new object[] { parentFolder, folder }, DateTime.Now);

            //WorkItem workItem = new WorkItem("Populating subfolders");
            //workItem.CallbackFunction = new WorkRequestDelegate(LoadSubFolder_Callback);
            //workItem.CallbackData = new object[] { parentFolder, folder };
            //workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            //BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        public void reloadItemList()
        {
            CurrentPageIndex = 0;
            string currentListItemCollectionPositionNext = string.Empty;

            WorkItem workItem = new WorkItem(Languages.Translate("Populating list view"));
            workItem.CallbackFunction = new WorkRequestDelegate(BindListView_Callback);
            workItem.CallbackData = new object[] { SelectedFolder, SelectedView, SortedFieldName, IsAsc, CurrentPageIndex, currentListItemCollectionPositionNext, this.GetCustomFilters() };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            NewButtonContextMenu.IsOpen = true;
        }

        private void SynchronizeButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void NewFileUploadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedFolder == null)
            {
                MessageBox.Show("Please select a folder first!");
                return;
            }

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;

            // Set filter for file extension and default file extension 
            //dlg.DefaultExt = ".png";
            //dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                List<IItem> emailItems = new List<IItem>();

                ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(SelectedFolder.SiteSettingID);

                List<UploadItem> uploadItems = ApplicationContext.Current.GetUploadItems(SelectedFolder.GetWebUrl(), siteSetting, SelectedFolder, dlg.FileNames);
                if (uploadItems == null)
                {
                    return;
                }

                WorkItem workItem = new WorkItem(Languages.Translate("Uploading emails"));
                workItem.CallbackFunction = new WorkRequestDelegate(UploadEmails_Callback);
                workItem.CallbackData = new object[] { siteSetting, SelectedFolder, uploadItems, this.ConnectorExplorer, true };
                workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
                BackgroundManager.GetInstance().AddWorkItem(workItem);

            }
        }

        private void FolderNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            object[] args = {"Test"};
            ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(SelectedFolder.SiteSettingID);
            ApplicationContext.Current.DoMenuItemAction(siteSetting, SC_MenuItemTypes.AddFolder, SelectedFolder, args);
        }

        private void NewWordDocumentMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void NewExcelWorkbookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void NewPowerPointPresentationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void NewOneNoteNotebookMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented yet");
        }

        private void SelectViewButton_Click(object sender, RoutedEventArgs e)
        {
            SelectViewButtonContextMenu.IsOpen = true;
        }
    }
}
