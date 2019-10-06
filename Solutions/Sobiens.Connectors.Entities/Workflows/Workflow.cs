using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Workflows
{
    public class Workflow
    {
        public Guid SiteSettingID { get; set; }
        public string StatusPageUrl { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string TemplateId { get; set; }
        public string ListId { get; set; }
        public string SiteId { get; set; }
        public string WebId { get; set; }
        public string ItemId { get; set; }
        public string ItemGUID { get; set; }
        public string TaskListId { get; set; }
        public string AdminTaskListId { get; set; }
        public string Author { get; set; }
        public string InternalState { get; set; }
        public string StatusTitle { get; set; }
        public string ActivenessGroupName { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }
        public bool CanBeInitiated
        {
            get
            {
                if (this.InternalState == "2")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
