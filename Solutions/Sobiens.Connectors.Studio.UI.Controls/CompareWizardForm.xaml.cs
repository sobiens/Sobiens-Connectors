using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using System.Windows.Data;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class CompareWizardForm : HostControl
    {
        private const int LoadingNodeTagValue = -1;
        public Folder SourceObject;
        public Folder DestinationObject;

        /*
        public Folder SelectedObject
        {
            get
            {
                if (FoldersTreeView.SelectedItem != null && ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as Folder != null)
                    return (Folder)((TreeViewItem)FoldersTreeView.SelectedItem).Tag;

                return null;
            }
        }
        */

        public CompareWizardForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad() {
            SiteSetting sourceSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];

            SiteSetting destinationSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[DestinationObject.SiteSettingID];
            SourceObjectLabel.Content = SourceObject.Title;
            DestinationObjectLabel.Content = DestinationObject.Title;
            List<CompareObjectsResult> items = null;
            LoadingWindow.ShowDialog("Generating codes...", delegate ()
            {
                items = ApplicationContext.Current.GetObjectDifferences(sourceSiteSetting, SourceObject, destinationSiteSetting, DestinationObject, delegate(int percentage, string message) {
                    LoadingWindow.SetMessage(percentage, message);
                });
                ListCollectionView customers = new ListCollectionView(items);
                customers.GroupDescriptions.Add(new PropertyGroupDescription("DifferenceType"));
                this.Dispatcher.Invoke((Action)(() =>
                {
                    CompareGrid.ItemsSource = customers;
                }));
            });
        }


        private TreeViewItem AddNode(ItemCollection itemCollection, string title, object dataItem, Brush foreground)
        {
            TreeViewItem node = new TreeViewItem();
            DockPanel treenodeDock = new DockPanel();
            DockPanel folderTitleDock = new DockPanel();

            Label lbl = new Label();
            lbl.Content = title;
            lbl.Foreground = foreground;

            folderTitleDock.Children.Add(lbl);

            CheckBox chkBox = new CheckBox();
            chkBox.Margin = new Thickness(0, 0, 0, 0);
            chkBox.IsChecked = false;
            chkBox.Content = folderTitleDock;
            chkBox.Tag = dataItem;
            treenodeDock.Children.Add(chkBox);

            node.Header = treenodeDock;
            node.Tag = dataItem;
            itemCollection.Add(node);
            return node;
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


        public void Initialize(Folder sourceObject, Folder objectToCompareWith)
        {
            this.SourceObject = sourceObject;
            this.DestinationObject = objectToCompareWith;
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
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private bool ValidateApplyDifferences() {

            return true;
        }

        private bool IsTreeNodeChecked(TreeViewItem node)
        {
            DockPanel panel = (DockPanel)node.Header;
            CheckBox checkBox = (CheckBox)panel.Children[1];
            return checkBox.IsChecked.HasValue? checkBox.IsChecked.Value:false;
        }

        private void ApplyChangeButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting sourceSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
            SiteSetting destinationSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[DestinationObject.SiteSettingID];

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    CompareObjectsResult compareObjectsResult = (CompareObjectsResult)row.Item;
                    if(compareObjectsResult.DifferenceType == "Missing")
                    {
                        ApplicationContext.Current.ApplyMissingCompareObjectsResult(compareObjectsResult, sourceSiteSetting, destinationSiteSetting);
                    }
                }
            this.OnLoad();
        }

        private void CompareGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
//            Button button = e.Row.DetailsTemplate.FindName("ApplyChangeButton") as Button;
        }

        private void ShowDifferencesButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting sourceSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
            SiteSetting destinationSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[DestinationObject.SiteSettingID];

            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    CompareObjectsResult compareObjectsResult = (CompareObjectsResult)row.Item;
                    if (compareObjectsResult.DifferenceType == "Update")
                    {
                        CompareSQLObjectsForm csof = new CompareSQLObjectsForm();
                        csof.Initialize((Folder)compareObjectsResult.SourceObject, (Folder)compareObjectsResult.ObjectToCompareWith);
                        csof.ShowDialog(this.ParentWindow, "Compare and Edit",false, true);
//                        ApplicationContext.Current.ApplyMissingCompareObjectsResult(compareObjectsResult, sourceSiteSetting, destinationSiteSetting);
                    }
                }

        }
    }
}
