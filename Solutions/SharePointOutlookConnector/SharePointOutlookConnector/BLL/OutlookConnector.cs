using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Office.SharePointOutlookConnector.BLL.Entities;
using Sobiens.Office.SharePointOutlookConnector.BLL.Interfaces;
using Sobiens.Office.SharePointOutlookConnector.BLL.FileSystem;

namespace Sobiens.Office.SharePointOutlookConnector.BLL
{
    public class OutlookConnector
    {
        public static IOutlookConnector GetConnector(EUSiteSetting siteSetting)
        {
            if (siteSetting.SiteSettingType == EUSiteSettingTypes.SharePoint)
                return new Sobiens.Office.SharePointOutlookConnector.BLL.SharePoint.SharePointOutlookConnector();
            else if (siteSetting.SiteSettingType == EUSiteSettingTypes.GMail)
                return new Sobiens.Office.SharePointOutlookConnector.BLL.SharePoint.GmailOutlookConnector();
            else
                return new FileSystemOutlookConnector();
        }
    }
}
