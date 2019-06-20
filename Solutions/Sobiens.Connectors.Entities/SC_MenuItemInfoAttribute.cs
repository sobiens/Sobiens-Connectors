using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SC_MenuItemInfoAttribute : Attribute
    {
        internal SC_MenuItemInfoAttribute(int id, string title, string icon)
        {
            this.ID = id;
            this.Title = title;
            this.Icon = icon;
        }

        public int ID { get; private set; }
        public string Title { get; private set; }
        public string Icon { get; private set; }
    }
}
