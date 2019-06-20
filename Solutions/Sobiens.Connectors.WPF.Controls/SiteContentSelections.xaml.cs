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
using Sobiens.Connectors.WPF.Controls.Settings;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;
using Sobiens.Connectors.Common.Interfaces;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SiteContentSelections.xaml
    /// </summary>
    public partial class SiteContentSelections : HostControl, IConnectorExplorer
    {
        private bool IsFolderSelector { get; set; }
        private Folder RootFolder { get; set; }
        public Folder SelectedFolder { get; private set; }
        public Folder SelectedParentFolder { get; private set; }

        private SiteSettings SiteSettings {get;set;}
        private List<Folder> Folders { get; set; }
        private int[] IncludedFolderTypes { get; set; }

        public SiteContentSelections()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            hierarchyNavigator1.SetConnectorExplorer(this);
            PopulateHierarchyNavigator();
        }

        public bool HasAnythingToDisplay
        {
            get
            {
                return this.Folders.Count > 0 ? true : false;
            }
        }

        public bool IsDataLoaded { get; set; }

        public IContentExplorer ContentExplorer
        {
            get
            {
                return null;
            }
        }

        public INavigatorExplorer BreadcrumbNavigatorExplorer
        {
            get
            {
                return null;
            }
        }

        public INavigatorExplorer TreeviewNavigatorExplorer
        {
            get
            {
                return hierarchyNavigator1;
            }
        }



        public void SetSelectedFolder(Folder folder)
        {
            this.SelectedFolder = folder;
        }

        private void PopulateHierarchyNavigator()
        {
            hierarchyNavigator1.Initialized = true;

            if (this.SelectedFolder != null)
            {
                SelectedLocationLabel.Content = this.SelectedFolder.GetUrl();
            }
            else
            {
                SelectedLocationLabel.Content = Languages.Translate("Not Selected");
            }

            object[] args = new object[] { };

            WorkItem workItem = new WorkItem(Languages.Translate("Populating subfolders"));
            workItem.CallbackFunction = new WorkRequestDelegate(callback);
            workItem.CallbackData = args;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);

        }

        void callback(object args, DateTime dateTime)
        {
            hierarchyNavigator1.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                /*
                if (BasicFolderDefinitions != null && BasicFolderDefinitions.Count > 0)
                {
                    SiteSetting siteSetting = SiteSettings[BasicFolderDefinitions[0].SiteSettingID];
                    IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                    Folders = new List<Folder>();
                    BasicFolderDefinitionFolder = serviceManager.GetFolderByBasicFolderDefinition(siteSetting, BasicFolderDefinitions[0], true);
                    Folders.Add(BasicFolderDefinitionFolder);
                }
                */ 

                hierarchyNavigator1.Initialize(this.SiteSettings, this.Folders, this.IncludedFolderTypes);
                hierarchyNavigator1.RefreshNodes();

                //this.HideLoadingStatus("Ready.");
            }));
        }

        public void InitializeForm(SiteSetting siteSetting, Folder rootFolder, bool isFolderSelector, int[] includedFolderTypes)
        {
            List<Folder> folders = new List<Folder>();
            folders.Add(rootFolder);
            SiteSettings siteSettings = new SiteSettings();
            siteSettings.Add(siteSetting);
            this.InitializeForm(siteSettings, folders, isFolderSelector, includedFolderTypes);
        }

        //public void InitializeForm(SiteSettings siteSettings, Folder rootFolder, bool isFolderSelector, int[] includedFolderTypes)
        //{
        //    List<Folder> folders = new List<Folder>();
        //    folders.Add(rootFolder);
        //    this.InitializeForm(siteSettings, folders, isFolderSelector, includedFolderTypes);
        //}

        public void InitializeForm(SiteSettings siteSettings, List<Folder> folders, bool isFolderSelector, int[] includedFolderTypes)
        {
            this.IncludedFolderTypes = includedFolderTypes;
            this.IsFolderSelector = isFolderSelector;
            this.SiteSettings = siteSettings;
            this.Folders = folders;
            
            if (this.IsFolderSelector == true)
            {
                hierarchyNavigator1.ShowCheckBoxes = false;
                hierarchyNavigator1.After_Select += new FoldersTreeViewControlAfter_Select(hierarchyNavigator1_After_Select);
            }
        }

        protected void hierarchyNavigator1_After_Select(Folder parentFolder, Folder folder)
        {
            this.SelectedFolder = folder;
            this.SelectedParentFolder = parentFolder;
        }

        public void Initialize(SiteSettings siteSettings, List<ExplorerLocation> explorerLocations)
        {
        }

        public void RefreshControls()
        {
            PopulateHierarchyNavigator();
        }

    }
}
