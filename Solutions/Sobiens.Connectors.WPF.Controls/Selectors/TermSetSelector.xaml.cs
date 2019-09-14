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
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Common.Threading;
using System.Windows.Threading;
using System.Threading;
using System.Xml;

namespace Sobiens.Connectors.WPF.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for PictureSelectors.xaml
    /// </summary>
    public partial class TermSetSelector : HostControl
    {
        public TermSetSelector()
        {
            InitializeComponent();
        }

        public SiteSetting SiteSetting { get; set; }
        public SPTaxonomyField Field { get; private set; }
        public string WebURL { get; private set; }
        public void Initialize(SiteSetting siteSetting, string webURL, SPTaxonomyField field)
        {
            SiteSetting = siteSetting;
            WebURL = webURL;
            Field = field;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.PopulateTermSets();
        }

        private void PopulateTermSets()
        {
            this.ShowLoadingStatus(Languages.Translate("Loading termsets..."));

            object[] args = new object[] { this.SiteSetting, this.Field, this.TermSetTreeView, this.WebURL };

            WorkItem workItem = new WorkItem(Languages.Translate("Populating termsets"));
            workItem.CallbackFunction = new WorkRequestDelegate(callback);
            workItem.CallbackData = args;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        void callback(object args, DateTime dateTime)
        {

            object[] arguments = args as object[];
            SiteSetting siteSetting = arguments[0] as SiteSetting;
            SPTaxonomyField field = arguments[1] as SPTaxonomyField;
            TreeView treeview = arguments[2] as TreeView;
            string webURL = arguments[3] as string;
            
            //http://www.novolocus.com/2012/02/09/working-with-the-taxonomyclientservice-part-2-get-the-termset-and-understand-it/
            //string result = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetKeywordTermsByGuids(siteSetting, webURL, field.LCID, field.TermSetId.ToString());
            //SPTermSet termSet = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTermSet(siteSetting, field.TermSetId);
            List<SPTerm> terms = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTerms(siteSetting, field.TermSetId);

            treeview.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                        {

                            foreach (SPTerm term in terms)
                            {
                                treeview.Items.Add(term);
                            }

                            this.HideLoadingStatus(Languages.Translate("Ready"));
                        }));

        }



        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TermSetTreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectNode();
            if (!Field.Mult) this.Close(true);
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            selectNode();
        }

        private void selectNode()
        {
            object item = this.TermSetTreeView.SelectedItem;
            if (item != null)
            {
                if (!Field.Mult) SelectedTemSetsListView.Items.Clear();//for field with single value 

                SelectedTemSetsListView.Items.Add(item);
            }
        }

    }
}
