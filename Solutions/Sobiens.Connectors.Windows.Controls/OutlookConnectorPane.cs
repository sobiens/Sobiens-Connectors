using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sobiens.Connectors.Common;
using Sobiens.Connectors.WPF.Controls;
using Sobiens.Connectors.Entities;

namespace Sobiens.Connectors.Windows.Controls
{
    public partial class OutlookConnectorPane : UserControl
    {
        public OutlookConnectorPane()
        {
            InitializeComponent();
        }

        public void SetInspector(object inspector)
        {
            ((IConnectorMainView)elementHost1.Child).Inspector = inspector;
        }
    }
}
