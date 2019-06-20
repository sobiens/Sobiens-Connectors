#if General
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Threading;
using System.Diagnostics;
using Sobiens.Connectors.Entities.Settings;
using Sobiens.Connectors.Entities;
using System.Windows.Controls;
using Sobiens.Connectors.Entities.Documents;
using Sobiens.Connectors.Common.Interfaces;
using System.Reflection;
using System.Xml.Linq;
using Sobiens.Connectors.Common.Threading;
using Sobiens.Connectors.Entities.State;
using Sobiens.Connectors.Common.Extensions;
using Microsoft.Office.Core;
namespace Sobiens.Connectors.Common
{
    public class StateManager
    {

        private static StateManager _Instance = null;

        public static StateManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new StateManager();
            }
            return _Instance;
        }

        private StateManager()
        {
        }

        private ConnectorState _ConnectorState = null;
        public ConnectorState ConnectorState
        {
            get
            {
                if (_ConnectorState == null)
                {
                    _ConnectorState = this.LoadState();
                }

                return _ConnectorState;
            }
            private set
            {
                _ConnectorState = value;
            }
        }

        public ConnectorPaneDockPosition GetConnectorPaneDockPosition(MsoCTPDockPosition dockPosition)
        {
            switch(dockPosition)
            {
                case MsoCTPDockPosition.msoCTPDockPositionBottom:
                    return ConnectorPaneDockPosition.DockPositionBottom;
                case MsoCTPDockPosition.msoCTPDockPositionFloating:
                    return ConnectorPaneDockPosition.DockPositionFloating;
                case MsoCTPDockPosition.msoCTPDockPositionLeft:
                    return ConnectorPaneDockPosition.DockPositionLeft;
                case MsoCTPDockPosition.msoCTPDockPositionRight:
                    return ConnectorPaneDockPosition.DockPositionRight;
                case MsoCTPDockPosition.msoCTPDockPositionTop:
                    return ConnectorPaneDockPosition.DockPositionTop;
            }

            return ConnectorPaneDockPosition.DockPositionRight;
        }

        public MsoCTPDockPosition GetMsoPaneDockPosition(ConnectorPaneDockPosition dockPosition)
        {
            switch (dockPosition)
            {
                case ConnectorPaneDockPosition.DockPositionBottom:
                    return MsoCTPDockPosition.msoCTPDockPositionBottom;
                case ConnectorPaneDockPosition.DockPositionFloating:
                    return MsoCTPDockPosition.msoCTPDockPositionFloating;
                case ConnectorPaneDockPosition.DockPositionLeft:
                    return MsoCTPDockPosition.msoCTPDockPositionLeft;
                case ConnectorPaneDockPosition.DockPositionRight:
                    return MsoCTPDockPosition.msoCTPDockPositionRight;
                case ConnectorPaneDockPosition.DockPositionTop:
                    return MsoCTPDockPosition.msoCTPDockPositionTop;
            }

            return MsoCTPDockPosition.msoCTPDockPositionRight;
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void SaveState()
        {
            string configurationFilePath = ConfigurationManager.GetInstance().GetStateFilePath();
            SerializationManager.SaveConfiguration<ConnectorState>(this.ConnectorState, configurationFilePath);
        }



        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <returns></returns>
        private ConnectorState LoadState()
        {
            string settingFilePath = ConfigurationManager.GetInstance().GetStateFilePath();
            if (File.Exists(settingFilePath) == false)
                return new ConnectorState();

            ConnectorState configuration = SerializationManager.ReadSettings<ConnectorState>(settingFilePath);
            return configuration;
        }

    }
}
#endif
