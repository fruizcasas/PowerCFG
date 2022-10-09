using System;
using Vanara.PInvoke;


namespace PowerCFG.Models
{

    public class PossibleValueModel
    {
        public int Index { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public REG_VALUE_TYPE ValueType { get; set; }
        public object Value { get; set; }

        public string ToolTip => $"{Description}";
        public SettingModel Setting { get; set; }

        public override string ToString()
        {
            if (Name == "...") return Name;
            return $"{Name}: {Value} {Setting?.Units}";
        }
    }
}