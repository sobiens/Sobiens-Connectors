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
using System.Collections.ObjectModel;
using System.Windows.Threading;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Common.Interfaces;

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemComboBoxControl : EditItemControl
    {
        public List<BoolStringClass> TheList = new List<BoolStringClass>();

        public EditItemComboBoxControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(string webURL, Field field, IItem listItem, ContentType contentType, ISiteSetting siteSetting, string rootFolder)
        {
            base.InitializeControl(webURL, field, listItem, contentType, siteSetting, rootFolder);

            if (field.Type == FieldTypes.Choice)
            {
                foreach (ChoiceDataItem choiceItem in field.ChoiceItems)
                {
                    TheList.Add(new BoolStringClass { TheText = choiceItem.DisplayName, TheValue = choiceItem.Value });
                }
            }
            else if (field.Type == FieldTypes.Lookup)
            {
                IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(siteSetting.SiteSettingType);
                List<IItem> items = serviceManager.GetListItems(siteSetting, rootFolder, field.List.ToString(), false);
                foreach (IItem item in items)
                {
                    TheList.Add(new BoolStringClass { TheText = item.Title, TheValue = item.GetID() });
                }
            }
            else
            {
                //TODO: Log error and handle accordingly
                return;
            }

            comboBox1.ItemsSource = TheList;
            this.DataContext = this;
            //comboBox1.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.DataBind);
        }

        object oldValue;

        public override bool hasBeenModified
        {
            get
            {
                return comboBox1.SelectedValue != oldValue;
            }
        }


        public override object Value
        {
            get
            {
                return comboBox1.SelectedValue;
            }
            set
            {
                string[] separator = { ";#" };
                string[] val = ((string)value).Split(separator, StringSplitOptions.RemoveEmptyEntries);
                string setval = (val.Length > 1) ? val[1] : val[0];
                comboBox1.SelectedValue = setval;
                oldValue = setval;
            }
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

    }
}
