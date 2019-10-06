using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using Sobiens.Connectors.Entities.Settings;
using System.Net;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.WPF.Controls
{
    /// <summary>
    /// Interaction logic for BrowserExplorer.xaml
    /// </summary>
    public partial class BrowserExplorer : HostControl
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        private ISiteSetting SiteSetting { get; set; }
        private string Url { get; set; }
        public BrowserExplorer()
        {
            InitializeComponent();
        }


        public void Initialize(ISiteSetting siteSetting, string url)
        {
            this.SiteSetting = siteSetting;
            this.Url = url;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Uri uri = new Uri(this.Url);

            this.myBrowser.Navigating += new NavigatingCancelEventHandler(myBrowser_Navigating);
            this.myBrowser.Navigated += new NavigatedEventHandler(myBrowser_Navigated);
            this.myBrowser.LoadCompleted += new LoadCompletedEventHandler(myBrowser_LoadCompleted);

            if (this.SiteSetting != null && this.SiteSetting.UseClaimAuthentication == true)
            {
                CookieContainer cookieContainer = ServiceManagerFactory.GetServiceManager(this.SiteSetting.SiteSettingType).GetCookieContainer(this.SiteSetting.Url, this.SiteSetting.Username, this.SiteSetting.Password);
                foreach (Cookie cookie in cookieContainer.GetCookies(new Uri(this.SiteSetting.Url)))
                {
                    InternetSetCookie(this.Url, null, cookie.ToString() + "; expires = Sun, 01-May-2013 00:00:00 GMT");
                }
            }
            this.myBrowser.Navigate(uri);
        }

        void myBrowser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            StatusLabel.Content =Languages.Translate("Loading...");
        }

        void myBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            HideScriptErrors(this.myBrowser, true);
        }

        void myBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            /*
            mshtml.HTMLDocumentClass document = (mshtml.HTMLDocumentClass)((WebBrowser)sender).Document;
            if (document.url.IndexOf("&SBSBrowserAction=Close") > 0)
            {
                this.Close(true);
            }

            string html = document.documentElement.outerHTML;
            if (
                html.IndexOf("<script type='text/javascript'>window.frameElement.commitPopup();</script>", StringComparison.InvariantCultureIgnoreCase) > 0
                ||
                html.IndexOf("<SCRIPT type=text/javascript>window.frameElement.commitPopup();</SCRIPT>", StringComparison.InvariantCultureIgnoreCase) > 0
                )
            {
                this.Close(true);
            }
            StatusLabel.Content = Languages.Translate("Completed.");
             */
            StatusLabel.Content = Languages.Translate("Check uncommented code.");
        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }




    }
}
