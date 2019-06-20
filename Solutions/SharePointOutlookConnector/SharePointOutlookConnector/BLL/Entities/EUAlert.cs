using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUAlert
    {
        public string AlertFrequency = "";
        public string AlertType = "";
        public string AlertTime = "";
        public string AlwaysNotify = "";
        public string DynamicRecipient = "";
        public string EventType = "";
        public string EventTypeBitmask = "";
        public string Filter = "";
        public string ID = "";
        public string ItemID = "";
        public string ListID = "";
        public string ListName = "";
        public string ListUrl = "";
        public string Status = "";
        public string Title = "";
        public string UserId = "";
        public List<EUCamlFilters> OrGroups = new List<EUCamlFilters>();
        public string GetFilterXML() {
            string xml = "<Filter>";
            for (var i = 0; i < this.OrGroups.Count; i++) {
                if (this.OrGroups[i].Count == 0)
                    continue;
                xml += "<Or>";
                for (var x = 0; x < this.OrGroups[i].Count; x++) {
                    xml += "<And>";
                    xml += "<FieldName>" + this.OrGroups[i][x].FieldName + "</FieldName>";
                    xml += "<FilterType>" + (int)this.OrGroups[i][x].FilterType + "</FilterType>";
                    xml += "<FilterValue>" + this.OrGroups[i][x].FilterValue + "</FilterValue>";
                    xml += "</And>";
                }
                xml += "</Or>";
            }
            xml += "</Filter>";
            return xml;
        }
        public DateTime? GetAlertTimeDate() {
            if (this.AlertTime == "")
                return null;
            //dd-MM-yyyy hh:mm
            string[] dateArray = this.AlertTime.Split(new char[]{'-'});
            int day = int.Parse(dateArray[0]);
            int month = int.Parse(dateArray[1]);
            int year = int.Parse(dateArray[2].Split(new char[]{' '})[0].ToString());
            var timeArray = dateArray[2].Split(new char[]{' '})[1].Split(new char[]{':'});
            int hours = int.Parse(timeArray[0]);
            int minutes = int.Parse(timeArray[1]);
            return new DateTime(year, month, day, hours, minutes, 0, 0);
        }
        public int GetWeekDay() {
            DateTime? date = this.GetAlertTimeDate();
            return (date.HasValue==true?(int)date.Value.DayOfWeek:0);
        }
        public int GetTime() {
            DateTime? date = this.GetAlertTimeDate();
            return (date.HasValue == true ? date.Value.Hour : 0);
        }
    }
}
