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
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common;

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemCheckedListBoxControl : EditItemControl
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }

        private List<object> SelectedValues = new List<object>();

        public EditItemCheckedListBoxControl()
        {
            InitializeComponent();
        }

        public override void InitializeControl(string webURL, Field field, IItem listItem, ContentType contentType, ISiteSetting siteSetting, string rootFolder)
        {
            base.InitializeControl(webURL, field, listItem, contentType, siteSetting, rootFolder);

            TheList = new ObservableCollection<BoolStringClass>();

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

            this.DataContext = this;

            listBoxZone.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.DataBind);
        }

        private void CheckBoxZone_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            if (SelectedValues.Contains(chkZone.Tag) == false)
            {
                SelectedValues.Add(chkZone.Tag);
            }
        }

        private void CheckBoxZone_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chkZone = (CheckBox)sender;
            if (SelectedValues.Contains(chkZone.Tag))
            {
                SelectedValues.Remove(chkZone.Tag);
            }
        }

        string oldValue="";

        public override bool hasBeenModified
        {
            get
            {
                return Value.ToString() != oldValue;
            }
        }

        public override object Value
        {
            get
            {
                string value = String.Empty;
                foreach (object selectedValue in SelectedValues)
                {
                    value += selectedValue.ToString() + ";#;#";
                }
                return value;
            }
            set
            {
                string[] separator = { ";#" };
                oldValue = (string)value;
                string[] val = ((string) value).Split(separator, StringSplitOptions.RemoveEmptyEntries);
                List<CheckBox> checkboxes = FindVisualChildren<CheckBox>(listBoxZone);
                foreach (CheckBox checkbox in checkboxes)
                {
                    checkbox.IsChecked = val.Contains(checkbox.Tag);
                }
            }
        }

        public static List<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            List<T> list = new List<T>();
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        list.Add((T)child);
                    }

                    List<T> childItems = FindVisualChildren<T>(child);
                    if (childItems != null && childItems.Count() > 0)
                    {
                        foreach (var item in childItems)
                        {
                            list.Add(item);
                        }
                    }
                }
            }
            return list;
        }

        public override bool IsValid
        {
            get
            {
                return true;
            }
        }

        public override double GetHeight()
        {
            this.Measure(new Size(180, 256));
            return scrollViewer.DesiredSize.Height;
        }
    }
}
