using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
    public class ChoiceDataItem
    {
        public ChoiceDataItem()
        {
            Parameters = new Dictionary<string, string>();
        }
        public ChoiceDataItem(string value, string displayName)
        {
            Parameters = new Dictionary<string, string>();
            Value = value;
            DisplayName = displayName;
        }
        public ChoiceDataItem(string value, string displayName, Dictionary<string, string> parameters)
        {
            Parameters = new Dictionary<string, string>();
            if (parameters != null)
                Parameters = parameters;
            Value = value;
            DisplayName = displayName;
        }
        public string Value { get; set; }
        public string DisplayName { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}
