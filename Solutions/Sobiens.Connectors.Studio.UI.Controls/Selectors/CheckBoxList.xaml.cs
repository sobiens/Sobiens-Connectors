using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sobiens.Connectors.Studio.UI.Controls.Selectors
{
    /// <summary>
    /// Interaction logic for CheckBoxList.xaml
    /// </summary>
    public partial class CheckBoxList : UserControl
    {
        public CheckBoxList()
        {
            InitializeComponent();
            Items = new ObservableCollection<CheckBoxListItem>();
            this.DataContext = this;
        }
        public List<CheckBoxListItem> SelectedItems 
        { 
            get 
            {
                ObservableCollection<CheckBoxListItem> items = (ObservableCollection<CheckBoxListItem>)listBoxZone.ItemsSource;
                return items.Where(t => t.IsChecked == true).ToList();
            }
        }

        public ObservableCollection<CheckBoxListItem> Items { get; set; }
        public class CheckBoxListItem
        {
            public string Text { get; set; }
            public object Value { get; set; }
            public bool? IsChecked { get; set; }
        }

        private void CheckBoxZone_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
//            ZoneText.Text = "Selected Zone Name= " + chkZone.Content.ToString();
//            ZoneValue.Text = "Selected Zone Value= " + chkZone.Tag.ToString();
        }
    }
}
