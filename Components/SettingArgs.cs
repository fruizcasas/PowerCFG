using PowerCFG.Models;

namespace PowerCFG.Components
{

    public delegate void RestoreDefaultClickHandler(object? sender, SettingArgs e);
    public class SettingArgs: EventArgs
    {
        public bool Cancel { get; set; }
        public TreeNode Node { get; set; } = null!;
        public SettingModel Setting { get; set; } = null!;
        public bool DCMode { get; set; }
    }
}
