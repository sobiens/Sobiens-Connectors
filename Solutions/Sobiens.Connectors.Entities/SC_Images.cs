using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class SC_Images:List<SC_Image>
    {
        public DateTime LastTimeSynchronized { get; set; }

        public List<SC_Image> GetImages(string category)
        {
            return (from img in this where img.Category.Equals(category, StringComparison.InvariantCultureIgnoreCase) == true select img).ToList();
        }

        public SC_Image GetImageByID(Guid id)
        {
            return this.Single(t => t.ID.Equals(id));
        }
    }
}
