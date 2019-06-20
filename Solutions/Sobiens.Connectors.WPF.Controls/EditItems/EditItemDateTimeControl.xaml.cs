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
    public partial class EditItemDateTimeControl : EditItemControl
    {
        public EditItemDateTimeControl()
        {
            InitializeComponent();
        }

        DateTime? oldValue;

        public override bool hasBeenModified
        {
            get
            {
                return this.datePicker1.SelectedDate != oldValue;
            }
        }

        public override object Value
        {
            get
            {
                return this.datePicker1.SelectedDate;
            }
            set
            {
                if (value == null)
                {
                    return;
                }

                if (value is DateTime)
                {
                    this.datePicker1.SelectedDate = (DateTime)value;
                }
                else
                {
                    string dateString = value.ToString().Trim();
                    this.datePicker1.SelectedDate = DateTime.Parse(dateString);
                }
                oldValue = datePicker1.SelectedDate;
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
