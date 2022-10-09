namespace PowerCFG.Models
{
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
}