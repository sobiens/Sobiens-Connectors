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

        public FieldCollection GetUsableFields()
        {
            FieldCollection fields = new FieldCollection();
            foreach (Field field in this)
            {
                if (field.Type == FieldTypes.Unknown
                    || field.Type == FieldTypes.ByteArray
                    || field.Type == FieldTypes.ByteImage
                    || ((SQLServer.SQLField)field).SQLFieldTypeName == "geography"
                    || ((SQLServer.SQLField)field).SQLFieldTypeName == "geometry"
                    || ((SQLServer.SQLField)field).SQLFieldTypeName == "hierarchyid"
                    )
                    continue;
                fields.Add(field);
            }
            return fields;
        }

        public FieldCollection GetUnusableFields()
        {
            FieldCollection fields = new FieldCollection();
            foreach (Field field in this)
            {
                if (field.Type == FieldTypes.Unknown
                    || field.Type == FieldTypes.ByteArray
                    || field.Type == FieldTypes.ByteImage
                    || ((SQLServer.SQLField)field).SQLFieldTypeName == "geography"
                    || ((SQLServer.SQLField)field).SQLFieldTypeName == "geometry"
                    || ((SQLServer.SQLField)field).SQLFieldTypeName == "hierarchyid"
                    )
                {
                    fields.Add(field);
                }
            }
            return fields;
        }

        public bool HasUnusableFieldsPrimary()
        {
            FieldCollection fields = GetUnusableFields();
            foreach (Field field in fields)
            {
                if (field.IsPrimary == true)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
