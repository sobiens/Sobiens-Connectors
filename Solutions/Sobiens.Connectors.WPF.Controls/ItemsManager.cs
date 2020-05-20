using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.WPF.Controls.Settings;
using System.Diagnostics;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.WPF.Controls.EditItems;
using System.Windows.Threading;
using System.Threading;
using System.Data;
using System.Windows.Data;
using Sobiens.Connectors.Entities.Data;
using System.Windows.Controls;
using Microsoft.Lync.Controls;
using System.Web;

namespace Sobiens.Connectors.WPF.Controls
{
    public class ItemsManager
    {
        private static CopiedItemInfo _CopiedItemInfo = null;

        public static CopiedItemInfo GetCopiedItemInfo()
        {
            return _CopiedItemInfo;
        }

        public static void SetCopiedItemInfo(ISiteSetting siteSetting, IItem item, bool move)
        {
            if (item.isExtracted())
            {
                MessageBox.Show(Languages.Translate("You must archive this document before doing this operation"), Languages.Translate("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _CopiedItemInfo = new CopiedItemInfo(siteSetting, item, move);
        }

        public static void ConnectorMainViewRefreshControls(IDocumentTemplateSelection documentTemplateSelection, ISearchExplorer searchExplorer, IConnectorExplorer connectorExplorer,
        IWorkflowExplorer workflowExplorer, TabControl tabControl, TabItem workflowTabItem,
        TabItem templateTabItem, TabItem searchTabItem, TabItem navigatorTabItem
            )
        {
            if (tabControl.SelectedItem == workflowTabItem)
            {
                workflowExplorer.RefreshControls();
            }
            else if (tabControl.SelectedItem == templateTabItem)
            {
                documentTemplateSelection.RefreshControls();
            }
            else if (tabControl.SelectedItem == searchTabItem)
            {
                searchExplorer.RefreshControls();
            }
            else if (tabControl.SelectedItem == navigatorTabItem)
            {
                connectorExplorer.RefreshControls();
            }
        }

        public static void ConnectorMainViewInitialize(IDocumentTemplateSelection documentTemplateSelection, ISearchExplorer searchExplorer, IConnectorExplorer connectorExplorer,
            IWorkflowExplorer workflowExplorer, TabControl tabControl, TabItem workflowTabItem, TabItem emptyConfigurationTabItem,
            TabItem templateTabItem, TabItem searchTabItem, TabItem navigatorTabItem, object inspector, StatusBar statusBar
            )
        {
            if (inspector != null)
            {
                statusBar.Visibility = System.Windows.Visibility.Collapsed;
            }

            try
            {
                workflowTabItem.Visibility = System.Windows.Visibility.Collapsed;
                emptyConfigurationTabItem.Visibility = System.Windows.Visibility.Collapsed;
                templateTabItem.Visibility = System.Windows.Visibility.Collapsed;
                searchTabItem.Visibility = System.Windows.Visibility.Collapsed;
                navigatorTabItem.Visibility = System.Windows.Visibility.Collapsed;
                documentTemplateSelection.Initialize(ConfigurationManager.GetInstance().GetSiteSettings(), ConfigurationManager.GetInstance().GetDocumentTemplates(ApplicationContext.Current.GetApplicationType()));
                searchExplorer.Initialize(ConfigurationManager.GetInstance().GetSiteSettings(), ConfigurationManager.GetInstance().GetDocumentTemplates());
                connectorExplorer.Initialize(ConfigurationManager.GetInstance().GetSiteSettings(), ConfigurationManager.GetInstance().GetExplorerLocations(ApplicationContext.Current.GetApplicationType()));
                workflowExplorer.Initialize(ConfigurationManager.GetInstance().GetSiteSettings(), ConfigurationManager.GetInstance().GetWorkflowConfigurations());
                if (documentTemplateSelection.HasAnythingToDisplay == true
                    || searchExplorer.HasAnythingToDisplay == true
                    || workflowExplorer.HasAnythingToDisplay == true
                    || connectorExplorer.HasAnythingToDisplay == true)
                {
                    bool isTabSelected = false;
                    if (documentTemplateSelection.HasAnythingToDisplay == true)
                    {
                        isTabSelected = true;
                        templateTabItem.Visibility = System.Windows.Visibility.Visible;
                        tabControl.SelectedItem = templateTabItem;
                    }

                    if (connectorExplorer.HasAnythingToDisplay == true)
                    {
                        navigatorTabItem.Visibility = System.Windows.Visibility.Visible;
                        if (isTabSelected == false)
                        {
                            tabControl.SelectedItem = navigatorTabItem;
                            isTabSelected = true;
                        }
                    }

                    if (searchExplorer.HasAnythingToDisplay == true)
                    {
                        searchTabItem.Visibility = System.Windows.Visibility.Visible;
                        if (isTabSelected == false)
                        {
                            tabControl.SelectedItem = searchTabItem;
                        }
                    }

                    if (workflowExplorer.HasAnythingToDisplay == true)
                    {
                        workflowTabItem.Visibility = System.Windows.Visibility.Visible;
                        if (isTabSelected == false)
                        {
                            tabControl.SelectedItem = workflowTabItem;
                        }
                    }
                }
                else
                {
                    emptyConfigurationTabItem.Visibility = System.Windows.Visibility.Visible;
                    tabControl.SelectedItem = emptyConfigurationTabItem;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public static void PasteItem(ISiteSetting siteSetting, object Item, Folder folder)
        {
            Folder f = folder;

            if (Item as SPListItem == null)
            {
                f = (Folder)Item;
            }

            FileCopyNameForm fileCopyNameForm = new FileCopyNameForm();
            fileCopyNameForm.Initialize(false, _CopiedItemInfo.Item.Title);
            bool needsTry = true;

            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            while (needsTry == true)
            {
                if (fileCopyNameForm.ShowDialog(null, Languages.Translate("File Name Confirmation"), 220, 400) == true)
                {

                    if (serviceManager.CheckItemCanBeCopied(siteSetting, folder, _CopiedItemInfo.Item, fileCopyNameForm.NewFileName) == true)
                    {

                        Result result =
                        serviceManager.CopyItem(siteSetting, folder, _CopiedItemInfo.Item, fileCopyNameForm.NewFileName);
                        needsTry = false;
                    }
                    else
                    {
                        fileCopyNameForm = new FileCopyNameForm();
                        fileCopyNameForm.Initialize(true, fileCopyNameForm.NewFileName);
                    }
                }
                else
                {
                    needsTry = false;
                }

            }

            if (_CopiedItemInfo.Move && true) serviceManager.DeleteFile(siteSetting, _CopiedItemInfo.Item);

            if (siteSetting.CheckInAfterCopy)
            {
                SPListItem item = new SPListItem(siteSetting.ID);
                item.WebURL = folder.GetWebUrl();
                item.URL = folder.GetUrl().CombineUrl(fileCopyNameForm.NewFileName);
                CheckinTypes checktype = siteSetting.useMajorVersionAsDefault ? CheckinTypes.MajorCheckIn : CheckinTypes.MinorCheckIn;
                serviceManager.CheckInFile(siteSetting, item, "", checktype);
            }

            _CopiedItemInfo = null;
        }
        public static bool moveItem(ISiteSetting siteSetting, IItem item, Folder folder)
        {
            if (item == null) return false;



            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            if (serviceManager.CheckItemCanBeCopied(siteSetting, folder, item, item.Title))
            {
                Result result = serviceManager.CopyItem(siteSetting, folder, item, item.Title);
                if (result.codeResult == "Success")
                {
                    serviceManager.DeleteFile(siteSetting, item);


                    if (siteSetting.CheckInAfterCopy)
                    {
                        SPListItem tempItem = new SPListItem(siteSetting.ID);
                        tempItem.WebURL = folder.GetWebUrl();
                        tempItem.URL = folder.GetUrl().CombineUrl(item.Title);
                        CheckinTypes checktype = siteSetting.useMajorVersionAsDefault ? CheckinTypes.MajorCheckIn : CheckinTypes.MinorCheckIn;
                        serviceManager.CheckInFile(siteSetting, item, "", checktype);
                    }

                    return true;
                }
            }
            else
            {
                MessageBox.Show(Languages.Translate("This object could not be move"), Languages.Translate("Error"), MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return false;
        }
        public static void displayFolder(ISiteSetting siteSetting, object Item, bool inExplorer)
        {
            string url;
            if (Item as Folder != null)
            {
                url = ((Folder)Item).GetUrl();
            }
            else
            {
                SPListItem item = (SPListItem)Item;
                //if (item.FolderPath == "") item.FolderPath = item.ListName;//root item doesn't have folderpath
                //url = item.WebURL.CombineUrl(item.FolderPath);
                url = item.GetListItemURL();
            }

            if (!string.IsNullOrEmpty(url))
            {
                if (inExplorer)
                {
                    string path = "file" + url.ToLower().Replace("http", string.Empty);
                    Process.Start("Explorer.exe", path);
                }
                else
                {
                    Process.Start("IExplore.exe", url);
                }
            }
        }

        public static void AddFolder(ISiteSetting siteSetting, object Item, string folderName)
        {
            string folderPath = "";
            string webURL = "";
            string listName = string.Empty;

            if (Item as SPListItem != null)
            {
                SPListItem item = (SPListItem)Item;
                //folderPath = item.;
                folderPath = string.IsNullOrEmpty(folderPath) ? item.ListName : folderPath;
                webURL = item.WebURL;
                listName = item.ListName;
            }
            else
            {
                SPFolder folder = (SPFolder)Item;
                folderPath = folder.FolderPath;
                webURL = folder.WebUrl;
                listName = folder.ListName;
            }

            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.AddFolder(siteSetting, webURL, folderName, folderPath, listName);
        }

        public static void DeleteItem(ISiteSetting siteSetting, IItem item)
        {
            MessageBoxResult result = MessageBox.Show(String.Format(Languages.Translate("Are you sure you would like to delete"), item.Title), Languages.Translate("Information"), MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.No)
                return;

            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.DeleteFile(siteSetting, item);
        }

        public static void OpenItem(ISiteSetting siteSetting, IItem item)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.OpenFile(siteSetting, item);
        }

        public static void OpenFolder(ISiteSetting siteSetting, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            //serviceManager.OpenFile(siteSetting, item);
        }

        public static void EditItemProperties(ISiteSetting siteSetting, object item, Folder folder)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            List<ContentType> contentTypes = serviceManager.GetContentTypes(siteSetting, folder, false);//JD 6/8/13
            //list content types for this item
            List<ContentType> sigleContentTypes = new List<ContentType>();

            string webUrl;
            string listName;
            int listItemID;
            string itemContentType;
            Dictionary<string, string> properties;

            if (item as SPFolder != null)
            {
                webUrl = ((SPFolder)item).GetWebUrl();
                listName = ((SPFolder)item).ListName;
                listItemID = int.Parse(((SPFolder)item).ID);
                itemContentType = ((SPFolder)item).Properties["ContentTypeId"].ToString();
                properties = ((SPFolder)item).Properties;
            }
            else
            {
                webUrl = ((SPListItem)item).WebURL;
                listName = ((SPListItem)item).ListName;
                listItemID = ((SPListItem)item).ID;
                itemContentType = ((SPListItem)item).Properties["ContentTypeId"].ToString();
                properties = ((SPListItem)item).Properties;
            }

            foreach (ContentType contentType in contentTypes)
            {

                if (contentType.Name.Equals(itemContentType) || contentType.ID.Equals(itemContentType))
                {
                    sigleContentTypes.Add(contentType);
                    break;
                }
            }



            EditItemPropertiesControl editControl = new EditItemPropertiesControl(folder.GetWebUrl(), null, sigleContentTypes, null, null, siteSetting, webUrl, properties,true);

            if (editControl.ShowDialog(null, Languages.Translate("Edit Properties")) == true)
            {
                ContentType contentType;
                Dictionary<string, object> values = editControl.GetValues(out contentType);
                serviceManager.UpdateListItem(siteSetting, webUrl, listName, listItemID.ToString(), values, new Dictionary<string, object>());

                if (siteSetting.CheckInAfterEditProperties && editControl.requiredFieldsOk&(item as SPListItem !=null))
                {
                    SPListItem i = ((SPListItem)item);
                    CheckinTypes checktype = siteSetting.useMajorVersionAsDefault ? CheckinTypes.MajorCheckIn : CheckinTypes.MinorCheckIn;
                    serviceManager.CheckInFile(siteSetting, (SPListItem)item, "", checktype);
                }
            }

        }

        public static void ShowVersionHistory(ISiteSetting siteSetting, IItem item)
        {
            ItemVersionHistoryForm itemVersionHistoryForm = new ItemVersionHistoryForm();
            itemVersionHistoryForm.Initialize(siteSetting, item);
            if (itemVersionHistoryForm.ShowDialog(null, "Version History") == true)
            {

            }
        }

        public static SC_MenuItems GetTaskMenuItems(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Workflows.Task task)
        {
            SC_MenuItems menuItems = new SC_MenuItems();
            menuItems.Add(SC_MenuItemTypes.EditTask);
            if (string.IsNullOrEmpty(task.RelatedContentUrl) == false)
            {
                menuItems.Add(SC_MenuItemTypes.OpenTaskDocument);
            }

            return menuItems;
        }

        public static SC_MenuItems GetItemMenuItems(ISiteSetting siteSetting, IItem item)
        {
            SC_MenuItems menuItems = new SC_MenuItems();
            menuItems.Add(SC_MenuItemTypes.OpenItem);
            SC_MenuItem attachMenuItem = new SC_MenuItem(SC_MenuItemTypes.Attach);
            attachMenuItem.SubItems.Add(SC_MenuItemTypes.AttachAsAHyperlink);
            attachMenuItem.SubItems.Add(SC_MenuItemTypes.AttachAsAnAttachment);
            menuItems.Add(attachMenuItem);

            menuItems.Add(SC_MenuItemTypes.Separator);

            menuItems.Add(SC_MenuItemTypes.CopyItem);
            menuItems.Add(SC_MenuItemTypes.Cut);
            if (ItemsManager.GetCopiedItemInfo() != null)
            {
                menuItems.Add(SC_MenuItemTypes.PasteItem);
            }

            menuItems.Add(SC_MenuItemTypes.Separator);

            menuItems.Add(SC_MenuItemTypes.DeleteItem);

            menuItems.Add(SC_MenuItemTypes.Separator);

            if (item as SPListItem != null)
            {
                SPListItem spListItem = item as SPListItem;
                menuItems.Add(SC_MenuItemTypes.Workflow);
                //menuItems.Add(SC_MenuItemTypes.ApproveRejectItem);//not yet implemented
                if (spListItem.CheckoutUser == String.Empty)
                {
                    menuItems.Add(SC_MenuItemTypes.CheckOutItem);
                }
                else
                {
                    menuItems.Add(SC_MenuItemTypes.CheckInItem);
                    menuItems.Add(SC_MenuItemTypes.UndoCheckOutItem);
                }
                menuItems.Add(SC_MenuItemTypes.Separator);
            }

            menuItems.Add(SC_MenuItemTypes.ShowItemVersionHistory);

            SC_MenuItem displayMenuItem = new SC_MenuItem(SC_MenuItemTypes.Display);
            displayMenuItem.SubItems.Add(SC_MenuItemTypes.Inexplorer);
            displayMenuItem.SubItems.Add(SC_MenuItemTypes.Innavigator);
            menuItems.Add(displayMenuItem);

            SC_MenuItem newMenuItem = new SC_MenuItem(SC_MenuItemTypes.New);
            newMenuItem.SubItems.Add(SC_MenuItemTypes.AddFolder);
            menuItems.Add(newMenuItem);

            menuItems.Add(SC_MenuItemTypes.EditItem);
            return menuItems;
        }

        public static SC_MenuItems GetItemVersionMenuItems(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            SC_MenuItems menuItems = new SC_MenuItems();
            menuItems.Add(SC_MenuItemTypes.OpenVersionHistory);
            if (itemVersion.Version.IndexOf("@") == -1)
            {
                menuItems.Add(SC_MenuItemTypes.RollbackVersionHistory);
            }
            return menuItems;
        }

        public static void OpenWorkflowDialog(ISiteSetting siteSetting, Folder list, IItem item)
        {
            SPList spList = (SPList)list;
            string url = spList.WebUrl + "/_layouts/Workflow.aspx?IsDlg=1&ID=" + item.GetID() + "&List=" + spList.ID.ToString();
            string source = HttpUtility.UrlEncode(url + "&SBSBrowserAction=Close");
            url += "&Source=" + source;
            BrowserExplorer browserExplorer = new BrowserExplorer();
            browserExplorer.Initialize(siteSetting, url);
            if (browserExplorer.ShowDialog(null, Languages.Translate("Workflows"), 600, 800, false, false) == true)
            {
            }

            //http://demo.sobiens.com/Sobiens.Connectors/_layouts/Workflow.aspx?IsDlg=1&ID=27&List={71F45CA4-389A-43C9-B600-4832AC2FA425}
            //ItemWorkflowForm itemWorkflowForm = new ItemWorkflowForm();
            //itemWorkflowForm.Initialize(siteSetting, item);
            //if (itemWorkflowForm.ShowDialog(null, "Workflow") == true)
            //{
            //}
        }

        public static void OpenWorkflowDialog(string itemUrl)
        {
            ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetProbableSiteSetting(itemUrl);
            if (siteSetting == null)
            {
                throw new Exception(string.Format(Languages.Translate("SiteSetting match could not be found for item:{0}"), itemUrl));
            }
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Folder itemFolder;
            IItem item = serviceManager.GetItem(siteSetting, itemUrl, out itemFolder);
            Folder workflowFolder = serviceManager.GetWorkflowFolder(siteSetting, itemFolder, itemUrl);
            OpenWorkflowDialog(siteSetting, itemFolder, item);
        }

        public static void OpenVersionHistory(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            Process.Start("IExplore.exe", itemVersion.URL);
        }

        public static void RollbackVersion(ISiteSetting siteSetting, ItemVersion itemVersion)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            serviceManager.RestoreVersion(siteSetting, itemVersion);
        }

        public static bool CheckInItem(ISiteSetting siteSetting, IItem item, IConnectorExplorer connector)
        {

            CheckInForm checkInForm = new CheckInForm();
            checkInForm.Initialize(siteSetting, item);
            if (checkInForm.ShowDialog(null, Languages.Translate("Check In Item")) == true)
            {
                string comment = checkInForm.Comments;
                CheckinTypes checkinType = checkInForm.CheckInType;
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                Result result;
                if (result = serviceManager.CheckInFile(siteSetting, item, comment, checkinType))
                {

                    if (checkInForm.KeepDocumentCheckedOut == true)
                    {
                        serviceManager.CheckOutFile(siteSetting, item);
                    }
                    else
                    {
                        if (connector != null)
                        {
                            item.Properties["ows__Level"] = "1";
                            //connector.ContentExplorer.UpdateItem(item.GetID(), item);

                        }
                    }
                }
                else
                {
                    MessageBox.Show(result.detailResult, result.messageResult, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }

        public static bool CheckOutItem(ISiteSetting siteSetting, IItem item, IConnectorExplorer connector)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Result result = serviceManager.CheckOutFile(siteSetting, item);
            if (result)
                if (connector != null)
                {
                    item.Properties["ows__Level"] = "255";
                    //connector.ContentExplorer.UpdateItem(item.GetID(), item);

                }
                else
                {
                    MessageBox.Show(result.detailResult, result.messageResult, MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            return true;
        }

        public static bool UndoCheckOutItem(ISiteSetting siteSetting, IItem item, IConnectorExplorer connector)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Result result = serviceManager.UndoCheckOutFile(siteSetting, item);
            if (result) item.Properties["ows__Level"] = "1";
            else
            {
                MessageBox.Show(result.detailResult, result.messageResult, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            //connector.ContentExplorer.UpdateItem(item.GetID(), item);
            return true;
        }

        public static void ApproveRejectItem(ISiteSetting siteSetting, IItem item)
        {
            MessageBox.Show(Languages.Translate("Not Implemented"));
        }

        public static void OpenTaskDocument(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Workflows.Task task)
        {
            ApplicationContext.Current.OpenFile(siteSetting, task.RelatedContentUrl);
        }

        public static void EditTask(ISiteSetting siteSetting, Sobiens.Connectors.Entities.Workflows.Task task)
        {
            string url = task.ListUrl + "/DispForm.aspx?ID=" + task.ID + "&IsDlg=1";
            BrowserExplorer browserExplorer = new BrowserExplorer();
            browserExplorer.Initialize(siteSetting, url);
            if (browserExplorer.ShowDialog(null, Languages.Translate("Edit Task"), 600, 800, false, false) == true)
            {
                //this.RefreshControls(true);
            }
        }

        public static void BindSearchResultsToListViewControl(ISiteSetting siteSetting, List<IItem> items, object LibraryContentDataGridView)
        {
            DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                libraryContentDataGridView.Columns.Clear();
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
                urlColumn.Visibility = Visibility.Hidden;
                //urlColumn.AllowAutoFilter = true;
                libraryContentDataGridView.Columns.Add(urlColumn);

                libraryContentDataGridView.Tag = ds;
                libraryContentDataGridView.ItemsSource = dt.AsDataView();
            }));
        }

        public static void BindFoldersToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, List<Folder> folders, object LibraryContentDataListView, object LibraryContentDataGridView)
        {
            ListView libraryContentDataListView = (ListView)LibraryContentDataListView;
            GridView libraryContentDataGridView = (GridView)LibraryContentDataGridView;

            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataSet ds = libraryContentDataListView.Tag as DataSet;
                if (ds == null)
                {
                    ds = GetBaseDataSet();
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

                SetBaseGridColumns(libraryContentDataListView, libraryContentDataGridView);
                libraryContentDataListView.ItemsSource = ds.Tables[0].AsDataView();
            }));
        }

        public static void AddTempItemInGrid(string id, string title, ListView libraryContentDataGridView)
        {
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataTable dt = ((DataView)libraryContentDataGridView.ItemsSource).Table;
                OC_Datarow newRow = (OC_Datarow)dt.NewRow();
                newRow["ID"] = id;
                newRow["Title"] = title;
                newRow["Picture"] = "/Sobiens.Connectors.WPF.Controls;component/Images/loader.GIF";
                dt.Rows.Add(newRow);
            }));
        }

        public static void BindItemsToListViewControl(ISiteSetting siteSetting, Folder parentFolder, Folder folder, IView view, List<Folder> folders, List<IItem> items, object LibraryContentDataListView, object LibraryContentDataGridView)
        {
            ListView libraryContentDataListView = (ListView)LibraryContentDataListView;
            GridView libraryContentDataGridView = (GridView)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataSet ds = libraryContentDataListView.Tag as DataSet;
                if (ds == null)
                {
                    ds = GetBaseDataSet();
                }
                else
                {
                    RemoveAdditionalColumns(ds);
                }

                if (folders != null)
                {
                    RemoveFolders(ds);
                    AddFolders(parentFolder, ds, folders);
                }

                if (items != null)
                {
                    RemoveItems(ds);
                    AddItems(ds, items);
                }

                SetBaseGridColumns(libraryContentDataListView, libraryContentDataGridView);
                /*
                foreach (CamlFieldRef viewField in view.ViewFields)
                {
                    DataGridTextColumn textColumn = new DataGridTextColumn(); 
                    textColumn.Header = viewField.Name; 
                    textColumn.Binding = new Binding(viewField.Name); 
                    libraryContentDataGridView.Columns.Add(textColumn); 
                }
                 */
                libraryContentDataListView.ItemsSource = ds.Tables[0].AsDataView();
            }));
        }

        private static void SetBaseGridColumns(ListView listView, GridView gridView)
        {
            gridView.Columns.Clear();

            GridViewColumn col1 = new GridViewColumn();
            col1.Header = Languages.Translate("Icon");
            FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));
            Binding b1 = new Binding("Picture");
            b1.Mode = BindingMode.TwoWay;
            factory1.SetValue(Image.SourceProperty, b1);
            DataTemplate cellTemplate1 = new DataTemplate();
            cellTemplate1.VisualTree = factory1;
            col1.CellTemplate = cellTemplate1;
            gridView.Columns.Add(col1);

            GridViewColumn titleColumn = new GridViewColumn();
            titleColumn.Header = Languages.Translate("Title");
            titleColumn.DisplayMemberBinding = new Binding("Title");
            gridView.Columns.Add(titleColumn);

            GridViewColumn urlColumn = new GridViewColumn();
            urlColumn.Header = Languages.Translate("URL");
            urlColumn.DisplayMemberBinding = new Binding("URL");
            urlColumn.Width = 0;
            //urlColumn.Visibility = Visibility.Hidden;
            gridView.Columns.Add(urlColumn);

            GridViewColumn modifiedByColumn = new GridViewColumn();
            DataTemplate cellTemplate2 = new DataTemplate();

            modifiedByColumn.Header = Languages.Translate("Modified By");
            FrameworkElementFactory modifiedByPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            modifiedByPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory presenceFactory = new FrameworkElementFactory(typeof(PresenceIndicator));
            Binding b2 = new Binding("ModifiedBySIP");
            b2.Mode = BindingMode.TwoWay;
            presenceFactory.SetValue(PresenceIndicator.SourceProperty, b2);
            presenceFactory.SetValue(PresenceIndicator.WidthProperty, (double)10);
            presenceFactory.SetValue(PresenceIndicator.HeightProperty, (double)10);

            FrameworkElementFactory modifiedByLabelFactory = new FrameworkElementFactory(typeof(Label));
            Binding b3 = new Binding("ModifiedBy");
            b3.Mode = BindingMode.TwoWay;
            modifiedByLabelFactory.SetValue(Label.ContentProperty, b3);

            modifiedByPanelFactory.AppendChild(presenceFactory);
            modifiedByPanelFactory.AppendChild(modifiedByLabelFactory);

            cellTemplate2.VisualTree = modifiedByPanelFactory;
            modifiedByColumn.CellTemplate = cellTemplate2;
            gridView.Columns.Add(modifiedByColumn);

            //PresenceIndicator presenceIndicator = new PresenceIndicator();
        }

        private static void AddFolders(Folder parentFolder, DataSet dataset, List<Folder> folders)
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
                if (subfolder.Selected == false)
                    continue;

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

        public static void UpdateUploadItemErrorInGrid(string id, IItem item, object LibraryContentDataGridView)
        {
            //DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;//JD
            ListView libraryContentDataGridView = (ListView)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataTable dt = ((DataView)libraryContentDataGridView.ItemsSource).Table;
                DataRow[] result = dt.Select("ID = '" + id.ToString() + "'");
                if (result.Count() > 0)
                {
                    OC_Datarow row = (OC_Datarow)result[0];
                    row["Picture"] = "/Sobiens.Connectors.WPF.Controls;component/Images/error.GIF";
                }
            }));
        }

        public static void UpdateUploadItemInGrid(string id, IItem item, object LibraryContentDataGridView)
        {
            //DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;//JD
            ListView libraryContentDataGridView = (ListView)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataTable dt = ((DataView)libraryContentDataGridView.ItemsSource).Table;
                DataRow[] result = dt.Select("ID = '" + id.ToString() + "'");
                if (result.Count() > 0)
                {
                    OC_Datarow row = (OC_Datarow)result[0];
                    UpdateItemProperties(row, item);
                }
            }));
        }

        public static void UpdateItem(string id, IItem item, object LibraryContentDataGridView)
        {
            //DataGrid libraryContentDataGridView = (DataGrid)LibraryContentDataGridView;//JD
            ListView libraryContentDataGridView = (ListView)LibraryContentDataGridView;
            libraryContentDataGridView.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                DataTable dt = ((DataView)libraryContentDataGridView.ItemsSource).Table;

                DataRow[] result = dt.Select("ID = '" + id.ToString() + "'");
                if (result.Count() > 0)
                {
                    OC_Datarow row = (OC_Datarow)result[0];
                    UpdateItemProperties(row, item);
                }
            }));
        }

        public static void UpdateItemProperties(OC_Datarow row, IItem item)
        {
            row.Tag = item;
            string extensionName = item.URL.Substring(item.URL.LastIndexOf('.') + 1);
            row["Picture"] = ImageManager.GetInstance().GetExtensionImageFromResource(extensionName, item.isExtracted());
            row["ID"] = item.UniqueIdentifier;
            row["Title"] = item.Title;
            row["URL"] = item.URL;
            string modifiedBy = item.Properties["Editor"].ToString();
            string modifiedBySIP = "sip:serkants@tesl.com";
            row["ModifiedBy"] = Sobiens.Connectors.Common.tools.keepBehind(modifiedBy, ";#");
            row["ModifiedBySIP"] = modifiedBySIP;
        }

        private static void AddItems(DataSet dataset, List<IItem> items)
        {
            OC_DataTable table = dataset.Tables[0] as OC_DataTable;
            foreach (IItem item in items)
            {
                OC_Datarow newRow = (OC_Datarow)table.NewRow();
                UpdateItemProperties(newRow, item);
                table.Rows.Add(newRow);
            }
        }

        private static void RemoveItems(DataSet dataset)
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

        private static void RemoveFolders(DataSet dataset)
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

        private static DataSet GetBaseDataSet()
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
            DataColumn dtModifiedBySIPColumn = dt.Columns.Add("ModifiedBySIP");
            dtModifiedBySIPColumn.Caption = Languages.Translate("Modified By");
            DataColumn dtURLColumn = dt.Columns.Add("URL");
            return ds;
        }

        private static void RemoveAdditionalColumns(DataSet dataset)
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


    }
}
