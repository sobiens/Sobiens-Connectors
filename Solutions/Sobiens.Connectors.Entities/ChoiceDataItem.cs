using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ChoiceDataItem
    {
        public ChoiceDataItem(string value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }
        public string Value { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
