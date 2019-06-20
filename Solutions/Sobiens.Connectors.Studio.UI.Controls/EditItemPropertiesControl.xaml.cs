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
    public partial class EditItemPropertiesControl : HostControl
    {
        public EditItemPropertiesControl()
        {
            InitializeComponent();
        }

        private Dictionary<string, string> _defaultValues { get; set; }
        private List<ApplicationItemProperty> _properties;
        private List<ContentType> _contentTypes;
        private FolderSettings _folderSettings;
        private FolderSetting _defaultFolderSetting;
        private ISiteSetting _siteSetting;
        private string _rootFolder;
        private string _webURL;
        private bool fieldRequiredPresnet;
        private bool fieldRequiredValidate = true;
        private Dictionary<object, object> mappings;
        private bool mappingIsSet = false;

        private bool _IsValid;
        private bool isFolder = false;
        private bool _displayFileName = true;

        public bool requiredFieldsOk = false;

        public EditItemPropertiesControl(string webURL, List<ApplicationItemProperty> properties, List<ContentType> contentTypes, FolderSettings folderSettings, FolderSetting defaultFolderSetting, ISiteSetting siteSetting, string rootFolder, Dictionary<string, string> defaultValues,bool displayFileName)
        {
            InitializeComponent();

            _contentTypes = contentTypes;
            _properties = properties;
            _folderSettings = folderSettings;
            _defaultFolderSetting = defaultFolderSetting;
            _siteSetting = siteSetting;
            _rootFolder = rootFolder;
            _defaultValues = defaultValues;
            _webURL = webURL;
            _displayFileName = displayFileName;

            CheckInAfter.IsChecked = siteSetting.CheckInAfterEditProperties;
            isFolder = GetProperty(defaultValues, "ows_FSObjType",true,false)=="1";

            foreach (ContentType contentType in _contentTypes)
            {
                ContentTypeComboBox.Items.Add(new { Name = contentType.Name, ID = contentType });
            }

            if (_contentTypes.Count > 0)
            {
                ContentTypeComboBox.SelectedIndex = 0;
            }

            displayGenericField(defaultValues);

        }

        private void displayGenericField(Dictionary<string, string> properties)
        {
            int height=0;
            Label versionLabel = new Label()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Content = string.Format("{0} : {1}", Languages.Translate("Version"), GetProperty(properties,"ows__UIVersionString")),
                Margin = new Thickness(10, height, 0, 0)
            };
            height += 20;
            Label CreatedLabel = new Label()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Content = string.Format("{0} {1} {2} {3}",
                    Languages.Translate("Created by"),
                    GetProperty(properties,"ows_Created_x0020_By",true,false),
                    Languages.Translate("the"),
                    GetProperty(properties,"ows_Created",true,true)),
                    Margin=new Thickness(10, height, 0, 0)
            };
            height+=20;
            Label ModifiedLabel = new Label()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Content = string.Format("{0} {1} {2} {3}",
                    Languages.Translate("Modified by"),
                    GetProperty(properties,"ows_Modified_x0020_By",true,false),
                    Languages.Translate("the"),
                    GetProperty(properties,"ows_Last_x0020_Modified",true,true)),
                    Margin=new Thickness(10, height, 0, 0)
            };
            height+=20;
            Label IDLabel = new Label()
            {
                HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                Content = string.Format("ID : {0}", GetProperty(properties,"ows__dlc_DocId")),
                Margin=new Thickness(10, height, 0, 0)
            };
            
            

            GenericFieldStackPanel.Children.Add(versionLabel);
            GenericFieldStackPanel.Children.Add(CreatedLabel);
            GenericFieldStackPanel.Children.Add(ModifiedLabel);
            GenericFieldStackPanel.Children.Add(IDLabel);
            

        }

        private string GetProperty(Dictionary<string, string> properties, string key)
        {
            return GetProperty(properties, key, false, false);
        }
        private string GetProperty(Dictionary<string,string> properties,string key,bool clean,bool toDate)
        {
            if (properties == null) return "-";
            if (!properties.ContainsKey(key)) return "-";

            if (!clean) return properties[key];
            string result=properties[key].cleanSharePointField();
            if (toDate) return DateTime.Parse(result).ToString();

            return result;
        }
        
        private void setContentType(ContentType contentType)
        {
            IServiceManager serviceManager = ServiceManagerFactory.GetServiceManager(_siteSetting.SiteSettingType);
            string[] folderFieldInLimitedView = new string[] { "FileLeafRef" };

            double height = 10;
            fieldRequiredPresnet = false;
            FieldMappingsStackPanel.Children.Clear();

            //Get default mappings
            ItemPropertyMappings itemPropertyMappings = null;
            if (_folderSettings != null)
            {
                itemPropertyMappings = _folderSettings.GetItemPropertyMappings(contentType.ID);
            }

            List<Field> filtredField = contentType.Fields;

            if (_siteSetting.limitFolderEditableProperties&isFolder)
            {
                filtredField = contentType.Fields.Where(f => folderFieldInLimitedView.Contains(f.Name)).ToList();
            }
            
            foreach (Field field in filtredField)
            {

                if (!_displayFileName&&field.Name=="FileLeafRef") continue;

                Label label = new Label() { Content = field.DisplayName.removeTextInsideParenthesis() };


                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                if (field.Required && !field.ReadOnly)
                {
                    label.Foreground = new SolidColorBrush(Colors.Red);
                    label.Content += "*";
                    fieldRequiredPresnet = true;
                }

                label.Margin = new Thickness(0, height, 5, 0);
                FieldMappingsStackPanel.Children.Add(label);

                //if (field.ReadOnly == false)
                //{
                EditItemControl editItemControl = EditItemManager.GetEditItemControl(_webURL, field, null, contentType, _siteSetting, _rootFolder);

                editItemControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                editItemControl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                editItemControl.Margin = new Thickness(160, height, 0, 0);//



                FieldMappingsStackPanel.Children.Add(editItemControl);
                if (_properties != null && _properties.Count > 0)
                {
                    //Set default mapping value
                    ApplicationItemProperty defaultApplicationItemProperty = null;
                    if (itemPropertyMappings != null)
                    {
                        ItemPropertyMapping mapping = itemPropertyMappings.Where(m => m.ServicePropertyName == field.Name).FirstOrDefault();
                        if (mapping != null)
                        {
                            defaultApplicationItemProperty = _properties.Where(p => p.Name == mapping.ApplicationPropertyName).FirstOrDefault();
                            editItemControl.IsEnabled = false;
                        }
                        else
                        {
                            mapping = _defaultFolderSetting.ItemPropertyMappings.Where(m => m.ServicePropertyName == field.Name).FirstOrDefault();
                            if (mapping != null)
                            {
                                defaultApplicationItemProperty = _properties.Where(p => p.Name == mapping.ApplicationPropertyName).FirstOrDefault();
                                editItemControl.IsEnabled = false;
                            }
                        }
                    }

                    EditItems.FieldMappingControl fieldMappingControl = new EditItems.FieldMappingControl(_properties, field, defaultApplicationItemProperty);
                    fieldMappingControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                    fieldMappingControl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    fieldMappingControl.Margin = new Thickness(350, height, 0, 0);

                    FieldMappingsStackPanel.Children.Add(fieldMappingControl);

                }

                height = height + editItemControl.GetHeight() + 5;

                if (_defaultValues != null)
                {
                    object[] args = { serviceManager, field, editItemControl };
                    Dispatcher.BeginInvoke(new setDefaultValuesDelegate(setDefaultValues), args);
                }


                //}

            }

            labelfieldRequired.Visibility = fieldRequiredPresnet ? Visibility.Visible : Visibility.Hidden;

        }

        private delegate void setDefaultValuesDelegate(IServiceManager serviceManager, Field field, EditItemControl editItemControl);

        private void setDefaultValues(IServiceManager serviceManager, Field field, EditItemControl editItemControl)
        {
            object val = serviceManager.GetProperty(_defaultValues, field.Name);
            if (val != null)
            {
                editItemControl.Value = val;
            }
        }

        private void ContentTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsInitialized && e.AddedItems.Count > 0)
            {
                setContentType((ContentType)ContentTypeComboBox.SelectedValue);
            }
        }

        public override bool IsValid
        {
            get
            {
                if (!fieldRequiredPresnet) return true;//no required field

                setMappings();

                if (fieldRequiredValidate)//All ok
                {
                    requiredFieldsOk = true;
                    return true;
                }

                requiredFieldsOk = false;

                if (MessageBox.Show(Languages.Translate("Please complete required field"), Languages.Translate("Information"), MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) ==
                    MessageBoxResult.Yes) return true;//continue anyway

                mappingIsSet = false;

                return false;//retry

            }
            set
            {
                this._IsValid = value;
            }
        }

        /// <summary>
        /// return Dictionary
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public Dictionary<object, object> GetValues(out ContentType contentType)
        {
            contentType = (ContentType)ContentTypeComboBox.SelectedValue;
            if (mappingIsSet) return mappings;
            return setMappings();
        }

        private Dictionary<object, object> setMappings()
        {
            mappingIsSet = true;
            mappings = new Dictionary<object, object>();
            fieldRequiredValidate = true;

            foreach (UIElement control in FieldMappingsStackPanel.Children)
            {
                if (control is Sobiens.Connectors.Studio.UI.Controls.EditItems.FieldMappingControl)
                {
                    Sobiens.Connectors.Studio.UI.Controls.EditItems.FieldMappingControl fieldMappingControl = control as Sobiens.Connectors.Studio.UI.Controls.EditItems.FieldMappingControl;
                    object value = fieldMappingControl.GetFieldValue();
                    if (value != null)
                    {
                        mappings.Add(fieldMappingControl.GetField(), value);
                    }
                }
            }
            foreach (UIElement control in FieldMappingsStackPanel.Children)
            {
                if (control is EditItemControl)
                {
                    EditItemControl editItemControl = control as EditItemControl;
                    if (!mappings.ContainsKey(editItemControl.Field))
                    {
                        if (editItemControl.hasBeenModified) mappings.Add(editItemControl.Field, editItemControl.Value);

                        bool isNull = editItemControl.Value == null;
                        if (!isNull) isNull = string.IsNullOrEmpty(editItemControl.Value.ToString());

                        if (editItemControl.Field.Required && isNull)
                            fieldRequiredValidate = false;
                    }
                }
            }
            return mappings;
        }
    }
}
