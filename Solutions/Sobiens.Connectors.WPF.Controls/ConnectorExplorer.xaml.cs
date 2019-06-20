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
using Sobiens.Connectors.Entities.Interfaces;
using System.Collections.Specialized;
using Sobiens.Connectors.Entities.SharePoint;
using System.Diagnostics;
using Sobiens.Connectors.Entities;
using System.ComponentModel;
using Sobiens.Connectors.Common;
using System.IO;
using System.Data;
using Sobiens.WPF.Controls.BreadcrumbBarControl;
using Sobiens.Connectors.Entities.Settings;
using System.Windows.Threading;
using System.Threading;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.Common.Interfaces;
using System.Collections;
using Sobiens.Connectors.Entities.Data;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.WPF.Controls.Outlook;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ListViewControl.xaml
    /// </summary>
    public partial class ConnectorExplorer : HostControl, IConnectorExplorer
    {
        public List<ExplorerLocation> ExplorerLocations { get; set; }
        public SiteSettings SiteSettings { get; set; }

        public bool IsDataLoaded { get; set; }

        public ConnectorExplorer()
        {
            InitializeComponent();
        }

        public void Initialize(SiteSettings siteSettings, List<ExplorerLocation> explorerLocations)
        {
            this.IsDataLoaded = false;
            this.ExplorerLocations = explorerLocations;
            this.SiteSettings = siteSettings;
        }

        public void RefreshControls()
        {
            if (this.IsDataLoaded == false)
            {
                List<Folder> folders = ConfigurationManager.GetInstance().GetFoldersByExplorerLocations(this.ExplorerLocations, false);
                this.RefreshControls(folders);
                this.IsDataLoaded = true;
            }
        }

        public IContentExplorer ContentExplorer
        {
            get
            {
                return this.LibraryContentDataGridView;
            }
        }
        /*
        public INavigatorExplorer BreadcrumbNavigatorExplorer
        {
            get
            {
                return this.hierarchyBreadcrumbBar1;
            }
        }
         */ 
        
        public INavigatorExplorer TreeviewNavigatorExplorer
        {
            get
            {
                return this.hierarchyNavigator;
            }
        }



        public bool HasAnythingToDisplay
        {
            get
            {
                return (this.ExplorerLocations != null && this.ExplorerLocations.Count > 0) ? true : false;
            }
        }


        protected override void OnLoad()
        {
            base.OnLoad();

            //this.BreadcrumbNavigatorExplorer.SetConnectorExplorer(this);
            this.TreeviewNavigatorExplorer.SetConnectorExplorer(this);
            this.ContentExplorer.SetConnectorExplorer(this);
            //this.OpenHierachyTreeView();

            /*
            if (StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType()).IsHierarchyTreeViewOpen == true)
            {
                this.OpenHierachyTreeView();
            }
            else
            {
                this.OpenHierachyBreadcumb();
            }
             */ 
        }

        private DataObject myData = new DataObject();
        public event System.EventHandler SelectionChanged;
        public event System.EventHandler EditListItemSelected;
        private IItem CopiedListItem = null;

        public List<Folder> Folders { get; set; }


        public void Initialize()
        {
        }

        public void RefreshControls(List<Folder> folders)
        {
            this.Folders = folders;
            //hierarchyBreadcrumbBar1.RefreshItems(folders);
            hierarchyNavigator.Initialize(ConfigurationManager.GetInstance().GetSiteSettings(), folders, null);
            hierarchyNavigator.RefreshNodes();
        }
        /*
        private void OpenHierachyTreeView()
        {
            //hierarchyBreadcrumbBar1.Visibility = System.Windows.Visibility.Hidden;
            hierarchyNavigator.Visibility = System.Windows.Visibility.Visible;
            HierarchyStackPanel.Height = 300;

            ImageBrush brush1 = new ImageBrush();
            Uri uri = new Uri(BaseUriHelper.GetBaseUri(this), "Images/Previous.PNG");
            BitmapImage image = new BitmapImage(uri);
            brush1.ImageSource = image;
            ExpandButton.Background = brush1;
        }
          


        private void OpenHierachyBreadcumb()
        {
            //hierarchyBreadcrumbBar1.Visibility = System.Windows.Visibility.Visible;
            hierarchyNavigator.Visibility = System.Windows.Visibility.Hidden;
            HierarchyStackPanel.Height = 30;
            ImageBrush brush1 = new ImageBrush();
            Uri uri = new Uri(BaseUriHelper.GetBaseUri(this), "Images/Next.PNG");
            BitmapImage image = new BitmapImage(uri);
            brush1.ImageSource = image;
            ExpandButton.Background = brush1;
        }
         */ 

        private void ExpandButton_Click(object sender, RoutedEventArgs e)
        {

            /*
            bool isHierarchyTreeViewOpen;
            if (hierarchyBreadcrumbBar1.Visibility == System.Windows.Visibility.Visible)
            {
                this.OpenHierachyTreeView();
                isHierarchyTreeViewOpen = true;
            }
            else
            {
                this.OpenHierachyBreadcumb();
                isHierarchyTreeViewOpen = false;
            }
            */
            //StateManager.GetInstance().ConnectorState.GetApplicationState(ApplicationContext.Current.GetApplicationType()).IsHierarchyTreeViewOpen = isHierarchyTreeViewOpen;
            //StateManager.GetInstance().SaveState();
        }

        private void hierarchyNavigator_After_Select_1(Folder parentFolder, Folder folder)
        {
            this.ContentExplorer.SelectFolder(parentFolder, folder);
        }

        private void hierarchyBreadcrumbBar1_After_Select(Folder parentFolder, Folder folder)
        {
            this.ContentExplorer.SelectFolder(parentFolder, folder);
        }
    }
}
