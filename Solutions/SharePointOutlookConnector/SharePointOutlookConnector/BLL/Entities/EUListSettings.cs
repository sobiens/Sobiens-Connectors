using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUListSetting
    {
        public string RootFolderPath { get; set; }
        public string WebURL { get; set; }
        public string ListName { get; set; }
        public List<EUEmailMapping> EmailMappings = new List<EUEmailMapping>();
        public override string ToString()
        {
            if (RootFolderPath == null || RootFolderPath == String.Empty)
                return "Default Settings";
            return RootFolderPath;
        }
        public EUListSetting() { } // keeps XmlSeralizer happy
    }
}
