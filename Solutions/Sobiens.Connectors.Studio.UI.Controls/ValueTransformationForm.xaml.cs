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

namespace Sobiens.Connectors.Studio.UI.Controls
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class ValueTransformationForm : HostControl
    {
        public string ValueTransformationSyntax { get; set; }
        public ValueTransformationForm()
        {
            InitializeComponent();
        }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateValues();
        }

        private void TestValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateValues();
        }

        private void updateValues()
        {
            if (OutputTextBox == null)
                return;

            //object returnValue = ValueTransformationHelper.Transform("Hello World!", "return value.Replace(\"World\", \"coni\")");
            try
            {
                object returnValue = ValueTransformationHelper.Transform(TestValueTextBox.Text, CodeTextBox.Text);
                OutputTextBox.Text = returnValue.ToString();
                ValueTransformationSyntax = CodeTextBox.Text;
            }
            catch(Exception ex)
            {
                OutputTextBox.Text = "Invalid syntax";
                ValueTransformationSyntax = string.Empty;
            }
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ValueTransformationSyntax = string.Empty;
            this.Close(true);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.OKButtonSelected += ValueTransformationForm_OKButtonSelected;
            this.CancelButtonSelected += ValueTransformationForm_CancelButtonSelected;
        }

        private void ValueTransformationForm_CancelButtonSelected(object sender, EventArgs e)
        {
            this.Close(false);
        }

        private void ValueTransformationForm_OKButtonSelected(object sender, EventArgs e)
        {
            this.Close(true);
        }
    }
}
