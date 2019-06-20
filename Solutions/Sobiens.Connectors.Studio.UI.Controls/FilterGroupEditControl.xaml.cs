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

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for EditItemPropertiesControl.xaml
    /// </summary>
    public partial class FilterGroupEditControl : HostControl
    {
        public CamlFilters Filters = null;
        public FilterGroupEditControl()
        {
            InitializeComponent();
        }

        public void Initialize(CamlFilters filters)
        {
            this.Filters = filters;
        }

        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.OKButtonSelected += FilterGroupEditControl_OKButtonSelected;
            if(this.Filters.IsOr == true)
            {
                ORRadioButton.IsChecked = true;
            }
            else
            {
                ANDRadioButton.IsChecked = true;
            }
        }

        private void FilterGroupEditControl_OKButtonSelected(object sender, EventArgs e)
        {
            this.Filters.IsOr = ORRadioButton.IsChecked.Value;
        }
    }
}
