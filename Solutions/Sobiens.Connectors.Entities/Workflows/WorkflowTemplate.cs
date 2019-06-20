using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Workflows
{
    public class WorkflowTemplate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateId { get; set; }
        public string BaseId { get; set; }
        public string AssociationData { get; set; }
        public string ImagePath
        {
            get
            {
                return "/Sobiens.Connectors.WPF.Controls;component/Images/MenuItems/WORKFLOWS.gif";
            }
        }
        public string Title
        {
            get
            {
                return this.Name;
            }
        }
        
    }
}
