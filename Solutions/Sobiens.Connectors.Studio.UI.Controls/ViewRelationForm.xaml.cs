using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
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

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for ViewRelationForm.xaml
    /// </summary>
    public partial class ViewRelationForm : HostControl
    {
        private IQueryPanel _MasterQueryPanel;
        private IQueryPanel _DetailQueryPanel;

        public ViewRelationForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.OKButtonSelected += ViewRelationForm_OKButtonSelected;
        }

        void ViewRelationForm_OKButtonSelected(object sender, EventArgs e)
        {
            ViewRelation viewRelation = (ViewRelation)this.Tag;
            viewRelation.MasterFieldDisplayName = ((CamlFieldRef)((ComboBoxItem)MasterFieldDisplayNameComboBox.SelectedItem).Tag).Name;
            viewRelation.MasterFieldValueName = ((CamlFieldRef)((ComboBoxItem)MasterFieldValueNameComboBox.SelectedItem).Tag).Name;
            viewRelation.DetailFieldName = ((CamlFieldRef)((ComboBoxItem)DetailFieldNameComboBox.SelectedItem).Tag).Name;
        }

        public void Initialize(IQueryPanel masterQueryPanel, IQueryPanel detailQueryPanel)
        {
            _MasterQueryPanel = masterQueryPanel;
            _DetailQueryPanel = detailQueryPanel;

            List<CamlFieldRef> masterViewFields = _MasterQueryPanel.GetViewFields();
            List<CamlFieldRef> masterAllFields = _MasterQueryPanel.GetAllFields();

            foreach (CamlFieldRef fieldRef in masterViewFields)
            {
                MasterFieldDisplayNameComboBox.Items.Add(new ComboBoxItem()
                {
                    Content = fieldRef.DisplayName,
                    Tag = fieldRef
                });
            }

            foreach (CamlFieldRef fieldRef in masterAllFields)
            {
                MasterFieldValueNameComboBox.Items.Add(new ComboBoxItem()
                {
                    Content = fieldRef.DisplayName,
                    Tag = fieldRef
                });
            }

            List<CamlFieldRef> detailViewFields = _DetailQueryPanel.GetAllFields();

            foreach (CamlFieldRef fieldRef in detailViewFields)
            {
                DetailFieldNameComboBox.Items.Add(new ComboBoxItem()
                {
                    Content = fieldRef.DisplayName,
                    Tag = fieldRef
                });
            }
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HostControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
