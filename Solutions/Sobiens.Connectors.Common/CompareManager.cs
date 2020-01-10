using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sobiens.Connectors.Common
{
    public class CompareManager
    {
        private static CompareManager _Instance = null;
        public static CompareManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new CompareManager();
                }

                return _Instance;
            }
        }

        public List<CompareObjectsResult> GetObjectsDifferences(ISiteSetting sourceSiteSetting, Folder sourceParentObject, List<Folder> sourceObjects, ISiteSetting destinationSiteSetting, List<Folder> destinationObjects, Folder destinationParentObject, string objectTypeName)
        {
            List<CompareObjectsResult> items = new List<CompareObjectsResult>();
            foreach (Folder sourceObject in sourceObjects)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                Folder destinationObject = destinationObjects.Where(t => t.Title.Equals(sourceObject.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (destinationObject == null)
                {
                    items.Add(new CompareObjectsResult(string.Empty, string.Empty, objectTypeName, sourceObject.Title, "Additional", sourceObject, null, sourceParentObject, destinationParentObject)); ;
                }
                else
                {
                    if (CheckIfEquals(sourceSiteSetting, sourceObject, destinationSiteSetting, destinationObject) == false)
                    {
                        items.Add(new CompareObjectsResult(string.Empty, string.Empty, objectTypeName, sourceObject.Title, "Update", sourceObject, destinationObject, sourceParentObject, destinationParentObject)); ;
                    }
                    else
                    {
                        items.Add(new CompareObjectsResult(string.Empty, string.Empty, objectTypeName, sourceObject.Title, "Equal", sourceObject, destinationObject, sourceParentObject, destinationParentObject)); ;
                    }
                }
            }

            foreach (Folder destinationObject in destinationObjects)
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                Folder sourceFolder = sourceObjects.Where(t => t.Title.Equals(destinationObject.Title, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                if (sourceFolder == null)
                {
                    items.Add(new CompareObjectsResult(string.Empty, string.Empty, objectTypeName, destinationObject.Title, "Missing", null, destinationObject, sourceParentObject, destinationParentObject)); ;
                }
            }

            return items;
        }

        private bool CheckIfEquals(ISiteSetting sourceSiteSetting, Folder sourceObject, ISiteSetting destinationSiteSetting, Folder destinationObject)
        {
            return true;
        }
    }
}
