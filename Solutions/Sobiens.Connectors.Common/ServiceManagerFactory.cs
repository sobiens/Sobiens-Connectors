using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sobiens.Connectors.Common.SharePoint;
using Sobiens.Connectors.Common.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common.FileSystem;
using Sobiens.Connectors.Common.SQLServer;
using Sobiens.Connectors.Common.CRM;

namespace Sobiens.Connectors.Common
{
    public static class ServiceManagerFactory
    {
        public static IServiceManager GetServiceManager(SiteSettingTypes siteSettingType)
        {
            switch (siteSettingType)
            {
                case SiteSettingTypes.SharePoint:
                    return SharePointServiceManager.GetInstance();
                case SiteSettingTypes.GMail:
                    return GMailServiceManager.GetInstance();
                case SiteSettingTypes.FileSystem:
                    return FileSystemServiceManager.GetInstance();
                case SiteSettingTypes.SQLServer:
                    return SQLServerServiceManager.GetInstance();
                case SiteSettingTypes.CRM:
                    return CRMServiceManager.GetInstance();
                default:
                    return null;
            }
        }
    }
}
