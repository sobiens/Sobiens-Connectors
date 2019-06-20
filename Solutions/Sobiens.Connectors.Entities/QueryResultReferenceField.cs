using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryResultReferenceField
    {
        public QueryResultReferenceField()
        {
        }

        public SiteSetting SiteSetting { get; set; }
        public string SourceFieldName { get; set; }
        public string ReferenceListName { get; set; }
        public string ReferenceFilterFieldName { get; set; }
        public string ReferenceValueFieldName { get; set; }
        public string OutputName { get; set; }
    }
}
