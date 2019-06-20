using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Workflows
{
    public class Task
    {
        public Guid SiteSettingID { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime DueDate { get; set; }
        public int Complete { get; set; }
        public string AssignedTo { get; set; }
        public int AssignedToID { get; set; }
        public string RelatedContentUrl { get; set; }
        public string RelatedContentTitle { get; set; }
        public string ListUrl { get; set; }
    }
}
