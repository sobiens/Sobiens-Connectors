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
using Sobiens.Connectors.Entities;
using Sobiens.Connectors.Entities.Interfaces;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Common;

namespace Sobiens.Connectors.WPF.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SiteSettingForm.xaml
    /// </summary>
    public partial class EmptyConfigurationControl : BaseUserControl, IEmptyConfigurationControl
    {
        public EmptyConfigurationControl()
        {
            InitializeComponent();

        }

        protected void ConfigureButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                SettingsForm sf = new SettingsForm();
                AppConfiguration configuration = ConfigurationManager.GetInstance().Configuration;
                sf.BindControls(configuration);
                if (sf.ShowDialog(null, Languages.Translate("Configuration")) == true)
                {
                    ConfigurationManager.GetInstance().SaveAppConfiguration();
                    ApplicationContext.Current.RefreshControlsFromConfiguration();
                }
                else
                {
                    ConfigurationManager.GetInstance().LoadConfiguration();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            this.InitializeForm();

        }

        private void InitializeForm()
        {
        }

        public void RefreshControls()
        {

        }

        private void BaseUserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
