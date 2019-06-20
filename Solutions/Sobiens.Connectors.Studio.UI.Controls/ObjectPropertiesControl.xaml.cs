using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class ObjectPropertiesControl : UserControl
    {
        private const int LoadingNodeTagValue = -1;
        public Folder SourceObject;
         
        public ObjectPropertiesControl()
        {
            InitializeComponent();
        }

        private TreeViewItem AddNode(ItemCollection itemCollection, string title, object dataItem, Brush foreground)
        {
            TreeViewItem node = new TreeViewItem();
            node.Header = title;
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


        public void Initialize(Folder sourceObject)
        {
            this.SourceObject = sourceObject;
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
            //TreeViewItem parentItem = item.Parent as TreeViewItem;
            if (item.Tag is IView)
            {
                MenuItem grantAccessMenuItem = new MenuItem(){Header="Grant access", Tag = item};
                grantAccessMenuItem.Click += GrantAccessMenuItem_Click; ;
                this.FoldersTreeView.ContextMenu.Items.Add(grantAccessMenuItem);
            }
        }

        private void GrantAccessMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.OriginalSource as MenuItem;
            IView view = ((TreeViewItem)menuItem.Tag).Tag as IView;
            SiteSetting siteSetting = ApplicationContext.Current.Configuration.SiteSettings[view.SiteSettingID];

            ObjectAccessMaintenanceForm objectAccessMaintenanceForm = new ObjectAccessMaintenanceForm();
            objectAccessMaintenanceForm.Initialize(siteSetting, view.Name, "View", view.UniqueIdentifier);
            objectAccessMaintenanceForm.ShowDialog(null, "Object Access Maintenance");

        }

        private void CreateMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private bool IsTreeNodeChecked(TreeViewItem node)
        {
            DockPanel panel = (DockPanel)node.Header;
            CheckBox checkBox = (CheckBox)panel.Children[1];
            return checkBox.IsChecked.HasValue? checkBox.IsChecked.Value:false;
        }

        private void FoldersTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Field field = ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as Field;
            List<Field> fields = ((TreeViewItem)FoldersTreeView.SelectedItem).Tag as List<Field>;

            /*
            if (field != null)
            {
                textBlock.Text = field.SchemaXml;
            }
            else if (fields != null)
            {
                textBlock.Text = string.Empty;
                foreach (Field _field in fields)
                {
                    textBlock.Text += _field.SchemaXml + Environment.NewLine;
                }
            }
            */
        }

        private void DeleteUniquePermissionsRecursivelyButton_Click(object sender, RoutedEventArgs e)
        {
            SiteSetting sourceObjectSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
            ApplicationContext.Current.DeleteUniquePermissions(sourceObjectSiteSetting, SourceObject, true);
            MessageBox.Show("Completed");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FoldersTreeView.AddHandler(TreeViewItem.ExpandedEvent, new RoutedEventHandler(node_Expanded));

        }

        void node_Expanded(object sender, RoutedEventArgs e)
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

            if (item.Tag == null)
                return;

            SiteSetting sourceObjectSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
            if (item.Tag.ToString() == "Fields")
            {
                List<Field> fields = ApplicationContext.Current.GetFields(sourceObjectSiteSetting, SourceObject);
                item.Items.Clear();
                foreach (Field field in fields)
                {
                    AddNode(item.Items, field.DisplayName, field, Brushes.Black);
                }
            }
            if (item.Tag.ToString() == "Personal views")
            {
                List<IView> views = ApplicationContext.Current.GetViews(sourceObjectSiteSetting, SourceObject);
                item.Items.Clear();
                foreach (IView view in views)
                {
                    AddNode(item.Items, view.Name, view, Brushes.Black);
                }
            }
            int c = 3;

        }
        public void PopulateControls()
        {
            if (SourceObject == null)
                return;

            Folder sourceObject = SourceObject;

            try
            {
                SiteSetting sourceObjectSiteSetting = ApplicationContext.Current.Configuration.SiteSettings[SourceObject.SiteSettingID];
                FoldersTreeView.Items.Clear();
                //TreeViewItem rootNode = AddNode(FoldersTreeView.Items, "Items", "Items", Brushes.DarkBlue);
                TreeViewItem listNode = AddNode(FoldersTreeView.Items, SourceObject.Title, SourceObject, Brushes.Black);
                TreeViewItem fieldsNode = AddNode(listNode.Items, "Fields", "Fields", Brushes.Black);
                AddLoadingNode(fieldsNode);

                TreeViewItem viewsNode = AddNode(listNode.Items, "Views", null, Brushes.Black);
                TreeViewItem sharedWithMeViewsNode = AddNode(viewsNode.Items, "Shared with me", "Shared with me", Brushes.Black);
                TreeViewItem personalViewsNode = AddNode(viewsNode.Items, "Personal views", "Personal views", Brushes.Black);
                AddLoadingNode(sharedWithMeViewsNode);
                AddLoadingNode(personalViewsNode);
            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occured:" + ex.Message);
                Logger.Error(ex, ApplicationContext.Current.GetApplicationType().ToString());
            }

        }
    }
}
