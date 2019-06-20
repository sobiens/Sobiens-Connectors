using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ItemVersion
    {
        public ItemVersion()
        {
        }
        public string Version { get; set; }
        public string URL { get; set; }
        public Guid SiteSettingID { get; set; }
        public string WebURL { get; set; }
        public string Created { get; set; }
        public string CreatedBy { get; set; }
        public string Comments { get; set; }
        public string Size { get; set; }
        public string CreatedRaw { get; set; }
        public string CreatedByName { get; set; }
    }
}
