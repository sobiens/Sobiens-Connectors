using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ViewRelation
    {
        public ViewRelation()
        {
            Relations = new List<ViewRelation>();
        }

        public static ViewRelation NewViewRelation()
        {
            ViewRelation viewRelation = new ViewRelation();
            viewRelation.ID = Guid.NewGuid();
            return viewRelation;
        }

        public Guid DetailQueryPanelID { get; set; }
        public Guid ID { get; set; }
        public Guid MasterViewRelationID { get; set; }
        public bool IsRoot { get; set; }
        public string Name { get; set; }
        public string DetailListName { get; set; }
        public string DetailSiteUrl { get; set; }
        public string MasterFieldDisplayName { get; set; }
        public string MasterFieldValueName { get; set; }
        //public string DetailGridID { get; set; }
        public string DetailFieldName { get; set; }
        public List<ViewRelation> Relations { get; set; }
    }
}
