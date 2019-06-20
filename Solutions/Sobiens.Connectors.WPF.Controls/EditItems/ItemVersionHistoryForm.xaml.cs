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

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class ItemVersionHistoryForm : HostControl
    {
        private IItem SelectedItem { get; set; }
        private ISiteSetting SiteSetting { get; set; }

        public ItemVersionHistoryForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(ItemVersionHistoryForm_Loaded);

        }

        void ItemVersionHistoryForm_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= new RoutedEventHandler(ItemVersionHistoryForm_Loaded);
            MappingsListView.ContextMenu = new ContextMenu();
            this.RefreshVersionListView();
        }

        public void Initialize(ISiteSetting siteSetting, IItem selectedItem)
        {
            this.SelectedItem= selectedItem;
            this.SiteSetting = siteSetting;
        }

        private void RefreshVersionListView()
        {
            List<ItemVersion> versions = ServiceManagerFactory.GetServiceManager(this.SiteSetting.SiteSettingType).GetListItemVersions(this.SiteSetting, this.SelectedItem);
            MappingsListView.Items.Clear();
            foreach (ItemVersion itemVersion in versions)
            {
                MappingsListView.Items.Add(itemVersion);
            }
        }

        private void MappingsListView_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ItemVersion itemVersion = MappingsListView.SelectedItem as ItemVersion;
            SiteSetting siteSetting = ConfigurationManager.GetInstance().GetSiteSetting(itemVersion.SiteSettingID);
            ContextMenuManager.Instance.FillContextMenuItems(MappingsListView.ContextMenu, siteSetting, itemVersion, null, null);
        }


    }
}
