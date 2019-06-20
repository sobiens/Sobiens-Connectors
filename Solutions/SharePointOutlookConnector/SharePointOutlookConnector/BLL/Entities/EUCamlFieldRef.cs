using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUCamlFieldRef
    {
        public EUCamlFieldRef(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
