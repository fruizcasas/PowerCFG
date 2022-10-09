using PowerCFG.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Vanara.InteropServices;
using Vanara.PInvoke;
using Vanara.Extensions;
using static Vanara.PInvoke.PowrProf;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static PowerCFG.Components.DropDownEditor;
using Vanara.Extensions.Reflection;

namespace PowerCFG.Components
{
    public partial class RangeEditor : UserControl
    {
        private bool loading = true;
        public RangeEditor()
        {
            InitializeComponent();
            Visible = DesignMode;
        }

        public TreeNode Node { get; set; } = null!;
        public bool DCMode { get; set; }
        public SettingModel Setting { get; set; } = null!;

        public bool Edit(TreeNode node, SettingModel setting, bool dc = false)
        {
            Node = node;
            Setting = setting;
            DCMode = dc;
            if (setting != null && setting.IsRange)
            {
                Graphics canvas = CreateGraphics();
                NameLabel.Text = $"{(dc ? Properties.Resources.On_battery : Properties.Resources.On_AC_Power)} ({setting.Units})";
                RestoreDefaultButton.Visible = dc ? Setting.DCDefaultIndex.HasValue : Setting.ACDefaultIndex.HasValue;
                toolTip.SetToolTip(RestoreDefaultButton, String.Format(Properties.Resources.Restore_to, dc ? Setting.DCDefaultIndexString() : Setting.ACDefaultIndexString()));
                toolTip.SetToolTip(ValueNumericUpDown, String.Format(Properties.Resources.RangeFromToIncrement, setting.Minimum, setting.Maximum, setting.Increment, setting.Units));

                float captionWith = canvas.MeasureString(NameLabel.Text, Font).Width * 105 / 100;
                float buttonWith = RestoreDefaultButton.Width;
                float numericWidth = canvas.MeasureString($"{setting.Maximum}", Font).Width + 30;
                canvas.Dispose();
                ValueNumericUpDown.Size = new Size((int)numericWidth, Height);
                Point location = node.Bounds.Location;
                location.Offset(node.TreeView.Location);
                Bounds = new Rectangle(location, new Size((int)(captionWith + numericWidth + buttonWith), Height));
                ValueNumericUpDown.Maximum = setting.Maximum;
                ValueNumericUpDown.Minimum = setting.Minimum;
                ValueNumericUpDown.Increment = setting.Increment;
                if (dc && setting.DCValue != null)
                {
                    ValueNumericUpDown.Value = (uint)setting.DCValue;
                    Visible = true;
                    ValueNumericUpDown.Focus();
                    loading = false;
                    return true;
                }
                if (!dc && setting.ACValue != null)
                {
                    ValueNumericUpDown.Value = (uint)setting.ACValue;
                    Visible = true;
                    ValueNumericUpDown.Focus();
                    loading = false;
                    return true;
                }
            }
            return false;
        }

        private void ValueNumericUpDown_Leave(object sender, EventArgs e)
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

        private void ValueNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (Setting != null && !loading)
            {
                if (DCMode)
                {
                    try
                    {
                        Setting.DCValue = (uint)ValueNumericUpDown.Value;
                        Win32Error err = PowerWriteDCValueIndex(default, Setting.SchemeId, Setting.SubgroupId, Setting.Id, (uint)ValueNumericUpDown.Value);
                        if (err.Failed)
                        {
                            MessageBox.Show(this, err.FormatMessage(), "PowerWriteDCValueIndex", MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                        Setting.ACValue = (uint)ValueNumericUpDown.Value;
                        Win32Error err = PowerWriteACValueIndex(default, Setting.SchemeId, Setting.SubgroupId, Setting.Id, (uint)ValueNumericUpDown.Value);
                        if (err.Failed)
                        {
                            MessageBox.Show(this, err.FormatMessage(), "PowerWriteACValueIndex", MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                        }
                    }
                    catch
                    {

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


                args.Node.Text = args.DCMode ? args.Setting.DCString() : args.Setting.ACString();

                if ((args.DCMode ? args.Setting.DCValue : args.Setting.ACValue) is uint value)
                {
                    ValueNumericUpDown.Value = (decimal)value;
                }

                loading = false;
            }
        }
    }
}
