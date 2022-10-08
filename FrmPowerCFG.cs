using System.Collections;
using System.Text;
using Vanara.InteropServices;
using Vanara.PInvoke;
using Vanara.Extensions;
using static Vanara.PInvoke.PowrProf;
using System.Runtime.InteropServices;
using PowerCFG.Models;
using PowerCFG.Components;
using System;

namespace PowerCFG
{
    public partial class FrmPowerCFG : Form
    {

        public readonly Guid SCHEME_MAX = new Guid("a1841308-3541-4fab-bc81-f71556f20b4a");
        public readonly Guid SCHEME_MIN = new Guid("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
        public readonly Guid SCHEME_BALANCED = new Guid("381b4222-f694-41f0-9685-ff5bb260df2e");

        public FrmPowerCFG()
        {
            InitializeComponent();
        }

        Guid ActiveSchemaGuid;

        public SchemasModel Schemas = new SchemasModel();

        Dictionary<string, IconExtractor> Icons = new Dictionary<string, IconExtractor>();

        private static unsafe IntPtr GuidToInPtr(Guid g1)
        {
            return (IntPtr)(void*)&g1;
        }

        private void FrmPowerCFG_Load(object sender, EventArgs e)
        {
            LoadSchemas();
            LoadNodes(ActiveSchemaGuid);
        }





        private void LoadSchemas()
        {

            char[] buffer = new char[1024];
            uint bufSize = (uint)buffer.Length;
            string resourceName = ExpandVars("@%SystemRoot%\\system32\\powrprof.dll");
            Cursor = Cursors.WaitCursor;
            Icons.Clear();
            Schemas.Clear();
            Icons.Add(resourceName, new IconExtractor(resourceName));

            Icon = Icons[resourceName][610];

            //EnumPwrSchemes(p);


            PowerGetActiveScheme(out Guid activeScheme);
            ActiveSchemaGuid = activeScheme;


            Win32Error err;
            IEnumerable<Guid> schemas = PowerEnumerate<Guid>(null, null);

            if (schemas is List<Guid> list)
            {
                if (!list.Contains(SCHEME_MIN))
                {
                    list.Insert(0, SCHEME_MIN);
                }
                if (!list.Contains(SCHEME_MAX))
                {
                    list.Insert(0, SCHEME_MAX);
                }

                if (!list.Contains(SCHEME_BALANCED))
                {
                    list.Insert(0, SCHEME_BALANCED);
                }
            }


            foreach (Guid schema in schemas)
            {
                string friendlyName = PowerReadFriendlyName(schema);
                string description = PowerReadDescription(schema);

                err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schema), IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, ref bufSize);
                if (err == Win32Error.ERROR_SUCCESS)
                {
                    using (var mem = new SafeHGlobalHandle((int)bufSize))
                    {
                        err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schema), IntPtr.Zero, IntPtr.Zero, (IntPtr)mem, ref bufSize);
                        if (!err.Failed)
                        {
                            var value = StringHelper.GetString((IntPtr)mem, CharSet.Auto, bufSize);
                        }
                    }
                }

                POWER_ATTR attributes;
                SchemaModel Schema = new SchemaModel() { Id = schema, Name = friendlyName, Description = description };
                err = ExtractIcon(schema, Guid.Empty, Guid.Empty, out Icon schemaIcon);
                if (err.Succeeded)
                {
                    Schema.Icon = schemaIcon;
                }
                Schema.Icon ??= Icons[resourceName][610];
                Schemas.Add(schema, Schema);

