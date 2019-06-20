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

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemMultiLineTextBoxControl : EditItemControl
    {
        public EditItemMultiLineTextBoxControl()
        {
            InitializeComponent();
        }

        string oldValue="";

        public override bool hasBeenModified
        {
            get
            {
                return textBox1.Text != oldValue;
            }
        }

        public override object Value
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = oldValue=value.ToString();
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
