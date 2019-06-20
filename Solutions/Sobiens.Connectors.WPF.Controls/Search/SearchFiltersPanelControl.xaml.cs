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
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Search;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common;
using System.Windows.Threading;
using System.Threading;

namespace Sobiens.Connectors.WPF.Controls.Search
{
    /// <summary>
    /// Interaction logic for SearchFilterControl.xaml
    /// </summary>
    public partial class SearchFiltersPanelControl : HostControl
    {
        public Dictionary<string, SearchFilters> SearchFilters = new Dictionary<string, SearchFilters>();
        private ContentType ContentType = null;
        private string ContentTypeID = null;
        private BasicFolderDefinition TargetFolder = null;
        private SiteSetting SiteSetting = null;

        public SearchFiltersPanelControl()
        {
            InitializeComponent();
        }
        
        public void Initialize(SiteSetting siteSetting, BasicFolderDefinition targetFolder, string contentTypeID)
        {
            this.SiteSetting = siteSetting;
            this.ContentTypeID = contentTypeID;
            this.TargetFolder = targetFolder;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.sdx();
            //this.RefreshSearchFiltersListPanel();
        }

        private void sdx()
        {
            this.ShowLoadingStatus(Languages.Translate("Loading content type..."));

            object[] args = new object[] { this.SearchFiltersListPanel, this.SiteSetting, this.TargetFolder, this.ContentTypeID, this.SearchFilters };

            WorkItem workItem = new WorkItem(Languages.Translate("Populating content types"));
            workItem.CallbackFunction = new WorkRequestDelegate(callback);
            workItem.CallbackData = args;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void callback(object args, DateTime dateTime)
        {
            object[] arguments = args as object[];
            StackPanel searchFiltersListPanel = arguments[0] as StackPanel;
            SiteSetting siteSetting = arguments[1] as SiteSetting;
            BasicFolderDefinition folder = arguments[2] as BasicFolderDefinition;
            string contentTypeID = arguments[3] as string;
            Dictionary<string, SearchFilters> searchFilters = arguments[4] as Dictionary<string, SearchFilters>;

            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
            Folder targetFolder = serviceManager.GetFolder(siteSetting, folder);
            this.ContentType = serviceManager.GetContentType(siteSetting, targetFolder, contentTypeID, false);

            searchFiltersListPanel.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
            {
                SearchFilter filter = new Entities.Search.SearchFilter(string.Empty, FieldTypes.Text, CamlFilterTypes.Contains, string.Empty);
                searchFilterControl1.Initialize(targetFolder.GetWebUrl(), this.ContentType, filter, true);
                this.RefreshSearchFiltersListPanel(searchFiltersListPanel, searchFilters, this.ContentType);

                this.HideLoadingStatus("Ready");
            }));
        }


        private void RefreshSearchFiltersListPanel(StackPanel searchFiltersListPanel, Dictionary<string, SearchFilters> searchFilters, ContentType contentType)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(SiteSetting.SiteSettingType);
            Folder targetFolder = serviceManager.GetFolder(SiteSetting, TargetFolder);

            SearchFiltersListPanel.Children.Clear();
            foreach (string key in searchFilters.Keys)
            {
                SearchFilters filters = searchFilters[key];
                SearchFilterControl searchFilterControl = new SearchFilterControl();
                searchFilterControl.Name = key;
                searchFilterControl.Initialize(targetFolder.GetWebUrl(), contentType, filters[0], filters.IsOr);
                searchFilterControl.ReadOnly = true;
                searchFilterControl.FilterRemoved += new EventHandler(searchFilterControl_FilterRemoved);
                searchFiltersListPanel.Children.Add(searchFilterControl);
            }
        }

        protected void searchFilterControl_FilterRemoved(object sender, EventArgs e)
        {
            SearchFilterControl searchFilterControl = (SearchFilterControl)sender;
            this.SearchFilters.Remove(searchFilterControl.Name);

            this.RefreshSearchFiltersListPanel(this.SearchFiltersListPanel, this.SearchFilters, this.ContentType);
        }

        private void searchFilterControl1_FilterAdded(object sender, EventArgs e)
        {
            SearchFilters filters = new Entities.Search.SearchFilters();
            filters.IsOr = searchFilterControl1.IsOr;
            filters.Add(searchFilterControl1.GetSearchFilter());
            SearchFilters.Add("SF" + Guid.NewGuid().ToString().Replace('-', 'A'), filters);

            this.RefreshSearchFiltersListPanel(this.SearchFiltersListPanel, this.SearchFilters, this.ContentType);
        }

    }
}