                IEnumerable<Guid> subgroups = PowerEnumerate<Guid>(schema, null);
                foreach (Guid subgroup in subgroups)
                {
                    friendlyName = PowerReadFriendlyName(schema, subgroup);
                    description = PowerReadDescription(schema, subgroup);
                    attributes = PowerReadSettingAttributes(subgroup, Guid.Empty);

                    err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schema), GuidToInPtr(subgroup), IntPtr.Zero, IntPtr.Zero, ref bufSize);
                    if (err == Win32Error.ERROR_SUCCESS)
                    {
                        using (var mem = new SafeHGlobalHandle((int)bufSize))
                        {
                            err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schema), GuidToInPtr(subgroup), IntPtr.Zero, (IntPtr)mem, ref bufSize);
                            if (!err.Failed)
                            {
                                var value = StringHelper.GetString((IntPtr)mem, CharSet.Auto, bufSize);
                            }
                        }
                    }
                    GroupModel Subgroup = new GroupModel() { Id = subgroup, SchemaId = schema, Name = friendlyName, Description = description, PowerAttr = attributes };

                    err = ExtractIcon(schema, subgroup, Guid.Empty, out Icon groupIcon);
                    if (err.Succeeded)
                    {
                        Subgroup.Icon = groupIcon;
                    }
                    Schema.Groups.Add(subgroup, Subgroup);
                    IEnumerable<Guid> settings = PowerEnumerate<Guid>(schema, subgroup);
                    foreach (Guid setting in settings)
                    {
                        friendlyName = PowerReadFriendlyName(schema, subgroup, setting);
                        description = PowerReadDescription(schema, subgroup, setting);
                        attributes = PowerReadSettingAttributes(subgroup, setting);
                        SettingModel Setting = new SettingModel() { Id = setting, SubgroupId = subgroup, SchemaId = schema, Name = friendlyName, Description = description, PowerAttr = attributes };

                        StringBuilder sb = new StringBuilder(1024);
                        bufSize = 1024;
                        err = PowerReadValueUnitsSpecifier(default, subgroup, setting, sb, ref bufSize);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.Units = sb.ToString().Replace(":", "");
                        }
                        err = PowerReadValueIncrement(default, subgroup, setting, out uint increment);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.Increment = increment;
                        }
                        err = PowerReadValueMax(default, subgroup, setting, out uint maxValue);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.Maximum = maxValue;
                        }
                        err = PowerReadValueMin(default, subgroup, setting, out uint minValue);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.Minimum = minValue;
                        }

                        err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_AC_POWER_SETTING_INDEX, setting);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.CanWriteAC = true;
                        }
                        err = PowerSettingAccessCheck(POWER_DATA_ACCESSOR.ACCESS_DC_POWER_SETTING_INDEX, setting);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.CanWriteDC = true;
                        }


                        Setting.PossibleValues.AddRange(ReadPossibleValues(Setting, subgroup, setting));

                        REG_VALUE_TYPE valueType;
                        err = PowerReadACValue(default, schema, subgroup, setting, out valueType, IntPtr.Zero, ref bufSize);
                        if (err == Win32Error.ERROR_SUCCESS)
                        {
                            Setting.ValueType = valueType;
                            using (var mem = new SafeHGlobalHandle((int)bufSize))
                            {
                                err = PowerReadACValue(default, schema, subgroup, setting, out valueType, (IntPtr)mem, ref bufSize);
                                if (!err.Failed)
                                {
                                    Setting.ACValue = valueType.GetValue((IntPtr)mem, bufSize);
                                    if (Setting.ACValue is byte[] data)
                                    {
                                        Setting.ACValue = new Guid(data);
                                    }
                                }
                                err = PowerReadDCValue(default, schema, subgroup, setting, out valueType, (IntPtr)mem, ref bufSize);
                                if (!err.Failed)
                                {
                                    Setting.DCValue = valueType.GetValue((IntPtr)mem, bufSize);
                                    if (Setting.DCValue is byte[] data)
                                    {
                                        Setting.DCValue = new Guid(data);
                                    }
                                }
                            }

                            err = PowerReadACValueIndex(default, schema, subgroup, setting, out uint acValueIndex);
                            if (!err.Failed)
                            {
                                Setting.ACValueIndex = acValueIndex;

                            }
                            err = PowerReadDCValueIndex(default, schema, subgroup, setting, out uint dcValueIndex);
                            if (!err.Failed)
                            {
                                Setting.DCValueIndex = dcValueIndex;
                            }

                            err = PowerReadACDefaultIndex(default, schema, subgroup, setting, out uint acDefaultIndex);
                            if (!err.Failed)
                            {
                                Setting.ACDefaultIndex = acDefaultIndex;

                            }
                            err = PowerReadDCDefaultIndex(default, schema, subgroup, setting, out uint dcDefaultIndex);
                            if (!err.Failed)
                            {
                                Setting.DCDefaultIndex = dcDefaultIndex;
                            }
                        }
                        err = ExtractIcon(schema, subgroup, setting, out Icon settingIcon);
                        if (err.Succeeded)
                        {
                            Setting.Icon = settingIcon;
                        }
                        Subgroup.Settings.Add(setting, Setting);

                    }
                }
            }
            Cursor = Cursors.Default;

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

        private Win32Error ExtractIcon(Guid schema, Guid subgroup, Guid setting, out Icon image)
        {
            uint bufSize = 0;
            image = null;
            Win32Error err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schema), GuidToInPtr(subgroup), GuidToInPtr(setting), IntPtr.Zero, ref bufSize);
            if (err == Win32Error.ERROR_SUCCESS)
            {
                using (var mem = new SafeHGlobalHandle((int)bufSize))
                {
                    err = PowerReadIconResourceSpecifier(default, GuidToInPtr(schema), GuidToInPtr(subgroup), GuidToInPtr(setting), (IntPtr)mem, ref bufSize);
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

        IDictionary EnvironmentVariables = Environment.GetEnvironmentVariables();

        private string ExpandVars(string? value)
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


        private void LoadNodes(Guid activeSchemeGuid)
        {
            Font italic = new Font(tv.Font, FontStyle.Italic);
            Font bold = new Font(tv.Font, FontStyle.Bold);
            tv.BeginUpdate();
            tv.Nodes.Clear();
            foreach (var s in Schemas.Values)
            {
                TreeNode sNode = tv.Nodes.Add(s.Id.ToString(), s.ToString());
                sNode.ToolTipText = s.ToolTip;
                sNode.Tag = s;
                if (s.Id == activeSchemeGuid)
                {
                    sNode.NodeFont = bold;
                }

                foreach (var g in s.Groups.Values)
                {
                    if (g.Settings.Values.Any(a => !UnhiddenCheckBox.Checked || (a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC))
                    {
                        if ((g.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) != POWER_ATTR.POWER_ATTRIBUTE_HIDE)
                        {
                            TreeNode gNode = sNode.Nodes.Add(g.Id.ToString(), g.ToString());
                            gNode.ToolTipText = g.ToolTip;
                            gNode.Tag = g;
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
                                if (!UnhiddenCheckBox.Checked || (a.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC)
                                {
                                    TreeNode aNode = gNode.Nodes.Add(a.Id.ToString(), a.ToString());
                                    aNode.ToolTipText = a.ToolTip;
                                    aNode.Tag = a;
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

        private void UnhiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadSchemas();
            LoadNodes(ActiveSchemaGuid);
        }


        SchemaModel? SelectedSchema = null;

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SelectedSchema = null;


            if (tv.SelectedNode?.Tag is SchemaModel schema)
            {
                SelectedSchema = schema;
                HiddenToolStripMenuItem.Visible = false;
                ShowToolStripMenuItem.Visible = false;
                ActivateSchemaToolStripMenuItem.Visible = (schema.Id != ActiveSchemaGuid);
                CopyGUIDToolStripMenuItem.Visible = SchemaModel.ShowGuids;
            }
            else if (tv.SelectedNode?.Tag is GroupModel group)
            {
                HiddenToolStripMenuItem.Visible = true;
                ShowToolStripMenuItem.Visible = true;
                ActivateSchemaToolStripMenuItem.Visible = false;
                HiddenToolStripMenuItem.Checked = (group.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE;
                ShowToolStripMenuItem.Checked = (group.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC;
                CopyGUIDToolStripMenuItem.Visible = GroupModel.ShowGuids;
            }
            else if (tv.SelectedNode?.Tag is SettingModel setting)
            {
                HiddenToolStripMenuItem.Visible = true;
                ShowToolStripMenuItem.Visible = true;
                ActivateSchemaToolStripMenuItem.Visible = false;
                HiddenToolStripMenuItem.Checked = (setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_HIDE) == POWER_ATTR.POWER_ATTRIBUTE_HIDE;
                ShowToolStripMenuItem.Checked = (setting.PowerAttr & POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC) == POWER_ATTR.POWER_ATTRIBUTE_SHOW_AOAC;
                CopyGUIDToolStripMenuItem.Visible = SettingModel.ShowGuids;
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void CopyGUIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode?.Tag is SchemaModel schema)
            {
                Clipboard.SetText($"{schema.Id}");
            }
            else if (tv.SelectedNode?.Tag is GroupModel group)
            {
                Clipboard.SetText($"{group.Id}");
            }
            else if (tv.SelectedNode?.Tag is SettingModel setting)
            {
                Clipboard.SetText($"{setting.Id}");
            }
        }

        private void ActivateSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedSchema != null)
            {

                Win32Error err = PowerSetActiveScheme(default, SelectedSchema.Id);
                if (err.Failed)
                {
                    System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerSetActiveScheme({SelectedSchema.Name})");
                }
                else
                {
                    ActiveSchemaGuid = SelectedSchema.Id;
                    Font bold = new Font(tv.Font, FontStyle.Bold);
                    foreach (TreeNode node in tv.Nodes)
                    {
                        if (node?.Tag is SchemaModel schema)
                        {
                            if (schema.Id == ActiveSchemaGuid)
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

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Icon icon = null;
            if (e.Node?.Tag is SettingModel setting)
            {
                icon = setting.Icon;
            }
            else if (e.Node?.Tag is SchemaModel schema)
            {
                icon = schema.Icon;
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

        private void ExpandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node)
            {
                node.ExpandAll();
            }
        }

        private void CollapseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode is TreeNode node)
            {
                node.Collapse(false);
            }
        }

        private void tv_MouseClick(object sender, MouseEventArgs e)
        {
            DropDownEditor.Visible = false;
            RangeEditor.Visible = false;
        }

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
                    err = PowerWriteDCValueIndex(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, args.Setting.DCDefaultIndex.Value);
                    if (err.Failed)
                    {
                        System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteDCValueIndex({args.Setting.Name},{args.Setting.DCDefaultIndexString()})");
                    }
                    else
                    {
                        args.Cancel = false;



                        err = PowerReadDCValueIndex(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, out uint dcValueIndex);
                        if (!err.Failed)
                        {
                            args.Setting.DCValueIndex = dcValueIndex;
                        }

                        uint bufSize = 0;
                        REG_VALUE_TYPE valueType;
                        err = PowerReadDCValue(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, out valueType, IntPtr.Zero, ref bufSize);
                        if (err.Succeeded)
                        {
                            args.Setting.ValueType = valueType;
                            using (var mem = new SafeHGlobalHandle((int)bufSize))
                            {
                                err = PowerReadDCValue(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, out valueType, (IntPtr)mem, ref bufSize);
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
                    err = PowerWriteACValueIndex(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, args.Setting.ACDefaultIndex.Value);
                    if (err.Failed)
                    {
                        System.Windows.Forms.MessageBox.Show(this, $"{err}", $"PowerWriteACValueIndex({args.Setting.Name},{args.Setting.DCDefaultIndexString()})");
                    }
                    else
                    {
                        args.Cancel = false;

                        err = PowerReadACValueIndex(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, out uint acValueIndex);
                        if (!err.Failed)
                        {
                            args.Setting.ACValueIndex = acValueIndex;

                        }

                        uint bufSize = 0;
                        REG_VALUE_TYPE valueType;
                        err = PowerReadACValue(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, out valueType, IntPtr.Zero, ref bufSize);
                        if (err.Succeeded)
                        {
                            args.Setting.ValueType = valueType;
                            using (var mem = new SafeHGlobalHandle((int)bufSize))
                            {
                                err = PowerReadACValue(default, args.Setting.SchemaId, args.Setting.SubgroupId, args.Setting.Id, out valueType, (IntPtr)mem, ref bufSize);
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

        private void tv_MouseDown(object sender, MouseEventArgs e)
        {
            if (tv.HitTest(e.Location) is TreeViewHitTestInfo testInfo)
            {
                tv.SelectedNode = testInfo.Node;

            }
        }

        private void ShowGuidsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SettingModel.ShowGuids = !SettingModel.ShowGuids;
            LoadNodes(ActiveSchemaGuid);
        }

        private void ExcelExportButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(Schemas.ExcelExport(UnhiddenCheckBox.Checked));
            MessageBox.Show(this, Properties.Resources.Paste_into_Excel);
        }

        //public class PowerScheme
        //{
        //    public uint uiIndex { get; set; }
        //    public uint dwName { get; set; }
        //    public string sName { get; set; }
        //    public uint dwDesc { get; set; }
        //    public string sDesc { get; set; }
        //    public POWER_POLICY pp { get; set; }
        //}
        //List<PowerScheme> PowerSchemes = new List<PowerScheme>();

        //public bool p(uint uiIndex, uint dwName, string sName, uint dwDesc, string sDesc, in POWER_POLICY pp, IntPtr lParam)
        //{
        //    PowerSchemes.Add(new PowerScheme()
        //    {
        //        dwDesc = dwDesc,
        //        dwName = dwName,
        //        sName = sName,
        //        pp = pp,
        //        sDesc = sDesc,
        //        uiIndex = uiIndex,
        //    });
        //    return true;
        //}

    }
}