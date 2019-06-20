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

namespace Sobiens.Connectors.Studio.UI.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemRichTextBoxControl : EditItemControl
    {
        public EditItemRichTextBoxControl()
        {
            InitializeComponent();
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
                return new TextRange(textBox1.Document.ContentStart, textBox1.Document.ContentEnd).Text;
            }
            set
            {
                textBox1.Document = new FlowDocument();
                textBox1.AppendText(value.ToString());
                oldValue = new TextRange(textBox1.Document.ContentStart, textBox1.Document.ContentEnd).Text;
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
