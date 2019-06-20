using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUFieldInformations:List<EUFieldInformation>
    {
        public EUContentType ContentType { get; set; }
    }
}
