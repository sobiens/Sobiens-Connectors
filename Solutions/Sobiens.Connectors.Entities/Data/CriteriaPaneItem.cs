using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Data
{
    public class CriteriaPaneItem
    {
        public string FieldInternalName { get; set; }
        public string FieldName { get; set; }
        public FieldTypes FieldType { get; set; }
        public bool Output { get; set; }
        public string SortType { get; set; }
        public string SortOrder { get; set; }
        public string Filter1 { get; set; }
        public string Filter2 { get; set; }
        public string Filter3 { get; set; }
        public string Filter4 { get; set; }
        public bool IsPrimary { get; set; }
    }
}
