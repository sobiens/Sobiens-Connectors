using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class CompareWizardForm : HostControl
    {
        private const int LoadingNodeTagValue = -1;
        public Folder SourceObject;
        public Folder ObjectToCompareWith;

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
            SiteSetting sourceObjectSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
            List<Folder> sourceFolders = ApplicationContext.Current.GetSubFolders(sourceObjectSiteSetting, SourceObject, null);

            SiteSetting objectToCompareWithSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[ObjectToCompareWith.SiteSettingID];
            List<Folder> objectToCompareWithFolders = ApplicationContext.Current.GetSubFolders(objectToCompareWithSiteSetting, ObjectToCompareWith, null);

            TreeViewItem additionalItemsNode = AddNode(FoldersTreeView.Items, "Additional Items", "Additional Items", Brushes.DarkBlue);
            TreeViewItem missingItemsNode = AddNode(FoldersTreeView.Items, "Missing Items", "Missing Items", Brushes.Red);
            TreeViewItem updateRequiresNode = AddNode(FoldersTreeView.Items, "Update Required Items", "Update Required Items", Brushes.Orange);

            foreach (Folder folder in sourceFolders)
            {
                if(objectToCompareWithFolders.Where(t=>t.Title.Equals(folder.Title, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                {
                    TreeViewItem node = AddNode(additionalItemsNode.Items, folder.Title, folder, Brushes.Black);
                }
            }

            foreach (Folder folder in objectToCompareWithFolders)
            {
                if (sourceFolders.Where(t => t.Title.Equals(folder.Title, StringComparison.InvariantCultureIgnoreCase)).Count() == 0)
                {
                    TreeViewItem listNode = AddNode(missingItemsNode.Items, folder.Title, folder, Brushes.Black);
                    TreeViewItem fieldsNode = AddNode(listNode.Items, "Fields", "Fields", Brushes.Black);
                    List<Field> fields = ApplicationContext.Current.GetFields(objectToCompareWithSiteSetting, folder);
                    foreach (Field field in fields)
                    {
                        AddNode(fieldsNode.Items, field.DisplayName, field, Brushes.Red);
                    }
                }
            }

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
                            AddNode(fieldsNode.Items, field.DisplayName, field, Brushes.Orange);
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
            this.ObjectToCompareWith = objectToCompareWith;
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
        }

        private bool IsTreeNodeChecked(TreeViewItem node)
        {
            DockPanel panel = (DockPanel)node.Header;
            CheckBox checkBox = (CheckBox)panel.Children[1];
            return checkBox.IsChecked.HasValue? checkBox.IsChecked.Value:false;
        }
    }
}
