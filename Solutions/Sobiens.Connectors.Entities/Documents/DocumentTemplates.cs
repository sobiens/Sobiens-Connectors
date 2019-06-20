using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Interfaces;

namespace Sobiens.Connectors.Entities.Documents
{
#if General
    [Serializable]
#endif
    public class DocumentTemplates:List<DocumentTemplate>
    {
        public DocumentTemplates()
        {
        }

        public DocumentTemplate this[Guid templateID]
        {
            get
            {
                return this.SingleOrDefault(t => t.ID.Equals(templateID));
            }
        }

        public List<DocumentTemplate> GetDocumentTemplates(ApplicationTypes applicationType)
        { 
            return (from s in this
                        where s.ApplicationType.Equals(applicationType) == true
                        select s).ToList();
        }
    }
}
