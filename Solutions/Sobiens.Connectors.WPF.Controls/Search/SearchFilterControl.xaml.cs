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
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Search;
using Sobiens.Connectors.WPF.Controls.EditItems;

namespace Sobiens.Connectors.WPF.Controls.Search
{
    /// <summary>
    /// Interaction logic for SearchFilterControl.xaml
    /// </summary>
    public partial class SearchFilterControl : UserControl
    {
        public event EventHandler FilterAdded = null;
        public event EventHandler FilterRemoved = null;
        public EditItemControl editItemControl = null;
        private ContentType ContentType = null;
        private SearchFilter SearchFilter = null;
        private string WebURL = null;

        private bool _ReadOnly = true;
        public bool ReadOnly
        {
            get
            {
                return _ReadOnly;
            }
            set
            {
                _ReadOnly = value;
                this.SetReadOnly(value);
            }
        }

        public bool IsOr
        {
            get;
            set;
        }

        public SearchFilterControl()
        {
            InitializeComponent();
        }

        private void SetReadOnly(bool isReadOnly)
        {
            PropertyComboBox.Visibility = isReadOnly == true? System.Windows.Visibility.Hidden: System.Windows.Visibility.Visible;
            PropertyLabel.Visibility =  isReadOnly == false? System.Windows.Visibility.Hidden: System.Windows.Visibility.Visible;
            FilterTypeComboBox.Visibility = isReadOnly == true? System.Windows.Visibility.Hidden: System.Windows.Visibility.Visible;
            FilterTypeLabel.Visibility = isReadOnly == false ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            FilterValueControlPanel.Visibility = isReadOnly == true? System.Windows.Visibility.Hidden: System.Windows.Visibility.Visible;
            FilterValueLabel.Visibility = isReadOnly == false ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            AndOrComboBox.Visibility  = isReadOnly == true? System.Windows.Visibility.Hidden: System.Windows.Visibility.Visible;
            AndOrLabel.Visibility = isReadOnly == false ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            AddFilterButton.Visibility = isReadOnly == true ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            RemoveFilterButton.Visibility = isReadOnly == false ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;

            if (isReadOnly == true)
            {
                PropertyLabel.Content = this.SearchFilter.FieldName;
                FilterTypeLabel.Content = this.SearchFilter.FilterType.ToString();
                FilterValueLabel.Content = this.SearchFilter.FilterValue;
                AndOrLabel.Content =  Languages.Translate(this.IsOr == true ?"Or" : "And");
            }

        }

        public void Initialize(string webURL, ContentType contentType, SearchFilter filter, bool isOr)
        {
            this.WebURL = webURL;
            this.ContentType = contentType;
            this.IsOr = isOr;
            this.FillFilterTypeComboBox();
            this.FillPropertyComboBox();

            string fieldName = filter.FieldName;
            CamlFilterTypes filterType = filter.FilterType;
            FieldTypes fieldType = filter.FieldType;
            this.SearchFilter = new SearchFilter(fieldName, fieldType, filterType, filter.FilterValue);

            PropertyComboBox.SelectedValue = this.SearchFilter.FieldName;
            FilterTypeComboBox.SelectedValue = this.SearchFilter.FieldType;
            if (this.IsOr == true)
            {
                AndOrComboBox.SelectedIndex = 1;
            }
            else
            {
                AndOrComboBox.SelectedIndex = 0;
            }

            PropertyLabel.Content = this.SearchFilter.FieldName;
            FilterTypeLabel.Content = this.SearchFilter.FilterType.ToString();
            FilterValueLabel.Content = this.SearchFilter.FilterValue;
            AndOrLabel.Content =  Languages.Translate(this.IsOr ==true ? "Or" : "And");
        }



        private void FillPropertyComboBox()
        {
            PropertyComboBox.Items.Clear();
            foreach (Field field in this.ContentType.Fields)
            {
                PropertyComboBox.Items.Add(field);
            }
        }

        private void FillFilterTypeComboBox()
        {
            FilterTypeComboBox.Items.Clear();
            foreach (int value in Enum.GetValues(typeof(CamlFilterTypes)))
            {
                FilterTypeComboBox.Items.Add((CamlFilterTypes)value);
            }
        }

        public SearchFilter GetSearchFilter()
        {
            SearchFilter sf = this.SearchFilter.Clone();
            sf.FilterValue = this.editItemControl.Value.ToString();
            return sf;
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterAdded != null)
            {
                FilterAdded(this, e);
            }
        }

        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterRemoved != null)
            {
                FilterRemoved(this, e);
            }
        }

        private void AndOrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.IsOr = ((ComboBoxItem)AndOrComboBox.SelectedValue).Content.ToString().ToLowerInvariant() == "or"?true:false ;
        }
        
        private void FilterTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CamlFilterTypes filterType = (CamlFilterTypes)FilterTypeComboBox.SelectedValue;
            this.SearchFilter.FilterType = filterType;
            editItemControl.TakeFocus();
        }

        private void PropertyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Field field = (Field)PropertyComboBox.SelectedValue;
            this.SearchFilter.FieldName = field.Name;
            //TODO: Check if last two parameters null causes a problem -> that is to say, can it have a lookup? If so, get them somehow!
            editItemControl = EditItemManager.GetEditItemControl(this.WebURL, field, null, this.ContentType, null, null);
            FilterValueControlPanel.Children.Clear();
            FilterValueControlPanel.Children.Add(editItemControl);

            if (FilterTypeComboBox.SelectedItem == null)
            {
                FilterTypeComboBox.SelectedIndex = 0;
            }
        }
    }
}
