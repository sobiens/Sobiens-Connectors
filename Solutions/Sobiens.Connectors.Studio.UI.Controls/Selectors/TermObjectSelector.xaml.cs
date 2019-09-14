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
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Studio.UI.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for TermObjectSelector.xaml
    /// </summary>
    public partial class TermObjectSelector : UserControl
    {
        public TermObjectSelector()
        {
            InitializeComponent();
        }

        private const int LoadingNodeTagValue = -1;
        public ISiteSetting SiteSetting { get; set; }
        public object SelectedObject {
            get
            {
                if (TermSetTreeView.SelectedItem == null)
                    return null;

                return ((TreeViewItem)TermSetTreeView.SelectedItem).Tag;
            }
        }

        public void Initialize(ISiteSetting siteSetting)
        {
            SiteSetting = siteSetting;
            TermSetTreeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(rootNode_Expanded));
            AddLoadingNode(TermSetTreeView.Items);
            PopulateTermSets();
        }

        private void PopulateTermSets()
        {
            object[] args = new object[] { this.SiteSetting, this.TermSetTreeView };

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
            TreeView treeview = arguments[1] as TreeView;

            if (treeview != null)
            {
                SPTermStore termStore = null;
                try
                {
                    termStore = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTermStore(siteSetting);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("It needs to be SharePoint 2013 or above");
                    return;
                }

                treeview.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    treeview.Items.Clear();

                    TreeViewItem termStoreNode = new TreeViewItem();
                    termStoreNode.Header = termStore.Title;
                    termStoreNode.Tag = termStore;

                    int index = treeview.Items.Add(termStoreNode);
                    AddLoadingNode(termStoreNode);

                }));
            }
            else if (treeview != null)
            {
                List<SPTermGroup> termGroups = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTermGroups(siteSetting);
                treeview.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                            {
                                treeview.Items.Clear();
                                foreach (SPTermGroup term in termGroups)
                                {
                                    TreeViewItem termGroupNode = new TreeViewItem();
                                    termGroupNode.Header = term.Title;
                                    termGroupNode.Tag = term;

                                    int index = treeview.Items.Add(termGroupNode);
                                    AddLoadingNode(termGroupNode);
                                }
                            }));
            }
            else {
                TreeViewItem treeViewItem = arguments[1] as TreeViewItem;
                treeViewItem.Dispatcher.Invoke(DispatcherPriority.Input, new ThreadStart(() =>
                {
                    if (treeViewItem.Tag as SPTermStore != null)
                    {
                        treeViewItem.Items.Clear();

                        //SPTermGroup termGroup = treeViewItem.Tag as SPTermGroup;
                        List<SPTermGroup> termGroups = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTermGroups(siteSetting);
                        foreach (SPTermGroup term in termGroups)
                        {
                            TreeViewItem termGroupNode = new TreeViewItem();
                            termGroupNode.Header = term.Title;
                            termGroupNode.Tag = term;

                            int index = treeViewItem.Items.Add(termGroupNode);
                            AddLoadingNode(termGroupNode);
                        }
                    }
                    else if (treeViewItem.Tag as SPTermGroup != null)
                    {
                        treeViewItem.Items.Clear();

                        SPTermGroup termGroup = treeViewItem.Tag as SPTermGroup;
                        List<SPTermSet> termSets = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetGroupTermSets(siteSetting, termGroup.ID);
                        foreach (SPTermSet termSet in termSets)
                        {
                            TreeViewItem termSetNode = new TreeViewItem();
                            termSetNode.Header = termSet.Title;
                            termSetNode.Tag = termSet;

                            int index = treeViewItem.Items.Add(termSetNode);
                            AddLoadingNode(termSetNode);
                        }
                    }
                    else if (treeViewItem.Tag as SPTermSet != null)
                    {
                        treeViewItem.Items.Clear();

                        SPTermSet termSet = treeViewItem.Tag as SPTermSet;
                        List<SPTerm> terms = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTerms(siteSetting, termSet.ID);
                        foreach (SPTerm term in terms)
                        {
                            TreeViewItem termNode = new TreeViewItem();
                            termNode.Header = term.Title;
                            termNode.Tag = term;

                            int index = treeViewItem.Items.Add(termNode);
                            AddLoadingNode(termNode);
                        }
                    }
                    else if (treeViewItem.Tag as SPTerm != null)
                    {
                        treeViewItem.Items.Clear();

                        SPTerm term = treeViewItem.Tag as SPTerm;
                        List<SPTerm> terms = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType).GetTerms(siteSetting, term.ID);
                        foreach (SPTerm _term in terms)
                        {
                            TreeViewItem termNode = new TreeViewItem();
                            termNode.Header = _term.Title;
                            termNode.Tag = _term;

                            int index = treeViewItem.Items.Add(termNode);
                            AddLoadingNode(termNode);
                        }
                    }
                }));
            }
            /*
            else if (arguments[2] as SPTermSet != null)
            {

            }
            */
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

        private void AddLoadingNode(ItemCollection items)
        {
            TreeViewItem childNode = new TreeViewItem();
            childNode.Header = Languages.Translate("Loading...");
            childNode.Tag = LoadingNodeTagValue;
            items.Add(childNode);
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

            object[] args = new object[] { this.SiteSetting, item };

            WorkItem workItem = new WorkItem(Languages.Translate("Populating treeview items"));
            workItem.CallbackFunction = new WorkRequestDelegate(callback);
            workItem.CallbackData = args;
            workItem.WorkItemType = WorkItem.WorkItemTypeEnum.NonCriticalWorkItem;
            BackgroundManager.GetInstance().AddWorkItem(workItem);
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
