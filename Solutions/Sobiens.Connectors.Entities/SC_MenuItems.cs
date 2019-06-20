using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Sobiens.Connectors.Entities
{
    public class SC_MenuItems:List<SC_MenuItem>
    {
        
        public SC_MenuItem Add(SC_MenuItemTypes menuItemType)
        {
            SC_MenuItem item = new SC_MenuItem(menuItemType);
            this.Add(item);
            return item;
        }
    }
}
