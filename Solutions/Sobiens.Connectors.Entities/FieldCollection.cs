using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class FieldCollection:List<Field>
    {
        public List<Field> GetEditableFields()
        {
            List<Field> fields = new List<Field>();
            foreach (Field field in this)
            {
                if (field.ReadOnly == true)
                    continue;
                fields.Add(field);
            }
            return fields;
        }
    }
}
