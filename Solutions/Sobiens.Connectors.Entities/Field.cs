﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sobiens.Connectors.Entities
{
#if General
    [Serializable]
#endif
    public class Field:Folder
    {
        public Guid ID;
        public string attachedField;//JD
        public string Name;
        public string DisplayName;
        public string Description;

        public string List;
        public string ShowField;
        public bool Mult;
        public bool RichText=false;
        public string RichTextMode = String.Empty;
        public string SchemaXml = string.Empty;

        public int Decimals=0;
        public decimal Min=decimal.MinValue;
        public decimal Max = decimal.MaxValue;

        public int NumLines = 0;
        public int MaxLength=255;
        public bool AppendOnly = false;
        public bool IsPrimary = false;
        public bool IsRetrievable = true;
        public bool Required = false;
        public bool ReadOnly = true;
        public bool FromBaseType = true; 
        public FieldTypes Type = FieldTypes.Text;
        public string DefaultValue = String.Empty;
        public string Formula = String.Empty;
        public List<ChoiceDataItem> ChoiceItems { get; set; }
        public override string GetUrl() { throw new NotImplementedException(); }
        public override string GetPath() { throw new NotImplementedException(); }
        public override string GetListName() { throw new NotImplementedException(); }
        public override string GetRoot() { throw new NotImplementedException(); }
        public override string GetWebUrl() { throw new NotImplementedException(); }
        public override string ToString()
        {
            return this.DisplayName;
        }
    }

    public enum FieldTypes
    {
        ContentTypeId = 0,
        Text = 1,
        Note = 2,
        File = 3,
        Boolean = 4,
        Counter = 5,
        ContentType = 6,
        DateTime = 7,
        User = 8,
        Lookup = 9,
        Computed = 10,
        Choice = 11,
        Number = 12,
        Unknown = 13,
        TaxonomyFieldType = 14,
        Virtual = 15,
        URL = 16,
        Currency = 17,
        Calculated = 18,
        OutcomeChoice = 19,
        ByteImage = 20,
        ByteArray = 21
    }
}
