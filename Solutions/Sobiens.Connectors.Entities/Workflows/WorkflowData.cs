using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Workflows
{
    public class WorkflowData
    {
        public ToDoData ToDoData { get; set; }
        public TemplateData TemplateData { get; set; }
        public ActiveWorkflowsData ActiveWorkflowsData { get; set; }

        public WorkflowData()
        {
            this.ToDoData = new ToDoData();
            this.TemplateData = new TemplateData();
            this.ActiveWorkflowsData = new ActiveWorkflowsData();
        }
    }
}
