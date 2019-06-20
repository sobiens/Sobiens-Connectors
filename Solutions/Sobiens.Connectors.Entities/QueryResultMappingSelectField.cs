using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryResultMappingSelectField
    {
        public QueryResultMappingSelectField()
        {
        }

        public QueryResultMappingSelectField(string fieldName, string staticValue, string outputHeaderName)
        {
            this.FieldName = fieldName;
            this.StaticValue = staticValue;
            this.OutputHeaderName = outputHeaderName;
        }

        public QueryResultMappingSelectField(string fieldName, string outputHeaderName)
        {
            this.FieldName = fieldName;
            this.OutputHeaderName = outputHeaderName;
        }

        public string FieldName { get; set; }
        public string StaticValue { get; set; }
        public string OutputHeaderName { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.StaticValue) == false)
                return this.StaticValue;

            if (string.IsNullOrEmpty(this.OutputHeaderName) == false)
                return this.OutputHeaderName;

            return this.FieldName;
        }
    }
}
