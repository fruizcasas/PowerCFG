using System.Text;
using static Vanara.PInvoke.PowrProf;


namespace PowerCFG.Models
{
    public class SchemesModel : Dictionary<Guid, SchemeModel>
    {
        public string Q(string value)
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }

        public bool CanCreate { get; set; }

        public bool CanSetActive { get; set; }

        public string ExcelExport(bool visibles = true)
        {
            StringBuilder sb = new StringBuilder();
            if (Values.FirstOrDefault() is SchemeModel firstScheme)
            {
                //Scheme Names
                sb.Append('\t');
                sb.Append('\t');
                sb.Append('\t');
                foreach (var scheme in Values)
                {
                    sb.Append(Q($"{SchemeModel.NameForGuid(scheme.Id)}")); sb.Append('\t');
                    sb.Append(Q($"{scheme.Id}")); sb.Append('\t');
                }
                sb.AppendLine();
                sb.Append('\t');
                sb.Append('\t');
                sb.Append('\t');
                foreach (var scheme in Values)
                {
                    sb.Append(Q($"{scheme.Name}")); sb.Append('\t');
                    sb.Append('\t');
                }
                sb.AppendLine();
                //  Headers
                sb.Append(Q("Group")); sb.Append('\t');
                sb.Append(Q(nameof(SettingModel.Name))); sb.Append('\t');
                sb.Append(Q(nameof(SettingModel.Units))); sb.Append('\t');
                foreach (var scheme in Values)
                {
                    sb.Append(Q($"{nameof(SettingModel.ACValue)}")); sb.Append('\t');
                    sb.Append(Q($"{nameof(SettingModel.DCValue)}")); sb.Append('\t');
                }
                sb.Append(Q(nameof(SettingModel.Description))); sb.Append('\t');
                sb.Append(Q(nameof(SettingModel.Id))); sb.Append('\t');
                sb.Append(Q("Alias")); sb.Append('\t');
                sb.Append(Q("Show")); sb.Append('\t');
                sb.Append(Q("Hidden"));
                sb.AppendLine();
                foreach (var group in firstScheme.Groups.Values)
                {
                    bool show = (group.Settings.Values.Any(s => (s.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC));
                    if (!visibles || show)
                    {
                        //  Group
                        sb.Append('\t');
                        sb.Append(Q(group.Name)); sb.Append('\t');
                        sb.Append('\t');
                        foreach (var scheme in Values)
                        {
                            sb.Append('\t');
                            sb.Append('\t');
                        }
                        sb.Append(Q(group.Description)); sb.Append('\t');
                        sb.Append(Q($"{group.Id}")); sb.Append('\t');
                        sb.Append(Q($"{GroupModel.NameForGuid(group.Id)}")); sb.Append('\t');
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
                                foreach (var scheme in Values)
                                {
                                    sb.Append(scheme.Groups[group.Id].Settings[setting.Id].ExcelValues);
                                }
                                sb.Append(Q(setting.Description)); sb.Append('\t');
                                sb.Append(Q($"{setting.Id}")); sb.Append('\t');
                                 sb.Append(Q($"{SettingModel.NameForGuid(setting.Id)}")); sb.Append('\t');
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