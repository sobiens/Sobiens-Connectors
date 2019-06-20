using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities.Settings
{
    public interface ISettingsService
    {
        void SaveConfiguration(AppConfiguration configuration);
        AppConfiguration LoadConfiguration();
        AppConfiguration LoadAdministrativeConfiguration();
    }
}
