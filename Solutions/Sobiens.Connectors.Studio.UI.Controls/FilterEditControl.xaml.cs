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
using System.Collections;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Studio.UI.Controls.EditItems;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Interfaces;
using System.ComponentModel;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for EditItemPropertiesControl.xaml
    /// </summary>
    public partial class FilterEditControl : HostControl
    {
        public FieldCollection Fields = null;
        public CamlFilter Filter = null;
        private List<ListViewItem> ComparisonTypes = new List<ListViewItem>();

        public FilterEditControl()
        {
            InitializeComponent();
        }

        public void Initialize(FieldCollection fields, CamlFilter filter)
        {
            this.Fields = fields;
            this.Filter = filter;
            FieldComboBox.ItemsSource = fields;

            ComparisonTypes.Add(new ListViewItem() { Content = "is equal to", Tag = CamlFilterTypes.Equals });
            ComparisonTypes.Add(new ListViewItem() { Content = "is not equal to", Tag = CamlFilterTypes.NotEqual });
            ComparisonTypes.Add(new ListViewItem() { Content = "is greater than", Tag = CamlFilterTypes.Greater });
            ComparisonTypes.Add(new ListViewItem() { Content = "is less than", Tag = CamlFilterTypes.Lesser });
            ComparisonTypes.Add(new ListViewItem() { Content = "is greater than or equal to", Tag = CamlFilterTypes.EqualsGreater });
            ComparisonTypes.Add(new ListViewItem() { Content = "is less than or equal to", Tag = CamlFilterTypes.EqualsLesser });
            ComparisonTypes.Add(new ListViewItem() { Content = "begins with", Tag = CamlFilterTypes.BeginsWith });
            ComparisonTypes.Add(new ListViewItem() { Content = "contains", Tag = CamlFilterTypes.Contains });
            ComparisonTypeComboBox.ItemsSource = ComparisonTypes;
        }

        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.OKButtonSelected += FilterGroupEditControl_OKButtonSelected;
            ListViewItem lvi = ComparisonTypes.Where(t => (CamlFilterTypes)t.Tag == this.Filter.FilterType).FirstOrDefault();
            if(lvi != null)
            {
                ComparisonTypeComboBox.SelectedItem = lvi;
            }

            Field field = this.Fields.Where(t => t.Name.Equals(this.Filter.FieldName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
            FieldComboBox.SelectedItem = field;

            FilterValueTextBox.Text = this.Filter.FilterValue;
        }

        private void FilterGroupEditControl_OKButtonSelected(object sender, EventArgs e)
        {
            Field field = (Field)FieldComboBox.SelectedItem;
            this.Filter.FieldName = field.Name;
            this.Filter.FieldType = field.Type;
            this.Filter.FilterType = (CamlFilterTypes)((ListViewItem)ComparisonTypeComboBox.SelectedItem).Tag;
            this.Filter.FilterValue = FilterValueTextBox.Text;
        }

    }
}
