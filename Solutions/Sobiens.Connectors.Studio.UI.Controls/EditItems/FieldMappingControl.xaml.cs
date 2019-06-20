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

namespace Sobiens.Connectors.Studio.UI.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for FieldMappingControl.xaml
    /// </summary>
    public partial class FieldMappingControl : UserControl
    {
        private Field _field;
        private List<ApplicationItemProperty> _properties;

        public FieldMappingControl(List<ApplicationItemProperty> properties, Field field, ApplicationItemProperty defaultApplicationItemProperty)
        {
            InitializeComponent();

            _field = field;
            _properties = properties;

            var defaultMapping = new { Name = "_Manual_Entry_", DisplayName = Languages.Translate("- Static -") };
            FieldMappingSelectionComboBox.Items.Add(defaultMapping);

            foreach (ApplicationItemProperty property in properties)
            {
                if (field.Type == FieldTypes.Boolean && property.Type == typeof(bool)
                    || (field.Type == FieldTypes.Choice
                        || field.Type == FieldTypes.ContentType
                        || field.Type == FieldTypes.ContentTypeId
                        || field.Type == FieldTypes.ContentType
                        || field.Type == FieldTypes.File
                        || field.Type == FieldTypes.Note
                        || field.Type == FieldTypes.Text
                        || field.Type == FieldTypes.User)
                    || (field.Type == FieldTypes.Counter
                        || field.Type == FieldTypes.Number)
                        && property.Type == typeof(decimal)
                    || field.Type == FieldTypes.DateTime
                        && property.Type == typeof(DateTime))
                {
                    var propertyMapping = new { Name = property.Name, DisplayName = Languages.Translate(property.DisplayName) };
                    FieldMappingSelectionComboBox.Items.Add(propertyMapping);

                    if (defaultApplicationItemProperty != null
                        && defaultApplicationItemProperty.Name == propertyMapping.Name)
                        //&& defaultApplicationItemProperty.DisplayName == propertyMapping.DisplayName)JD translation pb?
                    {
                        defaultMapping = propertyMapping;
                    }
                }
            }

            if (FieldMappingSelectionComboBox.Items.Count == 1) FieldMappingSelectionComboBox.Visibility = Visibility.Hidden;//hide selector if only one choice

            FieldMappingSelectionComboBox.SelectedItem = defaultMapping;
            FieldMappingSelectionComboBox.SelectionChanged += new SelectionChangedEventHandler(FieldMappingSelectionComboBox_SelectionChanged);
        }

        void FieldMappingSelectionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (UIElement element in ((Grid)this.Parent).Children)
            {
                if (element is EditItemControl)
                {
                    EditItemControl editItemControl = (EditItemControl)element;
                    if (editItemControl.Field == this._field)
                    {
                        if ((string)FieldMappingSelectionComboBox.SelectedValue == "_Manual_Entry_")
                        {
                            editItemControl.IsEnabled = true;
                        }
                        else
                        {
                            editItemControl.IsEnabled = false;
                        }
                    }
                }
            }
        }

        public Field GetField()
        {
            return _field;
        }

        public object GetFieldValue()
        {
            if (string.IsNullOrEmpty((string)FieldMappingSelectionComboBox.SelectedValue) || (string)FieldMappingSelectionComboBox.SelectedValue == "_Manual_Entry_")
            {
                return null;
            }
            else
            {
                return _properties.Where(p => p.Name == (string) FieldMappingSelectionComboBox.SelectedValue).FirstOrDefault();
            }
        }
    }
}
