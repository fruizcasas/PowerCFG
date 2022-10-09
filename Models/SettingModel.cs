using System.Reflection;
using Vanara.PInvoke;
using static Vanara.PInvoke.PowrProf;


namespace PowerCFG.Models
{
    public class SettingModel
    {
        public static bool ShowGuids { get; set; } = false;
        internal static string? NameForGuid(Guid guid) => typeof(PowrProf).GetFields(BindingFlags.Public | BindingFlags.Static).FirstOrDefault(i => i.FieldType == typeof(Guid) && guid.Equals(i.GetValue(null)))?.Name;

        public Guid Id { get; set; }
        public Guid SubgroupId { get; set; }
        public Guid SchemeId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public string Q(string value)
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        public string ExcelValues
        {
            get
            {
                if (!IsRange && (ACValueIndex.HasValue && DCValueIndex.HasValue))
                {
                    if ((PossibleValues.FirstOrDefault(v => (uint)v.Index == (uint)ACValueIndex) is PossibleValueModel possibleACValue) &&
                        (PossibleValues.FirstOrDefault(v => (uint)v.Index == (uint)DCValueIndex) is PossibleValueModel possibleDCValue))
                    {
                        return $"{possibleACValue.Name}\t{possibleDCValue.Name}\t";
                    }
                }
                if (ACValue is string acValue && DCValue is string dcValue)
                {
                    return $"{Q(acValue)}\t{Q(dcValue)}\t";
                }
                return $"{ACDefaultIndex}\t{DCDefaultIndex}\t";
            }
        }


        public uint Maximum { get; set; }
        public uint Minimum { get; set; }
        public uint Increment { get; set; }

        public bool IsRange { get; set; }

        public string? Units { get; set; }

        public POWER_ATTR PowerAttr { get; set; }

        public REG_VALUE_TYPE ValueType { get; set; }


        public bool CanWriteAC { get; set; }
        public object ACValue { get; set; }
        public bool CanWriteDC { get; set; }
        public object DCValue { get; set; }

        public uint? ACValueIndex { get; set; }
        public uint? ACDefaultIndex { get; set; }
        public uint? DCValueIndex { get; set; }
        public uint? DCDefaultIndex { get; set; }

        //        public string ToolTip => $"[{ValueType}] {Minimum} .. {Increment} .. {Maximum} : {PowerAttr}{Environment.NewLine}{Description}";
        public string ToolTip => $"{Description}";

        public Icon Icon { get; set; }
        public string KeyIcon { get; set; }

        public List<PossibleValueModel> PossibleValues { get; set; } = new List<PossibleValueModel>();

        public override string ToString()
        {
            return !ShowGuids ? $"{Name}" : $"{Name} - {NameForGuid(Id)} {Id}";
        }

        public string ACDefaultIndexString()
        {
            if (!IsRange && ACDefaultIndex.HasValue)
            {
                if (PossibleValues.FirstOrDefault(v => (uint)v.Index == (uint)ACDefaultIndex) is PossibleValueModel possibleValue)
                {
                    return $"{possibleValue.Name}";
                }
            }
            return $"{ACDefaultIndex} {Units}";
        }
        public string DCDefaultIndexString()
        {
            if (!IsRange && DCDefaultIndex.HasValue)
            {
                if (PossibleValues.FirstOrDefault(v => (uint)v.Index == (uint)DCDefaultIndex) is PossibleValueModel possibleValue)
                {
                    return $"{possibleValue.Name}";
                }
            }
            return $"{DCDefaultIndex} {Units}";
        }

        public string ACString()
        {
            if (!IsRange)
            {
                if (ACValue is Guid)
                {
                    if (PossibleValues.FirstOrDefault(v => (Guid)v.Value == (Guid)ACValue) is PossibleValueModel possibleValue)
                    {
                        return Properties.Resources.On_AC_Power+$": {possibleValue.Name}";
                    }
                }
                else if (PossibleValues.FirstOrDefault(v => (uint)v.Value == (uint)ACValue) is PossibleValueModel possibleValue)
                {
                    return Properties.Resources.On_AC_Power +$": { possibleValue.Name}";
                }
            }
            return Properties.Resources.On_AC_Power + $": {ACValue} {Units}";
        }

        public string DCString()
        {
            if (!IsRange)
            {
                if (DCValue is Guid)
                {
                    if (PossibleValues.FirstOrDefault(v => (Guid)v.Value == (Guid)DCValue) is PossibleValueModel possibleValue)
                    {
                        return Properties.Resources.On_battery+ $": {possibleValue.Name}";
                    }
                }
                else if (PossibleValues.FirstOrDefault(v => (uint)v.Value == (uint)DCValue) is PossibleValueModel possibleValue)
                {
                    return Properties.Resources.On_battery + $": {possibleValue.Name}";
                }
            }
            return Properties.Resources.On_battery + $": {DCValue} {Units}";
        }
    }
}