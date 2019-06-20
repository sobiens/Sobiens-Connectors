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
using System.Reflection;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class ItemPropertyMappingForm : HostControl
    {
        private List<ApplicationItemProperty> ApplicationItemProperties = null;
        private ContentType ContentType = null;
        public string SelectedApplicationPropertyID { get; private set; }
        public string SelectedServicePropertyID { get; private set; }
        public ItemPropertyMappingForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ItemPropertyMapping_Loaded);
        }

        public void Initialize(List<ApplicationItemProperty> applicationItemProperties, string selectedApplicationPropertyID, string selectedFieldID)
        {
            this.ApplicationItemProperties = applicationItemProperties;
            this.SelectedApplicationPropertyID = selectedApplicationPropertyID;
            this.SelectedServicePropertyID = selectedFieldID;
        }

        public void Initialize(List<ApplicationItemProperty> applicationItemProperties, ContentType contentType, string selectedApplicationPropertyID, string selectedFieldID)
        {
            this.ApplicationItemProperties = applicationItemProperties;
            this.ContentType = contentType;
            this.SelectedApplicationPropertyID = selectedApplicationPropertyID;
            this.SelectedServicePropertyID = selectedFieldID;
        }

        public void Initialize(List<ApplicationItemProperty> applicationItemProperties, ContentType contentType)
        {
            this.ApplicationItemProperties = applicationItemProperties;
            this.ContentType = contentType;
        }

        void ItemPropertyMapping_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadApplicationItemProperties();
            if (this.ContentType != null)
            {
                this.LoadFields();
                ServicePropertyTextBox.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                ServicePropertyTextBox.Text = this.SelectedServicePropertyID;
                ServicePropertyComboBox.Visibility = System.Windows.Visibility.Hidden;
            }

            this.Loaded -= new RoutedEventHandler(ItemPropertyMapping_Loaded);
        }

        private void LoadFields()
        {
            ServicePropertyComboBox.Items.Clear();
            foreach (Field field in this.ContentType.Fields)
            {
                ServicePropertyComboBox.Items.Add(new { Name = field.DisplayName, ID = field.Name });
            }

            if (ServicePropertyComboBox.Items.Count > 0)
            {
                if (string.IsNullOrEmpty(this.SelectedServicePropertyID) == true)
                {
                    ServicePropertyComboBox.SelectedIndex = 0;
                }
                else
                {
                    ServicePropertyComboBox.SelectedValue = this.SelectedServicePropertyID;
                }
            }
        }

        private void LoadApplicationItemProperties()
        {
            ApplicationItemPropertyComboBox.Items.Clear();
            foreach (ApplicationItemProperty applicationItemProperty in ApplicationItemProperties)
            {
                ApplicationItemPropertyComboBox.Items.Add(new { Name = Languages.Translate(applicationItemProperty.DisplayName), ID = applicationItemProperty.Name });
            }

            if (ApplicationItemPropertyComboBox.Items.Count > 0)
            {
                if (string.IsNullOrEmpty(this.SelectedApplicationPropertyID) == true)
                {
                    ApplicationItemPropertyComboBox.SelectedIndex = 0;
                }
                else
                {
                    ApplicationItemPropertyComboBox.SelectedValue = this.SelectedApplicationPropertyID;
                }
            }
        }

        private void ApplicationItemPropertyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ApplicationItemPropertyComboBox.SelectedValue != null)
            {
                this.SelectedApplicationPropertyID = ApplicationItemPropertyComboBox.SelectedValue.ToString();
            }
        }

        private void ServicePropertyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServicePropertyComboBox.SelectedValue != null)
            {
                this.SelectedServicePropertyID = ServicePropertyComboBox.SelectedValue.ToString();
            }
        }

        protected void ServicePropertyTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SelectedServicePropertyID = ServicePropertyTextBox.Text;
        }
    }
}
