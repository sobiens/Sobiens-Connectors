using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sobiens.Connectors.Entities
{
    public class CompareObjectsResult
    {
        public CompareObjectsResult() { }
        public CompareObjectsResult(string parentObjectType, string parentObjectName, string objectType, string name, string differenceType, object sourceObject, object objectToCompareWith, Folder sourceParentObject, Folder objectToCompareWithParentObject) {
            this.ParentObjectType = parentObjectType;
            this.ParentObjectName = parentObjectName;
            this.ObjectType = objectType;
            this.Name = name;
            this.DifferenceType = differenceType;
            this.SourceObject = sourceObject;
            this.ObjectToCompareWith = objectToCompareWith;
            this.SourceParentObject = sourceParentObject;
            this.ObjectToCompareWithParentObject = objectToCompareWithParentObject;
        }
        public Folder SourceParentObject { get; set; }
        public Folder ObjectToCompareWithParentObject { get; set; }
        public string ParentObjectType { get; set; }
        public string ParentObjectName { get; set; }
        public string ObjectType { get; set; }
        public string Name { get; set; }
        public string DifferenceType { get; set; }
        public object SourceObject { get; set; }
        public object ObjectToCompareWith { get; set; }
        public Visibility ApplyButtonVisibility
        {
            get
            {
                if (DifferenceType == "Missing")
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }

        public Visibility ShowDifferencesButtonVisibility
        {
            get
            {
                if (DifferenceType == "Update")
                    return Visibility.Visible;
                else
                    return Visibility.Hidden;
            }
        }
    }
}
