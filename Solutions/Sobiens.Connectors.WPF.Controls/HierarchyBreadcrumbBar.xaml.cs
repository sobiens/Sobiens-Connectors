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
using Sobiens.Connectors.Common;
using System.Windows.Threading;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using Sobiens.WPF.Controls.BreadcrumbBarControl;
using System.Threading;
using Sobiens.Connectors.Entities.Settings;
using System.Windows.Media.Animation;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.WPF.Controls.Settings;
using System.Diagnostics;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyBreadcrumbBar.xaml
    /// </summary>
    public partial class HierarchyBreadcrumbBar : UserControl, INavigatorExplorer
    {
        public event FoldersTreeViewControlAfter_Select After_Select;
        private bool DisableEventFiring = false;

        public Folder SelectedFolder
        {
            get
            {
                BreadcrumbItem selectedItem = bar.SelectedBreadcrumb;
                if (selectedItem != null)
                {
                    return selectedItem.Tag as Folder;
                }

                return null;

            }
        }

        public Folder SelectedParentFolder
        {
            get
            {
                if (bar.SelectedBreadcrumb != null)
                {
                    BreadcrumbItem parentItem = bar.SelectedBreadcrumb.ParentBreadcrumbItem;
                    if (parentItem != null)
                    {
                        return parentItem.Tag as Folder;
                    }
                }

                return null;
            }
        }

        public bool HasAnythingToDisplay
        {
            get
            {
                return bar.HasDropDownItems;
            }
        }

        public IConnectorExplorer ConnectorExplorer
        {
            get;
            private set;
        }

        public void SetConnectorExplorer(IConnectorExplorer connectorExplorer)
        {
            this.ConnectorExplorer = connectorExplorer;
        }

        public HierarchyBreadcrumbBar()
        {
            InitializeComponent();
        }

        /// <summary>
        /// A BreadcrumbItem needs to populate it's Items. This can be due to the fact that a new BreadcrumbItem is selected, and thus
        /// it's Items must be populated to determine whether this BreadcrumbItem show a dropdown button,
        /// or when the Path property of the BreadcrumbBar is changed and therefore the Breadcrumbs must be populated from the new path.
        /// </summary>
        private void BreadcrumbBar_PopulateItems(object sender, BreadcrumbItemEventArgs e)
        {
            if (DisableEventFiring == true)
                return;

            BreadcrumbItem item = e.Item;
            if (item.Items.Count > 0)
                return;

            WorkItem workItem = new WorkItem(Languages.Translate("Populating Breadcrumb items"));
            workItem.CallbackFunction = new WorkRequestDelegate(RefreshItems_Callback);
            workItem.CallbackData = item;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void RefreshItems_Callback(object item, DateTime dateTime)
        {
            BreadcrumbItem bcItem = (BreadcrumbItem)item;
            RefreshItems(bcItem);
        }

        public void FillBreadcrumbItem(BreadcrumbItem currentBar, Folder currentFolder, string remainingURL)
        {
            currentBar.Items.Clear();
            string currentFolderName = string.Empty;

            if (remainingURL.IndexOf('/') > -1)
            {
                currentFolderName = remainingURL.Substring(0, remainingURL.IndexOf('/'));
                remainingURL = remainingURL.Substring(remainingURL.IndexOf(currentFolderName) + currentFolderName.Length + 1);
            }
            else
            {
                currentFolderName = remainingURL;
                remainingURL = string.Empty;
            }

            foreach (Folder folder in currentFolder.Folders)
            {
                BreadcrumbItem child1 = new BreadcrumbItem();
                child1.Header = folder.Title;
                Image img = new Image();
                Uri uri = new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/" + folder.IconName + ".gif", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                child1.Image = imgSource;
                child1.Tag = folder;
                currentBar.Items.Add(child1);

                string folderName = folder.GetUrl().TrimEnd(new char[]{'/'});
                if (folderName.LastIndexOf("/") > -1)
                {
                    folderName = folderName.Substring(folderName.LastIndexOf("/") + 1);
                }

                if (string.IsNullOrEmpty(currentFolderName) == false && folderName.Equals(currentFolderName,StringComparison.InvariantCultureIgnoreCase) == true)
                {
                    currentBar.SelectedItem = child1;
                }

                if (string.IsNullOrEmpty(remainingURL) == false)
                {
                    FillBreadcrumbItem(child1, folder, remainingURL);
                }
            }
        }

        public void SelectFolder(Folder folder)
        {
            DisableEventFiring = true;

            BreadcrumbItem selectedTopBar = null;
            foreach (BreadcrumbItem item in bar.RootItem.Items)
            {
                if (((Folder)item.Tag).SiteSettingID.Equals(folder.SiteSettingID) == true)
                {
                    selectedTopBar = item;
                    break;
                }
            }

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);
            string url = folder.GetUrl();
            Folder rootFolder = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetFoldersTreeFromURL((ISiteSetting)siteSetting, folder.GetUrl());
            string remainingUrl = url.absoluteTorelative(siteSetting.Url);
            //string remainingUrl = string.Empty;
            //if (url.Equals(siteSetting.Url, StringComparison.InvariantCultureIgnoreCase) == true)
            //{
            //    remainingUrl = string.Empty;
            //}
            //else
            //{
            //    remainingUrl = url.Substring(url.IndexOf(siteSetting.Url) + siteSetting.Url.Length + 1);
            //}

            BreadcrumbItem currentBar = selectedTopBar;
            Folder currentFolder = rootFolder;
            if (selectedTopBar != null)
            {
                FillBreadcrumbItem(currentBar, currentFolder, remainingUrl);
            }
            DisableEventFiring = false;
        }

        private void PopulateItems(BreadcrumbItem parentItem, List<Folder> folders)
        {
            foreach (Folder folder in folders)
            {
                if (folder.Selected == false)
                    continue;

                BreadcrumbItem item = new BreadcrumbItem();
                item.Header = folder.Title;
                Image img = new Image();
                Uri uri = new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
                ImageSource imgSource = new BitmapImage(uri);
                item.Image = imgSource;
                item.Tag = folder;
                parentItem.Items.Add(item);

                PopulateItems(item, folder.Folders);
            }
        }

        public void RefreshControls()
        {
            throw new Exception(Languages.Translate("Not implemented yet"));
        }

        public void RefreshItems(List<Folder> folders)
        {
            try
            {
                bar.RootItem.Items.Clear();
                //List<ExplorerLocation> explorerLocations = ConfigurationManager.GetInstance().GetExplorerLocations(ApplicationContext.Current.GetApplicationType());
                //List<Folder> folders = ConfigurationManager.GetInstance().GetFoldersByExplorerLocations(explorerLocations, false);

                PopulateItems(bar.RootItem, folders);
            }
            catch (Exception ex)
            {
            }
        }

        private void RefreshItems(BreadcrumbItem item)
        {
            Folder folder = null;
            item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                folder = item.Tag as Folder;
            }));

            if (folder == null)
            {
                return;
            }
            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);

            List<Folder> subFolders;

            if (folder.Folders.Count > 0)
            {
                subFolders = folder.Folders;
            }
            else
            {
                subFolders = ApplicationContext.Current.GetSubFolders(siteSetting, folder, null, string.Empty);
            }

            folder.Folders = subFolders;
            item.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                item.BeginInit();
                item.Items.Clear();
                foreach (Folder subFolder in subFolders)
                {
                    BreadcrumbItem subItem = new BreadcrumbItem();
                    subItem.Header = subFolder.Title;
                    Image img = new Image();
                    Uri uri = new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
                    ImageSource imgSource = new BitmapImage(uri);
                    item.Image = imgSource;
                    subItem.Tag = subFolder;
                    item.Items.Add(subItem);
                }
                item.EndInit();
            }));

        }

        private void bar_SelectedBreadcrumbChanged(object sender, RoutedEventArgs e)
        {
            if (DisableEventFiring == true)
                return;

            if(After_Select != null)
            {
                if (this.SelectedFolder == null)
                {
                    return;
                }

                After_Select(this.SelectedParentFolder, this.SelectedFolder);
            }
        }

        private void bar_PathChangedByText(string oldValue, string newValue)
        {
            int u = 3;
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            BreadcrumbItem selectedItem = bar.SelectedItem as BreadcrumbItem;
            if (selectedItem == null)
            {
                return;
            }

            Folder folder = null;
            selectedItem.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                folder = selectedItem.Tag as Folder;
            }));
            if (folder == null || folder.CanUpload() == false)
            {
                MessageBox.Show(Languages.Translate("Please select a folder or a library or a list (attachment enabled)"));
                return;
            }

            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(folder.SiteSettingID);
            FoldersManager.EditItemPropertyMappings(siteSetting, folder);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            BreadcrumbItem selectedItem = bar.SelectedItem as BreadcrumbItem;
            if (selectedItem == null)
            {
                return;
            }

            RefreshItems(selectedItem);

            DoubleAnimation da = new DoubleAnimation(100, new Duration(new TimeSpan(0, 0, 2)));
            da.FillBehavior = FillBehavior.Stop;
            bar.BeginAnimation(BreadcrumbBar.ProgressValueProperty, da);
        }

        private void OpenInExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            Folder folder = getSelectedFolder();
            if (folder != null)
            {
                string path = "file" + folder.GetUrl().ToLower().Replace("http", string.Empty);
                Process.Start("Explorer.exe", path);
            }
            
        }

        private void OpenInNavigatorButton_Click(object sender, RoutedEventArgs e)
        {
            Folder folder = getSelectedFolder();

            if (folder != null) Process.Start("IExplore.exe", folder.GetUrl());
            
        }

        private Folder getSelectedFolder()
        {
            BreadcrumbItem selectedItem = bar.SelectedItem as BreadcrumbItem;
            if (selectedItem == null)
            {
                return null;
            }

            Folder folder = selectedItem.Tag as Folder;

            if (folder == null) return null;

            return folder;
        }
    }
}
