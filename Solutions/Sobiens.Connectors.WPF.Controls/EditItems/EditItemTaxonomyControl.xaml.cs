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
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.WPF.Controls.Selectors;

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemTaxonomyControl : EditItemControl
    {
        private string valueID;

        public EditItemTaxonomyControl()
        {
            InitializeComponent();
        }

        public override bool hasBeenModified
        {
            get
            {
                return valueID != null;
            }
        }

        public override object Value
        {
            get
            {
                string t = textBox1.Text;
                if (valueID != null) t += "|" + valueID;
                return  t;
            }
            set
            {
                textBox1.Text = Sobiens.Connectors.Common.tools.keepBehind(value.ToString(), ";#");
                //valueID=((Entities.ContentType)value).ID;
            }
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override void TakeFocus()
        {
            base.TakeFocus();
            textBox1.Focus();
        }

        private void SelectionButton_Click(object sender, RoutedEventArgs e)
        {
            SPTaxonomyField field = (SPTaxonomyField)this.Field;
            
            TermSetSelector termSetSelector = new TermSetSelector();
            termSetSelector.Initialize((SiteSetting)this.SiteSetting, this.WebURL, field);
            
            bool? dialogResult = termSetSelector.ShowDialog(null,Languages.Translate("Select:") + field.DisplayName);

            if (dialogResult.HasValue == true && dialogResult.Value == true)//JD
            {
                if (termSetSelector.SelectedTemSetsListView.Items.Count > 0)
                {
                    Term t = (Term)termSetSelector.SelectedTemSetsListView.Items[0];
                    textBox1.Text = t.Name;
                    this.valueID = t.Id.ToString();
                }
            }
        }
    }
}
