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
    public partial class EditItemBooleanControl : EditItemControl
    {
        bool? oldValue;

        public EditItemBooleanControl()
        {
            InitializeComponent();
        }

        public override bool hasBeenModified
        {
            get
            {
                return checkBox1.IsChecked != oldValue;
            }
        }

        public override object Value
        {
            get
            {
                return checkBox1.IsChecked;
            }
            set
            {
                
                if (value == null)
                {
                    return;
                }

                if (value is bool)
                {
                    checkBox1.IsChecked = (bool)value;
                }
                else
                {
                    string cmp = value.ToString().ToUpper().Trim();
                    if (cmp == "TRUE" || cmp == "1" || cmp == "YES" || cmp == "Y")
                    {
                        checkBox1.IsChecked = true;
                    }
                    else
                    {
                        checkBox1.IsChecked = false;
                    }
                }
                oldValue = checkBox1.IsChecked;
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
