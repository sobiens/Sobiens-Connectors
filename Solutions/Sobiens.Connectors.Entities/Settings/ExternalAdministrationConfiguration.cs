using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Documents;

namespace Sobiens.Connectors.Entities.Settings
{
    public class ExternalAdministrationConfiguration
    {
        public string Url { get; set; }
        public SiteSetting SiteSetting = new SiteSetting();
    }
}
