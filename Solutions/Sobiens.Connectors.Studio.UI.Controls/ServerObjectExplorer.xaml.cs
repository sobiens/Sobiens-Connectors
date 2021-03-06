﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Studio.UI.Controls.Settings;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.SQLServer;
using Sobiens.Connectors.Entities.Cryptography;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    public delegate void FoldersTreeViewControlAfter_Select(Folder parentFolder, Folder folder);

    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class ServerObjectExplorer : HostControl, IServerObjectExplorer
    {
        private const int LoadingNodeTagValue = -1;
        public event FoldersTreeViewControlAfter_Select After_Select;

        public bool ShowConnectMenu { get; set; }
        private List<Folder> _SelectedObjects = new List<Folder>();
        public List<Folder> SelectedObjects {
            get
            {
                return _SelectedObjects;
            }
        }

        public List<Folder> AllObjects
        {
            get
            {
                List<Folder> folders = new List<Folder>();
                PopulateFoldersFromTreeNode(folders, FoldersTreeView.Items);
                return folders;
            }
        }

        private void PopulateFoldersFromTreeNode(List<Folder> folders, ItemCollection treeviewItems)
        {
            foreach(object nodeObject in treeviewItems)
            {
                Folder subfolder = ((TreeViewItem)nodeObject).Tag as Folder;
                if (subfolder != null)
                {
                    folders.Add(subfolder);
                    PopulateFoldersFromTreeNode(subfolder.Folders, ((TreeViewItem)nodeObject).Items);
                }
            }
        }

        private List<Folder> DataSource { get; set; }
        public bool ShowCheckBoxes { get; set; }

        public bool _ShowPropertiesPanel = true;
        public bool ShowPropertiesPanel
        {
            get
            {
                return _ShowPropertiesPanel;
            }
            set
            {
                _ShowPropertiesPanel = value;
            }
        }
        public bool AdministrativeView { get; set; }
        new public bool Initialized { get; set; }
        public int[] IncludedFolderTypes { get; set; }

        public ISPCamlStudio ConnectorExplorer
        {
            get;
            private set;
        }

        public void SetConnectorExplorer(ISPCamlStudio connectorExplorer)
        {
            this.ConnectorExplorer = connectorExplorer;
        }

        public Folder SelectedObject
        {
            get
            {
                if (FoldersTreeView.SelectedItem != null && ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as Folder != null)
                    return (Folder)((TreeViewItem)FoldersTreeView.SelectedItem).Tag;

                return null;
            }
        }


        public bool HasAnythingToDisplay
        {
            get
            {
                return FoldersTreeView.Items.Count>0?true:false;
            }
        }


        public ServerObjectExplorer()
        {
            ShowConnectMenu = true;
            InitializeComponent();
        }

        public void RefreshControls()
        {
            throw new Exception(Languages.Translate("Not implemented yet"));
        }

        public void SelectFolder(Folder folder)
        {
            throw new Exception(Languages.Translate("Not implemented yet"));
        }

        /*
        public void Initialize(List<Folder> dataSource, int[] includedFolderTypes)
        {
            SiteSettings siteSettings = new SiteSettings();
            siteSettings.Add(siteSetting);
            this.Initialize(dataSource, includedFolderTypes);
        }
         */ 

        public void Initialize(List<Folder> dataSource, int[] includedFolderTypes)
        {
            this.IncludedFolderTypes = includedFolderTypes;
            this.DataSource = dataSource;
            FoldersTreeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(rootNode_Expanded));
        }

        public void RefreshNodes()
        {
            FoldersTreeView.Items.Clear();

            if (this.DataSource != null)
            {
                foreach (Folder folder in this.DataSource)
                {
                    AddNode(FoldersTreeView.Items, folder, folder.SiteSettingID);
                }
            }
        }

        private void AddNode(ItemCollection itemCollection, Folder folder, Guid siteSettingID)
        {
            if (folder.PublicFolder == true && AdministrativeView == false && folder.Selected == false)
            {
                return;
            }

            TreeViewItem rootNode = new TreeViewItem();

            DockPanel treenodeDock = new DockPanel();

            DockPanel folderTitleDock = new DockPanel();
            Image img = new Image();
            Uri uri = new Uri("/Sobiens.Connectors.Studio.UI.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;

            Label lbl = new Label();
            string title = folder.Title;
            if(folder as SPWeb != null && string.IsNullOrEmpty(folder.GetUrl()) == false && title.Equals(folder.GetUrl(), StringComparison.InvariantCultureIgnoreCase) == false)
                title = folder.Title + " - " + folder.GetUrl();
            lbl.Content = title;
            lbl.ToolTip = title;

            folderTitleDock.Children.Add(img);
            folderTitleDock.Children.Add(lbl);

            if (ShowCheckBoxes == true)
            {
                CheckBox chkBox = new CheckBox();
                chkBox.Margin = new Thickness(0, 0, 0, 0);
                chkBox.IsChecked = false;
                chkBox.Content = folderTitleDock;
                chkBox.IsChecked = folder.Selected;
                chkBox.Tag = folder;
                chkBox.Checked += new RoutedEventHandler(chkBox_Checked);
                chkBox.Unchecked += new RoutedEventHandler(chkBox_Unchecked);
                treenodeDock.Children.Add(chkBox);
            }
            else
            {
                treenodeDock.Children.Add(folderTitleDock);
            }

            rootNode.Header = treenodeDock;
            //rootNode.Expanded += new RoutedEventHandler(rootNode_Expanded);
            rootNode.Tag = folder;
            itemCollection.Add(rootNode);
            if(folder as SPWeb != null)
            {
                AddHeadingNode(rootNode, folder, "Lists and Libraries", true);
                AddHeadingNode(rootNode, folder, "Workflows", true);
                //AddHeadingNode(rootNode, folder, "Site Pages", true);
                //AddHeadingNode(rootNode, folder, "Site Assets", true);
                AddHeadingNode(rootNode, folder, "Content Types", true);
                AddHeadingNode(rootNode, folder, "Site Columns", true);
                AddHeadingNode(rootNode, folder, "Site Groups", true);
                AddHeadingNode(rootNode, folder, "Subsites", true);
                return;
            }

            if (folder as SQLDB != null)
            {
                AddHeadingNode(rootNode, folder, "Tables", true);
                AddHeadingNode(rootNode, folder, "Views", true);
                AddHeadingNode(rootNode, folder, "Functions", true);
                AddHeadingNode(rootNode, folder, "Stored Procedures", true);
                return;
            }

            if (folder as CRMWeb != null)
            {
                AddHeadingNode(rootNode, folder, "Entities", true);
                AddHeadingNode(rootNode, folder, "Dashboards", true);
                AddHeadingNode(rootNode, folder, "Option Sets", true);
                AddHeadingNode(rootNode, folder, "Processes", true);
                AddHeadingNode(rootNode, folder, "Plug-in Assemblies", true);
                AddHeadingNode(rootNode, folder, "Teams", true);
                AddHeadingNode(rootNode, folder, "Business Units", true);
                AddHeadingNode(rootNode, folder, "Security Roles", true);
                return;
            }

            if (folder.Folders.Count == 0)
            {
                AddLoadingNode(rootNode);
            }

            foreach(Folder subfolder in folder.Folders)
            {
                AddNode(rootNode.Items, subfolder, subfolder.SiteSettingID);
            }

            if (folder.Folders.Count > 0)
            {
                rootNode.IsExpanded = true;
            }

        }

        void chkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = e.Source as CheckBox;
            Folder folder = checkBox.Tag as Folder;
            folder.Selected = false;
            TreeViewItem p = (TreeViewItem)((DockPanel)checkBox.Parent).Parent;
            Folder parentFolder = null;
            if (p.Parent as TreeView == null)
            {

            }

            _SelectedObjects.Remove(folder);
            if(After_Select != null)
                After_Select(parentFolder, folder);
        }

        void chkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = e.Source as CheckBox;
            Folder folder = checkBox.Tag as Folder;
            folder.Selected = true;
            TreeViewItem p = (TreeViewItem)((DockPanel)checkBox.Parent).Parent;
            Folder parentFolder = null;
            if (p.Parent as TreeView == null)
            {

            }

            _SelectedObjects.Add(folder);
            if (After_Select != null)
                After_Select(parentFolder, folder);
        }

        /// <summary>
        /// Refreshes the node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void RefreshNode(TreeViewItem item, object _object, string headerText)
        {
            Folder folder = null;
            item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                folder = (Folder)_object;
            }));

            {
                string errorMessage = string.Empty;
                SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[folder.SiteSettingID];
                List<Folder> subFolders = null;
                try
                {
                    subFolders = ApplicationContext.Current.GetSubFolders(siteSetting, folder, this.IncludedFolderTypes, headerText);
                }
                catch(Exception ex)
                {
                    errorMessage = ex.Message;
                }
                if (string.IsNullOrEmpty(errorMessage) == true)
                {
                    folder.Folders = subFolders;
                    item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                    {
                        FoldersTreeView.BeginInit();
                        item.Items.Clear();
                        foreach (Folder subFolder in subFolders)
                        {
                            subFolder.PublicFolder = this.AdministrativeView;
                            subFolder.Selected = false;
                            AddNode(item.Items, subFolder, siteSetting.ID);
                        }
                        FoldersTreeView.EndInit();
                    }));
                }
                else
                {
                    item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                    {
                        FoldersTreeView.BeginInit();
                        item.Items.Clear();
                        TreeViewItem loadingTreeViewItem = new TreeViewItem();
                        loadingTreeViewItem.Header = "Failed";
                        loadingTreeViewItem.Tag = "Failed";
                        loadingTreeViewItem.Foreground = Brushes.Red;
                        loadingTreeViewItem.ToolTip = "Failed:" + errorMessage;
                        item.Items.Add(loadingTreeViewItem);
                    }));
                }
            }
        }

        void rootNode_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.Source as TreeViewItem;
            if (item.Items.Count == 1)
            {
                TreeViewItem subitem = item.Items[0] as TreeViewItem;
                if (subitem.Tag.GetType() != typeof(int) || ((int)subitem.Tag) != LoadingNodeTagValue)
                {
                    return;
                }
            }
            else
            {
                return;
            }

            WorkItem workItem = new WorkItem(Languages.Translate("Populating treeview items"));
            workItem.CallbackFunction = new WorkRequestDelegate(callback);
            workItem.CallbackData = new object[] { item, item.Tag, item.Header };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void callback(object item, DateTime dateTime)
        {
            object[] args = (object[])item;

            RefreshNode((TreeViewItem)args[0], args[1], args[2].ToString());
        }

        private bool IsLoadingNode(TreeViewItem node)
        {
            if (node.Tag.GetType() == typeof(int) && ((int)node.Tag) == LoadingNodeTagValue)
                return true;
            return false;
        }

        private void AddLoadingNode(TreeViewItem node)
        {
            TreeViewItem childNode = new TreeViewItem();
            childNode.Header = Languages.Translate("Loading...");
            childNode.Tag = LoadingNodeTagValue;
            node.Items.Add(childNode);
        }

        private TreeViewItem AddHeadingNode(TreeViewItem parentNode, object parentObject, string headerText, bool addLoadingNode)
        {
            TreeViewItem childNode = new TreeViewItem();
            childNode.Header = headerText;
            childNode.Tag = parentObject;
            parentNode.Items.Add(childNode);

            if(addLoadingNode == true)
            {
                AddLoadingNode(childNode);
            }

            return childNode;
        }


        private void FoldersTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FoldersTreeView.SelectedItem == null || After_Select == null)
                return;

            TreeViewItem tempNode = (TreeViewItem)FoldersTreeView.SelectedItem;
            Folder folder = ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as Folder;

            ObjectPropertiesControl.Initialize(folder);
            ObjectPropertiesControl.PopulateControls();

            if (After_Select != null)
                After_Select(null, folder);

        }

        public void Initialize() 
        {
            List<Sobiens.Connectors.Entities.Folder> folders = new List<Sobiens.Connectors.Entities.Folder>();
            foreach (SiteSetting siteSetting in ApplicationContext.Current.Configuration.SiteSettings)
            {
                Folder folder = null;
                if (siteSetting.SiteSettingType == SiteSettingTypes.SharePoint)
                {
                    if (string.IsNullOrEmpty(siteSetting.Parameters) == true)
                    {
                        try
                        {
                            folder = new Services.SharePoint.SharePointService().GetWeb(siteSetting, siteSetting.Url);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    else { 
                        string[] parameters = siteSetting.Parameters.Split(new string[] { "#;" }, StringSplitOptions.None);
                        string webTitle = parameters[0];
                        string webId = parameters[1];
                        string siteUrl = parameters[2];
                        string webUrl = parameters[3];
                        string serverRelativePath = parameters[4];

                        folder = new SPWeb(webUrl, webTitle, siteSetting.ID, webId, siteUrl, webUrl, serverRelativePath);
                    }
                }
                else if (siteSetting.SiteSettingType == SiteSettingTypes.SQLServer)
                {
                    folder = new Entities.SQLServer.SQLServer(siteSetting.Url, siteSetting.ID, Guid.NewGuid().ToString());
                }
                else if (siteSetting.SiteSettingType == SiteSettingTypes.CRM)
                {
                    folder = new CRMWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, Guid.NewGuid().ToString(), siteSetting.Url, siteSetting.Url);
                }
                folder.Selected = false;
                folders.Add(folder);
            }
            this.Initialize(folders, null);
            this.RefreshNodes();

            this.FoldersTreeView.ContextMenu = new ContextMenu();
        }

        private void FoldersTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TreeViewItem item = FindControls.FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (item == null)
            {
                return;
            }

            this.FoldersTreeView.ContextMenu.Items.Clear();
            TreeViewItem parentItem = item.Parent as TreeViewItem;
            if (parentItem == null)
            {
                MenuItem disconnectMenuItem = new MenuItem(){Header="Disconnect", Tag = item};
                disconnectMenuItem.Click += disconnectMenuItem_Click;
                this.FoldersTreeView.ContextMenu.Items.Add(disconnectMenuItem);
            }
            if (item.Tag as SPFolder != null || item.Tag as CRMEntity != null || item.Tag as SQLTable != null) 
            {
                MenuItem newQueryMenuItem = new MenuItem() { Header = "New Query", Tag = item };
                newQueryMenuItem.Click += newQueryMenuItem_Click;
                this.FoldersTreeView.ContextMenu.Items.Add(newQueryMenuItem);

                /*
                MenuItem objectDetailsMenuItem = new MenuItem() { Header = "Properties", Tag = item };
                objectDetailsMenuItem.Click += ObjectDetailsMenuItem_Click; ; ;
                this.FoldersTreeView.ContextMenu.Items.Add(objectDetailsMenuItem);
                */

                /*
                MenuItem createMenuItem = new MenuItem() { Header = "Create", Tag = item };
                createMenuItem.Click += CreateMenuItem_Click; ;
                this.FoldersTreeView.ContextMenu.Items.Add(createMenuItem);
                */
            }

            if (item.Tag as SPWeb != null)
            {
                MenuItem exportMenuItem = new MenuItem() { Header = "Export", Tag = item };
                exportMenuItem.Click += exportMenuItem_Click;
                this.FoldersTreeView.ContextMenu.Items.Add(exportMenuItem);
            }

            if (item.Tag == "Failed")
            {
                MenuItem retryMenuItem = new MenuItem() { Header = "Retry", Tag = item };
                retryMenuItem.Click += RetryMenuItem_Click;
                this.FoldersTreeView.ContextMenu.Items.Add(retryMenuItem);
            }

            if (item.Tag as SPWeb != null || item.Tag as CRMWeb != null || item.Tag as SQLDB != null
                || item.Tag as SQLTable != null || item.Tag as SPList != null || item.Tag as CRMEntity != null)
            {
                MenuItem compareMenuItem = new MenuItem() { Header = "Compare", Tag = item };
                compareMenuItem.Click += compareMenuItem_Click;
                this.FoldersTreeView.ContextMenu.Items.Add(compareMenuItem);
            }

        }

        private void RetryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            Folder sourceObject = ((TreeViewItem)menuItem.Tag).Tag as Folder;
            TreeViewItem parentNode = ((TreeViewItem)menuItem.Tag).Parent as TreeViewItem;
            RoutedEventArgs e1 = new RoutedEventArgs();
            e1.RoutedEvent = UIElement.MouseLeftButtonDownEvent;
            e1.Source = parentNode;
            parentNode.Items.Clear();
            AddLoadingNode(parentNode);
            rootNode_Expanded(null, e1);
        }

        private void ObjectDetailsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            Folder sourceObject = ((TreeViewItem)menuItem.Tag).Tag as Folder;
            /*
            ObjectPropertiesForm objectPropertiesForm = new ObjectPropertiesForm();
            objectPropertiesForm.Initialize(sourceObject);
            if (objectPropertiesForm.ShowDialog(this.ParentWindow, "Object Properties") == true)
            {
            }
            */
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        void compareMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            Folder sourceObject = ((TreeViewItem)menuItem.Tag).Tag as Folder;

            SelectEntityForm selectEntityForm = new SelectEntityForm();
            Type[] allowedTypes = null;
            if (sourceObject as SPWeb != null) {
                allowedTypes = new Type[] { typeof(SPWeb) };
            }
            else if (sourceObject as CRMWeb != null)
            {
                allowedTypes = new Type[] { typeof(CRMWeb) };
            }
            else if (sourceObject as SQLDB != null)
            {
                allowedTypes = new Type[] { typeof(SQLDB) };
            }
            else if (sourceObject as SQLTable != null)
            {
                allowedTypes = new Type[] { typeof(SQLTable) };
            }
            else if (sourceObject as SPList != null)
            {
                allowedTypes = new Type[] { typeof(SPList) };
            }
            else if (sourceObject as CRMEntity != null)
            {
                allowedTypes = new Type[] { typeof(CRMEntity) };
            }
            selectEntityForm.Initialize(allowedTypes);

            if (selectEntityForm.ShowDialog(this.ParentWindow, "Select an object to compare with") == true)
            {
                Folder objectToCompareWith = selectEntityForm.SelectedObject;
                CompareWizardForm compareWizardForm = new CompareWizardForm();
                compareWizardForm.Initialize(sourceObject, objectToCompareWith);
                compareWizardForm.ShowDialog(this.ParentWindow, "Compare Wizard", false, true);
            }
        }

        void exportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            Folder selectedFolder = ((TreeViewItem)menuItem.Tag).Tag as Folder;

            ExportWizardForm exportWizardForm = new ExportWizardForm();
            exportWizardForm.Initialize(selectedFolder);
            if (exportWizardForm.ShowDialog(this.ParentWindow, "Export Wizard") == true)
            {
            }
        }

        void newQueryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            Folder selectedFolder = ((TreeViewItem)menuItem.Tag).Tag as Folder;

            ApplicationContext.Current.AddNewQueryPanel(selectedFolder, null);
            ApplicationContext.Current.SPCamlStudio.QueryDesignerToolbar.ValidateButtonEnabilities();
        }

        void disconnectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            Folder selectedWeb = ((TreeViewItem)menuItem.Tag).Tag as Folder;

            for (int i = this.FoldersTreeView.Items.Count - 1; i > -1; i--) 
            {
                Folder web = ((TreeViewItem)this.FoldersTreeView.Items[i]).Tag as Folder;
                if (web.SiteSettingID == selectedWeb.SiteSettingID)
                    this.FoldersTreeView.Items.RemoveAt(i);
            }

            ConfigurationManager.GetInstance().Configuration.SiteSettings.Remove(selectedWeb.SiteSettingID);
            ConfigurationManager.GetInstance().SaveAppConfiguration();
            ApplicationContext.Current.Configuration.SiteSettings = ConfigurationManager.GetInstance().Configuration.SiteSettings;
        }

        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;

        }

        private Dictionary<object, object> getFieldMappings(string webURL, List<ApplicationItemProperty> properties, List<ContentType> contentTypes, FolderSettings folderSettings, FolderSetting defaultFolderSetting, ISiteSetting siteSetting, string rootFolder, out ContentType contentType,bool displayFileName)
        {
            Dictionary<object, object> mappings = new Dictionary<object, object>();

            EditItemPropertiesControl editItemPropertiesControl = new EditItemPropertiesControl(webURL, properties, contentTypes, folderSettings, defaultFolderSetting, siteSetting, rootFolder, null,displayFileName);
            bool? dialogResult = editItemPropertiesControl.ShowDialog(null, Languages.Translate("Mappings..."));

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

        private void SharePointConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = new SiteSetting();
            siteSetting.ID = Guid.NewGuid();
            siteSetting.SiteSettingType = SiteSettingTypes.SharePoint;
            SiteSettingForm siteSettingForm = new SiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this.ParentWindow, Languages.Translate("Site Settings")) == true)
            {
                string encryptedPassword = AesOperation.EncryptString(siteSetting.Password);
                siteSetting.Password = encryptedPassword;

                ConfigurationManager.GetInstance().Configuration.SiteSettings.Add(siteSetting);
                ConfigurationManager.GetInstance().SaveAppConfiguration();
                ApplicationContext.Current.Configuration.SiteSettings = ConfigurationManager.GetInstance().Configuration.SiteSettings;
                /*
                List<Sobiens.Connectors.Entities.Folder> folders = new List<Sobiens.Connectors.Entities.Folder>();
                SPWeb folder = new SPWeb(siteSetting.Url, siteSetting.Url, siteSetting.ID, Guid.NewGuid().ToString(), siteSetting.Url, siteSetting.Url, siteSetting.Url);
                folder.Selected = false;
                folders.Add(folder);
                this.Initialize(folders, null);
                */
                this.Initialize();
                this.RefreshNodes();
            }
        }

        private void CRMConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = new SiteSetting();
            siteSetting.ID = Guid.NewGuid();
            siteSetting.SiteSettingType = SiteSettingTypes.CRM;
            CRMSiteSettingForm siteSettingForm = new CRMSiteSettingForm();
            siteSettingForm.BindControls(siteSetting);
            if (siteSettingForm.ShowDialog(this.ParentWindow, Languages.Translate("Site Settings")) == true)
            {
                string encryptedPassword = AesOperation.EncryptString(siteSetting.Password);
                siteSetting.Password = encryptedPassword;
                ConfigurationManager.GetInstance().Configuration.SiteSettings.Add(siteSetting);
                ConfigurationManager.GetInstance().SaveAppConfiguration();
                ApplicationContext.Current.Configuration.SiteSettings = ConfigurationManager.GetInstance().Configuration.SiteSettings;
                this.Initialize();
                this.RefreshNodes();
            }
        }

        private void SQLServerConnectButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting siteSetting = new SiteSetting();
            siteSetting.ID = Guid.NewGuid();
            siteSetting.SiteSettingType = SiteSettingTypes.SQLServer;
            SQLServerSettingForm sqlServerSettingForm = new SQLServerSettingForm();
            sqlServerSettingForm.BindControls(siteSetting);
            if (sqlServerSettingForm.ShowDialog(this.ParentWindow, Languages.Translate("Site Settings")) == true)
            {
                string encryptedPassword = AesOperation.EncryptString(siteSetting.Password);
                siteSetting.Password = encryptedPassword;

                ConfigurationManager.GetInstance().Configuration.SiteSettings.Add(siteSetting);
                ConfigurationManager.GetInstance().SaveAppConfiguration();
                ApplicationContext.Current.Configuration.SiteSettings = ConfigurationManager.GetInstance().Configuration.SiteSettings;
                this.Initialize();
                this.RefreshNodes();
            }
        }

        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (ShowConnectMenu == false)
            {
                ConnectMenu.Visibility = Visibility.Collapsed;
            }

            if(ShowPropertiesPanel == false)
            {
                ShowPropertiesButton.Visibility = Visibility.Hidden;
                ObjectExplorerGridSplitter.Visibility = Visibility.Hidden;
                ObjectPropertiesControl.Visibility = Visibility.Hidden;
                if (MainGrid.RowDefinitions.Count == 3)
                {
                    MainGrid.RowDefinitions.RemoveAt(2);
                    MainGrid.RowDefinitions.RemoveAt(1);
                }
            }
        }

        private void ShowPropertiesButton_Click(object sender, RoutedEventArgs e)
        {
            if (ObjectExplorerGridSplitter.Visibility == Visibility.Visible) {
                ObjectExplorerGridSplitter.Visibility = Visibility.Hidden;
                ObjectPropertiesControl.Visibility = Visibility.Hidden;
                MainGrid.RowDefinitions[0].Height = new GridLength(100, GridUnitType.Star);
                MainGrid.RowDefinitions[1].Height = new GridLength(0, GridUnitType.Star);
                MainGrid.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Star);
            }
            else
            {
                ObjectExplorerGridSplitter.Visibility = Visibility.Visible;
                ObjectPropertiesControl.Visibility = Visibility.Visible;
                MainGrid.RowDefinitions[0].Height = new GridLength(50, GridUnitType.Star);
                MainGrid.RowDefinitions[1].Height = new GridLength(5, GridUnitType.Pixel);
                MainGrid.RowDefinitions[2].Height = new GridLength(50, GridUnitType.Star);
            }
        }
    }
}
