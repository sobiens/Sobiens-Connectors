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
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Studio.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class CheckInForm : HostControl
    {
        private IItem SelectedItem { get; set; }
        private ISiteSetting SiteSetting { get; set; }

        public CheckinTypes CheckInType { get; private set; }
        public string Comments { get; private set; }
        public bool KeepDocumentCheckedOut { get; private set; }

        public CheckInForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ItemVersionHistoryForm_Loaded);
        }

        void CheckInForm_OKButtonSelected(object sender, EventArgs e)
        {
            if (DraftRadioButton.IsChecked == true)
            {
                this.CheckInType = CheckinTypes.MinorCheckIn;
            }
            else if (PublishRadioButton.IsChecked == true)
            {
                this.CheckInType = CheckinTypes.MajorCheckIn;
            }
            else
            {
                this.CheckInType = CheckinTypes.OverwriteCheckIn;
            }

            this.Comments = CommentsTextBox.Text;
            this.KeepDocumentCheckedOut = KeepDocumentCheckedOutRadioButton.IsChecked.Value;
        }

        void ItemVersionHistoryForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(ItemVersionHistoryForm_Loaded);
            this.OKButtonSelected += new EventHandler(CheckInForm_OKButtonSelected);
            this.SetControlValues();
        }

        public void Initialize(ISiteSetting siteSetting, IItem selectedItem)
        {
            this.SelectedItem= selectedItem;
            this.SiteSetting = siteSetting;
        }

        private void SetControlValues()
        {
            DraftRadioButton.Content = this.SelectedItem.GetMajorVersion() + "." + (SelectedItem.GetMinorVersion() + 1) + Languages.Translate("Minor version");
            PublishRadioButton.Content = (SelectedItem.GetMajorVersion() + 1) + Languages.Translate("Major version");
            OverwriteRadioButton.Content = SelectedItem.GetMajorVersion() + "." + SelectedItem.GetMinorVersion() + Languages.Translate(" Overwrite the current minor version");

            if (SiteSetting.useMajorVersionAsDefault) this.PublishRadioButton.IsChecked = true;
        }



    }
}
