using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;
using System.Collections.Generic;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Extensions;
using Sobiens.Connectors.Entities.Settings;

namespace Sobiens.Connectors.Studio.UI.Controls
{

    public class ClosableTab : TabItem, IQueryPanel
    {
        public Guid ID { get; set; }
        private CriteriaPane _CriteriaPane;
        private ResultPane _ResultPane;
        private CamlTextEditorPane _CamlTextEditorPane;
        private Grid _Grid;

        // Constructor
        public ClosableTab()
        {
            // Create an instance of the usercontrol
            CloseableHeader closableTabHeader = new CloseableHeader();

            // Assign the usercontrol to the tab header
            this.Header = closableTabHeader;

            // Attach to the CloseableHeader events (Mouse Enter/Leave, Button Click, and Label resize)
            closableTabHeader.button_close.MouseEnter += new MouseEventHandler(button_close_MouseEnter);
            closableTabHeader.button_close.MouseLeave += new MouseEventHandler(button_close_MouseLeave);
            closableTabHeader.button_close.Click += new RoutedEventHandler(button_close_Click);
            closableTabHeader.label_TabTitle.SizeChanged += new SizeChangedEventHandler(label_TabTitle_SizeChanged);
            //this.Content
            _Grid = new Grid();
            _CriteriaPane = new CriteriaPane() ;
            _ResultPane = new ResultPane() ;
            _CamlTextEditorPane = new CamlTextEditorPane();
            _CriteriaPane.After_CriteriaChange += _CriteriaPane_After_CriteriaChange;

            this.Background = Brushes.Aquamarine;
            this.Content = _Grid;
            SetGridControls(true, true, true);
        }

        void _CriteriaPane_After_CriteriaChange()
        {
            Folder selectedFolder = this.AttachedObject;
            ISiteSetting siteSetting = this.SiteSetting;
            List<CamlFieldRef> viewFields = this.GetViewFields();
            CamlQueryOptions queryOptions = this.GetQueryOptions();
            CamlFilters filters = this.GetFilters();
            List<CamlOrderBy> orderBys = this.GetOrderBys();
            if(selectedFolder as Entities.SharePoint.SPFolder != null)
                _CamlTextEditorPane.PopulateCamlTextEditor(filters, viewFields, orderBys, queryOptions);
            else
                _CamlTextEditorPane.PopulateSQLServerTextEditor(selectedFolder.Title, filters, viewFields, orderBys, queryOptions);
        }

