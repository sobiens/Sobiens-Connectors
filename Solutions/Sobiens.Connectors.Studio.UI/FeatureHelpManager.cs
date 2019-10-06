using Sobiens.Connectors.Common;
using Sobiens.Connectors.Studio.UI.Controls;
using Sobiens.Connectors.WPF.Controls;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sobiens.Connectors.Studio.UI
{
    public class FeatureHelpManager
    {
        private static Dictionary<string, string[]> FeatureHelps
        {
            get
            {
                Dictionary<string, string[]> featureHelps = new Dictionary<string, string[]>();
                featureHelps.Add("1.6.1.1", new string[] { "General", "http://tutorials.sobiens.com/solutions/sobiens-studio/" });
                featureHelps.Add("1.6.2.1", new string[] { "Comparing MS SharePoint, MS CRM and SQL Server Schemas", "http://tutorials.sobiens.com/solutions/sobiens-studio/comparing-ms-sharepoint-ms-crm-and-sql-server-schemas/" });
                featureHelps.Add("1.6.3.1", new string[] { "Using query editor on Sobiens Studio", "http://tutorials.sobiens.com/solutions/sobiens-studio/using-query-editor-on-sobiens-studio/" });
                featureHelps.Add("1.6.5.1", new string[] { "Synchronizing SharePoint lists on Sobiens Studio", "http://tutorials.sobiens.com/solutions/sobiens-studio/synchronizing-sharepoint-lists-on-sobiens-studio/" });
                return featureHelps;
            }
        }


        public static void ShowNotSeenFeatureHelpDialog()
        {
            BrowserTabsPanel browserTabsPanel = new BrowserTabsPanel();
            int versionValue = GetVersionValue(ApplicationContext.Current.Configuration.LatestFeatureHelpVersionShown);
            foreach(string versionString in FeatureHelps.Keys)
            {
                int _versionValue = GetVersionValue(versionString);
                if (_versionValue > versionValue)
                {
                    browserTabsPanel.AddNewTab(FeatureHelps[versionString][0], FeatureHelps[versionString][1]);
                }
            }

            if (browserTabsPanel.Tabs.Count == 0)
                return;
            browserTabsPanel.ShowDialog(null, "New Sobiens Studio Features", 0, 0, false, false);

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string fileVersion = fvi.FileVersion;

            ConfigurationManager.GetInstance().Configuration.LatestFeatureHelpVersionShown = fileVersion;
            ConfigurationManager.GetInstance().SaveAppConfiguration();

        }

        private static int GetVersionValue(string versionString)
        {
            if (string.IsNullOrEmpty(versionString) == true)
                return 0;

            string[] versionStringValues = versionString.Split(new char[] { '.' });
            if(versionStringValues.Length == 4)
            {
                int value1 = 0;
                int value2 = 0;
                int value3 = 0;
                int value4 = 0;
                if (int.TryParse(versionStringValues[0], out value1) == false)
                    return 0;
                if (int.TryParse(versionStringValues[1], out value2) == false)
                    return 0;
                if (int.TryParse(versionStringValues[2], out value3) == false)
                    return 0;
                if (int.TryParse(versionStringValues[3], out value4) == false)
                    return 0;

                return (value1 * 10 ^ 9) + (value2 * 10 ^ 6) + (value3 * 10 ^ 3) + value4;
            }

            return 0;
        }

    }
}
