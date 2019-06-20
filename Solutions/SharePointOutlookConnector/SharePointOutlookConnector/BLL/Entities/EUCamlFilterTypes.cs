using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public enum EUCamlFilterTypes
    {
        Equals = 0,
        NotEqual =1,
        Greater = 2,
        Lesser =3,
        EqualsGreater =4,
        EqualsLesser =5,
        BeginsWith =6,
        Contains =7
    }
}
