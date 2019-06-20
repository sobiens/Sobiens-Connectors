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

namespace Sobiens.Connectors.WPFControlsTest
{
    /// <summary>
    /// Interaction logic for FilterableHeader.xaml
    /// </summary>
    public partial class FilterableHeader : UserControl
    {
        public FilterableHeader()
        {
            InitializeComponent();
        }

        public ItemCollection FilterItems
        {
            get;
            set;
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            List<ListBoxItem> items = new List<ListBoxItem>();
            for(int i=0;i<10;i++)
            {
                ListBoxItem li = new ListBoxItem();
                li.Content = "test " + i;
                
                items.Add(li);
            }
            cboTask.ItemsSource = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HeaderLabel.Visibility = System.Windows.Visibility.Collapsed;
            cboTask.Visibility = System.Windows.Visibility.Visible;
            cboTask.IsDropDownOpen = true;
        }

        private void cboTask_DropDownClosed(object sender, EventArgs e)
        {
            HeaderLabel.Visibility = System.Windows.Visibility.Visible;
            cboTask.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
