using Sobiens.Connectors.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class QueryResult
    {
        public QueryResult()
        {
            Fields = new string[] { };
            ReferenceFields = new List<Entities.QueryResultReferenceField>();
        }

        public static QueryResult NewQueryResult()
        {
            QueryResult queryResult = new QueryResult();
            queryResult.ID = Guid.NewGuid();
            return queryResult;
        }

        public Guid ID { get; set; }
        public string Name { get; set; }
        public bool IsDocumentLibrary { get; set; }
        public bool VersioningEnabled { get; set; }
        public string PrimaryIdFieldName { get; set; }
        public string PrimaryNameFieldName { get; set; }
        public string PrimaryFileReferenceFieldName { get; set; }
        public string ModifiedByFieldName { get; set; }
        public string ModifiedOnFieldName { get; set; }

        public SiteSetting SiteSetting { get; set; }
        public string FolderPath { get; set; }
        public string ListName { get; set; }
        public string[] Fields { get; set; }
        public CamlFilters Filters { get;set;}
        public List<QueryResultReferenceField> ReferenceFields { get; set; }
        
    }
}
