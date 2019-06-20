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
    public class DocumentTemplate
    {
        public DocumentTemplate()
        {
            //this.ImagePath = @"c:\temp\SampleTemplate.png";
        }

        public Guid ID { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ImageID { get; set; }
        public string ImagePath { get; set; }
        public string ImageURL { get; set; }
        public string TemplateURL { get; set; }
        public string TemplatePath { get; set; }

        public override string ToString()
        {
            return "[" + ApplicationType.ToString() + "]:" + this.Title;
        }
    }
}
