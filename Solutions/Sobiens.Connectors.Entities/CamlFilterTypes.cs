using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Sobiens.Connectors.Entities
{
    public enum CamlFilterTypes
    {
        [Description("is equal to")]
        Equals = 0,
        [Description("is not equal to")]
        NotEqual = 1,
        [Description("is greater than")]
        Greater = 2,
        [Description("is less than")]
        Lesser = 3,
        [Description("is greater than or equal to")]
        EqualsGreater = 4,
        [Description("is less than or equal to")]
        EqualsLesser = 5,
        [Description("begins with")]
        BeginsWith = 6,
        [Description("contains")]
        Contains = 7
    }
}
