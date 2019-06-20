using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    [Serializable]
    public class EUEmailMapping
    {
        public string OutlookFieldName { get; set; }
        public string SharePointFieldName  { get; set; }
    }
}
