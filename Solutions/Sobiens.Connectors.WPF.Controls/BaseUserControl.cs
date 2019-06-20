using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Sobiens.WPF.Controls.Classes;
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Common;

namespace Sobiens.Connectors.WPF.Controls
{
    public class BaseUserControl : UserControl
    {
        private bool isInitialized = false;

        protected virtual void OnLoad() { }

        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (isInitialized == false)
            {
                try
                {
                    Logger.Info(string.Format(Languages.Translate("UserControl:{0} is being loaded"), this.GetType().FullName), ApplicationContext.Current.GetApplicationType().ToString());
                    OnLoad();
                    Logger.Info(string.Format(Languages.Translate("UserControl:{0} has been loaded"), this.GetType().FullName), ApplicationContext.Current.GetApplicationType().ToString());
                }
                catch (Exception ex)
                {
                    try
                    {
                        Logger.Error(ex, ApplicationContext.Current.GetApplicationType().ToString());
                    }
                    catch { }
                }
                isInitialized = true;
            }
        }


    }
}
