using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.SharePoint
{
    public class SPTaxonomyField:Field
    {
        public Guid AnchorId { get; set; }
        public Guid TermSetId { get; set; }
        public Guid SspId { get; set; }
        public int LCID { get; set; }
        public string Path { get; set; }

    }
}
