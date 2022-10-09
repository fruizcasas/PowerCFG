using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vanara.Diagnostics;
using Vanara.PInvoke;

namespace PowerCFG
{


    public partial class FrmPowerManager : Form
    {


        public FrmPowerManager()
        {
            InitializeComponent();
        }

        private void FrmPowerManager_Load(object sender, EventArgs e)
        {
            tv.Nodes.Add($"PlatformRole:{PowerManager.PlatformRole}");
            TreeNode batteryNode = tv.Nodes.Add("Battery");
            TreeNode batteryStatusNode = batteryNode.Nodes.Add($"BatteryStatus: {PowerManager.BatteryStatus}");
            TreeNode batteryRemainingChargePercent = batteryNode.Nodes.Add($"RemainingChargePercent: {PowerManager.RemainingChargePercent}");
            TreeNode batteryRemainingDischargeTime = batteryNode.Nodes.Add($"RemainingDischargeTime: {PowerManager.RemainingDischargeTime}");
            TreeNode batteryOnACPower = batteryNode.Nodes.Add($"OnACPower: {PowerManager.OnACPower}");
            TreeNode batteryEnergySaverStatus = batteryNode.Nodes.Add($"EnergySaverStatus: {PowerManager.EnergySaverStatus}");
            TreeNode batteryPowerSupplyStatus = batteryNode.Nodes.Add($"PowerSupplyStatus: {PowerManager.PowerSupplyStatus}");

            TreeNode poweredDevicesNode = tv.Nodes.Add("PoweredDevices");
            foreach (var item in PowerManager.PoweredDevices)
            {
                poweredDevicesNode.Nodes.Add($"{item.Value} : {(item.Value.WakeEnabled ? "ON" : "off")}");
            }

            TreeNode deviceCapabilitiesNode = tv.Nodes.Add($"DeviceCapabilities:{PowerManager.DeviceCapabilities}");

            TreeNode schemesNode = tv.Nodes.Add("Powered Schemes");
            foreach (var scheme in PowerManager.Schemes.Values)
            {
                TreeNode schemeNode = schemesNode.Nodes.Add($"{scheme.Name} {(scheme.IsActive ? "*" : ":")} {scheme.ApiName}");
                schemeNode.ToolTipText = scheme.Description;
                foreach (var subgroup in scheme.Groups.Values)
                {
                    TreeNode groupNode = schemeNode.Nodes.Add($"{subgroup.Name} : {subgroup.ApiName}");
                    groupNode.ToolTipText = subgroup.Description;
                    foreach (var setting in subgroup.Settings.Values)
                    {
                        TreeNode settingNode = groupNode.Nodes.Add($"{setting.Name} : {setting.ApiName}");
                        settingNode.ToolTipText = setting.Description;

                        if (setting.IsRange)
                        {
                            var (min, max, increment, unitsSpecifier) = setting.Range;
                            TreeNode acValueNode = settingNode.Nodes.Add($"AC:{setting.ACValue} {unitsSpecifier}");
                            acValueNode.ToolTipText = $"from {min} to {max} each {increment} {unitsSpecifier}";
                            TreeNode dcValueNode = settingNode.Nodes.Add($"DC:{setting.DCValue} {unitsSpecifier}");
                            dcValueNode.ToolTipText = $"from {min} to {max} each {increment} {unitsSpecifier}";
                        }
                        else
                        {
                            if (setting.ACValue is Byte[])
                            {
                                var (acValue, acName, acDescription) = setting.PossibleValues.FirstOrDefault(p => ((byte[])p.value).SequenceEqual((byte[])setting.ACValue));
                                var (dcValue, dcName, dcDescription) = setting.PossibleValues.FirstOrDefault(p => ((byte[])p.value).SequenceEqual((byte[])setting.DCValue));
                                TreeNode acValueNode = settingNode.Nodes.Add($"AC:{acName} {new Guid((byte[])setting.ACValue)} PV:{new Guid((byte[])acValue)}");
                                acValueNode.ToolTipText = acDescription;
                                TreeNode dcValueNode = settingNode.Nodes.Add($"DC:{dcName} {new Guid((byte[])setting.DCValue)} PV:{new Guid((byte[])dcValue)}");
                                dcValueNode.ToolTipText = dcDescription;
                            }
                            else if (setting.ACValue is uint)
                            {
                                var (acValue, acName, acDescription) = setting.PossibleValues.FirstOrDefault(p => (uint)p.value == (uint)setting.ACValue);
                                var (dcValue, dcName, dcDescription) = setting.PossibleValues.FirstOrDefault(p => (uint)p.value == (uint)setting.DCValue);
                                TreeNode acValueNode = settingNode.Nodes.Add($"AC:{acName} {setting.ACValue} PV:{acValue}");
                                acValueNode.ToolTipText = acDescription;
                                TreeNode dcValueNode = settingNode.Nodes.Add($"DC:{dcName} {setting.DCValue} PV:{dcValue}");
                                dcValueNode.ToolTipText = dcDescription;
                            }


                        }


                    }
                }

            }
        }
    }
}
