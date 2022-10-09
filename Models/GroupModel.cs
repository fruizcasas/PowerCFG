using static Vanara.PInvoke.PowrProf;


namespace PowerCFG.Models
{
    public class GroupModel
    {
        public static bool ShowGuids { get => SettingModel.ShowGuids; set => SettingModel.ShowGuids = value; }

        internal static string? NameForGuid(Guid guid) => SettingModel.NameForGuid(guid);
        public Guid Id { get; set; }

        public Guid SchemeId { get; set; }

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public POWER_ATTR PowerAttr { get; set; }

        public Dictionary<Guid, SettingModel> Settings { get; set; } = new Dictionary<Guid, SettingModel>();
        public string ToolTip => $"{Description}";

        public Icon Icon { get; set; }
        public string KeyIcon { get; set; }
        public override string ToString()
        {
            return !ShowGuids ? $"{Name}" : $"{Name} - {NameForGuid(Id)} {Id}";
        }

    }
}