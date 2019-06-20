using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities.Documents;

namespace Sobiens.Connectors.Entities.Interfaces
{
    public interface IDocumentTemplateSelection
    {
        void RefreshControls();
        bool HasAnythingToDisplay { get; }
        bool IsDataLoaded { get; set; }
        void Initialize(SiteSettings siteSettings, List<DocumentTemplate> DocumentTemplates);
    }
}
