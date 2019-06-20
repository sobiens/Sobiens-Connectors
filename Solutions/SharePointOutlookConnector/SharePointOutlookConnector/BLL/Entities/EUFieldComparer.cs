using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUFieldComparer:IComparer<EUField>
    {
        public Int32 Compare(EUField firstField, EUField secondField)
        {
            return String.Compare(firstField.DisplayName, secondField.DisplayName);
        }
    }
}
