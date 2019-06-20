using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Settings
{
    public class FolderSettings:List<FolderSetting>
    {
        public FolderSettings() { }
        public FolderSetting this[string url]
        {
            get
            {
                return this.SingleOrDefault(t => t.Folder.GetUrl().Equals(url, StringComparison.InvariantCultureIgnoreCase));
            }
            set
            {
                FolderSetting folderSetting = this.SingleOrDefault(t => t.Folder.GetUrl().Equals(url, StringComparison.InvariantCultureIgnoreCase));
                folderSetting.ItemPropertyMappings = value.ItemPropertyMappings;
                folderSetting.BasicFolderDefinition = value.BasicFolderDefinition;
                folderSetting.Folder = value.Folder;
                folderSetting.ApplicationType = value.ApplicationType;
            }
        }

        public FolderSettings GetRelatedFolderSettings(string url)
        {
            var query = from folderSetting in this
                        where folderSetting.Folder != null
                        && url.ToLower().Contains(folderSetting.Folder.GetUrl().ToLower())
                        select folderSetting;

            List<FolderSetting> folderSettingArray = query.ToList();
            FolderSettings folderSettings = new FolderSettings();
            folderSettings.AddRange(folderSettingArray);
            return folderSettings;
        }

        public ItemPropertyMappings GetItemPropertyMappings(string contentTypeID)
        {
            ItemPropertyMappings itemPropertyMappings = new ItemPropertyMappings();

            foreach (FolderSetting fs in this)
            {
                ItemPropertyMappings tempItemPropertyMappings = fs.ItemPropertyMappings.GetItems(contentTypeID);
                if (fs.Folder != null)
                {
                    tempItemPropertyMappings.UpdateFolderDisplayName(fs.Folder);
                }

                itemPropertyMappings.AddRange(tempItemPropertyMappings);
            }

            return itemPropertyMappings;
        }

        public ItemPropertyMappings GetItemPropertyMappings()
        {
            ItemPropertyMappings itemPropertyMappings = new ItemPropertyMappings();

            foreach (FolderSetting fs in this)
            {
                if (fs.Folder != null)
                {
                    fs.ItemPropertyMappings.UpdateFolderDisplayName(fs.Folder);
                }

                itemPropertyMappings.AddRange(fs.ItemPropertyMappings);
            }

            return itemPropertyMappings;
        }

        public FolderSettings GetFolderSettings(ApplicationTypes applicationType)
        {
            var query = from folderSetting in this
                        where folderSetting.ApplicationType == applicationType
                        select folderSetting;
            List<FolderSetting> folderSettingArray = query.ToList();
            FolderSettings folderSettings = new FolderSettings();
            folderSettings.AddRange(folderSettingArray);
            return folderSettings;
        }

        public FolderSetting GetFolderSetting(ApplicationTypes applicationType, Folder folder)
        {
            var query = from folderSetting in this
                        where folderSetting.Folder != null 
                              && folderSetting.Folder.GetUrl().Equals(folder.GetUrl(), StringComparison.InvariantCultureIgnoreCase)
                              && folderSetting.ApplicationType.Equals(applicationType) == true
                        select folderSetting;
            return query.SingleOrDefault();
        }

        public FolderSetting GetDefaultFolderSetting(ApplicationTypes applicationType)
        {
            var query = from folderSetting in this
                        where folderSetting.ApplicationType.Equals(applicationType) == true
                              && folderSetting.Folder == null
                        select folderSetting;
            FolderSetting fs = query.SingleOrDefault();
            if (fs == null)
            {
                fs = new FolderSetting();
                fs.ApplicationType = applicationType;
                this.Add(fs);
            }

            return fs;
        }

        public FolderSetting GetDefaultFolderSetting()
        {
            var query = from folderSetting in this
                        where folderSetting.Folder == null
                        select folderSetting;
            FolderSetting fs = query.SingleOrDefault();
            if (fs == null)
            {
                fs = new FolderSetting();
                this.Add(fs);
            }

            return fs;
        }

        public ItemPropertyMapping GetItemPropertyMapping(Guid id, out FolderSetting folderSetting)
        {
            foreach (FolderSetting fs in this)
            {
                ItemPropertyMapping itemPropertyMapping = fs.ItemPropertyMappings.SingleOrDefault(t => t.ID.Equals(id));
                if (itemPropertyMapping != null)
                {
                    folderSetting = fs;
                    return itemPropertyMapping;
                }
            }

            folderSetting = null;
            return null;
        }

        public void DeleteItemPropertyMapping(Guid id)
        {
            foreach (FolderSetting fs in this)
            {
                ItemPropertyMapping itemPropertyMapping = fs.ItemPropertyMappings.SingleOrDefault(t => t.ID.Equals(id));
                if (itemPropertyMapping != null)
                {
                    fs.ItemPropertyMappings.Remove(itemPropertyMapping);
                }
            }
        }
    }
}
