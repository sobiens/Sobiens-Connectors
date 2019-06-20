using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUContentType
    {
      public string ID;
      public string Name;
      public string Group;
      public string Description;
      public string Version;
      public EUFieldCollection Fields;
      public override string ToString()
      {
          return Name;
      }
    }
}
