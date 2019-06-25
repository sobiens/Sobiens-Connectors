﻿using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for ResultPane.xaml
    /// </summary>
    public partial class ResultPane : UserControl
    {
        private ISiteSetting siteSetting;
        private CamlFilters filters;
        private List<CamlFieldRef> viewFields;
        private CamlQueryOptions queryOptions;
        private List<CamlOrderBy> orderBys;
        private string FolderServerRelativePath;

        public ResultPane()
        {
            InitializeComponent();
        }

        public void PopulateResults(ISiteSetting _siteSetting, string webUrl, string listName, CamlFilters _filters, List<CamlFieldRef> _viewFields, List<CamlOrderBy> _orderBys, CamlQueryOptions _queryOptions, string folderServerRelativePath)
        {
            this.FolderServerRelativePath = folderServerRelativePath;
            siteSetting = _siteSetting;
            filters = _filters;
            viewFields = _viewFields;
            orderBys= _orderBys;
            queryOptions = _queryOptions;
            string listItemCollectionPositionNext;
            int itemCount;
            List<IItem> items = ApplicationContext.Current.GetListItems(siteSetting, orderBys, filters, viewFields, queryOptions, webUrl, listName, out listItemCollectionPositionNext, out itemCount);
            DataTable dataTable = new DataTable();
            ResultGrid.Columns.Clear();
            foreach (CamlFieldRef fieldRef in _viewFields)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.IsReadOnly = true;
                column.Binding = new Binding(fieldRef.Name);
                column.Header = fieldRef.DisplayName;
                ResultGrid.Columns.Add(column);
                dataTable.Columns.Add(fieldRef.Name);
            }
            foreach (IItem item in items)
            {
                DataRow row = dataTable.NewRow();
                foreach (CamlFieldRef fieldRef in _viewFields)
                {
                    string key = fieldRef.Name;
                    if(item.Properties.ContainsKey(key) == true)
                        row[fieldRef.Name] = item.Properties[fieldRef.Name];
                }
                
                dataTable.Rows.Add(row);
            }

            ResultGrid.ItemsSource = dataTable.AsDataView();
            AttachContextMenu();
        }

        private void AttachContextMenu() {
            ContextMenu ctxMenu = new ContextMenu();
            ResultGrid.ContextMenu = ctxMenu;
            ResultGrid.ContextMenuOpening += ContextMenu_ContextMenuOpening;
        }

        private void WebPartPropertiesButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ResultGrid.ContextMenu.Items.Clear();
            object fileLeafRefObj = this.FolderServerRelativePath + "/" + ((DataRowView)ResultGrid.SelectedItem)["FileLeafRef"];
            if(fileLeafRefObj != null)
            {
                MenuItem webPartPropertiesMenuItem = new MenuItem();
                webPartPropertiesMenuItem.Click += WebPartPropertiesMenuItem_Click;
                webPartPropertiesMenuItem.Header = "WebPart Properties";
                webPartPropertiesMenuItem.Tag = new object[] { siteSetting, fileLeafRefObj };
                ResultGrid.ContextMenu.Items.Add(webPartPropertiesMenuItem);
            }
        }

        private void WebPartPropertiesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            object[] args = (object[])((MenuItem)sender).Tag;
            SiteSetting siteSetting = (SiteSetting)args[0];
            string fileLeafRef = args[1].ToString();
            WebPartPropertiesForm wpf = new WebPartPropertiesForm();
            wpf.Initialize(siteSetting, fileLeafRef);
            wpf.ShowDialog(null, "Webpart Properties", 500, 700,false, true);
        }
    }
}