        public void SetGridControls(bool showCriteriaPane, bool showResultPane, bool showCamlTextEditorPane) 
        {
            int totalVisibleComponent = 0;
            if (showCriteriaPane == true)
            {
                _CriteriaPane.Visibility = System.Windows.Visibility.Visible;
                totalVisibleComponent++;
            }
            else 
            {
                _CriteriaPane.Visibility = System.Windows.Visibility.Hidden;
            }

            if (showResultPane == true)
            {
                _ResultPane.Visibility = System.Windows.Visibility.Visible;
                totalVisibleComponent++;
            }
            else
            {
                _ResultPane.Visibility = System.Windows.Visibility.Hidden;
            }

            if (showCamlTextEditorPane == true)
            {
                _CamlTextEditorPane.Visibility = System.Windows.Visibility.Visible;
                totalVisibleComponent++;
            }
            else
            {
                _CamlTextEditorPane.Visibility = System.Windows.Visibility.Hidden;
            }

            _Grid.Children.Clear();
            _Grid.RowDefinitions.Clear();
            if (totalVisibleComponent == 0) 
            {
            }
            else if (totalVisibleComponent == 1)
            {
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(100, GridUnitType.Star) });
                if (showCriteriaPane == true) 
                {
                    _CriteriaPane.SetValue(Grid.RowProperty, 0);
                    _Grid.Children.Add(_CriteriaPane);
                }
                if (showResultPane == true)
                {
                    _ResultPane.SetValue(Grid.RowProperty, 0);
                    _Grid.Children.Add(_ResultPane);
                }
                if (showCamlTextEditorPane == true)
                {
                    _CamlTextEditorPane.SetValue(Grid.RowProperty, 0);
                    _Grid.Children.Add(_CamlTextEditorPane);
                }
            }
            else if (totalVisibleComponent == 2)
            {
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Star) });
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10, GridUnitType.Pixel) });
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(50, GridUnitType.Star) });

                int currentRowIndex = 0;
                if (showCriteriaPane == true)
                {
                    _CriteriaPane.SetValue(Grid.RowProperty, 0);
                    _Grid.Children.Add(_CriteriaPane);
                    currentRowIndex = currentRowIndex+2;
                }
                if (showResultPane == true)
                {
                    _ResultPane.SetValue(Grid.RowProperty, currentRowIndex);
                    _Grid.Children.Add(_ResultPane);
                    currentRowIndex = currentRowIndex + 2;
                }
                if (showCamlTextEditorPane == true)
                {
                    _CamlTextEditorPane.SetValue(Grid.RowProperty, currentRowIndex);
                    _Grid.Children.Add(_CamlTextEditorPane);
                }
            }
            else if (totalVisibleComponent == 3)
            {
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33, GridUnitType.Star) });
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10, GridUnitType.Pixel) });
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33, GridUnitType.Star) });
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10, GridUnitType.Pixel) });
                _Grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(33, GridUnitType.Star) });


                GridSplitter gs1 = new GridSplitter() { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };
                gs1.SetValue(Grid.RowProperty, 1);
                GridSplitter gs2 = new GridSplitter() { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch };
                gs2.SetValue(Grid.RowProperty, 3);

                _CriteriaPane.SetValue(Grid.RowProperty, 0);
                _ResultPane.SetValue(Grid.RowProperty, 2);
                _CamlTextEditorPane.SetValue(Grid.RowProperty, 4);
                _Grid.Children.Add(_CriteriaPane);
                _Grid.Children.Add(_ResultPane);
                _Grid.Children.Add(_CamlTextEditorPane);
                _Grid.Children.Add(gs1);
                _Grid.Children.Add(gs2);
            }

            /*
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new System.Uri("pack://application:,,,/Sobiens.Connectors.Studio.UI.Controls;component/Style/MainStyle.xaml", UriKind.Absolute);
            Application.Current.Resources.MergedDictionaries.Add(dict);
            Style style = this.FindResource("TabItemStyle") as Style;
            this.Style = style; // "{DynamicResource TabItemStyle}"
            */
        }



        /// <summary>
        /// Property - Set the Title of the Tab
        /// </summary>
        public string Title
        {
            set
            {
                ((CloseableHeader)this.Header).label_TabTitle.Content = value;
            }
        }

        private ISiteSetting _SiteSetting;
        private Folder _AttachedObject;
        private string _FileName;
        public ISiteSetting SiteSetting { get { return _SiteSetting; } }
        public Folder AttachedObject { get { return _AttachedObject; } }
        public string FileName { get { return _FileName; } }

        public void Initialize(string filename, ISiteSetting siteSetting, Folder attachedObject)
        {
            _SiteSetting = siteSetting;
            _AttachedObject = attachedObject;
            _FileName = filename;
            _CriteriaPane.Initialize(attachedObject);
        }



        //
        // - - - Overrides  - - -
        //


        // Override OnSelected - Show the Close Button
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnUnSelected - Hide the Close Button
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
        }

        // Override OnMouseEnter - Show the Close Button
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Visible;
        }

        // Override OnMouseLeave - Hide the Close Button (If it is NOT selected)
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
            {
                ((CloseableHeader)this.Header).button_close.Visibility = Visibility.Hidden;
            }
        }





        //
        // - - - Event Handlers  - - -
        //


        // Button MouseEnter - When the mouse is over the button - change color to Red
        void button_close_MouseEnter(object sender, MouseEventArgs e)
        {
            //((CloseableHeader)this.Header).button_close.Foreground = Brushes.Red;
        }

        // Button MouseLeave - When mouse is no longer over button - change color back to black
        void button_close_MouseLeave(object sender, MouseEventArgs e)
        {
            //((CloseableHeader)this.Header).button_close.Foreground = Brushes.Black;
        }


        // Button Close Click - Remove the Tab - (or raise an event indicating a "CloseTab" event has occurred)
        void button_close_Click(object sender, RoutedEventArgs e)
        {
            ((TabControl)this.Parent).Items.Remove(this);
        }


        // Label SizeChanged - When the Size of the Label changes (due to setting the Title) set position of button properly
        void label_TabTitle_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ((CloseableHeader)this.Header).button_close.Margin = new Thickness(((CloseableHeader)this.Header).label_TabTitle.ActualWidth + 5, 3, 4, 0);
        }



        public void PopulateResults(ISiteSetting siteSetting, string webUrl, string listName, CamlFilters _filters, List<CamlFieldRef> _viewFields, List<CamlOrderBy> _orderBys, CamlQueryOptions _queryOptions, string folderServerRelativePath)
        {
            _ResultPane.PopulateResults(siteSetting, webUrl, listName, _filters, _viewFields, _orderBys, _queryOptions, folderServerRelativePath, _CriteriaPane.GetPrimaryFields());
        }

        public List<CamlFieldRef> GetViewFields() 
        {
            return _CriteriaPane.GetViewFields();
        }

        public List<CamlFieldRef> GetAllFields()
        {
            return _CriteriaPane.GetAllFields();
        }

        public CamlQueryOptions GetQueryOptions()
        {
            return _CriteriaPane.GetQueryOptions();
        }

        public List<CamlOrderBy> GetOrderBys()
        {
            return _CriteriaPane.GetOrderBys();
        }

        public CamlFilters GetFilters()
        {
            return _CriteriaPane.GetFilters();
        }

        public void ChangeCriteriaPaneVisibility()
        {
            bool showCriteriaPane=_CriteriaPane.Visibility== System.Windows.Visibility.Visible?false:true;
            bool showResultPane = _ResultPane.Visibility== System.Windows.Visibility.Visible?true:false;
            bool showCamlTextEditorPane=_CamlTextEditorPane.Visibility== System.Windows.Visibility.Visible?true:false;
            this.SetGridControls(showCriteriaPane, showResultPane, showCamlTextEditorPane);
        }
        public void ChangeCamlTextPaneVisibility()
        {
            bool showCriteriaPane = _CriteriaPane.Visibility == System.Windows.Visibility.Visible ? true : false;
            bool showResultPane = _ResultPane.Visibility == System.Windows.Visibility.Visible ? true : false;
            bool showCamlTextEditorPane = _CamlTextEditorPane.Visibility == System.Windows.Visibility.Visible ? false : true;
            this.SetGridControls(showCriteriaPane, showResultPane, showCamlTextEditorPane);
        }
        public void ChangeResultsPaneVisibility() 
        {
            bool showCriteriaPane = _CriteriaPane.Visibility == System.Windows.Visibility.Visible ? true : false;
            bool showResultPane = _ResultPane.Visibility == System.Windows.Visibility.Visible ? false : true;
            bool showCamlTextEditorPane = _CamlTextEditorPane.Visibility == System.Windows.Visibility.Visible ? true : false;
            this.SetGridControls(showCriteriaPane, showResultPane, showCamlTextEditorPane);
        }

        public QueryPanelObject GetQueryPanel()
        {
            QueryPanelObject queryPanelObject = new QueryPanelObject();
            queryPanelObject.Folder = this.AttachedObject;
            queryPanelObject.SiteSetting = (SiteSetting)this.SiteSetting;
            queryPanelObject.ViewFields = this.GetViewFields();
            queryPanelObject.QueryOptions = this.GetQueryOptions();
            queryPanelObject.Filters = this.GetFilters();
            queryPanelObject.OrderBys = this.GetOrderBys();

            return queryPanelObject;
        }

        public void Load(QueryPanelObject queryPanelObject)
        {

//            QueryPanelObject queryPanelObject = SerializationManager.ReadSettings<QueryPanelObject>(folderPath + "\\" + filename);

//            this._FileName = queryPanelObject.filename;
            this._AttachedObject = queryPanelObject.Folder;
            this._SiteSetting = queryPanelObject.SiteSetting;
            _CriteriaPane.Initialize(queryPanelObject.Folder, queryPanelObject.ViewFields, queryPanelObject.QueryOptions, queryPanelObject.OrderBys, queryPanelObject.Filters);
        }


    }
}