using PowerCFG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Vanara.InteropServices;
using Vanara.PInvoke;
using Vanara.Extensions;
using static Vanara.PInvoke.PowrProf;
using static Vanara.PInvoke.Ole32.PROPERTYKEY.System;

namespace PowerCFG.Components
{

    public partial class DropDownEditor : UserControl
    {
        private bool loading = true;
        public DropDownEditor()
        {
            InitializeComponent();
            Visible = DesignMode;
        }

        public TreeNode Node { get; set; } = null!;
        public bool DCMode { get; set; }
        public SettingModel Setting { get; set; } = null!;

        public class ItemValue
        {
            public ItemValue(PossibleValueModel value)
            {
                Index = value.Index;
                Name = value.Name;
                ToolTip = value.Description;
                Value = value.Value;
            }

            public int Index { get; set; }
            public string Name { get; set; }
            public string ToolTip { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Name;
            }
        }

        public bool Edit(TreeNode node, SettingModel setting, bool dc = false)
        {
            Node = node;
            Setting = setting;
            DCMode = dc;
            if (setting != null && !setting.IsRange)
            {
                Graphics canvas = CreateGraphics();

                NameLabel.Text = $"{(dc ? Properties.Resources.On_battery : Properties.Resources.On_AC_Power)}: ";
                RestoreDefaultButton.Visible = dc ? Setting.DCDefaultIndex.HasValue : Setting.ACDefaultIndex.HasValue;
                toolTip.SetToolTip(RestoreDefaultButton, String.Format(Properties.Resources.Restore_to, dc ? Setting.DCDefaultIndexString() : Setting.ACDefaultIndexString()));
                float captionWith = canvas.MeasureString(NameLabel.Text, Font).Width * 105 / 100;
                float buttonWith = RestoreDefaultButton.Width;
                ValueComboBox.Items.Clear();
                ItemValue? firstItem = null;
                float valueWidth = 20;
                foreach (var value in setting.PossibleValues)
                {
                    valueWidth = Math.Max(valueWidth, canvas.MeasureString($"{value.Name}", Font).Width + 20);
                    ItemValue item = new ItemValue(value);
                    ValueComboBox.Items.Add(item);
                    if (Setting.DCValue is Guid)
                    {
                        if (dc && (Guid)value.Value == (Guid)setting.DCValue)
                        {
                            firstItem ??= item;
                        }
                        else if (!dc && (Guid)value.Value == (Guid)setting.ACValue)
                        {
                            firstItem ??= item;
                        }
                    }
                    else
                    {
                        if (dc && (uint)value.Value == (uint)setting.DCValue)
                        {
                            firstItem ??= item;
                        }
                        else if (!dc && (uint)value.Value == (uint)setting.ACValue)
                        {
                            firstItem ??= item;
                        }
                    }
                }
                canvas.Dispose();

                Point location = node.Bounds.Location;
                location.Offset(node.TreeView.Location);
                ValueComboBox.Width = (int)valueWidth;
                Bounds = new Rectangle(location, new Size((int)(captionWith + valueWidth + buttonWith), Height));
                ValueComboBox.SelectedItem = firstItem;
                ValueComboBox.Focus();
                Visible = true;
                loading = false;
                return true;
            }
            return false;
        }

        private void ValueComboBox_Leave(object sender, EventArgs e)
        {
            if (Setting != null)
            {
                if (DCMode)
                {
                    try
                    {
                        Node.Text = Setting.DCString();
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                        Node.Text = Setting.ACString();
                    }
                    catch
                    {

                    }
                }
            }
            //Visible = false;
        }

        private void ValueComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ValueComboBox.SelectedItem is ItemValue item && !loading)
            {
                toolTip.SetToolTip(ValueComboBox, item.ToolTip);
                if (DCMode)
                {
                    Setting.DCValue = item.Value;
                    Win32Error err = PowerWriteDCValueIndex(default, Setting.SchemeId, Setting.SubgroupId, Setting.Id, (uint)item.Index);
                    if (err.Failed)
                    {
                        MessageBox.Show(this, err.FormatMessage(), "PowerWriteDCValueIndex", MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Setting.ACValue = item.Value;
                    Win32Error err = PowerWriteACValueIndex(default, Setting.SchemeId, Setting.SubgroupId, Setting.Id, (uint)item.Index);
                    if (err.Failed)
                    {
                        MessageBox.Show(this, err.FormatMessage(), "PowerWriteACValueIndex", MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                    }
                }
            }
        }

        public event RestoreDefaultClickHandler RestoreDefaultClick;
        private void RestoreDefaultButton_Click(object sender, EventArgs e)
        {
            SettingArgs args = new SettingArgs() { Node = Node, Setting = Setting, DCMode = DCMode };
            RestoreDefaultClick?.Invoke(sender, args);
            if (!args.Cancel)
            {
                loading = true;

                ItemValue? itemSelected = null;
                foreach (var item in ValueComboBox.Items)
                {
                    if (item is ItemValue itemValue)
                    {
                        if (Setting.DCValue is Guid)
                        {
                            if (args.DCMode && (Guid)itemValue.Value == (Guid)args.Setting.DCValue)
                            {
                                itemSelected ??= itemValue;
                            }
                            else if (!args.DCMode && (Guid)itemValue.Value == (Guid)args.Setting.ACValue)
                            {
                                itemSelected ??= itemValue;
                            }
                        }
                        else
                        {
                            if (args.DCMode && (uint)itemValue.Value == (uint)args.Setting.DCValue)
                            {
                                itemSelected ??= itemValue;
                            }
                            else if (!args.DCMode && (uint)itemValue.Value == (uint)args.Setting.ACValue)
                            {
                                itemSelected ??= itemValue;
                            }
                        }
                    }
                    if (itemSelected != null) break;
                }
                args.Node.Text = args.DCMode ? args.Setting.DCString() : args.Setting.ACString();
                ValueComboBox.SelectedItem = itemSelected;

                loading = false;
            }
        }
    }
}
