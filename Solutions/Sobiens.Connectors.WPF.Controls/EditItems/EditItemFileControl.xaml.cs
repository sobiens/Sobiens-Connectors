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
using System.Text.RegularExpressions;

namespace Sobiens.Connectors.WPF.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemFileControl : EditItemControl
    {
        public EditItemFileControl()
        {
            InitializeComponent();
        }

        string oldValue="";

        public override bool hasBeenModified
        {
            get
            {
                return Value.ToString()!=oldValue;
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
                string _value = value.ToString().cleanSharePointField();
                extLabel.Content = System.IO.Path.GetExtension(_value.ToString());
                textBox1.Text = System.IO.Path.GetFileNameWithoutExtension(_value.ToString());
                oldValue = textBox1.Text;
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
        }
    }
}
