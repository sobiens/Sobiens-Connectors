using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using System.Xml;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class AlertManager
    {
        public static EUAlert NodeToSobiensAlert(XmlNode node)
        {
            EUAlert alert = new EUAlert();
            alert.AlertFrequency = node.Attributes["AlertFrequency"].Value;
            alert.AlertType = node.Attributes["AlertType"].Value;
            alert.AlertTime = node.Attributes["AlertTime"].Value;
            alert.AlwaysNotify = node.Attributes["AlwaysNotify"].Value;
            alert.DynamicRecipient = node.Attributes["DynamicRecipient"].Value;
            alert.EventType = node.Attributes["EventType"].Value;
            alert.EventTypeBitmask = node.Attributes["EventTypeBitmask"].Value;
            //alert.Filter = node.Attributes["Filter"].Value;
            alert.ID = node.Attributes["ID"].Value;
            alert.ItemID = node.Attributes["ItemID"].Value;
            alert.ListID = node.Attributes["ListID"].Value;
            alert.ListName = node.Attributes["ListName"].Value;
            alert.ListUrl = node.Attributes["ListUrl"].Value;
            alert.Status = node.Attributes["Status"].Value;
            alert.Title = node.Attributes["Title"].Value;
            alert.UserId = node.Attributes["UserId"].Value;
            if (node["FilterXml"].ChildNodes.Count > 0)
            {
                XmlNode filterNode = node["FilterXml"].ChildNodes[0];
                foreach (XmlNode orNode in filterNode.ChildNodes)
                {
                    EUCamlFilters orGroup = new EUCamlFilters();
                    foreach (XmlNode andNode in orNode.ChildNodes)
                    {
                        var fieldName = andNode["FieldName"].InnerText;
                        var operationType = andNode["FilterType"].InnerText;
                        EUCamlFilterTypes filterType = (EUCamlFilterTypes)int.Parse(operationType);
                        var value = andNode["FilterValue"].InnerText;
                        EUCamlFilter filter = new EUCamlFilter(fieldName, EUFieldTypes.Text, filterType, false, value);
                        orGroup.Add(filter);
                    }
                    alert.OrGroups.Add(orGroup);
                }
            }
            return alert;
        }

        public static List<EUAlert> NodeToSobiensAlerts(XmlElement element)
        {
            List<EUAlert> alerts = new List<EUAlert>();
            foreach (XmlNode node in element.ChildNodes)
            {
                alerts.Add(AlertManager.NodeToSobiensAlert(node));
            }
            return alerts;
        }

        public static bool CheckSobiensAlertServiceEnability(EUSiteSetting siteSetting, string webUrl)
        {
            SobiensAlertsWS.AlertsWebService ws = new Sobiens.Office.SharePointOutlookConnector.SobiensAlertsWS.AlertsWebService();
            ws.Credentials = SharePointManager.GetCredential(siteSetting);
            ws.Url = webUrl + "/_layouts/AlertsWebSebService.asmx";
            bool serviceExistency = false;
            try
            {
                serviceExistency = ws.CheckServiceExistency();
            }
            catch (Exception ex)
            {
            }
            return serviceExistency;
        }

        public static List<EUAlert> GetAlerts(EUSiteSetting siteSetting, string webUrl)
        {
            SobiensAlertsWS.AlertsWebService ws = new Sobiens.Office.SharePointOutlookConnector.SobiensAlertsWS.AlertsWebService();
            ws.Credentials = SharePointManager.GetCredential(siteSetting);
            ws.Url = webUrl + "/_layouts/AlertsWebSebService.asmx";
            XmlElement element = ws.GetMyAlerts();
            return AlertManager.NodeToSobiensAlerts(element);
        }

        public static SharePointAlertsWS.DeleteFailure[] DeleteAlert(EUSiteSetting siteSetting, string webUrl, string alertID)
        {
            SharePointAlertsWS.Alerts ws = new Sobiens.Office.SharePointOutlookConnector.SharePointAlertsWS.Alerts();
            ws.Credentials = SharePointManager.GetCredential(siteSetting);
            ws.Url = webUrl + "/_vti_bin/alerts.asmx";
            SharePointAlertsWS.DeleteFailure[] failures = ws.DeleteAlerts(new string[] { alertID });
            return failures;
        }

        public static string UpdateAlert(EUSiteSetting siteSetting, string webUrl, EUAlert alert)
        {
            SobiensAlertsWS.AlertsWebService ws = new Sobiens.Office.SharePointOutlookConnector.SobiensAlertsWS.AlertsWebService();
            ws.Credentials = SharePointManager.GetCredential(siteSetting);
            ws.Url = webUrl + "/_layouts/AlertsWebSebService.asmx";
            return ws.UpdateAlert(alert.ID, alert.Title, alert.ListID, alert.AlertTime, alert.EventType, alert.AlertFrequency, alert.GetFilterXML());
        }

    }
}
