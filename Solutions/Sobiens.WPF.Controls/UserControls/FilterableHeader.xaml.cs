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

namespace Sobiens.WPF.Controls.UserControls
{
    public delegate void FilterableHeader_FiltersChanged(FilterableHeader filterableHeader);

    /// <summary>
    /// Interaction logic for FilterableHeader.xaml
    /// </summary>
    public partial class FilterableHeader : UserControl
    {
        public FilterableHeader()
        {
            InitializeComponent();
        }

        public event FilterableHeader_FiltersChanged FiltersChanged;

        private bool HasChanged { get; set; }

        public List<ListBoxItemExt> FilterItems
        {
            get;
            set;
        }

        public List<string> SelectedItemTexts
        {
            get
            {
                List<string> filterValues = new List<string>();
                List<ListBoxItemExt> items = (List<ListBoxItemExt>)cboTask.ItemsSource;
                foreach (ListBoxItemExt item in items)
                {
                    if (item.IsChecked == true)
                        filterValues.Add(item.Content.ToString());
                }

                return filterValues;
            }
        }

        public List<string> NotSelectedItemTexts
        {
            get
            {
                List<string> filterValues = new List<string>();
                List<ListBoxItemExt> items = (List<ListBoxItemExt>)cboTask.ItemsSource;
                foreach (ListBoxItemExt item in items)
                {
                    if (item.IsChecked == false)
                        filterValues.Add(item.Content.ToString());
                }

                return filterValues;
            }
        }

        private string Header
        {
            get;
            set;
        }

        public void Initialize(List<ListBoxItemExt> items, string header)
        {
            this.FilterItems = items;
            this.Header = header;
            if (cboTask != null)
            {
                cboTask.ItemsSource = items;
                HeaderLabel.Content = header;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (this.FilterItems != null)
            {
                cboTask.ItemsSource = this.FilterItems;
                HeaderLabel.Content = this.Header;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HasChanged = false;

            HeaderLabel.Visibility = System.Windows.Visibility.Collapsed;
            cboTask.Visibility = System.Windows.Visibility.Visible;
            cboTask.IsDropDownOpen = true;
        }

        private void cboTask_DropDownClosed(object sender, EventArgs e)
        {
            HeaderLabel.Visibility = System.Windows.Visibility.Visible;
            cboTask.Visibility = System.Windows.Visibility.Collapsed;

            if (HasChanged == true)
            {
                if (FiltersChanged != null)
                {
                    FiltersChanged(this);
                }
            }
        }

        private void chkTask_Checked(object sender, RoutedEventArgs e)
        {
            HasChanged = true;
        }

        private void chkTask_Unchecked(object sender, RoutedEventArgs e)
        {
            HasChanged = true;
        }
    }
}
