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
            //List<Folder> sourceFolders = ApplicationContext.Current.GetSubFolders(sourceObjectSiteSetting, SourceObject, null, string.Empty);

            SiteSetting destinationSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[DestinationObject.SiteSettingID];
            //List<Folder> objectToCompareWithFolders = ApplicationContext.Current.GetSubFolders(objectToCompareWithSiteSetting, ObjectToCompareWith, null, string.Empty);
            SourceObjectLabel.Content = SourceObject.Title;
            DestinationObjectLabel.Content = DestinationObject.Title;
            List<CompareObjectsResult> items = ApplicationContext.Current.GetObjectDifferences(sourceSiteSetting, SourceObject, destinationSiteSetting, DestinationObject);
            /*
            TreeViewItem additionalItemsNode = AddNode(FoldersTreeView.Items, "Additional Items", "Additional Items", Brushes.DarkBlue);
            TreeViewItem missingItemsNode = AddNode(FoldersTreeView.Items, "Missing Items", "Missing Items", Brushes.Red);
            TreeViewItem updateRequiresNode = AddNode(FoldersTreeView.Items, "Update Required Items", "Update Required Items", Brushes.Orange);
            */
            /*
            foreach (Folder sourceFolder in sourceFolders)
            {
                Folder destinationFolder = objectToCompareWithFolders.Where(t => t.Title.Equals(sourceFolder.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (destinationFolder == null)
                {
                    items.Add(new CompareObjectsResult(string.Empty, string.Empty, "Table", sourceFolder.Title, "Additional", sourceFolder, null)); ;
                    //TreeViewItem node = AddNode(additionalItemsNode.Items, folder.Title, folder, Brushes.Black);
                }
                else
                {
                    List<Field> objectToCompareWithFields = ApplicationContext.Current.GetFields(objectToCompareWithSiteSetting, sourceFolder);
                    List<Field> sourceFields = ApplicationContext.Current.GetFields(sourceObjectSiteSetting, destinationFolder);
                    bool hasDifference = false;
                    foreach (Field field in objectToCompareWithFields)
                    {
                        if (sourceFields.Where(t => t.Name.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                        {
                            hasDifference = true;
                            //items.Add(new CompareObjectsResult("Table", destinationFolder.Title, "Field", field.DisplayName, "Missing", null, null)); ;
                        }
                    }

                    foreach (Field field in sourceFields)
                    {
                        if (objectToCompareWithFields.Where(t => t.Name.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                        {
                            hasDifference = true;
                            //items.Add(new CompareObjectsResult("Table", destinationFolder.Title, "Field", field.DisplayName, "Additional", null, null)); ;
                        }
                    }

                    if(hasDifference == true)
                    {
                        items.Add(new CompareObjectsResult(string.Empty, string.Empty, "Table", sourceFolder.Title, "Update", sourceFolder, null)); ;
                    }

                }
            }

            foreach (Folder folder in objectToCompareWithFolders)
            {
                Folder sourceFolder = sourceFolders.Where(t => t.Title.Equals(folder.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (sourceFolder == null)
                {
                    items.Add(new CompareObjectsResult(string.Empty, string.Empty, "Table", folder.Title, "Missing", folder, null)); ;

                    //TreeViewItem listNode = AddNode(missingItemsNode.Items, folder.Title, folder, Brushes.Black);
                    //TreeViewItem fieldsNode = AddNode(listNode.Items, "Fields", "Fields", Brushes.Black);
                }
                else
                {
                    List<Field> objectToCompareWithFields = ApplicationContext.Current.GetFields(objectToCompareWithSiteSetting, folder);
                    List<Field> sourceFields = ApplicationContext.Current.GetFields(sourceObjectSiteSetting, sourceFolder);
                    foreach (Field field in objectToCompareWithFields)
                    {
                        if (sourceFields.Where(t => t.Name.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                        {
                            items.Add(new CompareObjectsResult("Table", folder.Title, "Field", field.DisplayName, "Missing", null, null)); ;
                        }
                    }

                    foreach (Field field in sourceFields)
                    {
                        if (objectToCompareWithFields.Where(t => t.Name.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                        {
                            items.Add(new CompareObjectsResult("Table", folder.Title, "Field", field.DisplayName, "Additional", null, null)); ;
                        }
                    }
                }
            }
            */
            ListCollectionView customers = new ListCollectionView(items);
            customers.GroupDescriptions.Add(new PropertyGroupDescription("DifferenceType"));
            CompareGrid.ItemsSource = customers;

            /*
            foreach (Folder folder in objectToCompareWithFolders)
            {
                Folder sourceFolder = sourceFolders.Where(t => t.Title.Equals(folder.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (sourceFolder!=null)
                {
                    List<Field> sourceFields = ApplicationContext.Current.GetFields(sourceObjectSiteSetting, sourceFolder);
                    List<Field> objectToCompareWithFields = ApplicationContext.Current.GetFields(objectToCompareWithSiteSetting, folder);
                    TreeViewItem listNode = AddNode(updateRequiresNode.Items, folder.Title, folder, Brushes.Black);
                    TreeViewItem fieldsNode = AddNode(listNode.Items, "Fields", "Fields", Brushes.Black);
                    bool hasFieldChange = false;
                    foreach (Field field in objectToCompareWithFields)
                    {
                        if (sourceFields.Where(t => t.Name.Equals(field.Name, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                        {
                            hasFieldChange = true;
                            CompareGrid.Items.Add(new CompareObjectsResult("Table", folder.Title, "Field", field.DisplayName, "Update")); ;
                            //AddNode(fieldsNode.Items, field.DisplayName, field, Brushes.Orange);
                        }
                    }

                    if(hasFieldChange == false)
                    {
                        listNode.Items.Remove(fieldsNode);
                        updateRequiresNode.Items.Remove(listNode);
                    }

                    //TreeViewItem node = AddNode(missingItemsNode.Items, folder.Title, folder.Title, Brushes.DarkBlue);
                }
            }
            */
            //FoldersTreeView
        }


        private TreeViewItem AddNode(ItemCollection itemCollection, string title, object dataItem, Brush foreground)
        {
            TreeViewItem node = new TreeViewItem();
            DockPanel treenodeDock = new DockPanel();
            DockPanel folderTitleDock = new DockPanel();
            /*
            Image img = new Image();
            Uri uri = new Uri("/Sobiens.Connectors.Studio.UI.Controls;component/Images/" + folder.IconName + ".GIF", UriKind.Relative);
            ImageSource imgSource = new BitmapImage(uri);
            img.Source = imgSource;
            */
            Label lbl = new Label();
            lbl.Content = title;
            lbl.Foreground = foreground;

//            folderTitleDock.Children.Add(img);
            folderTitleDock.Children.Add(lbl);

            CheckBox chkBox = new CheckBox();
            chkBox.Margin = new Thickness(0, 0, 0, 0);
            chkBox.IsChecked = false;
            chkBox.Content = folderTitleDock;
            chkBox.Tag = dataItem;
            //chkBox.Checked += new RoutedEventHandler(chkBox_Checked);
            //chkBox.Unchecked += new RoutedEventHandler(chkBox_Unchecked);
            treenodeDock.Children.Add(chkBox);

            node.Header = treenodeDock;
            //rootNode.Expanded += new RoutedEventHandler(rootNode_Expanded);
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
            /*
            if (parentItem == null)
            {
                MenuItem disconnectMenuItem = new MenuItem(){Header="Disconnect", Tag = item};
                disconnectMenuItem.Click += disconnectMenuItem_Click;
                this.FoldersTreeView.ContextMenu.Items.Add(disconnectMenuItem);
            }
            */
        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private bool ValidateApplyDifferences() {

            return true;
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (object compareObjectsResult in CompareGrid.Items) {
                CompareObjectsResult _compareObjectsResult = (CompareObjectsResult)compareObjectsResult;
                if(_compareObjectsResult.DifferenceType=="Missing" && _compareObjectsResult.ObjectType == "Field")
                {
                    //ApplicationContext.Current.CreateFields(sourceObjectSiteSetting, folder);
                }
            }

            /*
            if (ValidateApplyDifferences() == false)
            {
                return;
            }

            foreach (object node in FoldersTreeView.Items)
            {
                TreeViewItem childNode = (TreeViewItem)node;
                if (childNode.Tag.ToString() == "Missing Items")
                {
                    foreach (object node1 in childNode.Items)
                    {
                        if (IsTreeNodeChecked((TreeViewItem)node1) == false)
                            continue;

                        Folder list = (Folder)((TreeViewItem)node1).Tag;
                        //List<Field> sourceFields = ApplicationContext.Current.
                            //GetFields(sourceObjectSiteSetting, sourceFolder);

                    }

                }
            }
            */
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
    }
}
