using Sobiens.Connectors.Common;
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
    public partial class AuditLogForm : HostControl
    {
        private ISiteSetting siteSetting;
        private string listName;
        private string itemId;

        public AuditLogForm()
        {
            InitializeComponent();
        }

        public void Initialize(ISiteSetting _siteSetting, string _listName, string _itemId)
        {
            siteSetting = _siteSetting;
            itemId = _itemId;
            listName = _listName;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            PopulateResults();
        }
        public void PopulateResults()
        {
            List<IItem> items = ApplicationContext.Current.GetAuditLogs(siteSetting, listName, itemId);
            DataTable dataTable = new DataTable();
            ResultGrid.Columns.Clear();
            if (items.Count > 0)
            {
                foreach (string propertyName in items[0].Properties.Keys)
                {
                    DataGridTextColumn column = new DataGridTextColumn();
                    column.IsReadOnly = true;
                    column.Binding = new Binding(propertyName);
                    column.Header = propertyName;
                    ResultGrid.Columns.Add(column);
                    dataTable.Columns.Add(propertyName);
                }
                foreach (IItem item in items)
                {
                    DataRow row = dataTable.NewRow();
                    foreach (string propertyName in item.Properties.Keys)
                    {
                        string key = propertyName;
                        if (item.Properties.ContainsKey(key) == true)
                            row[propertyName] = item.Properties[propertyName];
                    }

                    dataTable.Rows.Add(row);
                }

                ResultGrid.ItemsSource = dataTable.AsDataView();
            }
        }

    }
}
