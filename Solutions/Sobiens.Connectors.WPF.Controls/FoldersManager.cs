using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.WPF.Controls.Settings;
using Sobiens.Connectors.Entities.SharePoint;

namespace Sobiens.Connectors.WPF.Controls
{
    public class FoldersManager
    {
        public static void DeleteItem(ISiteSetting siteSetting, Folder folder)
        {
        }

        public static void OpenItem(ISiteSetting siteSetting, Folder folder)
        {
        }

        public static void EditItemPropertyMappings(ISiteSetting siteSetting, Folder folder)
        {
            FolderSettings folderSettings = ConfigurationManager.GetInstance().Configuration.FolderSettings;
            ItemPropertyMappingsForm itemPropertyMappings = new ItemPropertyMappingsForm();
            itemPropertyMappings.Initialize(ConfigurationManager.GetInstance().Configuration.SiteSettings, folder, ApplicationContext.Current.GetApplicationType(), ConfigurationManager.GetInstance().Configuration.FolderSettings);
            if (itemPropertyMappings.ShowDialog(null, Languages.Translate("Item Property Mappings")) == true)
            {
                ConfigurationManager.GetInstance().SaveAppConfiguration();
            }
            else
            {
                ConfigurationManager.GetInstance().LoadConfiguration();
            }
        }

        public static SC_MenuItems GetFolderMenuItems(ISiteSetting siteSetting, Folder folder)
        {
            SC_MenuItems menuItems = new SC_MenuItems();
            menuItems.Add(SC_MenuItemTypes.OpenFolder);

            if (ItemsManager.GetCopiedItemInfo() != null) menuItems.Add(SC_MenuItemTypes.PasteItem);

            SC_MenuItem newMenuItem = new SC_MenuItem(SC_MenuItemTypes.New);
            newMenuItem.SubItems.Add(SC_MenuItemTypes.AddFolder);
            menuItems.Add(newMenuItem);

            if (folder as SPFolder != null)
            {
                menuItems.Add(SC_MenuItemTypes.EditItemPropertyMappings);
            }

            SC_MenuItem displayMenuItem = new SC_MenuItem(SC_MenuItemTypes.Display);
            displayMenuItem.SubItems.Add(SC_MenuItemTypes.Inexplorer);
            displayMenuItem.SubItems.Add(SC_MenuItemTypes.Innavigator);
            menuItems.Add(displayMenuItem);

            if (folder as SPFolder != null)
            {
                menuItems.Add(SC_MenuItemTypes.EditItem);
            }

            return menuItems;
        }

        
    }
}
