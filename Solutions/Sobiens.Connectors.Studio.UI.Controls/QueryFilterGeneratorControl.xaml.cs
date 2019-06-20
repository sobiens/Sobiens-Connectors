using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.CRM;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for QueryResultControl.xaml
    /// </summary>
    public partial class QueryFilterGeneratorControl : UserControl
    {
        //private string[] RequiredFields = new string[] { "ID", "Title", "FileRef" };

        public FieldCollection SourceFields = null;
        public Folder SourceFolder = null;
        public ISiteSetting SourceSiteSetting = null;
        public QueryResult QueryResult = new QueryResult();
        private List<QueryResultMappingSelectField> localQueryResultMappingSelectFields = new List<QueryResultMappingSelectField>();
        public QueryResultMappingSelectField[] QueryResultMappingSelectFields
        {
            get
            {
                /*
                if (SourceFolder.IsDocumentLibrary == true)
                {
                    if(localQueryResultMappingSelectFields.Where(t=>t.FieldName.Equals("FileRef", StringComparison.InvariantCultureIgnoreCase) == true).Count()==0)
                    {
                        QueryResultMappingSelectField newSelectField = new QueryResultMappingSelectField()
                        {
                            FieldName = "FileRef",
                            OutputHeaderName = "FileRef",
                            StaticValue = string.Empty
                        };

                        localQueryResultMappingSelectFields.Add(newSelectField);

                    }
                }
                */
                return localQueryResultMappingSelectFields.ToArray();
            }
        }

        public void Initialize(QueryResult queryResult, QueryResultMappingSelectField[] queryResultMappingSelectFields)
        {
            QueryResult = queryResult;
            localQueryResultMappingSelectFields = queryResultMappingSelectFields.ToList();
        }

        public QueryFilterGeneratorControl()
        {
            InitializeComponent();
        }

        private void SourceSelectButton_Click(object sender, RoutedEventArgs e)
        {
            //SyncTask s = new SyncTask();
            //s.SourceQueryResultMapping.Mappings[0].QueryResult.
            SelectEntityForm selectEntityForm = new SelectEntityForm();
            selectEntityForm.Initialize(new Type[] { typeof(SPList),typeof(CRMEntity), typeof(SQLTable) });
            //HostControl hc = ((HostControl)this.Parent);

            if (selectEntityForm.ShowDialog(null, "Select an object to sync from") == true)
            {
                SourceFolder = selectEntityForm.SelectedObject;
                QueryResult.SiteSetting = (SiteSetting)ApplicationContext.Current.GetSiteSetting(SourceFolder.SiteSettingID);
                QueryResult.FolderPath = SourceFolder.GetPath();
                QueryResult.PrimaryIdFieldName = SourceFolder.PrimaryIdFieldName;
                QueryResult.PrimaryNameFieldName = SourceFolder.PrimaryNameFieldName;
                QueryResult.PrimaryFileReferenceFieldName = SourceFolder.PrimaryFileReferenceFieldName;
                QueryResult.ModifiedByFieldName = SourceFolder.ModifiedByFieldName;
                QueryResult.ModifiedOnFieldName = SourceFolder.ModifiedOnFieldName;
                QueryResult.VersioningEnabled = (SourceFolder is SPBaseFolder);
                Populate();
            }
        }

        private void Populate() {

            if (QueryResult.SiteSetting == null)
                return;

            if(SourceFolder == null)
            {
                SiteSetting siteSetting = (SiteSetting)QueryResult.SiteSetting;
                SourceFolder = ApplicationContext.Current.GetFolder((SiteSetting)QueryResult.SiteSetting, new BasicFolderDefinition() {
                    FolderUrl = siteSetting.Url + "/" + QueryResult.FolderPath,
                    SiteSettingID = QueryResult.SiteSetting.ID
                });

            }
            else
            {
                QueryResult.Filters = new CamlFilters();
                QueryResult.ReferenceFields = new List<QueryResultReferenceField>();
            }

            SourceSelectButton.Content = SourceFolder.GetPath();
            QueryResult.ListName = SourceFolder.GetListName();
            QueryResult.Name = SourceFolder.GetListName();
            SourceSiteSetting = ApplicationContext.Current.GetSiteSetting(SourceFolder.SiteSettingID);
            SourceFields = ApplicationContext.Current.GetFields(SourceSiteSetting, SourceFolder);
            string[] requiredFields = new string[] { QueryResult.PrimaryIdFieldName, QueryResult.PrimaryNameFieldName, QueryResult.PrimaryFileReferenceFieldName, QueryResult.ModifiedByFieldName, QueryResult.ModifiedOnFieldName};
            requiredFields = requiredFields.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            foreach (string requiredField in requiredFields)
            {
                Field sourceField = SourceFields.Where(t => t.Name.Equals(requiredField, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (sourceField == null)
                {
                    SourceFields.Add(new Field()
                    {
                        Name = requiredField,
                        Type = FieldTypes.Text,
                        DisplayName = requiredField
                    });
                }
            }


            QueryResult.SiteSetting = (SiteSetting)SourceSiteSetting;
            /*
            List<string> tempSourceFields = new List<string>();
            tempSourceFields.AddRange(QueryResult.Fields);
            foreach(string requiredField in RequiredFields)
            {
                if(tempSourceFields.Contains(requiredField) == false)
                {
                    tempSourceFields.Add(requiredField);
                }
            }
            QueryResult.Fields = tempSourceFields.ToArray();
            */
            QueryResult.IsDocumentLibrary = SourceFolder.IsDocumentLibrary;
            PopulateFilters();

            ViewFieldsMenuItem.Items.Clear();
            foreach (Field field in SourceFields)
            {
                MenuItem mi = new MenuItem();

                CheckBox checkBox = new CheckBox();
                checkBox.Tag = field;
                checkBox.Content = field.DisplayName;
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;
                checkBox.Margin = new Thickness(0, 0, 0, 0);
                checkBox.VerticalAlignment = VerticalAlignment.Top;
                checkBox.Click += CheckBox_Click;
                if (requiredFields.Contains(field.Name) == true)
                {
                    checkBox.IsChecked = true;
                    checkBox.IsEnabled = false;
                    ViewFieldsCheckBoxChanged(checkBox, field);
                    //CheckBox_Click(checkBox, new RoutedEventArgs() { Source = checkBox });
                }
                else
                {
                    checkBox.IsChecked = (localQueryResultMappingSelectFields.Where(t => t.FieldName.Equals(field.Name) == true).Count() > 0 ? true : false);
                }

                mi.Header = checkBox;

                ViewFieldsMenuItem.Items.Add(mi);
            }


            /*
            if (SourceFolder.IsDocumentLibrary == true)
            {
                if(QueryResult.Fields.Contains("FileRef") == false)
                {
                    List<string> newArray = QueryResult.Fields.ToList();
                    newArray.Add("FileRef");

                    QueryResult.Fields = newArray.ToArray();
                }
            }
            */
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = (CheckBox)e.Source;
            Field field = (Field)chk.Tag;
            ViewFieldsCheckBoxChanged(chk, field);
        }

        private void ViewFieldsCheckBoxChanged(CheckBox chk, Field field)
        {
            if (chk.IsChecked == true)
            {
                QueryResultMappingSelectField newSelectField = new QueryResultMappingSelectField()
                {
                    FieldName = field.Name,
                    OutputHeaderName = field.Name,
                    StaticValue = string.Empty
                };

                localQueryResultMappingSelectFields.Add(newSelectField);
            }
            else
            {
                QueryResultMappingSelectField selectField = localQueryResultMappingSelectFields.Where(t => t.FieldName == field.Name).First();
                if (selectField != null)
                {
                    localQueryResultMappingSelectFields.Remove(selectField);
                }
            }

            QueryResult.Fields = localQueryResultMappingSelectFields.Select(t => t.FieldName).ToArray();
            RefreshSelectedViewFieldsLabel();
        }

        private void RefreshSelectedViewFieldsLabel()
        {
            string selectedFields = string.Empty;
            for (int i=0;i< localQueryResultMappingSelectFields.Count;i++)
            {
                if (i > 0)
                    selectedFields += " , ";
                selectedFields += localQueryResultMappingSelectFields[i].OutputHeaderName;
            }

            if(string.IsNullOrEmpty(selectedFields) == true)
            {
                selectedFields = "Not Selected";
            }

            SelectedViewFieldsLabel.Content = selectedFields;

        }

        private void FilterUpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FilterDownButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PopulateFilters()
        {
            FiltersTreeView.Items.Clear();
            PopulateFilters(FiltersTreeView.Items, QueryResult.Filters);
        }

        private void PopulateFilters(ItemCollection items, CamlFilters filters)
        {
            Grid headerGrid = new Grid();
            Label label = new Label();
            label.Content = filters.IsOr ? "OR" : "AND";
            headerGrid.Children.Add(label);

            Menu actionMenu = new Menu();
            actionMenu.HorizontalAlignment = HorizontalAlignment.Left;
            actionMenu.Margin = new Thickness(40, 0, 0, 0);
            actionMenu.Width = 20;
            actionMenu.VerticalAlignment = VerticalAlignment.Top;

            MenuItem rootMenuItem = new MenuItem();
            Image rmiImage = new Image();
            rmiImage.Width = 20;
            rmiImage.Height = 20;
            rmiImage.Source = new BitmapImage(new Uri("Images/ArrowDown.png", UriKind.Relative));
            rmiImage.HorizontalAlignment = HorizontalAlignment.Left;
            rmiImage.Margin = new Thickness(-7, 2, 0, 0);
            rmiImage.VerticalAlignment = VerticalAlignment.Top;
            rootMenuItem.Header = rmiImage;
            actionMenu.Items.Add(rootMenuItem);

            headerGrid.Children.Add(actionMenu);

            TreeViewItem rootTreeViewItem = new TreeViewItem();
            rootTreeViewItem.Header = headerGrid;
            rootTreeViewItem.Tag = filters;
            rootTreeViewItem.IsExpanded = true;
            rootTreeViewItem.MouseDoubleClick += RootTreeViewItem_MouseDoubleClick;
            items.Add(rootTreeViewItem);


            MenuItem orMenuItem = new MenuItem();
            orMenuItem.Tag = filters;
            orMenuItem.Click += OrMenuItem_Click;
            StackPanel orGroupStackPanel = new StackPanel();
            Image orGroupImage = new Image();
            orGroupImage.Width = 20;
            orGroupImage.Height = 20;
            orGroupImage.Source = new BitmapImage(new Uri("Images/FilterGroup.png", UriKind.Relative));
            orGroupImage.HorizontalAlignment = HorizontalAlignment.Left;
            orGroupImage.Margin = new Thickness(-30, 0, 0, 0);
            orGroupImage.VerticalAlignment = VerticalAlignment.Top;

            Label orGroupLabel = new Label();
            orGroupLabel.Content = "OR";
            orGroupLabel.HorizontalAlignment = HorizontalAlignment.Left;
            orGroupLabel.Margin = new Thickness(0, -23, 0, 0);
            orGroupLabel.VerticalAlignment = VerticalAlignment.Top;
            orGroupStackPanel.Children.Add(orGroupImage);
            orGroupStackPanel.Children.Add(orGroupLabel);
            orMenuItem.Header = orGroupStackPanel;
            rootMenuItem.Items.Add(orMenuItem);

            MenuItem andMenuItem = new MenuItem();
            andMenuItem.Tag = filters;
            andMenuItem.Click += AndMenuItem_Click; ;
            StackPanel andGroupStackPanel = new StackPanel();
            Image andGroupImage = new Image();
            andGroupImage.Width = 20;
            andGroupImage.Height = 20;
            andGroupImage.Source = new BitmapImage(new Uri("Images/FilterGroup.png", UriKind.Relative));
            andGroupImage.HorizontalAlignment = HorizontalAlignment.Left;
            andGroupImage.Margin = new Thickness(-30, 0, 0, 0);
            andGroupImage.VerticalAlignment = VerticalAlignment.Top;

            Label andGroupLabel = new Label();
            andGroupLabel.Content = "AND";
            andGroupLabel.HorizontalAlignment = HorizontalAlignment.Left;
            andGroupLabel.Margin = new Thickness(0, -23, 0, 0);
            andGroupLabel.VerticalAlignment = VerticalAlignment.Top;
            andGroupStackPanel.Children.Add(andGroupImage);
            andGroupStackPanel.Children.Add(andGroupLabel);
            andMenuItem.Header = andGroupStackPanel;
            rootMenuItem.Items.Add(andMenuItem);

            MenuItem filterMenuItem = new MenuItem();
            filterMenuItem.Tag = filters;
            filterMenuItem.Click += AddFilterButton_Click;
            StackPanel filterStackPanel = new StackPanel();
            Image filterImage = new Image();
            filterImage.Width = 20;
            filterImage.Height = 20;
            filterImage.Source = new BitmapImage(new Uri("Images/Filter.png", UriKind.Relative));
            filterImage.HorizontalAlignment = HorizontalAlignment.Left;
            filterImage.Margin = new Thickness(-30, 0, 0, 0);
            filterImage.VerticalAlignment = VerticalAlignment.Top;

            Label filterLabel = new Label();
            filterLabel.Content = "Filter";
            filterLabel.HorizontalAlignment = HorizontalAlignment.Left;
            filterLabel.Margin = new Thickness(0, -23, 0, 0);
            filterLabel.VerticalAlignment = VerticalAlignment.Top;
            filterStackPanel.Children.Add(filterImage);
            filterStackPanel.Children.Add(filterLabel);
            filterMenuItem.Header = filterStackPanel;
            rootMenuItem.Items.Add(filterMenuItem);
            /* 
                        Button addGroupButton = new Button();
                        addGroupButton.Content = "Add Group";
                        addGroupButton.Click += AddGroupButton_Click;
                        addGroupButton.HorizontalAlignment = HorizontalAlignment.Left;
                        addGroupButton.Margin = new Thickness(70, 0, 0, 0);
                        addGroupButton.Width = 70;
                        addGroupButton.Tag = filters;
                        addGroupButton.DataContext = items;
                        headerGrid.Children.Add(addGroupButton);

                        Button addFilterButton = new Button();
                        addFilterButton.Tag = filters;
                        addFilterButton.DataContext = items;
                        addFilterButton.Content = "Add Filter";
                        addFilterButton.Click += AddFilterButton_Click; ;
                        addFilterButton.HorizontalAlignment = HorizontalAlignment.Left;
                        addFilterButton.Margin = new Thickness(170, 0, 0, 0);
                        addFilterButton.Width = 70;
                        headerGrid.Children.Add(addFilterButton);
            */
            foreach (CamlFilters _filters in filters.FilterCollections)
            {
                PopulateFilters(rootTreeViewItem.Items, _filters);
            }

            foreach (CamlFilter _filter in filters.Filters)
            {
                PopulateFilter(rootTreeViewItem.Items, _filter);
            }
        }

        private void AndMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            CamlFilters parentFilters = (CamlFilters)menuItem.Tag;

            CamlFilters filters = new CamlFilters();
            filters.IsOr = false;
            parentFilters.FilterCollections.Add(filters);
            PopulateFilters();
        }

        private void OrMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.Source;
            CamlFilters parentFilters = (CamlFilters)menuItem.Tag;

            CamlFilters filters = new CamlFilters();
            filters.IsOr = true;
            parentFilters.FilterCollections.Add(filters);
            PopulateFilters();
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            FilterEditControl filterControl = new FilterEditControl();
            MenuItem menuItem = (MenuItem)e.Source;
            //ItemCollection items = (ItemCollection)button.DataContext;
            CamlFilters parentFilters = (CamlFilters)menuItem.Tag;

            CamlFilter filter = new CamlFilter();
            filterControl.Initialize(SourceFields, filter);

            if (filterControl.ShowDialog(null, "Filter") == true)
            {
                parentFilters.Filters.Add(filter);
                PopulateFilters();
                /*
                items.Clear();
                PopulateFilters(items, parentFilters);
                */
            }
        }

        private void AddGroupButton_Click(object sender, RoutedEventArgs e)
        {
            FilterGroupEditControl filterControl = new FilterGroupEditControl();
            Button button = (Button)e.Source;
            ItemCollection items = (ItemCollection)button.DataContext;
            CamlFilters parentFilters = (CamlFilters)button.Tag;

            CamlFilters filters = new CamlFilters();
            filterControl.Initialize(filters);
            if (filterControl.ShowDialog(null, "Filter") == true)
            {
                parentFilters.FilterCollections.Add(filters);
                PopulateFilters();
                /*
                items.Clear();
                PopulateFilters(items, parentFilters);
                */
            }
        }

        private void PopulateFilter(ItemCollection items, CamlFilter filter)
        {
            Label label = new Label();
            label.Tag = filter;
            label.Content = filter.FieldName + " " + filter.FilterType.GetDescription() + " " + filter.FilterValue;
            label.MouseDoubleClick += Label_MouseDoubleClick;
            items.Add(label);
        }

        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FilterEditControl filterControl = new FilterEditControl();
            Label label = (Label)e.Source;
            CamlFilter filter = (CamlFilter)label.Tag;

            filterControl.Initialize(SourceFields, filter);
            if (filterControl.ShowDialog(null, "Filter") == true)
            {
                label.Content = filter.ToCaml();
            }
        }

        private void RootTreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FilterGroupEditControl filterControl = new FilterGroupEditControl();
            TreeViewItem tvi = (TreeViewItem)e.Source;
            CamlFilters filters = (CamlFilters)tvi.Tag;
            filterControl.Initialize(filters);
            if (filterControl.ShowDialog(null, "Filter") == true)
            {
                tvi.Header = filters.IsOr ? "OR" : "AND";
            }
        }

        private void SelectedViewFieldsLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Populate();
        }
    }
}
