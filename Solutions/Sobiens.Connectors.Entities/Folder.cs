using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Sobiens.Connectors.Entities.SharePoint;
using Sobiens.Connectors.Entities.FileSystem;
using Sobiens.Connectors.Entities.Gmail;

namespace Sobiens.Connectors.Entities
{
#if General
    [Serializable]
#endif
    [XmlInclude(typeof(SPWeb))]
    [XmlInclude(typeof(SPList))]
    [XmlInclude(typeof(SPFolder))]
    [XmlInclude(typeof(FSFolder))]
    [XmlInclude(typeof(GFolder))]
    public abstract class Folder
    {
        public Folder() 
        {
            this.Folders = new List<Folder>();
        }
        public string UniqueIdentifier { get; set; }
        public string Title { get; set; }
        public bool ContainsItems { get; set; }
        public List<Folder> Folders { get; set; }
        public Guid SiteSettingID { get; set; }
        public bool Selected { get; set; }
        public string PrimaryIdFieldName = string.Empty;
        public string PrimaryNameFieldName = string.Empty;
        public string PrimaryFileReferenceFieldName = string.Empty;
        public string ModifiedByFieldName { get; set; }
        public string ModifiedOnFieldName { get; set; }

        public bool IsDocumentLibrary
        {
            get
            {
                if(this is SPFolder == true)
                {
                    return ((SPFolder)this).IsDocumentLibrary;
                }

                return false;
            }
        }
        /// <summary>
        /// States whether this is configured by administrator or personal entry
        /// </summary>
        public bool PublicFolder { get; set; }
        public virtual string IconName
        {
            get
            {
                return "FOLDER";
            }
        }

        public virtual bool CanUpload()
        {
            return false;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public abstract string GetRoot();

        public abstract string GetUrl();

        public abstract string GetWebUrl();

        public abstract string GetPath();

        public abstract string GetListName();

        public BasicFolderDefinition GetBasicFolderDefinition()
        {
            return PopulateBasicFolderDefinition(this);
        }

        private BasicFolderDefinition PopulateBasicFolderDefinition(Folder folder)
        {
            BasicFolderDefinition basicFolderDefinition = new BasicFolderDefinition();
            basicFolderDefinition.FolderUrl = folder.GetUrl();
            basicFolderDefinition.SiteSettingID = folder.SiteSettingID;
            basicFolderDefinition.FolderType = folder.GetType().FullName;
            basicFolderDefinition.Title = folder.Title;

            foreach (Folder subFolder in folder.Folders)
            {
                BasicFolderDefinition subBasicFolderDefinition = PopulateBasicFolderDefinition(subFolder);
                basicFolderDefinition.Folders.Add(subBasicFolderDefinition);
            }

            return basicFolderDefinition;
        }

        public Folder GetSelectedFolders()
        {
            Folder root = this.MemberwiseClone() as Folder;
            this.RemoveNotSelectedFolder(root);
            return root;

        }

        private void RemoveNotSelectedFolder(Folder folder)
        {
            for (int i = folder.Folders.Count - 1; i > -1; i--)
            {
                if (folder.Folders[i].Selected == false)
                {
                    folder.Folders.RemoveAt(i);
                }
                else
                {
                    this.RemoveNotSelectedFolder(folder.Folders[i]);
                }
            }
        }

    }
}
