using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Office.SharePointOutlookConnector.BLL.Entities
{
    public class EUField
    {
        public Guid ID;
        public string Name;
        public string DisplayName;
        public string Description;

        public string List;
        public string ShowField;
        public bool Mult;
        public bool RichText=false;
        public string RichTextMode = String.Empty;

        public int Decimals=0;
        public decimal Min=decimal.MinValue;
        public decimal Max = decimal.MaxValue;

        public int NumLines = 0;
        public int MaxLength=255;
        public bool Required = false;
        public bool ReadOnly = true;
        public bool FromBaseType = true; 
        public EUFieldTypes Type = EUFieldTypes.Text;
        public string DefaultValue = String.Empty;
        public List<ChoiceDataItem> ChoiceItems { get; set; }
        public override string ToString()
        {
            return this.DisplayName;
        }
    }

    public enum EUFieldTypes
    {
        ContentTypeId =0,
        Text=1,
        Note=2,
        File=3,
        Boolean=4,
        Counter=5,
        ContentType=6,
        DateTime=7,
        User=8,
        Lookup=9,
        Computed=10,
        Choice=11,
        Number=12,
        Unknown=13
    }
}
