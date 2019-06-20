using System;

namespace Sobiens.WPF.Controls.Interface
{
    interface IExtendedColumn
    {
        Boolean AllowAutoFilter { get; set; }
        bool HasAutoFilter { get; }
    }
}
