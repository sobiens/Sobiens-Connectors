using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUFieldCollection:List<EUField>
    {
        public List<EUField> GetEditableFields()
        {
            List<EUField> fields = new List<EUField>();
            foreach (EUField field in this)
            {
                if (field.ReadOnly == true)
                    continue;
                fields.Add(field);
            }
            return fields;
        }
    }
}
