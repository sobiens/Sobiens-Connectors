using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Windows.Controls;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using System.Windows.Media.Imaging;
using Sobiens.WPF.Controls.Classes;
using System.Windows;
using Sobiens.Connectors.Entities.Workflows;

namespace Sobiens.Connectors.Common
{
    public class ContextMenuManager
    {
        private static ContextMenuManager _instance = null;
        public static ContextMenuManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ContextMenuManager();
                return _instance;
            }
        }

        private ContextMenuManager()
        {
        }

        public void FillContextMenuItems(ContextMenu contextMenu, SiteSetting siteSetting, object item, object inspector, object folder)
        {
            SC_MenuItems menuItems;
            if (item as IItem != null)
            {
                menuItems = ApplicationContext.Current.GetItemMenuItems(siteSetting, (IItem)item);
            }
            else if (item as Folder != null)
            {
                menuItems = ApplicationContext.Current.GetFolderMenuItems(siteSetting, (Folder)item);
            }
            else if (item as Task != null)
            {
                menuItems = ApplicationContext.Current.GetTaskMenuItems(siteSetting, (Task)item);
            }
            else
            {
                menuItems = ApplicationContext.Current.GetItemVersionMenuItems(siteSetting, (ItemVersion)item);
            }

            contextMenu.Items.Clear();
            contextMenu.RemoveHandler(MenuItem.ClickEvent, new RoutedEventHandler(mi_Click));
            contextMenu.AddHandler(MenuItem.ClickEvent, new RoutedEventHandler(mi_Click));
            AddContextMenuItems(contextMenu.Items, siteSetting, item, menuItems, inspector, folder);
        }

        private void AddContextMenuItems(ItemCollection itemCollection, SiteSetting siteSetting, object item, SC_MenuItems menuItems, object inspector, object folder)
        {
            foreach (SC_MenuItem menuItem in menuItems)
            {
                if (menuItem.ID == (int)SC_MenuItemTypes.Separator)
                {
                    itemCollection.Add(new Separator());
                    continue;
                }

                MenuItem mi = new MenuItem();
                mi.Header = menuItem.Title;
                mi.Icon = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri("/Sobiens.Connectors.WPF.Controls;component/Images/MenuItems/" + menuItem.Icon, UriKind.Relative))
                };
                mi.Tag = new object[] { menuItem.ID, siteSetting, item, inspector, folder };
                //mi.Click -= new System.Windows.RoutedEventHandler(mi_Click);
                //mi.Click += new System.Windows.RoutedEventHandler(mi_Click);
                
                itemCollection.Add(mi);

                AddContextMenuItems(mi.Items, siteSetting, item, menuItem.SubItems, inspector, folder);
            }
        }

        private void mi_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            object[] values = ((MenuItem)e.Source).Tag as object[];

            SC_MenuItemTypes menuItemType = (SC_MenuItemTypes)(int)values[0];
            ISiteSetting siteSetting = (SiteSetting)values[1];
            object obj = values[2];
            object inspector = values[3];
            object folder = values[4];
            
            ApplicationContext.Current.DoMenuItemAction(siteSetting, menuItemType, obj, new object[]{ inspector, folder});
        }
    }
}
