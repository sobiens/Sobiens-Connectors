using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sobiens.Connectors.Services.SharePoint;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Common.SharePoint;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;
using Sobiens.Connectors.Entities;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.WPF.Controls.Outlook;

namespace Sobiens.Connectors.WPF.Controls
{
    public delegate void FoldersTreeViewControlAfter_Select(Folder parentFolder, Folder folder);

    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class HierarchyNavigator : HostControl, INavigatorExplorer
    {
        private const int LoadingNodeTagValue = -1;
        public event FoldersTreeViewControlAfter_Select After_Select;

        private SiteSettings SiteSettings { get; set; }
        private List<Folder> DataSource { get; set; }
        public bool ShowCheckBoxes { get; set; }
        public bool AdministrativeView { get; set; }
        new public bool Initialized { get; set; }
        public int[] IncludedFolderTypes { get; set; }

        public IConnectorExplorer ConnectorExplorer
        {
            get;
            private set;
        }

        public void SetConnectorExplorer(IConnectorExplorer connectorExplorer)
        {
            this.ConnectorExplorer = connectorExplorer;
        }

        public bool HasAnythingToDisplay
        {
            get
            {
                return FoldersTreeView.Items.Count>0?true:false;
            }
        }


        public HierarchyNavigator()
        {
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


        public void Initialize(SiteSetting siteSetting, List<Folder> dataSource, int[] includedFolderTypes)
        {
            SiteSettings siteSettings = new SiteSettings();
            siteSettings.Add(siteSetting);
            this.Initialize(siteSettings, dataSource, includedFolderTypes);
        }

        public void Initialize(SiteSettings siteSettings, List<Folder> dataSource, int[] includedFolderTypes)
        {
            this.IncludedFolderTypes = includedFolderTypes;
            this.SiteSettings = siteSettings;
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
            //else
            //{
            //    this.SiteSettings = ConfigurationManager.GetInstance().GetSiteSettings();
            //    List<ExplorerLocation> explorerLocations = ConfigurationManager.GetInstance().GetExplorerLocations(ApplicationContext.Current.GetApplicationType());
            //    List<Folder> folders = ConfigurationManager.GetInstance().GetFoldersByExplorerLocations(explorerLocations, false);

            //    foreach (Folder folder in folders)
            //    {
            //        AddNode(FoldersTreeView.Items, folder, folder.SiteSettingID);
            //    }
            //}
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
            Uri uri = new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;

            Label lbl = new Label();
            lbl.Content = folder.Title;

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
        }

        void chkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = e.Source as CheckBox;
            Folder folder = checkBox.Tag as Folder;
            folder.Selected = true;
        }

        /// <summary>
        /// Refreshes the node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void RefreshNode(TreeViewItem item)
        {
            Folder folder = null;
            item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                folder = (Folder)item.Tag;
            }));

            {
                SiteSetting siteSetting = this.SiteSettings[folder.SiteSettingID];
                List<Folder> subFolders = ApplicationContext.Current.GetSubFolders(siteSetting, folder, this.IncludedFolderTypes, string.Empty);
                folder.Folders = subFolders;
                item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    FoldersTreeView.BeginInit();
                    item.Items.Clear();
                    foreach (Folder subFolder in subFolders)
                    {
                        subFolder.PublicFolder = this.AdministrativeView;
                        AddNode(item.Items, subFolder, siteSetting.ID);
                    }
                    FoldersTreeView.EndInit();
                }));
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
            workItem.CallbackData = e.Source;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void callback(object item, DateTime dateTime)
        {
            RefreshNode((TreeViewItem) item);
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


        private void FoldersTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (FoldersTreeView.SelectedItem == null || After_Select == null)
                return;

            TreeViewItem tempNode = (TreeViewItem)FoldersTreeView.SelectedItem;
            Folder folder = ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as Folder;
            After_Select(null, folder);

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Initialized == false)
            {
                //RefreshNodes();
                this.FoldersTreeView.ContextMenu = new ContextMenu();
            }
            Initialized = true;
        }

        private void FoldersTreeView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            TreeViewItem item = FindControls.FindParent<TreeViewItem>(e.OriginalSource as DependencyObject);
            if (item == null)
            {
                return;
            }

            TreeViewItem parentItem = item.Parent as TreeViewItem;
            if (parentItem == null)
                parentItem = (TreeViewItem)((TreeView)item.Parent).Items[0];

            Folder folder = item.Tag as Folder;
            Folder parentFolder = parentItem.Tag as Folder;
            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);
            object obj = item.Tag;
            IConnectorMainView connectorExplorer = FindControls.FindParent<OutlookConnectorExplorer>(this);
            object inspector = connectorExplorer != null ? connectorExplorer.Inspector : null;
            ContextMenuManager.Instance.FillContextMenuItems(FoldersTreeView.ContextMenu, siteSetting, folder, inspector, parentFolder);

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

        private Dictionary<string, object> getFieldMappings(string webURL, List<ApplicationItemProperty> properties, List<ContentType> contentTypes, FolderSettings folderSettings, FolderSetting defaultFolderSetting, ISiteSetting siteSetting, string rootFolder, out ContentType contentType,bool displayFileName)
        {
            Dictionary<string, object> mappings = new Dictionary<string, object>();

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

        private void FoldersTreeView_Drop(object sender, DragEventArgs e)
        {
            TreeViewItem targetItem = GetNearestContainer(e.OriginalSource as UIElement);
            Folder folder = targetItem.Tag as Folder;

            if (folder == null && folder.CanUpload() == false)
            {
                MessageBox.Show(Languages.Translate("Please drop into a folder or a library or a list(attachment enabled)"));
                return;
            }

            bool canUpdateItemInGrid = false;
            if (this.ConnectorExplorer.ContentExplorer.SelectedFolder != null && this.ConnectorExplorer.ContentExplorer.SelectedFolder.GetUrl() == folder.GetUrl())
            {
                canUpdateItemInGrid = true;
            }

            List<IItem> emailItems = new List<IItem>();

            ISiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);
            List<UploadItem> uploadItems = ApplicationContext.Current.GetUploadItems(folder.GetWebUrl(), siteSetting, folder, e, getFieldMappings);
            if (uploadItems == null)
            {
                return;
            }

            WorkItem workItem = new WorkItem(Languages.Translate("Uploading emails"));
            workItem.CallbackFunction = new WorkRequestDelegate(UploadEmails_Callback);
            workItem.CallbackData = new object[] { siteSetting, folder, uploadItems, this.ConnectorExplorer, canUpdateItemInGrid };
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
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

    }
}
