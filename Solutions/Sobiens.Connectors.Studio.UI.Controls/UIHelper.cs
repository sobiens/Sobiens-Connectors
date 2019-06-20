using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Sobiens.Connectors.Studio.UI.Controls
{
    public static class UIHelper
    {
        public static bool? ShowDialog(Window owner, UserControl control, string title)
        {
            return ShowDialog(owner, control, title, null, null, true, true);
        }

        public static bool? ShowDialog(Window owner, UserControl control, string title, double? height, double? width, bool showActionButtons, bool showLogo)
        {
            HostWindow hostWindow = new HostWindow();
            hostWindow.SetHostControl(control);
            if (height.HasValue == true)
                hostWindow.Height = height.Value;
            if (width.HasValue == true)
                hostWindow.Width = width.Value;
            hostWindow.Title = title;
            hostWindow.ShowActionButtons = showActionButtons;
            hostWindow.ShowLogo = showLogo;
            if (owner != null)
            {
                hostWindow.Owner = owner;
                hostWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                hostWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            return hostWindow.ShowDialog();
        }
    }
}
