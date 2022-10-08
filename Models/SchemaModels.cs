using System;
using System.Text;
using Vanara.PInvoke;
using static Vanara.PInvoke.PowrProf;


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


    public class SettingModel
    {
        public static bool ShowGuids { get; set; } = false;

        public Guid Id { get; set; }
        public Guid SubgroupId { get; set; }
        public Guid SchemaId { get; set; }
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

        public List<PossibleValueModel> PossibleValues { get; set; } = new List<PossibleValueModel>();

        public override string ToString()
        {
            return !ShowGuids ? $"{Name}" : $"{Name} [{Id}]";
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


    public class SettingValueModel
    {
        public SettingValueModel(bool dcMode, SettingModel setting)
        {
            DCMode = dcMode;
            Setting = setting;
        }
        public bool DCMode { get; set; }
        public SettingModel Setting { get; set; }
    }


    public class GroupModel
    {
        public static bool ShowGuids { get => SettingModel.ShowGuids; set => SettingModel.ShowGuids = value; }
        public Guid Id { get; set; }

        public Guid SchemaId { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public POWER_ATTR PowerAttr { get; set; }

        public Dictionary<Guid, SettingModel> Settings { get; set; } = new Dictionary<Guid, SettingModel>();
        public string ToolTip => $"{Description}";

        public Icon Icon { get; set; }
        public override string ToString()
        {
            return !ShowGuids ? $"{Name}" : $"{Name} [{Id}]";
        }

    }
    public class SchemaModel
    {
        public static bool ShowGuids { get => SettingModel.ShowGuids; set => SettingModel.ShowGuids = value; }
        public Guid Id { get; set; }
        public uint Index { get; set; }
        public POWER_POLICY PowerPolicy { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public Dictionary<Guid, GroupModel> Groups { get; set; } = new Dictionary<Guid, GroupModel>();
        public string ToolTip => $"{Description}";

        public Icon Icon { get; set; }
        public override string ToString()
        {

            return !ShowGuids ? $"{Name}" : $"{Name} [{Id}]";
        }

    }


    public class SchemasModel : Dictionary<Guid, SchemaModel>
    {
        public string Q(string value)
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        public string ExcelExport(bool visibles = true)
        {
            StringBuilder sb = new StringBuilder();
            if (Values.FirstOrDefault() is SchemaModel firstSchema)
            {
                //Schema Names
                sb.Append('\t');
                sb.Append('\t');
                sb.Append('\t');
                foreach (var schema in Values)
                {
                    sb.Append(Q($"{schema.Name}")); sb.Append('\t');
                    sb.Append(Q($"{schema.Id}")); sb.Append('\t');
                }
                sb.AppendLine();
                //  Headers
                sb.Append(Q("Group")); sb.Append('\t');
                sb.Append(Q(nameof(SettingModel.Name))); sb.Append('\t');
                sb.Append(Q(nameof(SettingModel.Units))); sb.Append('\t');
                foreach (var schema in Values)
                {
                    sb.Append(Q($"{nameof(SettingModel.ACValue)}")); sb.Append('\t');
                    sb.Append(Q($"{nameof(SettingModel.DCValue)}")); sb.Append('\t');
                }
                sb.Append(Q(nameof(SettingModel.Description))); sb.Append('\t');
                sb.Append(Q(nameof(SettingModel.Id))); sb.Append('\t');
                sb.Append("Show"); sb.Append('\t');
                sb.Append("Hidden");
                sb.AppendLine();
                foreach (var group in firstSchema.Groups.Values)
                {
                    bool show = (group.Settings.Values.Any(s => (s.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC));
                    if (!visibles || show)
                    {
                        //  Group
                        sb.Append('\t');
                        sb.Append(Q(group.Name)); sb.Append('\t');
                        sb.Append('\t');
                        foreach (var schema in Values)
                        {
                            sb.Append('\t');
                            sb.Append('\t');
                        }
                        sb.Append(Q(group.Description)); sb.Append('\t');
                        sb.Append(Q($"{group.Id}")); sb.Append('\t');
                        sb.Append(Q(show ? "Show" : "")); sb.Append('\t');
                        sb.AppendLine();
                        foreach (var setting in group.Settings.Values)
                        {
                            //  Setting
                            if (!visibles || (setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)
                            {
                                sb.Append(Q(group.Name)); sb.Append('\t');
                                sb.Append(Q(setting.Name)); sb.Append('\t');
                                sb.Append(Q(setting.Units ?? "")); sb.Append('\t');
                                foreach (var schema in Values)
                                {
                                    sb.Append(schema.Groups[group.Id].Settings[setting.Id].ExcelValues);
                                }
                                sb.Append(Q(setting.Description)); sb.Append('\t');
                                sb.Append(Q($"{setting.Id}")); sb.Append('\t');
                                sb.Append(Q((setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC ? "Show" : "")); sb.Append('\t');
                                sb.Append(Q((setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE ? "Hidden" : "")); sb.Append('\t');
                                sb.AppendLine();
                            }
                        }
                        sb.AppendLine();
                    }
                }
            }
            return sb.ToString();
        }
    }
}