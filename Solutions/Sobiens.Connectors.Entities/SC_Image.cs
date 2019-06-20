using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SC_Image
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
        public string ImagePath { get; set; }
    }
}
