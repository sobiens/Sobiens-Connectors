using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities.Settings
{
    public class SiteSettings :List<SiteSetting>
    {
        public SiteSettings() 
        {
        }

        public SiteSetting this[Guid id]
        {
            get
            {
                return this.SingleOrDefault(t => t.ID.Equals(id));
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].ID.Equals(value.ID) == true)
                    {
                        this[i] = value;
                        return;
                    }
                }
            }
        }

        public void Remove(Guid id)
        {
            base.Remove(this[id]);
        }
    }
}
