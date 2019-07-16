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

        public QueryResultMappingSelectField(string fieldName, string staticValue, string outputHeaderName, string valueTransformationSyntax)
        {
            this.FieldName = fieldName;
            this.StaticValue = staticValue;
            this.OutputHeaderName = outputHeaderName;
            this.ValueTransformationSyntax = valueTransformationSyntax;
        }
        public QueryResultMappingSelectField(string fieldName, string staticValue, string outputHeaderName) : this(fieldName, staticValue, outputHeaderName, string.Empty)
        {
        }

        public QueryResultMappingSelectField(string fieldName, string outputHeaderName):this(fieldName, string.Empty, outputHeaderName, string.Empty)
        {
        }

        public string FieldName { get; set; }
        public string StaticValue { get; set; }
        public string OutputHeaderName { get; set; }
        public string ValueTransformationSyntax { get; set; }

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
