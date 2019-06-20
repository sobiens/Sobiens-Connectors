#if General
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sobiens.Connectors.Common.Threading
{
    public class WorkItem: ISerializable
    {
        public enum WorkItemTypeEnum
        {
            NonCriticalWorkItem = 0,
            CriticalWorkItem = 1
        }

        public WorkItem()
        { 
        }

        public WorkItem(string title)
        {
            this.Title = title;
        }

        public WorkItem(SerializationInfo info, StreamingContext ctxt)
        {
            this.WorkItemType = (WorkItemTypeEnum)info.GetValue("WorkItemType", typeof(string));
            this.WorkItemData = info.GetValue("WorkItemData", typeof(object));
            this.CallbackFunction = (WorkRequestDelegate)info.GetValue("CallbackFunction", typeof(WorkRequestDelegate));
            this.CallbackData = info.GetValue("CallbackData", typeof(object));
        }

        public string Title { get; set; }
        public WorkItemTypeEnum WorkItemType { get; set; }
        public object WorkItemData { get; set; }
        public WorkRequestDelegate CallbackFunction { get; set; }
        public object CallbackData { get; set; }
        public string LogCategory { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("WorkItemType", this.WorkItemType);
            info.AddValue("WorkItemData", this.WorkItemData);
            info.AddValue("CallbackFunction", this.CallbackFunction);
            info.AddValue("CallbackData", this.CallbackData);
        }
    }
}
#endif
