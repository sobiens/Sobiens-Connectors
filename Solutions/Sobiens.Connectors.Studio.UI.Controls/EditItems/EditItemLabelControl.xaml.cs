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

namespace Sobiens.Connectors.Studio.UI.Controls.EditItems
{
    /// <summary>
    /// Interaction logic for EditItemBooleanControl.xaml
    /// </summary>
    public partial class EditItemLabelControl : EditItemControl
    {
        public EditItemLabelControl()
        {
            InitializeComponent();
        }

        string oldValue="";

        public override bool hasBeenModified
        {
            get
            {
                return false;
            }
        }

        public override object Value
        {
            get
            {
                return Label1.Content;
            }
            set
            {
                //remove extra data
                string[] txt = value.ToString().Split(new string[]{"w|",";#"},StringSplitOptions.None);
                string cleanTxt=txt[txt.Length-1];
                //remove texte into parentheses
                Label1.Content = Regex.Replace(cleanTxt, @" ?\(.*?\)", "");
                oldValue = Label1.Content.ToString();
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
