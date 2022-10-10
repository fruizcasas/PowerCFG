using System.Collections;
using System.Text;
using Vanara.InteropServices;
using Vanara.PInvoke;
using Vanara.Extensions;
using static Vanara.PInvoke.PowrProf;
using static Vanara.PInvoke.Shell32;
using System.Runtime.InteropServices;
using PowerCFG.Models;
using PowerCFG.Components;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Data;
using System.Xml.Linq;
using System.Reflection;



namespace PowerCFG
{
    public partial class FrmPowerCFG : Form
    {

        public static readonly Guid SCHEME_MAX = new Guid("a1841308-3541-4fab-bc81-f71556f20b4a");
        public static readonly Guid SCHEME_MIN = new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
        public static readonly Guid SCHEME_BALANCED = new Guid("381b4222-f694-41f0-9685-ff5bb260df2e");

        public static readonly Guid[] SCHEMES_BY_DEFAULT = new Guid[] { SCHEME_MAX, SCHEME_MIN, SCHEME_BALANCED };

        public FrmPowerCFG()
        {
            InitializeComponent();
        }

        // ====================================
        #region FIELDS
        // ====================================
        static IDictionary EnvironmentVariables = Environment.GetEnvironmentVariables();

        Guid ActiveSchemeGuid;

        SchemeModel? SelectedScheme = null;

        public SchemesModel Schemes = new SchemesModel();

        Dictionary<string, IconExtractor> Icons = new Dictionary<string, IconExtractor>();

        string PowerProfName = ExpandVars("@%SystemRoot%\\system32\\powrprof.dll");
        // ====================================
        #endregion FIELDS
        // ====================================


        // ====================================
        #region UTILITIES
        // ====================================

        private static string ExpandVars(string? value)
        {
            value ??= String.Empty;
            if (value.StartsWith('@'))
            {
                value = value.Trim().Replace("@", String.Empty);
                while (value.Contains('%'))
                {
                    int start = value.IndexOf('%');
                    int next = value.IndexOf('%', start + 1);
                    if (start >= next) break;
                    var key = value.Substring(start + 1, next - start - 1);
                    if (EnvironmentVariables.Contains(key))
                    {
                        value = value.Replace($"%{key}%", $"{EnvironmentVariables[key]}");
                    }
                }
            }
            return value;
        }

        private static unsafe IntPtr GuidToInPtr(Guid g1)
        {
            return (IntPtr)(void*)&g1;
        }

        private void ExpandAllNode(TreeNode? node)
        {
            if (node != null)
            {
                node.Expand();
                foreach (TreeNode child in node.Nodes)
                {
                    ExpandAllNode(child);
                }
            }
        }
        private void CollapseNode(TreeNode? node)
        {
            if (node != null)
            {
                node.Collapse();
                foreach (TreeNode child in node.Nodes)
                {
                    CollapseNode(child);
                }
            }
        }

        // ====================================
        #endregion UTILITIES
        // ====================================

        // ====================================
        #region EVENTS
        // ====================================

        private void FrmPowerCFG_Load(object sender, EventArgs e)
        {
            LoadSchemes();
            LoadNodes(ActiveSchemeGuid);
            HiddenCheckBox.Visible = IsUserAnAdmin();
            ShowGuidsCheckBox.Visible = IsUserAnAdmin();
        }

        private void FrmPowerCFG_Shown(object sender, EventArgs e)
        {
            if (IsUserAnAdmin()) Text += " (Admin)";
        }

        private void HiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadSchemes();
            LoadNodes(ActiveSchemeGuid);
        }

