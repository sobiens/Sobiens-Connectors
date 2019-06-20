using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SC_MenuItem
    {
        public SC_MenuItem()
        {
            this.SubItems = new SC_MenuItems();
        }

        public SC_MenuItem(SC_MenuItemTypes menuItemType):this()
        {
            var menuItemInfo = menuItemType.GetAttribute<SC_MenuItemInfoAttribute>();
            this.ID = menuItemInfo.ID;
            this.Title = Languages.Translate(menuItemInfo.Title);
            this.Icon = menuItemInfo.Icon;
        }

        public int ID { get; private set; }
        public string Title { get; private set; }
        public string Icon { get; private set; }
        public SC_MenuItems SubItems { get; private set; }
    }
}
