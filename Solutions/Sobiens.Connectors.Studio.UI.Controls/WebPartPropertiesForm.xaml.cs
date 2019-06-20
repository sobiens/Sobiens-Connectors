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
using Sobiens.Connectors.Services.SharePoint;
using Sobiens.Connectors.Entities.SharePoint;
using Microsoft.SharePoint.Client.WebParts;
using System.Windows.Data;
using Sobiens.Connectors.Entities.Data;
using System.Data;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for HierarchyNavigator.xaml
    /// </summary>
    public partial class WebPartPropertiesForm : HostControl
    {
        public SiteSetting SiteSetting;
        public string FileLeafRef;

        public WebPartPropertiesForm()
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

        public void Initialize(SiteSetting siteSetting, string fileLeafRef)
        {
            this.SiteSetting = siteSetting;
            this.FileLeafRef = fileLeafRef;
        }



        private void FoldersTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ResultGrid.Items.Clear();
            DataSet ds = new DataSet();
            OC_DataTable dt = new OC_DataTable();
            ds.Tables.Add(dt);
            DataColumn dtTypeColumn = dt.Columns.Add("Key");
            DataColumn dtPictureColumn = dt.Columns.Add("Value");

            SPWebpart webpart = ((TreeViewItem)WebpartsTreeView.SelectedItem).Tag as SPWebpart;
            foreach(string key in webpart.Properties.Keys)
            {
                OC_Datarow newRow = (OC_Datarow)dt.NewRow();
                newRow["Key"] = key;
                newRow["Value"] = webpart.Properties[key];
                newRow.Tag = webpart.Properties[key];
                dt.Rows.Add(newRow);
            }
            ResultGrid.ItemsSource = dt.AsDataView();

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SharePointService sharePointService = new SharePointService();
            List<SPWebpart> webparts = sharePointService.GetWebparts(this.SiteSetting, this.FileLeafRef);
            foreach(SPWebpart wp in webparts)
            {
                AddNode(WebpartsTreeView.Items, wp.Title, wp, Brushes.Black);
            }
            


        }

        private void SaveWebpartPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            string key = PropertiesLabel.Content.ToString();
            string value = ValueTextBox.Text;

            SPWebpart webpart = ((TreeViewItem)WebpartsTreeView.SelectedItem).Tag as SPWebpart;
            SharePointService sharePointService = new SharePointService();
            sharePointService.SaveWebpartProperty(this.SiteSetting, this.FileLeafRef, webpart.ID, key, value);

            DataRowView selectedRow = (DataRowView)ResultGrid.SelectedItem;
            selectedRow["Value"] = value;
            webpart.Properties[key] = value;
        }

        private void ResultGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRow = (DataRowView)ResultGrid.SelectedItem;
            string key = selectedRow["Key"].ToString();
            object value = selectedRow["Value"];
            PropertiesLabel.Content = key;
            ValueTextBox.Text = value.ToString();

        }
    }
}