        private void ShowGuidsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingModel.ShowGuids = !SettingModel.ShowGuids;
            LoadNodes(ActiveSchemeGuid);
        }

        private void ExcelExportButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Schemes.ExcelExport(!HiddenCheckBox.Checked));
            MessageBox.Show(this, Properties.Resources.Paste_into_Excel);
        }

        // ====================================
        #endregion EVENTS
        // ====================================

        // ====================================
        #region ToolStripMenuItems
        // ====================================
        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SelectedScheme = null;

            if (tv.SelectedNode is TreeNode node)
            {
                CollapseToolStripMenuItem.Visible = true;
                ExpandToolStripMenuItem.Visible = true;

                if (node.Tag is SchemeModel scheme)
                {
                    SelectedScheme = scheme;
                    HiddenToolStripMenuItem.Visible = false;
                    ShowToolStripMenuItem.Visible = false;
                    ActivateSchemeToolStripMenuItem.Visible = (scheme.Id != ActiveSchemeGuid);
                    CopyGUIDToolStripMenuItem.Visible = SchemeModel.ShowGuids;

                    SaveSchemeToolStripMenuItem.Visible = true;
                    ImportSchemeToolStripMenuItem.Visible = false;
                    DuplicateSchemeToolStripMenuItem.Visible = true;
                    DeleteSchemeToolStripMenuItem.Visible = scheme.Id != ActiveSchemeGuid && !SCHEMES_BY_DEFAULT.Contains(scheme.Id);

                    DuplicateSchemeToolStripMenuItem.Enabled = Schemes.CanCreate;
                    ActivateSchemeToolStripMenuItem.Enabled = Schemes.CanSetActive;
                    DeleteSchemeToolStripMenuItem.Enabled = scheme.CanWrite;

                }
                else if (node.Tag is GroupModel group)
                {
                    HiddenToolStripMenuItem.Visible = true;
                    ShowToolStripMenuItem.Visible = true;
                    ActivateSchemeToolStripMenuItem.Visible = false;
                    HiddenToolStripMenuItem.Checked = (group.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE;
                    ShowToolStripMenuItem.Checked = (group.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC;
                    CopyGUIDToolStripMenuItem.Visible = GroupModel.ShowGuids;

                    SaveSchemeToolStripMenuItem.Visible = false;
                    ImportSchemeToolStripMenuItem.Visible = false;
                    DuplicateSchemeToolStripMenuItem.Visible = false;
                    DeleteSchemeToolStripMenuItem.Visible = false;
                }
                else if (node.Tag is SettingModel setting)
                {
                    HiddenToolStripMenuItem.Visible = true;
                    ShowToolStripMenuItem.Visible = true;
                    ActivateSchemeToolStripMenuItem.Visible = false;
                    HiddenToolStripMenuItem.Checked = (setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE;
                    ShowToolStripMenuItem.Checked = (setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC;
                    CopyGUIDToolStripMenuItem.Visible = SettingModel.ShowGuids;

                    SaveSchemeToolStripMenuItem.Visible = false;
                    ImportSchemeToolStripMenuItem.Visible = false;
                    DuplicateSchemeToolStripMenuItem.Visible = false;
                    DeleteSchemeToolStripMenuItem.Visible = false;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                CollapseToolStripMenuItem.Visible = false;
                ExpandToolStripMenuItem.Visible = false;
                HiddenToolStripMenuItem.Visible = false;
                ShowToolStripMenuItem.Visible = false;
                ActivateSchemeToolStripMenuItem.Visible = false;
                CopyGUIDToolStripMenuItem.Visible = false;

                SaveSchemeToolStripMenuItem.Visible = false;
                ImportSchemeToolStripMenuItem.Visible = Schemes.CanCreate;
                DuplicateSchemeToolStripMenuItem.Visible = false;
                DeleteSchemeToolStripMenuItem.Visible = false;
            }



        }
        private void CopyGUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode?.Tag is SchemeModel scheme)
            {
                Clipboard.SetText($"{SchemeModel.NameForGuid(scheme.Id)} {scheme.Id}");
            }
            else if (tv.SelectedNode?.Tag is GroupModel group)
            {
                Clipboard.SetText($"{GroupModel.NameForGuid(group.Id)} {group.Id}");
            }
            else if (tv.SelectedNode?.Tag is SettingModel setting)
            {
                Clipboard.SetText($"{SettingModel.NameForGuid(setting.Id)} {setting.Id}");
            }
        }
        private void ActivateSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedScheme != null)
            {
                Win32Error err = PowerSetActiveScheme(default, SelectedScheme.Id);
                if (err.Failed)
                {
                    System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerSetActiveScheme({SelectedScheme.Name})");
                }
                else
                {
                    ActiveSchemeGuid = SelectedScheme.Id;
                    Font bold = new Font(tv.Font, FontStyle.Bold);
                    foreach (TreeNode node in tv.Nodes)
                    {
                        if (node?.Tag is SchemeModel scheme)
                        {
                            if (scheme.Id == ActiveSchemeGuid)
                            {
                                node.NodeFont = bold;
                            }
                            else
                            {
                                node.NodeFont = tv.Font;
                            }
                        }
                    }
                }
            }

        }
        private void HiddenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode?.Tag is GroupModel group)
            {
                group.PowerAttr ^= POWER_ATTR.POWER_ATTRIBUTE_HIDE;
                Win32Error err = PowerWriteSettingAttributes(group.Id, Guid.Empty, group.PowerAttr);
                if (err.Failed)
                {
                    System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteSettingAttributes({group.Name},{group.PowerAttr})");
                }
            }
            else if (tv.SelectedNode?.Tag is SettingModel setting)
            {
                setting.PowerAttr ^= POWER_ATTR.POWER_ATTRIBUTE_HIDE;
                Win32Error err = PowerWriteSettingAttributes(setting.SubgroupId, setting.Id, setting.PowerAttr);
                if (err.Failed)
                {
                    System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteSettingAttributes({setting.Name},{setting.PowerAttr})");
                }

                Font bold = new Font(tv.Font, FontStyle.Bold);
                Font italic = new Font(tv.Font, FontStyle.Italic);
                if ((setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE)
                {
                    tv.SelectedNode.NodeFont = italic;
                    tv.SelectedNode.ForeColor = SystemColors.GrayText;
                }
                else if ((setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)
                {
                    tv.SelectedNode.NodeFont = bold;
                    tv.SelectedNode.ForeColor = SystemColors.ControlText;
                    tv.SelectedNode.Parent.NodeFont = bold;
                    tv.SelectedNode.Parent.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    tv.SelectedNode.NodeFont = tv.Font;
                    tv.SelectedNode.ForeColor = SystemColors.ControlText;
                }
            }
        }
        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode?.Tag is GroupModel group)
            {
                group.PowerAttr ^= POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC;
                Win32Error err = PowerWriteSettingAttributes(group.Id, Guid.Empty, group.PowerAttr);
                if (err.Failed)
                {
                    System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteSettingAttributes({group.Name},{group.PowerAttr})");
                }
            }
            else if (tv.SelectedNode?.Tag is SettingModel setting)
            {
                setting.PowerAttr ^= POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC;
                Win32Error err = PowerWriteSettingAttributes(setting.SubgroupId, setting.Id, setting.PowerAttr);
                if (err.Failed)
                {
                    System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteSettingAttributes({setting.Name},{setting.PowerAttr})");
                }

                Font bold = new Font(tv.Font, FontStyle.Bold);
                Font italic = new Font(tv.Font, FontStyle.Italic);
                if ((setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE)
                {
                    tv.SelectedNode.NodeFont = italic;
                    tv.SelectedNode.ForeColor = SystemColors.GrayText;
                }
                else if ((setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)
                {
                    tv.SelectedNode.NodeFont = bold;
                    tv.SelectedNode.ForeColor = SystemColors.ControlText;
                    tv.SelectedNode.Parent.NodeFont = bold;
                    tv.SelectedNode.Parent.ForeColor = SystemColors.ControlText;
                }
                else
                {
                    tv.SelectedNode.NodeFont = tv.Font;
                    tv.SelectedNode.ForeColor = SystemColors.ControlText;
                }
            }
        }
        private void ExpandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node)
            {
                node.ExpandAll();
                tv.SelectedNode = node;
                node.EnsureVisible();
            }
        }
        private void CollapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node)
            {
                CollapseNode(node);
            }
        }
        private void SaveSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node && node.Tag is SchemeModel scheme)
            {
                if (!IsUserAnAdmin())
                {
                    MessageBox.Show(this, Properties.Resources.Admin_Reserved, scheme.Name, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }
                else
                {
                    PowerSchemeSaveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    PowerSchemeSaveFileDialog.FileName = $"{scheme.Name}.pow";
                    if (PowerSchemeSaveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        Process process = new Process();


                        process.StartInfo.FileName = ExpandVars("@%SystemRoot%\\system32\\powercfg.exe");
                        process.StartInfo.ArgumentList.Clear();
                        process.StartInfo.ArgumentList.Add("/export");
                        process.StartInfo.ArgumentList.Add(PowerSchemeSaveFileDialog.FileName);
                        process.StartInfo.ArgumentList.Add($"{scheme.Id}");
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.StandardErrorEncoding = Encoding.Latin1;

                        if (process.Start())
                        {
                            if (!process.WaitForExit(4000))
                            {
                                process.Kill(true);
                            }
                            if (process.ExitCode != 0)
                            {
                                string errorOutput = process.StandardError.ReadToEnd();
                                MessageBox.Show(this, errorOutput, Properties.Resources.Saving_Power_Scheme, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }
        private void ImportSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PowerSchemeOpenFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            PowerSchemeOpenFileDialog.FileName = $"Scheme.pow";
            if (PowerSchemeOpenFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                Win32Error err = PowerImportPowerScheme(default, PowerSchemeOpenFileDialog.FileName, out SafeLocalHandle destinationSchemeGuid);
                if (err.Failed)
                {
                    MessageBox.Show(this, $"{err}", $"PowerImportPowerScheme({PowerSchemeOpenFileDialog.FileName},{Guid.Empty})");
                }
                else
                {
                    Guid newGuid = destinationSchemeGuid.ToStructure<Guid>();
                    err = PowerWriteFriendlyName(newGuid, null, null, Properties.Resources.New_Scheme_Name); ;
                    if (err.Failed)
                    {
                        MessageBox.Show(this, $"{err}", $"PowerWriteFriendlyName({PowerSchemeOpenFileDialog.FileName},{newGuid}");
                    }
                    Cursor = Cursors.WaitCursor;
                    LoadScheme(newGuid, Icons[PowerProfName][610]);
                    LoadNodes(ActiveSchemeGuid);
                    Cursor = Cursors.Default;
                }
            }

        }
        private void DeleteSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node && node.Tag is SchemeModel scheme)
            {
                if (scheme.Id != ActiveSchemeGuid && !SCHEMES_BY_DEFAULT.Contains(scheme.Id))
                {
                    if (MessageBox.Show(this, String.Format(Properties.Resources.Do_you_want_to_delete_the_schema, scheme.Name),
                                        scheme.Name,
                                        icon: MessageBoxIcon.Warning,
                                        buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Win32Error err = PowerDeleteScheme(default, scheme.Id);
                        if (err.Failed)
                        {
                            MessageBox.Show(this, $"{err}", $"PowerDeleteScheme({scheme.Name}");
                        }
                        else
                        {
                            Cursor = Cursors.WaitCursor;
                            Schemes.Remove(scheme.Id);
                            LoadNodes(ActiveSchemeGuid);
                            Cursor = Cursors.Default;
                        }
                    }
                }
            }
        }
        private void DuplicateSchemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node && node.Tag is SchemeModel scheme)
            {
                if (MessageBox.Show(this, String.Format(Properties.Resources.Do_you_want_to_duplicate_the_schema, scheme.Name),
                                         scheme.Name,
                                         icon: MessageBoxIcon.Warning,
                                         buttons: MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Win32Error err = PowerDuplicateScheme(default, scheme.Id, out SafeLocalHandle destinationSchemeGuid);
                    if (err.Failed)
                    {
                        MessageBox.Show(this, $"{err}", $"PowerDuplicateScheme({scheme.Name})");
                    }
                    else
                    {
                        Guid newGuid = destinationSchemeGuid.ToStructure<Guid>();
                        int n = 1;
                        while (Schemes.Values.FirstOrDefault(p => p.Name == $"{scheme.Name} ({n})") is SchemeModel)
                        {
                            n++;
                        }
                        err = PowerWriteFriendlyName(newGuid, null, null, $"{scheme.Name} ({n})");
                        if (err.Failed)
                        {
                            MessageBox.Show(this, $"{err}", $"PowerWriteFriendlyName({scheme.Name} ({n}),{newGuid}");
                        }
                        Cursor = Cursors.WaitCursor;
                        LoadScheme(newGuid, Icons[PowerProfName][610]);
                        LoadNodes(ActiveSchemeGuid);
                        Cursor = Cursors.Default;
                    }
                }
            }
        }
        // ====================================
        #endregion ToolStripMenuItems
        // ====================================

        // ====================================
        #region TreeView Events
        // ====================================
        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Icon icon = null;
            //if (tv.SelectedImageKey != e.Node?.ImageKey)
            //{
            //    tv.SelectedImageKey = e.Node?.ImageKey;
            //}
            if (e.Node?.Tag is SettingModel setting)
            {
                icon = setting.Icon;
            }
            else if (e.Node?.Tag is SchemeModel scheme)
            {
                icon = scheme.Icon;
            }
            if (e.Node?.Tag is GroupModel group)
            {
                icon = group.Icon;
            }
            if (icon != null)
            {
                PictureBox.Image = Bitmap.FromHicon(icon.Handle);
            }
            else
            {
                PictureBox.Image = null;
            }
        }
        private void tv_DoubleClick(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node && node.Tag is SettingValueModel settingValue)
            {
                if ((((settingValue.Setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) != POWER_ATTR.POWER_ATTRIBUTE_HIDE) &&
                    ((settingValue.Setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)) ||
                    ((Control.ModifierKeys & Keys.Control) == Keys.Control))
                {
                    if (settingValue.Setting.IsRange)
                    {
                        if ((settingValue.Setting.CanWriteDC && settingValue.DCMode) ||
                            (settingValue.Setting.CanWriteAC && !settingValue.DCMode) ||
                            ((Control.ModifierKeys & Keys.Control) == Keys.Control))

                        {
                            RangeEditor.Edit(node, settingValue.Setting, settingValue.DCMode);
                        }
                    }
                    else
                    {
                        if ((settingValue.Setting.CanWriteDC && settingValue.DCMode) ||
                            (settingValue.Setting.CanWriteAC && !settingValue.DCMode) ||
                            ((Control.ModifierKeys & Keys.Control) == Keys.Control))

                        {
                            DropDownEditor.Edit(node, settingValue.Setting, settingValue.DCMode);
                        }
                    }
                }
            }
        }
        private void tv_MouseClick(object sender, MouseEventArgs e)
        {
            DropDownEditor.Visible = false;
            RangeEditor.Visible = false;
        }
        private void tv_MouseDown(object sender, MouseEventArgs e)
        {
            if (tv.HitTest(e.Location) is TreeViewHitTestInfo testInfo)
            {
                tv.SelectedNode = testInfo.Node;
            }
        }
        private void tv_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = true;
            if (e.Node.Tag is SchemeModel scheme && !SCHEMES_BY_DEFAULT.Contains(scheme.Id) && scheme.CanWrite && IsUserAnAdmin())
            {
                e.CancelEdit = false;
            }
        }
        private void tv_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            e.CancelEdit = true;
            if (e.Node.Tag is SchemeModel scheme && !string.IsNullOrWhiteSpace(e.Label))
            {
                Win32Error err = PowerWriteFriendlyName(scheme.Id, null, null, e.Label.Trim());
                if (err.Succeeded)
                {
                    scheme.Name = PowerReadFriendlyName(scheme.Id);
                    e.Node.Text = scheme.Name;
                    e.CancelEdit = true;
                }
            }
        }

        // ====================================
        #endregion TreeView Events
        // ====================================


        private void LoadSchemes()
        {
            Win32Error err;
            char[] buffer = new char[1024];
            uint bufSize = (uint)buffer.Length;

            Cursor = Cursors.WaitCursor;
            Icons.Clear();
            Schemes.Clear();

            err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_CREATE_SCHEME, Guid.Empty);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Schemes.CanCreate = true;
            }

            err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_ACTIVE_SCHEME, Guid.Empty);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Schemes.CanSetActive = true;
            }

            Icons.Add(PowerProfName, new IconExtractor(PowerProfName));

            Icon = Icons[PowerProfName][610];

            //EnumPwrSchemes(p);


            PowerGetActiveScheme(out Guid activeScheme);
            ActiveSchemeGuid = activeScheme;



            IEnumerable<Guid> schemeIds = PowerEnumerate<Guid>(null, null);

            if (schemeIds is List<Guid> list)
            {
                foreach (var guid in SCHEMES_BY_DEFAULT)
                    if (!list.Contains(guid))
                    {
                        list.Insert(0, guid);
                    }
            }


            foreach (Guid schemeId in schemeIds)
            {
                LoadScheme(schemeId, Icons[PowerProfName][610]);
            }
            Cursor = Cursors.Default;

        }

        private void LoadSubgroup(SchemeModel Scheme, Guid subgroupId)
        {
            char[] buffer = new char[1024];
            uint bufSize = (uint)buffer.Length;
            Win32Error err;
            string keyIcon;
            string friendlyName = PowerReadFriendlyName(Scheme.Id, subgroupId);
            string description = PowerReadDescription(Scheme.Id, subgroupId);
            POWER_ATTR attributes = PowerReadSettingAttributes(subgroupId, Guid.Empty);

            err = PowerReadIconResourceSpecifier(default, GuidToInPtr(Scheme.Id), GuidToInPtr(subgroupId), IntPtr.Zero, IntPtr.Zero, ref bufSize);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                using (var mem = new SafeHGlobalHandle((int)bufSize))
                {
                    err = PowerReadIconResourceSpecifier(default, GuidToInPtr(Scheme.Id), GuidToInPtr(subgroupId), IntPtr.Zero, (IntPtr)mem, ref bufSize);
                    if (!err.Failed)
                    {
                        var value = StringHelper.GetString((IntPtr)mem, CharSet.Auto, bufSize);
                    }
                }
            }
            GroupModel Subgroup = new GroupModel() { Id = subgroupId, SchemeId = Scheme.Id, Name = friendlyName, Description = description, PowerAttr = attributes };

            err = ExtractIcon(Scheme.Id, subgroupId, Guid.Empty, out Icon groupIcon, out keyIcon);
            if (err.Succeeded)
            {
                Subgroup.Icon = groupIcon;
                Subgroup.KeyIcon = String.Empty;
            }
            Scheme.Groups.Add(subgroupId, Subgroup);
            IEnumerable<Guid> settingIds = PowerEnumerate<Guid>(Scheme.Id, subgroupId);
            foreach (Guid settingId in settingIds)
            {
                LoadSetting(Scheme, Subgroup, settingId);
            }
        }

        private void LoadSetting(SchemeModel Scheme, GroupModel Subgroup, Guid settingId)
        {
            uint bufSize;
            Win32Error err;
            string keyIcon;
            string friendlyName = PowerReadFriendlyName(Scheme.Id, Subgroup.Id, settingId);
            string description = PowerReadDescription(Scheme.Id, Subgroup.Id, settingId);
            POWER_ATTR attributes = PowerReadSettingAttributes(Subgroup.Id, settingId);
            SettingModel Setting = new SettingModel()
            {
                Id = settingId,
                SubgroupId = Subgroup.Id,
                SchemeId = Scheme.Id,
                Name = friendlyName,
                Description = description,
                PowerAttr = attributes
            };

            StringBuilder sb = new StringBuilder(1024);
            bufSize = 1024;
            err = PowerReadValueUnitsSpecifier(default, Subgroup.Id, settingId, sb, ref bufSize);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.Units = sb.ToString().Replace(":", "");
            }
            err = PowerReadValueIncrement(default, Subgroup.Id, settingId, out uint increment);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.Increment = increment;
            }
            err = PowerReadValueMax(default, Subgroup.Id, settingId, out uint maxValue);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.Maximum = maxValue;
            }
            err = PowerReadValueMin(default, Subgroup.Id, settingId, out uint minValue);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.Minimum = minValue;
            }


            err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_AC_POWER_SETTING_INDEX, settingId);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.CanWriteAC = true;
            }
            err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_DC_POWER_SETTING_INDEX, settingId);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.CanWriteDC = true;
            }


            Setting.PossibleValues.AddRange(ReadPossibleValues(Setting, Subgroup.Id, settingId));

            REG_VALUE_TYPE valueType;
            err = PowerReadACValue(default, Scheme.Id, Subgroup.Id, settingId, out valueType, IntPtr.Zero, ref bufSize);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Setting.ValueType = valueType;
                using (var mem = new SafeHGlobalHandle((int)bufSize))
                {
                    err = PowerReadACValue(default, Scheme.Id, Subgroup.Id, settingId, out valueType, (IntPtr)mem, ref bufSize);
                    if (!err.Failed)
                    {
                        Setting.ACValue = valueType.GetValue((IntPtr)mem, bufSize);
                        if (Setting.ACValue is byte[] data)
                        {
                            Setting.ACValue = new Guid(data);
                        }
                    }
                    err = PowerReadDCValue(default, Scheme.Id, Subgroup.Id, settingId, out valueType, (IntPtr)mem, ref bufSize);
                    if (!err.Failed)
                    {
                        Setting.DCValue = valueType.GetValue((IntPtr)mem, bufSize);
                        if (Setting.DCValue is byte[] data)
                        {
                            Setting.DCValue = new Guid(data);
                        }
                    }
                }

                err = PowerReadACValueIndex(default, Scheme.Id, Subgroup.Id, settingId, out uint acValueIndex);
                if (!err.Failed)
                {
                    Setting.ACValueIndex = acValueIndex;

                }
                err = PowerReadDCValueIndex(default, Scheme.Id, Subgroup.Id, settingId, out uint dcValueIndex);
                if (!err.Failed)
                {
                    Setting.DCValueIndex = dcValueIndex;
                }

                err = PowerReadACDefaultIndex(default, Scheme.Id, Subgroup.Id, settingId, out uint acDefaultIndex);
                if (!err.Failed)
                {
                    Setting.ACDefaultIndex = acDefaultIndex;

                }
                err = PowerReadDCDefaultIndex(default, Scheme.Id, Subgroup.Id, settingId, out uint dcDefaultIndex);
                if (!err.Failed)
                {
                    Setting.DCDefaultIndex = dcDefaultIndex;
                }
            }
            err = ExtractIcon(Scheme.Id, Subgroup.Id, settingId, out Icon settingIcon, out keyIcon);
            if (err.Succeeded)
            {
                Setting.Icon = settingIcon;
                Setting.KeyIcon = keyIcon;
            }
            Subgroup.Settings.Add(settingId, Setting);
        }

        private void LoadScheme(Guid schemeId, Icon defaultIcon)
        {
            char[] buffer = new char[1024];
            uint bufSize = (uint)buffer.Length;
            Win32Error err;
            string friendlyName = PowerReadFriendlyName(schemeId);
            string description = PowerReadDescription(schemeId);

            err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schemeId), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref bufSize);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                using (var mem = new SafeHGlobalHandle((int)bufSize))
                {
                    err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schemeId), IntPtr.Zero, IntPtr.Zero, (IntPtr)mem, ref bufSize);
                    if (!err.Failed)
                    {
                        var value = StringHelper.GetString((IntPtr)mem, CharSet.Auto, bufSize);
                    }
                }
            }

            SchemeModel Scheme = new SchemeModel() { Id = schemeId, Name = friendlyName, Description = description };
            err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_SCHEME, schemeId);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                Scheme.CanWrite = true;
            }
            err = ExtractIcon(schemeId, Guid.Empty, Guid.Empty, out Icon schemeIcon, out string keyIcon);
            if (err.Succeeded)
            {
                Scheme.Icon = schemeIcon;
            }
            Scheme.Icon ??= defaultIcon;
            Scheme.KeyIcon = keyIcon;
            Schemes.Add(schemeId, Scheme);

            IEnumerable<Guid> subgroupIds = PowerEnumerate<Guid>(schemeId, null);
            foreach (Guid subgroupId in subgroupIds)
            {
                LoadSubgroup(Scheme, subgroupId);
            }
        }

        private IEnumerable<PossibleValueModel> ReadPossibleValues(SettingModel parent, Guid subgroup, Guid setting)
        {
            List<PossibleValueModel> result = new List<PossibleValueModel>();
            parent.IsRange = true;
            if (!PowerIsSettingRangeDefined(subgroup, setting))
            {
                parent.IsRange = false;
                uint index = 0;
                Win32Error err = Win32Error.ERROR_SUCCESS;
                while (err == Win32Error.ERROR_SUCCESS)
                {
                    uint bufSize = 2048;
                    StringBuilder sb = new StringBuilder((int)bufSize);
                    err = PowerReadPossibleFriendlyName(default, subgroup, setting, index, sb, ref bufSize);
                    if (err.Failed && err != Win32Error.ERROR_MORE_DATA)
                    {
                        break;
                    }
                    PossibleValueModel value = new PossibleValueModel();
                    value.Setting = parent;
                    value.Index = (int)index;
                    value.Name = sb.ToString();
                    err = PowerReadPossibleDescription(default, subgroup, setting, index, sb, ref bufSize);
                    if (err.Failed && err != Win32Error.ERROR_MORE_DATA)
                    {
                        break;
                    }
                    value.Description = sb.ToString();
                    REG_VALUE_TYPE valueType;
                    err = PowerReadPossibleValue(default, subgroup, setting, out valueType, index, IntPtr.Zero, ref bufSize);
                    if (err.Failed && err != Win32Error.ERROR_MORE_DATA)
                    {
                        break;
                    }
                    value.ValueType = valueType;
                    using (var mem = new SafeHGlobalHandle((int)bufSize))
                    {
                        err = PowerReadPossibleValue(default, subgroup, setting, out valueType, index, (IntPtr)mem, ref bufSize);
                        if (err.Failed && err != Win32Error.ERROR_MORE_DATA)
                        {
                            break;
                        }
                        value.Value = valueType.GetValue((IntPtr)mem, bufSize);
                        if (value.Value is byte[] data)
                        {
                            value.Value = new Guid(data);
                        }
                    }
                    result.Add(value);
                    index++;
                    if (index > 10)
                    {
                        value = new PossibleValueModel() { Setting = value.Setting, Description = value.Description, Index = (int)index, Name = "..." };
                        result.Add(value);
                        break;
                    }
                    err = Win32Error.ERROR_SUCCESS;
                }
            }
            return result.ToArray();
        }

        private Win32Error ExtractIcon(Guid schemeGuid, Guid subgroupGuid, Guid settingGuid, out Icon image, out string key)
        {
            uint bufSize = 0;
            image = null;
            key = (subgroupGuid == Guid.Empty && settingGuid == Guid.Empty) ? "610" : String.Empty;
            Win32Error err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schemeGuid), GuidToInPtr(subgroupGuid), GuidToInPtr(settingGuid), IntPtr.Zero, ref bufSize);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                using (var mem = new SafeHGlobalHandle((int)bufSize))
                {
                    err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schemeGuid), GuidToInPtr(subgroupGuid), GuidToInPtr(settingGuid), (IntPtr)mem, ref bufSize);
                    if (!err.Failed)
                    {
                        var value = StringHelper.GetString((IntPtr)mem, CharSet.Auto, bufSize);
                        try
                        {
                            value = ExpandVars(value);
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                var values = value.Split(",");
                                if (values.Length > 0 && int.TryParse(values[1], out int id))
                                {
                                    if (!Icons.TryGetValue(values[0], out IconExtractor iconExtractor))
                                    {
                                        iconExtractor = new IconExtractor(values[0]);
                                        Icons.Add(values[0], iconExtractor);
                                    }
                                    image = iconExtractor.GetIcon(Math.Abs(id));
                                    if (subgroupGuid != Guid.Empty || settingGuid != Guid.Empty)
                                    {
                                        key = Math.Abs(id).ToString();
                                    }
                                    else
                                    {
                                        key = "610";
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }

            return err;
        }


        private void LoadNodes(Guid activeSchemeGuid)
        {
            Font italic = new Font(tv.Font, FontStyle.Italic);
            Font bold = new Font(tv.Font, FontStyle.Bold);
            tv.BeginUpdate();
            tv.Nodes.Clear();
            foreach (var s in Schemes.Values)
            {
                TreeNode sNode = tv.Nodes.Add(s.Id.ToString(), s.ToString());
                sNode.ToolTipText = s.ToolTip;
                sNode.Tag = s;
                sNode.ImageKey = s.KeyIcon;
                sNode.SelectedImageKey = s.KeyIcon;
                if (s.Id == activeSchemeGuid)
                {
                    sNode.NodeFont = bold;
                }

                foreach (var g in s.Groups.Values)
                {
                    if (g.Settings.Values.Any(a => HiddenCheckBox.Checked || (a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC))
                    {
                        if ((g.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) != POWER_ATTR.POWER_ATTRIBUTE_HIDE)
                        {
                            TreeNode gNode = sNode.Nodes.Add(g.Id.ToString(), g.ToString());
                            gNode.ToolTipText = g.ToolTip;
                            gNode.Tag = g;
                            gNode.ImageKey = g.KeyIcon;
                            gNode.SelectedImageKey = g.KeyIcon;
                            if ((g.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE)
                            {
                                gNode.NodeFont = italic;
                                gNode.ForeColor = SystemColors.GrayText;
                            }
                            else if (g.Settings.Values.Any(a => (a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC))
                            {
                                gNode.NodeFont = bold;
                                gNode.ForeColor = SystemColors.ControlText;
                            }
                            foreach (var a in g.Settings.Values)
                            {
                                if (HiddenCheckBox.Checked || (a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)
                                {
                                    TreeNode aNode = gNode.Nodes.Add(a.Id.ToString(), a.ToString());
                                    aNode.ToolTipText = a.ToolTip;
                                    aNode.Tag = a;
                                    aNode.ImageKey = a.KeyIcon;
                                    aNode.SelectedImageKey = a.KeyIcon;
                                    if ((a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE)
                                    {
                                        aNode.NodeFont = italic;
                                        aNode.ForeColor = SystemColors.GrayText;
                                    }
                                    else if ((a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)
                                    {
                                        aNode.NodeFont = bold;
                                        aNode.ForeColor = SystemColors.ControlText;
                                    }

                                    TreeNode acNode = aNode.Nodes.Add(a.ACString());
                                    acNode.ToolTipText = a.ACDefaultIndex.HasValue ? String.Format(Properties.Resources.Default_Value, a.ACDefaultIndexString()) : String.Empty;
                                    acNode.Tag = new SettingValueModel(false, a);
                                    TreeNode dcNode = aNode.Nodes.Add(a.DCString());
                                    dcNode.ToolTipText = a.DCDefaultIndex.HasValue ? String.Format(Properties.Resources.Default_Value, a.DCDefaultIndexString()) : String.Empty;
                                    dcNode.Tag = new SettingValueModel(true, a);

                                }
                            }
                        }
                    }
                }
            }
            if (tv.Nodes.Find(activeSchemeGuid.ToString(), false).FirstOrDefault() is TreeNode node)
            {
                node.Expand();
                tv.SelectedNode = node;
            }
            tv.EndUpdate();

        }



        #region EDITORS Events

        private void RangeEditor_RestoreDefaultClick(object sender, SettingArgs e)
        {
            RestoreDefaultIndex(e);
        }
        private void DropDownEditor_RestoreDefaultClick(object sender, SettingArgs e)
        {
            RestoreDefaultIndex(e);
        }

        private void RestoreDefaultIndex(SettingArgs args)
        {
            Win32Error err;
            args.Cancel = true;
            if (args.DCMode)
            {
                if (args.Setting.DCDefaultIndex.HasValue)
                {
                    err = PowerWriteDCValueIndex(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, args.Setting.DCDefaultIndex.Value);
                    if (err.Failed)
                    {
                        System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteDCValueIndex({args.Setting.Name},{args.Setting.DCDefaultIndexString()})");
                    }
                    else
                    {
                        args.Cancel = false;



                        err = PowerReadDCValueIndex(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, out uint dcValueIndex);
                        if (!err.Failed)
                        {
                            args.Setting.DCValueIndex = dcValueIndex;
                        }

                        uint bufSize = 0;
                        REG_VALUE_TYPE valueType;
                        err = PowerReadDCValue(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, out valueType, IntPtr.Zero, ref bufSize);
                        if (err.Succeeded)
                        {
                            args.Setting.ValueType = valueType;
                            using (var mem = new SafeHGlobalHandle((int)bufSize))
                            {
                                err = PowerReadDCValue(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, out valueType, (IntPtr)mem, ref bufSize);
                                if (!err.Failed)
                                {
                                    args.Setting.DCValue = valueType.GetValue((IntPtr)mem, bufSize);
                                    if (args.Setting.DCValue is byte[] data)
                                    {
                                        args.Setting.DCValue = new Guid(data);
                                    }
                                }
                            }
                        }


                    }
                }
            }
            else
            {
                if (args.Setting.ACDefaultIndex.HasValue)
                {
                    err = PowerWriteACValueIndex(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, args.Setting.ACDefaultIndex.Value);
                    if (err.Failed)
                    {
                        System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteACValueIndex({args.Setting.Name},{args.Setting.DCDefaultIndexString()})");
                    }
                    else
                    {
                        args.Cancel = false;

                        err = PowerReadACValueIndex(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, out uint acValueIndex);
                        if (!err.Failed)
                        {
                            args.Setting.ACValueIndex = acValueIndex;

                        }

                        uint bufSize = 0;
                        REG_VALUE_TYPE valueType;
                        err = PowerReadACValue(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, out valueType, IntPtr.Zero, ref bufSize);
                        if (err.Succeeded)
                        {
                            args.Setting.ValueType = valueType;
                            using (var mem = new SafeHGlobalHandle((int)bufSize))
                            {
                                err = PowerReadACValue(default, args.Setting.SchemeId, args.Setting.SubgroupId, args.Setting.Id, out valueType, (IntPtr)mem, ref bufSize);
                                if (!err.Failed)
                                {
                                    args.Setting.ACValue = valueType.GetValue((IntPtr)mem, bufSize);
                                    if (args.Setting.ACValue is byte[] data)
                                    {
                                        args.Setting.ACValue = new Guid(data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion EDITORS Events


        // ====================================
        #region BATTERY REPORT
        // ====================================

        private void BatteryReportButton_Click(object sender, EventArgs e)
        {
            if (!BatteryBackgroundWorker.IsBusy)
            {
                BatteryReportButton.BackColor = SystemColors.ControlDark;
                BatteryBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show(this, Properties.Resources.Battery_Report, "/BATTERYREPORT", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (BatteryBackgroundWorker.IsBusy)
                    {
                        BatteryBackgroundWorker.CancelAsync();
                    }
                }
            }

        }


        Process? BatteryReportProcess = null;
        private void BatteryBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BatteryReportProcess = new Process();
            BatteryReportProcess.StartInfo.FileName = ExpandVars("@%SystemRoot%\\system32\\powercfg.exe");
            BatteryReportProcess.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            BatteryReportProcess.StartInfo.ArgumentList.Clear();
            BatteryReportProcess.StartInfo.ArgumentList.Add("/BATTERYREPORT");
            BatteryReportProcess.StartInfo.UseShellExecute = false;
            BatteryReportProcess.StartInfo.CreateNoWindow = true;
            BatteryReportProcess.StartInfo.RedirectStandardOutput = true;
            BatteryReportProcess.StartInfo.RedirectStandardError = true;
            BatteryReportProcess.StartInfo.StandardOutputEncoding = Encoding.Latin1;
            BatteryReportProcess.StartInfo.StandardErrorEncoding = Encoding.Latin1;
            BatteryReportProcess.EnableRaisingEvents = true;
            BatteryReportProcess.Start();
            do
            {
                if (BatteryBackgroundWorker.CancellationPending)
                {
                    BatteryReportProcess.Kill(true);
                    e.Cancel = true;
                }

            } while (!BatteryReportProcess.HasExited);
        }

        private void BatteryBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            BatteryReportButton.BackColor = SystemColors.Control;
            if (BatteryReportProcess is Process process)
            {
                if (e.Cancelled)
                {
                    MessageBox.Show(this, Properties.Resources.Report_Cancelled, "/BATTERYREPORT", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }
                else if (process.ExitCode != 0)
                {
                    string errorOutput = process.StandardError.ReadToEnd();
                    MessageBox.Show(this, errorOutput, "/BATTERYREPORT", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
                else
                {
                    string consoleOutput = process.StandardOutput.ReadToEnd();
                    try
                    {
                        string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "battery-report.html");
                        process = new Process();
                        process.StartInfo.FileName = ExpandVars("@%ComSpec%");
                        process.StartInfo.ArgumentList.Clear();
                        process.StartInfo.ArgumentList.Add("/c");
                        process.StartInfo.ArgumentList.Add("start");
                        process.StartInfo.ArgumentList.Add($"{filename}");
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.StandardOutputEncoding = Encoding.Latin1;
                        process.StartInfo.StandardErrorEncoding = Encoding.Latin1;
                        process.Start();
                    }
                    catch
                    {
                        MessageBox.Show(this, consoleOutput, "/BATTERYREPORT", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    }
                }
            }
        }

        // ====================================
        #endregion BATTERY REPORT
        // ====================================

        // ====================================
        #region POWER REPORT
        // ====================================

        private void PowerReportButton_Click(object sender, EventArgs e)
        {
            if (!PowerReportBackgroundWorker.IsBusy)
            {
                PowerProgressBar.Value = 0;
                PowerProgressBar.Visible = true;
                PowerReportButton.BackColor = SystemColors.ControlDark;
                PowerReportBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show(this, Properties.Resources.Power_Report, "/SYSTEMPOWERREPORT", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (PowerReportBackgroundWorker.IsBusy)
                    {
                        PowerReportBackgroundWorker.CancelAsync();
                    }
                }
            }
        }

        Process? PowerReportProcess = null;
        private void PowerReportBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            PowerReportProcess = new Process();

            PowerReportProcess.StartInfo.FileName = ExpandVars("@%SystemRoot%\\system32\\powercfg.exe");
            PowerReportProcess.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            PowerReportProcess.StartInfo.ArgumentList.Clear();
            PowerReportProcess.StartInfo.ArgumentList.Add("/SYSTEMPOWERREPORT");
            PowerReportProcess.StartInfo.UseShellExecute = false;
            PowerReportProcess.StartInfo.CreateNoWindow = true;
            PowerReportProcess.StartInfo.RedirectStandardOutput = true;
            PowerReportProcess.StartInfo.RedirectStandardError = true;
            PowerReportProcess.StartInfo.StandardOutputEncoding = Encoding.Latin1;
            PowerReportProcess.StartInfo.StandardErrorEncoding = Encoding.Latin1;
            PowerReportProcess.EnableRaisingEvents = true;

            PowerReportProcess.Start();
            DateTime lap = DateTime.Now;
            int seconds = 0;
            do
            {
                if (PowerReportBackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    PowerReportProcess.Kill(true);
                }
                if ((DateTime.Now - lap).TotalSeconds > 1)
                {
                    seconds++;
                    lap = DateTime.Now;
                    PowerReportBackgroundWorker.ReportProgress(seconds);
                }
            } while (!PowerReportProcess.HasExited);
        }

        private void PowerReportBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            PowerProgressBar.Value = (e.ProgressPercentage % PowerProgressBar.Maximum);
        }

        private void PowerReportBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            PowerReportButton.BackColor = SystemColors.Control;
            PowerProgressBar.Visible = false;
            if (PowerReportProcess is Process process)
            {
                if (e.Cancelled)
                {

                    MessageBox.Show(this, Properties.Resources.Report_Cancelled, "/SYSTEMPOWERREPORT", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }
                else if (process.ExitCode != 0)
                {
                    string errorOutput = process.StandardError.ReadToEnd();
                    MessageBox.Show(this, errorOutput, "/SYSTEMPOWERREPORT", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
                else
                {
                    string consoleOutput = process.StandardOutput.ReadToEnd();
                    try
                    {
                        string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sleepstudy-report.html");
                        process = new Process();
                        process.StartInfo.FileName = ExpandVars("@%ComSpec%");
                        process.StartInfo.ArgumentList.Clear();
                        process.StartInfo.ArgumentList.Add("/c");
                        process.StartInfo.ArgumentList.Add("start");
                        process.StartInfo.ArgumentList.Add($"{filename}");
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.StandardOutputEncoding = Encoding.Latin1;
                        process.StartInfo.StandardErrorEncoding = Encoding.Latin1;
                        process.Start();
                    }
                    catch
                    {
                        MessageBox.Show(this, consoleOutput, "/SYSTEMPOWERREPORT", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    }
                }
            }
        }

        // ====================================
        #endregion POWER REPORT
        // ====================================

        // ====================================
        #region ENERGY REPORT
        // ====================================
        private void EnergyReportButton_Click(object sender, EventArgs e)
        {
            if (!EnergyReportBackgroundWorker.IsBusy)
            {
                EnergyProgressBar.Value = 0;
                EnergyProgressBar.Visible = true;
                EnergyReportButton.BackColor = SystemColors.ControlDark;
                EnergyReportBackgroundWorker.RunWorkerAsync();
            }
            else
            {
                if (MessageBox.Show(this, Properties.Resources.Energy_Report, "/ENERGY", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (EnergyReportBackgroundWorker.IsBusy)
                    {
                        EnergyReportBackgroundWorker.CancelAsync();
                    }
                }
            }
        }
        Process? EnergyProcess = null;
        private void EnergyBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {


            EnergyProcess = new Process();
            EnergyProcess.StartInfo.FileName = ExpandVars("@%SystemRoot%\\system32\\powercfg.exe");
            EnergyProcess.StartInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            EnergyProcess.StartInfo.ArgumentList.Clear();
            EnergyProcess.StartInfo.ArgumentList.Add("/ENERGY");
            EnergyProcess.StartInfo.UseShellExecute = false;
            EnergyProcess.StartInfo.CreateNoWindow = true;
            EnergyProcess.StartInfo.RedirectStandardOutput = true;
            EnergyProcess.StartInfo.RedirectStandardError = true;
            EnergyProcess.StartInfo.StandardOutputEncoding = Encoding.Latin1;
            EnergyProcess.StartInfo.StandardErrorEncoding = Encoding.Latin1;
            EnergyProcess.EnableRaisingEvents = true;
            EnergyProcess.Start();
            DateTime lap = DateTime.Now;
            int seconds = 0;
            do
            {
                if (EnergyReportBackgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    EnergyProcess.Kill(true);
                }
                if ((DateTime.Now - lap).TotalSeconds > 1)
                {
                    seconds++;
                    lap = DateTime.Now;
                    EnergyReportBackgroundWorker.ReportProgress(seconds);
                }
            } while (!EnergyProcess.HasExited);
        }

        private void EnergyBackgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            EnergyProgressBar.Value = (e.ProgressPercentage % EnergyProgressBar.Maximum);
        }

        private void EnergyBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            EnergyReportButton.BackColor = SystemColors.Control;
            EnergyProgressBar.Visible = false;
            if (EnergyProcess is Process process)
            {
                if (e.Cancelled)
                {

                    MessageBox.Show(this, Properties.Resources.Report_Cancelled, "/ENERGY", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }
                else if (EnergyProcess.ExitCode != 0)
                {
                    string errorOutput = EnergyProcess.StandardError.ReadToEnd();
                    MessageBox.Show(this, errorOutput, "/ENERGY", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
                else
                {
                    string consoleOutput = EnergyProcess.StandardOutput.ReadToEnd();
                    try
                    {
                        string filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "energy-report.html");
                        process = new Process();
                        process.StartInfo.FileName = ExpandVars("@%ComSpec%");
                        process.StartInfo.ArgumentList.Clear();
                        process.StartInfo.ArgumentList.Add("/c");
                        process.StartInfo.ArgumentList.Add("start");
                        process.StartInfo.ArgumentList.Add($"{filename}");
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.StandardOutputEncoding = Encoding.Latin1;
                        process.StartInfo.StandardErrorEncoding = Encoding.Latin1;
                        process.Start();
                    }
                    catch
                    {
                        MessageBox.Show(this, consoleOutput, "/ENERGY", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                    }
                }
            }
        }
        // ====================================
        #endregion ENERGY REPORT
        // ====================================


    }
}