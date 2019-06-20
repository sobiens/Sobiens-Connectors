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
    public class DocumentTemplateMappings : List<DocumentTemplateMapping>
    {
        public DocumentTemplateMappings()
        {
        }

        public List<DocumentTemplateMapping> GetDocumentTemplateMappings(Guid templateID)
        {
            return (from s in this
                    where s.DocumentTemplateID.Equals(templateID) == true
                    select s).ToList();
        }

    }
}
