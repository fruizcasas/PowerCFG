using static Vanara.PInvoke.PowrProf;


namespace PowerCFG.Models
{
    public class SchemeModel
    {
        public static bool ShowGuids { get => SettingModel.ShowGuids; set => SettingModel.ShowGuids = value; }
        internal static string? NameForGuid(Guid guid) => SettingModel.NameForGuid(guid);
        public Guid Id { get; set; }
        public uint Index { get; set; }
        public POWER_POLICY PowerPolicy { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public Dictionary<Guid, GroupModel> Groups { get; set; } = new Dictionary<Guid, GroupModel>();
        public string ToolTip => $"{Description}";

        public bool CanWrite { get; set; }

        public Icon Icon { get; set; }
        public string  KeyIcon { get; set; }
        public override string ToString()
        {
            return !ShowGuids ? $"{Name}" : $"{Name} - {NameForGuid(Id)} {Id}";
        }

    }
}