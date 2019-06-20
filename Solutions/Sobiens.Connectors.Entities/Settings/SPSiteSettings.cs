using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Settings
{
    public class SPSiteSetting:SiteSetting
    {
        public override string ToString()
        {
            return this.Url;
        }
        public SPSiteSetting() { } 
    }
}
