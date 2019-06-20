using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Settings
{
    public class ItemPropertyMappings:List<ItemPropertyMapping>
    {
        public ItemPropertyMappings GetItems(string selectedContentTypeID)
        {
            ItemPropertyMappings mappings = new ItemPropertyMappings();
            var query = from items in this
                        where items.ContentTypeID != null
                        && items.ContentTypeID.Equals(selectedContentTypeID, StringComparison.InvariantCultureIgnoreCase)
                        select items;
            mappings.AddRange(query.ToList());
            return mappings;
        }

        public void UpdateFolderDisplayName(Folder folder)
        {
            foreach(ItemPropertyMapping ipm in this)
            {
                ipm.FolderDisplayName = folder.GetPath();
            }
        }
    }
}
